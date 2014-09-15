using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using LoLUpdater.Properties;
namespace LoLUpdater
{
    internal class Program
    {
        private static string _cgBinPath = Environment.GetEnvironmentVariable("CG_BIN_PATH", EnvironmentVariableTarget.User);
        private static readonly string GameCfg = Path.Combine("Game", "DATA", "CFG", "defaults");
        private static readonly string Arch = Environment.Is64BitProcess
? Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86)
: Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
        private static void Main()
        {
            Console.WriteLine(Resources.Terms);
            var terms = Console.ReadLine();
            if (terms != ("Y") && terms != ("y")) return;
            Console.Clear();
            Console.WriteLine(Resources.Program_Main_Patching_);
            Console.WriteLine("");
            Kill("LoLClient");
            Kill("LoLLauncher");
            Kill("LoLPatcher");
            Kill("League of Legends");
            Kill("PMB");
            var pmbUninstall = Path.Combine(Arch,
"Pando Networks", "Media Booster", "uninst.exe");
            if (File.Exists(pmbUninstall))
            { Process.Start(new ProcessStartInfo { FileName = pmbUninstall, Arguments = "/silent" }); }
            
            HandleCfg("DefaultMultiThreading=1");
                    if (Directory.Exists("Backup")) return;
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
                        if (File.Exists(Path.Combine(GameCfg, "game.cfg")))
                        {
                            Copy("game.cfg", (GameCfg, "Backup");
                            Copy("GamePermanent.cfg", (GameCfg, "Backup");
                            if (
                                File.Exists(Path.Combine((GameCfg,
                                    "GamePermanent_zh_MY.cfg")))
                            {
                                Copy("GamePermanent_zh_MY.cfg", (GameCfg,
                                    "Backup");
                                Copy("game.cfg", (GameCfg, "Backup");
                            }
                            if (
                                File.Exists(Path.Combine((GameCfg,
                                    "GamePermanent_en_SG.cfg")))
                            {
                                Copy("GamePermanent_en_SG.cfg", (GameCfg,
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
            if (_cgBinPath == null)
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
                "Cg.dll", _cgBinPath,
                "solutions", "lol_game_client_sln");
            AdvancedCopy(
                "Cg.dll", _cgBinPath,
                "projects", "lol_launcher");
            AdvancedCopy(
                "Cg.dll", _cgBinPath,
                "projects", "lol_patcher");
            AdvancedCopy(
                "Cg.dll", _cgBinPath,
                "solutions", "lol_game_client_sln");
            AdvancedCopy(
                "CgGL.dll", _cgBinPath,
                "projects", "lol_launcher");
            AdvancedCopy(
                "CgGL.dll", _cgBinPath,
                "projects", "lol_patcher");
            AdvancedCopy(
                "CgD3D9.dll", _cgBinPath,
                "solutions", "lol_game_client_sln");
            AdvancedCopy(
                "CgD3D9.dll", _cgBinPath,
                "projects", "lol_launcher");
            AdvancedCopy(
                "CgD3D9.dll", _cgBinPath,
                "projects", "lol_patcher");
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
        private static void LocalCopy(string path, string path1, string file, byte[] file1)
        {
            File.WriteAllBytes(Path.Combine("RADS", path, path1, "releases") + "\\" +
                               new DirectoryInfo(Path.Combine("RADS", path, path1, "releases"))
                                   .GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + "\\" +
                               Path.Combine("deploy", file), file1);
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
            File.Copy(Path.Combine(Path.Combine("RADS", folder, folder1, "releases") + "\\" +
                                   new DirectoryInfo(Path.Combine("RADS", folder, folder1, "releases"))
                                       .GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + "\\",
                Path.Combine(Path.Combine("deploy", to, file)), Path.Combine("Backup", file),
                true);
        }
        private static void Copy(string file, string from, string to)
        {
            File.Copy(Path.Combine(from, file),
                Path.Combine(to, file), true);
        }
        private static void AdvancedCopy(string file, string from, string folder, string folder1, string to)
        {
            File.Copy(
                Path.Combine(
                    from, file),
                Path.Combine("RADS", folder, folder1, "releases") + "\\" +
                new DirectoryInfo(Path.Combine("RADS", folder, folder1, "releases"))
                    .GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + "\\" +
                Path.Combine("deploy", file), true);
        }
        private static void InstallCg()
        {
            File.WriteAllBytes("Cg-3.1_April2012_Setup.exe", Resources.Cg_3_1_April2012_Setup);
            var cg = new Process
            {
                StartInfo =
                    new ProcessStartInfo { FileName = "Cg-3.1_April2012_Setup.exe", Arguments = "/verysilent /TYPE=compact" }
            };
            cg.Start();
            cg.WaitForExit();
            File.Delete("Cg-3.1_April2012_Setup.exe");
            _cgBinPath = Environment.GetEnvironmentVariable("CG_BIN_PATH", EnvironmentVariableTarget.User);
        }
        private static void HandleCfg(string setting)
        {
            if (File.Exists(Path.Combine("Config", "game.cfg")))
            {
                Cfg(setting, "game.cfg", "Config");
            }
            else if (File.Exists(Path.Combine(GameCfg, "game.cfg")))
            {
                Cfg(setting, "game.cfg", GameCfg);
                Cfg(setting, "GamePermanent.cfg", GameCfg);
                Cfg(setting, "GamePermanent_zh_MY.cfg", GameCfg);
                Cfg(setting, "GamePermanent_en_SG.cfg", GameCfg);
            }
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
