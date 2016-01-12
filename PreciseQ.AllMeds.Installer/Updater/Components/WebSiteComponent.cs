using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PreciseQ.AllMeds.Installer.Setting;

namespace PreciseQ.AllMeds.Installer.Updater.Components
{
    public class WebSiteComponent: UpdateComponentBase
    {
        public WebSiteComponent(SiteInstance siteConfig, ApplicationInstance appplicationConfig) : base(siteConfig, appplicationConfig)
        {
        }

        protected override void ConfigureSettings()
        {
            ComponentRelativeFolder = "Web";
            ConfigFileName = "Web.config";
            if (ApplicationConfig.SiteConfig != null)
            {
                CustomConfigOverride = ApplicationConfig.SiteConfig.ToList();
            }
        }

        protected override void AddPredefinedConfigs()
        {
            string connectionString = ApplicationConfig.ConnectionString;
            AddPredefinedConfig(@"/configuration/appSettings/add[@key='connection-string']", "value", connectionString);

            string allMedsConn = connectionString.TrimEnd(';') +";MultipleActiveResultSets=True;App = EntityFramework";
            AddPredefinedConfig(@"/configuration/connectionStrings/add[@name='AllMedsContext']", "connectionString", allMedsConn);
        }
    }
}
