using PreciseQ.AllMeds.Installer.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreciseQ.AllMeds.Installer.Updater
{
    public abstract class UpdateFileConfigBase
    {
        protected List<ConfigOverride> PredefinedConfigs;
        public string RelativeFileName { get; protected set; }
        public string FullFileFileName { get; protected set; }
        public UpdateFileConfigBase()
        {
            PredefinedConfigs = new List<ConfigOverride>();
            CustomConfigOverride = new List<ConfigOverride>();
        }

        public abstract void Init(string instanceFolder);

        public virtual void SetPredefinedConfigOverride(string connectionString)
        {

        }

        public IList<ConfigOverride> PredefinedConfigOverride
        {
            get { return PredefinedConfigs; }
        }

        public IList<ConfigOverride> CustomConfigOverride { get; set; }
        protected void SetFullFileNameFromRelative(string instanceFolder)
        {
            FullFileFileName = Path.Combine(instanceFolder, RelativeFileName);
        }
    }
}
