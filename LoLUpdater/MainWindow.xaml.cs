using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WUApiLib;
namespace LoLUpdater
{
    public partial class MainWindow : Window
    {
        private LoLFiles lolFiles = new LoLFiles();

        private void AdobeAIR_Checked(object sender, RoutedEventArgs e)
        {
            adobeAlert();
        }
        private void Flash_Checked(object sender, RoutedEventArgs e)
        {
            adobeAlert();
        }
        private void OK_Click(object sender, RoutedEventArgs e)
        {
            if (Process.GetProcessesByName("LoLClient").Length > 0)
            {
                Process[] proc = Process.GetProcessesByName("LoLClient");
                proc[0].Kill();
                proc[0].WaitForExit();
            }
            taskProgress.IsIndeterminate = true;
            if (Visual.IsChecked == true)
            {
                taskProgress.Tag = "Opening System Performance Properties...";
                Process.Start("SystemPropertiesPerformance.exe");
            }
            if (WinUpdate.IsChecked == true)
            {
                taskProgress.Tag = "Setting Up Windows Update...";
                handleWindowsUpdate();
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
                handleCfg("DefaultParticleMultithreading=1");
                handlePandoUninstall();
                handleCGInstall();
                handleAdobeAndTBB();
                runCleanManager();
                handleMouseHz();
                if (Inking.IsChecked == true)
                {
                    handleCfg("Inking=0");
                                    }
                if (AdvancedReflection.IsChecked == true)
                {
                   handleCfg("AdvanceReflection=0");
                }
                if (PerPixelPointLighting.IsChecked == true)
                {
                    handleCfg("PerPixelPointLighting=0");
                }
            }

            else if (Remove.IsChecked == true)
            {
                taskProgress.Tag = "Removing Patch...";
                handleUninstall();
            }
            taskProgress.IsIndeterminate = false;
            taskProgress.Value = 100;
            taskProgress.Tag = "All Processes Completed Successfully!";
            if (MessageBox.Show("It is recommended you do a restart after patching, would you like to do it?", "LoLUpdater", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Process.Start("shutdown.exe", "-r -t 0");
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
                    File.Copy(Path.Combine(lolFiles.lolGameClientSlnPath, lolFiles.deployCgPath), lolFiles.backupCgPath, true);
                    File.Copy(Path.Combine(lolFiles.lolGameClientSlnPath, lolFiles.deployCgD3D9Path), lolFiles.backupCgD3D9Path, true);
                    File.Copy(Path.Combine(lolFiles.lolGameClientSlnPath, lolFiles.deployCgGLPath), lolFiles.backupCgGLPath, true);
                    File.Copy(Path.Combine(lolFiles.lolGameClientSlnPath, lolFiles.deployTbbPath), lolFiles.backupTbbPath, true);
                    File.Copy(Path.Combine(lolFiles.lolAirClientPath, lolFiles.deployNPSWF32Path), lolFiles.backupNPSWF32Path, true);
                    File.Copy(Path.Combine(lolFiles.lolAirClientPath, lolFiles.deployAdobeAirPath), lolFiles.backupAdobeAirPath, true);
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
                    File.Copy("tbb.dll", Path.Combine("RADS", "solutions", "lol_game_client_sln", "releases") + @"\" + new DirectoryInfo(Path.Combine("RADS", "solutions", "lol_game_client_sln", "releases")).GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\" + Path.Combine("deploy", "tbb.dll"), true);
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
                    File.Copy("tbb.dll", Path.Combine("Game", "tbb.dll"), true);
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
                    Process.Start("NvidiaCGLicence.txt");
                    if (MessageBox.Show("By clicking Yes you agree to NvidiaCGs Licence", "LoLUpdater", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        Process cg = new Process();
                        ProcessStartInfo startInfo = new ProcessStartInfo();
                        startInfo.FileName = "Cg_3_1_April2012_Setup.exe";
                        startInfo.Arguments = "/silent";
                        cg.StartInfo = startInfo;
                        cg.Start();
                        cg.WaitForExit();
                    }
                }
            }
            else
            {
                if (!File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "NVIDIA Corporation", "Cg", "Bin", "cg.dll")))
                {
                    Process.Start("NvidiaCGLicence.txt");
                    if (MessageBox.Show("By clicking Yes you agree to NvidiaCGs Licence", "LoLUpdater", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        Process cg = new Process();
                        ProcessStartInfo startInfo = new ProcessStartInfo();
                        startInfo.FileName = "Cg_3_1_April2012_Setup.exe";
                        startInfo.Arguments = "/silent";
                        cg.StartInfo = startInfo;
                        cg.Start();
                        cg.WaitForExit();
                    }
                }
            }
        }
        private void adobeAlert()
        {
            if (MessageBox.Show("We are unable to include any Adobe products, HOWEVER, you are fully capable of installing it yourself. Click yes to download and run the installer then apply the patch.", "LoLUpdater", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Process.Start("http://airdownload.adobe.com/air/win/download/14.0/AdobeAIRInstaller.exe");
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
            Version win8version = new Version(6, 2, 9200, 0);
            if (System.Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version >= win8version)
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
        }
        private void handleCfg(string setting)
        {
            if (File.Exists(Path.Combine("Config", "game.cfg")))
            {
                if (!File.ReadAllText(Path.Combine("Config", "game.cfg")).Contains(setting))
                {
                    File.AppendAllText(Path.Combine("Config", "game.cfg"), Environment.NewLine + setting);
                }
            }
            else if (File.Exists(Path.Combine("Game", "DATA", "CFG", "defaults", "game.cfg")))
            {
                if (!File.ReadAllText(Path.Combine("Game", "DATA", "CFG", "defaults", "game.cfg")).Contains(setting))
                {
                    File.AppendAllText(Path.Combine("Game", "DATA", "CFG", "defaults", "game.cfg"), Environment.NewLine + setting);
                }
                if (File.Exists(Path.Combine("Game", "DATA", "CFG", "defaults", "game.cfg")))
                {
                    if (!File.ReadAllText(Path.Combine("Game", "DATA", "CFG", "defaults", "GamePermanent.cfg")).Contains(setting))
                    {
                        File.AppendAllText(Path.Combine("Game", "DATA", "CFG", "defaults", "GamePermanent.cfg"), Environment.NewLine + setting);
                    }
                }
                if (File.Exists(Path.Combine("Game", "DATA", "CFG", "defaults", "GamePermanent_zh_MY.cfg")))
                {
                    if (!File.ReadAllText(Path.Combine("Game", "DATA", "CFG", "defaults", "GamePermanent_zh_MY.cfg")).Contains(setting))
                    {
                        File.AppendAllText(Path.Combine("Game", "DATA", "CFG", "defaults", "GamePermanent_zh_MY.cfg"), Environment.NewLine + setting);
                    }
                }
                if (File.Exists(Path.Combine("Game", "DATA", "CFG", "defaults", "GamePermanent_en_SG.cfg")))
                {
                    if (!File.ReadAllText(Path.Combine("Game", "DATA", "CFG", "defaults", "GamePermanent_en_SG.cfg")).Contains(setting))
                    {
                        File.AppendAllText(Path.Combine("Game", "DATA", "CFG", "defaults", "GamePermanent_en_SG.cfg"), Environment.NewLine + setting);
                    }
                }
            }
        }
        private void handleWindowsUpdate()
        {
            UpdateSession uSession = new UpdateSession();
            IUpdateSearcher uSearcher = uSession.CreateUpdateSearcher();
            ISearchResult uResult = uSearcher.Search("IsInstalled=0 and BrowseOnly=0 and Type='Software'");
            UpdateDownloader downloader = uSession.CreateUpdateDownloader();
            downloader.Updates = uResult.Updates;
            downloader.Download();
            UpdateCollection updatesToInstall = new UpdateCollection();
            foreach (IUpdate update in uResult.Updates)
            {
                if (update.IsDownloaded)
                    updatesToInstall.Add(update);
            }
            IUpdateInstaller installer = uSession.CreateUpdateInstaller();
            installer.Updates = updatesToInstall;
            IInstallationResult installationRes = installer.Install();
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
                case "WinUpdate":
                    lblDescription.Text = "Performs a Windows Update on the computer, might take some time. (Requires the app to be run as administratior)";
                    break;
                case "Visual":
                    lblDescription.Text = "Enables you to edit the visual style of Windows";
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
                case "Inking":
                    lblDescription.Text = "Takes off the new 'graphics'";
                    break;
                case "AdvancedReflection":
                    lblDescription.Text = "Takes off the reflections";
                    break;
                case "PerPixelPointLighting":
                    lblDescription.Text = "Takes off the particles";
                    break;
            }
        }
        private void chkOption_MouseExit(object sender, System.Windows.Input.MouseEventArgs e)
        {
            lblDescription.Text = "";
        }

        private void Cg_Checked(object sender, RoutedEventArgs e)
        {
            CGCheck();
        }

    }
}