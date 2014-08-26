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
        private static readonly string Arch = Environment.Is64BitProcess
            ? Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86)
            : Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);

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
            HandleCgInstall();
            HandleAdobeAndTbb();
            RunCleanManager();
            HandlePmbUninstall();
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
                MessageBox.Show(String.Format("It is recommended you do a restart after {0} the patch", message),
                    "LoLUpdater",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Process.Start("shutdown.exe", "-r -t 0");
            }
        }

        private static void HandleBackup()
        {
            if (Directory.Exists("Backup")) return;
            Directory.CreateDirectory("Backup");
            if (Directory.Exists("RADS"))
            {
                if (File.Exists(Path.Combine("Config", "game.cfg")))
                {
                    Copy("game.cfg", "Config", "Backup");
                }
                Backup("solutions", "lol_game_client_sln", "Cg.dll", string.Empty);
                Backup("solutions", "lol_game_client_sln", "CgD3D9.dll", string.Empty);
                Backup("solutions", "lol_game_client_sln", "CgGL.dll", string.Empty);
                Backup("solutions", "lol_game_client_sln", "tbb.dll", string.Empty);
                Backup("projects", "lol_air_client", "Adobe AIR.dll", Path.Combine("Adobe Air", "Versions", "1.0"));
                Backup("projects", "lol_air_client", "NPSWF32.dll",
                    Path.Combine("Adobe Air", "Versions", "1.0", "Resources"));
            }
            else if (Directory.Exists("Game"))
            {
                if (File.Exists(Path.Combine("Game", "DATA", "CFG", "defaults", "game.cfg")))
                {
                    Copy("game.cfg", Path.Combine("Game", "DATA", "CFG", "defaults"), "Backup");
                    Copy("GamePermanent.cfg", Path.Combine("Game", "DATA", "CFG", "defaults"), "Backup");
                    if (File.Exists(Path.Combine("Game", "DATA", "CFG", "defaults", "GamePermanent_zh_MY.cfg")))
                    {
                        Copy("GamePermanent_zh_MY.cfg", Path.Combine("Game", "DATA", "CFG", "defaults"), "Backup");
                        Copy("game.cfg", Path.Combine("Game", "DATA", "CFG", "defaults"), "Backup");
                    }
                    if (File.Exists(Path.Combine("Game", "DATA", "CFG", "defaults", "GamePermanent_en_SG.cfg")))
                    {
                        Copy("GamePermanent_en_SG.cfg", Path.Combine("Game", "DATA", "CFG", "defaults"), "Backup");
                    }
                }
                Copy("Cg.dll", "Game", "Backup");
                Copy("CgGL.dll", "Game", "Backup");
                Copy("CgD3D9.dll", "Game", "Backup");
                Copy("tbb.dll", "Game", "Backup");
                Copy("Adobe AIR.dll", Path.Combine("Air", "Adobe AIR", "Versions", "1.0"), "Backup");
                Copy("NPSWF32.dll", Path.Combine("Air", "Adobe AIR", "Versions", "1.0", "Resources"), "Backup");
            }
        }

        private static void Backup(string folder, string folder1, string file, string extension)
        {
            File.Copy(Path.Combine(Path.Combine("RADS", folder, folder1, "releases") + @"\" +
                                   new DirectoryInfo(Path.Combine("RADS", folder, folder1, "releases"))
                                       .GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\",
                Path.Combine("deploy", extension, file)), Path.Combine("Backup", file),
                true);
        }


        private static void Copy(string file, string from, string to)
        {
            File.Copy(Path.Combine(from, file),
                Path.Combine(to, file), true);
        }

        private void HandleAdobeAndTbb()
        {
            var airPath = Path.Combine(Arch, "Common Files", "Adobe AIR", "Versions", "1.0");
            var flashPath = Path.Combine(airPath, "Resources");

            if (Directory.Exists("RADS"))
            {
                if (Tbb.IsChecked == true)
                {
                    AdvancedCopy("tbb.dll", string.Empty, "solutions", "lol_game_client_sln", "deploy");
                }

                if (AdobeAir.IsChecked == true)
                {
                    AdvancedCopy(
                        "Adobe AIR.dll",
                        airPath,
                        "projects",
                        "lol_air_client",
                        Path.Combine("deploy", "Adobe Air", "Versions", "1.0"));
                }
                if (Flash.IsChecked == true)
                {
                    AdvancedCopy(
                        "NPSWF32.dll",
                        flashPath,
                        "projects",
                        "lol_air_client",
                        Path.Combine("deploy", "Adobe Air", "Versions", "1.0", "Resources"));
                }
            }
            if (!Directory.Exists("Game")) return;
            if (Tbb.IsChecked == true)
            {
                Copy("tbb.dll",
                    string.Empty,
                    Path.Combine("Game", "tbb.dll"));
            }
            if (AdobeAir.IsChecked == true)
            {
                Copy(
                    "Adobe AIR.dll",
                    airPath,
                    Path.Combine("Air", "Adobe Air", "Versions", "1.0"));
            }
            if (Flash.IsChecked == true)
            {
                Copy(
                    "NPSWF32.dll",
                    flashPath,
                    Path.Combine("Air", "Adobe Air", "Versions", "1.0", "Resources"));
            }
        }


        private static void AdvancedCopy(string file, string from, string folder, string folder1, string to)
        {
            File.Copy(
                Path.Combine(
                    from, file),
                Path.Combine("RADS", folder, folder1, "releases") + @"\" +
                new DirectoryInfo(Path.Combine("RADS", folder, folder1, "releases"))
                    .GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\" +
                Path.Combine(to, file), true);
        }


        private void HandleCgInstall()
        {
            var cgBinPath = Environment.GetEnvironmentVariable("CG_BIN_PATH", EnvironmentVariableTarget.User);
            if (Directory.Exists("RADS"))
            {
                if (Cg.IsChecked == true)
                {
                    AdvancedCopy(
                        "Cg.dll", cgBinPath,
                        "solutions", "lol_game_client_sln", "deploy");
                }
                if (Cg1.IsChecked == true)
                {
                    AdvancedCopy(
                        "Cg.dll", cgBinPath,
                        "projects", "lol_launcher", "deploy");
                }

                if (CgGl.IsChecked == true)
                {
                    AdvancedCopy(
                        "Cg.dll", cgBinPath,
                        "solutions", "lol_game_client_sln", "deploy");
                }
                if (CgGl1.IsChecked == true)
                {
                    AdvancedCopy(
                        "CgGL.dll", cgBinPath,
                        "projects", "lol_launcher", "deploy");
                }

                if (CgD3D9.IsChecked == true)
                {
                    AdvancedCopy(
                        "CgD3D9.dll", cgBinPath,
                        "solutions", "lol_game_client_sln", "deploy");
                }
                if (CgD3D1.IsChecked == true)
                {
                    AdvancedCopy(
                        "CgD3D9.dll", cgBinPath,
                        "projects", "lol_launcher", "deploy");
                }
            }
            else if (Directory.Exists("Game"))
            {
                if (Cg.IsChecked == true)
                {
                    Copy("Cg.dll",
                        cgBinPath,
                        "Game");
                }

                if (CgGl.IsChecked == true)
                {
                    Copy("CgGL.dll",
                        cgBinPath,
                        "Game");
                }

                if (CgD3D9.IsChecked == true)
                {
                    Copy("CgD3D9.dll",
                        cgBinPath,
                        "Game");
                }
            }
        }


        private static void HandlePmbUninstall()
        {
            var pmbUninstall = Path.Combine(Arch,
                "Pando Networks", "Media Booster", "uninst.exe");
            if (!File.Exists(pmbUninstall)) return;
            var pmb = new ProcessStartInfo
            {
                FileName = pmbUninstall,
                Arguments = "/silent"
            };
            var process = new Process {StartInfo = pmb};
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
                Uninstall("Cg.dll", "deploy", "solutions", "lol_game_client_sln");
                Uninstall("CgGL.dll", "deploy", "solutions", "lol_game_client_sln");
                Uninstall("CgD3D9.dll", "deploy", "solutions", "lol_game_client_sln");

                Uninstall("Cg.dll", "deploy", "projects", "lol_launcher");
                Uninstall("CgGL.dll", "deploy", "projects", "lol_launcher");
                Uninstall("CgD3D9.dll", "deploy", "projects", "lol_launcher");

                Uninstall("tbb.dll", "deploy", "solutions", "lol_game_client_sln");

                Uninstall("Adobe AIR.dll", Path.Combine("deploy", "Adobe Air", "Versions", "1.0"),
                    "projects", "lol_air_client");
                Uninstall("NPSWF32.dll", Path.Combine("deploy", "Adobe Air", "Versions", "1.0", "Resources"),
                    "projects", "lol_air_client");
            }
            else if (Directory.Exists("Game"))
            {
                Copy("game.cfg", "Backup", Path.Combine("Game", "DATA", "CFG", "defaults"));
                Copy("GamePermanent.cfg", "Backup", Path.Combine("Game", "DATA", "CFG", "defaults"));

                if (File.Exists(Path.Combine("Backup", "GamePermanent_zh_MY.cfg")))
                {
                    Copy("GamePermanent_zh_MY.cfg", "Backup", Path.Combine("Game", "DATA", "CFG", "defaults"));
                }
                if (File.Exists(Path.Combine("Backup", "GamePermanent_en_SG.cfg")))
                {
                    Copy("GamePermanent_en_SG.cfg", "Backup", Path.Combine("Game", "DATA", "CFG", "defaults"));
                }
                Copy("Cg.dll", "Backup", "Game");
                Copy("CgGL.dll", "Backup", "Game");
                Copy("CgD3D9.dll", "Backup", "Game");
                Copy("tbb.dll", "Backup", "Game");
                Copy("Adobe AIR.dll", "Backup", Path.Combine("Air", "Adobe AIR", "Versions", "1.0"));
                Copy("NPSWF32.dll", "Backup", Path.Combine("Air", "Adobe AIR", "Versions", "1.0", "Resources"));
            }
            Directory.Delete("Backup", true);
            Reboot("Removing");
        }


        private static void Uninstall(string file, string extension, string folder, string folder1)
        {
            File.Copy(Path.Combine("Backup", file),
                Path.Combine("RADS", folder, folder1, "releases") + @"\" +
                new DirectoryInfo(Path.Combine("RADS", folder, folder1, "releases"))
                    .GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\" +
                Path.Combine(extension, file), true);
        }


        private static void AdobeAlert()
        {
            string airPath = Path.Combine(Arch, "Common Files", "Adobe AIR", "Versions", "1.0");

            if (
                !File.Exists(Path.Combine(airPath, "Adobe Air.dll")) &&
                MessageBox.Show(
                    "We are unable to include any Adobe products, HOWEVER, you are fully capable of installing it yourself. Click yes to download and run the installer then apply the patch.",
                    "LoLUpdater", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Process.Start("http://airdownload.adobe.com/air/win/download/14.0/AdobeAIRInstaller.exe");
            }
            else
            {
                var fileVersion = FileVersionInfo.GetVersionInfo(Path.Combine(airPath, "Adobe Air.dll")).FileVersion.Split(".".ToCharArray());
                float versionNumber = float.Parse(String.Format("{0}.{1}", fileVersion[0], fileVersion[1]), System.Globalization.CultureInfo.InvariantCulture);
                if (
                    versionNumber < 14.0 &&
                    MessageBox.Show(
                        "The Adobe Air version which is installed on your computer is outdated. Do you want to update it to ensure greater performance gains in the LoL client?",
                        "LoLUpdater", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    // Don't know how to do this silently, doesn't work if air hasn't checked for updates (though it prompts after a short time)
                    var airUpdate = new ProcessStartInfo
                    {
                        FileName = Path.Combine(airPath, "Resources", "Adobe AIR Updater.exe"),
                    };
                    var process = new Process { StartInfo = airUpdate };
                    process.Start();
                    process.WaitForExit();
                }
            }
        }

        private static void RunCleanManager()
        {
            var cm = new ProcessStartInfo { Arguments = "sagerun:1", FileName = "cleanmgr.exe" };
            var process = new Process {StartInfo = cm};
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
                    MessageBox.Show(String.Format(
                        @"Your {0} Located in Game\DATA\CFG\defaults is read only, please remove this and try again",
                        file),
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


        private void Cg_Checked(object sender, RoutedEventArgs e)
        {
            var cgPath = Path.Combine(Arch, "NVIDIA Corporation", "Cg", "Bin", "cg.dll");
            if (File.Exists(cgPath))
            {
                var fileRecent = FileVersionInfo.GetVersionInfo(cgPath).FileVersion == "3.1.0013";
                if (fileRecent ||
                    !fileRecent &&
                    MessageBox.Show("You already have Nvdia CG installed. Do you want to update it?", "LoLUpdater",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                {
                    return;
                }
            }

            Process.Start("NvidiaCGLicence.txt");
            if (MessageBox.Show("By clicking Yes you agree to NvidiaCGs Licence", "LoLUpdater",
                MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
            {
                return;
            }

            var startInfo = new ProcessStartInfo {FileName = "Cg_3_1_April2012_Setup.exe", Arguments = "/silent"};
            var cg = new Process {StartInfo = startInfo};
            cg.Start();
            cg.WaitForExit();
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