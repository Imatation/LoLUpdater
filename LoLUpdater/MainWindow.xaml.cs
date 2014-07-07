using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Windows;
using WUApiLib;
namespace LoLUpdater
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Version win8version = new Version(6, 2, 9200, 0);

            if (System.Environment.OSVersion.Platform == PlatformID.Win32NT &&
                Environment.OSVersion.Version >= win8version)
            {
                MouseHz_.Visibility = Visibility.Visible;
            }

            if (!UacHelper.IsProcessElevated)
            {
                MouseHz_.IsEnabled = false;
                WinUpdate.IsEnabled = false;
                var result = MessageBox.Show("Certain features have been disabled. To enable all features, please run application " +
                    "as administrator. Restart as administrator? You can disable these prompts from within Application Options",
                    "LoLUpdater", MessageBoxButton.YesNo, MessageBoxImage.Information);

                if (result == MessageBoxResult.Yes)
                {
                    restartAsAdmin();
                }
            }
        }

        private void AdobeAIR_Checked(object sender, RoutedEventArgs e)
        {
            adobeAlert();
        }

        private void Flash_Checked(object sender, RoutedEventArgs e)
        {
            adobeAlert();
        }

        private void Deleteoldlogs_Checked(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("This deletes Riot logs older than 7 days", "LoLUpdater",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Todo: Make this prettier and add more servers
        private void ping_Click(object sender, RoutedEventArgs e)
        {
            Ping ping = new Ping();
            PingReply reply;

            if (NA.IsSelected)
            {
                reply = ping.Send("64.7.194.1");
                Label.Content = reply.RoundtripTime.ToString();
            }
            else if (EUW.IsSelected)
            {
                reply = ping.Send("190.93.245.13");
                Label.Content = reply.RoundtripTime.ToString();
            }
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            //Todo: add some sort of progress indication
            if (Visual.IsChecked == true)
            {
                Process.Start("SystemPropertiesPerformance.exe");
            }

            if (Clean.IsChecked == true)
            {
                runCleanManager();
            }

            if (MouseHz_.IsChecked == true)
            {
                handleMouseHz();
            }

            if (WinUpdate.IsChecked == true)
            {
                handleWindowsUpdate();
            }

            if (Riot_Logs.IsChecked == true)
            {
                handleRiotLogs();
            }

            if (!Directory.Exists("Backup"))
            {
                Directory.CreateDirectory("Backup");
                if (Directory.Exists("RADS"))
                {
                    if (File.Exists(Path.Combine("Config", "game.cfg")))
                    {
                        File.Copy(Path.Combine("Config", "game.cfg"), Path.Combine("Backup", "game.cfg"), true);
                    }
                    File.Copy(Path.Combine("RADS", "solutions", "lol_game_client_sln", "releases") + @"\" + new DirectoryInfo(Path.Combine("RADS", "solutions", "lol_game_client_sln", "releases")).GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\" + Path.Combine("deploy", "cg.dll"), Path.Combine("Backup", "cg.dll"), true);
                    File.Copy(Path.Combine("RADS", "solutions", "lol_game_client_sln", "releases") + @"\" + new DirectoryInfo(Path.Combine("RADS", "solutions", "lol_game_client_sln", "releases")).GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\" + Path.Combine("deploy", "cgD3D9.dll"), Path.Combine("Backup", "cgD3D9.dll"), true);
                    File.Copy(Path.Combine("RADS", "solutions", "lol_game_client_sln", "releases") + @"\" + new DirectoryInfo(Path.Combine("RADS", "solutions", "lol_game_client_sln", "releases")).GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\" + Path.Combine("deploy", "cgGL.dll"), Path.Combine("Backup", "cgGL.dll"), true);
                    File.Copy(Path.Combine("RADS", "solutions", "lol_game_client_sln", "releases") + @"\" + new DirectoryInfo(Path.Combine("RADS", "solutions", "lol_game_client_sln", "releases")).GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\" + Path.Combine("deploy", "tbb.dll"), Path.Combine("Backup", "tbb.dll"), true);
                    File.Copy(Path.Combine("RADS", "projects", "lol_air_client", "releases") + @"\" + new DirectoryInfo(Path.Combine("RADS", "projects", "lol_air_client", "releases")).GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\" + Path.Combine("deploy", "Adobe AIR", "Versions", "1.0", "Resources", "NPSWF32.dll"), Path.Combine("Backup", "NPSWF32.dll"), true);
                    File.Copy(Path.Combine("RADS", "projects", "lol_air_client", "releases") + @"\" + new DirectoryInfo(Path.Combine("RADS", "projects", "lol_air_client", "releases")).GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\" + Path.Combine("deploy", "Adobe AIR", "Versions", "1.0", "Adobe AIR.dll"), Path.Combine("Backup", "Adobe AIR.dll"), true);

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
                    File.Copy(Path.Combine("Game", "cg.dll"), Path.Combine("Backup", "cg.dll"), true);
                    File.Copy(Path.Combine("Game", "cgD3D9.dll"), Path.Combine("Backup", "cgD3D9.dll"), true);
                    File.Copy(Path.Combine("Game", "cgGL.dll"), Path.Combine("Backup", "cgGL.dll"), true);
                    File.Copy(Path.Combine("Game", "tbb.dll"), Path.Combine("Backup", "tbb.dll"), true);
                    File.Copy(Path.Combine("Air", "Adobe Air", "Versions", "1.0", "Resources", "NPSWF32.dll"), Path.Combine("Backup", "NPSWF32.dll"), true);
                    File.Copy(Path.Combine("Air", "Adobe Air", "Versions", "1.0", "Adobe AIR.dll"), Path.Combine("Backup", "Adobe AIR.dll"), true);
                }
            }

            if (Patch.IsChecked == true)
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
                if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Pando Networks", "Media Booster", "uninst.exe")))
                {
                    if (Environment.Is64BitProcess == true)
                    {
                        var PMB = new ProcessStartInfo();
                        var process = new Process();
                        PMB.FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Pando Networks", "Media Booster", "uninst.exe");
                        PMB.Arguments = "/silent";
                        process.StartInfo = PMB;
                        process.Start();
                    }
                    else
                    {
                        var PMB = new ProcessStartInfo();
                        var process = new Process();
                        PMB.FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Pando Networks", "Media Booster", "uninst.exe");
                        PMB.Arguments = "/silent";
                        process.StartInfo = PMB;
                        process.Start();
                    }
                }
                if (Directory.Exists("RADS"))
                {
                    var releasePath = Path.Combine("RADS", "projects", "lol_launcher", "releases");
                    var fullReleasePath = releasePath + @"\" + new DirectoryInfo(releasePath).GetDirectories().OrderByDescending(d => d.CreationTime)
                        .FirstOrDefault() + @"\";
                    var solutionPath = Path.Combine("RADS", "solutions", "lol_game_client_sln", "releases");
                    var fullSolutionPath = solutionPath + @"\" + new DirectoryInfo(solutionPath).GetDirectories().OrderByDescending(d => d.CreationTime)
                        .FirstOrDefault() + @"\";
                    var airClientPath = Path.Combine("RADS", "projects", "lol_air_client", "releases");
                    String programFiles;

                    if (Environment.Is64BitProcess == true)
                    {
                        programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
                    }
                    else
                    {
                        programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
                    }

                    if (Cg.IsChecked == true || CgGL.IsChecked == true || CgD3D9.IsChecked == true)
                    {
                        if (CGCheck())
                        {
                            var cgBinPath = Environment.GetEnvironmentVariable("CG_BIN_PATH", EnvironmentVariableTarget.User);
                            var cgPath = Path.Combine(cgBinPath, "cg.dll");
                            var cgGLPath = Path.Combine(cgBinPath, "cgGL.dll");
                            var CgD3D9Path = Path.Combine(cgBinPath, "cgD3D9.dll");

                            string cgDeploy = "";
                            string cgGLDeploy = "";
                            string cgD3D9Deploy = "";

                            if (Directory.Exists("RADS"))
                            {
                                cgDeploy = Path.Combine("deploy", "cg.dll");
                                cgGLDeploy = Path.Combine("deploy", "cgGL.dll");
                                cgD3D9Deploy = Path.Combine("deploy", "cgD3D9.dll");
                            }
                            else if (Directory.Exists("Game"))
                            {
                                cgDeploy = Path.Combine("Game", "cg.dll");
                                cgGLDeploy = Path.Combine("Game", "cgGL.dll");
                                cgD3D9Deploy = Path.Combine("Game", "cgD3D9.dll");
                            }

                            if (Cg.IsChecked == true)
                            {
                                File.Copy(cgPath, fullReleasePath + cgDeploy, true);
                                File.Copy(cgPath, fullSolutionPath + cgDeploy, true);
                            }

                            if (CgGL.IsChecked == true)
                            {
                                File.Copy(cgGLPath, fullReleasePath + cgGLDeploy, true);
                                File.Copy(cgGLPath, fullSolutionPath + cgGLDeploy, true);
                            }

                            if (CgD3D9.IsChecked == true)
                            {
                                File.Copy(CgD3D9Path, fullReleasePath + cgD3D9Deploy, true);
                                File.Copy(CgD3D9Path, fullSolutionPath + cgD3D9Deploy, true);
                            }
                        } 
                    }

                    if (tbb.IsChecked == true)
                    {
                        File.WriteAllBytes(fullSolutionPath + Path.Combine("deploy", "tbb.dll"), Properties.Resources.tbb);
                    }

                    if (AdobeAIR.IsChecked == true)
                    {
                        File.Copy(Path.Combine(programFiles, "Common Files", "Adobe AIR", "Versions", "1.0", "Adobe AIR.dll"),
                            airClientPath + @"\" + new DirectoryInfo(airClientPath).GetDirectories().OrderByDescending(d => d.CreationTime)
                            .FirstOrDefault() + @"\" + Path.Combine("deploy", "Adobe AIR", "Versions", "1.0", "Adobe AIR.dll"), true);
                    }

                    if (Flash.IsChecked == true)
                    {
                        File.Copy(Path.Combine(programFiles, "Common Files", "Adobe AIR", "Versions", "1.0", "Resources", "NPSWF32.dll"),
                            airClientPath + @"\" + new DirectoryInfo(airClientPath).GetDirectories().OrderByDescending(d => d.CreationTime)
                            .FirstOrDefault() + @"\" + Path.Combine("deploy", "Adobe AIR", "Versions", "1.0", "Resources", "NPSWF32.dll"), true);
                    }
                }
                else if (Directory.Exists("Game"))
                {

                    if (Cg.IsChecked == true)
                    {
                        CGCheck();
                        File.Copy(Path.Combine(Environment.GetEnvironmentVariable("CG_BIN_PATH"), "cg.dll"), Path.Combine("Game", "cg.dll"), true);
                    }
                    if (CgD3D9.IsChecked == true)
                    {
                        CGCheck();
                        File.Copy(Path.Combine(Environment.GetEnvironmentVariable("CG_BIN_PATH"), "cgD3D9.dll"), Path.Combine("Game", "cgD3D9.dll"), true);
                    }
                    if (CgGL.IsChecked == true)
                    {
                        CGCheck();
                        File.Copy(Path.Combine(Environment.GetEnvironmentVariable("CG_BIN_PATH"), "cgCgGL.dll"), Path.Combine("Game", "cgCgGL.dll"), true);
                    }
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
            else if (Remove.IsChecked == true)
            {
                if (Directory.Exists("RADS"))
                {
                    File.Copy(Path.Combine("Backup", "cg.dll"), Path.Combine("RADS", "projects", "lol_launcher", "releases") + @"\" + new DirectoryInfo(Path.Combine("RADS", "projects", "lol_launcher", "releases")).GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\" + Path.Combine("deploy", "cg.dll"), true);
                    File.Copy(Path.Combine("Backup", "cg.dll"), Path.Combine("RADS", "solutions", "lol_game_client_sln", "releases") + @"\" + new DirectoryInfo(Path.Combine("RADS", "solutions", "lol_game_client_sln", "releases")).GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\" + Path.Combine("deploy", "cg.dll"), true);
                    File.Copy(Path.Combine("Backup", "cgGL.dll"), Path.Combine("RADS", "projects", "lol_launcher", "releases") + @"\" + new DirectoryInfo(Path.Combine("RADS", "projects", "lol_launcher", "releases")).GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\" + Path.Combine("deploy", "cgGL.dll"), true);
                    File.Copy(Path.Combine("Backup", "cgGL.dll"), Path.Combine("RADS", "solutions", "lol_game_client_sln", "releases") + @"\" + new DirectoryInfo(Path.Combine("RADS", "solutions", "lol_game_client_sln", "releases")).GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\" + Path.Combine("deploy", "cgGL.dll"), true);
                    File.Copy(Path.Combine("Backup", "cgD3D9.dll"), Path.Combine("RADS", "projects", "lol_launcher", "releases") + @"\" + new DirectoryInfo(Path.Combine("RADS", "projects", "lol_launcher", "releases")).GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\" + Path.Combine("deploy", "cgD3D9.dll"), true);
                    File.Copy(Path.Combine("Backup", "cgD3D9.dll"), Path.Combine("RADS", "solutions", "lol_game_client_sln", "releases") + @"\" + new DirectoryInfo(Path.Combine("RADS", "solutions", "lol_game_client_sln", "releases")).GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\" + Path.Combine("deploy", "cgD3D9.dll"), true);
                    File.Copy(Path.Combine("Backup", "tbb.dll"), Path.Combine("RADS", "solutions", "lol_game_client_sln", "releases") + @"\" + new DirectoryInfo(Path.Combine("RADS", "solutions", "lol_game_client_sln", "releases")).GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\" + Path.Combine("deploy", "tbb.dll"), true);
                    File.Copy(Path.Combine("Backup", "NPSWF32.dll"), Path.Combine("RADS", "projects", "lol_air_client", "releases") + @"\" + new DirectoryInfo(Path.Combine("RADS", "projects", "lol_air_client", "releases")).GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\" + Path.Combine("deploy", "NPSWF32.dll"), true);
                    File.Copy(Path.Combine("Backup", "Adobe AIR.dll"), Path.Combine("RADS", "projects", "lol_air_client", "releases") + @"\" + new DirectoryInfo(Path.Combine("RADS", "projects", "lol_air_client", "releases")).GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\" + Path.Combine("deploy", "Adobe AIR.dll"), true);

                    if (File.Exists(Path.Combine("Backup", "game.cfg")))
                    {
                        File.Copy(Path.Combine("Backup", "game.cfg"), Path.Combine("Config", "game.cfg"), true);
                    }
                }
                else if (Directory.Exists("Game"))
                {
                    File.Copy(Path.Combine("Backup", "cg.dll"), Path.Combine("Game", "cg.dll"), true);
                    File.Copy(Path.Combine("Backup", "cgD3D9.dll"), Path.Combine("Game", "cgD3D9.dll"), true);
                    File.Copy(Path.Combine("Backup", "cgGL.dll"), Path.Combine("Game", "cgGL.dll"), true);
                    File.Copy(Path.Combine("Backup", "tbb.dll"), Path.Combine("Game", "tbb.dll"), true);
                    File.Copy(Path.Combine("Backup", "NPSWF32.dll"), Path.Combine("AIR", "Adobe Air", "Versions", "1.0", "Resources", "NPSWF32.dll"), true);
                    File.Copy(Path.Combine("Backup", "Adobe AIR.dll"), Path.Combine("AIR", "Adobe Air", "Versions", "1.0", "Adobe AIR.dll"), true);
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
                }

                Directory.Delete("Backup", true);
            }

            MessageBox.Show("Finished!", "LoLUpdater", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void restartAsAdmin()  // Closes the application and restarts as an administrator.
        {
            ProcessStartInfo proc = new ProcessStartInfo();
            proc.UseShellExecute = true;
            proc.WorkingDirectory = Environment.CurrentDirectory;
            proc.FileName = System.Reflection.Assembly.GetExecutingAssembly().Location;
            proc.Verb = "runas";

            try
            {
                Process.Start(proc);
            }
            catch
            {
                return;
            }
            Application.Current.Shutdown();
        }

        private static Boolean CGCheck()
        {
            if (Environment.Is64BitProcess == true)
            {
                string amd64Location = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),   // Store the path in a variable.
                    "NVIDIA Corporation", "Cg", "Bin", "cg.dll");                                                           // It creates cleaner code.

                if (File.Exists(amd64Location))
                {
                    return true;                                                                                                 // Return if found, we don't
                }                                                                                                           // need to go any further.
            }
            else
            {
                string x86Location = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                    "NVIDIA Corporation", "Cg", "Bin", "cg.dll");

                if (File.Exists(x86Location))
                {
                    return true;
                }
            }

            File.WriteAllBytes("Cg-3.1 April2012 Setup.exe", Properties.Resources.Cg_3_1_April2012_Setup);                  // Extract and write the file
            Process cg = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "Cg-3.1 April2012 Setup.exe";
            startInfo.Arguments = "/silent";
            cg.StartInfo = startInfo;
            cg.Start();
            cg.WaitForExit();
            File.Delete("Cg-3.1 April2012 Setup.exe");
            return false;
        }

        private void adobeAlert()
        {
            MessageBoxResult alertMessage = MessageBox.Show("We are unable to include any Adobe products, " +
                "HOWEVER, you are fully capable of installing it yourself. Click yes to download and run the" +
                " installer then apply the patch.", "LoLUpdater", MessageBoxButton.YesNo, MessageBoxImage.Question);

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
                mousehz = Registry.LocalMachine.CreateSubKey(Path.Combine("SOFTWARE", "Microsoft", "Windows NT",
                    "CurrentVersion", "AppCompatFlags", "Layers"));
            }
            else
            {
                mousehz = Registry.LocalMachine.CreateSubKey(Path.Combine("SOFTWARE", "WoW64Node", "Microsoft",
                    "Windows NT", "CurrentVersion", "AppCompatFlags", "Layers"));
            }

            mousehz.SetValue("NoDTToDITMouseBatch", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows),
                "explorer.exe"), RegistryValueKind.String);
            var cmd = new ProcessStartInfo();
            var process = new Process();
            cmd.FileName = "cmd.exe";
            cmd.Verb = "runas";
            cmd.Arguments = "/C Rundll32 apphelp.dll,ShimFlushCache";
            process.StartInfo = cmd;
            process.Start();
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

        private void handleRiotLogs()
        {
            if (Directory.Exists("RADS"))
            {
                string[] files = Directory.GetFiles("Logs");

                foreach (string file in files)
                {
                    FileInfo fi = new FileInfo(file);
                    if (fi.LastAccessTime < DateTime.Now.AddDays(-7))
                        fi.Delete();
                }
            }
        }

    }
}