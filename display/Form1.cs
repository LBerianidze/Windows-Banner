using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;

namespace display
{
    public partial class Form1 : Form
    {
        private string v1;
        private string v2;

        [DllImport("winmm.dll")]
        private static extern long mciSendString(string strCommand, StringBuilder strReturn, int iReturnLength, IntPtr hwndCalBack);

        public Form1(string v1, string v2)
        {
            InitializeComponent();
            //File.WriteAllText(@"D:\ифв.txt", "Running ban banner");
            try
            {
                this.v1 = v1;
                this.v2 = v2;
                WebClient wb = new WebClient();
                var image = wb.DownloadData(v1);
                var soundpath  = Path.GetTempFileName().Replace("tmp","mp3");
                wb.DownloadFile(v2, soundpath);
                this.BackgroundImage = Image.FromStream(new MemoryStream(image));
                var str = $@"open ""{soundpath}"" type mpegvideo alias MediaFile";
                mciSendString(str, null, 0, IntPtr.Zero);
                str = @"Play MediaFile";
                mciSendString(str, null, 0, IntPtr.Zero);
            }
            catch(Exception ex)
            {
                
            }

        }

        private void label1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
