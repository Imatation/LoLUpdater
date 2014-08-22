using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace LoLUpdaterXP
{
    public partial class MainWindow
    {
        private readonly LoLFiles _lolFiles = new LoLFiles();

        private void AdobeAIR_Checked(object sender, RoutedEventArgs e)
        {
            AdobeAlert();
        }

        private void Flash_Checked(object sender, RoutedEventArgs e)
        {
            AdobeAlert();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            Kill("LoLClient");
            Kill("LoLLauncher");
            Kill("League of Legends");
            if (!Directory.Exists("Backup"))
            {
                HandleBackup();
            }
            if (Patch.IsChecked == true)
            {
                HandlePatch();
            }
            else if (Remove.IsChecked == true)
            {
                HandleUninstall();
            }
        }

        private static void Kill(string process)
        {
            if (Process.GetProcessesByName(process).Length <= 0) return;
            var proc = Process.GetProcessesByName(process);
            proc[0].Kill();
            proc[0].WaitForExit();
        }
        private void HandlePatch()
        {
            HandleCfg("DefaultParticleMultithreading=1");
            HandlePandoUninstall();
            HandleCgInstall();
            HandleAdobeAndTbb();
            RunCleanManager();
            if (Inking.IsChecked == true)
            {
                HandleCfg("Inking=0");
            }
            if (AdvancedReflection.IsChecked == true)
            {
                HandleCfg("AdvanceReflection=0");
            }
            if (PerPixelPointLighting.IsChecked == true)
            {
                HandleCfg("PerPixelPointLighting=0");
            }
            if (
                MessageBox.Show("It is recommended you do a restart after installing the patch", "LoLUpdater",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Process.Start("shutdown.exe", "-r -t 0");
            }
        }

        private void HandleBackup()
        {
            if (Directory.Exists("Backup")) return;
            Directory.CreateDirectory("Backup");
            if (Directory.Exists("RADS"))
            {
                if (File.Exists(Path.Combine("Config", "game.cfg")))
                {
                    File.Copy(Path.Combine("Config", "game.cfg"), Path.Combine("Backup", "game.cfg"), true);
                }
                File.Copy(Path.Combine(_lolFiles.LolGameClientSlnPath, _lolFiles.DeployCgPath), _lolFiles.BackupCgPath,
                    true);
                File.Copy(Path.Combine(_lolFiles.LolGameClientSlnPath, _lolFiles.DeployCgD3D9Path),
                    _lolFiles.BackupCgD3D9Path, true);
                File.Copy(Path.Combine(_lolFiles.LolGameClientSlnPath, _lolFiles.DeployCgGlPath),
                    _lolFiles.BackupCgGlPath, true);
                File.Copy(Path.Combine(_lolFiles.LolGameClientSlnPath, _lolFiles.DeployTbbPath),
                    _lolFiles.BackupTbbPath, true);
                File.Copy(Path.Combine(_lolFiles.LolAirClientPath, _lolFiles.DeployNpswf32Path),
                    _lolFiles.BackupNpswf32Path, true);
                File.Copy(Path.Combine(_lolFiles.LolAirClientPath, _lolFiles.DeployAdobeAirPath),
                    _lolFiles.BackupAdobeAirPath, true);
            }
            else if (Directory.Exists("Game"))
            {
                if (File.Exists(Path.Combine("Game", "DATA", "CFG", "defaults", "game.cfg")))
                {
                    GameBackup("game.cfg");
                    GameBackup("GamePermanent.cfg");
                    if (File.Exists(Path.Combine("Game", "DATA", "CFG", "defaults", "GamePermanent_zh_MY.cfg")))
                    {
                        GameBackup("GamePermanent_zh_MY.cfg");
                    }
                    if (File.Exists(Path.Combine("Game", "DATA", "CFG", "defaults", "GamePermanent_en_SG.cfg")))
                    {
                        GameBackup("GamePermanent_en_SG.cfg");
                    }
                }
                GameFileBackup("Cg.dll");
                GameFileBackup("CgGL.dll");
                GameFileBackup("CgD3D9.dll");
                GameFileBackup("tbb.dll");
                File.Copy(Path.Combine("Air", "Adobe AIR", "Versions", "1.0", "Adobe AIR.dll"),
                    Path.Combine("Backup", "Adobe AIR.dll"), true);
                File.Copy(Path.Combine("Air", "Adobe AIR", "Versions", "1.0", "Resources", "NPSWF32.dll"),
                    Path.Combine("Backup", "NPSWF32.dll"), true);
            }
        }

        private static void GameFileBackup(string file)
        {
            File.Copy(Path.Combine("Game", file), Path.Combine("Backup", file), true);
        }

        private static void GameBackup(string file)
        {
            File.Copy(Path.Combine("Game", "DATA", "CFG", "defaults", file),
                Path.Combine("Backup", file), true);
        }

        private void HandleAdobeAndTbb()
        {
            if (Directory.Exists("Rads"))
            {
                if (Tbb.IsChecked == true)
                {
                    File.Copy("tbb.dll",
                        Path.Combine("RADS", "solutions", "lol_game_client_sln", "releases") + @"\" +
                        new DirectoryInfo(Path.Combine("RADS", "solutions", "lol_game_client_sln", "releases"))
                            .GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\" +
                        Path.Combine("deploy", "tbb.dll"), true);
                }
                if (AdobeAir.IsChecked == true)
                {
                    if (Environment.Is64BitProcess)
                    {
                        File.Copy(
                            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
                                "Common Files", "Adobe AIR", "Versions", "1.0", "Adobe AIR.dll"),
                            Path.Combine("RADS", "projects", "lol_air_client", "releases") + @"\" +
                            new DirectoryInfo(Path.Combine("RADS", "projects", "lol_air_client", "releases"))
                                .GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\" +
                            Path.Combine("deploy", "Adobe Air", "Versions", "1.0", "Adobe AIR.dll"), true);
                    }
                    else
                    {
                        File.Copy(
                            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                                "Common Files", "Adobe AIR", "Versions", "1.0", "Adobe AIR.dll"),
                            Path.Combine("RADS", "projects", "lol_air_client", "releases") + @"\" +
                            new DirectoryInfo(Path.Combine("RADS", "projects", "lol_air_client", "releases"))
                                .GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\" +
                            Path.Combine("deploy", "Adobe Air", "Versions", "1.0", "Adobe AIR.dll"), true);
                    }
                }
                if (Flash.IsChecked == true)
                {
                    if (Environment.Is64BitProcess)
                    {
                        File.Copy(
                            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
                                "Common Files", "Adobe AIR", "Versions", "1.0", "Resources", "NPSWF32.dll"),
                            Path.Combine("RADS", "projects", "lol_air_client", "releases") + @"\" +
                            new DirectoryInfo(Path.Combine("RADS", "projects", "lol_air_client", "releases"))
                                .GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\" +
                            Path.Combine("deploy", "Adobe Air", "Versions", "1.0", "Resources", "NPSWF32.dll"), true);
                    }
                    else
                    {
                        File.Copy(
                            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                                "Common Files", "Adobe AIR", "Versions", "1.0", "Resources", "NPSWF32.dll"),
                            Path.Combine("RADS", "projects", "lol_air_client", "releases") + @"\" +
                            new DirectoryInfo(Path.Combine("RADS", "projects", "lol_air_client", "releases"))
                                .GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\" +
                            Path.Combine("deploy", "Adobe Air", "Versions", "1.0", "Resources", "NPSWF32.dll"), true);
                    }
                }
            }
            if (!Directory.Exists("Game")) return;
            if (Tbb.IsChecked == true)
            {
                File.Copy("tbb.dll", Path.Combine("Game", "tbb.dll"), true);
            }
            if (AdobeAir.IsChecked == true)
            {
                if (Environment.Is64BitProcess)
                {
                    File.Copy(
                        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
                            "Common Files", "Adobe AIR", "Versions", "1.0", "Adobe AIR.dll"),
                        Path.Combine("Air", "Adobe Air", "Versions", "1.0", "Adobe AIR.dll"), true);
                }
                else
                {
                    File.Copy(
                        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                            "Common Files", "Adobe AIR", "Versions", "1.0", "Adobe AIR.dll"),
                        Path.Combine("Air", "Adobe Air", "Versions", "1.0", "Adobe AIR.dll"), true);
                }
            }
            if (Flash.IsChecked != true) return;
            if (Environment.Is64BitProcess)
            {
                File.Copy(
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
                        "Common Files", "Adobe AIR", "Versions", "1.0", "Resources", "NPSWF32.dll"),
                    Path.Combine("Air", "Adobe Air", "Versions", "1.0", "Resources", "NPSWF32.dll"), true);
            }
            else
            {
                File.Copy(
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                        "Common Files", "Adobe AIR", "Versions", "1.0", "Resources", "NPSWF32.dll"),
                    Path.Combine("Air", "Adobe Air", "Versions", "1.0", "Resources", "NPSWF32.dll"), true);
            }
        }

        private void HandleCgInstall()
        {
            if (Directory.Exists("RADS"))
            {
                if (Cg.IsChecked == true)
                {
                    RadSsln("Cg.dll");
                }
                if (Cg1.IsChecked == true)
                {
                    Radslaunch("Cg.dll");
                }

                if (CgGl.IsChecked == true)
                {
                    RadSsln("CgGL.dll");
                }
                if (CgGl1.IsChecked == true)
                {
                    Radslaunch("CgGL.dll");
                }

                if (CgD3D9.IsChecked == true)
                {
                    RadSsln("CgD3D9.dll");
                }
                if (CgD3D1.IsChecked == true)
                {
                    Radslaunch("CgD3D9.dll");
                }
            }
            else if (Directory.Exists("Game"))
            {
                if (Cg.IsChecked == true)
                {
                    Game("Cg.dll");
                }

                if (CgGl.IsChecked == true)
                {
                    Game("CgGL.dll");
                }

                if (CgD3D9.IsChecked == true)
                {
                    Game("CgD3D9.dll");
                }
            }
        }

        private static void Game(string file)
        {
            File.Copy(
                Path.Combine(Environment.GetEnvironmentVariable("CG_BIN_PATH", EnvironmentVariableTarget.User),
                    file), Path.Combine("Game", file), true);
        }

        private static void Radslaunch(string file)
        {
            File.Copy(
                Path.Combine(Environment.GetEnvironmentVariable("CG_BIN_PATH", EnvironmentVariableTarget.User),
                    file),
                Path.Combine("RADS", "projects", "lol_launcher", "releases") + @"\" +
                new DirectoryInfo(Path.Combine("RADS", "projects", "lol_launcher", "releases")).GetDirectories()
                    .OrderByDescending(d => d.CreationTime)
                    .FirstOrDefault() + @"\" + Path.Combine("deploy", file), true);
        }

        private static void RadSsln(string file)
        {
            File.Copy(
                Path.Combine(Environment.GetEnvironmentVariable("CG_BIN_PATH", EnvironmentVariableTarget.User),
                    file),
                Path.Combine("RADS", "solutions", "lol_game_client_sln", "releases") + @"\" +
                new DirectoryInfo(Path.Combine("RADS", "solutions", "lol_game_client_sln", "releases"))
                    .GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\" +
                Path.Combine("deploy", file), true);
        }

        private static void HandlePandoUninstall()
        {
            ProcessStartInfo pmb;
            Process process;
            if (Environment.Is64BitProcess)
            {
                if (!File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
                    "Pando Networks", "Media Booster", "uninst.exe"))) return;

                pmb = new ProcessStartInfo
                {
                    FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
                        "Pando Networks", "Media Booster", "uninst.exe"),
                    Arguments = "/silent"
                };
                process = new Process { StartInfo = pmb };
                process.Start();

                pmb.FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
                    "Pando Networks", "Media Booster", "uninst.exe");

            }
            else
            {
                if (!File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                    "Pando Networks", "Media Booster", "uninst.exe"))) return;

                pmb = new ProcessStartInfo
                {
                    FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                        "Pando Networks", "Media Booster", "uninst.exe"),
                    Arguments = "/silent"
                };
                process = new Process { StartInfo = pmb };
                process.Start();

                pmb.FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                    "Pando Networks", "Media Booster", "uninst.exe");

            }
            pmb.Arguments = "/silent";
            process.StartInfo = pmb;
            process.Start();
            process.WaitForExit();
        }

        private static void HandleUninstall()
        {
            if (Directory.Exists("RADS"))
            {
                if (File.Exists(Path.Combine("Backup", "game.cfg")))
                {
                    File.Copy(Path.Combine("Backup", "game.cfg"), Path.Combine("Config", "game.cfg"), true);
                }
                UninstallRads("Cg.dll");
                UninstallRads("CgGL.dll");
                UninstallRads("CgD3D9.dll");
                UninstallRads("tbb.dll");
                File.Copy(Path.Combine("Backup", "NPSWF32.dll"),
                    Path.Combine("RADS", "projects", "lol_air_client", "releases") + @"\" +
                    new DirectoryInfo(Path.Combine("RADS", "projects", "lol_air_client", "releases")).GetDirectories()
                        .OrderByDescending(d => d.CreationTime)
                        .FirstOrDefault() + @"\" +
                    Path.Combine("deploy", "Adobe Air", "Versions", "1.0", "Resources", "NPSWF32.dll"), true);
                File.Copy(Path.Combine("Backup", "Adobe AIR.dll"),
                    Path.Combine("RADS", "projects", "lol_air_client", "releases") + @"\" +
                    new DirectoryInfo(Path.Combine("RADS", "projects", "lol_air_client", "releases")).GetDirectories()
                        .OrderByDescending(d => d.CreationTime)
                        .FirstOrDefault() + @"\" +
                    Path.Combine("deploy", "Adobe Air", "Versions", "1.0", "Adobe AIR.dll"), true);
            }
            else if (Directory.Exists("Game"))
            {
                File.Copy(Path.Combine("Backup", "a.cfg"),
                    Path.Combine("Game", "DATA", "CFG", "defaults", "game.cfg"), true);
                File.Copy(Path.Combine("Backup", "GamePermanent.cfg"),
                    Path.Combine("Game", "DATA", "CFG", "defaults", "GamePermanent.cfg"), true);

                if (File.Exists(Path.Combine("Backup", "GamePermanent_zh_MY.cfg")))
                {
                    File.Copy(Path.Combine("Backup", "GamePermanent_zh_MY.cfg"),
                        Path.Combine("Game", "DATA", "CFG", "defaults", "GamePermanent_zh_MY.cfg"), true);
                }
                if (File.Exists(Path.Combine("Backup", "GamePermanent_en_SG.cfg")))
                {
                    File.Copy(Path.Combine("Backup", "GamePermanent_en_SG.cfg"),
                        Path.Combine("Game", "DATA", "CFG", "defaults", "GamePermanent_en_SG.cfg"), true);
                }
                UninstallGame("Cg.dll");
                UninstallGame("CgGL.dll");
                UninstallGame("CgD3D9.dll");
                UninstallGame("tbb.dll");
                File.Copy(Path.Combine("Backup", "NPSWF32.dll"),
                    Path.Combine("Air", "Adobe AIR", "Versions", "1.0", "Resources", "NPSWF32.dll"), true);
                File.Copy(Path.Combine("Backup", "Adobe AIR.dll"),
                    Path.Combine("Air", "Adobe AIR", "Versions", "1.0", "Adobe AIR.dll"), true);
            }
            Directory.Delete("Backup", true);
            if (
                 MessageBox.Show("It is recommended you do a restart after removing the patch", "LoLUpdater",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Process.Start("shutdown.exe", "-r -t 0");
            }
        }

        private static void UninstallGame(string file)
        {
            File.Copy(Path.Combine("Backup", file), Path.Combine("Game", file), true);
        }

        private static void UninstallRads(string file)
        {
            File.Copy(Path.Combine("Backup", file),
                Path.Combine("RADS", "solutions", "lol_game_client_sln", "releases") + @"\" +
                new DirectoryInfo(Path.Combine("RADS", "solutions", "lol_game_client_sln", "releases"))
                    .GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\" +
                Path.Combine("deploy", file), true);
        }

        private static void CgCheck()
        {
            ProcessStartInfo startInfo;
            Process cg;
            if (Environment.Is64BitProcess)
            {
                if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
                    "NVIDIA Corporation", "Cg", "Bin", "cg.dll"))) return;

                Process.Start("NvidiaCGLicence.txt");
                if (MessageBox.Show("By clicking Yes you agree to NvidiaCGs Licence", "LoLUpdater",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes) return;

                startInfo = new ProcessStartInfo { FileName = "Cg_3_1_April2012_Setup.exe", Arguments = "/silent" };
                cg = new Process { StartInfo = startInfo };
                cg.Start();
                cg.WaitForExit();


            }
            else
            {
                if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                    "NVIDIA Corporation", "Cg", "Bin", "cg.dll"))) return;
                Process.Start("NvidiaCGLicence.txt");
                if (MessageBox.Show("By clicking Yes you agree to NvidiaCGs Licence", "LoLUpdater",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes) return;

                startInfo = new ProcessStartInfo { FileName = "Cg_3_1_April2012_Setup.exe", Arguments = "/silent" };
                cg = new Process { StartInfo = startInfo };
                cg.Start();
                cg.WaitForExit();

            }
            Process.Start("NvidiaCGLicence.txt");
            if (MessageBox.Show("By clicking Yes you agree to NvidiaCGs Licence", "LoLUpdater",
                MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                return;
            cg = new Process();
            startInfo = new ProcessStartInfo {FileName = "Cg_3_1_April2012_Setup.exe", Arguments = "/silent"};
            cg.StartInfo = startInfo;
            cg.Start();
            cg.WaitForExit();
        }

        private static void AdobeAlert()
        {
            if (
                MessageBox.Show(
                    "We are unable to include any Adobe products, HOWEVER, you are fully capable of installing it yourself. Click yes to download and run the installer then apply the patch.",
                    "LoLUpdater", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Process.Start("http://airdownload.adobe.com/air/win/download/14.0/AdobeAIRInstaller.exe");
            }
        }

        private static void RunCleanManager()
        {
            var cm = new ProcessStartInfo { FileName = "cleanmgr.exe", Arguments = "sagerun:1" };
            var process = new Process { StartInfo = cm };
            process.Start();
            process.WaitForExit();
        }

   

        private static void HandleCfg(string setting)
        {
            if (File.Exists(Path.Combine("Config", "game.cfg")))
            {
                var fi = new FileInfo(Path.Combine("Config", "game.cfg"));

                if (FileAttributes.ReadOnly == fi.Attributes)
                {
                    MessageBox.Show(@"Your game.cfg Located in Config\ is read only, please remove this and try again",
                        "LoLUpdater");
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
                    File.AppendAllText(Path.Combine("Game", "DATA", "CFG", "defaults", "game.cfg"),
                        Environment.NewLine + setting);
                }
                FileInfo fi1;
                if (File.Exists(Path.Combine("Game", "DATA", "CFG", "defaults", "game.cfg")))
                {
                    var fi = new FileInfo(Path.Combine("Game", "DATA", "CFG", "defaults", "game.cfg"));

                    if (FileAttributes.ReadOnly == fi.Attributes)
                    {
                        MessageBox.Show(
                            @"Your game.cfg Located in Game\DATA\CFG\defaults is read only, please remove this and try again",
                            "LoLUpdater");
                        return;
                    }

                    if (File.Exists(Path.Combine("Game", "DATA", "CFG", "defaults", "GamePermanent.cfg")))
                    {
                        fi1 = new FileInfo(Path.Combine("Game", "DATA", "CFG", "defaults", "GamePermanent.cfg"));

                        if (FileAttributes.ReadOnly == fi1.Attributes)
                        {
                            MessageBox.Show(
                                @"Your GamePermanent.cfg Located in Game\DATA\CFG\defaults is read only, please remove this and try again",
                                "LoLUpdater");
                            return;
                        }

                        if (
                            !File.ReadAllText(Path.Combine("Game", "DATA", "CFG", "defaults", "GamePermanent.cfg"))
                                .Contains(setting))
                        {
                            File.AppendAllText(Path.Combine("Game", "DATA", "CFG", "defaults", "GamePermanent.cfg"),
                                Environment.NewLine + setting);
                        }
                    }
                }
                if (File.Exists(Path.Combine("Game", "DATA", "CFG", "defaults", "GamePermanent_zh_MY.cfg")))
                {
                    fi1 = new FileInfo(Path.Combine("Game", "DATA", "CFG", "defaults", "GamePermanent_zh_MY.cfg"));

                    if (FileAttributes.ReadOnly == fi1.Attributes)
                    {
                        MessageBox.Show(
                            @"Your GamePermanent_zh_MY.cfg Located in Game\DATA\CFG\defaults is read only, please remove this and try again",
                            "LoLUpdater");
                        return;
                    }

                    if (
                        !File.ReadAllText(Path.Combine("Game", "DATA", "CFG", "defaults", "GamePermanent_zh_MY.cfg"))
                            .Contains(setting))
                    {
                        File.AppendAllText(Path.Combine("Game", "DATA", "CFG", "defaults", "GamePermanent_zh_MY.cfg"),
                            Environment.NewLine + setting);
                    }
                }
                if (!File.Exists(Path.Combine("Game", "DATA", "CFG", "defaults", "GamePermanent_en_SG.cfg"))) return;
                fi1 = new FileInfo(Path.Combine("Game", "DATA", "CFG", "defaults", "GamePermanent_en_SG.cfg"));

                if (FileAttributes.ReadOnly == fi1.Attributes)
                {
                    MessageBox.Show(
                        @"Your GamePermanent_en_SG.cfg Located in Game\DATA\CFG\defaults is read only, please remove this and try again",
                        "LoLUpdater");
                    return;
                }

                if (
                    !File.ReadAllText(Path.Combine("Game", "DATA", "CFG", "defaults", "GamePermanent_en_SG.cfg"))
                        .Contains(setting))
                {
                    File.AppendAllText(Path.Combine("Game", "DATA", "CFG", "defaults", "GamePermanent_en_SG.cfg"),
                        Environment.NewLine + setting);
                }
            }
        }



        private void Cg_Checked(object sender, RoutedEventArgs e)
        {
            CgCheck();
        }

        private void Image_MouseEnter(object sender, MouseEventArgs e)
        {
            Xclose.Source = new BitmapImage(new Uri("Resources/closemouseenter.png", UriKind.Relative));
        }

        private void Image_MouseLeave(object sender, MouseEventArgs e)
        {
            Xclose.Source = new BitmapImage(new Uri("Resources/close.png", UriKind.Relative));
        }

        private void Image_MouseEnter_1(object sender, MouseEventArgs e)
        {
            Xminimize.Source = new BitmapImage(new Uri("Resources/minimizemouseneter.png", UriKind.Relative));
        }

        private void Image_MouseLeave_1(object sender, MouseEventArgs e)
        {
            Xminimize.Source = new BitmapImage(new Uri("Resources/minimize.png", UriKind.Relative));
        }

        private void Xclose_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Xminimize_MouseDown(object sender, MouseButtonEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
    }
}