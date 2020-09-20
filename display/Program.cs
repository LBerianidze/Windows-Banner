using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace display
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            //args = new string[] { "big;https://www.publicdomainpictures.net/pictures/320000/velka/background-image.png;https://filedll.xyz/sound/sound.mp3" };
            //args = new string[] { "small;https://previews.123rf.com/images/alexis84/alexis841404/alexis84140400557/27773925-planet-earth-and-blue-human-eye-elements-of-this-image-furnished-by-nasa-.jpg;https://youtube.com" };
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            args = args[0].Split(';');
            if (args[0] == "big")
            {
                Application.Run(new Form1(args[1],args[2]));
            }
            else if (args[0] == "small")
            {
                Application.Run(new Form2(args[1],args[2]));
            }
        }
    }
}
