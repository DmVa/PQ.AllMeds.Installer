using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreciseQ.AllMeds.Installer.Updater
{
    public class WorkerInstallCmdConverter
    {
        public static void Apply(string fullFileName, string appInstanceName)
        {
            string fileContent = File.ReadAllText(fullFileName);
            string oldName = "\"PreciseQ.AllMeds.TaskService\"";
            string newName = string.Format("\"PreciseQ.AllMeds.TaskService.{0}\"", appInstanceName);
            fileContent = fileContent.Replace(oldName, newName);
            File.WriteAllText(fullFileName, fileContent);
        }
    }
}
