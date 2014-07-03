using System;
using System.IO;
using System.Windows;
using System.Diagnostics;
using System.Linq;
namespace LoLUpdater
{
    public partial class MainWindow : Window
    {
        private void AdobeAIRBeta_Checked(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("We are unable to include the Adobe AIR Redistributable due to not having a licence, HOWEVER you are fully capable of installing it yourself. Click yes to download and run the installer then apply the patch.", "LoLUpdater", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Process.Start("http://labsdownload.adobe.com/pub/labs/flashruntimes/air/air14_win.exe");
            }
        }
        private void FlashBeta_Checked(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("We are unable to include the Adobe AIR Redistributable due to not having a licence, HOWEVER you are fully capable of installing it yourself. Click yes to download and run the installer then apply the patch.", "LoLUpdater", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Process.Start("http://labsdownload.adobe.com/pub/labs/flashruntimes/air/air14_win.exe");
            }
        }
        private void OK_Click(object sender, RoutedEventArgs e)
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
                    File.Copy(@"RADS\solutions\lol_game_client_sln\releases\" + new DirectoryInfo(Path.Combine("RADS", "solutions", "lol_game_client_sln", "releases")).GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\deploy\cg.dll", Path.Combine("Backup", "cg.dll"), true);
                    File.Copy(@"RADS\solutions\lol_game_client_sln\releases\" + new DirectoryInfo(Path.Combine("RADS", "solutions", "lol_game_client_sln", "releases")).GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\deploy\cgD3D9.dll", Path.Combine("Backup", "cgD3D9.dll"), true);
                    File.Copy(@"RADS\solutions\lol_game_client_sln\releases\" + new DirectoryInfo(Path.Combine("RADS", "solutions", "lol_game_client_sln", "releases")).GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\deploy\cgGL.dll", Path.Combine("Backup", "cgGL.dll"), true);
                    File.Copy(@"RADS\solutions\lol_game_client_sln\releases\" + new DirectoryInfo(Path.Combine("RADS", "solutions", "lol_game_client_sln", "releases")).GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\deploy\tbb.dll", Path.Combine("Backup", "tbb.dll"), true);
                    File.Copy(@"RADS\projects\lol_air_client\releases\" + new DirectoryInfo(Path.Combine("RADS", "projects", "lol_air_client", "releases")).GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\deploy\Adobe AIR\Versions\1.0\Resources\NPSWF32.dll", Path.Combine("Backup", "NPSWF32.dll"), true);
                    File.Copy(@"RADS\projects\lol_air_client\releases\" + new DirectoryInfo(Path.Combine("RADS", "projects", "lol_air_client", "releases")).GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\deploy\Adobe AIR\Versions\1.0\Adobe AIR.dll", Path.Combine("Backup", "Adobe AIR.dll"), true);

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
                    var PMB = new ProcessStartInfo();
                    var process = new Process();
                    PMB.FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Pando Networks", "Media Booster", "uninst.exe");
                    PMB.Arguments = "/silent";
                    process.StartInfo = PMB;
                    process.Start();
                }
                if (Directory.Exists("RADS"))
                {
                    if (Cg.IsChecked == true)
                    {
                        File.Copy(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "NVIDIA Corporation", "Cg", "bin", "cg.dll"), @"RADS\projects\lol_launcher\releases\" + new DirectoryInfo(Path.Combine("RADS", "projects", "lol_launcher", "releases")).GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\deploy\cg.dll", true);

                        File.Copy(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "NVIDIA Corporation", "Cg", "bin", "cg.dll"), @"RADS\solutions\lol_game_client_sln\releases\" + new DirectoryInfo(Path.Combine("RADS", "solutions", "lol_game_client_sln", "releases")).GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\deploy\cg.dll", true);
                    }
                    if (CgGL.IsChecked == true)
                    {
                        File.Copy(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "NVIDIA Corporation", "Cg", "bin", "cgGL.dll"), @"RADS\projects\lol_launcher\releases\" + new DirectoryInfo(Path.Combine("RADS", "projects", "lol_launcher", "releases")).GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\deploy\cgGL.dll", true);

                        File.Copy(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "NVIDIA Corporation", "Cg", "bin", "cgGL.dll"), @"RADS\solutions\lol_game_client_sln\releases\" + new DirectoryInfo(Path.Combine("RADS", "solutions", "lol_game_client_sln", "releases")).GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\deploy\cgGL.dll", true);
                    }
                    if (CgD3D9.IsChecked == true)
                    {
                        File.Copy(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "NVIDIA Corporation", "Cg", "bin", "cgD3D9.dll"), @"RADS\projects\lol_launcher\releases\" + new DirectoryInfo(Path.Combine("RADS", "projects", "lol_launcher", "releases")).GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\deploy\cgD3D9.dll", true);

                        File.Copy(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "NVIDIA Corporation", "Cg", "bin", "cgD3D9.dll"), @"RADS\solutions\lol_game_client_sln\releases\" + new DirectoryInfo(Path.Combine("RADS", "solutions", "lol_game_client_sln", "releases")).GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\deploy\cgD3D9.dll", true);
                    }
                    if (tbb.IsChecked == true)
                    {
                        File.WriteAllBytes(@"RADS\solutions\lol_game_client_sln\releases\" + new DirectoryInfo(Path.Combine("RADS", "solutions", "lol_game_client_sln", "releases")).GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\deploy\tbb.dll", Properties.Resources.tbb);
                    }
                    if (AdobeAIRBeta.IsChecked == true)
                    {
                        File.Copy(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Common Files", "Adobe AIR", "Versions", "1.0", "Adobe AIR.dll"), @"RADS\projects\lol_air_client\releases\" + new DirectoryInfo(Path.Combine("RADS", "projects", "lol_air_client", "releases")).GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\deploy\Adobe AIR\Versions\1.0\Adobe AIR.dll", true);
                    }
                    if (FlashBeta.IsChecked == true)
                    {
                        File.Copy(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Common Files", "Adobe AIR", "Versions", "1.0", "Resources", "NPSWF32.dll"), @"RADS\projects\lol_air_client\releases\" + new DirectoryInfo(Path.Combine("RADS", "projects", "lol_air_client", "releases")).GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\deploy\Adobe AIR\Versions\1.0\Resources\NPSWF32.dll", true);
                    }
                    if (AdobeAIR.IsChecked == true)
                    {
                        File.Copy(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Common Files", "Adobe AIR", "Versions", "1.0", "Adobe AIR.dll"), @"RADS\projects\lol_air_client\releases\" + new DirectoryInfo(Path.Combine("RADS", "projects", "lol_air_client", "releases")).GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\deploy\Adobe AIR\Versions\1.0\Adobe AIR.dll", true);
                    }
                    if (Flash.IsChecked == true)
                    {
                        File.Copy(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Common Files", "Adobe AIR", "Versions", "1.0", "Resources", "NPSWF32.dll"), @"RADS\projects\lol_air_client\releases\" + new DirectoryInfo(Path.Combine("RADS", "projects", "lol_air_client", "releases")).GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\deploy\Adobe AIR\Versions\1.0\Resources\NPSWF32.dll", true);
                    }
                }
                else if (Directory.Exists("Game"))
                {
                    if (Cg.IsChecked == true)
                    {
                        File.Copy(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "NVIDIA Corporation", "Cg", "bin", "cg.dll"), Path.Combine("Game", "cg.dll"), true);
                    }
                    if (CgD3D9.IsChecked == true)
                    {
                        File.Copy(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "NVIDIA Corporation", "Cg", "bin", "cgD3D9.dll"), Path.Combine("Game", "cgD3D9.dll"), true);
                    }
                    if (CgGL.IsChecked == true)
                    {
                        File.Copy(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "NVIDIA Corporation", "Cg", "bin", "cgGL.dll"), Path.Combine("Game", "cgGL.dll"), true);
                    }
                    if (tbb.IsChecked == true)
                    {
                        File.WriteAllBytes(Path.Combine("Game", "tbb.dll"), Properties.Resources.tbb);
                    }
                    if (AdobeAIRBeta.IsChecked == true)
                    {
                        File.Copy(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Common Files", "Adobe AIR", "Versions", "1.0", "Adobe AIR.dll"), Path.Combine("Air", "Adobe Air", "Versions", "1.0", "Adobe AIR.dll"), true);
                    }
                    if (FlashBeta.IsChecked == true)
                    {
                        File.Copy(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Common Files", "Adobe AIR", "Versions", "1.0", "Resources", "NPSWF32.dll"), Path.Combine("Air", "Adobe Air", "Versions", "1.0", "Resources", "NPSWF32.dll"), true);
                    }
                    if (AdobeAIR.IsChecked == true)
                    {
                        File.Copy(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Common Files", "Adobe AIR", "Versions", "1.0", "Adobe AIR.dll"), Path.Combine("Air", "Adobe Air", "Versions", "1.0", "Adobe AIR.dll"), true);
                    }
                    if (Flash.IsChecked == true)
                    {
                        File.Copy(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Common Files", "Adobe AIR", "Versions", "1.0", "Resources", "NPSWF32.dll"), Path.Combine("Air", "Adobe Air", "Versions", "1.0", "Resources", "NPSWF32.dll"), true);
                    }
                }
            }
            else if (Remove.IsChecked == true)
            {
                if (Directory.Exists("Rads"))
                {
                    File.Copy(Path.Combine("Backup", "cg.dll"), @"RADS\solutions\lol_game_client_sln\releases\" + new DirectoryInfo(Path.Combine("RADS", "solutions", "lol_game_client_sln", "releases")).GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\deploy\cg.dll", true);
                    File.Copy(Path.Combine("Backup", "cgD3D9.dll"), @"RADS\solutions\lol_game_client_sln\releases\" + new DirectoryInfo(Path.Combine("RADS", "solutions", "lol_game_client_sln", "releases")).GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\deploy\cgD3D9.dll", true);
                    File.Copy(Path.Combine("Backup", "cgGL.dll"), @"RADS\solutions\lol_game_client_sln\releases\" + new DirectoryInfo(Path.Combine("RADS", "solutions", "lol_game_client_sln", "releases")).GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\deploy\cgGL.dll", true);
                    File.Copy(Path.Combine("Backup", "tbb.dll"), @"RADS\solutions\lol_game_client_sln\releases\" + new DirectoryInfo(Path.Combine("RADS", "solutions", "lol_game_client_sln", "releases")).GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\deploy\tbb.dll", true);
                    File.Copy(Path.Combine("Backup", "cg.dll"), @"RADS\projects\lol_launcher\releases\" + new DirectoryInfo(Path.Combine("RADS", "projects", "lol_launcher", "releases")).GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\deploy\cg.dll", true);
                    File.Copy(Path.Combine("Backup", "cgD3D9.dll"), @"RADS\projects\lol_launcher\releases\" + new DirectoryInfo(Path.Combine("RADS", "projects", "lol_launcher", "releases")).GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\deploy\cgD3D9.dll", true);
                    File.Copy(Path.Combine("Backup", "cgGL.dll"), @"RADS\projects\lol_launcher\releases\" + new DirectoryInfo(Path.Combine("RADS", "projects", "lol_launcher", "releases")).GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\deploy\cgGL.dll", true);
                    File.Copy(Path.Combine("Backup", "NPSWF32.dll"), @"RADS\projects\lol_air_client\releases\" + new DirectoryInfo(Path.Combine("RADS", "projects", "lol_air_client", "releases")).GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\deploy\Adobe AIR\Versions\1.0\Resources\NPSWF32.dll", true);
                    File.Copy(Path.Combine("Backup", "Adobe AIR.dll"), @"RADS\projects\lol_air_client\releases\" + new DirectoryInfo(Path.Combine("RADS", "projects", "lol_air_client", "releases")).GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\deploy\Adobe AIR\Versions\1.0\Adobe AIR.dll", true);
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
            }
            MessageBox.Show("Finished!", "LoLUpdater");
        }
        private void CgD3D9_Checked(object sender, RoutedEventArgs e)
        {
            CGCheck();
        }
        private void CgGL_Checked(object sender, RoutedEventArgs e)
        {
            CGCheck();
        }
        private void Cg_Checked(object sender, RoutedEventArgs e)
        {
            CGCheck();
        }
        private static void CGCheck()
        {
            if (!Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "NVIDIA Corporation", "Cg", "bin")))
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
        private void Flash_Checked(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("We are unable to include the Adobe AIR Redistributable due to not having a licence, HOWEVER you are fully capable of installing it yourself. Click yes to download and run the installer then apply the patch.", "LoLUpdater", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Process.Start("http://airdownload.adobe.com/air/win/download/14.0/AdobeAIRInstaller.exe");
            }
        }
        private void AdobeAIR_Checked(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("We are unable to include the Adobe AIR Redistributable due to not having a licence, HOWEVER you are fully capable of installing it yourself. Click yes to download and run the installer then apply the patch.", "LoLUpdater", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Process.Start("http://airdownload.adobe.com/air/win/download/14.0/AdobeAIRInstaller.exe");
            }
        }
        private void Visual_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("SystemPropertiesPerformance.exe");
        }
        private void Clean_Click(object sender, RoutedEventArgs e)
        {
            var cm = new ProcessStartInfo();
            var process = new Process();
            cm.FileName = "cleanmgr.exe";
            cm.Arguments = "sagerun:1";
            process.StartInfo = cm;
            process.Start();
        }
    }
}