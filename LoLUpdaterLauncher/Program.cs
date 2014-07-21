using System;
using System.IO;
using System.Diagnostics;
using System.Net;
using System.Reflection;
namespace LoLUpdater
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileName = "version.txt";
            WebClient wc = new WebClient();
            wc.Credentials = new NetworkCredential("ikorsu", "lolapplication1");
            wc.DownloadFile("ftp://lol.jdhpro.com/lol.jdhpro.com/version.txt", fileName);
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string latestversion = fvi.FileVersion;
            string currentversion = System.IO.File.ReadAllText("version.txt");
            var version1 = new Version(latestversion);
            var version2 = new Version(currentversion);
            var result = version1.CompareTo(version2);
            if (result < 0)
            {
                Console.Write("Update found, Downloading...");
                string fileName1 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "LoLUpdaterLauncher.exe");
                WebClient wc1 = new WebClient();
                wc1.Credentials = new NetworkCredential("ikorsu", "lolapplication1"); // Todo: Password could be exposed (clear text password login)
                wc1.DownloadFile("ftp://lol.jdhpro.com/lol.jdhpro.com/LoLUpdaterLauncher.exe", fileName1);
                Console.Write("Finished downloading, saved to desktop.");
                Console.ReadLine();
            }
            else
            {
                File.WriteAllBytes("tbb.dll", Properties.Resources.tbb);
                File.WriteAllBytes("Cg_3_1_April2012_Setup.exe", Properties.Resources.Cg_3_1_April2012_Setup);
                File.WriteAllText("NvidiaCGLicence.txt", Properties.Resources.NvidiaCGLicence);
                Version winvistaversion = new Version(6, 0, 6000, 0);
                if (System.Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version >= winvistaversion)
                {
                    File.WriteAllBytes("LoLUpdater.exe", Properties.Resources.LoLUpdater);
                    var cmd = new ProcessStartInfo();
                    var process = new Process();
                    cmd.FileName = "LoLUpdater.exe";
                    cmd.Verb = "runas";
                    process.StartInfo = cmd;
                    process.Start();
                }
                else if (System.Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version < winvistaversion || Environment.OSVersion.Version == null)
                {
                    File.WriteAllBytes("LoLUpdaterXP.exe", Properties.Resources.LoLUpdaterXP);
                    Process.Start("LoLUpdaterXP.exe");
                }
            }
        }
    }
}
