using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
namespace LoLUpdater
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            System.Windows.Threading.DispatcherTimer pingTimer = new System.Windows.Threading.DispatcherTimer();
            pingTimer.Tick += new EventHandler(pingTimer_Tick);
            pingTimer.Interval = new TimeSpan(0, 0, 5);
            pingTimer.Start();
        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            handlePing();
        }
        private void pingTimer_Tick(object sender, EventArgs e)
        {
            handlePing();
        }
        private void AdobeAIR_Checked(object sender, RoutedEventArgs e)
        {
            adobeAlert();
        }
        private void Flash_Checked(object sender, RoutedEventArgs e)
        {
            adobeAlert();
        }
        public void DoEvents()
        {
            DispatcherFrame frame = new DispatcherFrame();
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background,
                new DispatcherOperationCallback(ExitFrame), frame);
            Dispatcher.PushFrame(frame);
        }
        public object ExitFrame(object f)
        {
            ((DispatcherFrame)f).Continue = false;
            return null;
        }
        private void OK_Click(object sender, RoutedEventArgs e)
        {
            if (Process.GetProcessesByName("LoLClient").Length > 0)
            {
                Process[] proc = Process.GetProcessesByName("LoLClient");
                proc[0].Kill();
                proc[0].WaitForExit();
            }
            handleCGInstall();
            taskProgress.IsIndeterminate = true;
            DoEvents();
            if (Clean.IsChecked == true)
            {
                taskProgress.Tag = "Opening Disk Cleanup...";
                runCleanManager();
            }
            taskProgress.Tag = "Performing Backup...";
            if (!Directory.Exists("Backup"))
            {
                taskProgress.Tag = "Performing Backup...";
                handleBackup();
            }
            if (Patch.IsChecked == true)
            {
                taskProgress.Tag = "Patching...";
                handleParticleMultithreading();
                handlePandoUninstall();
                handleCGInstall();
                handleAdobeAndTBB();
            }
            else if (Remove.IsChecked == true)
            {
                taskProgress.Tag = "Removing Patch...";
                handleUninstall();
            }
            taskProgress.IsIndeterminate = false;
            taskProgress.Value = 100;
            taskProgress.Tag = "All Processes Completed Successfully!";
        }
        private void handleParticleMultithreading()
        {
            if (File.Exists(Path.Combine("Config", "game.cfg")))
            {
                if (!File.ReadAllText(Path.Combine("Config", "game.cfg")).Contains("DefaultParticleMultithreading=1"))
                {
                    File.AppendAllText(Path.Combine("Config", "game.cfg"), Environment.NewLine + "DefaultParticleMultithreading=1");
                }
            }
            else if (File.Exists(Path.Combine("Game", "DATA", "CFG", "defaults", "game.cfg")))
            {
                if (!File.ReadAllText(Path.Combine("Game", "DATA", "CFG", "defaults", "game.cfg")).Contains("DefaultParticleMultithreading=1"))
                {
                    File.AppendAllText(Path.Combine("Game", "DATA", "CFG", "defaults", "game.cfg"), Environment.NewLine + "DefaultParticleMultithreading=1");
                }
                if (File.Exists(Path.Combine("Game", "DATA", "CFG", "defaults", "game.cfg")))
                {
                    if (!File.ReadAllText(Path.Combine("Game", "DATA", "CFG", "defaults", "GamePermanent.cfg")).Contains("DefaultParticleMultithreading=1"))
                    {
                        File.AppendAllText(Path.Combine("Game", "DATA", "CFG", "defaults", "GamePermanent.cfg"), Environment.NewLine + "DefaultParticleMultithreading=1");
                    }
                }
                if (File.Exists(Path.Combine("Game", "DATA", "CFG", "defaults", "GamePermanent_zh_MY.cfg")))
                {
                    if (!File.ReadAllText(Path.Combine("Game", "DATA", "CFG", "defaults", "GamePermanent_zh_MY.cfg")).Contains("DefaultParticleMultithreading=1"))
                    {
                        File.AppendAllText(Path.Combine("Game", "DATA", "CFG", "defaults", "GamePermanent_zh_MY.cfg"), Environment.NewLine + "DefaultParticleMultithreading=1");
                    }
                }
                if (File.Exists(Path.Combine("Game", "DATA", "CFG", "defaults", "GamePermanent_en_SG.cfg")))
                {
                    if (!File.ReadAllText(Path.Combine("Game", "DATA", "CFG", "defaults", "GamePermanent_en_SG.cfg")).Contains("DefaultParticleMultithreading=1"))
                    {
                        File.AppendAllText(Path.Combine("Game", "DATA", "CFG", "defaults", "GamePermanent_en_SG.cfg"), Environment.NewLine + "DefaultParticleMultithreading=1");
                    }
                }
            }
        }
        private void handleBackup()
        {
            if (!Directory.Exists("Backup"))
            {
                Directory.CreateDirectory("Backup");
                if (Directory.Exists("RADS"))
                {
                    if (File.Exists(Path.Combine("Config", "game.cfg")))
                    {
                        File.Copy(Path.Combine("Config", "game.cfg"), Path.Combine("Backup", "game.cfg"), true);
                    }
                    File.Copy(Path.Combine("RADS", "solutions", "lol_game_client_sln", "releases") + @"\" + new DirectoryInfo(Path.Combine("RADS", "solutions", "lol_game_client_sln", "releases")).GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\" + Path.Combine("deploy", "Cg.dll"), Path.Combine("Backup", "Cg.dll"), true);
                    File.Copy(Path.Combine("RADS", "solutions", "lol_game_client_sln", "releases") + @"\" + new DirectoryInfo(Path.Combine("RADS", "solutions", "lol_game_client_sln", "releases")).GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\" + Path.Combine("deploy", "CgD3D9.dll"), Path.Combine("Backup", "CgD3D9.dll"), true);
                    File.Copy(Path.Combine("RADS", "solutions", "lol_game_client_sln", "releases") + @"\" + new DirectoryInfo(Path.Combine("RADS", "solutions", "lol_game_client_sln", "releases")).GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\" + Path.Combine("deploy", "CgGL.dll"), Path.Combine("Backup", "CgGL.dll"), true);
                    File.Copy(Path.Combine("RADS", "solutions", "lol_game_client_sln", "releases") + @"\" + new DirectoryInfo(Path.Combine("RADS", "solutions", "lol_game_client_sln", "releases")).GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\" + Path.Combine("deploy", "tbb.dll"), Path.Combine("Backup", "tbb.dll"), true);
                    File.Copy(Path.Combine("RADS", "projects", "lol_air_client", "releases") + @"\" + new DirectoryInfo(Path.Combine("RADS", "projects", "lol_air_client", "releases")).GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\" + Path.Combine("deploy", "Adobe Air", "Versions", "1.0", "Resources", "NPSWF32.dll"), Path.Combine("Backup", "NPSWF32.dll"), true);
                    File.Copy(Path.Combine("RADS", "projects", "lol_air_client", "releases") + @"\" + new DirectoryInfo(Path.Combine("RADS", "projects", "lol_air_client", "releases")).GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\" + Path.Combine("deploy", "Adobe Air", "Versions", "1.0", "Adobe AIR.dll"), Path.Combine("Backup", "Adobe AIR.dll"), true);
                }
                else if (Directory.Exists("Game"))
                {
                    if (File.Exists(Path.Combine("Game", "DATA", "CFG", "defaults", "game.cfg")))
                    {
                        File.Copy(Path.Combine("Game", "DATA", "CFG", "defaults", "game.cfg"), Path.Combine("Backup", "game.cfg"), true);
                        File.Copy(Path.Combine("Game", "DATA", "CFG", "defaults", "GamePermanent.cfg"), Path.Combine("Backup", "GamePermanent.cfg"), true);
                        if (File.Exists(Path.Combine("Game", "DATA", "CFG", "defaults", "GamePermanent_zh_MY.cfg")))
                        {
                            File.Copy(Path.Combine("Game", "DATA", "CFG", "defaults", "GamePermanent_zh_MY.cfg"), Path.Combine("Backup", "GamePermanent_zh_MY.cfg"), true);
                        }
                        if (File.Exists(Path.Combine("Game", "DATA", "CFG", "defaults", "GamePermanent_en_SG.cfg")))
                        {
                            File.Copy(Path.Combine("Game", "DATA", "CFG", "defaults", "GamePermanent_én_SG.cfg"), Path.Combine("Backup", "GamePermanent_en_SG.cfg"), true);
                        }
                    }
                    File.Copy(Path.Combine("Game", "Cg.dll"), Path.Combine("Backup", "Cg.dll"), true);
                    File.Copy(Path.Combine("Game", "CgGL.dll"), Path.Combine("Backup", "CgGL.dll"), true);
                    File.Copy(Path.Combine("Game", "CgD3D9.dll"), Path.Combine("Backup", "CgD3D9.dll"), true);
                    File.Copy(Path.Combine("Game", "tbb.dll"), Path.Combine("Backup", "tbb.dll"), true);
                    File.Copy(Path.Combine("Air", "Adobe AIR", "Versions", "1.0", "Adobe AIR.dll"), Path.Combine("Backup", "Adobe AIR.dll"), true);
                    File.Copy(Path.Combine("Air", "Adobe AIR", "Versions", "1.0", "Resources", "NPSWF32.dll"), Path.Combine("Backup", "NPSWF32.dll"), true);
                }
            }
        }
        private void handleAdobeAndTBB()
        {
            if (Directory.Exists("Rads"))
            {
                if (tbb.IsChecked == true)
                {
                    File.WriteAllBytes(Path.Combine("RADS", "solutions", "lol_game_client_sln", "releases") + @"\" + new DirectoryInfo(Path.Combine("RADS", "solutions", "lol_game_client_sln", "releases")).GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\" + Path.Combine("deploy", "tbb.dll"), Properties.Resources.tbb);
                }

                if (AdobeAIR.IsChecked == true)
                {
                    if (Environment.Is64BitProcess == true)
                    {
                        File.Copy(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Common Files", "Adobe AIR", "Versions", "1.0", "Adobe AIR.dll"), Path.Combine("RADS", "projects", "lol_air_client", "releases") + @"\" + new DirectoryInfo(Path.Combine("RADS", "projects", "lol_air_client", "releases")).GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\" + Path.Combine("deploy", "Adobe Air", "Versions", "1.0", "Adobe AIR.dll"), true);
                    }
                    else
                    {
                        File.Copy(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Common Files", "Adobe AIR", "Versions", "1.0", "Adobe AIR.dll"), Path.Combine("RADS", "projects", "lol_air_client", "releases") + @"\" + new DirectoryInfo(Path.Combine("RADS", "projects", "lol_air_client", "releases")).GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\" + Path.Combine("deploy", "Adobe Air", "Versions", "1.0", "Adobe AIR.dll"), true);
                    }
                }
                if (Flash.IsChecked == true)
                {
                    if (Environment.Is64BitProcess == true)
                    {
                        File.Copy(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Common Files", "Adobe AIR", "Versions", "1.0", "Resources", "NPSWF32.dll"), Path.Combine("RADS", "projects", "lol_air_client", "releases") + @"\" + new DirectoryInfo(Path.Combine("RADS", "projects", "lol_air_client", "releases")).GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\" + Path.Combine("deploy", "Adobe Air", "Versions", "1.0", "Resources", "NPSWF32.dll"), true);
                    }
                    else
                    {
                        File.Copy(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Common Files", "Adobe AIR", "Versions", "1.0", "Resources", "NPSWF32.dll"), Path.Combine("RADS", "projects", "lol_air_client", "releases") + @"\" + new DirectoryInfo(Path.Combine("RADS", "projects", "lol_air_client", "releases")).GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\" + Path.Combine("deploy", "Adobe Air", "Versions", "1.0", "Resources", "NPSWF32.dll"), true);
                    }
                }
            }
            if (Directory.Exists("Game"))
            {
                if (tbb.IsChecked == true)
                {
                    File.WriteAllBytes(Path.Combine("Game", "tbb.dll"), Properties.Resources.tbb);
                }
                if (AdobeAIR.IsChecked == true)
                {
                    if (Environment.Is64BitProcess == true)
                    {
                        File.Copy(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Common Files", "Adobe AIR", "Versions", "1.0", "Adobe AIR.dll"), Path.Combine("Air", "Adobe Air", "Versions", "1.0", "Adobe AIR.dll"), true);
                    }
                    else
                    {
                        File.Copy(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Common Files", "Adobe AIR", "Versions", "1.0", "Adobe AIR.dll"), Path.Combine("Air", "Adobe Air", "Versions", "1.0", "Adobe AIR.dll"), true);
                    }
                }
                if (Flash.IsChecked == true)
                {
                    if (Environment.Is64BitProcess == true)
                    {
                        File.Copy(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Common Files", "Adobe AIR", "Versions", "1.0", "Resources", "NPSWF32.dll"), Path.Combine("Air", "Adobe Air", "Versions", "1.0", "Resources", "NPSWF32.dll"), true);
                    }
                    else
                    {
                        File.Copy(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Common Files", "Adobe AIR", "Versions", "1.0", "Resources", "NPSWF32.dll"), Path.Combine("Air", "Adobe Air", "Versions", "1.0", "Resources", "NPSWF32.dll"), true);
                    }
                }
            }
        }
        private void handleCGInstall()
        {
            CGCheck();
            if (Directory.Exists("RADS"))
            {
                if (Cg.IsChecked == true)
                {
                    File.Copy(Path.Combine(Environment.GetEnvironmentVariable("CG_BIN_PATH", EnvironmentVariableTarget.User), "Cg.dll"), Path.Combine("RADS", "solutions", "lol_game_client_sln", "releases") + @"\" + new DirectoryInfo(Path.Combine("RADS", "solutions", "lol_game_client_sln", "releases")).GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\" + Path.Combine("deploy", "Cg.dll"), true);
                }
                if (Cg1.IsChecked == true)
                {
                    File.Copy(Path.Combine(Environment.GetEnvironmentVariable("CG_BIN_PATH", EnvironmentVariableTarget.User), "Cg.dll"), Path.Combine("RADS", "projects", "lol_launcher", "releases") + @"\" + new DirectoryInfo(Path.Combine("RADS", "projects", "lol_launcher", "releases")).GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\" + Path.Combine("deploy", "Cg.dll"), true);
                }

                if (CgGL.IsChecked == true)
                {
                    File.Copy(Path.Combine(Environment.GetEnvironmentVariable("CG_BIN_PATH", EnvironmentVariableTarget.User), "CgGL.dll"), Path.Combine("RADS", "solutions", "lol_game_client_sln", "releases") + @"\" + new DirectoryInfo(Path.Combine("RADS", "solutions", "lol_game_client_sln", "releases")).GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\" + Path.Combine("deploy", "CgGL.dll"), true);
                }
                if (CgGL1.IsChecked == true)
                {
                    File.Copy(Path.Combine(Environment.GetEnvironmentVariable("CG_BIN_PATH", EnvironmentVariableTarget.User), "CgGL.dll"), Path.Combine("RADS", "projects", "lol_launcher", "releases") + @"\" + new DirectoryInfo(Path.Combine("RADS", "projects", "lol_launcher", "releases")).GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\" + Path.Combine("deploy", "CgGL.dll"), true);
                }

                if (CgD3D9.IsChecked == true)
                {
                    File.Copy(Path.Combine(Environment.GetEnvironmentVariable("CG_BIN_PATH", EnvironmentVariableTarget.User), "CgD3D9.dll"), Path.Combine("RADS", "solutions", "lol_game_client_sln", "releases") + @"\" + new DirectoryInfo(Path.Combine("RADS", "solutions", "lol_game_client_sln", "releases")).GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\" + Path.Combine("deploy", "CgD3D9.dll"), true);
                }
                if (CgD3D1.IsChecked == true)
                {
                    File.Copy(Path.Combine(Environment.GetEnvironmentVariable("CG_BIN_PATH", EnvironmentVariableTarget.User), "CgD3D9.dll"), Path.Combine("RADS", "projects", "lol_launcher", "releases") + @"\" + new DirectoryInfo(Path.Combine("RADS", "projects", "lol_launcher", "releases")).GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\" + Path.Combine("deploy", "CgD3D9.dll"), true);
                }
            }
            else if (Directory.Exists("Game"))
            {
                if (Cg.IsChecked == true)
                {
                    File.Copy(Path.Combine(Environment.GetEnvironmentVariable("CG_BIN_PATH", EnvironmentVariableTarget.User), "Cg.dll"), Path.Combine("Game", "Cg.dll"), true);
                }

                if (CgGL.IsChecked == true)
                {
                    File.Copy(Path.Combine(Environment.GetEnvironmentVariable("CG_BIN_PATH", EnvironmentVariableTarget.User), "CgGL.dll"), Path.Combine("Game", "CgGL.dll"), true);
                }

                if (CgD3D9.IsChecked == true)
                {
                    File.Copy(Path.Combine(Environment.GetEnvironmentVariable("CG_BIN_PATH", EnvironmentVariableTarget.User), "CgD3D9.dll"), Path.Combine("Game", "CgD3D9.dll"), true);
                }
            }
        }
        private void handlePandoUninstall()
        {
            if (Environment.Is64BitProcess == true)
            {
                if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Pando Networks", "Media Booster", "uninst.exe")))
                {
                    var PMB = new ProcessStartInfo();
                    var process = new Process();
                    PMB.FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Pando Networks", "Media Booster", "uninst.exe");
                    PMB.Arguments = "/silent";
                    process.StartInfo = PMB;
                    process.Start();
                }
            }
            else
            {
                if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Pando Networks", "Media Booster", "uninst.exe")))
                {
                    var PMB = new ProcessStartInfo();
                    var process = new Process();
                    PMB.FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Pando Networks", "Media Booster", "uninst.exe");
                    PMB.Arguments = "/silent";
                    process.StartInfo = PMB;
                    process.Start();
                }
            }
        }
        private void handleUninstall()
        {
            if (Directory.Exists("RADS"))
            {
                if (File.Exists(Path.Combine("Backup", "game.cfg")))
                {
                    File.Copy(Path.Combine("Backup", "game.cfg"), Path.Combine("Config", "game.cfg"), true);
                }
                File.Copy(Path.Combine("Backup", "Cg.dll"), Path.Combine("RADS", "solutions", "lol_game_client_sln", "releases") + @"\" + new DirectoryInfo(Path.Combine("RADS", "solutions", "lol_game_client_sln", "releases")).GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\" + Path.Combine("deploy", "Cg.dll"), true);
                File.Copy(Path.Combine("Backup", "Cg.dll"), Path.Combine("RADS", "solutions", "lol_game_client_sln", "releases") + @"\" + new DirectoryInfo(Path.Combine("RADS", "solutions", "lol_game_client_sln", "releases")).GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\" + Path.Combine("deploy", "Cg.dll"), true);
                File.Copy(Path.Combine("Backup", "CgGL.dll"), Path.Combine("RADS", "solutions", "lol_game_client_sln", "releases") + @"\" + new DirectoryInfo(Path.Combine("RADS", "solutions", "lol_game_client_sln", "releases")).GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\" + Path.Combine("deploy", "CgGL.dll"), true);
                File.Copy(Path.Combine("Backup", "CgGL.dll"), Path.Combine("RADS", "solutions", "lol_game_client_sln", "releases") + @"\" + new DirectoryInfo(Path.Combine("RADS", "solutions", "lol_game_client_sln", "releases")).GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\" + Path.Combine("deploy", "CgGL.dll"), true);
                File.Copy(Path.Combine("Backup", "CgD3D9.dll"), Path.Combine("RADS", "solutions", "lol_game_client_sln", "releases") + @"\" + new DirectoryInfo(Path.Combine("RADS", "solutions", "lol_game_client_sln", "releases")).GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\" + Path.Combine("deploy", "CgD3D9.dll"), true);
                File.Copy(Path.Combine("Backup", "CgD3D9.dll"), Path.Combine("RADS", "solutions", "lol_game_client_sln", "releases") + @"\" + new DirectoryInfo(Path.Combine("RADS", "solutions", "lol_game_client_sln", "releases")).GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\" + Path.Combine("deploy", "CgD3D9.dll"), true);
                File.Copy(Path.Combine("Backup", "tbb.dll"), Path.Combine("RADS", "solutions", "lol_game_client_sln", "releases") + @"\" + new DirectoryInfo(Path.Combine("RADS", "solutions", "lol_game_client_sln", "releases")).GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\" + Path.Combine("deploy", "CgD3D9.dll"), true);
                File.Copy(Path.Combine("Backup", "NPSWF32.dll"), Path.Combine("RADS", "projects", "lol_air_client", "releases") + @"\" + new DirectoryInfo(Path.Combine("RADS", "projects", "lol_air_client", "releases")).GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\" + Path.Combine("deploy", "Adobe Air", "Versions", "1.0", "Resources", "NPSWF32.dll"), true);
                File.Copy(Path.Combine("Backup", "Adobe AIR.dll"), Path.Combine("RADS", "projects", "lol_air_client", "releases") + @"\" + new DirectoryInfo(Path.Combine("RADS", "projects", "lol_air_client", "releases")).GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\" + Path.Combine("deploy", "Adobe Air", "Versions", "1.0", "Adobe AIR.dll"), true);
            }
            else if (Directory.Exists("Games"))
            {
                File.Copy(Path.Combine("Backup", "game.cfg"), Path.Combine("Game", "DATA", "CFG", "defaults", "game.cfg"), true);
                File.Copy(Path.Combine("Backup", "GamePermanent.cfg"), Path.Combine("Game", "DATA", "CFG", "defaults", "GamePermanent.cfg"), true);

                if (File.Exists(Path.Combine("Backup", "GamePermanent_zh_MY.cfg")))
                {
                    File.Copy(Path.Combine("Backup", "GamePermanent_zh_MY.cfg"), Path.Combine("Game", "DATA", "CFG", "defaults", "GamePermanent_zh_MY.cfg"), true);
                }
                if (File.Exists(Path.Combine("Backup", "GamePermanent_en_SG.cfg")))
                {
                    File.Copy(Path.Combine("Backup", "GamePermanent_en_SG.cfg"), Path.Combine("Game", "DATA", "CFG", "defaults", "GamePermanent_en_SG.cfg"), true);
                }
                File.Copy(Path.Combine("Backup", "Cg.dll"), Path.Combine("Game", "Cg.dll"), true);
                File.Copy(Path.Combine("Backup", "CgGL.dll"), Path.Combine("Game", "CgGL.dll"), true);
                File.Copy(Path.Combine("Backup", "CgD3D9.dll"), Path.Combine("Game", "CgD3D9.dll"), true);
                File.Copy(Path.Combine("Backup", "tbb.dll"), Path.Combine("Game", "tbb.dll"), true);
                File.Copy(Path.Combine("Backup", "NPSWF32.dll"), Path.Combine("Air", "Adobe AIR", "Versions", "1.0", "Resources", "NPSWF32.dll"), true);
                File.Copy(Path.Combine("Backup", "Adobe AIR.dll"), Path.Combine("Air", "Adobe AIR", "Versions", "1.0", "Adobe AIR.dll"), true);
            }
            Directory.Delete("Backup", true);
        }
        private void CGCheck()
        {
            if (Environment.Is64BitProcess == true)
            {
                if (!File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "NVIDIA Corporation", "Cg", "Bin", "cg.dll")))
                {
                    File.WriteAllBytes("Cg-3.1 April2012 Setup.exe", Properties.Resources.Cg_3_1_April2012_Setup);
                    Process cg = new Process();
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.FileName = "Cg-3.1 April2012 Setup.exe";
                    startInfo.Arguments = "/silent";
                    cg.StartInfo = startInfo;
                    cg.Start();
                    cg.WaitForExit();
                    File.Delete("Cg-3.1 April2012 Setup.exe");
                }
            }
            else
            {
                if (!File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "NVIDIA Corporation", "Cg", "Bin", "cg.dll")))
                {
                    File.WriteAllBytes("Cg-3.1 April2012 Setup.exe", Properties.Resources.Cg_3_1_April2012_Setup);
                    Process cg = new Process();
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.FileName = "Cg-3.1 April2012 Setup.exe";
                    startInfo.Arguments = "/silent";
                    cg.StartInfo = startInfo;
                    cg.Start();
                    cg.WaitForExit();
                    File.Delete("Cg-3.1 April2012 Setup.exe");
                }
            }
        }
        private void adobeAlert()
        {
            MessageBoxResult alertMessage = MessageBox.Show("We are unable to include any Adobe products, HOWEVER, you are fully capable of installing it yourself. Click yes to download and run the installer then apply the patch.", "LoLUpdater", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (alertMessage == MessageBoxResult.Yes)
            {
                Process.Start("http://labsdownload.adobe.com/pub/labs/flashruntimes/air/air14_win.exe");
            }
        }
        private void runCleanManager()
        {
            var cm = new ProcessStartInfo();
            var process = new Process();
            cm.FileName = "cleanmgr.exe";
            cm.Arguments = "sagerun:1";
            process.StartInfo = cm;
            process.Start();
        }
        private void handleMouseHz()
        {
            RegistryKey mousehz;

            if (Environment.Is64BitProcess)
            {
                mousehz = Registry.LocalMachine.CreateSubKey(Path.Combine("SOFTWARE", "Microsoft", "Windows NT", "CurrentVersion", "AppCompatFlags", "Layers"));
            }
            else
            {
                mousehz = Registry.LocalMachine.CreateSubKey(Path.Combine("SOFTWARE", "WoW64Node", "Microsoft", "Windows NT", "CurrentVersion", "AppCompatFlags", "Layers"));
            }
            mousehz.SetValue("NoDTToDITMouseBatch", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "explorer.exe"), RegistryValueKind.String);
            var cmd = new ProcessStartInfo();
            var process = new Process();
            cmd.FileName = "cmd.exe";
            cmd.Verb = "runas";
            cmd.Arguments = "/C Rundll32 apphelp.dll,ShimFlushCache";
            process.StartInfo = cmd;
            process.Start();
        }
        private static readonly System.Collections.Generic.Dictionary<string, string> Ping_Dictionary = new System.Collections.Generic.Dictionary<string, string>()
        {
            { "EUW", "190.93.245.13"},
            { "NA", "64.7.194.1"},
            { "Garena Singapore", "lol.garena.com"},
            { "Garena Philippines", "garena.ph"}
        };
        private void handlePing()
        {
            if (!IsNetworkAvailable())
            {
                Label.Content = "No Internet";
            }
            else if (Ping_Server.SelectedIndex == -1)
            {
                long? ping = null;
                for (int index = 0; index < Ping_Dictionary.Count; index++)
                {
                    var item = Ping_Dictionary.ElementAt(index);
                    long itemPing = getPing(item.Value);

                    if (ping == null || itemPing < ping)
                    {
                        ping = itemPing;
                        Ping_Server.SelectedIndex = index;
                    }
                }
                Label.Content = ping;
            }
            else
            {
                string ip = Ping_Dictionary.ElementAt(Ping_Server.SelectedIndex).Value;
                Label.Content = getPing(ip);
            }
        }
        private long getPing(string ip)
        {
            Ping ping = new Ping();
            PingReply reply = ping.Send(ip);
            return reply.RoundtripTime;
        }
        private bool IsNetworkAvailable()
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
                return false;
            foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                if ((ni.OperationalStatus != OperationalStatus.Up) ||
                    (ni.NetworkInterfaceType == NetworkInterfaceType.Loopback) ||
                    (ni.NetworkInterfaceType == NetworkInterfaceType.Tunnel))
                    continue;
                if ((ni.Description.IndexOf("virtual", StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (ni.Name.IndexOf("virtual", StringComparison.OrdinalIgnoreCase) >= 0))
                    continue;
                if (ni.Description.Equals("Microsoft Loopback Adapter", StringComparison.OrdinalIgnoreCase))
                    continue;
                return true;
            }
            return false;
        }
        private void ComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            handlePing();
        }
        //Todo: Use WPF to do this so we can remove the WinForms using
        private void chkOption_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var chkBox = (CheckBox)sender;
            switch (chkBox.Name)
            {
                case "AdobeAIR":
                    lblDescription.Text = "Provides you a link to the Adobe AIR redistributable so you can install it before patching. This upgrades the PvP.NET client";
                    break;
                case "Flash":
                    lblDescription.Text = "Provides you a link to the Adobe AIR redistributable so you can install it before patching. This upgrades the built in Flash player in the Air Client";
                    break;
                case "Cg":
                    lblDescription.Text = "Installs one of the DLLs from the Nvidia CG toolkit, yes you need it even if you are on ATI/Intel. This modifies the shader.";
                    break;
                case "CgD3D9":
                    lblDescription.Text = "Installs one of the DLLs from the Nvidia CG toolkit, yes you need it even if you are on ATI/Intel. This modifies the shader.";
                    break;
                case "CgGL":
                    lblDescription.Text = "Installs one of the DLLs from the Nvidia CG toolkit, yes you need it even if you are on ATI/Intel. This modifies the shader.";
                    break;
                case "tbb":
                    lblDescription.Text = "Installs a custom lightweight tbb.dll file that increases the fps of the game, This makes multiprocessing available for LoL";
                    break;
                case "Clean":
                    lblDescription.Text = "Do a quick clean of the harddrive using the Windows automatic disk cleanup manager";
                    break;
                case "Cg1":
                    lblDescription.Text = "Installs one of the DLLs from the Nvidia CG toolkit, yes you need it even if you are on ATI/Intel. This modifies the shader.";
                    break;
                case "CgD3D1":
                    lblDescription.Text = "Installs one of the DLLs from the Nvidia CG toolkit, yes you need it even if you are on ATI/Intel. This modifies the shader.";
                    break;
                case "CgGL1":
                    lblDescription.Text = "Installs one of the DLLs from the Nvidia CG toolkit, yes you need it even if you are on ATI/Intel. This modifies the shader.";
                    break;
            }
        }
        private void chkOption_MouseExit(object sender, System.Windows.Input.MouseEventArgs e)
        {
            lblDescription.Text = "";
        }
    }
}