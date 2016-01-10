using PreciseQ.AllMeds.Installer.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreciseQ.AllMeds.Installer.Settings
{
    [Serializable]
    public class SiteInstance : BasePropertyChanged
    {
        private string _instancesRootFolder { get; set; }
        private string _repositoryFolder { get; set; }

        private ObservableCollection<ApplicationInstance> _instances;
        public SiteInstance()
        {
            _instances = new ObservableCollection<ApplicationInstance>();
        }

        public string RepositoryFolder
        {
            get
            {
                return _repositoryFolder;
            }
            set
            {
                _repositoryFolder = value;
                RaisePropertyChanged(nameof(RepositoryFolder));
            }
        }

        public string InstancesRootFolder
        {
            get
            {
                return _instancesRootFolder;
            }
            set
            {
                _instancesRootFolder = value;
                RaisePropertyChanged(nameof(InstancesRootFolder));
            }
        }
        public ObservableCollection<ApplicationInstance> Instances
        {
            get
            {
                return _instances;
            }
            set
            {
                _instances = value;
                RaisePropertyChanged(nameof(Instances));
            }
        }
     
    }
}
