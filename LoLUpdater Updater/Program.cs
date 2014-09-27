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
                    using (MemoryStream stream = new MemoryStream(webClient.DownloadData("http://www.svenskautogrupp.se/LoLUpdater.txt")))
                    {
                        webClient.DownloadData("http://www.svenskautogrupp.se/LoLUpdater.txt");
                        stream.Position = 0;
                        var sr = new StreamReader(stream);
                        if (new Version(FileVersionInfo.GetVersionInfo("LoLUpdater.exe").FileVersion) <
                            new Version(sr.ReadToEnd()))
                        {
                            Console.WriteLine("Update found, downloading...");

                            webClient.DownloadFile("http://www.svenskautogrupp.se/LoLUpdater.exe",
                                "LoLUpdater.exe");
                        }
                        else
                        {
                            Console.WriteLine("No update found, starting LoLUpdater...");
                            Console.ReadLine();
                        }
                        Process.Start("LoLUpdater.exe");
                    }
                }
            }
        }
    }
}