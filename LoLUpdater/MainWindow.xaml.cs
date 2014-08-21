using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WUApiLib;

namespace LoLUpdater
{
    public partial class MainWindow
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
            if (Process.GetProcessesByName("LoLLauncher").Length > 0)
            {
                Process[] proc = Process.GetProcessesByName("LoLLauncher");
                proc[0].Kill();
                proc[0].WaitForExit();
            }
            if (Visual.IsChecked == true)
            {
                Process.Start("SystemPropertiesPerformance.exe");
            }
            if (WinUpdate.IsChecked == true)
            {
                handleWindowsUpdate();
            }
            if (!Directory.Exists("Backup"))
            {
                handleBackup();
            }
            if (Patch.IsChecked == true)
            {
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
                if (MessageBox.Show("It is recommended you do a restart after installing the patch", "LoLUpdater", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    Process.Start("shutdown.exe", "-r -t 0");
                }
            }
            else if (Remove.IsChecked == true)
            {
                handleUninstall();
                if (MessageBox.Show("It is recommended you do a restart after removing the patch", "LoLUpdater", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    Process.Start("shutdown.exe", "-r -t 0");
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
                System.IO.FileInfo fi = new System.IO.FileInfo(Path.Combine("Config", "game.cfg"));

                if (System.IO.FileAttributes.ReadOnly == fi.Attributes)
                {
                    MessageBox.Show(@"Your game.cfg Located in Config\ is read only, please remove this and try again", "LoLUpdater");
                    return;
                }

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
                    System.IO.FileInfo fi = new System.IO.FileInfo(Path.Combine("Game", "DATA", "CFG", "defaults", "game.cfg"));

                    if (System.IO.FileAttributes.ReadOnly == fi.Attributes)
                    {
                        MessageBox.Show(@"Your game.cfg Located in Game\DATA\CFG\defaults is read only, please remove this and try again", "LoLUpdater");
                        return;
                    }

                    if (File.Exists(Path.Combine("Game", "DATA", "CFG", "defaults", "GamePermanent.cfg")))
                    {
                        System.IO.FileInfo fi1 = new System.IO.FileInfo(Path.Combine("Game", "DATA", "CFG", "defaults", "GamePermanent.cfg"));

                        if (System.IO.FileAttributes.ReadOnly == fi1.Attributes)
                        {
                            MessageBox.Show(@"Your GamePermanent.cfg Located in Game\DATA\CFG\defaults is read only, please remove this and try again", "LoLUpdater");
                            return;
                        }

                        if (!File.ReadAllText(Path.Combine("Game", "DATA", "CFG", "defaults", "GamePermanent.cfg")).Contains(setting))
                        {
                            File.AppendAllText(Path.Combine("Game", "DATA", "CFG", "defaults", "GamePermanent.cfg"), Environment.NewLine + setting);
                        }
                    }
                }
                if (File.Exists(Path.Combine("Game", "DATA", "CFG", "defaults", "GamePermanent_zh_MY.cfg")))
                {
                    System.IO.FileInfo fi1 = new System.IO.FileInfo(Path.Combine("Game", "DATA", "CFG", "defaults", "GamePermanent_zh_MY.cfg"));

                    if (System.IO.FileAttributes.ReadOnly == fi1.Attributes)
                    {
                        MessageBox.Show(@"Your GamePermanent_zh_MY.cfg Located in Game\DATA\CFG\defaults is read only, please remove this and try again", "LoLUpdater");
                        return;
                    }

                    if (!File.ReadAllText(Path.Combine("Game", "DATA", "CFG", "defaults", "GamePermanent_zh_MY.cfg")).Contains(setting))
                    {
                        File.AppendAllText(Path.Combine("Game", "DATA", "CFG", "defaults", "GamePermanent_zh_MY.cfg"), Environment.NewLine + setting);
                    }
                }
                if (File.Exists(Path.Combine("Game", "DATA", "CFG", "defaults", "GamePermanent_en_SG.cfg")))
                {
                    System.IO.FileInfo fi1 = new System.IO.FileInfo(Path.Combine("Game", "DATA", "CFG", "defaults", "GamePermanent_en_SG.cfg"));

                    if (System.IO.FileAttributes.ReadOnly == fi1.Attributes)
                    {
                        MessageBox.Show(@"Your GamePermanent_en_SG.cfg Located in Game\DATA\CFG\defaults is read only, please remove this and try again", "LoLUpdater");
                        return;
                    }

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

        private void Cg_Checked(object sender, RoutedEventArgs e)
        {
            CGCheck();
        }

        private void Image_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Xclose.Source = new BitmapImage(new Uri("Resources/closemouseenter.png", UriKind.Relative));
        }

        private void Image_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Xclose.Source = new BitmapImage(new Uri("Resources/close.png", UriKind.Relative));
        }

        private void Image_MouseEnter_1(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Xminimize.Source = new BitmapImage(new Uri("Resources/minimizemouseneter.png", UriKind.Relative));
        }

        private void Image_MouseLeave_1(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Xminimize.Source = new BitmapImage(new Uri("Resources/minimize.png", UriKind.Relative));
        }

        private void Xclose_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Xminimize_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
    }
}