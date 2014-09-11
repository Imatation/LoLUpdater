using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Xml;
using Microsoft.Win32;

namespace LoLUpdaterXP
{
    public partial class MainWindow
    {
        private static readonly string GameCfg = Path.Combine("Game", "DATA", "CFG", "defaults");

        private static string _cgBinPath = Environment.GetEnvironmentVariable("CG_BIN_PATH",
            EnvironmentVariableTarget.User);

        private static readonly string Reg = Environment.Is64BitProcess
            ? string.Empty
            : "WoW64Node";

        private static readonly string Arch = Environment.Is64BitProcess
            ? Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86)
            : Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);

        private static readonly string AirPath = Path.Combine(Arch, "Common Files", "Adobe AIR", "Versions", "1.0");

        private const string IntendedVersion = "0.0.1.105";

        private readonly BackgroundWorker _worker = new BackgroundWorker();
        private bool _wasPatched = true;

        private readonly List<WorstHack> _reassembleLocations;

        public MainWindow()
        {
            InitializeComponent();
            _reassembleLocations = new List<WorstHack>();
            FindButton.AddHandler(MouseDownEvent, new RoutedEventHandler(FindButton_MouseDown), true);
            if (Directory.Exists("temp"))
            {
                DeletePathWithLongFileNames(Path.GetFullPath("temp"));
            }
            _worker.DoWork += worker_DoWork;
            _worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            AppDomain.CurrentDomain.FirstChanceException += CurrentDomain_FirstChanceException;
        }

        private void CurrentDomain_FirstChanceException(object sender,
            System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs e)
        {
            var ex = e.Exception;
            MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine);
            _wasPatched = false;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (!Directory.Exists("mods"))
            {
                MessageBox.Show("Missing mods directory. Ensure that all files were extracted properly.",
                    "Missing files");
            }

            var modList = Directory.GetDirectories("mods");

            foreach (var mod in modList)
            {
                var check = new CheckBox {IsChecked = true, Content = mod.Replace("mods\\", "")};
                if (File.Exists(Path.Combine(mod, "disabled")))
                    check.IsChecked = false;
                ModsListBox.Items.Add(check);
            }
        }

        private void ModsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var box = (CheckBox) ModsListBox.SelectedItem;

            if (box == null)
                return;

            var selectedMod = (string) box.Content;
            using (var reader = XmlReader.Create(Path.Combine("mods", selectedMod, "info.xml")))
            {
                while (reader.Read())
                {
                    if (!reader.IsStartElement()) continue;
                    switch (reader.Name)
                    {
                        case "name":
                            reader.Read();
                            ModNameLabel.Content = reader.Value;
                            break;
                        case "description":
                            reader.Read();
                            ModDescriptionBox.Text = reader.Value;
                            break;
                    }
                }
            }
        }

        private void FindButton_MouseDown(object sender, RoutedEventArgs e)
        {
            PatchButton.IsEnabled = false;
            var findLeagueDialog = new OpenFileDialog
            {
                InitialDirectory =
                    !Directory.Exists(Path.Combine("C:\\", "Riot Games", "League of Legends"))
                        ? Path.Combine("C:\\", "Program Files (x86)", "GarenaLoL", "GameData", "Apps", "LoL")
                        : Path.Combine("C:\\", "Riot Games", "League of Legends"),
                DefaultExt = ".exe",
                Filter = "League of Legends Launcher|lol.launcher*.exe|Garena Launcher|lol.exe"
            };
            var result = findLeagueDialog.ShowDialog();
            if (result != true) return;
            var filename = findLeagueDialog.FileName.Replace("lol.launcher.exe", "")
                .Replace("lol.launcher.admin.exe", "");
            if (filename.Contains("lol.exe"))
            {
                PatchButton.IsEnabled = true;
                RemoveButton.IsEnabled = false;

                filename = filename.Replace("lol.exe", "");

                LocationTextbox.Text = Path.Combine(filename, "Air");
            }
            else
            {
                var radLocation = Path.Combine(filename, "RADS", "projects", "lol_air_client", "releases");
                var versionDirectories = Directory.GetDirectories(radLocation);
                var finalDirectory = "";
                var version = "";
                uint versionCompare = 0;
                foreach (var x in versionDirectories)
                {
                    var compare1 = x.Substring(x.LastIndexOfAny(new[] {'\\', '/'}) + 1);

                    var versionParts = compare1.Split(new[] {'.'});

                    if (!compare1.Contains(".") || versionParts.Length != 4)
                    {
                        continue;
                    }

                    uint compareVersion;
                    try
                    {
                        compareVersion = Convert.ToUInt32(versionParts[0]) << 24 |
                                         Convert.ToUInt32(versionParts[1]) << 16 |
                                         Convert.ToUInt32(versionParts[2]) << 8 | Convert.ToUInt32(versionParts[3]);
                    }
                    catch (FormatException)
                    {
                        continue;
                    }

                    if (compareVersion <= versionCompare) continue;
                    versionCompare = compareVersion;
                    version = x.Replace(radLocation + "\\", "");
                    finalDirectory = x;
                }

                if (version != IntendedVersion)
                {
                    var versionMismatchResult =
                        MessageBox.Show(
                            "This version of LESs is intended for " + IntendedVersion +
                            ". Your current version of League of Legends is " + version +
                            ". Continue? This could harm your installation.", "Invalid Version", MessageBoxButton.YesNo);
                    if (versionMismatchResult == MessageBoxResult.No)
                        return;
                }

                PatchButton.IsEnabled = true;

                RemoveButton.IsEnabled = true;

                LocationTextbox.Text = Path.Combine(finalDirectory, "deploy");
            }

            Directory.CreateDirectory("LESsBackup");
            Directory.CreateDirectory(Path.Combine("LESsBackup", IntendedVersion));
        }

        private void PatchButton_Click(object sender, RoutedEventArgs e)
        {
            _worker.RunWorkerAsync();
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!File.Exists(Path.Combine(LocationTextbox.Text.Substring(0, LocationTextbox.Text.Length - 7), "S_OK")))
                return;
            File.Delete(Path.Combine(LocationTextbox.Text.Substring(0, LocationTextbox.Text.Length - 7), "S_OK"));
            MessageBox.Show("LESs will be removed next time League of Legends launches!");
            StatusLabel.Content = "Removed LESs";
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            ItemCollection modCollection = null;
            Dispatcher.BeginInvoke(DispatcherPriority.Input,
                new ThreadStart(() => { modCollection = ModsListBox.Items; }));

            // ReSharper disable once LoopVariableIsNeverChangedInsideLoop
            while (modCollection == null)
            {
            }

            Directory.CreateDirectory("temp");

            foreach (var x in modCollection)
            {
                var box = (CheckBox) x;
                bool? isBoxChecked = null;
                var boxName = "";
                Dispatcher.BeginInvoke(DispatcherPriority.Input, new ThreadStart(() =>
                {
                    if (box.IsChecked != null && (bool) box.IsChecked)
                    {
                        isBoxChecked = true;
                        boxName = (string) box.Content;
                    }
                    else
                    {
                        isBoxChecked = false;
                        boxName = "blah";
                    }
                }));

                while (isBoxChecked == null || String.IsNullOrEmpty(boxName))
                {
                }

                if (!(bool) isBoxChecked) continue;
                var amountOfPatches = 1;

                using (var reader = XmlReader.Create(Path.Combine("mods", boxName, "info.xml")))
                {
                    while (reader.Read())
                    {
                        if (!reader.IsStartElement()) continue;
                        switch (reader.Name)
                        {
                            case "files":
                                reader.Read();
                                amountOfPatches = Convert.ToInt32(reader.Value);
                                break;
                        }
                    }
                }

                for (var i = 0; i < amountOfPatches; i++)
                {
                    Patcher(boxName, i);
                }
            }

            foreach (var s in _reassembleLocations)
            {
                Repackage(s);
            }

            var copiedNames = new List<string>();

            foreach (var s in _reassembleLocations.Where(s => !copiedNames.Contains(s.FileName)))
            {
                copiedNames.Add(s.FileName);
                CopyToClient(s);
            }

            DeletePathWithLongFileNames(Path.GetFullPath("temp"));
        }


        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show(_wasPatched
                ? "LESs has been successfully patched into League of Legends!"
                : "LESs encountered errors during patching. However, some patches may still be applied.");
            PatchButton.IsEnabled = true;
            StatusLabel.Content = "Done patching!";
        }

        private void Patcher(string modName, int amountOfPatches)
        {
            var patchNumber = "";
            if (amountOfPatches >= 1)
                patchNumber = amountOfPatches.ToString(CultureInfo.InvariantCulture);

            var modDetails = File.ReadAllLines(Path.Combine("mods", modName, "patch" + patchNumber + ".txt"));
            var fileLocation = "null";
            var tryFindClass = "null";
            var traitToModify = "null";
            var isNewTrait = false;
            foreach (var s in modDetails)
            {
                if (s.StartsWith("#"))
                {
                    tryFindClass = s.Substring(1);
                }
                else if (s.StartsWith("@@@"))
                {
                    traitToModify = s.Substring(3);
                }
                else if (s.StartsWith("@+@"))
                {
                    traitToModify = s.Substring(3);
                    isNewTrait = true;
                }
                else if (s.StartsWith("~"))
                {
                    fileLocation = s.Substring(1);
                }
            }


            var filePart = fileLocation.Split('/');
            var fileName = filePart[filePart.Length - 1];

            var locationText = "";
            Dispatcher.BeginInvoke(DispatcherPriority.Input,
                new ThreadStart(() => { locationText = LocationTextbox.Text; }));

            while (String.IsNullOrEmpty(locationText))
            {
            }

            if (!Directory.Exists(Path.Combine("temp", fileLocation.Replace(".dat", ""))))
            {
                Directory.CreateDirectory(Path.Combine("temp", fileLocation.Replace(".dat", "")));

                var n = "";
                foreach (var s in filePart.Take(filePart.Length - 1))
                {
                    n = Path.Combine(n, s);
                    if (!Directory.Exists(Path.Combine(locationText, "LESsBackup", IntendedVersion, n)))
                    {
                        Directory.CreateDirectory(Path.Combine(locationText, "LESsBackup", IntendedVersion, n));
                    }
                }
                if (!File.Exists(Path.Combine(locationText, "LESsBackup", IntendedVersion, fileLocation)))
                {
                    File.Copy(Path.Combine(locationText, fileLocation),
                        Path.Combine(locationText, "LESsBackup", IntendedVersion, fileLocation));
                }

                File.Copy(Path.Combine(locationText, fileLocation),
                    Path.Combine("temp", fileLocation.Replace(".dat", ""), fileName));

                Dispatcher.BeginInvoke(DispatcherPriority.Input,
                    new ThreadStart(() => { StatusLabel.Content = "Exporting patch " + modName; }));


                var export = new ProcessStartInfo
                {
                    FileName = "abcexport.exe",
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    Arguments = Path.Combine("temp", fileLocation.Replace(".dat", ""), fileName)
                };
                var exportProc = Process.Start(export);
                if (exportProc != null)
                {
                    exportProc.WaitForExit();
                }

                Dispatcher.BeginInvoke(DispatcherPriority.Input,
                    new ThreadStart(() => { StatusLabel.Content = "Disassembling patch (" + modName + ")"; }));

                var abcFiles = Directory.GetFiles(Path.Combine("temp", fileLocation.Replace(".dat", "")), "*.abc");


                foreach (var disasmProc in abcFiles.Select(s => new ProcessStartInfo
                {
                    FileName = "rabcdasm.exe",
                    Arguments = s,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }).Select(Process.Start).Where(disasmProc => disasmProc != null))
                {
                    disasmProc.WaitForExit();
                }
            }

            if (tryFindClass.IndexOf(':') == 0)
            {
                throw new Exception("Invalid mod " + modName);
            }

            var directories =
                Directory.GetDirectories(Path.Combine("temp", fileLocation.Replace(".dat", "")), "*",
                    SearchOption.AllDirectories).ToList();
            var searchFor = tryFindClass.Substring(0, tryFindClass.IndexOf(':'));
            var foundDirectories = new List<string>();
            foreach (var s in directories)
            {
                if (!s.Contains("com"))
                    continue;

                var tempS = s;
                tempS = tempS.Substring(tempS.IndexOf("com", StringComparison.Ordinal));
                tempS = tempS.Replace("\\", ".");
                if (tempS == searchFor)
                {
                    foundDirectories.Add(s);
                }
            }

            if (foundDirectories.Count == 0)
            {
                throw new Exception("No class matching " + searchFor + " for mod " + modName);
            }

            var finalDirectory = "";
            var Class = tryFindClass.Substring(tryFindClass.IndexOf(':')).Replace(":", "");
            foreach (var s in from s in foundDirectories
                let m = Directory.GetFiles(s)
                let x = Path.Combine(s, Class + ".class.asasm")
                where m.Contains(x)
                select s)
            {
                finalDirectory = s;
            }

            var classModifier = File.ReadAllLines(Path.Combine(finalDirectory, Class + ".class.asasm"));

            if (isNewTrait)
            {
                if (classModifier.Any(l => l == modDetails[3]))
                {
                    return;
                }
            }

            var traitStartPosition = 0;
            var traitEndLocation = 0;
            for (var i = 0; i < classModifier.Length; i++)
            {
                if (classModifier[i] != traitToModify) continue;
                traitStartPosition = i;
                break;
            }

            if (traitStartPosition == 0)
            {
                throw new Exception("Trait start location was not found! Corrupt mod?");
            }

            if (!isNewTrait)
            {
                for (var i = traitStartPosition; i < classModifier.Length; i++)
                {
                    if (classModifier[i].Trim() != "end ; method") continue;
                    if (classModifier[i + 1].Trim() == "end ; trait")
                    {
                        traitEndLocation = i + 2;
                    }
                    else
                    {
                        traitEndLocation = i + 1;
                    }
                    break;
                }

                if (traitEndLocation < traitStartPosition)
                {
                    throw new Exception("Trait end location was smaller than trait start location! " + traitEndLocation +
                                        ", " + traitStartPosition);
                }

                var startTrait = new string[traitStartPosition];
                Array.Copy(classModifier, startTrait, traitStartPosition);
                var afterTrait = new string[classModifier.Length - traitEndLocation];
                Array.Copy(classModifier, traitEndLocation, afterTrait, 0, classModifier.Length - traitEndLocation);

                var finalClass = new string[startTrait.Length + (modDetails.Length - 3) + afterTrait.Length];
                Array.Copy(startTrait, finalClass, traitStartPosition);
                Array.Copy(modDetails, 3, finalClass, traitStartPosition, (modDetails.Length - 3));
                Array.Copy(afterTrait, 0, finalClass, traitStartPosition + (modDetails.Length - 3), afterTrait.Length);

                File.Delete(Path.Combine(finalDirectory, Class + ".class.asasm"));
                File.WriteAllLines(Path.Combine(finalDirectory, Class + ".class.asasm"), finalClass);
            }
            else
            {
                var finalClass = new string[classModifier.Length + (modDetails.Length - 3)];
                Array.Copy(classModifier, 0, finalClass, 0, traitStartPosition);
                Array.Copy(modDetails, 3, finalClass, traitStartPosition, modDetails.Length - 3);
                Array.Copy(classModifier, traitStartPosition, finalClass, traitStartPosition + modDetails.Length - 3,
                    classModifier.Length - traitStartPosition);

                File.Delete(Path.Combine(finalDirectory, Class + ".class.asasm"));
                File.WriteAllLines(Path.Combine(finalDirectory, Class + ".class.asasm"), finalClass);
            }

            var h = new WorstHack
            {
                FileName = fileName,
                LocationText = locationText,
                ReAssembleLocation =
                    finalDirectory.Substring(0, finalDirectory.IndexOf("com", StringComparison.Ordinal))
                        .Replace("temp\\", ""),
                FileLocation = fileLocation
            };

            if (!_reassembleLocations.Contains(h))
                _reassembleLocations.Add(h);
        }

        private void Repackage(WorstHack data)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Input,
                new ThreadStart(() => { StatusLabel.Content = "Patching mods to client..."; }));

            var abcNumber =
                data.ReAssembleLocation.Substring(data.ReAssembleLocation.IndexOf('-'))
                    .Replace("-", "")
                    .Replace("\\", "");

            var reAsm = new ProcessStartInfo
            {
                FileName = "rabcasm.exe",
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                Arguments =
                    Path.Combine("temp",
                        data.ReAssembleLocation + data.FileName.Replace(".dat", "") + "-" + abcNumber + ".main.asasm")
            };
            var reAsmProc = Process.Start(reAsm);
            if (reAsmProc != null)
            {
                reAsmProc.WaitForExit();
            }

            var doPatch = new ProcessStartInfo
            {
                FileName = "abcreplace.exe",
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                Arguments =
                    Path.Combine("temp", data.FileLocation.Replace(".dat", ""), data.FileName) + " " + abcNumber + " " +
                    Path.Combine("temp",
                        data.ReAssembleLocation + data.FileName.Replace(".dat", "") + "-" + abcNumber + ".main.abc")
            };
            var finalPatchProc = Process.Start(doPatch);
            if (finalPatchProc != null)
            {
                finalPatchProc.WaitForExit();
            }
        }

        private void CopyToClient(WorstHack data)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Input,
                new ThreadStart(() => { StatusLabel.Content = "Patched " + data.FileName + "!"; }));

            File.Copy(Path.Combine("temp", data.FileLocation.Replace(".dat", ""), data.FileName),
                Path.Combine(data.LocationText, data.FileLocation), true);
        }

        private static void DeletePathWithLongFileNames(string path)
        {
            var tmpPath = @"\\?\" + path;
            var fso = new Scripting.FileSystemObject();
            fso.DeleteFolder(tmpPath, true);
        }


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
            Ok.IsEnabled = false;
            Ok.Content = "Working...";
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
            Ok.Content = "Go";
            Ok.IsEnabled = true;
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
            Process.Start(new ProcessStartInfo {Arguments = "sagerun:1", FileName = "cleanmgr.exe"});
            HandlePmbUninstall();
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
            var flashPath = Path.Combine(AirPath, "Resources");

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
                        AirPath,
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
                    AirPath,
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
            if (Directory.Exists("RADS"))
            {
                if (Cg.IsChecked == true)
                {
                    AdvancedCopy(
                        "Cg.dll", _cgBinPath,
                        "solutions", "lol_game_client_sln", "deploy");
                }
                if (Cg1.IsChecked == true)
                {
                    AdvancedCopy(
                        "Cg.dll", _cgBinPath,
                        "projects", "lol_launcher", "deploy");
                }
                if (Cg2.IsChecked == true)
                {
                    AdvancedCopy(
                        "Cg.dll", _cgBinPath,
                        "projects", "lol_patcher", "deploy");
                }

                if (CgGl.IsChecked == true)
                {
                    AdvancedCopy(
                        "Cg.dll", _cgBinPath,
                        "solutions", "lol_game_client_sln", "deploy");
                }
                if (CgGl1.IsChecked == true)
                {
                    AdvancedCopy(
                        "CgGL.dll", _cgBinPath,
                        "projects", "lol_launcher", "deploy");
                }
                if (CgGl2.IsChecked == true)
                {
                    AdvancedCopy(
                        "CgGL.dll", _cgBinPath,
                        "projects", "lol_patcher", "deploy");
                }

                if (CgD3D9.IsChecked == true)
                {
                    AdvancedCopy(
                        "CgD3D9.dll", _cgBinPath,
                        "solutions", "lol_game_client_sln", "deploy");
                }
                if (CgD3D1.IsChecked == true)
                {
                    AdvancedCopy(
                        "CgD3D9.dll", _cgBinPath,
                        "projects", "lol_launcher", "deploy");
                }
                if (CgD3D2.IsChecked == true)
                {
                    AdvancedCopy(
                        "CgD3D9.dll", _cgBinPath,
                        "projects", "lol_patcher", "deploy");
                }
            }
            else if (Directory.Exists("Game"))
            {
                if (Cg.IsChecked == true)
                {
                    Copy("Cg.dll",
                        _cgBinPath,
                        "Game");
                }

                if (CgGl.IsChecked == true)
                {
                    Copy("CgGL.dll",
                        _cgBinPath,
                        "Game");
                }

                if (CgD3D9.IsChecked == true)
                {
                    Copy("CgD3D9.dll",
                        _cgBinPath,
                        "Game");
                }
            }
        }


        private static void HandlePmbUninstall()
        {
            var pmbUninstall = Path.Combine(Arch,
                "Pando Networks", "Media Booster", "uninst.exe");
            if (!File.Exists(pmbUninstall)) return;
            Process.Start(new ProcessStartInfo {FileName = pmbUninstall, Arguments = "/silent"});
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
                if (File.Exists(Path.Combine(GameCfg, "game.cfg")))
                {
                    Copy("game.cfg", "Backup", GameCfg);
                }
                if (File.Exists(Path.Combine(GameCfg, "GamePermanent")))
                {
                    Copy("GamePermanent.cfg", "Backup", GameCfg);
                }
                if (File.Exists(Path.Combine(GameCfg, "GamePermanent_zh_MY.cfg")))
                {
                    Copy("GamePermanent_zh_MY.cfg", "Backup", GameCfg);
                }
                if (File.Exists(Path.Combine(GameCfg, "GamePermanent_en_SG.cfg")))
                {
                    Copy("GamePermanent_en_SG.cfg", "Backup", GameCfg);
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
            if (!File.Exists(Path.Combine(AirPath, "Adobe AIR.dll")))
                InstallAir();
            else
            {
                {
                    var currentVersion =
                        new Version(FileVersionInfo.GetVersionInfo(Path.Combine(AirPath, "Adobe AIR.dll")).FileVersion);
                    var latestVersion = new Version("14.0.0.178");

                    if (currentVersion >= latestVersion) return;
                    if (
                        MessageBox.Show(
                            "The Adobe Air version which is installed on your computer is outdated. Do you want to update it to ensure greater performance gains in the LoL client?",
                            "LoLUpdater", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        InstallAir();
                    }
                }
            }
        }

        private static void InstallAir()
        {
            if (MessageBox.Show(
                "We are unable to include any Adobe products, HOWEVER, you are fully capable of installing it yourself. Click yes to download and run the installer then apply the patch.",
                "LoLUpdater", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Process.Start("http://airdownload.adobe.com/air/win/download/14.0/AdobeAIRInstaller.exe");
            }
        }

        private static void HandleMouseHz()
        {
            var win8Version = new Version(6, 2, 9200, 0);
            if (Environment.OSVersion.Platform != PlatformID.Win32NT || Environment.OSVersion.Version < win8Version)
                return;


            var mousehz = Registry.LocalMachine.CreateSubKey(Path.Combine("SOFTWARE", Reg, "Microsoft",
                "Windows NT", "CurrentVersion", "AppCompatFlags", "Layers"));
            if (mousehz != null)
                mousehz.SetValue("NoDTToDITMouseBatch",
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "explorer.exe"),
                    RegistryValueKind.String);
            Process.Start(new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Verb = "runas",
                Arguments = "/C Rundll32 apphelp.dll,ShimFlushCache"
            });
        }

        private static void HandleCfg(string setting)
        {
            if (File.Exists(Path.Combine("Config", "game.cfg")))
            {
                var fi = new FileInfo(Path.Combine("Config", "game.cfg"));

                if (fi.Attributes == FileAttributes.ReadOnly)
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
            else if (File.Exists(Path.Combine(GameCfg, "game.cfg")))
            {
                Cfg(setting, "game.cfg");
                Cfg(setting, "GamePermanent.cfg");
                Cfg(setting, "GamePermanent_zh_MY.cfg");
                Cfg(setting, "GamePermanent_en_SG.cfg");
            }
        }

        private static void Cfg(string setting, string file)
        {
            if (!File.Exists(Path.Combine(GameCfg, file)))
            {
                var fi = new FileInfo(Path.Combine(GameCfg, file));
                if (FileAttributes.ReadOnly == fi.Attributes)
                {
                    MessageBox.Show(String.Format(
                        @"Your {0} Located in Game\DATA\CFG\defaults is read only, please remove this and try again",
                        file),
                        "LoLUpdater");
                    return;
                }
                if (
                    !File.ReadAllText(Path.Combine(GameCfg, file))
                        .Contains(setting))
                {
                    File.AppendAllText(Path.Combine(GameCfg, file),
                        Environment.NewLine + setting);
                }
            }
        }


        private void Cg_Checked(object sender, RoutedEventArgs e)
        {
            if (_cgBinPath == null || !File.Exists(Path.Combine(_cgBinPath, "cg.dll")))
            {
                InstallCg();

                _cgBinPath = Environment.GetEnvironmentVariable("CG_BIN_PATH", EnvironmentVariableTarget.User);
            }
            else
            {
                var currentVersion =
                    new Version(FileVersionInfo.GetVersionInfo(Path.Combine(_cgBinPath, "cg.dll")).FileVersion);
                var latestVersion = new Version("3.1.0013");
                if (
                    currentVersion < latestVersion &&
                    MessageBox.Show("You already have Nvdia CG installed. Do you want to update it?", "LoLUpdater",
                        MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    InstallCg();
                }
            }
        }

        private static void InstallCg()
        {
            Process.Start("NvidiaCGLicence.txt");
            if (MessageBox.Show("By clicking Yes you agree to NvidiaCGs Licence", "LoLUpdater",
                MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
            {
                return;
            }

            var cg = new Process
            {
                StartInfo = new ProcessStartInfo {FileName = "Cg_3_1_April2012_Setup.exe", Arguments = "/silent"}
            };
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
            Close();
        }

        private void Xminimize_MouseDown(object sender, MouseButtonEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
    }

    public class WorstHack
    {
        public string ReAssembleLocation { get; set; }
        public string FileName { get; set; }
        public string LocationText { get; set; }
        public string FileLocation { get; set; }
    }
}