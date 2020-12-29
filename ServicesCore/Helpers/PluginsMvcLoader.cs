using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HitHelpersNetCore.Helpers;
using HitHelpersNetCore.Interfaces;
using HitHelpersNetCore.Models;
using HitServicesCore.Models;
using HitServicesCore.Models.IS_Services;
using HitServicesCore.Models.SharedModels;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NCrontab;
using NLog.Time;

namespace HitServicesCore.Helpers
{
    public class PluginsMvcLoader
    {
        private readonly ILogger<PluginsMvcLoader> logger;

        private readonly SystemInfo sysInfo;

        private readonly List<PlugInDescriptors> plugInDescriptors;

        private readonly List<MainConfigurationModel> configurations;

        private readonly List<SchedulerServiceModel> hangfireServices;

        private readonly PlugInAnnotationedClassesHelper plgAnnotHelp;

        private readonly LoginsUsers loginsUsers;
        private readonly SmtpHelper smtpConfiguration;

        /// <summary>
        /// Lock read write json files
        /// </summary>
        private object lockJsons = new object();

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="rootPath">app root path</param>
        /// <param name="logger">logger</param>
        public PluginsMvcLoader(SystemInfo _sysInfo, List<PlugInDescriptors> _plugInDescriptors,
            List<MainConfigurationModel> _configurations, List<SchedulerServiceModel> _hangfireServices,
            LoginsUsers _loginsUsers, PlugInAnnotationedClassesHelper _plgAnnotHelp, ILogger<PluginsMvcLoader> _logger,SmtpHelper _smtpConfiguration)
        {
            sysInfo = _sysInfo;
            logger = _logger;
            plugInDescriptors = _plugInDescriptors;
            configurations = _configurations;
            hangfireServices = _hangfireServices;
            loginsUsers = _loginsUsers;
            plgAnnotHelp = _plgAnnotHelp;
            smtpConfiguration = _smtpConfiguration;
        }

        /// <summary>
        /// chack folder existase and optionally create folder hierarchy
        /// </summary>
        /// <param name="path">path</param>
        /// <param name="create">true: create if path does not exist</param>
        private bool checkfolderExist(string path, bool create = true, bool delete = false)
        {
            bool res = Directory.Exists(path);
            if (res && delete)
            {
                Directory.Delete(path, true);
                res = false;
            }
            if (!res && create)
            {
                //   _logger.LogInformation($"Creating path '{path}'... ");
                try
                {
                    Directory.CreateDirectory(path);
                    return true;
                }
                catch (Exception ex)
                {
                    logger.LogError(ex.ToString());
                    return false;
                }
            }
            return res;
        }

        /// <summary>
        /// load controllers (mvc and api)
        /// </summary>
        public void LoadMvcPlugins(ApplicationPartManager apm)
        {
            //Initialize Main Configuration (HitServicesCore)
            InitializeMainConfiguration();

            //Read all plugins
            ConfigureApplicationParts(apm);

            //adds configurations to list og Mainconfiguration Model
            AddConfigurationsToModel();

            //check Services from plugin if exists on main configuration scheduler
            CheckServicesForConfiguration();

            //Add logins to DI class
            AddLogins();
            //
            smtpConfiguration.GetConfig();
            //Add Intializers Lastest updated version
            FindInitizlersLatestUpdatedVersion();
        }

        private void ConfigureApplicationParts(ApplicationPartManager apm)
        {
            //
            string error;

            // check for plugins path
            if (checkfolderExist(sysInfo.pluginPath))
            {

                string[] plugIns = Directory.GetDirectories(sysInfo.pluginPath);


                foreach (string plgPath in plugIns)
                {
                    bool AddToDI;

                    /*Read Configuration file*/
                    //Assembly EntryAssembly = Assembly.GetEntryAssembly();

                    var assemblyFiles = Directory.GetFiles(plgPath, "*.dll", SearchOption.AllDirectories);

                    foreach (var assemblyFile in assemblyFiles)
                    {
                        if (assemblyFile.EndsWith(".Views.dll"))
                        {
                            Assembly assembly = Assembly.LoadFrom(assemblyFile);
                            apm.ApplicationParts.Add(new CompiledRazorAssemblyPart(assembly));
                            continue;
                        }

                        AddToDI = plgAnnotHelp.AddPlugInToPlugInDecriptors(assemblyFile, plugInDescriptors, plgPath, out error);
                        if (!string.IsNullOrWhiteSpace(error))
                        {
                            if (error == "No descriptor for plugin exists")
                                logger.LogInformation(error);
                            else
                                logger.LogError(error);
                        }

                        if (AddToDI)
                            try
                            {
                                //Assembly assembly = Assembly.LoadFile(assemblyFile);//  .LoadFrom(assemblyFile);
                                Assembly assembly = Assembly.LoadFrom(assemblyFile);
                                apm.ApplicationParts.Add(new AssemblyPart(assembly));
                                saveResources(assembly);
                                //{ TODO: For future ...}
                                //setEntryPoints(assembly);



                            }
                            catch (Exception ex)
                            {
                                string a = ex.ToString();
                            }
                    }
                }


            }

            apm.FeatureProviders.Add(new ViewComponentFeatureProvider());
        }

        /// <summary>
        /// Add Logins from json file
        /// </summary>
        private void AddLogins()
        {
            try
            {
                lock (lockJsons)
                {
                    string sFileName = Path.GetFullPath(Path.Combine(new string[] { sysInfo.rootPath, "Config", "pwss.json" }));
                    loginsUsers.logins = FillSubDictionary(sFileName);
                    //if (File.Exists(mainconfigPath))
                    //{
                    //    sVal = System.IO.File.ReadAllText(mainconfigPath, Encoding.Default);
                    //    mainConfigPlugIn.mainConfiguration.logins = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, List<string>>>(sVal);
                    //}
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
            }
        }

        /// <summary>
        /// Reads Lastests DB version updates from json file
        /// </summary>
        private void FindInitizlersLatestUpdatedVersion()
        {
            try
            {
                lock (lockJsons)
                {
                    //Default date for non existing initializer on json file
                    DateTime dtNow = new DateTime(1900, 1, 1);// DateTime.UtcNow.Date;

                    //Plugins having initializer interface.. All other from json file will be removed
                    List<Guid> availablePlugins = new List<Guid>();

                    //List with Latest init plufings from json file
                    List<InitializersLastUpdateModel> initUpdats = new List<InitializersLastUpdateModel>();
                    //json file name
                    string sFileName = Path.GetFullPath(Path.Combine(new string[] { sysInfo.rootPath, "Config", "latestUpdates.json" }));

                    if (File.Exists(sFileName)) //File exist (not the first installation run)
                    {
                        //fill list of Latest init plufings from json file
                        string sVal = File.ReadAllText(sFileName);
                        initUpdats = System.Text.Json.JsonSerializer.Deserialize<List<InitializersLastUpdateModel>>(sVal);

                        //Check all plugins Initial classes
                        foreach (PlugInDescriptors item in plugInDescriptors)
                        {
                            //Initial class exists
                            if (item.initialerDescriptor != null)
                            {
                                var fld = initUpdats.Find(f => f.plugInId == item.mainDescriptor.plugIn_Id);
                                //Not Found on json file so insert on list with init values (version 0.0.0.0 and date equals to now)
                                if (fld == null)
                                {
                                    item.initialerDescriptor.latestUpdate = "0.0.0.0";
                                    item.initialerDescriptor.latestUpdateDate = dtNow;
                                    initUpdats.Add(new InitializersLastUpdateModel { plugInId = item.mainDescriptor.plugIn_Id, latestUpdate = "0.0.0.0", latestUpdateDate = dtNow });
                                }
                                else //not found on json file so add it as version 0.0.0.0 and date equals to now
                                {
                                    item.initialerDescriptor.latestUpdate = fld.latestUpdate;
                                    item.initialerDescriptor.latestUpdateDate = fld.latestUpdateDate;
                                }
                                //Add to available plugins. All the others from the list will be removed
                                availablePlugins.Add(item.mainDescriptor.plugIn_Id);
                            }
                        }

                    }
                    else //New installation and the initialzers have not been runned
                    {

                        foreach (PlugInDescriptors item in plugInDescriptors)
                        {
                            if (item.initialerDescriptor != null)
                            {
                                item.initialerDescriptor.latestUpdate = "0.0.0.0";
                                item.initialerDescriptor.latestUpdateDate = dtNow;
                                initUpdats.Add(new InitializersLastUpdateModel { plugInId = item.mainDescriptor.plugIn_Id, latestUpdate = "0.0.0.0", latestUpdateDate = dtNow });
                                availablePlugins.Add(item.mainDescriptor.plugIn_Id);
                            }
                        }
                    }
                    //Remove unnecessary records
                    initUpdats.RemoveAll(r => !availablePlugins.Contains(r.plugInId));

                    //Save new changes
                    string newFile = System.Text.Json.JsonSerializer.Serialize(initUpdats);
                    File.WriteAllText(sFileName, newFile);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
            }
        }


        private void InitializeMainConfiguration()
        {
            MainConfigHelper configHlp = new MainConfigHelper(configurations, plugInDescriptors);
            configHlp.InitializeHitServiceCore();
        }

        /// <summary>
        /// Add configurations from plugins and main project to DI list of MainConfigurationModels
        /// </summary>
        private void AddConfigurationsToModel()
        {
            MainConfigHelper configHlp = new MainConfigHelper(configurations, plugInDescriptors);
            configHlp.InitializeConfigs();
        }

        /// <summary>
        /// Check all services to main configuration services if exists and remove unnecessary 
        /// </summary>
        /// <param name="configScheduler"></param>
        private void CheckServicesForConfiguration()
        {
            try
            {
                //Services found on plugins. Remove from main configuration list all other services
                List<Guid> availableServices = new List<Guid>();
                lock (lockJsons)
                {
                    //Load all services from main project and json file scheduler
                    string sFileName = Path.GetFullPath(Path.Combine(new string[] { sysInfo.rootPath, "Config", "scheduler.json" }));
                    if (File.Exists(sFileName))
                    {
                        string sVal = System.IO.File.ReadAllText(sFileName, Encoding.Default);
                        EncryptionHelper eh = new EncryptionHelper();
                        sVal = eh.Decrypt(sVal);
                        if (!string.IsNullOrWhiteSpace(sVal))
                            hangfireServices.AddRange(System.Text.Json.JsonSerializer.Deserialize<List<SchedulerServiceModel>>(sVal));
                    }
                }

                //checks all services from plugin if exists on DI method hangfireServices
                GetPluginServices(ref availableServices);

                //Check all Incorporated Services type Export Data if exists on DI method hangfireServices
                Get_IS_ExportDataServices(ref availableServices);

                //Check all Incorporated Services type Run Sql Script if exists on DI method hangfireServices
                Get_IS_SqlScriptsServices(ref availableServices);

                //Check all Incorporated Services type Save To Table if exists on DI method hangfireServices
                Get_IS_SaveToTableServices(ref availableServices);

                //Check all Incorporated Services type Read from Csv if exists on DI method hangfireServices
                Get_IS_ReadFromCsvServices(ref availableServices);

                //Remove unnecessary services
                hangfireServices.RemoveAll(r => !availableServices.Contains(r.serviceId));
                
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
            }

        }

        /// <summary>
        /// Check plugin services
        /// </summary>
        /// <param name="availableServices"></param>
        private void GetPluginServices(ref List<Guid> availableServices)
        {
            int serviceCount = 0;
            lock (lockJsons)
            {
                foreach (PlugInDescriptors item in plugInDescriptors)
                {
                    //Hit service core has no services to add to Hang fire
                    if (item.mainDescriptor.plugIn_Id == Guid.Empty)
                        continue;

                    //Plug in has services to add to Hang fire
                    if (item.serviceDescriptor != null)
                    {
                        //Check All services if exists on main configuration list
                        foreach (ServiceDescriptorWithTypeModel service in item.serviceDescriptor)
                        {
                            var objectType = Type.GetType(service.fullNameSpace + ", " + service.assemblyFileName);
                            if (objectType.BaseType.Name== "ServiceExecutions")
                            {
                                serviceCount = hangfireServices.Where(w => w.classFullName == service.fullNameSpace).Count();
                                //Service exists only one time on list
                                if (serviceCount == 1)
                                {
                                    var fld = hangfireServices.Find(f => f.classFullName == service.fullNameSpace);
                                    //service exists on list but not initialized
                                    if (fld.serviceId == null || fld.serviceId == Guid.Empty || fld.serviceId != service.serviceId)
                                        fld.serviceId = service.serviceId;
                                    //change service class with the latest from plugins
                                    fld.description = "";
                                    fld = FillEmptyValues(fld, service);

                                    availableServices.Add(service.serviceId);
                                }
                                //Service not exists on list
                                else if (serviceCount == 0)
                                {
                                    hangfireServices.Add(new SchedulerServiceModel
                                    {
                                        serviceId = service.serviceId,
                                        serviceName = service.seriveName,
                                        classFullName = service.fullNameSpace,
                                        description = service.serviceDescription,
                                        isActive = false,
                                        schedulerTime = "* * * * *",
                                        schedulerDescr = "Every minute",
                                        assemblyFileName = service.assemblyFileName,
                                        serviceVersion = service.serviceVersion,
                                        serviceType = Enums.HangFireServiceTypeEnum.Plugin
                                    });
                                    availableServices.Add(service.serviceId);
                                }
                                //Many same services (same classFullName) exists on list. Check if Id existst or exists any with no Id on List to update
                                else
                                {
                                    var fld = hangfireServices.Find(f => f.classFullName == service.fullNameSpace && f.serviceId == service.serviceId);
                                    //Not found service with same service id
                                    if (fld == null)
                                        fld = hangfireServices.Find(f => f.classFullName == service.fullNameSpace && (f.serviceId == null || f.serviceId == Guid.Empty));
                                    //Not found service with classFullName and empty service id
                                    if (fld == null)
                                    {
                                        hangfireServices.Add(new SchedulerServiceModel
                                        {
                                            serviceId = service.serviceId,
                                            classFullName = service.fullNameSpace,
                                            description = service.serviceDescription,
                                            isActive = false,
                                            schedulerTime = "* * * * *",
                                            schedulerDescr = "Every minute",
                                            assemblyFileName = service.assemblyFileName,
                                            serviceVersion = service.serviceVersion
                                        });
                                    }
                                    //Found service with same service id or empty service id
                                    else
                                    {
                                        fld.serviceId = service.serviceId;
                                        //change service class with the latest from plugins
                                        fld.description = "";
                                        fld = FillEmptyValues(fld, service);
                                    }
                                    availableServices.Add(service.serviceId);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Check for Incorporated Services type Export Data
        /// </summary>
        /// <param name="availableServices"></param>
        private void Get_IS_ExportDataServices(ref List<Guid> availableServices)
        {
            //Path for Sql Scripts json files
            string isServicePath = Path.Combine(new string[] { sysInfo.rootPath, "IS_Services", "ExportData" });

            if (!Directory.Exists(isServicePath))
                return;
            if (isServicePath[isServicePath.Length - 1] != '\\')
                isServicePath += '\\';

            //Run Sql script model
            ISExportDataModel scriptModel;
            string sVal;
            try
            {
                //Lock Files
                lock (lockJsons)
                {
                    //List of json from SqlScripts directory
                    List<string> sqlScriptsJsons = Directory.EnumerateFiles(isServicePath, "*.json").ToList();
                    foreach (string item in sqlScriptsJsons)
                    {
                        sVal = System.IO.File.ReadAllText(item, Encoding.Default);
                        if (!string.IsNullOrWhiteSpace(sVal))
                        {
                            //Deserialize json to ISRunSqlScriptsModel
                            scriptModel = System.Text.Json.JsonSerializer.Deserialize<ISExportDataModel>(sVal);
                            if (scriptModel != null)
                            {
                                //Create inherited class ISServiceGeneralModel
                                ISServiceGeneralModel generalClass = new ISServiceGeneralModel();
                                generalClass.ClassDescription = scriptModel.ClassDescription;
                                generalClass.ClassType = scriptModel.ClassType;
                                generalClass.FullClassName = "HitServicesCore.InternalServices.ISExportDataService";
                                generalClass.serviceId = scriptModel.serviceId;
                                generalClass.serviceName = scriptModel.serviceName;
                                generalClass.serviceType = scriptModel.serviceType;
                                generalClass.serviceVersion = scriptModel.serviceVersion == null ? long.MinValue : scriptModel.serviceVersion;
                                generalClass.serviceType = Enums.HangFireServiceTypeEnum.ExportData;

                                //add to HangFire class
                                AddISServiceToHangFireList(generalClass, "HitServicesCore");

                                //add service to available services
                                availableServices.Add(scriptModel.serviceId);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
            }
        }

        /// <summary>
        /// Check for Incorporated Services type Run Sql Scripts
        /// </summary>
        /// <param name="availableServices"></param>
        private void Get_IS_SqlScriptsServices(ref List<Guid> availableServices)
        {
            //Path for Sql Scripts json files
            string isServicePath = Path.Combine(new string[] { sysInfo.rootPath, "IS_Services", "SqlScripts" });
            if (!Directory.Exists(isServicePath))
                return;
            if (isServicePath[isServicePath.Length - 1] != '\\')
                isServicePath += '\\';
            
            //Run Sql script model
            ISRunSqlScriptsModel scriptModel;
            string sVal;
            try
            {
                //Lock Files
                lock (lockJsons)
                {
                    //List of json from SqlScripts directory
                    List<string> sqlScriptsJsons = Directory.EnumerateFiles(isServicePath, "*.json").ToList();
                    foreach (string item in sqlScriptsJsons)
                    {
                        sVal = System.IO.File.ReadAllText(item, Encoding.Default);
                        if (!string.IsNullOrWhiteSpace(sVal))
                        {
                            //Deserialize json to ISRunSqlScriptsModel
                            scriptModel = System.Text.Json.JsonSerializer.Deserialize<ISRunSqlScriptsModel>(sVal);
                            if (scriptModel != null)
                            {
                                //Create inherited class ISServiceGeneralModel
                                ISServiceGeneralModel generalClass = new ISServiceGeneralModel();
                                generalClass.ClassDescription = scriptModel.ClassDescription;
                                generalClass.ClassType = scriptModel.ClassType;
                                generalClass.FullClassName = "HitServicesCore.InternalServices.ISRunSqlScriptService";
                                generalClass.serviceId = scriptModel.serviceId;
                                generalClass.serviceName = scriptModel.serviceName;
                                generalClass.serviceType = scriptModel.serviceType;
                                generalClass.serviceVersion = scriptModel.serviceVersion == null ? long.MinValue : scriptModel.serviceVersion;
                                generalClass.serviceType = Enums.HangFireServiceTypeEnum.SqlScripts;

                                //add to HangFire class
                                AddISServiceToHangFireList(generalClass, "HitServicesCore");

                                //add service to available services
                                availableServices.Add(scriptModel.serviceId);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
            }
        }

        /// <summary>
        /// Check for Incorporated Services type Save To Table
        /// </summary>
        /// <param name="availableServices"></param>
        private void Get_IS_SaveToTableServices(ref List<Guid> availableServices)
        {
            //Path for Sql Scripts json files
            string isServicePath = Path.Combine(new string[] { sysInfo.rootPath, "IS_Services", "SaveToTable" });
            if (!Directory.Exists(isServicePath))
                return;
            if (isServicePath[isServicePath.Length - 1] != '\\')
                isServicePath += '\\';

            //Run Sql script model
            ISSaveToTableModel scriptModel;
            string sVal;
            try
            {
                //Lock Files
                lock (lockJsons)
                {
                    //List of json from SqlScripts directory
                    List<string> saveToTableJsons = Directory.EnumerateFiles(isServicePath, "*.json").ToList();
                    foreach (string item in saveToTableJsons)
                    {
                        sVal = System.IO.File.ReadAllText(item, Encoding.Default);
                        if (!string.IsNullOrWhiteSpace(sVal))
                        {
                            //Deserialize json to ISSaveToTableModel
                            scriptModel = System.Text.Json.JsonSerializer.Deserialize<ISSaveToTableModel>(sVal);
                            if (scriptModel != null)
                            {
                                //Create inherited class ISServiceGeneralModel
                                ISServiceGeneralModel generalClass = new ISServiceGeneralModel();
                                generalClass.ClassDescription = scriptModel.ClassDescription;
                                generalClass.ClassType = scriptModel.ClassType;
                                generalClass.FullClassName = "HitServicesCore.InternalServices.ISSaveToTableService";
                                generalClass.serviceId = scriptModel.serviceId;
                                generalClass.serviceName = scriptModel.serviceName;
                                generalClass.serviceType = scriptModel.serviceType;
                                generalClass.serviceVersion = scriptModel.serviceVersion == null ? long.MinValue : scriptModel.serviceVersion;
                                generalClass.serviceType = Enums.HangFireServiceTypeEnum.SaveToTable;

                                //add to HangFire class
                                AddISServiceToHangFireList(generalClass, "HitServicesCore");

                                //add service to available services
                                availableServices.Add(scriptModel.serviceId);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
            }
        }

        /// <summary>
        /// Check for Incorporated Services type read from csv
        /// </summary>
        /// <param name="availableServices"></param>
        private void Get_IS_ReadFromCsvServices(ref List<Guid> availableServices)
        {
            //Path for Sql Scripts json files
            string isServicePath = Path.Combine(new string[] { sysInfo.rootPath, "IS_Services", "ReadCsv" });
            if (!Directory.Exists(isServicePath))
                return;
            if (isServicePath[isServicePath.Length - 1] != '\\')
                isServicePath += '\\';

            //Run Sql script model
            ISReadFromCsvModel scriptModel;
            string sVal;
            try
            {
                //Lock Files
                lock (lockJsons)
                {
                    //List of json from SqlScripts directory
                    List<string> saveToTableJsons = Directory.EnumerateFiles(isServicePath, "*.json").ToList();
                    foreach (string item in saveToTableJsons)
                    {
                        sVal = System.IO.File.ReadAllText(item, Encoding.Default);
                        if (!string.IsNullOrWhiteSpace(sVal))
                        {
                            //Deserialize json to ISSaveToTableModel
                            scriptModel = System.Text.Json.JsonSerializer.Deserialize<ISReadFromCsvModel>(sVal);
                            if (scriptModel != null)
                            {
                                //Create inherited class ISServiceGeneralModel
                                ISServiceGeneralModel generalClass = new ISServiceGeneralModel();
                                generalClass.ClassDescription = scriptModel.ClassDescription;
                                generalClass.ClassType = scriptModel.ClassType;
                                generalClass.FullClassName = "HitServicesCore.InternalServices.ISReadCsvService";
                                generalClass.serviceId = scriptModel.serviceId;
                                generalClass.serviceName = scriptModel.serviceName;
                                generalClass.serviceType = scriptModel.serviceType;
                                generalClass.serviceVersion = scriptModel.serviceVersion == null ? long.MinValue : scriptModel.serviceVersion;
                                generalClass.serviceType = Enums.HangFireServiceTypeEnum.ReadFromCsv;

                                //add to HangFire class
                                AddISServiceToHangFireList(generalClass, "HitServicesCore");

                                //add service to available services
                                availableServices.Add(scriptModel.serviceId);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
            }
        }

        /// <summary>
        /// add IS service to Hangfire List. IS service is base Class (ISServiceGeneralModel)
        /// </summary>
        /// <param name="isModel"></param>
        /// <param name="fileName"></param>
        private void AddISServiceToHangFireList(ISServiceGeneralModel isModel, string fileName)
        {
            int servCount;
            if (hangfireServices == null)
                servCount = 0;
            else if (hangfireServices.Find(f => f.serviceId == isModel.serviceId) == null)
                servCount = 0;
            else
                servCount = hangfireServices.FindAll(f => f.serviceId == isModel.serviceId).Count();

            switch (servCount)
            {
                //Not found on list of services and adds it
                case 0:
                    hangfireServices.Add(new SchedulerServiceModel
                    {
                        assemblyFileName = fileName,
                        classFullName = isModel.FullClassName,
                        description = isModel.ClassDescription,
                        isActive = false,
                        schedulerTime = "* * * * *",
                        schedulerDescr = "Every minute",
                        serviceId = isModel.serviceId,
                        serviceName = isModel.serviceName,
                        serviceType = isModel.serviceType,
                        serviceVersion = isModel.serviceVersion.ToString()
                    });
                    break;
                //Found on list of services and replace id
                case 1:
                    var fld = hangfireServices.Find(f => f.serviceId == isModel.serviceId);
                    //change service class with the latest from plugins
                    fld.description = "";
                    fld = FillEmptyValuesForISService(fld, isModel, fileName);
                    break;
                //more than one on list of services. Remove all and add new one
                default:
                    hangfireServices.RemoveAll(r => r.serviceId == isModel.serviceId);
                    hangfireServices.Add(new SchedulerServiceModel
                    {
                        assemblyFileName = fileName,
                        classFullName = isModel.FullClassName,
                        description = isModel.ClassDescription,
                        isActive = false,
                        schedulerTime = "* * * * *",
                        schedulerDescr = "Every minute",
                        serviceId = isModel.serviceId,
                        serviceName = isModel.serviceName,
                        serviceType = isModel.serviceType,
                        serviceVersion = isModel.serviceVersion.ToString()
                    });
                    break;
            }
        }

        /// <summary>
        /// Check if any value is empty to fill it from json file
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private SchedulerServiceModel FillEmptyValues(SchedulerServiceModel model, ServiceDescriptorWithTypeModel pluginModel)
        {
            if (string.IsNullOrWhiteSpace(model.assemblyFileName))
                model.assemblyFileName = pluginModel.assemblyFileName;
            if (string.IsNullOrWhiteSpace(model.classFullName))
                model.classFullName = pluginModel.fullNameSpace;
            if (string.IsNullOrWhiteSpace(model.description))
                model.description = pluginModel.serviceDescription;
            if (string.IsNullOrWhiteSpace(model.serviceVersion))
                model.serviceVersion = pluginModel.serviceVersion;
            if (string.IsNullOrWhiteSpace(model.schedulerDescr))
                model.schedulerDescr = ParseCron(model.schedulerTime);
            if (string.IsNullOrWhiteSpace(model.serviceName))
                model.serviceName = pluginModel.seriveName;
            return model;
        }

        /// <summary>
        /// Check if any value is empty to fill it from json file
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private SchedulerServiceModel FillEmptyValuesForISService(SchedulerServiceModel model, ISServiceGeneralModel isModel, string fileName)
        {
            if (string.IsNullOrWhiteSpace(model.assemblyFileName))
                model.assemblyFileName = fileName;
            if (string.IsNullOrWhiteSpace(model.classFullName))
                model.classFullName = isModel.FullClassName;
            if (string.IsNullOrWhiteSpace(model.description))
                model.description = isModel.ClassDescription;
            if (string.IsNullOrWhiteSpace(model.serviceVersion))
                model.serviceVersion = (isModel.serviceVersion == null ? long.MinValue : isModel.serviceVersion).ToString();
            if (string.IsNullOrWhiteSpace(model.schedulerDescr))
                model.schedulerDescr = ParseCron(model.schedulerTime);
            if (string.IsNullOrWhiteSpace(model.serviceName))
                model.serviceName = isModel.serviceName;
            return model;
        }

        /// <summary>
        /// Convert Scheduler Time to Description
        /// </summary>
        /// <param name="cron"></param>
        /// <returns></returns>
        private string ParseCron(string cron)
        {
            string[] cronParce = cron.Split(" ");

            var minuteState = "";
            var hourState = "";
            var daysState = "";
            var monthsState = "";
            var weekdayState = "";
            //Minutes
            if (cronParce[0] == "*") minuteState = "At every minute";
            if (cronParce[0] == "*/2") minuteState = "At every 2nd minute";
            if (cronParce[0] == "1-59/2") minuteState = "At every 2nd minute from 1 through 59";
            //if (cronParce[0] == "*/" + self.xminute() && self.xminute() != undefined) minuteState = "At every " + self.xminute() + " minute(s) ";
            //if (cronParce[0] == "*/" + self.mminute() && self.mminute() != undefined) minuteState = " At every " + self.mminute() + " minute(s) ";

            //Hours
            if (cronParce[1] == "*") hourState = "past every hour";
            if (cronParce[1] == "*/2") hourState = "past every two hours";
            if (cronParce[1] == "1-23/2") hourState = "past every 2nd hour from 1 through 23";
            //if (cronParce[1] == "*/" + self.xhour() && self.xhour() != undefined) hourState = "past every " + self.xhour() + " hour(s) ";
            //if (cronParce[1] == "*/" + self.mhour() && self.mhour() != undefined) hourState = "past every " + self.mhour() + " hour(s) ";

            //Days
            if (cronParce[2] == "*") daysState = "on every day";
            if (cronParce[2] == "*/2") daysState = "on 2nd day";
            if (cronParce[2] == "1-31/2") daysState = "on every 2nd day-of-month from 1 through 31";
            //if (cronParce[2] == "*/" + self.xday() && self.xday() != undefined) daysState = "on every " + self.xday() + " day(s) ";
            //if (cronParce[2] == "*/" + self.mday() && self.mday() != undefined) daysState = "on every " + self.mday() + " day(s) ";

            //Motnhs
            if (cronParce[3] == "*") monthsState = "in every month";
            if (cronParce[3] == "*/2") monthsState = "in every 2nd month";
            if (cronParce[3] == "1-12/2") monthsState = "in every 2nd month from January through December";
            //if (cronParce[3] == "*/" + self.xmonth() && self.xmonth() != undefined) monthsState = "in every " + self.xmonth() + " months(s) ";
            //if (cronParce[3] == "*/" + self.mmonth() && self.mmonth() != undefined) monthsState = "in every " + self.mmonth() + " months(s) ";

            //Week Day
            if (cronParce[4] == "*") weekdayState = "on every weekday";
            if (cronParce[4] == "*/2") weekdayState = "on every 2nd weekday";
            if (cronParce[4] == "0-6/2") weekdayState = "on every 2nd day-of-week from Monday through Sunday";
            //if (cronParce[4] == "*/" + self.xweekday() && self.xweekday() != undefined) weekdayState = "on every " + self.xweekday() + " weekday(s) ";
            //if (cronParce[4] == "*/" + self.mweekday() && self.mweekday() != undefined) weekdayState = "on every " + self.mweekday() + " weekday(s) ";

            return (!string.IsNullOrWhiteSpace(minuteState) ? minuteState + ", " : "") +
                (!string.IsNullOrWhiteSpace(hourState) ? hourState + ", " : "") +
                (!string.IsNullOrWhiteSpace(daysState) ? daysState + ", " : "") +
                (!string.IsNullOrWhiteSpace(monthsState) ? monthsState + ", " : "") +
                (!string.IsNullOrWhiteSpace(weekdayState) ? weekdayState : "");

                //+ hourState + ", " + daysState + ", " + monthsState + ", " + weekdayState;
        }

        private static Dictionary<string, dynamic> FillSubDictionary(string jsonFile)
        {
            Dictionary<string, dynamic> dictionary;
            string rawData = File.ReadAllText(jsonFile, Encoding.Default);

            EncryptionHelper eh = new EncryptionHelper();
            rawData = eh.Decrypt(rawData);
            //dictionary = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(rawData);
            dictionary = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, dynamic>>(rawData);
            return dictionary;
        }

        private void saveResources(Assembly assembly)
        {
            string[] manifestResourceNames = assembly.GetManifestResourceNames();
            foreach (string recource in manifestResourceNames)
            {
                //logger.Info($"Saving resource '{recource}'... ");
                try
                {

                    FileInfo fi = setFileInfo(recource);
                    if (fi == null) continue;
                    string ext = fi.Extension.Replace(".", "");

                    //1. get embeded resource. recource format: "Plugin1.wwwroot.js.plugin.js" 
                    Stream recourceStream = assembly.GetManifestResourceStream(recource);

                    //2. write to file
                    if (ext == "jpg" || ext == "png" || ext == "mp4" || ext == "ico" || ext == "gif" || ext == "spg")
                        saveBinaryResource(fi, recourceStream);
                    else
                        saveTextResource(fi, recourceStream);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex.ToString());
                }
            }

        }

        private FileInfo setFileInfo(string key)
        {
            List<string> parts = key.Split(".").ToList();
            string path = sysInfo.pluginFilePath;
            for (int i = 0; i < parts.Count() - 2; i++)
            {
                path = Path.Combine(path, parts[i]);
            }
            if (checkfolderExist(path))
            {
                string ext = parts[parts.Count() - 1].ToLower();
                path = Path.Combine(path, parts[parts.Count() - 2]);
                path = path + "." + ext;
                return new FileInfo(path);
            }

            return null;
        }

        /// <summary>
        ///  write text resource content to file
        /// </summary>
        /// <param name="fi">FileInfo</param>
        /// <param name="recourceStream">recource Stream</param>
        private void saveTextResource(FileInfo fi, Stream recourceStream)
        {
            try
            {
                lock (lockJsons)
                {
                    string content = string.Empty;
                    using (var reader = new StreamReader(recourceStream, Encoding.Default))
                    {
                        content += reader.ReadToEnd();
                    }
                    System.IO.File.WriteAllText(fi.FullName, content);
                }
            }
            catch(Exception ex)
            {

            }

        }

        /// <summary>
        ///  write binary resource content to file
        /// </summary>
        /// <param name="fi">FileInfo</param>
        /// <param name="recourceStream">recource Stream</param>
        private void saveBinaryResource(FileInfo fi, Stream recourceStream)
        {
            BinaryReader br = new BinaryReader(recourceStream);
            FileStream fs = new FileStream(fi.FullName, FileMode.Create); // say 
            BinaryWriter bw = new BinaryWriter(fs);
            byte[] ba = new byte[recourceStream.Length];
            recourceStream.Read(ba, 0, ba.Length);
            bw.Write(ba);
            br.Close();
            bw.Close();
            recourceStream.Close();
        }
    }

}
