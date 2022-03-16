using System;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;

internal class m
{
    private byte[] m_a = new byte[16];

    private byte[] m_b;

    public static byte[] a(string A_0)
    {
        if (A_0.Length % 2 != 0)
        {
            throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "The binary key cannot have an odd number of digits: {0}", new object[1]
            {
                A_0
            }));
        }
        byte[] array = new byte[A_0.Length / 2];
        for (int i = 0; i < array.Length; i++)
        {
            string s = A_0.Substring(i * 2, 2);
            array[i] = byte.Parse(s, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
        }
        return array;
    }

    public static bool a(byte[] A_0, byte[] A_1)
    {
        if (A_0 == null || A_1 == null)
        {
            return false;
        }
        if (A_0.Length != A_1.Length)
        {
            return false;
        }
        for (int i = 0; i < A_0.Length; i++)
        {
            if (A_0[i] != A_1[i])
            {
                return false;
            }
        }
        return true;
    }

    public m(string A_0 = "")
    {
        using (SHA256 sHA = SHA256.Create())
        {
            this.m_b = sHA.ComputeHash(Encoding.UTF8.GetBytes(A_0));
        }
    }

    public string b(string A_0)
    {
        byte[] array = a(A_0);
        byte[] array2;
        using (MemoryStream memoryStream = new MemoryStream())
        {
            using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
            {
                rijndaelManaged.KeySize = 256;
                rijndaelManaged.BlockSize = 128;
                rijndaelManaged.Key = this.m_b;
                rijndaelManaged.IV = this.m_a;
                rijndaelManaged.Mode = CipherMode.CBC;
                rijndaelManaged.Padding = PaddingMode.Zeros;
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, rijndaelManaged.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cryptoStream.Write(array, 0, array.Length);
                    cryptoStream.Close();
                }
                array2 = memoryStream.ToArray();
            }
        }
        if (array2.Length < 4)
        {
            return "";
        }
        int num = BitConverter.ToInt32(new byte[4]
        {
            array2[0],
            array2[1],
            array2[2],
            array2[3]
        }, 0);
        if (array2.Length < 4 + num + 32)
        {
            return "";
        }
        byte[] array3 = new byte[num];
        byte[] array4 = new byte[32];
        byte[] a_ = new byte[32];
        Array.Copy(array2, 4, array3, 0, num);
        Array.Copy(array2, 4 + num, array4, 0, 32);
        using (SHA256 sHA = SHA256.Create())
        {
            a_ = sHA.ComputeHash(array3);
        }
        if (a(a_, array4))
        {
            return Encoding.UTF8.GetString(array3);
        }
        return "";
    }
}
