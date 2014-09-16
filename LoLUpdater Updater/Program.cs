using System;
using System.Diagnostics;
using System.Net;
using System.IO;
namespace LoLUpdater_Updater
{
    class Program
    {
        static void Main()
        {
            try
            {
                if (!File.Exists("LoLUpdater.exe"))
                {
                    Console.WriteLine("LoLUpdater not found, downloading...");
                    using (WebClient webClient = new WebClient())
                    {
                        webClient.DownloadFile("http://www.svenskautogrupp.se/LoLUpdater.exe", "LoLUpdater.exe");
                    }
                    Process.Start("LoLUpdater.exe");
                }
                else
                {
                    Console.WriteLine("Downloading latest versioninfo...");
                    using (WebClient webClient = new WebClient())
                    {
                        webClient.DownloadFile("http://www.svenskautogrupp.se/LoLUpdater.txt", "LoLUpdater.txt");
                    }
                    Console.WriteLine("Comparing versions...");
                    if (new Version(FileVersionInfo.GetVersionInfo("LoLUpdater.exe").FileVersion) <
                        new Version(File.ReadAllLines("LoLUpdater.txt").ToString()))
                    {
                        Console.WriteLine("Update found, downloading...");
                        using (WebClient webClient = new WebClient())
                        {
                            webClient.DownloadFile("http://www.svenskautogrupp.se/LoLUpdater.exe", "LoLUpdater.exe");
                        }
                        Process.Start("LoLUpdater.exe");
                    }
                    else
                    {
                        Console.WriteLine("No update found.");
                        Console.ReadLine();
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
            finally
            {
                if(File.Exists("LoLUpdater.txt"))
                { File.Delete("LoLUpdater.txt"); }
                Environment.Exit(0);
            }

        }
    }
}
