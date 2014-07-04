using System;
using Microsoft.Win32;
using System.Diagnostics;
using System.Windows.Forms;
using IWshRuntimeLibrary;
namespace LoLUpdaterInstaller
{
    public partial class Form : System.Windows.Forms.Form
    {
        public Form()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Press OK and please wait.", "LoLUpdater Installer");
            if (Environment.OSVersion.Version.Major == 5 && Environment.OSVersion.Version.Minor == 1)
            {
                if (IntPtr.Size == 8)
                {
                    if (Registry.LocalMachine.OpenSubKey(@"SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\v4", false) == null)
                    {
                        System.IO.File.WriteAllBytes("NDP452-KB2901907-x86-x64-AllOS-ENU.exe", Properties.Resources.NDP452_KB2901907_x86_x64_AllOS_ENU);
                        Process net452 = new Process();
                        ProcessStartInfo startInfo = new ProcessStartInfo();
                        startInfo.FileName = "NDP452-KB2901907-x86-x64-AllOS-ENU.exe";
                        startInfo.Arguments = "/q";
                        net452.StartInfo = startInfo;
                        net452.Start();
                        net452.WaitForExit();
                        System.IO.File.Delete("NDP452-KB2901907-x86-x64-AllOS-ENU.exe");
                    }
                }
                else if (IntPtr.Size == 4)
                {
                    if (Registry.LocalMachine.OpenSubKey(@"SOFTWARE\\WoW6432Node\\Microsoft\\NET Framework Setup\\NDP\\v4", false) == null)
                    {
                        System.IO.File.WriteAllBytes("NDP452-KB2901907-x86-x64-AllOS-ENU.exe", Properties.Resources.NDP452_KB2901907_x86_x64_AllOS_ENU);
                        Process net452 = new Process();
                        ProcessStartInfo startInfo = new ProcessStartInfo();
                        startInfo.FileName = "NDP452-KB2901907-x86-x64-AllOS-ENU.exe";
                        startInfo.Arguments = "/q";
                        net452.StartInfo = startInfo;
                        net452.Start();
                        net452.WaitForExit();
                        System.IO.File.Delete("NDP452-KB2901907-x86-x64-AllOS-ENU.exe");
                    }
                }
            }
            else
            {
                if (IntPtr.Size == 8)
                {
                        if (Registry.LocalMachine.OpenSubKey(@"SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\v4\\Client", false) == null | Registry.LocalMachine.OpenSubKey(@"SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\v4\\Client", false).GetValue("Version").ToString() != "4.5.51650")
                    {
                        System.IO.File.WriteAllBytes("NDP452-KB2901907-x86-x64-AllOS-ENU.exe", Properties.Resources.NDP452_KB2901907_x86_x64_AllOS_ENU);
                        Process net452 = new Process();
                        ProcessStartInfo startInfo = new ProcessStartInfo();
                        startInfo.FileName = "NDP452-KB2901907-x86-x64-AllOS-ENU.exe";
                        startInfo.Arguments = "/q";
                        net452.StartInfo = startInfo;
                        net452.Start();
                        net452.WaitForExit();
                        System.IO.File.Delete("NDP452-KB2901907-x86-x64-AllOS-ENU.exe");
                    }
                }
                else if (IntPtr.Size == 4)
                {
                    if (Registry.LocalMachine.OpenSubKey(@"SOFTWARE\\WoW632Node\\Microsoft\\NET Framework Setup\\NDP\\v4\\Client", false).GetValue("Version") == null | Registry.LocalMachine.OpenSubKey(@"SOFTWARE\\WoW632Node\\Microsoft\\NET Framework Setup\\NDP\\v4\\Client", false).GetValue("Version").ToString() != "4.5.51650")
                    {
                        System.IO.File.WriteAllBytes("NDP452-KB2901907-x86-x64-AllOS-ENU.exe", Properties.Resources.NDP452_KB2901907_x86_x64_AllOS_ENU);
                        Process net452 = new Process();
                        ProcessStartInfo startInfo = new ProcessStartInfo();
                        startInfo.FileName = "NDP452-KB2901907-x86-x64-AllOS-ENU.exe";
                        startInfo.Arguments = "/q";
                        net452.StartInfo = startInfo;
                        net452.Start();
                        net452.WaitForExit();
                        System.IO.File.Delete("NDP452-KB2901907-x86-x64-AllOS-ENU.exe");
                    }
                }
            }
            if (Registry.LocalMachine.OpenSubKey(@"SOFTWARE\\WoW6432Node\\Garena\\LoL", false) != null)
            {
                if (Environment.OSVersion.Version.Major == 5 && Environment.OSVersion.Version.Minor == 1)
                {
                    System.IO.File.WriteAllBytes(Registry.LocalMachine.OpenSubKey(@"SOFTWARE\\WoW6432Node\\Garena\\LoL", false).GetValue("Path") + @"\LoLUpdaterXP.exe", Properties.Resources.LoLUpdaterXP);
                    WshShellClass shell = new WshShellClass();
                    IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Garena LoLUpdater.lnk");
                    shortcut.TargetPath = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\\WoW6432Node\\Garena\\LoL", false).GetValue("Path") + @"\LoLUpdaterXP.exe";
                    shortcut.Description = "LoL Patcher";
                    shortcut.Save();
                }
                else
                {
                    System.IO.File.WriteAllBytes(Registry.LocalMachine.OpenSubKey(@"SOFTWARE\\WoW6432Node\\Garena\\LoL", false).GetValue("Path") + @"\LoLUpdater.exe", Properties.Resources.LoLUpdaterXP);
                    System.IO.File.WriteAllBytes(Registry.LocalMachine.OpenSubKey(@"SOFTWARE\\WoW6432Node\\Garena\\LoL", false).GetValue("Path") + @"\Interop.WUApiLib.dll", Properties.Resources.Interop_WUApiLib);
                    WshShellClass shell = new WshShellClass();
                    IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Garena LoLUpdater.lnk");
                    shortcut.TargetPath = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\\WoW6432Node\\Garena\\LoL", false).GetValue("Path") + @"\LoLUpdater.exe";
                    shortcut.Description = "LoL Patcher";
                    shortcut.Save();
                }
            }
            if (Registry.LocalMachine.OpenSubKey(@"SOFTWARE\\WoW6432Node\\Garena\\LoLPH", false) != null)
            {
                if (Environment.OSVersion.Version.Major == 5 && Environment.OSVersion.Version.Minor == 1)
                {
                    System.IO.File.WriteAllBytes(Registry.LocalMachine.OpenSubKey(@"SOFTWARE\\WoW6432Node\\Garena\\LoLPH", false).GetValue("Path") + @"\LoLUpdaterXP.exe", Properties.Resources.LoLUpdaterXP);
                    WshShellClass shell = new WshShellClass();
                    IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\GarenaPH LoLUpdater.lnk");
                    shortcut.TargetPath = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\\WoW6432Node\\Garena\\LoLPH", false).GetValue("Path") + @"\LoLUpdaterXP.exe";
                    shortcut.Description = "LoL Patcher";
                    shortcut.Save();
                }
                else
                {
                    System.IO.File.WriteAllBytes(Registry.LocalMachine.OpenSubKey(@"SOFTWARE\\WoW6432Node\\Garena\\LoLPH", false).GetValue("Path") + @"\LoLUpdater.exe", Properties.Resources.LoLUpdater);
                    System.IO.File.WriteAllBytes(Registry.LocalMachine.OpenSubKey(@"SOFTWARE\\WoW6432Node\\Garena\\LoLPH", false).GetValue("Path") + @"\Interop.WUApiLib.dll", Properties.Resources.Interop_WUApiLib);
                    WshShellClass shell = new WshShellClass();
                    IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\GarenaPH LoLUpdater.lnk");
                    shortcut.TargetPath = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\\WoW6432Node\\Garena\\LoLPH", false).GetValue("Path") + @"\LoLUpdater.exe";
                    shortcut.Description = "LoL Patcher";
                    shortcut.Save();
                }
            }
            if (Registry.LocalMachine.OpenSubKey(@"SOFTWARE\\WoW6432Node\\Riot Games\\League of Legends", false) != null)
            {
                if (Environment.OSVersion.Version.Major == 5 && Environment.OSVersion.Version.Minor == 1)
                {
                    System.IO.File.WriteAllBytes(Registry.LocalMachine.OpenSubKey(@"SOFTWARE\\WoW64Node\\Riot Games\\League of Legends", false).GetValue("Path") + @"\LoLUpdaterXP.exe", Properties.Resources.LoLUpdaterXP);
                    WshShellClass shell = new WshShellClass();
                    IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\LoLUpdater.lnk");
                    shortcut.TargetPath = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\\WoW6432Node\\Riot Games\\League of Legends", false).GetValue("Path") + @"\LoLUpdaterXP.exe";
                    shortcut.Description = "LoL Patcher";
                    shortcut.Save();
                }
                else
                {
                    System.IO.File.WriteAllBytes(Registry.LocalMachine.OpenSubKey(@"SOFTWARE\\WoW6432Node\\Riot Games\\League of Legends", false).GetValue("Path") + @"\LoLUpdater.exe", Properties.Resources.LoLUpdater);
                    System.IO.File.WriteAllBytes(Registry.LocalMachine.OpenSubKey(@"SOFTWARE\\WoW6432Node\\Riot Games\\League of Legends", false).GetValue("Path") + @"\Interop.WUApiLib.dll", Properties.Resources.Interop_WUApiLib);
                    WshShellClass shell = new WshShellClass();
                    IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\LoLUpdater.lnk");
                    shortcut.TargetPath = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\\WoW6432Node\\Riot Games\\League of Legends", false).GetValue("Path") + @"\LoLUpdater.exe";
                    shortcut.Description = "LoL Patcher";
                    shortcut.Save();
                }
            }
            MessageBox.Show("Desktop Icon(s) have been created!", "LoLUpdater Installer");
        }
    }
}

