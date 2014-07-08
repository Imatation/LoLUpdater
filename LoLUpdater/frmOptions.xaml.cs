using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LoLUpdater
{
	/// <summary>
	/// Interaction logic for frmOptions.xaml
	/// </summary>
	public partial class frmOptions : Window
	{
		public frmOptions()
		{
			this.InitializeComponent();

            chkDisableWarnings.IsChecked = Properties.Settings.Default.disableWarnings;
		}

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.disableWarnings = (chkDisableWarnings.IsChecked == true);
            Properties.Settings.Default.Save();
            this.Close();
        }
	}
}