using PreciseQ.AllMeds.Installer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreciseQ.AllMeds.Installer.Settings
{

    [Serializable]
    public class ConfigOverride : BasePropertyChanged
    {
        private string _configXPath { get; set; }
        public string ConfigXPath
        {
            get
            {
                return _configXPath;
            }
            set
            {
                _configXPath = value;
                RaisePropertyChanged(nameof(ConfigXPath));
            }
        }

        private string _value { get; set; }
        public string Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                RaisePropertyChanged(nameof(Value));
            }
        }

        private string _attributeName { get; set; }
        public string AttributeName
        {
            get
            {
                return _attributeName;
            }
            set
            {
                _attributeName = value;
                RaisePropertyChanged(nameof(AttributeName));
            }
        }
    }
}

