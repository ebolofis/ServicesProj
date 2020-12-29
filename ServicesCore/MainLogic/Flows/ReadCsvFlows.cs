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
    public class ReadCsvFlows
    {
        /// <summary>
        /// Instance for logger
        /// </summary>
        private readonly ILogger<ReadCsvFlows> logger;

        /// <summary>
        /// Instance for service settings
        /// </summary>
        private readonly ISReadFromCsvModel settings;

        /// <summary>
        /// Instance of Run Sql Scripts
        /// </summary>
        private readonly SQLFlows scriptFlow;

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

        /// <summary>
        /// Instance for mapper
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// instance for file helper
        /// </summary>
        private readonly FileHelpers fh;

        /// <summary>
        /// Instance for ConvertDynamicHelper
        /// </summary>
        private readonly ConvertDynamicHelper dynamicCast;

        /// <summary>
        /// Instance for helper to save changeson json
        /// </summary>
        private readonly IS_ServicesHelper isServicesHlp;
        
        public ReadCsvFlows(ISReadFromCsvModel _settings)
        {
            if (DIHelper.AppBuilder != null)
            {
                var services = DIHelper.AppBuilder.ApplicationServices;
                logger = services.GetService<ILogger<ReadCsvFlows>>();
                mapper = services.GetService<IMapper>();
                smtpHelper = services.GetService<SmtpHelper>();
            }

            settings = _settings;
            isServicesHlp = new IS_ServicesHelper();

            fh = new FileHelpers();
            dynamicCast = new ConvertDynamicHelper(mapper);

            scriptFlow = new SQLFlows(null);

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
                model.Body = "[" + DateTime.Now.ToString() + "] Succesfully readed data from csv file " + settings.serviceName;
            else
                model.Body = "[" + DateTime.Now.ToString() + "] Error on reading data from csv file " + settings.serviceName + " \r\n" + sMess;
            
            model.From = smtpModel.sender;
            model.Subject = "Result After read data from csv file";
            emailHelper.Send(model);
        }

        /// <summary>
        /// Read a CSV file and then save the selected data (insert or/and update) to a destination DB Table.
        /// </summary>
        /// <param name="preSqlScript"> pre-insert/update script running to destination DB</param>
        ///
        public void ReadFromCsv()
        {
            try
            {

                //1. read data from csv file
                List<IDictionary<string, dynamic>> rawData = fh.ReadCsvFile(settings.CsvFilePath, settings.CsvDelimenter, settings.CsvFileHeader.Value, mapper, settings.CsvFileHeaders, settings.Encoding).ToList();

                string preSqlScript = settings.SqlDestPreScript;

                //2. Run Destination SQL Pre-Insert/Update Script
                if (!string.IsNullOrEmpty(settings.SqlDestPreScript))
                    scriptFlow.RunScript(preSqlScript, settings.DestinationDB);

                //3. Read Destination's Table info
                DbTableModel tableInfo = scriptFlow.GetTableInfo(settings.DestinationDB, settings.DestinationDBTableName);

                //4. Save Data to destination Table
                SaveDataToDB(rawData, tableInfo);

                //5.1 Exists settings so change parameters if exists and save to json file
                if (settings != null)
                {
                    //4. save changes on settings
                    List<ISReadFromCsvModel> saveSettings = new List<ISReadFromCsvModel>();

                    //add version to current plus one
                    if ((settings.serviceVersion ?? 0) == long.MaxValue - 1)
                        settings.serviceVersion = 0;
                    else
                        settings.serviceVersion++;

                    saveSettings.Add(settings);
                    isServicesHlp.SaveReadFromCsvJsons(saveSettings);
                }


                if (settings.sendEmailOnSuccess)
                    SendEmails(true, "");
            }
            catch (Exception ex)
            {
                if (settings.sendEmailOnFailure)
                    SendEmails(false, ex.Message + (ex.InnerException != null ? " InnerException : " + ex.InnerException.Message : ""));
                logger.LogError(ex.ToString());
            }

        }

        /// <summary>
        /// Save data to a Destination DB Table. Default Connecetion String: settings.DestinationDB
        /// </summary>
        /// <param name="rawData">data to save</param>
        /// <param name="tableinfo">destination table</param>
        /// <param name="conString">connection string. If conString=null then default Connecetion String: settings.DestinationDB</param>
        private void SaveDataToDB(dynamic rawData, DbTableModel tableinfo, string conString = null)
        {
            if (conString == null) 
                conString = settings.DestinationDB;

            //1. convert data as List<IDictionary<string, dynamic>>
            if (rawData == null) 
                return;
            
            List<IDictionary<string, dynamic>> dictionary = dynamicCast.ToListDictionary(rawData);
            if (dictionary == null) 
                return;

            //2. Insert or Update data to a Data Table
            scriptFlow.SaveToTable(dictionary, conString, tableinfo, settings.DBOperation, settings.DBTransaction);
        }
    }
}
