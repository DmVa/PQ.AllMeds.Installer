using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using PreciseQ.AllMeds.Installer.Setting;


namespace PreciseQ.AllMeds.Installer.Updater.Components
{
    public  class UpdateComponentBase
    {
        protected SiteInstance SiteConfig { get; set; }
        protected ApplicationInstance ApplicationConfig { get; set; }

        public string ComponentRelativeFolder { get; protected set; }
        public string ConfigFileName { get; protected set; }
        public string ComponentAbsoluteRootFolder { get; protected set; }

        protected List<ConfigOverride> PredefinedConfigOverride;

        protected List<ConfigOverride> CustomConfigOverride = null;
        protected Action<string> UpdateProgerss { get; set; }

        public UpdateComponentBase(SiteInstance siteConfig, ApplicationInstance appplicationConfig)
        {
            SiteConfig = siteConfig;
            ApplicationConfig = appplicationConfig;
            PredefinedConfigOverride = new List<ConfigOverride>();
        }

        public void Init(Action<string> updateProgerss)
        {
            UpdateProgerss = updateProgerss;
            ConfigureSettings();
            AfterInit();
            AddPredefinedConfigs();
        }
        public virtual void PostUpdateConfig()
        {
        }

        public virtual void PrepareStart()
        {
        }

        public void UpdateConfig()
        {
            string fullConfigFileName = Path.Combine(ComponentAbsoluteRootFolder, ConfigFileName);
            UpdateProgerss($"Update {fullConfigFileName}");
            UpdateConfig(PredefinedConfigOverride, fullConfigFileName);
            UpdateConfig(CustomConfigOverride, fullConfigFileName);
            UpdateProgerss("Ok");
        }

        public virtual void PostCopySources()
        {
        }

        public virtual void PreCopySources()
        {
        }

        public void CopySources()
        {
            string rootFolder = Path.Combine(SiteConfig.RepositoryFolder, ComponentRelativeFolder);
            string instanceFolder = ComponentAbsoluteRootFolder;

            DirectoryInfo sourceDir = new DirectoryInfo(rootFolder);
            DirectoryInfo targetDir = new DirectoryInfo(instanceFolder);
            UpdateProgerss($"Copy from {sourceDir.FullName} to {targetDir.FullName}");
            CopyFiles(sourceDir, targetDir);
            UpdateProgerss($"Ok");
        }


        protected void AddPredefinedConfig(string xpath, string attributeName, string newValue)
        {
            ConfigOverride connection = new ConfigOverride();
            connection.ConfigXPath = xpath;
            connection.AttributeName = attributeName;
            connection.Value = newValue;
            PredefinedConfigOverride.Add(connection);
        }

        protected void AddSftpRootConfig()
        {
            string sftpRoot = Path.Combine(SiteConfig.SftpRootFolder, ApplicationConfig.VirtualApplicationName);
            AddPredefinedConfig(@"/configuration/appSettings/add[@key='sftp.root']", "value", sftpRoot);
        }

        protected void AddEhrSftpConfig()
        {
            AddSftpConfig(
                "hl7.export.usesftp", 
                "hl7.export.sftpaddress", 
                "hl7.export.sftpport",
                "hl7.export.sftpuser", 
                "hl7.export.sftppassword", 
                ApplicationConfig.EhrSftpSettings);
        }
        protected void AddEdiSftpConfig()
        {
            AddSftpConfig(
                "edi.usesftp",
                "edi.sftpaddress",
                "edi.sftpport",
                "edi.sftpuser",
                "edi.sftppassword",
                ApplicationConfig.EdiSftpSettings);
        }

        private void AddSftpConfig(string useSftpKey, string sftpAddressKey, string portKey, string userKey, string pwdKey, SftpSettings sttings)
        {
            var useSftp = sttings.UseSftp.ToString().ToLower();

            AddPredefinedConfig($"/configuration/appSettings/add[@key='{useSftpKey}']", "value", useSftp);
            AddPredefinedConfig($"/configuration/appSettings/add[@key='{sftpAddressKey}']", "value", sttings.Address);
            AddPredefinedConfig($"/configuration/appSettings/add[@key='{portKey}']", "value", sttings.Port);
            AddPredefinedConfig($"/configuration/appSettings/add[@key='{userKey}']", "value", sttings.User);
            AddPredefinedConfig($"/configuration/appSettings/add[@key='{pwdKey}']", "value", sttings.Password);
        }


        protected virtual void AddPredefinedConfigs()
        {
        }

        protected virtual void ConfigureSettings()
        {
           
        }

        private void AfterInit()
        {
            string instanceRootFoleder = Path.Combine(SiteConfig.InstancesRootFolder, ApplicationConfig.VirtualApplicationName);
            ComponentAbsoluteRootFolder = Path.Combine(instanceRootFoleder, ComponentRelativeFolder);
        }

    

        private void UpdateConfig(List<ConfigOverride> toOverride, string fullConfigFileName)
        {
            if (toOverride != null && toOverride.Count > 0)
            {
                ConfigConverter.Apply(fullConfigFileName, toOverride, UpdateProgerss);
            }
        }

        private void CopyFiles(DirectoryInfo sourceDir, DirectoryInfo targetDir)
        {
            if (!targetDir.Exists)
                targetDir.Create();

            System.IO.FileInfo[] files = sourceDir.GetFiles("*.*");
            foreach (System.IO.FileInfo sourceFile in files)
            {

                string destFileName = GetDestFileName(sourceFile, targetDir);
                sourceFile.CopyTo(destFileName, true);
            }

            // First, process all the files directly under this folder
            System.IO.DirectoryInfo[] subDirs = sourceDir.GetDirectories();
            foreach (System.IO.DirectoryInfo dirInfo in subDirs)
            {

                DirectoryInfo targetSubDir = GetDestDir(dirInfo.Name, targetDir);
                CopyFiles(dirInfo, targetSubDir);
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
    }
}
