using PreciseQ.AllMeds.Installer.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PreciseQ.AllMeds.Installer.Setting;

namespace PreciseQ.AllMeds.Installer.Setting
{
    [Serializable]
    public class SiteInstance : BasePropertyChanged
    {
        private string _instancesRootFolder;
        private string _repositoryFolder;

        private string _sftpRootFolder;

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

        
        public string SftpRootFolder
        {
            get
            {
                return _sftpRootFolder;
            }
            set
            {
                _sftpRootFolder = value;
                RaisePropertyChanged(nameof(SftpRootFolder));
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
