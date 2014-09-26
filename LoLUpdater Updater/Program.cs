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
            try
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
                        Console.WriteLine("Downloading latest versioninfo...");
                        using (MemoryStream stream = new MemoryStream(webClient.DownloadData("http://www.svenskautogrupp.se/LoLUpdater.txt")))
                        {
                            webClient.DownloadData("http://www.svenskautogrupp.se/LoLUpdater.txt");
                            Console.WriteLine("Comparing versions...");
                            if (new Version(FileVersionInfo.GetVersionInfo("LoLUpdater.exe").FileVersion) <
                                new Version(stream.ToString()))
                            {
                                Console.WriteLine("Update found, downloading...");

                                webClient.DownloadFile("http://www.svenskautogrupp.se/LoLUpdater.exe",
                                    "LoLUpdater.exe");
                            }
                            else
                            {
                                Console.WriteLine("No update found.");
                                Console.ReadLine();
                            }
                            Process.Start("LoLUpdater.exe");
                        }
                    }
                }
            }
            catch
                (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
            finally
            {
                Process.Start("LoLUpdater.exe");
                Environment.Exit(0);
            }
        }
    }
}