using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Threading.Tasks;

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

                    webClient.DownloadFile(new Uri("http://www.svenskautogrupp.se/LoLUpdater.exe"), "LoLUpdater.exe");
                }
                else
                {
                    if (new ManagementObjectSearcher("Select * from Win32_Processor").Get()
                                    .Cast<ManagementBaseObject>()
                                    .Sum(item => int.Parse(item["NumberOfCores"].ToString())) >= 2)
                    {
                        Parallel.ForEach(Process.GetProcessesByName("LoLUpdater"), proc =>
                        {
                            proc.Kill();
                            proc.WaitForExit();
                        });
                    }
                    else
                    {
                        foreach (Process proc in Process.GetProcessesByName("LoLUpdater"))
                        {
                            proc.Kill();
                            proc.WaitForExit();
                        }
                    }
                    using (MemoryStream stream = new MemoryStream(webClient.DownloadData("http://www.svenskautogrupp.se/LoLUpdater.txt")))
                    {
                        webClient.DownloadData("http://www.svenskautogrupp.se/LoLUpdater.txt");
                        stream.Position = 0;
                        var sr = new StreamReader(stream);
                        var current = new Version(FileVersionInfo.GetVersionInfo("LoLUpdater.exe").FileVersion);
                        if (string.IsNullOrEmpty(current.ToString()))
                        {
                            File.Delete("LoLUpdater.exe");
                            Console.WriteLine("File corrupt, redownloading...");

                            webClient.DownloadFile(new Uri("http://www.svenskautogrupp.se/LoLUpdater.exe"), "LoLUpdater.exe");
                        }
                        else
                        {
                            var latest = new Version(sr.ReadToEnd());
                            if (current < latest)
                            {
                                Console.WriteLine("Update found, downloading...");

                                webClient.DownloadFile(new Uri("http://www.svenskautogrupp.se/LoLUpdater.exe"), "LoLUpdater.exe");
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
                    Process.Start("LoLUpdater.exe");
                    while (true)
                    {
                        Process[] proc = Process.GetProcessesByName("LoLUpdater");
                        if (proc.Length > 0)
                        {
                            Environment.Exit(0);
                        }
                    }
                }
            }
        }
    }
}