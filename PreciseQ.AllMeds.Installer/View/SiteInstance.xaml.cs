using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using PreciseQ.AllMeds.Installer.Utils;

namespace PreciseQ.AllMeds.Installer.View
{
    /// <summary>
    /// Interaction logic for SiteInstance.xaml
    /// </summary>
    public partial class SiteInstance : System.Windows.Controls.UserControl
    {
        public SiteInstance()
        {
            InitializeComponent();
        }

        private void GetFolder(System.Windows.Controls.TextBox textBoxControl)
        {
            var dlg = new FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dlg.ShowDialog(this.GetIWin32Window());
            if (result == DialogResult.OK)
            {
                textBoxControl.Text = dlg.SelectedPath;
            }
        }

        private void btnRepositoryRootFolderClick(object sender, RoutedEventArgs e)
        {
            GetFolder(ReposotoryFolderControl);
        }

        private void btnInstancesRootFolderClick(object sender, RoutedEventArgs e)
        {
            GetFolder(InstancesFolderControl);
        }

        private void btnSftpRootFolderClick(object sender, RoutedEventArgs e)
        {

            GetFolder(SftpRootFolderFolderControl);
        }
    }
}
