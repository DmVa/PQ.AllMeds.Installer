using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PreciseQ.AllMeds.Installer.Setting;

namespace PreciseQ.AllMeds.Installer.Updater.Components
{
    public class DbUpdaterComponent : UpdateComponentBase
    {
        public DbUpdaterComponent(SiteInstance siteConfig, ApplicationInstance appplicationConfig) : base(siteConfig, appplicationConfig)
        {
        }

        protected override void ConfigureSettings()
        {
            ComponentRelativeFolder = "DB";
            ConfigFileName = "DbUpdater\\Updater.exe.config";
        }

        protected override void AddPredefinedConfigs()
        {
            AddPredefinedConfig(@"/configuration/DbUpdater", "ConnectionString", ApplicationConfig.ConnectionString);
        }

        public override void PostUpdateConfig()
        {
            RunSqlUpdater();
        }

        private void RunSqlUpdater()
        {
            string updaterFolder = Path.Combine(ComponentAbsoluteRootFolder, "DBUpdater");
            string fullFileName = Path.Combine(updaterFolder, @"Updater.exe");
            UpdateProgerss($"Run {fullFileName}");
            Process process = new Process();
            process.StartInfo.FileName = fullFileName;
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.WorkingDirectory = updaterFolder;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            process.Start();
            process.WaitForExit();
            UpdateProgerss($"execution {fullFileName} finished");
        }
    }
}
