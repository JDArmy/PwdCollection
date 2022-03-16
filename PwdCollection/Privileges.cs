using System;
using System.Diagnostics;
using System.Security.Principal;
using System.Runtime.InteropServices;
using static RegSave.Interop;

internal class Privileges
{
    public static bool IsHighIntegrity()
    {
        // returns true if the current process is running with adminstrative  privs in a high integrity context
        var identity = WindowsIdentity.GetCurrent();
        var principal = new WindowsPrincipal(identity);
        return principal.IsInRole(WindowsBuiltInRole.Administrator);
    }

    public static void EnableDisablePrivilege(string PrivilegeName, bool EnableDisable)
    {
        var htok = IntPtr.Zero;

        if (!OpenProcessToken(Process.GetCurrentProcess().Handle, TokenAccessLevels.AdjustPrivileges | TokenAccessLevels.Query, out htok))
        {
            Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            return;
        }

        var tkp = new TOKEN_PRIVILEGES { PrivilegeCount = 1 };
        LUID luid;

        if (!LookupPrivilegeValue(null, PrivilegeName, out luid))
        {
            Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            return;
        }

        tkp.Luid = luid;
        tkp.Attributes = (uint)(EnableDisable ? 2 : 0);
        TOKEN_PRIVILEGES prv;
        uint rb;

        if (!AdjustTokenPrivileges(htok, false, ref tkp, 256, out prv, out rb))
        {
            Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
        }
    }
}
