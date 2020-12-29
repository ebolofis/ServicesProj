using HitHelpersNetCore.Models;
using HitServicesCore.Models.Helpers;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HitHelpersNetCore.Helpers
{
    public abstract class AbstractConfigurationHelper
    {
        /// <summary>
        /// configuration App
        /// </summary>
        static List<MainConfigurationModel> Configuration;

        //private List<MainConfigurationModel> configs;

        //private List<PlugInDescriptors> plugIns;

        private string CurrentPath;

        /// <summary>
        /// lock read, write jsons file
        /// </summary>
        private object lockJsons = new object();

        /// <summary>
        /// Instance for string encryption
        /// </summary>
        private readonly EncryptionHelper eh;

        //private string tmpServiceName;

        /// <summary>
        /// File to read and convert to dictionary
        /// </summary>
        /// <param name="_fileName"></param>
        public AbstractConfigurationHelper(/*List<MainConfigurationModel> _configs, List<PlugInDescriptors> _plugIns*/)
        {
            //configs = _configs;
            //plugIns = _plugIns;
            CurrentPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            Directory.SetCurrentDirectory(CurrentPath);

            eh = new EncryptionHelper();
        }

        /// <summary>
        /// Return a MainConfigurationModel based on plugin
        /// </summary>
        /// <param name="plugIn_Id"></param>
        /// <param name="configPath"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public MainConfigurationModel ReadConfiguration()
        {
            //Logger
            var ps = new List<string>() { CurrentPath, "Config", "NLog.config" };
            var logpath = Path.GetFullPath(Path.Combine(ps.ToArray()));
            var logger = NLog.Web.NLogBuilder.ConfigureNLog(logpath).GetCurrentClassLogger();

            //Guid Id to find it on list of config models
            Guid? guidId = null;

            MainConfigurationModel result = null;

            //Try to get the model from static MainConfigurationModel list
            if (Configuration != null)
            {
                result = Configuration.Find(f => /*f.plugInId == guidId &&*/ f.configClassName == this.GetType().FullName);

                if (result != null)
                {
                    if (guidId != null)
                    {
                        result.plugInId = guidId;
                        AddConfigToStaticConfiguration(result);
                    }
                }
                if (result != null)
                    return result;
            }

            result = new MainConfigurationModel();

            //tmpServiceName = this.GetType().Assembly.GetName().Name;
            
            //Not found on lists and get data from file
            string configPath = Path.GetDirectoryName(this.GetType().Assembly.Location);
            try
            {
                //logger.Info("Read Config Path from " + configPath);
                result = GetConfigurationsFromfile(configPath, guidId, logger);
                result.configClassName = this.GetType().FullName;
                if (guidId != null)
                    result.plugInId = guidId;

                if (result.config != null)
                {
                    if (Configuration == null)
                        Configuration = new List<MainConfigurationModel>();
                    Configuration.Add(result);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return result;
        }

        /// <summary>
        /// Writes a config Model to file
        /// </summary>
        /// <param name="configToSave"></param>
        /// <param name="error"></param>
        public void SaveConfiguration(MainConfigurationModel configToSave)
        {
            //Logger
            var ps = new List<string>() { CurrentPath, "Config", "NLog.config" };
            var logpath = Path.GetFullPath(Path.Combine(ps.ToArray()));
            var logger = NLog.Web.NLogBuilder.ConfigureNLog(logpath).GetCurrentClassLogger();

            string configPath = "";
            string json = "";
            try
            {
                lock (lockJsons)
                {
                    MainConfigurationModel fld = null;
                    if (configToSave.plugInId == null)
                        fld = Configuration.Find(f => f.configClassName == this.GetType().FullName);
                    else
                    {
                        fld = Configuration.Find(f => f.plugInId == configToSave.plugInId);
                        if (fld == null)
                            fld = Configuration.Find(f=> f.configClassName == this.GetType().FullName);
                    }
                    
                    
                    if (fld == null && fld.config != null)
                    {
                        Configuration.Add(configToSave);
                        fld = Configuration.Find(f => f.plugInId == configToSave.plugInId || f.configClassName == this.GetType().FullName);
                    }
                    else
                        fld = configToSave;

                    json = JsonSerializer.Serialize(configToSave.config.config);

                    json = eh.Encrypt(json);

                    configPath = Path.GetFullPath(Path.Combine(new string[] { configToSave.config.basePath, configToSave.config.fileName }));
                    File.WriteAllText(configPath, json, Encoding.Default);

                    //Add or update new configuration to static list on Abstract config class
                    AddConfigToStaticConfiguration(configToSave);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
        }

        /// <summary>
        /// Gets the configuration file for HitServicecore project
        /// </summary>
        /// <returns></returns>
        public MainConfigurationModel ReadHitServiceCoreConfig()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var isDevelopment = environment == Environments.Development;

            //Logger
            var ps = new List<string>() { CurrentPath, "Config", "NLog.config" };
            var logpath = Path.GetFullPath(Path.Combine(ps.ToArray()));
            var logger = NLog.Web.NLogBuilder.ConfigureNLog(logpath).GetCurrentClassLogger();

            //Guid Id to find it on list of config models
            Guid? guidId = Guid.Empty;

            MainConfigurationModel result = null;

            string mainconfigPath, descriptoPath;

            if (isDevelopment)
            {

                //logger.Info("HitServiceCore Run as Developer mode");

                mainconfigPath = Path.GetFullPath(Path.Combine(new string[] { CurrentPath, "Config", "HitServiceCore.json" }));
                descriptoPath = Path.GetFullPath(Path.Combine(new string[] { CurrentPath, "Descriptors", "HitServiceCoreDescriptor.json" }));
            }
            else
            {
                string sPath = Path.GetDirectoryName(this.GetType().Assembly.Location);
                mainconfigPath = Path.GetFullPath(Path.Combine(new string[] { sPath, "Config", "HitServiceCore.json" }));
                descriptoPath = Path.GetFullPath(Path.Combine(new string[] { sPath, "Descriptors", "HitServiceCoreDescriptor.json" }));
            }

            //logger.Info("HitServiceCore Config Path : " + mainconfigPath);
            //logger.Info("HitServiceCore Descriptor Path : " + descriptoPath);

            if (Configuration != null)
                result = Configuration.Find(f => f.plugInId == Guid.Empty);

            if (result == null)
                result = GetModelByName(mainconfigPath, descriptoPath, Guid.Empty, logger);
            if (string.IsNullOrWhiteSpace(result.configClassName))
                result.configClassName = "HitServicesCore.Helpers.MainConfigHelper";

            //Add model to static Configuration
            if (result.config != null)
            {
                if (Configuration == null)
                {
                    Configuration = new List<MainConfigurationModel>();
                    Configuration.Add(result);
                }
                else if (Configuration.Find(f => f.plugInId == Guid.Empty) == null)
                    Configuration.Add(result);
            }


            return result;
        }

        /// <summary>
        /// Read config files for specific file name
        /// </summary>
        /// <param name="sFileName"></param>
        /// <param name="descrFileName"></param>
        /// <param name="plugInId"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        private MainConfigurationModel GetModelByName(string sFileName, string descrFileName, Guid? plugInId, NLog.Logger logger)
        {
            MainConfigurationModel result = new MainConfigurationModel();
            try
            {
                lock (lockJsons)
                {
                    if (File.Exists(sFileName))
                    {
                        string[] tmpNames = sFileName.Split("\\");
                        //Config file name
                        string flName = tmpNames[tmpNames.Length - 1];
                        string flPath = "";

                        for (int i = 0; i < tmpNames.Length - 1; i++)
                            flPath += tmpNames[i] + "\\";
                        //Descriptor file name
                        string descrFlName = "";
                        string descrFlPath = "";
                        if (File.Exists(descrFileName))
                        {
                            tmpNames = descrFileName.Split("\\");
                            descrFlName = tmpNames[tmpNames.Length - 1];
                            for (int i = 0; i < tmpNames.Length - 1; i++)
                                descrFlPath += tmpNames[i] + "\\";
                        }

                        result.config = new MainConfiguration();

                        result.config.basePath = flPath;
                        result.config.fileName = flName;
                        string sVal = System.IO.File.ReadAllText(sFileName, Encoding.Default);
                        
                        //Decrypt string
                        sVal = eh.Decrypt(sVal);

                        //Get the data from json file.  We have to convert them so we will have the lists ready for use.
                        Dictionary<string, dynamic> firstData = JsonSerializer.Deserialize<Dictionary<string, dynamic>>(sVal);

                        result.descriptors = new MainConfigDescriptors();
                        result.descriptors.basePath = descrFlPath;
                        result.descriptors.fileName = descrFlName;

                        if (File.Exists(descrFileName))
                        {
                            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                            Encoding enc = GetEncoding(descrFileName);

                            sVal = System.IO.File.ReadAllText(descrFileName, enc /*Encoding.Default*/);
                            result.descriptors.descriptions = JsonSerializer.Deserialize<Dictionary<string, List<DescriptorsModel>>>(sVal);
                        }
                        else
                        {
                            logger.Info("File " + sFileName + " not exists.");
                            result.descriptors.descriptions = new Dictionary<string, List<DescriptorsModel>>();
                        }

                        //Data after conversion for lists. This will be the final config for the user
                        Dictionary<string, dynamic> finalData = ConvertValuesForDictionary(firstData, result.descriptors.descriptions);
                        //Add to results the converted dictionarys
                        result.config.config = finalData;

                        if (plugInId != null)
                            result.plugInId = plugInId ?? Guid.Empty;


                        //Check main config and set null values to default from descriptor
                        CheckIfNullValuesExistsOnConfiguration(result.config.config, result.descriptors.descriptions);

                        //Remove keys from config where not in descriptor
                        RemoveKeysNotExistsOnDescriptor(result.config.config, result.descriptors.descriptions);

                        //Convert to real type based on descriptor
                        ConvertConfigurationValuestoRealType(result.config.config, result.descriptors.descriptions);
                    }
                    else
                        logger.Info("File " + sFileName + " not exists.");
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return result;
        }

        /// <summary>
        /// Check file and return encondig (UTF-8, UTF-8 BOM or 1253)
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        private Encoding GetEncoding(string filename)
        {
            Byte[] bytes = File.ReadAllBytes(filename);
            Encoding encoding = null;
            String text = null;
            // Test UTF8 with BOM. This check can easily be copied and adapted
            // to detect many other encodings that use BOMs.
            UTF8Encoding encUtf8Bom = new UTF8Encoding(true, true);
            Boolean couldBeUtf8 = true;
            Byte[] preamble = encUtf8Bom.GetPreamble();
            Int32 prLen = preamble.Length;
            if (bytes.Length >= prLen && preamble.SequenceEqual(bytes.Take(prLen)))
            {
                // UTF8 BOM found; use encUtf8Bom to decode.
                try
                {
                    // Seems that despite being an encoding with preamble,
                    // it doesn't actually skip said preamble when decoding...
                    text = encUtf8Bom.GetString(bytes, prLen, bytes.Length - prLen);
                    encoding = encUtf8Bom;
                }
                catch (ArgumentException)
                {
                    // Confirmed as not UTF-8!
                    couldBeUtf8 = false;
                }
            }
            // use boolean to skip this if it's already confirmed as incorrect UTF-8 decoding.
            if (couldBeUtf8 && encoding == null)
            {
                // test UTF-8 on strict encoding rules. Note that on pure ASCII this will
                // succeed as well, since valid ASCII is automatically valid UTF-8.
                UTF8Encoding encUtf8NoBom = new UTF8Encoding(false, true);
                try
                {
                    text = encUtf8NoBom.GetString(bytes);
                    encoding = encUtf8NoBom;
                }
                catch (ArgumentException)
                {
                    // Confirmed as not UTF-8!
                }
            }
            // fall back to default ANSI encoding.
            if (encoding == null)
            {
                encoding = Encoding.GetEncoding(1253);
            }

            return encoding;

            /*   OLD CODE 
            // Read the BOM
            var bom = new byte[4];
            using (var file = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                file.Read(bom, 0, 4);
            }

            // Analyze the BOM
            if (bom[0] == 0x2b && bom[1] == 0x2f && bom[2] == 0x76) return Encoding.UTF7;
            if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf) return Encoding.UTF8;
            if (bom[0] == 0xff && bom[1] == 0xfe && bom[2] == 0 && bom[3] == 0) return Encoding.UTF32; //UTF-32LE
            if (bom[0] == 0xff && bom[1] == 0xfe) return Encoding.Unicode; //UTF-16LE
            if (bom[0] == 0xfe && bom[1] == 0xff) return Encoding.BigEndianUnicode; //UTF-16BE
            if (bom[0] == 0 && bom[1] == 0 && bom[2] == 0xfe && bom[3] == 0xff) return new UTF32Encoding(true, true);  //UTF-32BE

            // We actually have no idea what the encoding is if we reach this point, so
            // you may wish to return null instead of defaulting to ASCII
            return Encoding.GetEncoding(1253);// Encoding.ASCII;
            */
        }


        /// <summary>
        /// Convert a appsettings.json and a descriptorsd.json to MainConfigurationModel
        /// </summary>
        /// <param name="sPath"></param>
        /// <param name="plugInId"></param>
        /// <returns></returns>
        private MainConfigurationModel GetConfigurationsFromfile(string sPath, Guid? plugInId, NLog.Logger logger, bool isDeveloper = false)
        {
            MainConfigurationModel result = new MainConfigurationModel();
            try
            {
                string mainconfigPath, descriptorsPath;
                mainconfigPath = Path.GetFullPath(Path.Combine(new string[] { sPath, "Config", "settings.json" }));
                descriptorsPath = Path.GetFullPath(Path.Combine(new string[] { sPath, "Descriptors", "descriptors.json" }));

                //string servName;
                //if (!string.IsNullOrWhiteSpace(tmpServiceName))
                //    servName = tmpServiceName;
                //else
                //    servName = plugInId == null ? "" : plugInId.ToString();
                //logger.Info(tmpServiceName + " Config Path : " + mainconfigPath);
                //logger.Info(tmpServiceName + " Descriptor Path : " + descriptorsPath);


                result = GetModelByName(mainconfigPath, descriptorsPath, plugInId, logger);
                result.configClassName = this.GetType().FullName;
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return result;
        }

        /// <summary>
        /// Convert dictionary values to specific types and restun a dictionary
        /// </summary>
        /// <param name="start"></param>
        /// <param name="descriptor"></param>
        /// <returns></returns>
        private Dictionary<string, dynamic> ConvertValuesForDictionary(Dictionary<string, dynamic> start, Dictionary<string, List<DescriptorsModel>> descriptor)
        {
            Dictionary<string, dynamic> result = new Dictionary<string, dynamic>();
            bool found;
            foreach (var item in start)
            {
                found = false;
                foreach (var tmpDesc in descriptor)
                {
                    var fld = ((List<DescriptorsModel>)tmpDesc.Value).Find(f => f.Key == item.Key);
                    if (fld != null)
                    {
                        if (fld.Type.ToLower().Contains("list,db"))
                        {
                            try
                            {
                                List<string> tmpLst = JsonSerializer.Deserialize<List<string>>(item.Value.ToString());
                                result[item.Key] = tmpLst;
                            }
                            catch
                            {
                                result[item.Key] = new List<string>();
                            }

                            //if (item.Value == null)
                            //    result[item.Key] = new List<string>();
                            //else
                            //{
                            //    List<string> tmpLst = JsonSerializer.Deserialize<List<string>>(item.Value.ToString());
                            //    result[item.Key] = tmpLst;
                            //}
                        }
                        else if (fld.Type.ToLower().Contains("list,string"))
                            {
                                try
                                {
                                    List<string> tmpLst = JsonSerializer.Deserialize<List<string>>(item.Value.ToString());
                                    result[item.Key] = tmpLst;
                                }
                                catch
                                {
                                    result[item.Key] = new List<string>();
                                }
                            }
                        else if (fld.Type.ToLower().Contains("list,mpehotel"))
                        {
                            try
                            {
                                List<HotelConfigModel> tmpLst = JsonSerializer.Deserialize<List<HotelConfigModel>>(item.Value.ToString());
                                result[item.Key] = tmpLst;
                            }
                            catch
                            {
                                result[item.Key] = new List<string>();
                            }
                        }
                        else
                        {
                            try
                            {
                                   result[item.Key] = Convert.ToString(item.Value);
                            }
                            catch
                            {
                                result[item.Key] = "";
                            }
                        }
                            
                        found = true;
                        break;
                    }
                }

                if (!found)
                    result[item.Key] = item.Value.ToString();
            }
            return result;
        }

        /// <summary>
        /// Check if all items to a configuration exists and has values based on descriptor
        /// </summary>
        /// <param name="config"></param>
        /// <param name="descriptor"></param>
        public void CheckIfNullValuesExistsOnConfiguration(Dictionary<string, dynamic> config, Dictionary<string, List<DescriptorsModel>> descriptor)
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
                            case "customdb":
                                config[dspVal.Key] = dspVal.DefaultValue;
                                break;
                            case "maindb":
                                config[dspVal.Key] = dspVal.DefaultValue;
                                break;
                            case "string":
                                //if (dspVal.DefaultValue.ToLower().Contains("server") &&
                                //    dspVal.DefaultValue.ToLower().Contains("database") &&
                                //    dspVal.DefaultValue.ToLower().Contains("user id") &&
                                //    dspVal.DefaultValue.ToLower().Contains("password"))
                                //    config[dspVal.Key] = "";
                                //else
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
        /// Convert values to real type based on descriptor
        /// </summary>
        /// <param name="config"></param>
        /// <param name="descriptor"></param>
        private void ConvertConfigurationValuestoRealType(Dictionary<string, dynamic> config, Dictionary<string, List<DescriptorsModel>> descriptor)
        {
            foreach (var item in descriptor)
            {
                List<DescriptorsModel> dsp = item.Value;
                foreach (var dspVal in dsp)
                {
                    if (config.ContainsKey(dspVal.Key))
                    {
                        switch (dspVal.Type.ToLower())
                        {
                            case "bool":
                                bool tmpBool;
                                bool.TryParse(config[dspVal.Key], out tmpBool);
                                config[dspVal.Key] = tmpBool;
                                break;

                            case "int":
                                int tmpInt;
                                int.TryParse(config[dspVal.Key], out tmpInt);
                                config[dspVal.Key] = tmpInt;
                                break;
                            case "int32":
                                int tmpInt32;
                                Int32.TryParse(config[dspVal.Key], out tmpInt32);
                                config[dspVal.Key] = tmpInt32;
                                break;
                            case "int64":
                                Int64 tmpInt64;
                                Int64.TryParse(config[dspVal.Key], out tmpInt64);
                                config[dspVal.Key] = tmpInt64;
                                break;
                            case "decimal":
                                decimal tmpDec;
                                decimal.TryParse(config[dspVal.Key], out tmpDec);
                                config[dspVal.Key] = tmpDec;
                                break;
                            case "float":
                                float tmpFloat;
                                float.TryParse(config[dspVal.Key], out tmpFloat);
                                config[dspVal.Key] = tmpFloat;
                                break;
                            case "double":
                                double tmpDouble;
                                double.TryParse(config[dspVal.Key], out tmpDouble);
                                config[dspVal.Key] = tmpDouble;
                                break;
                            case "datetime":
                                DateTime tmpDate;
                                DateTime.TryParse(config[dspVal.Key], out tmpDate);
                                config[dspVal.Key] = tmpDate;
                                break;
                            //default:
                            //    config[dspVal.Key] = dspVal.DefaultValue;
                            //    break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        /// <param name="descriptor"></param>
        public void RemoveKeysNotExistsOnDescriptor(Dictionary<string, dynamic> config, Dictionary<string, List<DescriptorsModel>> descriptor)
        {
            //No values for descriptor exist so the configuration initialzed
            if (descriptor == null || descriptor.Count < 1)
            {
                config = new Dictionary<string, dynamic>();
                return;
            }


            List<string> removeKeys = new List<string>();

            bool found;
            foreach (var item in config)
            {
                found = false;
                foreach (var tmpDesc in descriptor)
                {
                    found = ((List<DescriptorsModel>)tmpDesc.Value).Find(f => f.Key == item.Key) != null;
                    if (found)
                        break;
                }

                if (!found)
                    removeKeys.Add(item.Key);
            }

            foreach (string item in removeKeys)
                config.Remove(item);
        }

        /// <summary>
        /// Adds a Config Model to Static List
        /// </summary>
        /// <param name="configModel"></param>
        public void AddConfigToStaticConfiguration(MainConfigurationModel configModel)
        {
            //var ps = new List<string>() { CurrentPath, "Config", "NLog.config" };
            //var logpath = Path.GetFullPath(Path.Combine(ps.ToArray()));
            //var logger = NLog.Web.NLogBuilder.ConfigureNLog(logpath).GetCurrentClassLogger();

            //logger.Info("Passed AddConfigToStaticConfiguration");

            if (configModel.config != null)
            {
                if (Configuration == null)
                    Configuration = new List<MainConfigurationModel>();
                var fld = Configuration.Find(f => f.plugInId == configModel.plugInId || f.configClassName == this.GetType().FullName);
                if (fld == null)
                {
                    Configuration.Add(configModel);
                    //logger.Info("No main configuration found for plugin id " + configModel.plugInId.ToString());

                }
                else
                {
                    if (fld.plugInId == null)
                        fld.plugInId = configModel.plugInId;
                    fld = configModel;
                    //logger.Info("configuration for plugin id " + configModel.plugInId.ToString() + "  found");
                }
            }
        }

        /// <summary>
        /// Initialize a configuration based on config file
        /// </summary>
        /// <param name="configPath"></param>
        /// <param name="plugInId"></param>
        /// <returns></returns>
        public MainConfigurationModel InitilizeConfiguration(string configPath, Guid? plugInId, string fullClassName)
        {
            //Logger
            var ps = new List<string>() { CurrentPath, "Config", "NLog.config" };
            var logpath = Path.GetFullPath(Path.Combine(ps.ToArray()));
            var logger = NLog.Web.NLogBuilder.ConfigureNLog(logpath).GetCurrentClassLogger();
            
            MainConfigurationModel result = null;

            try
            {
                //logger.Info("Read from " + configPath + "  for : " + (plugInId == null ? " No plugin id" : plugInId.ToString()) + "  and class name : " + fullClassName);

                //static list already has values..
                if (Configuration != null)
                {
                    //if plugin is not null then try to find it using plugin id
                    if (plugInId != null)
                        result = Configuration.Find(f => f.plugInId == plugInId);
                    //not found based on plugin id and try to find based on inherited from abstract class
                    if (result == null)
                        result = Configuration.Find(f => f.configClassName == fullClassName);
                    //found. before return if plugin has value then update plugin id on result
                    if (result != null)
                    {
                        if (plugInId != null)
                            result.plugInId = plugInId;

                        return result;
                    }
                }
                //tmpServiceName = fullClassName

                result = GetConfigurationsFromfile(configPath, plugInId, logger);
                //logger.Info("Returnd config " + JsonSerializer.Serialize(tmp));

                result.configClassName = fullClassName; // this.GetType().FullName;

                //For check if exists on lists
                MainConfigurationModel fld;
                if (result.config != null)
                {
                    //Add to local static list
                    if (Configuration == null)
                        Configuration = new List<MainConfigurationModel>();
                    //try to find based on plugin id
                    fld = Configuration.Find(f => f.plugInId == plugInId);
                    
                    //try to find based on inherited abstract class name
                    if (fld == null)
                        fld = Configuration.Find(f => f.configClassName == fullClassName);
                    
                    //found
                    if (fld != null)
                    {
                        if (plugInId != null)
                            fld.plugInId = plugInId;
                        //change it by config readed from json file
                        fld = result;
                    }
                    else
                        Configuration.Add(result);
                }
                return result;
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                return null;
            }

        }

        /// <summary>
        /// Returns all configurations from static list
        /// </summary>
        /// <returns></returns>
        public List<MainConfigurationModel> GetAllConfigs()
        {
            //var ps = new List<string>() { CurrentPath, "Config", "NLog.config" };
            //var logpath = Path.GetFullPath(Path.Combine(ps.ToArray()));
            //var logger = NLog.Web.NLogBuilder.ConfigureNLog(logpath).GetCurrentClassLogger();
            //if (Configuration != null)
            //    logger.Info(" Returned all configurations " + JsonSerializer.Serialize(Configuration));


            return Configuration;
        }
    }
}
