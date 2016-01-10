using PreciseQ.AllMeds.Installer.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreciseQ.AllMeds.Installer.Updater
{
    public class UpdateFileConfigWebSite : UpdateFileConfigBase
    {
        public override void Init(string instanceFolder)
        {
            RelativeFileName = "web\\web.config";
            SetFullFileNameFromRelative(instanceFolder);
        }

        public override void SetPredefinedConfigOverride(string connectionString)
        {
            ConfigOverride connStr = new ConfigOverride();
            connStr.ConfigXPath = @"/configuration/appSettings/add[@key='connection-string']";
            connStr.AttributeName = "value";
            connStr.Value = connectionString;
            PredefinedConfigs.Add(connStr);

            ConfigOverride connStrAllMeds = new ConfigOverride();
            connStrAllMeds.ConfigXPath = @"/configuration/connectionStrings/add[@name='AllMedsContext']";
            connStrAllMeds.AttributeName = "connectionString";
            connStrAllMeds.Value = connectionString+";MultipleActiveResultSets = True;App = EntityFramework";
            PredefinedConfigs.Add(connStrAllMeds);
        }
    }
    public class UpdateFileConfigWorker : UpdateFileConfigBase
    {
        public override void Init(string instanceFolder)
        {
            RelativeFileName = "Worker\\PreciseQ.AllMeds.TaskService.exe.config";
            SetFullFileNameFromRelative(instanceFolder);
        }

        public override void SetPredefinedConfigOverride(string connectionString)
        {
            ConfigOverride connStrAllMeds = new ConfigOverride();
            connStrAllMeds.ConfigXPath = @"/configuration/connectionStrings/add[@name='AllMedsContext']";
            connStrAllMeds.AttributeName = "connectionString";
            connStrAllMeds.Value = connectionString + ";MultipleActiveResultSets = True;App = EntityFramework";
            PredefinedConfigs.Add(connStrAllMeds);
        }
    }
    public class UpdateFileConfigDbUpdater : UpdateFileConfigBase
    {
        public override void Init(string instanceFolder)
        {
            RelativeFileName = "Db\\DbUpdater\\Updater.exe.config";
            SetFullFileNameFromRelative(instanceFolder);
        }

        public override void SetPredefinedConfigOverride(string connectionString)
        {
            ConfigOverride connStrAllMeds = new ConfigOverride();
            connStrAllMeds.ConfigXPath = @"/configuration/DbUpdater";
            connStrAllMeds.AttributeName = "ConnectionString";
            connStrAllMeds.Value = connectionString;
            PredefinedConfigs.Add(connStrAllMeds);
        }
    }

}
