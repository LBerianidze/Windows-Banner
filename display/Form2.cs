using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace display
{
    public partial class Form2 : Form
    {
        private readonly string img_url;
        private readonly string redirect;
        public Form2()
        {
            InitializeComponent();
        }

        private readonly string DesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\log.txt";
        public Form2(string img_url, string _redirect)
        {
            InitializeComponent();
            try
            {
                this.img_url = img_url;
                this.redirect = _redirect;
                BackgroundImage = DownloadImage(img_url);
            }
            catch (Exception)
            {

            }
        }

        private Image DownloadImage(string fromUrl)
        {
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)768 | (SecurityProtocolType)3072;
            using (System.Net.WebClient webClient = new System.Net.WebClient())
            {
                using (Stream stream = webClient.OpenRead(fromUrl))
                {
                    return Image.FromStream(stream);
                }
            }
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            try
            {
                const int margin = 0;
                int x = Screen.PrimaryScreen.WorkingArea.Right - Width - margin;
                int y = Screen.PrimaryScreen.WorkingArea.Bottom - Height - margin;
                Location = new Point(x, y);
            }
            catch (Exception)
            {
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form2_Click(object sender, EventArgs e)
        {
            if (redirect != "")
            {
                Process.Start(redirect);
            }
        }
    }
}
