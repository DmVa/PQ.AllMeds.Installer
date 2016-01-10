using PreciseQ.AllMeds.Installer.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using log4net;
using System.Configuration;

namespace PreciseQ.AllMeds.Installer.Service
{

    public class ConfigService
    {
        private static ConfigService _current = new ConfigService();
        private InstallerConfigSection _appConfiguration = null;
        private ConfigService() { }

        public static ConfigService Current
        {
            get { return _current; }
        }

        public ILog Logger()
        {
            return LogManager.GetLogger(this.GetType());
        }

        public InstallerConfigSection GetConfig()
        {
            if (_appConfiguration == null)
                _appConfiguration = ConfigurationManager.GetSection("installer") as InstallerConfigSection;

            return _appConfiguration;
        }
    }
}
