
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using PreciseQ.AllMeds.Installer.Setting;

namespace PreciseQ.AllMeds.Installer.Service
{
    public class SettingsService
    {
        private static SettingsService _current = new SettingsService();
        private SettingsService() { }

        public static SettingsService Current
        {
            get { return _current; }
        }

        public SiteInstance  GetSettings()
        {
            SiteInstance settings = null;
            XmlSerializer ser = new XmlSerializer(typeof(SiteInstance));
            string fileName = ConfigService.Current.GetConfig().SettingsFile;
            if (!File.Exists(fileName))
                return null;
            try
            {
                using (FileStream reader = new FileStream(fileName, FileMode.Open))
                {
                    settings = (SiteInstance)ser.Deserialize(reader);
                    //foreach (var item in settings.Instances)
                    //{
                    //    item.IsChecked = true;
                    //}
                    reader.Close();
                }
            }
            catch (Exception)
            {

            }

            return settings;
        }

        public void SaveSettings(SiteInstance settings)
        {
            string fileName = ConfigService.Current.GetConfig().SettingsFile;

            XmlSerializer ser = new XmlSerializer(typeof(SiteInstance));
            using (TextWriter writer = new StreamWriter(fileName, false))
            {
                ser.Serialize(writer, settings);
                writer.Close();
            }
        }

        public SiteInstance GetDefault()
        {
            var instance = new SiteInstance();
            //instance.RepositoryFolder = "g:\\";
            return instance;
        }

    }
}
