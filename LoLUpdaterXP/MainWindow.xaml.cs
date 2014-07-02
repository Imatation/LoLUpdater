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
            if (MessageBox.Show("We are unable to include the Adobe AIR Redistributable due to not having a licence BUT you are fully capable of installing it yourself. Click yes to download and run the installer before applying the patch.", "LoLUpdater", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Process.Start("http://labsdownload.adobe.com/pub/labs/flashruntimes/air/air14_win.exe");
            }
        }
        private void FlashBeta_Checked(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("We are unable to include the Adobe AIR Redistributable due to not having a licence BUT you are fully capable of installing it yourself. Click yes to download and run the installer before applying the patch.", "LoLUpdater", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
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
                    if (File.Exists(@"Config\game.cfg"))
                    {
                        File.Copy(@"Config\game.cfg", @"Backup\game.cfg", true);
                    }
                    File.Copy(@"RADS\solutions\lol_game_client_sln\releases\" + new DirectoryInfo(@"RADS\solutions\lol_game_client_sln\releases").GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\deploy\cg.dll", @"Backup\cg.dll", true);
                    File.Copy(@"RADS\solutions\lol_game_client_sln\releases\" + new DirectoryInfo(@"RADS\solutions\lol_game_client_sln\releases").GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\deploy\cgD3D9.dll", @"Backup\cgD3D9.dll", true);
                    File.Copy(@"RADS\solutions\lol_game_client_sln\releases\" + new DirectoryInfo(@"RADS\solutions\lol_game_client_sln\releases").GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\deploy\cgGL.dll", @"Backup\cgGL.dll", true);
                    File.Copy(@"RADS\solutions\lol_game_client_sln\releases\" + new DirectoryInfo(@"RADS\solutions\lol_game_client_sln\releases").GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\deploy\tbb.dll", @"Backup\tbb.dll", true);
                    File.Copy(@"RADS\projects\lol_air_client\releases\" + new DirectoryInfo(@"RADS\projects\lol_air_client\releases").GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\deploy\Adobe AIR\Versions\1.0\Resources\NPSWF32.dll", @"Backup\NPSWF32.dll", true);
                    File.Copy(@"RADS\projects\lol_air_client\releases\" + new DirectoryInfo(@"RADS\projects\lol_air_client\releases").GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\deploy\Adobe AIR\Versions\1.0\Adobe AIR.dll", @"Backup\Adobe AIR.dll", true);

                }
                else if (Directory.Exists("Game"))
                {
                    if (File.Exists(@"Game\DATA\CFG\defaults\game.cfg"))
                    {
                        File.Copy(@"Game\DATA\CFG\defaults\game.cfg", @"Backup\game.cfg", true);
                        File.Copy(@"Game\DATA\CFG\defaults\GamePermanent.cfg", @"Backup\GamePermanent.cfg", true);
                        if (File.Exists(@"Game\DATA\CFG\defaults\GamePermanent_zh_MY.cfg"))
                        {
                            File.Copy(@"Game\DATA\CFG\defaults\GamePermanent_zh_MY.cfg", @"Backup\GamePermanent_zh_MY.cfg", true);
                        }
                        if (File.Exists(@"Game\DATA\CFG\defaults\GamePermanent_en_SG.cfg"))
                        {
                            File.Copy(@"Game\DATA\CFG\defaults\GamePermanent_en_SG.cfg", @"Backup\GamePermanent_en_SG.cfg", true);
                        }
                    }
                    File.Copy(@"Game\cg.dll", @"Backup\cg.dll", true);
                    File.Copy(@"Game\cgD3D9.dll", @"Backup\cgD3D9.dll", true);
                    File.Copy(@"Game\cgGL.dll", @"Backup\cgGL.dll", true);
                    File.Copy(@"Game\tbb.dll", @"Backup\tbb.dll", true);
                    File.Copy(@"Air\Adobe Air\Versions\1.0\Resources\NPSWF32.dll", @"Backup\NPSWF32.dll", true);
                    File.Copy(@"Air\Adobe Air\Versions\1.0\Adobe AIR.dll", @"Backup\Adobe AIR.dll", true);

                }
            }
            if (Patch.IsChecked == true)
            {
                if (File.Exists(@"Config\game.cfg"))
                {
                    if (!File.ReadAllText(@"Config\game.cfg").Contains("DefaultParticleMultithreading=1"))
                    {
                        File.AppendAllText(@"Config\game.cfg", Environment.NewLine + "DefaultParticleMultithreading=1");
                    }
                }
                else if (File.Exists(@"Game\DATA\CFG\defaults\game.cfg"))
                {
                    if (!File.ReadAllText(@"Game\DATA\CFG\defaults\game.cfg").Contains("DefaultParticleMultithreading=1"))
                    {
                        File.AppendAllText(@"Game\DATA\CFG\defaults\game.cfg", Environment.NewLine + "DefaultParticleMultithreading=1");
                    }
                    if (File.Exists(@"Game\DATA\CFG\defaults\GamePermanent.cfg"))
                    {
                        if (!File.ReadAllText(@"Game\DATA\CFG\defaults\GamePermanent.cfg").Contains("DefaultParticleMultithreading=1"))
                        {
                            File.AppendAllText(@"Game\DATA\CFG\defaults\GamePermanent.cfg", Environment.NewLine + "DefaultParticleMultithreading=1");
                        }
                    }
                    if (File.Exists(@"Game\DATA\CFG\defaults\GamePermanent_zh_MY.cfg"))
                    {
                        if (!File.ReadAllText(@"Game\DATA\CFG\defaults\GamePermanent_zh_MY.cfg").Contains("DefaultParticleMultithreading=1"))
                        {
                            File.AppendAllText(@"Game\DATA\CFG\defaults\GamePermanent_zh_MY.cfg", Environment.NewLine + "DefaultParticleMultithreading=1");
                        }
                    }
                    if (File.Exists(@"Game\DATA\CFG\defaults\GamePermanent_en_SG.cfg"))
                    {
                        if (!File.ReadAllText(@"Game\DATA\CFG\defaults\GamePermanent_en_SG.cfg").Contains("DefaultParticleMultithreading=1"))
                        {
                            File.AppendAllText(@"Game\DATA\CFG\defaults\GamePermanent_en_SG.cfg", Environment.NewLine + "DefaultParticleMultithreading=1");
                        }
                    }
                }
                if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), @"Pando Networks\Media Booster\uninst.exe")))
                {
                    var PMB = new ProcessStartInfo();
                    var process = new Process();
                    PMB.FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), @"Pando Networks\Media Booster\uninst.exe");
                    PMB.Arguments = "/silent";
                    process.StartInfo = PMB;
                    process.Start();
                }
                if (Directory.Exists("RADS"))
                {
                    if (Cg.IsChecked == true)
                    {
                        File.Copy(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), @"NVIDIA Corporation\Cg\bin\cg.dll"), @"RADS\projects\lol_launcher\releases\" + new DirectoryInfo(@"RADS\projects\lol_launcher\releases").GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\deploy\cg.dll", true);
                        File.Copy(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), @"NVIDIA Corporation\Cg\bin\cg.dll"), @"RADS\solutions\lol_game_client_sln\releases\" + new DirectoryInfo(@"RADS\solutions\lol_game_client_sln\releases").GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\deploy\cg.dll", true);
                    }
                    if (CgGL.IsChecked == true)
                    {
                        File.Copy(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), @"NVIDIA Corporation\Cg\bin\cgGL.dll"), @"RADS\projects\lol_launcher\releases\" + new DirectoryInfo(@"RADS\projects\lol_launcher\releases").GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\deploy\cgGL.dll", true);
                        File.Copy(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), @"NVIDIA Corporation\Cg\bin\cgGL.dll"), @"RADS\solutions\lol_game_client_sln\releases\" + new DirectoryInfo(@"RADS\solutions\lol_game_client_sln\releases").GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\deploy\cgGL.dll", true);
                    }
                    if (CgD3D9.IsChecked == true)
                    {
                        File.Copy(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), @"NVIDIA Corporation\Cg\bin\cgD3D9.dll"), @"RADS\projects\lol_launcher\releases\" + new DirectoryInfo(@"RADS\projects\lol_launcher\releases").GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\deploy\cgD3D9.dll", true);
                        File.Copy(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), @"NVIDIA Corporation\Cg\bin\cgD3D9.dll"), @"RADS\solutions\lol_game_client_sln\releases\" + new DirectoryInfo(@"RADS\solutions\lol_game_client_sln\releases").GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\deploy\cgD3D9.dll", true);
                    }
                    if (tbb.IsChecked == true)
                    {
                        File.WriteAllBytes(@"RADS\solutions\lol_game_client_sln\releases\" + new DirectoryInfo(@"RADS\solutions\lol_game_client_sln\releases").GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\deploy\tbb.dll", Properties.Resources.tbb);
                    }
                    if (AdobeAIRBeta.IsChecked == true)
                    {
                        File.Copy(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), @"Common Files\Adobe AIR\Versions\1.0\Adobe AIR.dll"), @"RADS\projects\lol_air_client\releases\" + new DirectoryInfo(@"RADS\projects\lol_air_client\releases").GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\deploy\Adobe AIR\Versions\1.0\Adobe AIR.dll", true);
                    }
                    if (FlashBeta.IsChecked == true)
                    {
                        File.Copy(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), @"Common Files\Adobe AIR\Versions\1.0\Resources\NPSWF32.dll"), @"RADS\projects\lol_air_client\releases\" + new DirectoryInfo(@"RADS\projects\lol_air_client\releases").GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\deploy\Adobe AIR\Versions\1.0\Resources\NPSWF32.dll", true);
                    }
                    if (AdobeAIR.IsChecked == true)
                    {
                        File.Copy(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), @"Common Files\Adobe AIR\Versions\1.0\Adobe AIR.dll"), @"RADS\projects\lol_air_client\releases\" + new DirectoryInfo(@"RADS\projects\lol_air_client\releases").GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\deploy\Adobe AIR\Versions\1.0\Adobe AIR.dll", true);
                    }
                    if (Flash.IsChecked == true)
                    {
                        File.Copy(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), @"Common Files\Adobe AIR\Versions\1.0\Resources\NPSWF32.dll"), @"RADS\projects\lol_air_client\releases\" + new DirectoryInfo(@"RADS\projects\lol_air_client\releases").GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\deploy\Adobe AIR\Versions\1.0\Resources\NPSWF32.dll", true);
                    }
                }
                else if (Directory.Exists("Game"))
                {
                    if (Cg.IsChecked == true)
                    {
                        File.Copy(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), @"NVIDIA Corporation\Cg\bin\cg.dll"), @"Game\cg.dll", true);
                    }
                    if (CgD3D9.IsChecked == true)
                    {
                        File.Copy(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), @"NVIDIA Corporation\Cg\bin\cgD3D9.dll"), @"Game\cgdD3D9.dll", true);
                    }
                    if (CgGL.IsChecked == true)
                    {
                        File.Copy(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), @"NVIDIA Corporation\Cg\bin\cgGL.dll"), @"Game\cgGL.dll", true);
                    }
                    if (tbb.IsChecked == true)
                    {
                        File.WriteAllBytes(@"Game\tbb.dll", Properties.Resources.tbb);
                    }
                    if (AdobeAIRBeta.IsChecked == true)
                    {
                        File.Copy(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), @"Common Files\Adobe AIR\Versions\1.0\Adobe AIR.dll"), @"Air\Adobe Air\Versions\1.0\Adobe AIR.dll", true);
                    }
                    if (FlashBeta.IsChecked == true)
                    {
                        File.Copy(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), @"Common Files\Adobe AIR\Versions\1.0\Resources\NPSWF32.dll"), @"Air\Adobe Air\Versions\1.0\Resources\NPSWF32.dll", true);
                    }
                    if (AdobeAIR.IsChecked == true)
                    {
                        File.Copy(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), @"Common Files\Adobe AIR\Versions\1.0\Adobe AIR.dll"), @"Air\Adobe Air\Versions\1.0\Adobe AIR.dll", true);
                    }
                    if (Flash.IsChecked == true)
                    {
                        File.Copy(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), @"Common Files\Adobe AIR\Versions\1.0\Resources\NPSWF32.dll"), @"Air\Adobe Air\Versions\1.0\Resources\NPSWF32.dll", true);
                    }
                }
            }
            else if (Remove.IsChecked == true)
            {
                if (Directory.Exists("Rads"))
                {
                    File.Copy(@"Backup\cg.dll", @"RADS\solutions\lol_game_client_sln\releases\" + new DirectoryInfo(@"RADS\solutions\lol_game_client_sln\releases").GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\deploy\cg.dll", true);
                    File.Copy(@"Backup\cgD3D9.dll", @"RADS\solutions\lol_game_client_sln\releases\" + new DirectoryInfo(@"RADS\solutions\lol_game_client_sln\releases").GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\deploy\cgD3D9.dll", true);
                    File.Copy(@"Backup\cgGL.dll", @"RADS\solutions\lol_game_client_sln\releases\" + new DirectoryInfo(@"RADS\solutions\lol_game_client_sln\releases").GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\deploy\cgGL.dll", true);
                    File.Copy(@"Backup\tbb.dll", @"RADS\solutions\lol_game_client_sln\releases\" + new DirectoryInfo(@"RADS\solutions\lol_game_client_sln\releases").GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\deploy\tbb.dll", true);
                    File.Copy(@"Backup\cg.dll", @"RADS\projects\lol_launcher\releases\" + new DirectoryInfo(@"RADS\projects\lol_launcher\releases").GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\deploy\cg.dll", true);
                    File.Copy(@"Backup\cgD3D9.dll", @"RADS\projects\lol_launcher\releases\" + new DirectoryInfo(@"RADS\projects\lol_launcher\releases").GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\deploy\cgD3D9.dll", true);
                    File.Copy(@"Backup\cgGL.dll", @"RADS\projects\lol_launcher\releases\" + new DirectoryInfo(@"RADS\projects\lol_launcher\releases").GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\deploy\cgGL.dll", true);
                    File.Copy(@"Backup\NPSWF32.dll", @"RADS\projects\lol_air_client\releases\" + new DirectoryInfo(@"RADS\projects\lol_air_client\releases").GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\deploy\Adobe AIR\Versions\1.0\Resources\NPSWF32.dll", true);
                    File.Copy(@"Backup\Adobe AIR.dll", @"RADS\projects\lol_air_client\releases\" + new DirectoryInfo(@"RADS\projects\lol_air_client\releases").GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\deploy\Adobe AIR\Versions\1.0\Adobe AIR.dll", true);
                    if (File.Exists(@"Backup\game.cfg"))
                    {
                        File.Copy(@"Backup\game.cfg", @"Config\game.cfg", true);
                    }
                }
                else if (Directory.Exists("Game"))
                {
                    File.Copy(@"Backup\cg.dll", @"Game\cg.dll", true);
                    File.Copy(@"Backup\cgD3D9.dll", @"Game\cgD3D9.dll", true);
                    File.Copy(@"Backup\cgGL.dll", @"Game\cgGL.dll", true);
                    File.Copy(@"Backup\tbb.dll", @"Game\tbb.dll", true);
                    File.Copy(@"Backup\NPSWF32.dll", @"AIR\Adobe Air\Versions\1.0\Resources\NPSWF32.dll", true);
                    File.Copy(@"Backup\Adobe AIR.dll", @"AIR\Adobe Air\Versions\1.0\Adobe AIR.dll", true);
                    File.Copy(@"Backup\game.cfg", @"Game\DATA\CFG\defaults\game.cfg", true);
                    File.Copy(@"Backup\GamePermanent.cfg", @"Game\DATA\CFG\defaults\GamePermanent.cfg", true);
                    if (File.Exists(@"Backup\GamePermanent_zh_MY.cfg"))
                    {
                        File.Copy(@"Backup\GamePermanent_zh_MY.cfg", @"Game\DATA\CFG\defaults\GamePermanent_zh_MY.cfg", true);
                    }
                    if (File.Exists(@"Backup\GamePermanent_en_SG.cfg"))
                    {
                        File.Copy(@"Game\DATA\CFG\defaults\GamePermanent_en_SG.cfg", @"Game\DATA\CFG\defaults\GamePermanent_en_SG.cfg", true);
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
            if (!Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), @"NVIDIA Corporation\Cg\bin")))
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
            if (MessageBox.Show("We are unable to include the Adobe AIR Redistributable due to not having a licence BUT you are fully capable of installing it yourself. Click yes to download and run the installer before applying the patch.", "LoLUpdater", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Process.Start("http://airdownload.adobe.com/air/win/download/14.0/AdobeAIRInstaller.exe");
            }
        }
        private void AdobeAIR_Checked(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("We are unable to include the Adobe AIR Redistributable due to not having a licence BUT you are fully capable of installing it yourself. Click yes to download and run the installer before applying the patch.", "LoLUpdater", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Process.Start("http://airdownload.adobe.com/air/win/download/14.0/AdobeAIRInstaller.exe");
            }
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