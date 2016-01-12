using PreciseQ.AllMeds.Installer.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PreciseQ.AllMeds.Installer.Setting
{
    [Serializable]
    public class ApplicationInstance : BasePropertyChanged
    {
        private string _virtualApplicationName { get; set; }
        private bool _isChecked { get; set; }
        private bool _isUpdateSite { get; set; }
        private bool _isUpdateWorker { get; set; }
        private bool _isUpdateDb { get; set; }

        private string _connectionString { get; set; }
        private ObservableCollection<ConfigOverride> _siteConfig;
        private ObservableCollection<ConfigOverride> _workerConfig;
        private SftpSettings _ehrSftpSettings;
        private SftpSettings _ediSftpSettings;

        public ApplicationInstance()
        {
            _siteConfig = new ObservableCollection<ConfigOverride>();
            _workerConfig = new ObservableCollection<ConfigOverride>();
            _ehrSftpSettings = new SftpSettings();
            _ediSftpSettings = new SftpSettings();
        }

        public bool IsChecked
        {
            get
            {
                return _isChecked;
            }
            set
            {
                _isChecked = value;
                RaisePropertyChanged(nameof(IsChecked));
            }
        }
        public bool IsUpdateSite
        {
            get
            {
                return _isUpdateSite;
            }
            set
            {
                _isUpdateSite = value;
                RaisePropertyChanged(nameof(IsUpdateSite));
            }
        }

        public bool IsUpdateWorker
        {
            get
            {
                return _isUpdateWorker;
            }
            set
            {
                _isUpdateWorker = value;
                RaisePropertyChanged(nameof(IsUpdateWorker));
            }
        }

        public bool IsUpdateDb
        {
            get
            {
                return _isUpdateDb;
            }
            set
            {
                _isUpdateDb = value;
                RaisePropertyChanged(nameof(IsUpdateDb));
            }
        }

        public string VirtualApplicationName
        {
            get
            {
                return _virtualApplicationName;
            }
            set
            {
                _virtualApplicationName = value;
                RaisePropertyChanged(nameof(VirtualApplicationName));
            }
        }


        public string ConnectionString
        {
            get
            {
                return _connectionString;
            }
            set
            {
                _connectionString = value;
                RaisePropertyChanged(nameof(ConnectionString));
            }
        }

        public SftpSettings EhrSftpSettings
        {
            get
            {
                return _ehrSftpSettings;
            }
            set
            {
                _ehrSftpSettings = value;
                RaisePropertyChanged(nameof(EhrSftpSettings));
            }
        }

        public SftpSettings EdiSftpSettings
        {
            get
            {
                return _ediSftpSettings;
            }
            set
            {
                _ediSftpSettings = value;
                RaisePropertyChanged(nameof(EdiSftpSettings));
            }
        }

        public ObservableCollection<ConfigOverride> SiteConfig
        {
            get
            {
                return _siteConfig;
            }
        }

        public ObservableCollection<ConfigOverride> WorkerConfig
        {
            get
            {
                return _workerConfig;
            }
        }

        

        public ApplicationInstance MakeCopy()
        {
            XmlSerializer ser = new XmlSerializer(typeof(ApplicationInstance));
            var stream = new MemoryStream();
            using (stream)
            {
                ser.Serialize(stream, this);
                stream.Seek(0, SeekOrigin.Begin);
                ApplicationInstance copy = (ApplicationInstance)ser.Deserialize(stream);
                return copy;
            }
        }
    }
}
