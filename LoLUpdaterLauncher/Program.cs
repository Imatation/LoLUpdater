using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace ConsoleApplication2
{
    class Program
    {
        static void Main(string[] args)
        {
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
            else if (System.Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version < winvistaversion)
            {
                File.WriteAllBytes("LoLUpdaterXP.exe", Properties.Resources.LoLUpdaterXP);
                Process.Start("LoLUpdaterXP.exe");
            }
            else
            {
                File.WriteAllBytes("LoLUpdaterXP.exe", Properties.Resources.LoLUpdaterXP);
                Process.Start("LoLUpdaterXP.exe");
            }
        }
    }
}
