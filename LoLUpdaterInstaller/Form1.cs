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

            if (IntPtr.Size == 8)
            {
                if (Registry.LocalMachine.OpenSubKey(@"SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\v4\\Client", false).GetValue("Version").ToString() != "4.5.51650")
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
                if (Registry.LocalMachine.OpenSubKey(@"SOFTWARE\\WoW632Node\\Microsoft\\NET Framework Setup\\NDP\\v4\\Client", false).GetValue("Version").ToString() != "4.5.51650")
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
            if (folderBrowserDialogGarena.SelectedPath != null)
            {
                System.IO.File.WriteAllBytes(folderBrowserDialogGarena.SelectedPath + @"\LoLUpdater.exe", Properties.Resources.LoLUpdater);
                System.IO.File.WriteAllBytes(folderBrowserDialogGarena.SelectedPath + @"\Interop.WUApiLib.dll", Properties.Resources.Interop_WUApiLib);
                WshShellClass shell = new WshShellClass();
                IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Garena LoLUpdater.lnk");
                shortcut.TargetPath = folderBrowserDialogGarena.SelectedPath + "LoLUpdater.exe";
                shortcut.Description = "LoL Patcher";
                shortcut.Save();
            }
            if (folderBrowserDialogGarenaPH.SelectedPath != null)
            {
                System.IO.File.WriteAllBytes(folderBrowserDialogGarenaPH.SelectedPath + @"\LoLUpdater.exe", Properties.Resources.LoLUpdater);
                System.IO.File.WriteAllBytes(folderBrowserDialogGarenaPH.SelectedPath + @"\Interop.WUApiLib.dll", Properties.Resources.Interop_WUApiLib);
                WshShellClass shell = new WshShellClass();
                IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\GarenaPH LoLUpdater.lnk");
                shortcut.TargetPath = folderBrowserDialogGarenaPH.SelectedPath + "LoLUpdater.exe";
                shortcut.Description = "LoL Patcher";
                shortcut.Save();
            }
            if (folderBrowserDialogLoL.SelectedPath != null)
            {
                System.IO.File.WriteAllBytes(folderBrowserDialogLoL.SelectedPath + @"\LoLUpdater.exe", Properties.Resources.LoLUpdater);
                System.IO.File.WriteAllBytes(folderBrowserDialogLoL.SelectedPath + @"\Interop.WUApiLib.dll", Properties.Resources.Interop_WUApiLib);
                WshShellClass shell = new WshShellClass();
                IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\LoLUpdater.lnk");
                shortcut.TargetPath = folderBrowserDialogLoL.SelectedPath + "LoLUpdater.exe";
                shortcut.Description = "LoL Patcher";
                shortcut.Save();
            }
            MessageBox.Show("Desktop Icon(s) have been created!", "LoLUpdater Installer");
        }
        private void GarenaBrowseButton_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialogGarena.ShowDialog() == DialogResult.OK)
            {
                label1.Text = folderBrowserDialogGarena.SelectedPath;
            }
        }
        private void GarenaPHBrowseButton_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialogGarenaPH.ShowDialog() == DialogResult.OK)
            {
                label2.Text = folderBrowserDialogGarenaPH.SelectedPath;
            }
        }

        private void LoLBrowseButton_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialogLoL.ShowDialog() == DialogResult.OK)
            {
                label3.Text = folderBrowserDialogLoL.SelectedPath;
            }
        }
    }
}