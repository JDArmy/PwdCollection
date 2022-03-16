// NavicatCrypto.Navicat11Cipher
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

internal class Navicat11Cipher
{
    private Blowfish blowfishCipher;

    protected static byte[] StringToByteArray(string hex)
    {
        return (from x in Enumerable.Range(0, hex.Length)
                where x % 2 == 0
                select Convert.ToByte(hex.Substring(x, 2), 16)).ToArray();
    }

    protected static string ByteArrayToString(byte[] bytes)
    {
        return BitConverter.ToString(bytes).Replace("-", string.Empty);
    }

    protected static void XorBytes(byte[] a, byte[] b, int len)
    {
        for (int i = 0; i < len; i++)
        {
            a[i] ^= b[i];
        }
    }

    public Navicat11Cipher()
    {
        byte[] bytes = Encoding.UTF8.GetBytes("3DC5CA39");
        SHA1CryptoServiceProvider sHA1CryptoServiceProvider = new SHA1CryptoServiceProvider();
        sHA1CryptoServiceProvider.TransformFinalBlock(bytes, 0, bytes.Length);
        blowfishCipher = new Blowfish(sHA1CryptoServiceProvider.Hash);
    }

    public Navicat11Cipher(string CustomUserKey)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(CustomUserKey);
        SHA1CryptoServiceProvider sHA1CryptoServiceProvider = new SHA1CryptoServiceProvider();
        byte[] userKey = sHA1CryptoServiceProvider.TransformFinalBlock(bytes, 0, 8);
        blowfishCipher = new Blowfish(userKey);
    }

    public string EncryptString(string plaintext)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(plaintext);
        byte[] array = Enumerable.Repeat(byte.MaxValue, Blowfish.BlockSize).ToArray();
        blowfishCipher.Encrypt(array, Blowfish.Endian.Big);
        string text = "";
        int num = bytes.Length / Blowfish.BlockSize;
        int num2 = bytes.Length % Blowfish.BlockSize;
        byte[] array2 = new byte[Blowfish.BlockSize];
        for (int i = 0; i < num; i++)
        {
            Array.Copy(bytes, Blowfish.BlockSize * i, array2, 0, Blowfish.BlockSize);
            XorBytes(array2, array, Blowfish.BlockSize);
            blowfishCipher.Encrypt(array2, Blowfish.Endian.Big);
            XorBytes(array, array2, Blowfish.BlockSize);
            text += ByteArrayToString(array2);
        }
        if (num2 != 0)
        {
            blowfishCipher.Encrypt(array, Blowfish.Endian.Big);
            XorBytes(array, bytes.Skip(num * Blowfish.BlockSize).Take(num2).ToArray(), num2);
            text += ByteArrayToString(array.Take(num2).ToArray());
        }
        return text;
    }

    public string DecryptString(string ciphertext)
    {
        byte[] array = StringToByteArray(ciphertext);
        byte[] array2 = Enumerable.Repeat(byte.MaxValue, Blowfish.BlockSize).ToArray();
        blowfishCipher.Encrypt(array2, Blowfish.Endian.Big);
        byte[] array3 = new byte[0];
        int num = array.Length / Blowfish.BlockSize;
        int num2 = array.Length % Blowfish.BlockSize;
        byte[] array4 = new byte[Blowfish.BlockSize];
        byte[] array5 = new byte[Blowfish.BlockSize];
        for (int i = 0; i < num; i++)
        {
            Array.Copy(array, Blowfish.BlockSize * i, array4, 0, Blowfish.BlockSize);
            Array.Copy(array4, array5, Blowfish.BlockSize);
            blowfishCipher.Decrypt(array4, Blowfish.Endian.Big);
            XorBytes(array4, array2, Blowfish.BlockSize);
            array3 = array3.Concat(array4).ToArray();
            XorBytes(array2, array5, Blowfish.BlockSize);
        }
        if (num2 != 0)
        {
            Array.Clear(array4, 0, array4.Length);
            Array.Copy(array, Blowfish.BlockSize * num, array4, 0, num2);
            blowfishCipher.Encrypt(array2, Blowfish.Endian.Big);
            XorBytes(array4, array2, Blowfish.BlockSize);
            array3 = array3.Concat(array4.Take(num2).ToArray()).ToArray();
        }
        return Encoding.UTF8.GetString(array3);
    }
}
