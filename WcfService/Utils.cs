using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Security.Cryptography;
using System.Configuration;

using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

using WcfService.DataAccess;

namespace WcfService
{
    public class Utils
    {
        public static string GetHash(string password) 
        {
            string hash = String.Empty;
            using (MD5 md5Hash = MD5.Create())
            {
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                hash = Convert.ToBase64String(data);
            }
            return hash;
        }

        public static bool VerifyHash(string input, string hashValue)
        {
            string inputHash = GetHash(input);
            bool result = (hashValue.Equals(inputHash));
            return result;
        }

        public static IRepository GetRepository(string containerName)
        {
            //specify the file path
            string configName = String.Format(@"{0}/unity.config", System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath);
            var fileMap = new ExeConfigurationFileMap { ExeConfigFilename = configName };
            Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            var unitySection = (UnityConfigurationSection)configuration.GetSection("unity");

            var container = new UnityContainer();
            if (String.IsNullOrEmpty(containerName))
                container.LoadConfiguration(unitySection);
            else
                container.LoadConfiguration(unitySection,containerName);

            return container.Resolve<IRepository>();
        }
    }
}