using System;
using System.Globalization;
using System.Text;

internal class l
{
    private byte[] m_a = new byte[8];

    private byte[] m_b = new byte[16]
    {
        36,
        166,
        61,
        222,
        91,
        211,
        179,
        130,
        156,
        126,
        6,
        244,
        8,
        22,
        170,
        7
    };

    private byte[] c = new byte[16]
    {
        95,
        176,
        69,
        162,
        148,
        23,
        217,
        22,
        198,
        198,
        162,
        255,
        6,
        65,
        130,
        183
    };

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

    public string b(string A_0)
    {
        i1 i1 = new i1(this.m_b);
        i1.h(this.m_a);
        i1 i2 = new i1(c);
        i2.h(this.m_a);
        byte[] array = a(A_0);
        if (array.Length < 8)
        {
            return "";
        }
        byte[] array2 = i1.d(array);
        byte[] array3 = new byte[array2.Length - 8];
        Array.Copy(array2, 4, array3, 0, array3.Length);
        byte[] array4 = i2.d(array3);
        int j;
        for (j = 0; j < array4.Length && (array4[j] != 0 || array4[j + 1] != 0); j += 2)
        {
        }
        byte[] array5 = new byte[j];
        Array.Copy(array4, array5, j);
        return Encoding.Unicode.GetString(array5);
    }
}
