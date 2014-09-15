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

        private static readonly string Air = Version("projects", "lol_air_client");
        private static readonly string Sln = Version("solutions", "lol_game_client_sln");
        private static readonly string Launch = Version("projects", "lol_launcher");
        private static readonly string Patch = Version("projects", "lol_patcher");
        private static void Main()
        {
            Console.WriteLine(Resources.Terms);
            if (!string.Equals(Console.ReadLine(), "y", StringComparison.InvariantCultureIgnoreCase)) return;
            Console.Clear();
            Console.WriteLine(Resources.Program_Main_Patching___);
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
                Cfg("game.cfg", "Config");
            }
            else if (File.Exists(Path.Combine(Path.Combine("Game", "DATA", "CFG", "defaults"), "game.cfg")))
            {
                Cfg("game.cfg", Path.Combine("Game", "DATA", "CFG", "defaults"));
                Cfg("GamePermanent.cfg", Path.Combine("Game", "DATA", "CFG", "defaults"));
                Cfg("GamePermanent_zh_MY.cfg",
                    Path.Combine("Game", "DATA", "CFG", "defaults"));
                Cfg("GamePermanent_en_SG.cfg",
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
                    Copybak("solutions", "lol_game_client_sln", "Cg.dll", string.Empty, Sln);
                    Copybak("solutions", "lol_game_client_sln", "CgD3D9.dll", string.Empty, Sln);
                    Copybak("solutions", "lol_game_client_sln", "CgGL.dll", string.Empty, Sln);
                    Copybak("solutions", "lol_game_client_sln", "tbb.dll", string.Empty, Sln);
                    Copybak("projects", "lol_air_client", "Adobe AIR.dll",
                        Path.Combine("Adobe Air", "Versions", "1.0"), Air);
                    Copybak("projects", "lol_air_client", "NPSWF32.dll",
                        Path.Combine("Adobe Air", "Versions", "1.0", "Resources"), Air);
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
                    "Cg.dll", "solutions", "lol_game_client_sln", Sln);
                AdvancedCopy(
                    "Cg.dll", "projects", "lol_launcher", Launch);
                AdvancedCopy(
                    "Cg.dll", "projects", "lol_patcher", Patch);
                AdvancedCopy(
                    "Cg.dll", "solutions", "lol_game_client_sln", Sln);
                AdvancedCopy(
                    "CgGL.dll", "projects", "lol_launcher", Launch);
                AdvancedCopy(
                    "CgGL.dll", "projects", "lol_patcher", Patch);
                AdvancedCopy(
                    "CgD3D9.dll", "solutions", "lol_game_client_sln", Sln);
                AdvancedCopy(
                    "CgD3D9.dll", "projects", "lol_launcher", Launch);
                AdvancedCopy(
                    "CgD3D9.dll", "projects", "lol_patcher", Patch);
                LocalCopy("solutions", "lol_game_client_sln", "tbb.dll", Resources.tbb, Sln);
                LocalCopy("projects", "lol_air_client",
                    Path.Combine("Adobe Air", "Versions", "1.0", "Resources", "NPSWF32.dll"),
                    Resources.NPSWF32, Air);
                LocalCopy("projects", "lol_air_client",
                    Path.Combine("Adobe Air", "Versions", "1.0", "Adobe AIR.dll"), Resources.Adobe_AIR, Air);
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

        private static void Copybak(string folder, string folder1, string file, string to,string version)
        {
                File.Copy(
                    Path.Combine("RADS", folder, folder1, "releases", version, "deploy", to, file)
                    , Path.Combine("Backup", file),
                    true);
        }

        private static void LocalCopy(string folder, string folder1, string file, byte[] file1,string version)
        {
                File.WriteAllBytes(
                    Path.Combine("RADS", folder, folder1, "releases", version, "deploy", file), file1);
        }

        private static void AdvancedCopy(string file, string folder, string folder1,string version)
        {
                File.Copy(
                    Path.Combine(
                        _cgBinPath, file),
                    Path.Combine("RADS", folder, folder1, "releases", version, "deploy", file), true);
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

        private static void Cfg(string file, string path)
        {
            if (
                !File.ReadAllText(Path.Combine(path, file))
                    .Contains(Resources.Program_Cfg_))
            {
                File.AppendAllText(Path.Combine(path, file),
                    Environment.NewLine + Resources.Program_Cfg_);
            }
        }
        private static string Version(string folder, string folder1)
        {
            return Path.GetFileName(Directory.GetDirectories(Path.Combine("RADS", folder, folder1, "releases")).Max());
        }
    }
}