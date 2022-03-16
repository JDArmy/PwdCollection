namespace Firefox.Cryptography
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Security.Cryptography;
    using System.IO;

    public class MozillaPBE
    {
        private byte[] GlobalSalt { get; set; }
        private byte[] MasterPassword { get; set; }
        private byte[] EntrySalt { get; set; }
        private Asn1DerObject decodedItem { get; set; }
        public byte[] Key { get; private set; }
        public byte[] IV { get; private set; }
        public static Dictionary<string, string> oidValues = new Dictionary<string, string> {
            { "2A864886F70D010C050103", "1.2.840.113549.1.12.5.1.3 pbeWithSha1AndTripleDES-CBC" },
            { "2A864886F70D0307", "1.2.840.113549.3.7 des-ede3-cbc" },
            { "2A864886F70D010101", "1.2.840.113549.1.1.1 pkcs-1" },
            { "2A864886F70D01050D", "1.2.840.113549.1.5.13 pkcs5 pbes2" },
            { "2A864886F70D01050C", "1.2.840.113549.1.5.12 pkcs5 PBKDF2" },
            { "2A864886F70D0209", "1.2.840.113549.2.9 hmacWithSHA256" },
            { "60864801650304012A", "2.16.840.1.101.3.4.1.42 aes256-CBC" }
        };

        public MozillaPBE(byte[] GlobalSalt, byte[] MasterPassword, Asn1DerObject decodedItem2)
        {
            this.GlobalSalt = GlobalSalt;
            this.MasterPassword = MasterPassword;
            this.decodedItem = decodedItem2;
        }

        public byte[] Compute()
        {
            
            byte[] pbeAlgo = decodedItem.objects[0].objects[0].objects[0].Data;
            String pbeAlgostring = BitConverter.ToString(pbeAlgo).Replace("-", string.Empty).ToUpper();
            if (oidValues[pbeAlgostring].Contains("1.2.840.113549.1.12.5.1.3"))
            {
                SHA1 sha = new SHA1CryptoServiceProvider();

                this.EntrySalt = this.decodedItem.objects[0].objects[0].objects[1].objects[0].Data;
                byte[] cipherT = this.decodedItem.objects[0].objects[1].Data;
                Mozilla3DES CheckPwd = new Mozilla3DES(this.GlobalSalt, this.MasterPassword, this.EntrySalt);
                CheckPwd.Compute();
                byte[] key = { 0xa0,0x81,0xa9, 0x9e, 0x19, 0x66, 0xb4, 0x05, 0x99, 0x9d, 0xc5, 0x1b, 0xbd, 0xab, 0x1f, 0x70, 0xf2, 0xbb, 0x43, 0x80, 0xd2, 0xa6, 0xbb, 0x9b };
                byte[] iv = { 0x90,0x25, 0x5f, 0x91, 0x77, 0x4a, 0x59, 0xcf };
                byte[] clearText = this.DES3Decrypt(cipherT, CheckPwd.Key, CheckPwd.IV);
                return clearText;
            }
            else if (oidValues[pbeAlgostring].Contains("1.2.840.113549.1.5.13"))
            {
                SHA1 sha = new SHA1CryptoServiceProvider();
                byte[] GLMP; // GlobalSalt + MasterPassword
                byte[] k; // SHA1(GLMP)
                byte[] key; // final value conytaining key and iv
                this.EntrySalt = this.decodedItem.objects[0].objects[0].objects[1].objects[0].objects[1].objects[0].Data;
                int iterationCount = this.decodedItem.objects[0].objects[0].objects[1].objects[0].objects[1].objects[1].Data[0];
                int keyLength = this.decodedItem.objects[0].objects[0].objects[1].objects[0].objects[1].objects[2].Data[0];

                //GLMP;
                GLMP = new byte[this.GlobalSalt.Length + this.MasterPassword.Length];
                Array.Copy(this.GlobalSalt, 0, GLMP, 0, this.GlobalSalt.Length);
                Array.Copy(this.MasterPassword, 0, GLMP, this.GlobalSalt.Length, this.MasterPassword.Length);
                k = sha.ComputeHash(GLMP);
                using (var hmac = new HMACSHA256())
                {
                    var df = new Pbkdf2(hmac, k, this.EntrySalt, iterationCount);
                    this.Key = df.GetBytes(32);
                }
                byte[] source = { 0x4, 0xe };
                Byte[] ivb = this.decodedItem.objects[0].objects[0].objects[1].objects[2].objects[1].Data;
                this.IV = new byte[ivb.Length + source.Length];
                Array.Copy(source, 0, this.IV, 0, source.Length);
                Array.Copy(ivb, 0, this.IV, source.Length, ivb.Length);

                byte[] cipherT = this.decodedItem.objects[0].objects[1].Data;
                byte[] clearText = this.AesDecrypt(cipherT, this.Key, this.IV);
                return clearText;
            }
            return null;
        }
        public byte[] AesDecrypt(byte[] data, byte[] key, byte[] iv)
        {
            //try
            //{

            //    RijndaelManaged rDel = new RijndaelManaged();
            //    rDel.Key = key;
            //    rDel.IV = iv;
            //    rDel.Mode = CipherMode.CBC;
            //    rDel.Padding = PaddingMode.PKCS7;

            //    ICryptoTransform cTransform = rDel.CreateDecryptor();
            //    byte[] resultArray = cTransform.TransformFinalBlock(data, 0, data.Length);

            //    return resultArray;
            //}
            //catch
            //{
            //    return null;
            //}
            byte[] decrypt = null;
            Rijndael aes = Rijndael.Create();
            // 开辟一块内存流
            using (MemoryStream mStream = new MemoryStream())
            {
                // 把内存流对象包装成加密流对象
                using (CryptoStream cStream = new CryptoStream(mStream, aes.CreateDecryptor(key, iv), CryptoStreamMode.Write))
                {
                    // 明文数据写入加密流
                    cStream.Write(data, 0, data.Length);
                    cStream.FlushFinalBlock();
                    decrypt = mStream.ToArray();
                }
            }
            aes.Clear();
            return decrypt;
        }

        public byte[] DES3Decrypt(byte[] data, byte[] key, byte[] iv)
        {
            try
            {
                var des = new TripleDESCryptoServiceProvider
                {
                    Key = key,
                    IV = iv,
                    Mode = CipherMode.CBC,
                    Padding = System.Security.Cryptography.PaddingMode.PKCS7
                };
                var desDecrypt = des.CreateDecryptor();
                byte[] result = { };
                byte[] buffer = data;
                result = desDecrypt.TransformFinalBlock(buffer, 0, buffer.Length);
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine("Des3 Exeption:\n" + e.Message);
                return null;
            }
        }
    }
}
