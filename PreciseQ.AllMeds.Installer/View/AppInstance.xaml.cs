using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Button = System.Windows.Controls.Button;

namespace PreciseQ.AllMeds.Installer.View
{
    /// <summary>
    /// Interaction logic for AppInstance.xaml
    /// </summary>
    public partial class AppInstance : Window
    {
        public AppInstance()
        {
            InitializeComponent();
        }
        private void ButtonOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void ButtonEdiSettings_Click(object sender, RoutedEventArgs e)
        {
            Button theButton = (Button) sender;
            ShowSftSettings(theButton.DataContext);
        }

        private void ButtonEhrSettings_Click(object sender, RoutedEventArgs e)
        {
            Button theButton = (Button)sender;
            ShowSftSettings(theButton.DataContext);
        }

        private void ShowSftSettings(object dataContext)
        {
            SftpSettingsWindow window = new SftpSettingsWindow();

            window.DataContext = dataContext;
            window.Owner = this;
            window.ShowDialog();
        }
    }
}
