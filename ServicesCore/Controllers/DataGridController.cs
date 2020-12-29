
using HitServicesCore.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using HitServicesCore.Filters;
using HitServicesCore.Helpers;
using Microsoft.Extensions.Logging;
using HitHelpersNetCore.Models;
using HitServicesCore.Models.SharedModels;
using HitHelpersNetCore.Helpers;
using HitServicesCore.Models.Helpers;

namespace HitServicesCore.Controllers
{
    public class DataGridController : Controller
    {
        private readonly List<PlugInDescriptors> plugins;
        private readonly ManageConfiguration manconf;
        private readonly List<MainConfigurationModel> mainconfModel;
        private static string currentPluginId;
        private readonly LoginsUsers loginsUsers;
        ILogger<DataGridController> logger;
        public DataGridController(LoginsUsers loginsUsers, ILogger<DataGridController> _logger, ManageConfiguration _manconf, List<PlugInDescriptors> _plugins, List<MainConfigurationModel> _mainconfModel)
        {
            this.manconf = _manconf;
            this.plugins = _plugins;
            this.mainconfModel = _mainconfModel;
            this.loginsUsers = loginsUsers;
            this.logger = _logger;
        }
            [ServiceFilter(typeof(LoginFilter))]
        public ActionResult Configuration(string pluginId)
        {
            List<string> keys = new List<string>();
            ViewBag.isAdmin = false;
            MainConfigurationModel myModel = new MainConfigurationModel();
            List<PlugInDescriptors> plgs = plugins;
            List<MainConfigurationModel> configList = manconf.GetConfigs();

            ViewBag.plugins = plgs;
            if(pluginId !=null)
            myModel = configList.Where(x => x.plugInId == new Guid(pluginId)).FirstOrDefault();
            else
                return View();
            if (myModel != null)
            {
                foreach (KeyValuePair<string, System.Collections.Generic.List<HitHelpersNetCore.Models.DescriptorsModel>> item in myModel.descriptors.descriptions)
                    foreach (HitHelpersNetCore.Models.DescriptorsModel mod in item.Value)
                        if (mod.Type.Contains("list,"))
                            keys.Add(mod.Key);
            }
            Dictionary<string, dynamic> values;
            ViewBag.keys = keys;
            if (myModel != null)
            {
                values = myModel.config.config;
                ViewBag.values = values;
            }
            
            ViewBag.MyModel = myModel;
            ViewBag.plugInId = pluginId;
            
            currentPluginId = ViewBag.plugInId;
            if (pluginId != "{00000000-0000-0000-0000-000000000000}")
                ViewBag.plugInname = plgs.Where(x => x.mainDescriptor.plugIn_Id == new Guid(pluginId)).FirstOrDefault().mainDescriptor.plugIn_Name;
            else
                ViewBag.plugInname = "Main Configuration";
            ViewBag.plugins = plgs;
            if (loginsUsers.logins["isAdmin"] == true)
                ViewBag.isAdmin = true;

            List<PlugInDescriptors> plg = plugins;
            List<MainDescriptorWithAssemplyModel> mainDesc = new List<MainDescriptorWithAssemplyModel>();
            foreach (PlugInDescriptors model in plg)
                mainDesc.Add(model.mainDescriptor);
            ViewBag.plgs = mainDesc;
            return View();
        }
        public class DictWrapper
        {
            Dictionary<string, dynamic> config = new Dictionary<string, dynamic>();
            public static DictWrapper Create(Dictionary<string, dynamic> nestedDict)
            {

                DictWrapper wrapper = new DictWrapper();
                wrapper.config = nestedDict;
                return wrapper;
            }

            public dynamic this[string someName]
            {
                get
                {
                    return config[someName];
                }
                set
                {
                    config[someName] = value;
                }
            }
        }
        [HttpPost]
        public async Task<IActionResult> SaveEditedData(Dictionary<string, string> SaveEditedData)
        {
            logger.LogInformation("Initiating Saving of Data");
            try { 
            MainConfigurationModel myModel = new MainConfigurationModel();
            List<MainConfigurationModel> configList = manconf.GetConfigs();
            if (currentPluginId == null) currentPluginId = "{00000000-0000-0000-0000-000000000000}";
             myModel = configList.Where(x => x.plugInId == new Guid(currentPluginId)).FirstOrDefault();
            if (myModel == null)
                myModel = configList[0];
            List<PlugInDescriptors> _plugins = plugins;
            PlugInDescriptors plugin = _plugins.Where(x => x.mainDescriptor.plugIn_Id == new Guid(currentPluginId)).FirstOrDefault();
            Dictionary<string, dynamic> dic = myModel.config.config;
            DictWrapper wrap = DictWrapper.Create(dic);

            foreach (var desc in myModel.descriptors.descriptions)
            {
                foreach (DescriptorsModel mod in desc.Value)
                {
                    if (dic.ContainsKey(mod.Key) && SaveEditedData.ContainsKey(mod.Key))
                    {
                        switch (mod.Type.ToLower())
                        {
                            case "list,db":
                                var lsval = SaveEditedData[mod.Key];
                                string[] lines = lsval.Split(new string[] { "#" }, StringSplitOptions.None);
                                List<string> listofdb = new List<string>(lines.Where(x=>x!=""));
                                dic[mod.Key] = listofdb;
                                break;
                                case "list,string":
                                    var lssval = SaveEditedData[mod.Key];
                                    string[] linesstring = lssval.Split(new string[] { "#" }, StringSplitOptions.None);
                                    List<string> listofstring = new List<string>(linesstring.Where(x => x != ""));
                                    dic[mod.Key] = listofstring;
                                    break;
                                case "list,mpehotel":
                                    var lsmp = SaveEditedData[mod.Key];
                                    string[] hotelstrings = lsmp.Split(new string[] { "#" }, StringSplitOptions.None);
                                    List<string> listofmpeconfig = new List<string>(hotelstrings.Where(x => x != ""));
                                    List<HotelConfigModel> hotelModels = new List<HotelConfigModel>();
                                    foreach (string hotelconfig in listofmpeconfig)
                                    {
                                        HotelConfigModel model = new HotelConfigModel();
                                      string[] temp = hotelconfig.Split(new string[] { ">" }, StringSplitOptions.None);
                                        model.Db = temp[0].Trim(' ');
                                        model.mpehotel = temp[0].Substring(temp[0].Length - 1);
                                        string[] temp2 = temp[1].Split(new string[] { "<" }, StringSplitOptions.None);
                                        model.HotelName = temp2[0];
                                        model.Value = temp2[1];
                                        hotelModels.Add(model);
                                    }
                                    dic[mod.Key] = hotelModels;
                                    break;
                                case "customdb":
                                var dbval = SaveEditedData[mod.Key];
                                dic[mod.Key] = dbval;
                                break;
                                case "maindb":
                                    var mdb = SaveEditedData[mod.Key];
                                    dic[mod.Key] = mdb;
                                    break;
                            case "string":
                                var sval = SaveEditedData[mod.Key];
                                dic[mod.Key] = sval;
                                break;
                            case "bool":
                                var bval = false;
                                if (SaveEditedData.ContainsKey(mod.Key))
                                    bval = bool.Parse(SaveEditedData[mod.Key]);
                                dic[mod.Key] = bval;
                                break;
                            case "int":
                                var ival = int.Parse(SaveEditedData[mod.Key]);
                                dic[mod.Key] = ival;
                                break;
                            case "decimal":
                                var dval = decimal.Parse(SaveEditedData[mod.Key]);
                                dic[mod.Key] = dval;
                                break;
                            case "datetime":
                                    var daval = DateTime.Parse(SaveEditedData[mod.Key]);
                                    dic[mod.Key] = daval;
                                    break;
                            default:
                                // config[dspVal.Key] = dspVal.DefaultValue;
                                break;
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            configList.Remove(myModel);
            myModel.config.config = dic;
            configList.Add(myModel);

            manconf.SaveConfigs(configList);
            }
            catch(Exception e)
            {
                logger.LogError("Error while Saving Data : " + e);
                BadRequest();
            }
            return Ok();
        }
        [HttpPost]
        public IActionResult CheckConnections(string constr)
        {
            CheckConnectionsHelper cch = new CheckConnectionsHelper();
            string res = cch.CheckConnection(constr);
            
            return Ok(res);
        }
    }
}