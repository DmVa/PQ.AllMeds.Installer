using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using PreciseQ.AllMeds.Installer.Setting;

namespace PreciseQ.AllMeds.Installer.Updater
{
    public class ConfigConverter
    {
        public static void Apply(string fullFileName, IList<ConfigOverride> configs,  Action<string> updateProgerss )
        {
            if (configs == null || configs.Count == 0)
                return;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.PreserveWhitespace = true;
            xmlDoc.Load(fullFileName);
           
            foreach (var toOverride in configs)
            {
                XmlNode node = xmlDoc.SelectSingleNode(toOverride.ConfigXPath);
                if (node == null )
                {
                    if (updateProgerss != null)
                    {
                        updateProgerss($"config path {toOverride.ConfigXPath} not found in {fullFileName}");
                    }
                    //throw new ApplicationException($"Node {toOverride.ConfigXPath} not found in {fullFileName}");
                    continue;
                }
                if (node.Attributes == null)
                {
                    if (updateProgerss != null)
                    {
                        updateProgerss($"node {toOverride.ConfigXPath} does not have attributes in {fullFileName}");
                    }
                    continue;
                }

                node.Attributes[toOverride.AttributeName].Value = toOverride.Value;
            }
            
          
            xmlDoc.Save(fullFileName);
        }
    }
}
