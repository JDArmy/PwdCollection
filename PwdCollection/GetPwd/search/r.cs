using System;
using System.Collections;
using System.IO;

internal class r
{
    private ArrayList m_a = new ArrayList();

    private string b = "";

    public r(string A_0)
    {
        b = A_0;
        a();
    }

    private void a()
    {
        this.m_a.Add("C:\\Program Files".ToLower());
        this.m_a.Add("Program Files".ToLower());
        this.m_a.Add("C:\\Program Files (x86)".ToLower());
        this.m_a.Add("Program Files (x86)".ToLower());
        this.m_a.Add("C:\\Windows".ToLower());
        this.m_a.Add("Windows".ToLower());
        this.m_a.Add("C:\\PerfLogs".ToLower());
        this.m_a.Add("PerfLogs".ToLower());
        this.m_a.Add("C:\\Python27".ToLower());
        this.m_a.Add("C:\\Python36".ToLower());
        this.m_a.Add("C:\\Python37".ToLower());
        this.m_a.Add("C:\\Python38".ToLower());
        this.m_a.Add("C:\\ProgramData".ToLower());
        this.m_a.Add("C:\\MinGW".ToLower());
        this.m_a.Add("C:\\Users\\All Users".ToLower());
        this.m_a.Add("All Users".ToLower());
        this.m_a.Add("C:\\Users\\Default".ToLower());
        this.m_a.Add("C:\\Users\\Public".ToLower());
        this.m_a.Add("WeGame".ToLower());
        this.m_a.Add("CloudMusic".ToLower());
        this.m_a.Add("masm32".ToLower());
        this.m_a.Add("WindowsApps".ToLower());
    }

    public void a(DirectoryInfo A_0)
    {
        try
        {
            FileInfo[] files = A_0.GetFiles(b);
            
            foreach (FileInfo fileInfo in files)
            {
                string fileEx = System.IO.Path.GetExtension(fileInfo.FullName);
                //string fileName = System.IO.Path.GetFileName(fileInfo.FullName);
                if(fileEx == ".txt" || fileEx == ".xml" || fileEx == ".sql" || fileEx ==　".bak" || fileEx == ".rtf" || fileEx==".doc" || fileEx==".docx" || fileEx==".xls" || fileEx==".xlsx" || fileEx == ".csv")
                {
                    Console.WriteLine("[+] " + fileInfo.FullName);
                }
                string[] configFileName = { "web.config", "config.php", "database.php", "webdev-jdbc.xml", "web.xml", "tomcat-users.xml", "server.xml", "context.xml", "application.properties", "application.yml", "jca\\*-ds.xml",  };

               // if (fileName == "web.config" || fileName == "config.php" || fileName == "database.php" || fileName == "webdev-jdbc.xml" || fileName == "web.xml" || fileName == "tomcat-users.xml")

               // {

               // }

            }
            DirectoryInfo[] directories = A_0.GetDirectories("*", SearchOption.TopDirectoryOnly);
            foreach (DirectoryInfo directoryInfo in directories)
            {
                if (!this.m_a.Contains(directoryInfo.Name.ToLower()) && !this.m_a.Contains(directoryInfo.FullName.ToLower()))
                {
                    if (!b.Equals(directoryInfo.Name.ToLower()))
                    {
                        a(directoryInfo);
                    }
                    else
                    {
                        Console.WriteLine("[D] " + directoryInfo.FullName);
                    }
                }
            }

            
        }
        catch (UnauthorizedAccessException)
        {
        }
        catch (PathTooLongException)
        {
        }
    }
}
