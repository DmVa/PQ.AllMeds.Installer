using PreciseQ.AllMeds.Installer.Settings;
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

namespace PreciseQ.AllMeds.Installer.View
{
    /// <summary>
    /// Interaction logic for ConfigOverrideSettingControl.xaml
    /// </summary>
    public partial class ConfigOverrideSettingControl : UserControl
    {
        public ConfigOverrideSettingControl()
        {
            InitializeComponent();
        }
        public ICollection<ConfigOverride> Items
        {
            get { return ((ICollection<ConfigOverride>)DataContext); }
            set { DataContext = value; }
        }

        public ConfigOverride Selected
        {
            get { return ItemsList.SelectedItem as ConfigOverride; }
            set { ItemsList.SelectedItem = value; }
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (Selected == null)
            {
                MessageBox.Show("Select line", "Not selected", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            MessageBoxResult result = MessageBox.Show("Do you want to delete this line?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Items.Remove(Selected);
            }
        }
    }
}
