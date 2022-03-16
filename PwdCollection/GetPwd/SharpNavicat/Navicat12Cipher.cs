// NavicatCrypto.Navicat12Cipher
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Text;

internal class Navicat12Cipher : Navicat11Cipher
{
    private AesCryptoServiceProvider AesServiceProvider;

    public Navicat12Cipher()
    {
        AesServiceProvider = new AesCryptoServiceProvider();
        AesServiceProvider.KeySize = 128;
        AesServiceProvider.Mode = CipherMode.CBC;
        AesServiceProvider.Padding = PaddingMode.PKCS7;
        AesServiceProvider.Key = Encoding.UTF8.GetBytes("libcckeylibcckey");
        AesServiceProvider.IV = Encoding.UTF8.GetBytes("libcciv libcciv ");
    }

    public string EncryptStringForNCX(string plaintext)
    {
        ICryptoTransform transform = AesServiceProvider.CreateEncryptor();
        MemoryStream memoryStream = new MemoryStream();
        CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write);
        byte[] bytes = Encoding.UTF8.GetBytes(plaintext);
        cryptoStream.Write(bytes, 0, bytes.Length);
        cryptoStream.FlushFinalBlock();
        return Navicat11Cipher.ByteArrayToString(memoryStream.ToArray());
    }

    public string DecryptStringForNCX(string ciphertext)
    {
        ICryptoTransform transform = AesServiceProvider.CreateDecryptor();
        MemoryStream memoryStream = new MemoryStream();
        CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write);
        byte[] array = Navicat11Cipher.StringToByteArray(ciphertext);
        cryptoStream.Write(array, 0, array.Length);
        cryptoStream.FlushFinalBlock();
        return Encoding.UTF8.GetString(memoryStream.ToArray());
    }
}
