using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Principal;
using System.Text.RegularExpressions;


    internal class SearchFile
    {
        public static void run(string A_0, string A_1)
        {
            try
            {
                r r = new r(A_0);
                if (A_1.Length > 0 && A_1 != null)
                {
                    DirectoryInfo a_ = new DirectoryInfo(A_1);
                    r.a(a_);
                }
                else
                {
                    DriveInfo[] drives = DriveInfo.GetDrives();
                    foreach (DriveInfo driveInfo in drives)
                    {
                        if (driveInfo.IsReady)
                        {
                            DirectoryInfo a_2 = new DirectoryInfo(driveInfo.Name);
                            r.a(a_2);
                        }
                    }
                }
                //Console.WriteLine("[+] Find file over: " + A_0 + "  Done.");
            }
            catch (Exception value)
            {
                Console.WriteLine(value);
            }
        }
    }