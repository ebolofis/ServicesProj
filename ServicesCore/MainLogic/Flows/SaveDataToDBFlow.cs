using AutoMapper;
using HitHelpersNetCore.Classes;
using HitHelpersNetCore.Helpers;
using HitHelpersNetCore.Models;
using HitServicesCore.Helpers;
using HitServicesCore.Models;
using HitServicesCore.Models.IS_Services;
using Microsoft.Extensions.Logging;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace HitServicesCore.MainLogic.Flows
{
    public class SaveDataToDBFlow
    {
        /// <summary>
        /// Instance of service
        /// </summary>
        private readonly ISSaveToTableModel settings;

        /// <summary>
        /// Instance of Run Sql Scripts
        /// </summary>
        private readonly SQLFlows scriptFlow;

        /// <summary>
        /// Instance for logger
        /// </summary>
        private readonly ILogger<SaveDataToDBFlow> logger;
        
        /// <summary>
        /// instance for mapper
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// Instance for ConvertDynamicHelper
        /// </summary>
        private readonly ConvertDynamicHelper dynamicCast;

        /// <summary>
        /// Instsnce for Services helpet to save settings after execution
        /// </summary>
        private readonly IS_ServicesHelper isServicesHlp;

        /// <summary>
        /// Instance to get Smtps 
        /// </summary>
        private readonly SmtpHelper smtpHelper;

        /// <summary>
        /// Instance for helper to senf Emails
        /// </summary>
        private readonly EmailHelper emailHelper;

        /// <summary>
        /// Model with crententials about smtp
        /// </summary>
        private readonly SmtpModel smtpModel;

        public SaveDataToDBFlow(ISSaveToTableModel _settings)
        {

            if (DIHelper.AppBuilder != null)
            {
                var services = DIHelper.AppBuilder.ApplicationServices;
                logger = services.GetService<ILogger<SaveDataToDBFlow>>();
                mapper = services.GetService<IMapper>();
                smtpHelper = services.GetService<SmtpHelper>();
            }

            settings = _settings;
            scriptFlow = new SQLFlows(null);
            dynamicCast = new ConvertDynamicHelper(mapper);
            isServicesHlp = new IS_ServicesHelper();

            if (smtpHelper != null && smtpHelper._smhelper != null && smtpHelper._smhelper.ElementAt(0).Value != null)
            {
                try
                {
                    smtpModel = smtpHelper._smhelper.ElementAt(0).Value;
                    if (
                    !string.IsNullOrWhiteSpace(smtpModel.smtp) &&
                    !string.IsNullOrWhiteSpace(smtpModel.port) &&
                    !string.IsNullOrWhiteSpace(smtpModel.ssl) &&
                    !string.IsNullOrWhiteSpace(smtpModel.username) &&
                    !string.IsNullOrWhiteSpace(smtpModel.password) &&
                    !string.IsNullOrWhiteSpace(smtpModel.sender))
                    {
                        bool hasSsl = string.IsNullOrWhiteSpace(smtpModel.ssl) ? false : smtpModel.ssl == "0" ? false : true;
                        emailHelper = new EmailHelper();
                        emailHelper.Init(smtpModel.smtp, Convert.ToInt32(smtpModel.port), hasSsl, smtpModel.username, smtpModel.password);
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex.ToString());
                }
            }
        }

        /// <summary>
        /// Send email on succes and failure.
        /// </summary>
        /// <param name="succeded"></param>
        private void SendEmails(bool succeded, string sMess)
        {
            if (smtpHelper == null || emailHelper == null || string.IsNullOrWhiteSpace(settings.sendEmailTo))
                return;

            EmailSendModel model = new EmailSendModel();

            model.To = settings.sendEmailTo.Split(',').ToList();
            if (succeded)
                model.Body = "[" + DateTime.Now.ToString() + "] Succesfully executed save data to table " + settings.serviceName;
            else
                model.Body = "[" + DateTime.Now.ToString() + "] Error on execution of save data to table " + settings.serviceName + " \r\n" + sMess;
            model.From = smtpModel.sender;
            model.Subject = "Result After Saved data to db tables";
            emailHelper.Send(model);
        }

        /// <summary>
        /// Get Data from a select statement and insert/update/upsert in DB Table. 
        /// </summary>
        /// <param name="sqlScripts">Dictionary with the required scripts. 
        ///   Two keys are required: 
        ///    'MAIN' for the main script running to source DB and 
        ///    'PRE'  for the pre-insert/update script running to destination DB</param>
        /// <returns>return selected data</returns>
        public IEnumerable<dynamic> SaveDataToDB()
        {
            //0. Manipulate data:  If rawDataList contains 2 items then the 1st item contains the Data and  the 2nd contains new values for sqlScript's parameters.
            IEnumerable<dynamic> rawData = null;
            Dictionary<string, string> sqlScripts = new Dictionary<string, string>();
            sqlScripts.Add("MAIN", settings.SqlScript);
            if (!string.IsNullOrEmpty(settings.SqlDestPreScript))
                sqlScripts.Add("PRE", settings.SqlDestPreScript);
            else
                sqlScripts.Add("PRE", null);

            try
            {
                string sqlScript = sqlScripts["MAIN"];
                string preSqlScript = sqlScripts["PRE"];

                //1. Replace sqlScript's parameters (ex:@Id) with values from  HangFireDB.
                sqlScript = scriptFlow.PrepareSqlScript(sqlScript, settings.SqlParameters);

                //2. select data from DB

                int timeout;
                int.TryParse(settings.DBTimeout, out timeout);
                if (timeout == 0)
                    timeout = 60;
                IEnumerable<dynamic> newSqlParameters = null;
                List<IEnumerable<dynamic>> rawDataList = scriptFlow.RunMultySelect(sqlScript, settings.SourceDB, timeout);

                //    3. sql script does not return any datasets
                if (rawDataList == null || rawDataList.Count() == 0)
                    rawData = new List<dynamic>(); // <--- Selected Data
                else
                {
                    //3a. sql script  return (at least) one dataset
                    rawData = rawDataList[0];

                    //3b. sql script return two datasets
                    if (rawDataList.Count() >= 2)
                        newSqlParameters = rawDataList[1];
                }
                //4. Run Destination SQL Pre-Insert/Update Script
                if (!string.IsNullOrEmpty(settings.SqlDestPreScript))
                    scriptFlow.RunScript(preSqlScript, settings.DestinationDB);

                //5. Read Destination's Table info
                DbTableModel tableInfo = scriptFlow.GetTableInfo(settings.DestinationDB, settings.DestinationDBTableName, timeout);

                //6. Save Data to destination Table
                SaveDataToDB(rawData, tableInfo);
                //7. Update SQL Parameters to HangFireDB
                scriptFlow.UpdateSqlParams(newSqlParameters, settings.SqlParameters);


                //8. save changes on settings
                List<ISSaveToTableModel> saveSettings = new List<ISSaveToTableModel>();

                //add version to current plus one
                if ((settings.serviceVersion ?? 0) == long.MaxValue - 1)
                    settings.serviceVersion = 0;
                else
                    settings.serviceVersion++;

                saveSettings.Add(settings);
                isServicesHlp.SaveSaveToTableJsons(saveSettings);

                if (settings.sendEmailOnSuccess)
                    SendEmails(true, "");

            }
            catch (Exception ex)
            {
                if (settings.sendEmailOnFailure)
                    SendEmails(false, ex.Message + (ex.InnerException != null ? " InnerException : " + ex.InnerException.Message : ""));
                logger.LogError(ex.ToString());
            }
            //9. return data
            return rawData;
        }

        /// <summary>
        /// Save data to a Destination DB Table. Default Connecetion String: settings.DestinationDB
        /// </summary>
        /// <param name="rawData">data to save</param>
        /// <param name="tableinfo">destination table</param>
        /// <param name="conString">connection string. If conString=null then default Connecetion String: settings.DestinationDB</param>
        public void SaveDataToDB(dynamic rawData, DbTableModel tableinfo, string conString = null)
        {
            if (conString == null)
                conString = settings.DestinationDB;

            //1. convert data as List<IDictionary<string, dynamic>>
            if (rawData == null) return;
            List<IDictionary<string, dynamic>> dictionary = dynamicCast.ToListDictionary(rawData);
            if (dictionary == null) return;

            //2. Insert or Update data to a Data Table
            scriptFlow.SaveToTable(dictionary, conString, tableinfo, settings.DBOperation, settings.DBTransaction);
        }

    }
}
