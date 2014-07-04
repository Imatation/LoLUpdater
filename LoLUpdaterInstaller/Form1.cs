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
            if (folderBrowserDialog1.SelectedPath != null)
            {
                System.IO.File.WriteAllBytes(folderBrowserDialog1.SelectedPath + @"\LoLUpdater.exe", Properties.Resources.LoLUpdater);
                System.IO.File.WriteAllBytes(folderBrowserDialog1.SelectedPath + @"\Interop.WUApiLib.dll", Properties.Resources.Interop_WUApiLib);
                WshShellClass shell = new WshShellClass();
                IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Garena LoLUpdater.lnk");
                shortcut.TargetPath = folderBrowserDialog1.SelectedPath + "LoLUpdater.exe";
                shortcut.Description = "LoL Patcher";
                shortcut.Save();
            }
            if (folderBrowserDialog2.SelectedPath != null)
            {
                System.IO.File.WriteAllBytes(folderBrowserDialog2.SelectedPath + @"\LoLUpdater.exe", Properties.Resources.LoLUpdater);
                System.IO.File.WriteAllBytes(folderBrowserDialog2.SelectedPath + @"\Interop.WUApiLib.dll", Properties.Resources.Interop_WUApiLib);
                WshShellClass shell = new WshShellClass();
                IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\GarenaPH LoLUpdater.lnk");
                shortcut.TargetPath = folderBrowserDialog2.SelectedPath + "LoLUpdater.exe";
                shortcut.Description = "LoL Patcher";
                shortcut.Save();
            }
            if (folderBrowserDialog3.SelectedPath != null)
            {

                System.IO.File.WriteAllBytes(folderBrowserDialog3.SelectedPath + @"\LoLUpdater.exe", Properties.Resources.LoLUpdater);
                System.IO.File.WriteAllBytes(folderBrowserDialog3.SelectedPath + @"\Interop.WUApiLib.dll", Properties.Resources.Interop_WUApiLib);
                WshShellClass shell = new WshShellClass();
                IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\LoLUpdater.lnk");
                shortcut.TargetPath = folderBrowserDialog3.SelectedPath + "LoLUpdater.exe";
                shortcut.Description = "LoL Patcher";
                shortcut.Save();
            }
            MessageBox.Show("Desktop Icon(s) have been created!", "LoLUpdater Installer");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                label1.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (folderBrowserDialog2.ShowDialog() == DialogResult.OK)
            {
                label2.Text = folderBrowserDialog2.SelectedPath;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog3.ShowDialog() == DialogResult.OK)
            {
                label3.Text = folderBrowserDialog3.SelectedPath;
            }
        }
    }
}