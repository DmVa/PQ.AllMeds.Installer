using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using PreciseQ.AllMeds.Installer.Setting;

namespace PreciseQ.AllMeds.Installer.Updater.Components
{
    public class WorkerComponent : UpdateComponentBase
    {
        protected string ServiceName { get; set; }
        public WorkerComponent(SiteInstance siteConfig, ApplicationInstance appplicationConfig) : base(siteConfig, appplicationConfig)
        {
        }

        protected override void ConfigureSettings()
        {
            ComponentRelativeFolder = "Worker";
            ConfigFileName = "PreciseQ.AllMeds.TaskService.exe.config";
            ServiceName = "PreciseQ.AllMeds.TaskService." + ApplicationConfig.VirtualApplicationName;
            if (ApplicationConfig.WorkerConfig != null)
            {
                CustomConfigOverride = ApplicationConfig.WorkerConfig.ToList();
            }
        }

        protected override void AddPredefinedConfigs()
        {
            string connectionString = ApplicationConfig.ConnectionString;
            string allMedsConn = connectionString.TrimEnd(';') + ";MultipleActiveResultSets=True;App = EntityFramework";

            AddPredefinedConfig(@"/configuration/connectionStrings/add[@name='AllMedsContext']", "connectionString", allMedsConn);
            AddSftpRootConfig();
            AddEdiSftpConfig();
            AddEhrSftpConfig();

            string mainSiteRootFolder = Path.Combine(SiteConfig.InstancesRootFolder,
                ApplicationConfig.VirtualApplicationName);
            AddPredefinedConfig($"/configuration/appSettings/add[@key='hl7.import.MainSiteRootFolder']", "value", mainSiteRootFolder);
        }

        public override void PreCopySources()
        {
            StopService();
        }
        public override void PrepareStart()
        {
            StartService();
        }

        public override void PostCopySources()
        {
            UpdateInstallCmd();
        }

        private void UpdateInstallCmd()
        {
            string fullFileName = Path.Combine(ComponentAbsoluteRootFolder, "InstallWorker.cmd");
            string fileContent = File.ReadAllText(fullFileName);
            string oldName = "\"PreciseQ.AllMeds.TaskService\"";
            string newName = $"\"PreciseQ.AllMeds.TaskService.{ApplicationConfig.VirtualApplicationName}\"";
            fileContent = fileContent.Replace(oldName, newName);
            File.WriteAllText(fullFileName, fileContent);
        }

    
        private void StopService()
        {
            ServiceController sc = new ServiceController(ServiceName);
            ServiceControllerStatus? status = null;
            try
            {
                status = sc.Status;
            }
            catch (Exception)
            {
                status = null;
            }

            if (status.HasValue)
            {
                if (!(sc.Status.Equals(ServiceControllerStatus.Stopped)) || (sc.Status.Equals(ServiceControllerStatus.StopPending)))
                {
                    sc.Stop();
                    UpdateProgerss($"Wait for Stop service {ServiceName}");
                }

                sc.WaitForStatus(ServiceControllerStatus.Stopped);
                UpdateProgerss($"Service {ServiceName} stopped");

            }
            else
            {
                UpdateProgerss($"Cannot get status of service {ServiceName} need to install service manually - continue");
            };
        }

        private void StartService()
        {
            ServiceController sc = new ServiceController(ServiceName);
            ServiceControllerStatus? status = null;
            try
            {
                status = sc.Status;
            }
            catch (Exception)
            {
                status = null;
            }

            if (status.HasValue)
            {
                if (!(sc.Status.Equals(ServiceControllerStatus.Running)) || (sc.Status.Equals(ServiceControllerStatus.StartPending)))
                {
                    sc.Start();
                    UpdateProgerss($"Service {ServiceName} force to start");
                }
            }
            else
            {
                UpdateProgerss($"Cannot get status of service {ServiceName} continue, need to install and start service manually");
            };
        }
    }
}
