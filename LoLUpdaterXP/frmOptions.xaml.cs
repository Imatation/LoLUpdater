using System.Windows;

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