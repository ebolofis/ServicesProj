using Hangfire;
using HitServicesCore.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HitHelpersNetCore.Models;
using HitCustomAnnotations.Interfaces;
using HitCustomAnnotations.Classes;
using HitServicesCore.Models.SharedModels;
using HitHelpersNetCore.Interfaces;
using HitHelpersNetCore.Models.SharedModels;
using HitServicesCore.Enum;

namespace HitServicesCore.Helpers
{
    public class PlugInAnnotationedClassesHelper
    {
        private readonly SystemInfo sysInfo;

        private string CurrentPath;


        public PlugInAnnotationedClassesHelper(SystemInfo _sysInfo)
        {
            sysInfo = _sysInfo;

            CurrentPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            System.IO.Directory.SetCurrentDirectory(CurrentPath);
        }


        #region Manage all plugin dll classes 
        /// <summary>
        /// check if dll has implement IMainDescriptor interface
        /// </summary>
        /// <param name="assembly"></param>
        private bool CheckIfPlugInCanAddedToService(Assembly assembly)
        {
            bool result = false;
            foreach (Type item in assembly.GetExportedTypes())
            {
                if (typeof(IMainDescriptor).IsAssignableFrom(item))
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// Adds classes from dll to DI on StartUp
        /// </summary>
        /// <param name="services"></param>
        public void AddClassesToDI_OnStartUp(IServiceCollection services)
        {
            var ps = new List<string>() { CurrentPath, "Config", "NLog.config" };
            var logpath = Path.GetFullPath(Path.Combine(ps.ToArray()));
            var logger = NLog.Web.NLogBuilder.ConfigureNLog(logpath).GetCurrentClassLogger();

            if (checkfolderExist(sysInfo.pluginPath, logger))
            {

                string[] plugIns = Directory.GetDirectories(sysInfo.pluginPath);
                Attribute tblAttr;

                foreach (string plgPath in plugIns)
                {

                    var assemblyFiles = Directory.GetFiles(plgPath, "*.dll", SearchOption.AllDirectories);

                    foreach (var assemblyFile in assemblyFiles)
                    {
                        try
                        {
                            Assembly assembly = Assembly.LoadFrom(assemblyFile);
                            if (CheckIfPlugInCanAddedToService(assembly))
                            {
                                foreach (Type item in assembly.GetExportedTypes())
                                {
                                    tblAttr = item.GetCustomAttribute(typeof(AddClassesToContainer));
                                    if (tblAttr != null)
                                        AddClassesToContainer(services, item, tblAttr);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            logger.Error(ex.ToString());
                        }
                    }

                }

            }
        }

        /// <summary>
        /// Checks all plugin class to find if the interface IMainDescriptor exists
        /// </summary>
        /// <param name="assemblyFile"></param>
        /// <param name="plgInDescr"></param>
        /// <param name="plgPath"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool AddPlugInToPlugInDecriptors(string assemblyFile, List<PlugInDescriptors> plgInDescr, string plgPath, out string error)
        {
            bool result = false;
            error = "";
            try
            {
                //Assembly assembly = Assembly.LoadFile(assemblyFile);//  .LoadFrom(assemblyFile);
                Assembly assembly = Assembly.LoadFrom(assemblyFile);

                PlugInDescriptors plgModel = null;

                foreach (Type item in assembly.GetExportedTypes())
                {
                    //Class is IMainDescriptor
                    if (typeof(IMainDescriptor).IsAssignableFrom(item))
                    {
                        var tmpActiv = Activator.CreateInstance(item);

                        object tmpObj = item.GetProperty("plugIn_Id").GetValue(tmpActiv);

                        if (tmpObj != null)
                        {
                            plgModel = plgInDescr.Find(f => f.mainDescriptor != null && f.mainDescriptor.plugIn_Id == Guid.Parse(tmpObj.ToString()));

                            //Initialize PlugInDescriptors model
                            if (plgModel == null)
                            {
                                plgModel = new PlugInDescriptors();
                                plgModel.mainDescriptor = new MainDescriptorWithAssemplyModel();
                                plgInDescr.Add(plgModel);

                            }
                            else
                            {
                                if (plgModel.mainDescriptor == null)
                                    plgModel.mainDescriptor = new MainDescriptorWithAssemplyModel();
                            }
                        }
                        //Plug In Unique Guid Id
                        plgModel.mainDescriptor.plugIn_Id = Guid.Parse(tmpObj.ToString());

                        //Plug In Name
                        tmpObj = item.GetProperty("plugIn_Name").GetValue(tmpActiv);
                        plgModel.mainDescriptor.plugIn_Name = tmpObj.ToString();

                        //Plug In Description
                        tmpObj = item.GetProperty("plugIn_Description").GetValue(tmpActiv);
                        if (tmpObj != null)
                            plgModel.mainDescriptor.plugIn_Description = tmpObj.ToString();

                        //Plug In File Path
                        plgModel.mainDescriptor.path = plgPath; // tmpObj.ToString();

                        //Plug In File Name
                        //tmpObj = item.GetProperty("fileName").GetValue(tmpActiv);
                        //if (tmpObj != null)
                        plgModel.mainDescriptor.fileName = item.Name;

                        //Plug In Full Name space
                        plgModel.mainDescriptor.fullNameSpace = item.FullName;

                        //Plug In Version
                        //tmpObj = item.GetProperty("plugIn_Version").GetValue(tmpActiv);
                        //if (tmpObj != null)
                        System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
                        plgModel.mainDescriptor.plugIn_Version = fvi.FileVersion;// tmpObj.ToString();

                        //Add assembly file to mainDescriptor
                        plgModel.mainDescriptor.assembly = assembly;

                        string[] tmpLst = assemblyFile.Split("\\");
                        string asemblFileName = tmpLst[tmpLst.Length - 1];
                        asemblFileName = asemblFileName.ToLower().Replace(".dll", "");

                        FindAllOtherDescriptors(assembly, plgModel, plgPath, asemblFileName, out error);

                        result = true;
                        break;
                    }
                }

            }
            catch (Exception ex)
            {
                error = ex.ToString();
            }
            return result;
        }

        /// <summary>
        /// Checks all other classes except IMainDescriptor to add them to PlugInDescriptor
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="plgInDescr"></param>
        /// <param name="sPath"></param>
        /// <param name="error"></param>
        private void FindAllOtherDescriptors(Assembly assembly, PlugInDescriptors plgInDescr, string sPath, string assemblyFileName, out string error)
        {
            error = "";
            Attribute tblAttr;
            try
            {
                foreach (Type item in assembly.GetExportedTypes())
                {
                    //check if custom attribute for add classes to container exists
                    
                    if (item.GetCustomAttribute(typeof(AddClassesToContainer)) != null)
                    {
                        tblAttr = item.GetCustomAttribute(typeof(AddClassesToContainer));

                        if (plgInDescr.dIDescriptor == null)
                            plgInDescr.dIDescriptor = new List<DIDescriptorWithTypeModel>();

                        //New model to added on list
                        DIDescriptorWithTypeModel diDescr = new DIDescriptorWithTypeModel();
                        
                        //Description for the class
                        diDescr.di_ClassDescription = (tblAttr as AddClassesToContainer).GetDescription();
                        
                        //Assemply type
                        diDescr.classType = item;
                        
                        //Assembly file name
                        diDescr.fileName = item.Name;
                        
                        //assembly file path
                        diDescr.path = sPath;
                        
                        //Assembly full name space
                        diDescr.fullNameSpace = item.FullName;
                        
                        //Class scope type
                        diDescr.scope = (tblAttr as AddClassesToContainer).GetServiceType();

                        plgInDescr.dIDescriptor.Add(diDescr);

                        //Added to container but the class inherited the AbstractConfigurationHelper
                        if (item.BaseType.Name == "AbstractConfigurationHelper")
                        {
                            ConfigDescriptorModel confClass = new ConfigDescriptorModel();

                            //Class load type
                            confClass.confFile = item;
                            //Full class name with namespace
                            confClass.fullClassName = item.FullName;

                            plgInDescr.configClass = confClass;
                        }

                    }  //Get all classes to run as services
                    else if (item.GetCustomAttribute(typeof(SchedulerAnnotation)) != null)
                    {
                        //check if class inheritred the Interface IServiceExecutions. Must have to inherited 
                        if (item.BaseType.Name == "ServiceExecutions")
                        {
                            tblAttr = item.GetCustomAttribute(typeof(SchedulerAnnotation));

                            if (plgInDescr.serviceDescriptor == null)
                                plgInDescr.serviceDescriptor = new List<ServiceDescriptorWithTypeModel>();

                            ServiceDescriptorWithTypeModel servDescr = new ServiceDescriptorWithTypeModel();

                            //Service Load class
                            servDescr.classType = item;
                            //Service File Name
                            servDescr.fileName = item.Name;
                            //Service Full name space
                            servDescr.fullNameSpace = item.FullName;
                            //Service Path
                            servDescr.path = sPath;
                            //Service Name
                            servDescr.seriveName = (tblAttr as SchedulerAnnotation).GetServiceName();
                            //Service Description
                            servDescr.serviceDescription = (tblAttr as SchedulerAnnotation).GetDescription();
                            //Service Unique Guid Id
                            servDescr.serviceId = Guid.Parse((tblAttr as SchedulerAnnotation).GetId());
                            //Service Version
                            servDescr.serviceVersion = (tblAttr as SchedulerAnnotation).GetVersion();
                            //Real dll file name
                            servDescr.assemblyFileName = assemblyFileName;

                            plgInDescr.serviceDescriptor.Add(servDescr);
                        }
                    }
                    else //Get's all initial for data base classes
                    if (typeof(IInitialerDescriptor).IsAssignableFrom(item))
                    {
                        var tmpActiv = Activator.CreateInstance(item);

                        if (plgInDescr.initialerDescriptor == null)
                            plgInDescr.initialerDescriptor = new InitialerDescriptorModel();

                        //Initializer File name
                        //object tmpObj = item.GetProperty("fileName").GetValue(tmpActiv);
                        //if (tmpObj != null)
                        //    plgInDescr.initialerDescriptor.fileName = tmpObj.ToString();

                        //Initializer Path
                        //plgInDescr.initialerDescriptor.path = sPath;

                        //Initializer Full name space
                        //plgInDescr.initialerDescriptor.fullNameSpace = item.FullName;

                        //Initializer Version
                        object tmpObj = item.GetProperty("dbVersion").GetValue(tmpActiv);
                        if (tmpObj != null)
                            plgInDescr.initialerDescriptor.dbVersion = tmpObj.ToString();

                        //Service Full name space
                        plgInDescr.initialerDescriptor.fullNameSpace = item.FullName;
                        //Real dll file name
                        plgInDescr.initialerDescriptor.assemblyFileName = assemblyFileName;

                        //Plug In Guid where the Initializer belongs to
                        plgInDescr.initialerDescriptor.plugIn_Id = plgInDescr.mainDescriptor.plugIn_Id;
                    }
                    else //this class is for configuration helper inherited from AbstractConfigurationHelper
                    if (item.BaseType.Name == "AbstractConfigurationHelper")
                    {
                        ConfigDescriptorModel confClass = new ConfigDescriptorModel();

                        //Class load type
                        confClass.confFile = item;
                        //Full class name with namespace
                        confClass.fullClassName = item.FullName;

                        plgInDescr.configClass = confClass;
                    }
                    
                }
            }
            catch (Exception ex)
            {
                error = ex.ToString();
            }
        }

        /// <summary>
        /// Check if all items to a configuration exists and has values based on descriptor
        /// </summary>
        /// <param name="config"></param>
        /// <param name="descriptor"></param>
        public void CheckIfNullValuesExistsOnConfiguration(Dictionary<string, dynamic> config, Dictionary<string, List<HitHelpersNetCore.Models.DescriptorsModel>> descriptor )
        {
            foreach (var item in descriptor)
            {
                List<DescriptorsModel> dsp = item.Value;
                foreach (var dspVal in dsp)
                {
                    if (!config.ContainsKey(dspVal.Key) ||
                        string.IsNullOrEmpty(config[dspVal.Key]?.ToString()))
                    {
                        switch (dspVal.Type.ToLower())
                        {
                            case "string":
                                config[dspVal.Key] = dspVal.DefaultValue;
                                break;
                            case "bool":
                                config[dspVal.Key] = bool.Parse(dspVal.DefaultValue);
                                break;
                            case "int":
                                config[dspVal.Key] = int.Parse(dspVal.DefaultValue);
                                break;
                            case "decimal":
                                config[dspVal.Key] = decimal.Parse(dspVal.DefaultValue);
                                break;
                            case "datetime":
                                config[dspVal.Key] = DateTime.Parse(dspVal.DefaultValue);
                                break;
                            case "int64":
                                config[dspVal.Key] = Int64.Parse(dspVal.DefaultValue);
                                break;
                            case "int32":
                                config[dspVal.Key] = Int32.Parse(dspVal.DefaultValue);
                                break;
                            case "float":
                                config[dspVal.Key] = float.Parse(dspVal.DefaultValue);
                                break;
                            case "double":
                                config[dspVal.Key] = double.Parse(dspVal.DefaultValue);
                                break;
                            default:
                                config[dspVal.Key] = dspVal.DefaultValue;
                                break;
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Add classes to container
        /// </summary>
        /// <param name="services"></param>
        /// <param name="classToAdd"></param>
        /// <param name="tblAttr"></param>
        private void AddClassesToContainer(IServiceCollection services, Type classToAdd, Attribute tblAttr)
        {
            ServicesAddTypeEnum serviceType = (tblAttr as AddClassesToContainer).GetServiceType();
            switch (serviceType)
            {
                case ServicesAddTypeEnum.addScoped:
                    services.AddScoped(classToAdd);
                    break;
                case ServicesAddTypeEnum.addSingleton:
                    services.AddSingleton(classToAdd);
                    break;
                case ServicesAddTypeEnum.addTransient:
                    services.AddTransient(classToAdd);
                    break;
                default:
                    break;
            }
        }

        #endregion


        /// <summary>
        /// upserts jobs to hangfire
        /// </summary>
        /// <param name="schedulerClient"></param>
        /// <param name="item"></param>
        /// <param name="execCron"></param>
        //private void UpsertSchedulerJobs(RecurringJobManager schedulerClient, Type item, CronsModel execCron)
        //{
        //    // Create an instance of that type
        //    Object obj = Activator.CreateInstance(item);
        //    // Retrieve the method you are looking for
        //    MethodInfo methodInfo = item.GetMethod("Start");

        //    object parameters = null;

        //    if (!string.IsNullOrWhiteSpace(execCron.parameters))
        //    {
        //        object tmpObj = (JObject)JsonConvert.DeserializeObject<object>(ReturnDynamicObjectFromString(execCron.parameters));
        //        parameters = tmpObj;
        //    }

        //    Hangfire.Common.Job job = new Hangfire.Common.Job(methodInfo, parameters);

        //    // Invoke the method on the instance we created above
        //    if (obj != null && methodInfo != null)
        //    {
        //        if (execCron.isActive)
        //            schedulerClient.AddOrUpdate(execCron.classFullName, job, execCron.classCron);
        //        //.Schedule(() => methodInfo.Invoke(obj, parameters), TimeSpan.FromSeconds(1));
        //        else
        //            schedulerClient.RemoveIfExists(execCron.classFullName);
        //    }
        //}

        /// <summary>
        /// Returns a string as JObject or desirialixe to object and use it as JObject
        /// ((JObject)tmpObject).GetValues("parameters name").ToString();
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string ReturnDynamicObjectFromString(string value)
        {
            Dictionary<string, object> objDiction = new Dictionary<string, object>();
            string[] objProperties = value.Split(',');


            foreach (string item in objProperties)
            {
                string[] objPropValues = item.Split('=');

                objDiction.Add(objPropValues[0], objPropValues[1]);
            }
            return System.Text.Json.JsonSerializer.Serialize(DictionaryToObject(objDiction));
            //return JsonConvert.SerializeObject(DictionaryToObject(objDiction), Formatting.Indented);
        }

        /// <summary>
        /// Converts a dictionary to dynamic
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        private dynamic DictionaryToObject(IDictionary<string, object> source)
        {
            var expandoObj = new ExpandoObject();
            var expandoObjCollection = (ICollection<KeyValuePair<String, Object>>)expandoObj;

            foreach (var keyValuePair in source)
            {
                expandoObjCollection.Add(keyValuePair);
            }
            dynamic eoDynamic = expandoObj;
            return eoDynamic;
        }


        /// <summary>
        /// chack folder existase and optionally create folder hierarchy
        /// </summary>
        /// <param name="path">path</param>
        /// <param name="create">true: create if path does not exist</param>
        private bool checkfolderExist(string path, NLog.Logger logger, bool create = true, bool delete = false)
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
                    logger.Error(ex.ToString());
                    return false;
                }
            }
            return res;
        }

    }
}
