// navicatpwd.Program
using Microsoft.Win32;
using System;

internal class NavicatPwd
{
    public void NavicatCrypt()
    {
        string value = "";
        
        value = RegRuturnRes();
                
        Console.WriteLine(value);
    }

    private static string RegRuturnRes(string ver = "11")
    {
        string text = "";
        if (IsRegeditsubkeyExit("Software", "PremiumSoft"))
        {
            string[] array = new string[8]
            {
                "Navicat",
                "NavicatMSSQL",
                "NavicatOra",
                "NavicatMARIADB",
                "NavicatPG",
                "NavicatSQLite",
                "NavicatMONGODB",
                "NavicatMongoDB"
            };
            string[] array2 = array;
            foreach (string text2 in array2)
            {
                if (!IsRegeditsubkeyExit("Software\\PremiumSoft", text2) || !IsRegeditsubkeyExit("Software\\PremiumSoft\\" + text2, "Servers"))
                {
                    continue;
                }
                RegistryKey currentUser = Registry.CurrentUser;
                RegistryKey registryKey = currentUser.OpenSubKey("Software\\PremiumSoft\\" + text2 + "\\Servers");
                string[] subKeyNames = registryKey.GetSubKeyNames();
                if (subKeyNames.Length != 0)
                {
                    string[] array3 = subKeyNames;
                    foreach (string connect_name in array3)
                    {
                        if (IsRegeditKeyExit(text2, connect_name))
                        {
                            string text3 = "";
                            string text4 = Readreg(text2, connect_name);
                            string text5 = split(text4);
                            if (text5 != "")
                            {
                                text3 = Decrypt(text5, ver);
                            }
                            string text6 = text;
                            text = text6 + text2 + "->" + text4 + "(" + text3 + ")\r\n";
                        }
                    }
                }
                else
                {
                    text = text + text2 + " No connection record\r\n";
                }
                currentUser.Close();
            }
        }
        else
        {
            text = "Maybe Not Install Navicat\r\n";
        }
        return text;
    }

    private static string split(string str)
    {
        string[] array = str.Split(':');
        return array[3];
    }

    private static string Decrypt(string val, string ver = "11")
    {
        string result = "";
        if (val != "")
        {
            if (ver == "12")
            {
                try
                {
                    Navicat12Cipher navicat12Cipher = new Navicat12Cipher();
                    return navicat12Cipher.DecryptStringForNCX(val);
                }
                catch (Exception ex)
                {
                    return "ver select error:" + ex.Message;
                }
            }
            try
            {
                Navicat11Cipher navicat11Cipher = new Navicat11Cipher();
                return navicat11Cipher.DecryptString(val);
            }
            catch (Exception ex2)
            {
                return "ver select error:" + ex2.Message;
            }
        }
        return result;
    }

    private static string Readreg(string datatype, string connect_name)
    {
        string text = "";
        RegistryKey currentUser = Registry.CurrentUser;
        string name = "Software\\PremiumSoft\\" + datatype + "\\Servers\\" + connect_name;
        RegistryKey registryKey = currentUser.OpenSubKey(name);
        text = registryKey.GetValue("Host").ToString();
        text = text + ":" + registryKey.GetValue("Port").ToString();
        text = text + ":" + registryKey.GetValue("UserName").ToString();
        text = text + ":" + registryKey.GetValue("Pwd").ToString();
        currentUser.Close();
        return text;
    }

    private static bool IsRegeditsubkeyExit(string path, string keyname)
    {
        RegistryKey currentUser = Registry.CurrentUser;
        RegistryKey registryKey = currentUser.OpenSubKey(path);
        string[] subKeyNames = registryKey.GetSubKeyNames();
        string[] array = subKeyNames;
        foreach (string a in array)
        {
            if (a == keyname)
            {
                currentUser.Close();
                return true;
            }
        }
        currentUser.Close();
        return false;
    }

    private static bool IsRegeditKeyExit(string datatype, string connect_name)
    {
        RegistryKey currentUser = Registry.CurrentUser;
        string name = "Software\\PremiumSoft\\" + datatype + "\\Servers\\" + connect_name;
        RegistryKey registryKey = currentUser.OpenSubKey(name);
        string[] valueNames = registryKey.GetValueNames();
        string[] array = valueNames;
        foreach (string a in array)
        {
            if (a == "Host")
            {
                currentUser.Close();
                return true;
            }
        }
        currentUser.Close();
        return false;
    }
}
