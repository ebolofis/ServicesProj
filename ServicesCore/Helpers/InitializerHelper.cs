using HitHelpersNetCore.Models;
using HitServicesCore.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HitServicesCore.Helpers
{
    public class InitializerHelper
    {
        //Instance for Logger
        private readonly ILogger<InitializerHelper> logger;

        //Instance for plugins
        public readonly List<PlugInDescriptors> plugins;

        //Instance for sys info values
        private readonly SystemInfo sysInfo;

        /// <summary>
        /// lock read, write json files
        /// </summary>
        private object lockJsons = new object();
        public InitializerHelper(List<PlugInDescriptors> _plugins, SystemInfo _sysInfo, ILogger<InitializerHelper> _logger)
        {
            plugins = _plugins;
            sysInfo = _sysInfo;
            logger = _logger;
        }

        /// <summary>
        /// Execute the start method from IInitialerDescriptor implement interface for a specific plugin
        /// </summary>
        /// <param name="plugInId"></param>
        /// <returns></returns>
        public bool RunInitialMethod(Guid plugInId, IApplicationBuilder _app)
        {
            bool result = true;
            try
            {
                //1. Get the plugin to execute initializer from DI List
                var fld = plugins.Find(f => f.mainDescriptor.plugIn_Id == plugInId);

                //2. Plugin not found
                if (fld == null)
                {
                    logger.LogInformation("PlugIn Id " + plugInId.ToString() + " not exists"); 
                    return true;
                }

                //3. Plugin has not implement the Initilizer Interface.
                if (fld.initialerDescriptor == null)
                {
                    logger.LogInformation("PlugIn [" + fld.mainDescriptor.plugIn_Description + "] not having initialazer method");
                    return true;
                }

                //3.1 Check if latest version is equal to current and return true if are same
                if (fld.initialerDescriptor.latestUpdate == fld.initialerDescriptor.dbVersion)
                {
                    logger.LogInformation("PlugIn [" + fld.mainDescriptor.plugIn_Description + "] is updated to latest version");
                    return true;
                }

                //4. Create the Type of Initilizer class to execute the Start method
                var t = Type.GetType(fld.initialerDescriptor.fullNameSpace + ", " + fld.initialerDescriptor.assemblyFileName);

                //5. Type is null, cannot create instance
                if (t == null)
                {
                    logger.LogInformation("Cannot create instance for plugIn [" + fld.mainDescriptor.plugIn_Description + "]");
                    return false;
                }

                //6. Object with parameters. As parameter is the latest update version
                object[]? obj = { fld.initialerDescriptor.latestUpdate, _app };

                //7. Instance of the Initializer class
                object instance = Activator.CreateInstance(t);

                //8. Get the start method
                var method = t.GetMethod("Start");

                //9. Execute the start method
                var res = method.Invoke(instance, obj);

                //10. Update pluing on list with the latest version
                fld.initialerDescriptor.latestUpdate = fld.initialerDescriptor.dbVersion;
                fld.initialerDescriptor.latestUpdateDate = DateTime.UtcNow.Date;

                //11. Save changes to latestUpdates.json on disc
                List<InitializersLastUpdateModel> initUpdats = new List<InitializersLastUpdateModel>();
                foreach (PlugInDescriptors item in plugins)
                {
                    if (item.initialerDescriptor != null)
                    {
                        InitializersLastUpdateModel tmp = new InitializersLastUpdateModel();
                        tmp.latestUpdate = item.initialerDescriptor.latestUpdate;
                        tmp.latestUpdateDate = item.initialerDescriptor.latestUpdateDate;
                        tmp.plugInId = item.mainDescriptor.plugIn_Id;
                        initUpdats.Add(tmp);
                    }
                }
                if (initUpdats.Count > 0)
                    SaveUpdateChangesToFile(initUpdats);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw;
            }
            return result;
        }

        /// <summary>
        /// Saves a list of Initializer to disc file
        /// </summary>
        /// <param name="initUpdats"></param>
        private void SaveUpdateChangesToFile(List<InitializersLastUpdateModel> initUpdats)
        {
            try
            {
                lock (lockJsons)
                {
                    //json file name
                    string sFileName = Path.GetFullPath(Path.Combine(new string[] { sysInfo.rootPath, "Config", "latestUpdates.json" }));

                    //Serialize list of Latest init plufings 
                    string sVal = System.Text.Json.JsonSerializer.Serialize(initUpdats);

                    //Save file to disc
                    File.WriteAllText(sFileName, sVal);
                }
            }
            catch
            {

            }
        }
    }
}
