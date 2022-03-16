using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;


internal class SearchKey
{
    public void run()
    {
        string command1 = "reg query HKCU /f password /t REG_SZ /s";
        //string command2 = "reg query HKLM /f password /t REG_SZ /s";
        Process p = new Process();
        p.StartInfo.FileName = "cmd.exe";
        p.StartInfo.UseShellExecute = false;
        p.StartInfo.RedirectStandardInput = true;
        p.StartInfo.RedirectStandardOutput = true;
        p.StartInfo.RedirectStandardError = true;
        p.StartInfo.CreateNoWindow = true;
        p.Start();

        p.StandardInput.WriteLine(command1 + "&exit");
        p.StandardInput.AutoFlush = true;

        //获取输出信息
        string strOuput = p.StandardOutput.ReadToEnd();
        p.WaitForExit();
        p.Close();
        Console.WriteLine(strOuput);
    }
}