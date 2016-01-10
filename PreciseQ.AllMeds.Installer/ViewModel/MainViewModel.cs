using PreciseQ.AllMeds.Installer.Base;
using PreciseQ.AllMeds.Installer.Service;
using PreciseQ.AllMeds.Installer.Settings;
using PreciseQ.AllMeds.Installer.Updater;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PreciseQ.AllMeds.Installer.ViewModel
{
    public class MainViewModel : BasePropertyChanged
    {
        private SiteInstance _siteConfig = null;
        private string _updateLogText = string.Empty;
        private bool _isUpdateInProgress = false;
        private BaseCommand _saveSettingsCommand = null;
        private BaseCommand _doUpdateCommand = null;
        private BaseCommand _baseUICommand = null;
        
        private List<BaseCommand> _commandsList;

        public MainViewModel()
        {
            CreateCommands();
            Initialize();
        }
        public string UpdateLogText
        {
            get { return _updateLogText; }
            set
            {
                _updateLogText = value;
                RaisePropertyChanged(nameof(UpdateLogText));
            }
        }

        public SiteInstance SiteConfig
        {
            get { return _siteConfig; }
        }
        #region "Commands"
        public ICommand SaveCommand
        {
            get { return _saveSettingsCommand; }
        }
        public ICommand DoUpdateCommand
        {
            get { return _doUpdateCommand; }
        }
        public ICommand BaseUICommand
        {
            get { return _baseUICommand; }
        }
        #endregion

        private void CreateCommands()
        {
            _commandsList = new List<BaseCommand>();
            _saveSettingsCommand = new BaseCommand(SaveSettings, CanRunCommands);
            _commandsList.Add(_saveSettingsCommand);
            _doUpdateCommand = new BaseCommand(DoUpdate, CanRunCommands);
            _commandsList.Add(_doUpdateCommand);
            _baseUICommand = new BaseCommand(ExecuteBaseUI, CanRunCommands);
            _commandsList.Add(_baseUICommand);
        }

        private bool CanRunCommands()
        {
            return !_isUpdateInProgress;
        }

        public bool IsUpdateInProgress
        {
            get { return _isUpdateInProgress; }
            set
            {
                _isUpdateInProgress = value;
                foreach (var cmd in _commandsList)
                    cmd.RaiseCanExecuteChanged(null);
            }
        }

        private void SaveSettings()
        {
            SettingsService.Current.SaveSettings(SiteConfig);
        }

        private void DoUpdate()
        {
            IsUpdateInProgress = true;
            UpdateLogText = string.Empty;
            try
            {
                SaveSettings();
                var bw = new BackgroundWorker();
                bw.WorkerReportsProgress = true;
                bw.DoWork += (sender, args) =>
                {
                    var updater = new UpdateManager(_siteConfig, ConfigService.Current.Logger());
                    updater.UpdateProgress += (s, e) => bw.ReportProgress(0, e);
                    updater.Update();
                };

                bw.ProgressChanged += Update_ProgressChanged;
                bw.RunWorkerCompleted += (s, e) =>
                {
                    IsUpdateInProgress = false;
                };

                IsUpdateInProgress = true;
                bw.RunWorkerAsync();
            }
            finally
            {
                IsUpdateInProgress = false;
            }

        }

        private void Update_ProgressChanged(object sender, ProgressChangedEventArgs progressArgs)
        {
            UpdateProgressEventArgs e = (UpdateProgressEventArgs)progressArgs.UserState;
            UpdateLogText += e.Message + Environment.NewLine;
        }

        private void ExecuteBaseUI()
        {
            //do nothing, command execute in UI.
        }

        private void Initialize()
        {
            _siteConfig = SettingsService.Current.GetSettings();
            if (_siteConfig == null)
                _siteConfig = SettingsService.Current.GetDefault();
            RaisePropertyChanged(nameof(SiteConfig));
        }
    }
}
