using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Text.Json;
using System.Threading.Tasks;
using HitHelpersNetCore.Models;
using HitServicesCore.Helpers;
using HitServicesCore.Models;
using HitServicesCore.Models.Helpers;
using HitServicesCore.Models.IS_Services;
using HitServicesCore.Models.SharedModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HitServicesCore.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class FetchDataApiController : ControllerBase
    {
        private readonly List<PlugInDescriptors> plugins;
        private readonly ManageConfiguration manconf;
        ILogger<FetchDataApiController> logger;
        private readonly SystemInfo sysInfo;
        private readonly List<SchedulerServiceModel> hangfireServices;
        private readonly HangFire_ManageServices hangfire;


        public FetchDataApiController(ILogger<FetchDataApiController> logger, HangFire_ManageServices _hangfire,List<PlugInDescriptors> _plugins, ManageConfiguration _manconf, SystemInfo _sysInfo, List<SchedulerServiceModel> _hangfireServices, ILogger<FetchDataApiController> _logger)
        {
            this.plugins = _plugins;
            this.manconf = _manconf;
            this.logger = logger;
            this.hangfire = _hangfire;
            hangfireServices = _hangfireServices;
            hangfireServices = hangfireServices;
            sysInfo = _sysInfo;
        }

        [Route("/FetchDataApi/dataApi/GetPlugins")]
        [HttpGet]
        public async Task<string> GetPlugins(long id)
        {
            List<PlugInDescriptors> plg = plugins;
            List<PluginHelper> mainDesc = new List<PluginHelper>();
            foreach (PlugInDescriptors model in plg)
            {
                PluginHelper tempModel = new PluginHelper() ;
                tempModel.plugIn_Id = model.mainDescriptor.plugIn_Id;
                tempModel.plugIn_Description = model.mainDescriptor.plugIn_Description;
                tempModel.plugIn_Name = model.mainDescriptor.plugIn_Name;
                tempModel.plugIn_Version = model.mainDescriptor.plugIn_Version;
                mainDesc.Add(tempModel);
            }

            try
            {
                string json = JsonSerializer.Serialize(mainDesc);
                return json;
            }
            catch (Exception ex)
            {
                string s = ex.ToString();
                return null;
            }
        }


        [Route("/FetchDataApi/dataApi/FireJobExternally/{guid}")]
        [HttpGet]
        public async Task<bool> FireJobExternally(Guid guid)
        {
            try
            {
                    foreach(SchedulerServiceModel service in hangfireServices)
                    if (service.serviceId == guid)
                    {
                        hangfire.FireAndForget(guid);
                        break;
                    }
            }
            catch (Exception ex)
            {
                logger.LogError("Service with ServiceId" + guid + " did not start");
                logger.LogError(Convert.ToString(ex));
                return false;
            }
            return true;
        }


        [Route("/FetchDataApi/dataApi/test")]
        [HttpGet]
        [AllowAnonymous]
        public  IActionResult test()
        {
            return Ok("The test is ok");
        }

        [Route("/FetchDataApi/dataApi/GetSqlScripts")]
        [HttpGet]
        public async Task<List<ISRunSqlScriptsModel>> GetSqlScripts()
        {
            try
            {
                IS_ServicesHelper serviceshelper = new IS_ServicesHelper();
                List<ISRunSqlScriptsModel> res = serviceshelper.GetRunSqlScriptsFromJsonFiles();
                return res;
            }
            catch (Exception ex)
            {
                logger.LogError("Failed to get the list of existing Sql Scripts ");
                logger.LogError("Error:" + Convert.ToString(ex));
                return null;
            }
        }

        [Route("/FetchDataApi/dataApi/GetSaveToTable")]
        [HttpGet]
        public async Task<List<ISSaveToTableModel>> GetSaveToTable()
        {
            try
            {
                IS_ServicesHelper serviceshelper = new IS_ServicesHelper();
                List<ISSaveToTableModel> res = serviceshelper.GetSaveToTableFromJsonFiles();
                return res;
            }
            catch (Exception ex)
            {
                logger.LogError("Failed to get the list of existing SaveToTable Scripts ");
                logger.LogError("Error:" + Convert.ToString(ex));
                return null;
            }
        }


        [Route("/FetchDataApi/dataApi/GetReadFromCsv")]
        [HttpGet]
        public async Task<List<ISReadFromCsvModel>> GetReadFromCsv()
        {
            try
            {
                IS_ServicesHelper serviceshelper = new IS_ServicesHelper();
                List<ISReadFromCsvModel> res = serviceshelper.GetReadFromCsvFromJsonFiles();
                return res;
            }
            catch (Exception ex)
            {
                logger.LogError("Failed to get the list of existing ReadFromCsv Scripts ");
                logger.LogError("Error:" + Convert.ToString(ex));
                return null;
            }
        }

        [Route("/FetchDataApi/dataApi/GetExportData")]
        [HttpGet]
        public async Task<List<ISExportDataModel>> GetExportData()
        {
            try
            {
                IS_ServicesHelper serviceshelper = new IS_ServicesHelper();
                List<ISExportDataModel> res = serviceshelper.GetExportdataFromJsonFiles();
                return res;
            }
            catch (Exception ex)
            {
                logger.LogError("Failed to get the list of existing ExportData Scripts ");
                logger.LogError("Error:" + Convert.ToString(ex));
                return null;
            }
        }



        [Route("Get")]
        [HttpGet]
        public async Task<string> GetMappedConfigurationData(long id)
        {
            List<ConfigurationHelper> MappedConfigList = new List<ConfigurationHelper>();

            List<MainConfigurationModel> configList = manconf.GetConfigs();
            foreach (MainConfigurationModel model in configList)
            {
                if (model.descriptors.descriptions.Count != 0)
                {
                    string pluginid = Convert.ToString(model.plugInId);
                    foreach (KeyValuePair<string, List<DescriptorsModel>> descriptor in model.descriptors.descriptions)
                    {
                        string group = descriptor.Key;
                        ConfigurationHelper mapper;
                        foreach (DescriptorsModel values in descriptor.Value)
                        {
                            mapper = new ConfigurationHelper();
                            mapper.PlugInId = pluginid;
                            //mapper.Group = group;
                            //mapper.Key = values.Key;
                            //mapper.Description = values.Description;
                            //mapper.DefaultValue = values.DefaultValue;
                            //mapper.Type = values.Type;
                            //mapper.ApiVersion = values.ApiVersion;
                            foreach (KeyValuePair<string, dynamic> dynval in model.config.config)
                            {
                                //if (dynval.Key == mapper.Key)
                                //{
                                //    mapper.ActualValue = Convert.ToString(dynval.Value);
                                //    MappedConfigList.Add(mapper);
                                //}
                            }
                        }

                    }
                }
            }
            
            try
            {
                string json = JsonSerializer.Serialize(MappedConfigList);
                return json;
            }
            catch (Exception ex)
            {
                string s = ex.ToString();
                return null;
            }
            
        }

        [Route("Post")]
        [HttpPost]
        public IActionResult Post(ConfigurationHelper data)
        {
            List<MainConfigurationModel> configList = manconf.GetConfigs();
            MainConfigurationModel tempmodel = configList.Where(x => x.plugInId == new Guid(data.PlugInId)).FirstOrDefault();
            configList.Remove(tempmodel);
            Dictionary<string, dynamic> dic = tempmodel.config.config;
            try { 
            foreach (var desc in tempmodel.descriptors.descriptions)
            {
                foreach (DescriptorsModel mod in desc.Value)
                {
                    //if (dic.ContainsKey(data.Key) && mod.Key==data.Key)
                    //{
                    //    switch (mod.Type.ToLower())
                    //    {
                    //        case "string":
                    //            var sval = data.ActualValue;
                    //            dic[mod.Key] = sval;
                    //            break;
                    //        case "bool":
                    //            var bval = false;
                    //            if (data.Key==mod.Key)
                    //                bval = bool.Parse(data.ActualValue);
                    //            dic[mod.Key] = bval;
                    //            break;
                    //        case "int":
                    //            var ival = int.Parse(data.ActualValue);
                    //            dic[mod.Key] = ival;
                    //            break;
                    //        case "decimal":
                    //            var dval = decimal.Parse(data.ActualValue);
                    //            dic[mod.Key] = dval;
                    //            break;
                    //        case "datetime":
                    //            var daval = DateTime.Parse(data.ActualValue);
                    //            dic[mod.Key] = daval;
                    //            break;
                    //        case "int64":
                    //            var i64vall = Int64.Parse(data.ActualValue);
                    //            dic[mod.Key] = i64vall;
                    //            break;
                    //        case "int32":
                    //            var i32val = Int64.Parse(data.ActualValue);
                    //            dic[mod.Key] = i32val;
                    //            break;
                    //        case "float":
                    //            var fval = float.Parse(data.ActualValue);
                    //            dic[mod.Key] = fval;
                    //            break;
                    //        case "double":
                    //            var doval = double.Parse(data.ActualValue);
                    //            dic[mod.Key] = doval;
                    //            break;
                    //        default:
                    //            // config[dspVal.Key] = dspVal.DefaultValue;
                    //            break;
                    //    }
                    //}
                    //else
                    //{
                    //    continue;
                    //}
                }
            }
            }
            catch(Exception ex)
            {
                logger.LogError(ex.ToString());
            }
            configList.Remove(tempmodel);
            tempmodel.config.config = dic;
            configList.Add(tempmodel);
            return Ok();
        }

        [HttpPut]
        public IActionResult Put(string key, string values)
        {
            return Ok();
        }
    }
}
