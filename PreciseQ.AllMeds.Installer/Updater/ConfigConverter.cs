using PreciseQ.AllMeds.Installer.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PreciseQ.AllMeds.Installer.Updater
{
    public class ConfigConverter
    {
        public static void Apply(string fullFileName, IList<ConfigOverride> configs )
        {
            if (configs == null || configs.Count == 0)
                return;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.PreserveWhitespace = true;
            xmlDoc.Load(fullFileName);
            XmlNode root = xmlDoc.DocumentElement;
            foreach (var toOverride in configs)
            {
                XmlNode node = xmlDoc.SelectSingleNode(toOverride.ConfigXPath);
                if (node == null)
                    throw new ApplicationException($"Node {toOverride.ConfigXPath} not found in {fullFileName}");
                
                node.Attributes[toOverride.AttributeName].Value = toOverride.Value;
            }
            
          
            xmlDoc.Save(fullFileName);
        }
    }
}
