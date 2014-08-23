using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using WUApiLib;

namespace LoLUpdater
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
            if (Visual.IsChecked == true)
            {
                Process.Start("SystemPropertiesPerformance.exe");
            }
            if (WinUpdate.IsChecked == true)
            {
                HandleWindowsUpdate();
            }
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
            HandleMouseHz();
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
            Reboot("Installing");
        }

        private static void Reboot(string message)
        {
            if (
                MessageBox.Show(String.Format("It is recommended you do a restart after {0} the patch", message), "LoLUpdater",
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
                GameAirBackup("Adobe AIR.dll", string.Empty);
                GameAirBackup("NPSWF32.dll", "Resources");
            }
        }

        private static void GameAirBackup(string file, string extension)
        {
            File.Copy(Path.Combine("Air", "Adobe AIR", "Versions", "1.0", extension, file),
                Path.Combine("Backup", file), true);
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
                    RadsAir(
                        Environment.Is64BitProcess
                            ? Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86)
                            : Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), string.Empty,
                        "Adobe AIR.dll");
                }
                if (Flash.IsChecked == true)
                {

                    RadsAir(
    Environment.Is64BitProcess
        ? Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86)
        : Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Resources",
    "NPSWF32.dll");

                }
            }
            if (!Directory.Exists("Game")) return;
            if (Tbb.IsChecked == true)
            {
                File.Copy("tbb.dll", Path.Combine("Game", "tbb.dll"), true);
            }
            if (AdobeAir.IsChecked == true)
            {
                GameAir(
Environment.Is64BitProcess
    ? Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86)
    : Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), string.Empty,
"Adobe AIR.dll");
            }
            if (Flash.IsChecked != true) return;

            GameAir(
Environment.Is64BitProcess
? Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86)
: Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Resources",
"NPSWF32.dll");

        }

        private static void RadsAir(string arch, string extension, string file)
        {
            File.Copy(
                Path.Combine(arch,
                    "Common Files", "Adobe AIR", "Versions", "1.0", "Adobe AIR.dll"),
                Path.Combine("RADS", "projects", "lol_air_client", "releases") + @"\" +
                new DirectoryInfo(Path.Combine("RADS", "projects", "lol_air_client", "releases"))
                    .GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\" +
                Path.Combine("deploy", "Adobe Air", "Versions", "1.0", extension, file), true);
        }

        private static void GameAir(string arch, string extension, string file)
        {
            File.Copy(
                Path.Combine(arch,
                    "Common Files", "Adobe AIR", "Versions", "1.0", "Resources", "NPSWF32.dll"),
                Path.Combine("Air", "Adobe Air", "Versions", "1.0", extension, file), true);
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

            Process process;
            if (Environment.Is64BitProcess)
            {
                if (Pmb(out process, Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86))) return;
            }
            else
            {
                if (Pmb(out process, Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles))) return;


            }
            process.Start();
            process.WaitForExit();
        }

        private static bool Pmb(out Process process, string arch)
        {
            if (!File.Exists(Path.Combine(arch,
                "Pando Networks", "Media Booster", "uninst.exe")))
            {
                process = null;
                return true;
            }

            var pmb = new ProcessStartInfo
            {
                FileName = Path.Combine(arch,
                    "Pando Networks", "Media Booster", "uninst.exe"),
                Arguments = "/silent"
            };
            process = new Process { StartInfo = pmb };
            return false;
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
                UninstallAir("Resources", "NPSWF32.dll");
                UninstallAir(string.Empty, "Adobe AIR.dll");
            }
            else if (Directory.Exists("Game"))
            {
                Gameconfig("game.cfg");
                Gameconfig("GamePermanent.cfg");

                if (File.Exists(Path.Combine("Backup", "GamePermanent_zh_MY.cfg")))
                {
                    Gameconfig("GamePermanent_zh_MY.cfg");
                }
                if (File.Exists(Path.Combine("Backup", "GamePermanent_en_SG.cfg")))
                {
                    Gameconfig("GamePermanent_en_SG.cfg");
                }
                UninstallGame("Cg.dll");
                UninstallGame("CgGL.dll");
                UninstallGame("CgD3D9.dll");
                UninstallGame("tbb.dll");
                UninstallGameAir("Resources", "NPSWF32.dll");
                UninstallGameAir(string.Empty, "Adobe AIR.dll");
            }
            Directory.Delete("Backup", true);
            Reboot("Removing");
        }

        private static void UninstallGameAir(string extension, string file)
        {
            File.Copy(Path.Combine("Backup", "NPSWF32.dll"),
                Path.Combine("Air", "Adobe AIR", "Versions", "1.0", extension, file), true);
        }

        private static void Gameconfig(string file)
        {
            File.Copy(Path.Combine("Backup", "game.cfg"),
                Path.Combine("Game", "DATA", "CFG", "defaults", file), true);
        }

        private static void UninstallAir(string extension, string file)
        {
            File.Copy(Path.Combine("Backup", "NPSWF32.dll"),
                Path.Combine("RADS", "projects", "lol_air_client", "releases") + @"\" +
                new DirectoryInfo(Path.Combine("RADS", "projects", "lol_air_client", "releases")).GetDirectories()
                    .OrderByDescending(d => d.CreationTime)
                    .FirstOrDefault() + @"\" +
                Path.Combine("deploy", "Adobe Air", "Versions", "1.0", extension, file), true);
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



        private static void CgFix(string arch)
        {
            if (File.Exists(Path.Combine(arch,
                "NVIDIA Corporation", "Cg", "Bin", "cg.dll")))
            {
                return;
            }

            Process.Start("NvidiaCGLicence.txt");
            if (MessageBox.Show("By clicking Yes you agree to NvidiaCGs Licence", "LoLUpdater",
                MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
            {
                return;
            }

            var startInfo = new ProcessStartInfo { FileName = "Cg_3_1_April2012_Setup.exe", Arguments = "/silent" };
            var cg = new Process { StartInfo = startInfo };
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

        private static void HandleMouseHz()
        {
            var win8Version = new Version(6, 2, 9200, 0);
            if (Environment.OSVersion.Platform != PlatformID.Win32NT || Environment.OSVersion.Version < win8Version)
                return;
            RegistryKey mousehz;

            if (Environment.Is64BitProcess)
            {
                mousehz =
                    Registry.LocalMachine.CreateSubKey(Path.Combine("SOFTWARE", "Microsoft", "Windows NT",
                        "CurrentVersion", "AppCompatFlags", "Layers"));
            }
            else
            {
                mousehz =
                    Registry.LocalMachine.CreateSubKey(Path.Combine("SOFTWARE", "WoW64Node", "Microsoft",
                        "Windows NT", "CurrentVersion", "AppCompatFlags", "Layers"));
            }
            if (mousehz != null)
                mousehz.SetValue("NoDTToDITMouseBatch",
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "explorer.exe"),
                    RegistryValueKind.String);
            var cmd = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Verb = "runas",
                Arguments = "/C Rundll32 apphelp.dll,ShimFlushCache"
            };
            var process = new Process { StartInfo = cmd };
            process.Start();
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
                Cfg(setting, "game.cfg");
                Cfg(setting, "GamePermanent.cfg");
                Cfg(setting, "GamePermanent_zh_MY.cfg");
                Cfg(setting, "GamePermanent_en_SG.cfg");
            }
        }

        private static void Cfg(string setting, string file)
        {

            if (!File.Exists(Path.Combine("Game", "DATA", "CFG", "defaults", file))) ;
            {
                var fi = new FileInfo(Path.Combine("Game", "DATA", "CFG", "defaults", file));
                if (FileAttributes.ReadOnly == fi.Attributes)
                {
                    MessageBox.Show(
                        "Your " + file + @" Located in Game\DATA\CFG\defaults is read only, please remove this and try again",
                        "LoLUpdater");
                    return;
                }
                if (
    !File.ReadAllText(Path.Combine("Game", "DATA", "CFG", "defaults", file))
        .Contains(setting))
                {
                    File.AppendAllText(Path.Combine("Game", "DATA", "CFG", "defaults", file),
                        Environment.NewLine + setting);
                }
            }

        }



        private static void HandleWindowsUpdate()
        {
            var uSession = new UpdateSession();
            var uSearcher = uSession.CreateUpdateSearcher();
            var uResult = uSearcher.Search("IsInstalled=0 and BrowseOnly=0 and Type='Software'");
            var downloader = uSession.CreateUpdateDownloader();
            downloader.Updates = uResult.Updates;
            downloader.Download();
            var updatesToInstall = new UpdateCollection();
            foreach (var update in uResult.Updates.Cast<IUpdate>().Where(update => update.IsDownloaded))
            {
                updatesToInstall.Add(update);
            }
            var installer = uSession.CreateUpdateInstaller();
            installer.Updates = updatesToInstall;
            installer.Install();
        }

        private void Cg_Checked(object sender, RoutedEventArgs e)
        {
            if (Environment.Is64BitProcess)
            {
                CgFix(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86));
            }
            else
            {
                CgFix(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles));
            }
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