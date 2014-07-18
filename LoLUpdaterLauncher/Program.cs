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
                File.WriteAllBytes("tbb.dll", LoLUpdaterLauncher.Properties.Resources.tbb);
                File.WriteAllBytes("Cg_3_1_April2012_Setup.exe", LoLUpdaterLauncher.Properties.Resources.Cg_3_1_April2012_Setup);
                File.WriteAllText("NvidiaCGLicence.txt", LoLUpdaterLauncher.Properties.Resources.NvidiaCGLicence);
                Version winvistaversion = new Version(6, 0, 6000, 0);
                if (System.Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version >= winvistaversion)
                {
                    File.WriteAllBytes("LoLUpdater.exe", LoLUpdaterLauncher.Properties.Resources.LoLUpdater);
                    var cmd = new ProcessStartInfo();
                    var process = new Process();
                    cmd.FileName = "LoLUpdater.exe";
                    cmd.Verb = "runas";
                    process.StartInfo = cmd;
                    process.Start();
                }
                else if (System.Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version < winvistaversion || Environment.OSVersion.Version == null)
                {
                    File.WriteAllBytes("LoLUpdaterXP.exe", LoLUpdaterLauncher.Properties.Resources.LoLUpdaterXP);
                    Process.Start("LoLUpdaterXP.exe");
                }
        }
    }
}
