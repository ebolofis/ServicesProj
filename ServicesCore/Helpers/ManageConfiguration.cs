using HitHelpersNetCore.Helpers;
using HitHelpersNetCore.Models;
using HitServicesCore.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace HitServicesCore.Helpers
{
    public class ManageConfiguration
    {
        /// <summary>
        /// Root execution path
        /// </summary>
        private string CurrentPath; 

        /// <summary>
        /// Instance for DI List of MainConfigurationModels
        /// </summary>
        private List<MainConfigurationModel> configurations;

        /// <summary>
        /// Instnce for DI Login Users
        /// </summary>
        private LoginsUsers loginUsers;

        /// <summary>
        /// Instnce for DI HangFire Services
        /// </summary>
        private List<SchedulerServiceModel> scheduledServices;

        /// <summary>
        /// Instance for DI List of plugins
        /// </summary>
        private List<PlugInDescriptors> plugIns;

        /// <summary>
        /// used to lock the jsons files on read and write
        /// </summary>
        private object lockJsons = new object();

        /// <summary>
        /// Instance for Encryption Helepr
        /// </summary>
        private readonly EncryptionHelper eh;

        private ILogger<ManageConfiguration> logger;

        public ManageConfiguration(List<MainConfigurationModel> _configurations, 
            LoginsUsers _loginUsers, List<SchedulerServiceModel> _scheduledServices,
            List<PlugInDescriptors> _plugIns)
        {
            CurrentPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            System.IO.Directory.SetCurrentDirectory(CurrentPath);

            configurations = _configurations;
            loginUsers = _loginUsers;
            scheduledServices = _scheduledServices;
            plugIns = _plugIns;
            eh = new EncryptionHelper();
            CheckLogger();
        }

        private void CheckLogger()
        {
            if (logger == null)
            {
                IApplicationBuilder _app = DIHelper.AppBuilder;
                if (_app != null)
                {
                    //variable to get from HitServiceCore Sigletons
                    var services = _app.ApplicationServices;//diHelper.Services();//
                    logger = services.GetService<ILogger<ManageConfiguration>>();
                }
            }
        }

        #region Configuration
        /// <summary>
        /// Returns a list of configurations for all plugins
        /// </summary>
        /// <returns></returns>
        public List<MainConfigurationModel> GetConfigs()
        {
            if (configurations.Count == 0)
            {
                ConfigHelperFixingErros fixErrors = new ConfigHelperFixingErros();
                configurations = fixErrors.GetAllConfigs();
            }

            if (configurations == null)
                configurations = new List<MainConfigurationModel>();

            return configurations;
        }

        /// <summary>
        /// Saves values to memory list of configurations and to file
        /// </summary>
        /// <param name="config"></param>
        public void SaveConfigs(List<MainConfigurationModel> config)
        {
            CheckLogger();

            //Instance for abstract class
            MainConfigHelper configHlp = new MainConfigHelper(configurations, plugIns);

            try
            {
                foreach (MainConfigurationModel item in config)
                {
                    //Save changes to file using abstract
                    configHlp.SaveConfigChanges(item);
                }
            }
            catch(Exception ex)
            {
                logger.LogError(ex.ToString());
            }
        }

        #endregion 

        /// <summary>
        /// Save logins to path and on DI Instance
        /// </summary>
        /// <param name="logins"></param>
        public void SaveLogins(LoginsUsers logins)
        {
            CheckLogger();
            try
            {
                lock (lockJsons)
                {
                    string json = JsonSerializer.Serialize(logins);
                    string configPath = Path.GetFullPath(Path.Combine(new string[] { CurrentPath, "pwss.json" }));
                    json = eh.Encrypt(json);
                    File.WriteAllText(configPath, json, Encoding.Default);

                    //Changes the DI instance with new changes
                    loginUsers = logins;
                }
            }
            catch(Exception ex)
            {
                logger.LogError(ex.ToString());
            }
        }

    }
}
