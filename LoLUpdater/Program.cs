using LoLUpdater.Properties;
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Threading.Tasks;

namespace LoLUpdater
{
    internal static class Program
    {
        private static readonly string Sln = Version("solutions", "lol_game_client_sln");
        private static readonly string Air = Version("projects", "lol_air_client");
        private static readonly string Launch = Version("projects", "lol_launcher");
        private static readonly string Patch = Version("projects", "lol_patcher");
        private static readonly string[] LoLProcces = { "LoLClient", "LoLLauncher", "LoLPatcher", "League of Legends" };

        private static readonly string PmbUninstall = Path.Combine(Environment.Is64BitProcess
                ? Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86)
                : Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                "Pando Networks", "Media Booster", "uninst.exe");

        private static readonly bool IsMultiCore = new ManagementObjectSearcher("Select * from Win32_Processor").Get()
                    .Cast<ManagementBaseObject>()
                    .Sum(item => int.Parse(item["NumberOfCores"].ToString())) > 1;
        private static readonly bool IsHaswell = new ManagementObjectSearcher("Select * from Win32_Processor").Get()
            .Cast<ManagementBaseObject>()
            .Sum(item => int.Parse(item["Name"].ToString().Contains("Haswell") == true;

        private static string _cgBinPath = Environment.GetEnvironmentVariable("CG_BIN_PATH",
            EnvironmentVariableTarget.User);

        private static bool _intercept;

        private static void Main()
        {
            try
            {
                if (IsMultiCore)
                {
                    if (File.Exists(Path.Combine("Config", "game.cfg")))
                    {
                        Cfg("game.cfg", "Config", true);
                    }
                    if (File.Exists(Path.Combine("Game", "DATA", "CFG", "defaults", "game.cfg")))
                    {
                        Cfg("game.cfg", Path.Combine("Game", "DATA", "CFG", "defaults"), true);
                    }
                    if (File.Exists(Path.Combine("Game", "DATA", "CFG", "defaults", "GamePermanent.cfg")))
                    {
                        Cfg("GamePermanent.cfg", Path.Combine("Game", "DATA", "CFG", "defaults"), true);
                    }
                    if (File.Exists(Path.Combine("Game", "DATA", "CFG", "defaults", "GamePermanent_zh_MY.cfg")))
                    {
                        Cfg("GamePermanent_zh_MY.cfg",
                            Path.Combine("Game", "DATA", "CFG", "defaults"), true);
                    }
                    if (!File.Exists(Path.Combine("Game", "DATA", "CFG", "defaults", "GamePermanent_en_SG.cfg"))) return;
                    Cfg("GamePermanent_en_SG.cfg", Path.Combine("Game", "DATA", "CFG", "defaults"), true);
                }
                else
                {
                    if (File.Exists(Path.Combine("Config", "game.cfg")))
                    {
                        Cfg("game.cfg", "Config", false);
                    }
                    if (File.Exists(Path.Combine("Game", "DATA", "CFG", "defaults", "game.cfg")))
                    {
                        Cfg("game.cfg", Path.Combine("Game", "DATA", "CFG", "defaults"), false);
                    }
                    if (File.Exists(Path.Combine("Game", "DATA", "CFG", "defaults", "GamePermanent.cfg")))
                    {
                        Cfg("GamePermanent.cfg", Path.Combine("Game", "DATA", "CFG", "defaults"), false);
                    }
                    if (File.Exists(Path.Combine("Game", "DATA", "CFG", "defaults", "GamePermanent_zh_MY.cfg")))
                    {
                        Cfg("GamePermanent_zh_MY.cfg",
                            Path.Combine("Game", "DATA", "CFG", "defaults"), false);
                    }
                    if (!File.Exists(Path.Combine("Game", "DATA", "CFG", "defaults", "GamePermanent_en_SG.cfg"))) return;
                    Cfg("GamePermanent_en_SG.cfg", Path.Combine("Game", "DATA", "CFG", "defaults"), false);
                }
                if (File.Exists(PmbUninstall))
                {
                    Kill("PMB");
                    Process.Start(new ProcessStartInfo { FileName = PmbUninstall, Arguments = "/verysilent" });
                }
                if (Directory.Exists("Backup"))
                {
                    Console.WriteLine(Resources.UninstPrompt);

                    do
                    {
                        var readLine = Console.ReadLine();
                        if (readLine != null && readLine.Equals("n", StringComparison.OrdinalIgnoreCase))
                        {
                            Install();
                            _intercept = false;
                        }
                        else if (readLine != null && readLine.Equals("y", StringComparison.OrdinalIgnoreCase))
                        {
                            Console.Clear();
                            Console.WriteLine(Resources.Uninst);
                            Console.WriteLine("");
                            Kill(LoLProcces);
                            if (Directory.Exists("RADS"))
                            {
                                CopyY("Cg.dll", "solutions", "lol_game_client_sln", string.Empty, Sln);
                                CopyY("CgD3D9.dll", "solutions", "lol_game_client_sln", string.Empty, Sln);
                                CopyY("CgGL.dll", "solutions", "lol_game_client_sln", string.Empty, Sln);
                                CopyY("tbb.dll", "solutions", "lol_game_client_sln", string.Empty, Sln);
                                CopyY("Adobe AIR.dll", "projects", "lol_air_client",
                                    Path.Combine("Adobe Air", "Versions", "1.0"), Air);
                                CopyY("NPSWF32.dll", "projects", "lol_air_client",
                                    Path.Combine("Adobe Air", "Versions", "1.0", "Resources"), Air);
                                Directory.Delete("Backup", true);
                            }
                            else if (Directory.Exists("Game"))
                            {
                                Copy("Cg.dll", "Backup", "Game");
                                Copy("CgGL.dll", "Backup", "Game");
                                Copy("CgD3D9.dll", "Backup", "Game");
                                Copy("Tbb.dll", "Backup", "Game");
                                Copy("NPSWF32.dll", "Backup",
                                    Path.Combine("Air", "Adobe AIR", "Versions", "1.0", "Resources"));
                                Copy("Adobe AIR.dll", "Backup", Path.Combine("Air", "Adobe AIR", "Versions", "1.0"));
                                Directory.Delete("Backup", true);
                            }
                            _intercept = false;
                        }
                    } while (_intercept);
                }
                else
                {
                    Install();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.ReadLine();
                Environment.Exit(0);
            }
            finally
            {
                Console.WriteLine("");
                Console.WriteLine(Resources.Done);
                if (File.Exists("lol.launcher.exe"))
                {
                    Process.Start("lol.launcher.exe");
                }
                Console.WriteLine();
                Environment.Exit(0);
            }
        }

        private static void Install()
        {
            Console.WriteLine(Resources.Terms);
            if (!string.Equals(Console.ReadLine(), "y", StringComparison.OrdinalIgnoreCase)) return;
            Console.Clear();
            Console.WriteLine(Resources.Patching);
            Console.WriteLine("");
            Kill(LoLProcces);
            if (!Directory.Exists("Backup"))
            {
                if (Directory.Exists("RADS"))
                {
                    Directory.CreateDirectory("Backup");
                    CopyX("solutions", "lol_game_client_sln", "Cg.dll", string.Empty, Sln);
                    CopyX("solutions", "lol_game_client_sln", "CgD3D9.dll", string.Empty, Sln);
                    CopyX("solutions", "lol_game_client_sln", "CgGL.dll", string.Empty, Sln);
                    CopyX("solutions", "lol_game_client_sln", "tbb.dll", string.Empty, Sln);
                    CopyX("projects", "lol_air_client", "Adobe AIR.dll",
                        Path.Combine("Adobe Air", "Versions", "1.0"), Air);
                    CopyX("projects", "lol_air_client", "NPSWF32.dll",
                        Path.Combine("Adobe Air", "Versions", "1.0", "Resources"), Air);
                    if (!File.Exists(Path.Combine("Config", "game.cfg"))) return;
                    Copy("game.cfg", "Config", "Backup");
                }
                else if (!File.Exists("Game")) return;
                Directory.CreateDirectory("Backup");
                Copy("Cg.dll", "Game", "Backup");
                Copy("CgGL.dll", "Game", "Backup");
                Copy("CgD3D9.dll", "Game", "Backup");
                Copy("tbb.dll", "Game", "Backup");
                Copy("Adobe AIR.dll", Path.Combine("Air", "Adobe AIR", "Versions", "1.0"), "Backup");
                Copy("NPSWF32.dll", Path.Combine("Air", "Adobe AIR", "Versions", "1.0", "Resources"),
                    "Backup");
                if (File.Exists(Path.Combine("Game", "DATA", "CFG", "defaults", "game.cfg")))
                {
                    Copy("game.cfg", Path.Combine("Game", "DATA", "CFG", "defaults"), "Backup");
                }

                if (File.Exists(Path.Combine(Path.Combine("Game", "DATA", "CFG", "defaults"),
                    "GamePermanent.cfg")))
                {
                    Copy("GamePermanent.cfg", Path.Combine("Game", "DATA", "CFG", "defaults"),
                        "Backup");
                }
                if (
                    File.Exists(Path.Combine(Path.Combine("Game", "DATA", "CFG", "defaults"),
                        "GamePermanent_zh_MY.cfg")))
                {
                    Copy("GamePermanent_zh_MY.cfg", Path.Combine("Game", "DATA", "CFG", "defaults"),
                        "Backup");
                }
                if (
                    !File.Exists(Path.Combine(Path.Combine("Game", "DATA", "CFG", "defaults"),
                        "GamePermanent_en_SG.cfg"))) return;
                Copy("GamePermanent_en_SG.cfg", Path.Combine("Game", "DATA", "CFG", "defaults"),
                    "Backup");
            }
            if (string.IsNullOrEmpty(_cgBinPath) ||
                new Version(FileVersionInfo.GetVersionInfo(Path.Combine(_cgBinPath, "cg.dll")).FileVersion) <
                new Version("3.1.0013"))
            {
                File.WriteAllBytes(Path.Combine(Path.GetTempFileName(), "Cg-3.1_April2012_Setup.exe"),
                    Resources.Cg_3_1_April2012_Setup);
                Process cg = new Process
                {
                    StartInfo =
                        new ProcessStartInfo
                        {
                            FileName = Path.Combine(Path.GetTempFileName(), "Cg-3.1_April2012_Setup.exe"),
                            Arguments = "/silent /TYPE=compact"
                        }
                };
                cg.Start();
                cg.WaitForExit();
                File.Delete(Path.Combine(Path.GetTempFileName(), "Cg-3.1_April2012_Setup.exe"));
                _cgBinPath = Environment.GetEnvironmentVariable("CG_BIN_PATH", EnvironmentVariableTarget.User);
            }
            if (Directory.Exists("RADS"))
            {
                CopyRes("projects", "lol_air_client",
                    Path.Combine("Adobe Air", "Versions", "1.0", "Resources", "NPSWF32.dll"),
                    Resources.NPSWF32, Air);
                CopyRes("projects", "lol_air_client",
                    Path.Combine("Adobe Air", "Versions", "1.0", "Adobe AIR.dll"), Resources.Adobe_AIR, Air);

                Copy(
                    "Cg.dll", "solutions", "lol_game_client_sln", Sln);
                Copy(
                    "Cg.dll", "projects", "lol_launcher", Launch);
                Copy(
                    "Cg.dll", "projects", "lol_patcher", Patch);

                Copy(
                    "CgGL.dll", "projects", "lol_launcher", Launch);
                Copy(
                    "CgGL.dll", "projects", "lol_patcher", Patch);
                Copy(
                    "CgD3D9.dll", "solutions", "lol_game_client_sln", Sln);
                Copy(
                    "CgD3D9.dll", "projects", "lol_launcher", Launch);
                Copy(
                    "CgD3D9.dll", "projects", "lol_patcher", Patch);

                if (IsHaswell)
                {
                    CopyRes("solutions", "lol_game_client_sln", "tbb.dll", Resources.Haswell, Sln);
                }
                else
                {
                    if (IsMultiCore)
                    {
                        if (InstructionsSupported(6) || InstructionsSupported(10) || InstructionsSupported(17))
                        {
                            CopyRes("solutions", "lol_game_client_sln", "tbb.dll", Resources.SSE, Sln);
                            if (!InstructionsSupported(10) || !InstructionsSupported(17)) return;
                            CopyRes("solutions", "lol_game_client_sln", "tbb.dll", Resources.SSE2, Sln);
                            if (!InstructionsSupported(17)) return;
                            CopyRes("solutions", "lol_game_client_sln", "tbb.dll", Resources.AVX, Sln);
                        }
                        else
                        {
                            CopyRes("solutions", "lol_game_client_sln", "tbb.dll", Resources.tbb, Sln);
                        }
                    }
                    else
                    {
                        if (InstructionsSupported(6) || InstructionsSupported(10))
                        {
                            CopyRes("solutions", "lol_game_client_sln", "tbb.dll", Resources.SSEST, Sln);
                            if (!InstructionsSupported(10)) return;
                            CopyRes("solutions", "lol_game_client_sln", "tbb.dll", Resources.SSE2ST, Sln);
                        }
                        else
                        {
                            CopyRes("solutions", "lol_game_client_sln", "tbb.dll", Resources.tbbST, Sln);
                        }
                    }
                }
            }
            else if (Directory.Exists("Game"))
            {
                Copy("Cg.dll",
                _cgBinPath,
                "Game");
                Copy("CgGL.dll",
                    _cgBinPath,
                    "Game");
                Copy("CgD3D9.dll", _cgBinPath, "Game");

                File.WriteAllBytes(
                    Path.Combine("Air", "Adobe Air", "Versions", "1.0", "Adobe AIR.dll"), Resources.Adobe_AIR);
                File.WriteAllBytes(
                    Path.Combine("Air", "Adobe Air", "Versions", "1.0", "Resources", "NPSWF32.dll"),
                    Resources.NPSWF32);

                if (IsHaswell)
                {
                    File.WriteAllBytes(Path.Combine("Game", "tbb.dll"), Resources.Haswell);
                }
                else
                {
                    if (IsMultiCore)
                    {
                        if (InstructionsSupported(6) || InstructionsSupported(10) || InstructionsSupported(17))
                        {
                            File.WriteAllBytes(Path.Combine("Game", "tbb.dll"), Resources.SSE);
                            if (!InstructionsSupported(10) || !InstructionsSupported(17)) return;
                            File.WriteAllBytes(Path.Combine("Game", "tbb.dll"), Resources.SSE2);
                            if (!InstructionsSupported(17)) return;
                            File.WriteAllBytes(Path.Combine("Game", "tbb.dll"), Resources.AVX);
                        }
                        else
                        {
                            File.WriteAllBytes(Path.Combine("Game", "tbb.dll"), Resources.tbb);
                        }
                    }
                    else
                    {
                        if (InstructionsSupported(6) || InstructionsSupported(10))
                        {
                            File.WriteAllBytes(Path.Combine("Game", "tbb.dll"), Resources.SSEST);
                            if (!InstructionsSupported(10)) return;
                            File.WriteAllBytes(Path.Combine("Game", "tbb.dll"), Resources.SSE2ST);
                        }
                        else
                        {
                            File.WriteAllBytes(Path.Combine("Game", "tbb.dll"), Resources.tbbST);
                        }
                    }
                }
            }
        }

        private static void Kill(IEnumerable process)
        {
            if (IsMultiCore)
            {
                Parallel.ForEach(Process.GetProcessesByName(process.ToString()), proc =>
                {
                    proc.Kill();
                    proc.WaitForExit();
                });
            }
            else
            {
                foreach (Process proc in Process.GetProcessesByName(process.ToString()))
                {
                    proc.Kill();
                    proc.WaitForExit();
                }
            }
        }

        private static void CopyX(string folder, string folder1, string file, string to, string version)
        {
            File.Copy(
                Path.Combine("RADS", folder, folder1, "releases", version, "deploy", to, file)
                , Path.Combine("Backup", file),
                true);
        }

        private static void CopyY(string folder, string folder1, string file, string to, string version)
        {
            File.Copy(Path.Combine("Backup", file)

                , Path.Combine("RADS", folder, folder1, "releases", version, "deploy", to, file),
                true);
        }

        private static void CopyRes(string folder, string folder1, string file, byte[] file1, string version)
        {
            File.WriteAllBytes(
                Path.Combine("RADS", folder, folder1, "releases", version, "deploy", file), file1);
        }

        private static void Copy(string file, string folder, string folder1, string version)
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

        private static void Cfg(string file, string path, bool mode)
        {
            if (mode)
            {
                if (File.ReadAllText(Path.Combine(path, file))
                    .Contains(Resources.CfgString)) return;
                File.AppendAllText(Path.Combine(path, file),
                    string.Format("{0}{1}", Environment.NewLine, Resources.CfgString));
            }
            else
            {
                var oldLines = File.ReadAllLines(Path.Combine(path, file));
                if (!oldLines.Contains(Resources.CfgString)) return;
                var newLines = oldLines.Select(line => new { Line = line, Words = line.Split(' ') }).Where(lineInfo => !lineInfo.Words.Contains(Resources.CfgString)).Select(lineInfo => lineInfo.Line);
                File.WriteAllLines(Path.Combine(path, file), newLines);
            }
        }

        private static string Version(string folder, string folder1)
        {
            return Path.GetFileName(Directory.GetDirectories(Path.Combine("RADS", folder, folder1, "releases")).Max());
        }

        private static bool InstructionsSupported(int id)
        {
            return (ushort)new ManagementObject("Win32_Processor.DeviceID='CPU0'")["Architecture"] == id;
        }
    }
}
