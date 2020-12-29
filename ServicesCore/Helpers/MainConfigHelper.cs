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
    public class MainConfigHelper : AbstractConfigurationHelper
    {
        /// <summary>
        /// Instance for DI list of MainConfigurationModel
        /// </summary>
        public List<MainConfigurationModel> configs;

        /// <summary>
        /// Instance for DI list of PlugInDescriptors
        /// </summary>
        private List<PlugInDescriptors> plugIns;

        /// <summary>
        /// Gets the Root execution Path
        /// </summary>
        private string rootPath;

        private ILogger<MainConfigHelper> logger;

        public MainConfigHelper(List<MainConfigurationModel> _configs, List<PlugInDescriptors> _plugIns) : base()
        {
            configs = _configs;
            plugIns = _plugIns;

            rootPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            Directory.SetCurrentDirectory(rootPath);
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
                    logger = services.GetService<ILogger<MainConfigHelper>>();
                }
            }
        }

        public void InitializeHitServiceCore()
        {
            CheckLogger();
            try
            {
                configs = new List<MainConfigurationModel>();
                MainConfigurationModel tmpConfig;
                tmpConfig = ReadHitServiceCoreConfig();
                if (tmpConfig != null)
                    configs.Add(tmpConfig);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
            }
        }


        /// <summary>
        /// Intitialize the DI list of MainconfigurationModels based on File paths
        /// </summary>
        public void InitializeConfigs()
        {
            CheckLogger();
            try
            {
                configs = new List<MainConfigurationModel>();
                MainConfigurationModel tmpConfig;
                tmpConfig = ReadHitServiceCoreConfig(); // InitilizeConfiguration(rootPath, Guid.Empty, "HitServicesCore.Helpers.MainConfigHelper");
                if (tmpConfig != null)
                    configs.Add(tmpConfig);

                if (plugIns != null)
                    foreach (PlugInDescriptors item in plugIns)
                    {
                        if (item.configClass != null)
                        {
                            tmpConfig = InitilizeConfiguration(item.mainDescriptor.path, item.mainDescriptor.plugIn_Id, item.configClass.fullClassName);
                            if (tmpConfig != null)
                                configs.Add(tmpConfig);
                            //Add config model to static list on HitHelpersNetCore Helper
                            //AddConfigToStaticConfiguration(tmpConfig);
                        }
                    }

            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
            }
        }

        /// <summary>
        /// Saves changes to settings.json file and to DI Configuration model
        /// </summary>
        /// <param name="configToSave"></param>
        public void SaveConfigChanges(MainConfigurationModel configToSave)
        {
            SaveConfiguration(configToSave);
            //Add changes to DI configuration model
            var fld = configs.Find(f => f.plugInId == configToSave.plugInId);
            if (fld == null)
            {
                configs.Add(configToSave);
                fld = configs.Find(f => f.plugInId == configToSave.plugInId);
            }
            else
                fld = configToSave;
        }
    }


    public class ConfigHelperFixingErros : AbstractConfigurationHelper
    {
        public ConfigHelperFixingErros() : base()
        {

        }

    }
}
