using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace PreciseQ.AllMeds.Installer.Config
{
    public class InstallerConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("settingsfile", DefaultValue = "", IsRequired = false)]
        public string SettingsFile
        {
            get { return (String)this["settingsfile"]; }
            set { this["settingsfile"] = value; }
        }
    }
}
