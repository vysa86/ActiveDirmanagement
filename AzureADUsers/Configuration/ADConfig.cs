using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ActiveDirectorySearch
{
  public static class ADConfig
    {
        public static string BaseDirectory { get; private set; } = new FileInfo(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath).DirectoryName;
        public static string ConfigurationDirectory { get; private set; } = Path.Combine(BaseDirectory, "Configuration");
        
        public static string AgilePointServiceBaseUrl { get;set; } = string.Empty;
        public static string AgilePointDomain { get;  set; }
        public static string AgilePointUsername { get; set; }
        public static string AgilePointPassword { get; set; }
        public static string AppName { get; set; }
        public static string ActiveDirectoryUsername { get; set; }
        public static string ActiveDirectoryPassword { get; set; }

        static ADConfig()
        {
            InitializeConfiguration();
        }

        private static void InitializeConfiguration()
        {
            try
            {
                //string configContents = File.ReadAllText(Path.Combine(ConfigurationDirectory, "ActiveDirectory.Config.json"));
                //dynamic config = JsonConvert.DeserializeObject(configContents);
                //dynamic ADConfig = config[ConfigConstants.SECTION_AD] ?? throw new MissingFieldException("ActiveDirectory section failed load!");
                //ActiveDirectoryUsername = ADConfig[ConfigConstants.AD_USERNAME];
                //ActiveDirectoryUsername= ADConfig[ConfigConstants.AD_PASSWORD];
                AppName = ConfigConstants.AGILEPOINT_APP_NAME;
                Logger.Info("Configuration Loaded Successfully!");
            }
            catch (FileNotFoundException fex)
            {
                Logger.Error(fex.Message);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }
        }
    }
}
