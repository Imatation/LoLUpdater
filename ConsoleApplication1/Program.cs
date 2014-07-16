using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {

                        Version winxpversion = new Version(5, 1, 2600, 0);
            if (System.Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version = winxpversion)
            {
                 File.WriteAllBytes("LoLUpdaterXP.exe", Properties.Resources.LoLUpdaterXP);
                                Process.Start("LoLUpdaterXP.exe");

            }
                                    Version winxp2version = new Version(5, 1, 3790, 0);
            if (System.Environment.OSVersion.Platform == PlatformID.Win32NT &&
                Environment.OSVersion.Version = winxp2version)
            {
                 File.WriteAllBytes("LoLUpdaterXP.exe", Properties.Resources.LoLUpdaterXP);
                                                Process.Start("LoLUpdaterXP.exe");

            }

            5.1
            2600
                3790

                    6.0
                        6000
        }
    }
}
