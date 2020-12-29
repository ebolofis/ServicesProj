using HitHelpersNetCore.Helpers;
using HitServicesCore.Models;
using HitServicesCore.Models.IS_Services;
using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Logging;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace HitServicesCore.Helpers
{
    public class IS_ServicesHelper
    {
        /// <summary>
        /// System information
        /// </summary>
        private readonly SystemInfo sysInfo;

        /// <summary>
        /// Lock read write json files
        /// </summary>
        private object lockJsons = new object();

        /// <summary>
        /// Instance for hang fire services
        /// </summary>
        private readonly List<SchedulerServiceModel> hangfireServices;

        /// <summary>
        /// Instance for Encryption Helper
        /// </summary>
        private readonly EncryptionHelper eh;

        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger<IS_ServicesHelper> logger;

        public IS_ServicesHelper()
        {
            if (DIHelper.AppBuilder != null)
            {
                var services = DIHelper.AppBuilder.ApplicationServices;
                logger = services.GetService<ILogger<IS_ServicesHelper>>();
                sysInfo = services.GetService<SystemInfo>();
                hangfireServices = services.GetService<List<SchedulerServiceModel>>();
            }
            eh = new EncryptionHelper();
        }

        /// <summary>
        /// Returns a list of ISRunSqlScriptsModel from all json files on directory SqlScripts
        /// </summary>
        /// <returns></returns>
        public List<ISRunSqlScriptsModel> GetRunSqlScriptsFromJsonFiles()
        {
            List<ISRunSqlScriptsModel> result = new List<ISRunSqlScriptsModel>();
            try
            {
                //Path for Sql Scripts json files
                string isServicePath = Path.Combine(new string[] { sysInfo.rootPath, "IS_Services", "SqlScripts" });
                string sVal;
                if (!Directory.Exists(isServicePath))
                    return result;
                if (isServicePath[isServicePath.Length - 1] != '\\')
                    isServicePath += '\\';
                //Lock Files
                lock (lockJsons)
                {
                    //List of json from SqlScripts directory
                    List<string> sqlScriptsJsons = Directory.EnumerateFiles(isServicePath, "*.json").ToList();
                    foreach (string item in sqlScriptsJsons)
                    {
                        sVal = System.IO.File.ReadAllText(item, Encoding.Default);
                        sVal = eh.Decrypt(sVal);
                        if (!string.IsNullOrWhiteSpace(sVal))
                            //Deserialize json to ISRunSqlScriptsModel
                            result.Add(System.Text.Json.JsonSerializer.Deserialize<ISRunSqlScriptsModel>(sVal));
                    }
                }
            }
            catch (Exception ex)
            {
                //logger.LogError(ex.ToString());
                logger.LogError(ex.ToString());
            }
            return result;
        }

        /// <summary>
        /// Saves a list of ISRunSqlScriptsModel
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool SaveRunsSqlScriptsJsons(List<ISRunSqlScriptsModel> model)
        {
            bool result = true;
            //Path for Sql Scripts json files
            string isServicePath = Path.Combine(new string[] { sysInfo.rootPath, "IS_Services", "SqlScripts" });
            
            //directory not exists
            if (!Directory.Exists(isServicePath))
                Directory.CreateDirectory(isServicePath);

            if (isServicePath[isServicePath.Length - 1] != '\\')
                isServicePath += '\\';

            string fileName;
            try
            {
                foreach (ISRunSqlScriptsModel item in model)
                {
                    if (item.serviceId == null || item.serviceId == Guid.Empty)
                    {
                        item.serviceId = Guid.NewGuid();
                        item.FullClassName = "HitServicesCore.InternalServices.ISRunSqlScriptService";
                    }

                    //File name equals to service name plus .json
                    fileName = isServicePath + item.serviceName;
                    if (!fileName.EndsWith(".json"))
                        fileName += ".json";
                    
                    //set service type to Sql Script Model
                    item.serviceType = Enums.HangFireServiceTypeEnum.SqlScripts;

                    //Save model to directory
                    string savedVal = System.Text.Json.JsonSerializer.Serialize(item);
                    //Encrypt string
                    savedVal = eh.Encrypt(savedVal);
                    
                    File.WriteAllText(fileName, savedVal);
                    
                    if (hangfireServices.Find(f=> f.serviceId == item.serviceId) == null)
                    {
                        hangfireServices.Add(new SchedulerServiceModel
                        {
                            assemblyFileName = fileName,
                            classFullName = item.FullClassName,
                            description = item.ClassDescription,
                            isActive = false,
                            schedulerTime = "* * * * *",
                            schedulerDescr = "Every minute",
                            serviceId = item.serviceId,
                            serviceName = item.serviceName,
                            serviceType = item.serviceType,
                            serviceVersion = (item.serviceVersion == null ? long.MinValue : item.serviceVersion).ToString()
                        });
                    }
                }
            }
            catch(Exception ex)
            {
                result = false;
                //logger.LogError(ex.ToString());
                logger.LogError(ex.ToString());
            }
            return result;
        }

        /// <summary>
        /// Returns a list of ISSaveToTableModel from all json files on directory SaveToTable
        /// </summary>
        /// <returns></returns>
        public List<ISSaveToTableModel> GetSaveToTableFromJsonFiles()
        {
            List<ISSaveToTableModel> result = new List<ISSaveToTableModel>();
            try
            {
                //Path for Sql Scripts json files
                string isServicePath = Path.Combine(new string[] { sysInfo.rootPath, "IS_Services", "SaveToTable" });
                string sVal;
                if (!Directory.Exists(isServicePath))
                    return result;
                if (isServicePath[isServicePath.Length - 1] != '\\')
                    isServicePath += '\\';
                //Lock Files
                lock (lockJsons)
                {
                    //List of json from SqlScripts directory
                    List<string> sqlScriptsJsons = Directory.EnumerateFiles(isServicePath, "*.json").ToList();
                    foreach (string item in sqlScriptsJsons)
                    {
                        sVal = System.IO.File.ReadAllText(item, Encoding.Default);
                        sVal = eh.Decrypt(sVal);

                        if (!string.IsNullOrWhiteSpace(sVal))
                            //Deserialize json to ISRunSqlScriptsModel
                            result.Add(System.Text.Json.JsonSerializer.Deserialize<ISSaveToTableModel>(sVal));
                    }
                }
            }
            catch (Exception ex)
            {
                //logger.LogError(ex.ToString());
                logger.LogError(ex.ToString());
            }
            return result;
        }

        /// <summary>
        /// Saves a list of ISSaveToTableModel
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool SaveSaveToTableJsons(List<ISSaveToTableModel> model)
        {
            bool result = true;
            //Path for Sql Scripts json files
            string isServicePath = Path.Combine(new string[] { sysInfo.rootPath, "IS_Services", "SaveToTable" });

            //directory not exists
            if (!Directory.Exists(isServicePath))
                Directory.CreateDirectory(isServicePath);

            if (isServicePath[isServicePath.Length - 1] != '\\')
                isServicePath += '\\';

            string fileName;
            try
            {
                foreach (ISSaveToTableModel item in model)
                {
                    if (item.serviceId == null || item.serviceId == Guid.Empty)
                    {
                        item.serviceId = Guid.NewGuid();
                        item.FullClassName = "HitServicesCore.InternalServices.ISSaveToTableService";
                    }

                    //File name equals to service name plus .json
                    fileName = isServicePath + item.serviceName;
                    if (!fileName.EndsWith(".json"))
                        fileName += ".json";

                    //set service type to Save to Table
                    item.serviceType = Enums.HangFireServiceTypeEnum.SaveToTable;

                    string savedVal = System.Text.Json.JsonSerializer.Serialize(item);
                    //Encrypt string
                    savedVal = eh.Encrypt(savedVal);

                    //Save model to directory
                    File.WriteAllText(fileName, savedVal);

                    if (hangfireServices.Find(f => f.serviceId == item.serviceId) == null)
                    {
                        hangfireServices.Add(new SchedulerServiceModel
                        {
                            assemblyFileName = fileName,
                            classFullName = item.FullClassName,
                            description = item.ClassDescription,
                            isActive = false,
                            schedulerTime = "* * * * *",
                            schedulerDescr = "Every minute",
                            serviceId = item.serviceId,
                            serviceName = item.serviceName,
                            serviceType = item.serviceType,
                            serviceVersion = (item.serviceVersion == null ? long.MinValue : item.serviceVersion).ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                //logger.LogError(ex.ToString());
                logger.LogError(ex.ToString());
            }
            return result;
        }

        /// <summary>
        /// Returns a list of ISReadFromCsvModel from all json files on directory ReadCsv
        /// </summary>
        /// <returns></returns>
        public List<ISReadFromCsvModel> GetReadFromCsvFromJsonFiles()
        {
            List<ISReadFromCsvModel> result = new List<ISReadFromCsvModel>();
            try
            {
                //Path for Sql Scripts json files
                string isServicePath = Path.Combine(new string[] { sysInfo.rootPath, "IS_Services", "ReadCsv" });
                string sVal;
                if (!Directory.Exists(isServicePath))
                    return result;
                if (isServicePath[isServicePath.Length - 1] != '\\')
                    isServicePath += '\\';
                //Lock Files
                lock (lockJsons)
                {
                    //List of json from SqlScripts directory
                    List<string> sqlScriptsJsons = Directory.EnumerateFiles(isServicePath, "*.json").ToList();
                    foreach (string item in sqlScriptsJsons)
                    {
                        sVal = System.IO.File.ReadAllText(item, Encoding.Default);
                        sVal = eh.Decrypt(sVal);
                        if (!string.IsNullOrWhiteSpace(sVal))
                            //Deserialize json to ISRunSqlScriptsModel
                            result.Add(System.Text.Json.JsonSerializer.Deserialize<ISReadFromCsvModel>(sVal));
                    }
                }
            }
            catch (Exception ex)
            {
                //logger.LogError(ex.ToString());
                logger.LogError(ex.ToString());
            }
            return result;
        }

        /// <summary>
        /// Saves a list of ISReadFromCsvModel
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool SaveReadFromCsvJsons(List<ISReadFromCsvModel> model)
        {
            bool result = true;
            //Path for Sql Scripts json files
            string isServicePath = Path.Combine(new string[] { sysInfo.rootPath, "IS_Services", "ReadCsv" });

            //directory not exists
            if (!Directory.Exists(isServicePath))
                Directory.CreateDirectory(isServicePath);

            if (isServicePath[isServicePath.Length - 1] != '\\')
                isServicePath += '\\';

            string fileName;
            try
            {
                foreach (ISReadFromCsvModel item in model)
                {
                    if (item.serviceId == null || item.serviceId == Guid.Empty)
                    {
                        item.serviceId = Guid.NewGuid();
                        item.FullClassName = "HitServicesCore.InternalServices.ISReadCsvService";
                    }

                    //File name equals to service name plus .json
                    fileName = isServicePath + item.serviceName;
                    if (!fileName.EndsWith(".json"))
                        fileName += ".json";

                    //Set service type to Read From csv
                    item.serviceType = Enums.HangFireServiceTypeEnum.ReadFromCsv;

                    string savedVal = System.Text.Json.JsonSerializer.Serialize(item);
                    //Encrypt string
                    savedVal = eh.Encrypt(savedVal);
                    //Save model to directory
                    File.WriteAllText(fileName, savedVal);

                    if (hangfireServices.Find(f => f.serviceId == item.serviceId) == null)
                    {
                        hangfireServices.Add(new SchedulerServiceModel
                        {
                            assemblyFileName = fileName,
                            classFullName = item.FullClassName,
                            description = item.ClassDescription,
                            isActive = false,
                            schedulerTime = "* * * * *",
                            schedulerDescr = "Every minute",
                            serviceId = item.serviceId,
                            serviceName = item.serviceName,
                            serviceType = item.serviceType,
                            serviceVersion = (item.serviceVersion == null ? long.MinValue : item.serviceVersion).ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                //logger.LogError(ex.ToString());
                logger.LogError(ex.ToString());
            }
            return result;
        }

        /// <summary>
        /// Returns a list of ISExportDataModel from all json files on directory ExportData
        /// </summary>
        /// <returns></returns>
        public List<ISExportDataModel> GetExportdataFromJsonFiles()
        {
            List<ISExportDataModel> result = new List<ISExportDataModel>();
            try
            {
                //Path for Sql Scripts json files
                string isServicePath = Path.Combine(new string[] { sysInfo.rootPath, "IS_Services", "ExportData" });
                string sVal;
                if (!Directory.Exists(isServicePath))
                    return result;
                if (isServicePath[isServicePath.Length - 1] != '\\')
                    isServicePath += '\\';
                //Lock Files
                lock (lockJsons)
                {
                    //List of json from SqlScripts directory
                    List<string> sqlScriptsJsons = Directory.EnumerateFiles(isServicePath, "*.json").ToList();
                    foreach (string item in sqlScriptsJsons)
                    {
                        sVal = System.IO.File.ReadAllText(item, Encoding.Default);
                        sVal = eh.Decrypt(sVal);
                        if (!string.IsNullOrWhiteSpace(sVal))
                            //Deserialize json to ISRunSqlScriptsModel
                            result.Add(System.Text.Json.JsonSerializer.Deserialize<ISExportDataModel>(sVal));
                    }
                }
            }
            catch (Exception ex)
            {
                //logger.LogError(ex.ToString());
                logger.LogError(ex.ToString());
            }
            return result;
        }

        /// <summary>
        /// Saves a list of ISExportDataModel
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool SaveExportDataJsons(List<ISExportDataModel> model)
        {
            bool result = true;
            //Path for Sql Scripts json files
            string isServicePath = Path.Combine(new string[] { sysInfo.rootPath, "IS_Services", "ExportData" });

            //directory not exists
            if (!Directory.Exists(isServicePath))
                Directory.CreateDirectory(isServicePath);

            if (isServicePath[isServicePath.Length - 1] != '\\')
                isServicePath += '\\';

            string fileName;
            try
            {
                foreach (ISExportDataModel item in model)
                {
                    if (item.serviceId == null || item.serviceId == Guid.Empty)
                    {
                        item.serviceId = Guid.NewGuid();
                        item.FullClassName = "HitServicesCore.InternalServices.ISExportDataService";
                    }


                    //File name equals to service name plus .json
                    fileName = isServicePath + item.serviceName;
                    if (!fileName.EndsWith(".json"))
                        fileName += ".json";

                    //Set service type to Export Data
                    item.serviceType = Enums.HangFireServiceTypeEnum.ExportData;

                    string savedVal = System.Text.Json.JsonSerializer.Serialize(item);
                    //Encrypt string
                    savedVal = eh.Encrypt(savedVal);
                    //Save model to directory
                    File.WriteAllText(fileName, savedVal);

                    if (hangfireServices.Find(f => f.serviceId == item.serviceId) == null)
                    {
                        hangfireServices.Add(new SchedulerServiceModel
                        {
                            assemblyFileName = fileName,
                            classFullName = item.FullClassName,
                            description = item.ClassDescription,
                            isActive = false,
                            schedulerTime = "* * * * *",
                            schedulerDescr = "Every minute",
                            serviceId = item.serviceId,
                            serviceName = item.serviceName,
                            serviceType = item.serviceType,
                            serviceVersion = (item.serviceVersion == null ? long.MinValue : item.serviceVersion).ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                ///logger.LogError(ex.ToString());
                logger.LogError(ex.ToString());
            }
            return result;
        }
    }
}
