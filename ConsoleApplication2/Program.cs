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
            Version winxpversion = new Version(5, 1, 2600, 0);
            if (System.Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version == winxpversion)
            {
                File.WriteAllBytes("LoLUpdaterXP.exe", Properties.Resources.LoLUpdaterXP);
                Process.Start("LoLUpdaterXP.exe");
            }
            Version winxp2version = new Version(5, 1, 3790, 0);
            if (System.Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version == winxp2version)
            {
                File.WriteAllBytes("LoLUpdaterXP.exe", Properties.Resources.LoLUpdaterXP);
                Process.Start("LoLUpdaterXP.exe");
            }
            Version winvistaversion = new Version(6, 0, 6000, 0);
            if (System.Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version >= winvistaversion)
            {
                File.WriteAllBytes("LoLUpdater.exe", Properties.Resources.LoLUpdater);
                Process.Start("LoLUpdater.exe");
            }
        }
    }
}
