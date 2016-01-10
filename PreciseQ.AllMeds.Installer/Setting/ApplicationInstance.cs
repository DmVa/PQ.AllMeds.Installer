using PreciseQ.AllMeds.Installer.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PreciseQ.AllMeds.Installer.Settings
{
    [Serializable]
    public class ApplicationInstance : BasePropertyChanged
    {
        private string _virtualApplicationName { get; set; }
        private bool _isChecked { get; set; }

        private string _connectionString { get; set; }
        private ObservableCollection<ConfigOverride> _siteConfig;
        private ObservableCollection<ConfigOverride> _workerConfig;
        public ApplicationInstance()
        {
            _siteConfig = new ObservableCollection<ConfigOverride>();
            _workerConfig = new ObservableCollection<ConfigOverride>();
        }

        [XmlIgnore]
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
                copy.IsChecked = this.IsChecked;
                return copy;
            }
        }
    }
}
