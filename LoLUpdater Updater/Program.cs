using System;
using System.Diagnostics;
using System.IO;
using System.Net;

namespace LoLUpdater_Updater
{
    internal static class Program
    {
        private static void Main()
        {
            using (WebClient webClient = new WebClient())
            {
                if (!File.Exists("LoLUpdater.exe"))
                {
                    Console.WriteLine("LoLUpdater not found, downloading...");

                    webClient.DownloadFile("http://www.svenskautogrupp.se/LoLUpdater.exe", "LoLUpdater.exe");
                }
                else
                {
                    foreach (Process process in Process.GetProcessesByName("LoLUpdater"))
                    {
                        process.Kill();
                        process.WaitForExit();
                    }
                    using (MemoryStream stream = new MemoryStream(webClient.DownloadData("http://www.svenskautogrupp.se/LoLUpdater.txt")))
                    {
                        webClient.DownloadData("http://www.svenskautogrupp.se/LoLUpdater.txt");
                        stream.Position = 0;
                        var sr = new StreamReader(stream);
                        var current = new Version(FileVersionInfo.GetVersionInfo("LoLUpdater.exe").FileVersion);
                        var latest = new Version(sr.ReadToEnd());
                        if (current < latest)
                        {
                            Console.WriteLine("Update found, downloading...");

                            webClient.DownloadFile("http://www.svenskautogrupp.se/LoLUpdater.exe",
                                "LoLUpdater.exe");
                        }
                        else if (current == latest)
                        {
                            Console.WriteLine("You have the latest version, starting LoLUpdater...");
                        }
                        else if (current > latest)
                        {
                            Console.WriteLine("You are not supposed to get here...");
                        }
                        else
                        {
                            Console.WriteLine("Something very strange happend...");
                        }
                    }
                }
                if (!File.Exists("LoLUpdater.exe")) return;
                Process[] proc = Process.GetProcessesByName("LoLUpdater");
                if (proc.Length > 0)
                { Environment.Exit(0); }
            }
        }
    }
}