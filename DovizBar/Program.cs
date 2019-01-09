using System;
using System.Net; //client
using System.Reflection;
using System.Windows.Forms;

namespace DovizBar
{
    static class Program
    {
        public static Version Version;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]

        static void Main()
        {
            try
            {
                Version = Version.Parse(Application.ProductVersion);

                if (!System.IO.File.Exists("PostSharp.dll") || !System.IO.File.Exists("Castle.Core.dll"))
                {
                    var path = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                    using (var client = new WebClient())
                    {
                        client.DownloadFile("https://github.com/bgoktugozdemir/VersionChecker/raw/master/DovizBar/DownloadFiles/Castle.Core.dll", path + @"/Castle.Core.dll");
                        client.DownloadFile("https://github.com/bgoktugozdemir/VersionChecker/raw/master/DovizBar/DownloadFiles/PostSharp.dll", path + @"/PostSharp.dll");
                    }
                }
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }
            catch (ObjectDisposedException)
            {
                //Environment.FailFast("Güncelleme Başladı!");
                Application.Exit();
            }
        }
    }
}
