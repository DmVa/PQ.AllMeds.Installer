using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using System.IO;
using PreciseQ.AllMeds.Installer.Updater;
using System.Diagnostics;
using PreciseQ.AllMeds.Installer.Setting;
using PreciseQ.AllMeds.Installer.Updater.Components;

namespace PreciseQ.AllMeds.Installer.Updater
{
    public class UpdateManager
    {
        private SiteInstance _settings;
        public ILog _log;
        public UpdateManager(SiteInstance settings, ILog log)
        {
            _log = log;
            if (_log == null)
                throw new ArgumentNullException("log");
            _settings = settings;
            if (settings == null)
                throw new ArgumentNullException("settings");
        }

        public event EventHandler<UpdateProgressEventArgs> UpdateProgress;

        public void Update()
        {
            string logMsg = String.Format("Start update at {0}", DateTime.Now);
            RaiseUpdateProgerss(UpdateProgressStatus.None, logMsg);
            _log.Info(logMsg);
            try
            {
                foreach(ApplicationInstance appInstance in _settings.Instances)
                {
                    if (appInstance.IsChecked)
                        UpdateInstance(appInstance);
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
                RaiseUpdateProgerss(UpdateProgressStatus.Error, string.Format("Error {0}", ex.Message));
            }

            RaiseUpdateProgerss(UpdateProgressStatus.Completed, "Finished");
        }

        private void UpdateInstance(ApplicationInstance appInstance)
        {
            string rootFolder = _settings.RepositoryFolder;
            if (!Directory.Exists(rootFolder))
            {
                throw new ApplicationException($"Directory {rootFolder} does not exists");
            }

            if (string.IsNullOrEmpty(_settings.InstancesRootFolder))
            {
                throw new ApplicationException("Instances RootFolder is not defined");
            }

            RaiseUpdateProgerss($"Update {appInstance.VirtualApplicationName}");
            List<UpdateComponentBase> components = CreateUpdateComponentsList(appInstance);

            foreach (var component in components)
                component.PreCopySources();

            foreach (var component in components)
                component.CopySources();

            foreach (var component in components)
                component.PostCopySources();

            foreach (var component in components)
                component.UpdateConfig();

            foreach (var component in components)
                component.PostUpdateConfig();

            foreach (var component in components)
                component.PrepareStart();

        }

        private void RunSqlUpdater(string instanceFolder)
        {
            string updaterFolder = Path.Combine(instanceFolder, "DB\\DBUpdater");
            string fullFileName = Path.Combine(updaterFolder, @"Updater.exe");
            Process process = new Process();
            process.StartInfo.FileName = fullFileName;
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.WorkingDirectory = updaterFolder;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            process.Start();
            process.WaitForExit();
        }

        private List<UpdateComponentBase> CreateUpdateComponentsList(ApplicationInstance appInstance)
        {
            List<UpdateComponentBase> components = new List<UpdateComponentBase>();
            if (appInstance.IsUpdateWorker)
            {
                var updateComponent = new WorkerComponent(_settings, appInstance);
                updateComponent.Init(RaiseUpdateProgerss);
                components.Add(updateComponent);
            }

            if (appInstance.IsUpdateSite)
            {
                var updateComponent = new WebSiteComponent(_settings, appInstance);
                updateComponent.Init(RaiseUpdateProgerss);
                components.Add(updateComponent);
            }

            if (appInstance.IsUpdateDb)
            {
                var updateComponent = new DbUpdaterComponent(_settings, appInstance);
                updateComponent.Init(RaiseUpdateProgerss);
                components.Add(updateComponent);
            }

            return components;
        }

        private void RaiseUpdateProgerss(string message)
        {
            _log.Info(message);
            RaiseUpdateProgerss(UpdateProgressStatus.None, message);
        }

        private void RaiseUpdateProgerss(UpdateProgressStatus status, string message)
        {
            if (UpdateProgress != null)
            {
                UpdateProgress(this, new UpdateProgressEventArgs(status, message));
            }
        }
    }
}
