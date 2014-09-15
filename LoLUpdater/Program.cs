using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using LoLUpdater.Properties;

namespace LoLUpdater
{
    internal class Program
    {
        private static string _cgBinPath = Environment.GetEnvironmentVariable("CG_BIN_PATH",
            EnvironmentVariableTarget.User);

        private static void Main()
        {
            Console.WriteLine(Resources.Terms);
            var terms = Console.ReadLine();
            if (!string.Equals(terms, "y", StringComparison.InvariantCultureIgnoreCase)) return;
            Console.Clear();
            Console.WriteLine(Resources.Program_Main_Patching_);
            Console.WriteLine("");
            Kill("LoLClient");
            Kill("LoLLauncher");
            Kill("LoLPatcher");
            Kill("League of Legends");
            Kill("PMB");
            var pmbUninstall = Path.Combine(Environment.Is64BitProcess
                ? Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86)
                : Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                "Pando Networks", "Media Booster", "uninst.exe");
            if (File.Exists(pmbUninstall))
            {
                Process.Start(new ProcessStartInfo { FileName = pmbUninstall, Arguments = "/verysilent" });
            }
            if (File.Exists(Path.Combine("Config", "game.cfg")))
            {
                Cfg("DefaultMultiThreading=1", "game.cfg", "Config");
            }
            else if (File.Exists(Path.Combine(Path.Combine("Game", "DATA", "CFG", "defaults"), "game.cfg")))
            {
                Cfg("DefaultMultiThreading=1", "game.cfg", Path.Combine("Game", "DATA", "CFG", "defaults"));
                Cfg("DefaultMultiThreading=1", "GamePermanent.cfg", Path.Combine("Game", "DATA", "CFG", "defaults"));
                Cfg("DefaultMultiThreading=1", "GamePermanent_zh_MY.cfg",
                    Path.Combine("Game", "DATA", "CFG", "defaults"));
                Cfg("DefaultMultiThreading=1", "GamePermanent_en_SG.cfg",
                    Path.Combine("Game", "DATA", "CFG", "defaults"));
            }
            if (!Directory.Exists("Backup"))
            {
                Directory.CreateDirectory("Backup");
                if (Directory.Exists("RADS"))
                {
                    if (File.Exists(Path.Combine("Config", "game.cfg")))
                    {
                        Copy("game.cfg", "Config", "Backup");
                    }
                    Copybak("solutions", "lol_game_client_sln", "Cg.dll", string.Empty);
                    Copybak("solutions", "lol_game_client_sln", "CgD3D9.dll", string.Empty);
                    Copybak("solutions", "lol_game_client_sln", "CgGL.dll", string.Empty);
                    Copybak("solutions", "lol_game_client_sln", "tbb.dll", string.Empty);
                    Copybak("projects", "lol_air_client", "Adobe AIR.dll",
                        Path.Combine("Adobe Air", "Versions", "1.0"));
                    Copybak("projects", "lol_air_client", "NPSWF32.dll",
                        Path.Combine("Adobe Air", "Versions", "1.0", "Resources"));
                }
                else if (Directory.Exists("Game"))
                {
                    if (File.Exists(Path.Combine(Path.Combine("Game", "DATA", "CFG", "defaults"), "game.cfg")))
                    {
                        Copy("game.cfg", Path.Combine("Game", "DATA", "CFG", "defaults"), "Backup");
                        Copy("GamePermanent.cfg", Path.Combine("Game", "DATA", "CFG", "defaults"), "Backup");
                        if (
                            File.Exists(Path.Combine(Path.Combine("Game", "DATA", "CFG", "defaults"),
                                "GamePermanent_zh_MY.cfg")))
                        {
                            Copy("GamePermanent_zh_MY.cfg", Path.Combine("Game", "DATA", "CFG", "defaults"),
                                "Backup");
                            Copy("game.cfg", Path.Combine("Game", "DATA", "CFG", "defaults"), "Backup");
                        }
                        if (
                            File.Exists(Path.Combine(Path.Combine("Game", "DATA", "CFG", "defaults"),
                                "GamePermanent_en_SG.cfg")))
                        {
                            Copy("GamePermanent_en_SG.cfg", Path.Combine("Game", "DATA", "CFG", "defaults"),
                                "Backup");
                        }
                    }
                    Copy("Cg.dll", "Game", "Backup");
                    Copy("CgGL.dll", "Game", "Backup");
                    Copy("CgD3D9.dll", "Game", "Backup");
                    Copy("tbb.dll", "Game", "Backup");
                    Copy("Adobe AIR.dll", Path.Combine("Air", "Adobe AIR", "Versions", "1.0"), "Backup");
                    Copy("NPSWF32.dll", Path.Combine("Air", "Adobe AIR", "Versions", "1.0", "Resources"),
                        "Backup");
                }
            }
            if (string.IsNullOrEmpty(_cgBinPath))
            {
                InstallCg();
            }
            else
            {
                var currentCgVersion =
                    new Version(FileVersionInfo.GetVersionInfo(Path.Combine(_cgBinPath, "cg.dll")).FileVersion);
                if (
                    currentCgVersion < new Version("3.1.0013"))
                {
                    InstallCg();
                }
            }
            if (Directory.Exists("RADS"))
            {
                AdvancedCopy(
                    "Cg.dll", "solutions", "lol_game_client_sln");
                AdvancedCopy(
                    "Cg.dll", "projects", "lol_launcher");
                AdvancedCopy(
                    "Cg.dll", "projects", "lol_patcher");
                AdvancedCopy(
                    "Cg.dll", "solutions", "lol_game_client_sln");
                AdvancedCopy(
                    "CgGL.dll", "projects", "lol_launcher");
                AdvancedCopy(
                    "CgGL.dll", "projects", "lol_patcher");
                AdvancedCopy(
                    "CgD3D9.dll", "solutions", "lol_game_client_sln");
                AdvancedCopy(
                    "CgD3D9.dll", "projects", "lol_launcher");
                AdvancedCopy(
                    "CgD3D9.dll", "projects", "lol_patcher");
                LocalCopy("solutions", "lol_game_client_sln", "tbb.dll", Resources.tbb);
                LocalCopy("projects", "lol_air_client",
                    Path.Combine("Adobe Air", "Versions", "1.0", "Resources", "NPSWF32.dll"),
                    Resources.NPSWF32);
                LocalCopy("projects", "lol_air_client",
                    Path.Combine("Adobe Air", "Versions", "1.0", "Adobe AIR.dll"), Resources.Adobe_AIR);
                Process.Start("lol.launcher.exe");
            }
            if (Directory.Exists("Game"))
            {
                Copy("Cg.dll",
                    _cgBinPath,
                    "Game");
                Copy("CgGL.dll",
                    _cgBinPath,
                    "Game");
                Copy("CgD3D9.dll", _cgBinPath, "Game");
                File.WriteAllBytes(Path.Combine("Game", "tbb.dll"), Resources.tbb);
                File.WriteAllBytes(
                    Path.Combine("Air", "Adobe Air", "Versions", "1.0", "Adobe AIR.dll"), Resources.Adobe_AIR);
                File.WriteAllBytes(
                    Path.Combine("Air", "Adobe Air", "Versions", "1.0", "Resources", "NPSWF32.dll"), Resources.NPSWF32);
            }
            Console.WriteLine(Resources.Program_Main_Done_);
            Console.ReadLine();
        }

        private static void Kill(string process)
        {
            foreach (var proc in Process.GetProcessesByName(process))
            {
                proc.Kill();
                proc.WaitForExit();
            }
        }

        private static void Copybak(string folder, string folder1, string file, string to)
        {
            var firstOrDefault = new DirectoryInfo(Path.Combine("RADS", folder, folder1, "releases"))
                .GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault();
            if (firstOrDefault != null)
                File.Copy(
                    Path.Combine("RADS", folder, folder1, "releases", firstOrDefault.ToString(), "deploy", to, file)
                    , Path.Combine("Backup", file),
                    true);
        }

        private static void LocalCopy(string folder, string folder1, string file, byte[] file1)
        {
            var firstOrDefault = new DirectoryInfo(Path.Combine("RADS", folder, folder1, "releases"))
                .GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault();
            if (firstOrDefault != null)
                File.WriteAllBytes(
                    Path.Combine("RADS", folder, folder1, "releases", firstOrDefault.ToString(), "deploy", file), file1);
        }

        private static void AdvancedCopy(string file, string folder, string folder1)
        {
            var firstOrDefault = new DirectoryInfo(Path.Combine("RADS", folder, folder1, "releases"))
                .GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault();
            if (firstOrDefault != null)
                File.Copy(
                    Path.Combine(
                        _cgBinPath, file),
                    Path.Combine("RADS", folder, folder1, "releases", firstOrDefault.ToString(), "deploy", file), true);
        }

        private static void Copy(string file, string from, string to)
        {
            File.Copy(Path.Combine(from, file),
                Path.Combine(to, file), true);
        }

        private static void InstallCg()
        {
            File.WriteAllBytes("Cg-3.1_April2012_Setup.exe", Resources.Cg_3_1_April2012_Setup);
            var cg = new Process
            {
                StartInfo =
                    new ProcessStartInfo
                    {
                        FileName = "Cg-3.1_April2012_Setup.exe",
                        Arguments = "/verysilent /TYPE=compact"
                    }
            };
            cg.Start();
            cg.WaitForExit();
            File.Delete("Cg-3.1_April2012_Setup.exe");
            _cgBinPath = Environment.GetEnvironmentVariable("CG_BIN_PATH", EnvironmentVariableTarget.User);
        }

        private static void Cfg(string setting, string file, string game)
        {
            if (
                !File.ReadAllText(Path.Combine(game, file))
                    .Contains(setting))
            {
                File.AppendAllText(Path.Combine(game, file),
                    Environment.NewLine + setting);
            }
        }
    }
}