using PreciseQ.AllMeds.Installer.Settings;
using PreciseQ.AllMeds.Installer.View;
using PreciseQ.AllMeds.Installer.ViewModel;
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

namespace PreciseQ.AllMeds.Installer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainViewModel _model;
        public MainWindow()
        {
            _model = new MainViewModel();
            DataContext = _model;
            InitializeComponent();
        }

        public ApplicationInstance Selected
        {
            get { return InstancesList.SelectedItem as ApplicationInstance; }
            set { InstancesList.SelectedItem = value; }
        }

        private void buttonAddSetting_Click(object sender, RoutedEventArgs e)
        {
            ApplicationInstance model = new ApplicationInstance();
            AppInstance window = new AppInstance();
            window.DataContext = model;
            window.Owner = this;
            window.ShowDialog();
            if (window.DialogResult == true)
            {
                _model.SiteConfig.Instances.Add(model);
            }
        }

        private void buttonCloneSetting_Click(object sender, RoutedEventArgs e)
        {
            CloneAndEditSetting(true);
        }

        private void CloneAndEditSetting(bool makeClone)
        {
            if (Selected == null)
            {
                MessageBox.Show("Select configuration", "Not selected", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            int selectedIndex = InstancesList.SelectedIndex;

            AppInstance window = new AppInstance();
            var selectedItem = Selected;
            ApplicationInstance settingsCopy = Selected.MakeCopy();

            window.DataContext = settingsCopy;
            window.Owner = this;
            window.ShowDialog();
            if (window.DialogResult == true)
            {
                if (makeClone)
                    _model.SiteConfig.Instances.Add(settingsCopy);
                else
                    _model.SiteConfig.Instances[selectedIndex] = settingsCopy;
            }
        }


        private void buttonEditSetting_Click(object sender, RoutedEventArgs e)
        {
            CloneAndEditSetting(false);
        }

        private void buttonDeleteApplication_Click(object sender, RoutedEventArgs e)
        {
            if (Selected == null)
            {
                MessageBox.Show("Select configuration", "Not selected", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            MessageBoxResult result = MessageBox.Show("Do you want to delete this configuration?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                var selectedItem = Selected;
                _model.SiteConfig.Instances.Remove(selectedItem);
            }
        }
    }
}
