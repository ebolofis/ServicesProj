using AutoMapper;
using HitHelpersNetCore.Classes;
using HitHelpersNetCore.Helpers;
using HitHelpersNetCore.Models;
using HitServicesCore.Helpers;
using HitServicesCore.MainLogic.Tasks;
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
    public class SQLFlows
    {
        /// <summary>
        /// Instance for logger
        /// </summary>
        private readonly ILogger<SQLFlows> logger;
        
        /// <summary>
        /// Instance for settings
        /// </summary>
        private readonly ISRunSqlScriptsModel settings;

        /// <summary>
        /// Instance for SqlScript task
        /// </summary>
        private readonly SQLTasks sqlTasks;

        /// <summary>
        /// Instance for ConvertDynamicHelper
        /// </summary>
        private readonly ConvertDynamicHelper castDynamic;

        /// <summary>
        /// Instance for mapper
        /// </summary>
        private readonly IMapper mapper;

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

        public SQLFlows(ISRunSqlScriptsModel _settings)
        {
            if (DIHelper.AppBuilder != null)
            {
                var services = DIHelper.AppBuilder.ApplicationServices;
                logger = services.GetService<ILogger<SQLFlows>>();
                mapper = services.GetService<IMapper>();
                smtpHelper = services.GetService<SmtpHelper>();
            }
            settings = _settings;
            sqlTasks = new SQLTasks(/*_logger,*/ _settings);
            castDynamic = new ConvertDynamicHelper(mapper);
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
                    throw new Exception(ex.ToString());
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
                model.Body = "[" + DateTime.Now.ToString() + "] Succesfully executed sql script " + settings.serviceName;
            else
                model.Body = "[" + DateTime.Now.ToString() + "] Error on execution of sql script " + settings.serviceName + " \r\n" + sMess;
            model.From = smtpModel.sender;
            model.Subject = "Result After Sql Script Execution";
            emailHelper.Send(model);
        }

        /// <summary>
        /// Run sql script (insert/updates/deletes)
        /// </summary>
        public void RunScript(string sqlScript, string dbConnection)
        {
            try
            {
                //1. Replace sqlScript's parameters (ex:@Id) with values from HangFireDB.
                sqlScript = PrepareSqlScript(sqlScript);

                //2. Run sql script (insert/updates/deletes) and return updated sql parameters
                IEnumerable<dynamic> newSqlParameters = sqlTasks.RunSelect(sqlScript, dbConnection);

                //2.1 Exists settings so change parameters if exists and save to json file
                if (settings != null)
                {
                    //3. Update SQL Parameters to setting model
                    if (settings.SqlParameters != null && settings.SqlParameters.Count > 0)
                        UpdateSqlParams(newSqlParameters, settings.SqlParameters);

                    //4. save changes on settings
                    List<ISRunSqlScriptsModel> saveSettings = new List<ISRunSqlScriptsModel>();

                    //add version to current plus one
                    if ((settings.serviceVersion ?? 0) == long.MaxValue - 1)
                        settings.serviceVersion = 0;
                    else
                        settings.serviceVersion++;

                    saveSettings.Add(settings);
                    isServicesHlp.SaveRunsSqlScriptsJsons(saveSettings);
                    if (settings.sendEmailOnSuccess)
                        SendEmails(true, "");
                }
            }
            catch(Exception ex)
            {
                if (settings != null && settings.sendEmailOnFailure)
                    SendEmails(false, ex.Message + (ex.InnerException != null ? " InnerException : " + ex.InnerException.Message : ""));
                logger.LogError(ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Get an sqlScript and replace parameters (ex:@Id) with values (sqlParameters).
        /// </summary>
        /// <param name="sqlScript"></param>
        /// <returns></returns>
        public string PrepareSqlScript(string sqlScript)
        {
            try
            {
                if (settings == null)
                    return sqlScript;

                if (settings.SqlParameters == null || settings.SqlParameters.Count == 0)
                    return sqlScript;

                foreach (string key in settings.SqlParameters.Keys)
                {

                    //1. get parameter's value from HangFireDB
                    //string value = hitServicesDt.GetParameterValue(HangFireDB.ConString, settings.SettingsFile, key);

                    string value = settings.SqlParameters[key.Trim()].Replace("'", "''");

                    //2. if parameter does not exist in HangFireDB then get the initial value from settingsfile
                    //if (value == null) value = settings.SqlParameters[key.Trim()].Replace("'", "''");

                    //2. replace value to sql script
                    sqlScript = sqlScript.Replace(key, "'" + value + "'");
                }
            }
            catch(Exception ex)
            {
                logger.LogError(ex.ToString());
                throw new Exception(ex.ToString());
            }
            return sqlScript;
        }

        /// <summary>
        /// Get an sqlScript and SqlParameters and replace parameters (ex:@Id) with values (sqlParameters).
        /// </summary>
        /// <param name="sqlScript"></param>
        /// <param name="SqlParameters"></param>
        /// <returns></returns>
        public string PrepareSqlScript(string sqlScript, Dictionary<string,string> SqlParameters)
        {
            try
            {
                if (SqlParameters == null || SqlParameters.Count == 0)
                    return sqlScript;

                foreach (string key in SqlParameters.Keys)
                {

                    //1. get parameter's value from HangFireDB
                    //string value = hitServicesDt.GetParameterValue(HangFireDB.ConString, settings.SettingsFile, key);

                    string value = SqlParameters[key.Trim()].Replace("'", "''");

                    //2. if parameter does not exist in HangFireDB then get the initial value from settingsfile
                    //if (value == null) value = settings.SqlParameters[key.Trim()].Replace("'", "''");

                    //2. replace value to sql script
                    sqlScript = sqlScript.Replace(key, "'" + value + "'");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw new Exception(ex.ToString());
            }
            return sqlScript;
        }

        /// <summary>
        /// Update or Insert (one or more) SQL Parameters to HangFireDB
        /// </summary>
        /// <param name="newSqlParameters">the new SQL Parameters</param>
        public void UpdateSqlParams(IEnumerable<dynamic> newSqlParameters, Dictionary<string, string> SqlParameters)
        {
            if (newSqlParameters == null) 
                return;
            try
            {
                IDictionary<string, dynamic> sqlParamsDict = castDynamic.ToListDictionary(newSqlParameters).FirstOrDefault();

                if (sqlParamsDict == null)
                    return;
                foreach (string key in sqlParamsDict.Keys)
                {
                    if (SqlParameters.ContainsKey("@" + key))
                        SqlParameters["@" + key] = ConvertDynamicValueToString(sqlParamsDict[key]);
                    else if (SqlParameters.ContainsKey(key))
                        SqlParameters[key] = ConvertDynamicValueToString(sqlParamsDict[key]);
                }
            }
            catch(Exception ex)
            {
                logger.LogError(ex.ToString());
                throw new Exception(ex.ToString());
            }
        }

        /// <summary>
        /// return a stirng based on real type
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string ConvertDynamicValueToString(dynamic value)
        {
            try
            {
                string type = value.GetType().Name;
                if (type.ToLower().Contains("date"))
                    return value.ToString("yyyy-MM-dd");
                else
                    return value.ToString();
            }
            catch(Exception ex)
            {
                logger.LogError(ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Return the results of a select query as IEnumerable(dynamic). (Use AutoMapper to convert dynamic to specific model)
        /// </summary>
        /// <param name="sqlScript">sql script to run</param>
        /// <param name="conString">connection string. If null then pick  settings.Custom1DB </param>
        /// <returns>the reusult of the script</returns>
        public List<IEnumerable<dynamic>> RunMultySelect(string sqlScript, string conString, int timeout = 0)
        {
            try
            {
                return sqlTasks.RunMultySelect(sqlScript, conString, 0);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// for a DB Table return the List of Columns 
        /// </summary>
        /// <param name="constr">connection string</param>
        /// <param name="tableName">Table Name</param>
        /// <returns>Table info</returns>
        public DbTableModel GetTableInfo(string constr, string tableName, int timeout = 60)
        {
            try
            {
                return sqlTasks.GetTableInfo(constr, tableName, timeout);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Insert or Update data to a Data Table
        /// </summary>
        /// <param name="data">data to Insert or Update</param>
        /// <param name="constr">destination connection string</param>
        /// <param name="tableinfo">destination Table</param>
        /// <param name="operation">Operation to perform: 0: Inserts & Updates, 1: Inserts only, 2: Updates only </param>
        /// <param name="useTransaction">true if transaction is used</param>
        public void SaveToTable(List<IDictionary<string, dynamic>> data, string constr, DbTableModel tableinfo, int operation, bool useTransaction, int timeout = 60)
        {
            try
            {
                sqlTasks.SaveToTable(data, constr, tableinfo, operation, useTransaction, timeout);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw;
            }
        }
    }
};
