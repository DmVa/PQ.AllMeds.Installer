using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PreciseQ.AllMeds.Installer.Base;

namespace PreciseQ.AllMeds.Installer.Setting
{
    public  class SftpSettings : BasePropertyChanged
    {
        private bool _usesftp;
        private string _address;
        private string _port;
        private string _user;
        private string _password;

        public bool UseSftp
        {
            get { return _usesftp; }
            set
            {
                _usesftp = value;
                RaisePropertyChanged(nameof(UseSftp));
            }
        }

        public string Address
        {
            get { return _address; }
            set
            {
                _address = value;
                RaisePropertyChanged(nameof(Address));
            }
        }
        public string Port
        {
            get { return _port; }
            set
            {
                _port = value;
                RaisePropertyChanged(nameof(Port));
            }
        }
        public string User
        {
            get { return _user; }
            set
            {
                _user = value;
                RaisePropertyChanged(nameof(User));
            }
        }
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                RaisePropertyChanged(nameof(Password));
            }
        }
    }
}
