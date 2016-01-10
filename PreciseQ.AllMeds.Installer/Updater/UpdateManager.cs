using PreciseQ.AllMeds.Installer.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using System.IO;
using PreciseQ.AllMeds.Installer.Updater;
using System.Diagnostics;

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
            RaiseUpdateProgerss("Copy sources");
           
            string instanceFolder = CopySources(appInstance);
            RaiseUpdateProgerss("Copied, Update ConnectionStrings");
            List<UpdateFileConfigBase> components = CreateUpdateComponentsList(instanceFolder, appInstance);
            UpdateConnectionString(appInstance, components);
            RaiseUpdateProgerss("Updated, Update custom setting");
            UpdateCustomSettings(appInstance, components);
            RaiseUpdateProgerss("Updated, Update custom files");
            UpdateCustomFiles(instanceFolder, appInstance);
            RunSqlUpdater(instanceFolder);
            RaiseUpdateProgerss("Updated");
        }

        private void RunSqlUpdater(string instanceFolder)
        {
            string fullFileName = Path.Combine(instanceFolder, @"DB\DBUpdater\Updater.exe");
            Process process = new Process();
            process.StartInfo.FileName = fullFileName;
            //process.StartInfo.Arguments = "-n";
            process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            process.Start();
            process.WaitForExit();
        }

        private void UpdateCustomFiles(string instanceFolder, ApplicationInstance appInstance)
        {
            string fullFileName = Path.Combine(instanceFolder, "Worker\\Installworker.cmd");
            WorkerInstallCmdConverter.Apply(fullFileName, appInstance.VirtualApplicationName);
        }

        private List<UpdateFileConfigBase> CreateUpdateComponentsList(string instanceFolder, ApplicationInstance appInstance)
        {
            List<UpdateFileConfigBase> components = new List<UpdateFileConfigBase>();

            var dbUpdater = new UpdateFileConfigDbUpdater();
            dbUpdater.Init(instanceFolder);
            components.Add(dbUpdater);

            var site = new UpdateFileConfigWebSite();
            site.Init(instanceFolder);
            site.CustomConfigOverride = appInstance.SiteConfig;
            components.Add(site);

            var worker = new UpdateFileConfigWorker();
            worker.Init(instanceFolder);
            worker.CustomConfigOverride = appInstance.WorkerConfig;
            components.Add(worker);

            return components;
        }

        private void UpdateConnectionString(ApplicationInstance appInstance, List<UpdateFileConfigBase> components)
        {
            foreach(var component in components)
            {
                component.SetPredefinedConfigOverride(appInstance.ConnectionString);
                ConfigConverter.Apply(component.FullFileFileName, component.PredefinedConfigOverride);
            }
        }
        private void UpdateCustomSettings(ApplicationInstance appInstance, List<UpdateFileConfigBase> components)
        {
            foreach (var component in components)
            {
                ConfigConverter.Apply(component.FullFileFileName, component.CustomConfigOverride);
            }
        }

        private string CopySources(ApplicationInstance appInstance)
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

            string instanceFolder = Path.Combine(_settings.InstancesRootFolder, appInstance.VirtualApplicationName);

            DirectoryInfo sourceDir = new DirectoryInfo(rootFolder);
            DirectoryInfo targetDir = new DirectoryInfo(instanceFolder);
            
            CopyFiles(sourceDir, targetDir);
            return instanceFolder;
        }

        private void CopyFiles(DirectoryInfo sourceDir, DirectoryInfo targetDir)
        {
            if (!targetDir.Exists)
                targetDir.Create();

            System.IO.FileInfo[]  files = sourceDir.GetFiles("*.*");
            foreach (System.IO.FileInfo sourceFile in files)
            {
                if (IsValidToCopy(sourceFile))
                {
                    string destFileName = GetDestFileName(sourceFile, targetDir);
                    sourceFile.CopyTo(destFileName, true);
                }
            }

            // First, process all the files directly under this folder
            System.IO.DirectoryInfo[]  subDirs = sourceDir.GetDirectories();
            foreach (System.IO.DirectoryInfo dirInfo in subDirs)
            {
                if (IsValidToCopy(dirInfo))
                {
                    DirectoryInfo targetSubDir = GetDestDir(dirInfo.Name, targetDir);
                    CopyFiles(dirInfo, targetSubDir);
                }
            }
        }

        private string GetDestFileName(FileInfo sourceFile, DirectoryInfo targetDir)
        {
            string shortFileName = sourceFile.Name;
            string destFileName = Path.Combine(targetDir.FullName, shortFileName);
            return destFileName; 
        }

        private DirectoryInfo GetDestDir(string subName, DirectoryInfo targetDir)
        {
            string dirName = targetDir.FullName;
            string destDir = Path.Combine(dirName, subName);
            return new DirectoryInfo(destDir);
        }

        private bool IsValidToCopy(DirectoryInfo dirInfo)
        {
            string dirName = dirInfo.Name;
            if (string.Compare(dirName, ".svn", StringComparison.InvariantCultureIgnoreCase) == 0)
                return false;
            if (string.Compare(dirName, ".git", StringComparison.InvariantCultureIgnoreCase) == 0)
                return false;
            return true;
        }
        private bool IsValidToCopy(FileInfo fileInfo)
        {
            return true;
        }

        private void RaiseUpdateProgerss(string message)
        {
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
