using AutoMapper;
using HitHelpersNetCore.Classes;
using HitHelpersNetCore.Helpers;
using HitHelpersNetCore.Models;
using HitServicesCore.Helpers;
using HitServicesCore.Models;
using HitServicesCore.Models.Helpers;
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
    public class ExportDataFlows
    {
        /// <summary>
        /// Instance for current settings model
        /// </summary>
        private readonly ISExportDataModel settings;

        /// <summary>
        /// Instance of Run Sql Scripts
        /// </summary>
        private readonly SQLFlows scriptFlow;

        /// <summary>
        /// Instance for logger
        /// </summary>
        private readonly ILogger<ExportDataFlows> logger;

        /// <summary>
        /// instance for mapper
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

        /// <summary>
        /// Instance for ConvertDynamicHelper
        /// </summary>
        private readonly ConvertDynamicHelper dynamicCast;

        /// <summary>
        /// Instance for ConvertDataHelper
        /// </summary>
        private ConvertDataHelper convertDataHelper;

        /// <summary>
        /// Instance for file helpers class
        /// </summary>
        private FileHelpers fileHelpers;

        public ExportDataFlows(ISExportDataModel _settings)
        {
            if (DIHelper.AppBuilder != null)
            {
                var services = DIHelper.AppBuilder.ApplicationServices;
                logger = services.GetService<ILogger<ExportDataFlows>>();
                mapper = services.GetService<IMapper>();
                smtpHelper = services.GetService<SmtpHelper>();
            }

            settings = _settings;

            isServicesHlp = new IS_ServicesHelper();
            dynamicCast = new ConvertDynamicHelper(mapper);
            fileHelpers = new FileHelpers();

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
                model.Body = "[" + DateTime.Now.ToString() + "] Succesfully executed export data " + settings.serviceName;
            else
                model.Body = "[" + DateTime.Now.ToString() + "] Error on execution of export data " + settings.serviceName + " \r\n" + sMess;
            model.From = smtpModel.sender;
            model.Subject = "Result After Export Data Execution";
            emailHelper.Send(model);
        }

        /// <summary>
        ///  Run a select query and then save as file OR/AND return to local api OR/AND post to an external Rest Server. 
        ///  Return data as dynamic list.
        /// </summary>
        /// <param name="sqlScript">the sql script to produce data</param>
        /// <returns></returns>
        public IEnumerable<dynamic> ExportData()
        {
            try
            {
                //0. Exection stirng
                string sqlScript = settings.SqlScript;

                //1. Replace sqlScript's parameters (ex:@Id) with values from HangFireDB.
                sqlScript = scriptFlow.PrepareSqlScript(sqlScript, settings.SqlParameters);

                //2. select data from DB
                IEnumerable<dynamic> newSqlParameters = null;
                int timeOut;
                int.TryParse(settings.DBTimeout, out timeOut);
                List<IEnumerable<dynamic>> rawDataList = scriptFlow.RunMultySelect(sqlScript, settings.Custom1DB, timeOut);

                //3. Manipulate data:  If rawDataList contains 2 items then the 1st item contains the Data and  the 2nd contains  new values for sqlScript's parameters.
                IEnumerable<dynamic> rawData = null;

                //    3a. sql script does not return any datasets
                if (rawDataList == null || rawDataList.Count() == 0)
                    rawData = new List<dynamic>(); // <--- Selected Data
                else
                {
                    //3b. sql script  return (at least) one dataset
                    rawData = rawDataList[0];

                    //3c. sql script return two datasets
                    if (rawDataList.Count() >= 2)
                        newSqlParameters = rawDataList[1];
                }

                //4. Export Data to FILE (xml, csv, fixes length, json, html, pdf) or post data to rest server
                ExportData(rawData);

                //5. Update SQL Parameters to HangFireDB
                scriptFlow.UpdateSqlParams(newSqlParameters, settings.SqlParameters);

                //6. Save changes to json path
                //4. save changes on settings
                List<ISExportDataModel> saveSettings = new List<ISExportDataModel>();

                //add version to current plus one
                if ((settings.serviceVersion ?? 0) == long.MaxValue - 1)
                    settings.serviceVersion = 0;
                else
                    settings.serviceVersion++;

                saveSettings.Add(settings);
                isServicesHlp.SaveExportDataJsons(saveSettings);
                if (settings.sendEmailOnSuccess)
                    SendEmails(true, "");

                //7. return data
                return rawData;
            }
            catch(Exception ex)
            {
                if (settings.sendEmailOnSuccess)
                    SendEmails(false, ex.Message + (ex.InnerException != null ? " InnerException : " + ex.InnerException.Message : ""));
                logger.LogError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Export Data to file (xml, csv, fixes length, json, html) or post data to rest server 
        /// </summary>
        /// <param name="rawData"></param>
        /// <param name="extension">the ending part of url (not included to settings.RestServerUrl)</param>
        /// <returns>for the RestServer call, return the Servers's response message</returns>
        public string ExportData(dynamic rawData, string extension = "")
        {
            //1. if export to file is needed then Convert Data to List<IDictionary<string, dynamic>>
            List<IDictionary<string, dynamic>> dictionary = null;

            if (!String.IsNullOrEmpty(settings.CsvFilePath) ||
                !String.IsNullOrEmpty(settings.HtmlFilePath) ||
                !String.IsNullOrEmpty(settings.PdfFilePath) ||
                !String.IsNullOrEmpty(settings.JsonFilePath) ||
                !String.IsNullOrEmpty(settings.XmlFilePath) ||
                !String.IsNullOrEmpty(settings.FixedLenghtFilePath)
                )
            {
                dictionary = dynamicCast.ToListDictionary(rawData);
            }

            //2. export to file or to rest server
            if (!String.IsNullOrEmpty(settings.CsvFilePath))
            {
                ToCsv(dictionary);
            }
            if (!String.IsNullOrEmpty(settings.HtmlFilePath))
            {
                ToHtml(dictionary);
            }
            if (!String.IsNullOrEmpty(settings.PdfFilePath))
            {
                ToPdf(dictionary);
            }
            if (!String.IsNullOrEmpty(settings.JsonFilePath))
            {
                ToJson(dictionary);
            }
            if (!String.IsNullOrEmpty(settings.XmlFilePath))
            {
                ToXml(dictionary);
            }
            if (!String.IsNullOrEmpty(settings.FixedLenghtFilePath))
            {
                ToFixedLenght(dictionary);
            }
            if (!String.IsNullOrEmpty(settings.RestServerUrl))
            {
                //check if rawData are List or not
                if (isIEnumerable(rawData))
                    return PostToRestServer<List<dynamic>>(ToList(rawData), extension); //rawData is a list
                else
                    return PostToRestServer<dynamic>(rawData, extension); //rawData is not a list
            }

            return "";
        }

        /// <summary>
        /// return true id rawData is a List
        /// </summary>
        /// <param name="rawData">Data (list or not list)</param>
        /// <returns></returns>
        public bool isIEnumerable(dynamic rawData)
        {
            var enumerable = rawData as System.Collections.IEnumerable;
            if (enumerable != null)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Convert dynamic rawData as List of dynamic. (rawData must be checked if they are list, see isIEnumerable)
        /// </summary>
        /// <param name="rawData">dynamic data</param>
        /// <returns></returns>
        public List<dynamic> ToList(dynamic rawData)
        {
            return (rawData as IEnumerable<dynamic>).ToList();
        }

        /// <summary>
        /// POST Data to Rest Server
        /// </summary>
        /// <param name="data"></param>
        /// <param name="extension">the last part of url (not included to settings.RestServerUrl)</param>
        public string PostToRestServer<T>(T data, string extension)
        {
            string ErrorMsg = "";
            int returnCode = 0;
            string result = "";
            WebApiHelper webHelper = new WebApiHelper();

            if (settings.RestServerHttpMethod.ToUpper() == "POST")
            {
                result = webHelper.Post<T>(constractUrl(settings.RestServerUrl, extension),
                    data,
                    out returnCode,
                    out ErrorMsg,
                    settings.RestServerAuthenticationHeader,
                    settings.RestServerCustomHeaders,
                    settings.RestServerMediaType
                    );
            }
            else
            {
                throw new Exception("No Post Http Method has been set.");
            }
            if (returnCode != 200)
            {
                throw new Exception("Http Post Error: " + ErrorMsg + " Code: " + returnCode.ToString());
            }

            return result;
        }

        /// <summary>
        /// Write data to pdf file
        /// </summary>
        /// <param name="data"></param>
        public void ToPdf(List<IDictionary<string, dynamic>> data)
        {
            convertDataHelper = new ConvertDataHelper(CreateFormaters());
            string html = convertDataHelper.ToHtml(data, settings.HtmlHeader ?? false, settings.PdfTitle, false, "");
            html = html.Replace("<style type=\"text/css\">", "");
            html = html.Replace("</style>", "");
            fileHelpers.WriteHtmlToPdf(settings.PdfFilePath, html, settings.Pdfcss, settings.TimeStamp);
        }

        /// <summary>
        /// Write data to xml file
        /// </summary>
        /// <param name="data"></param>
        public void ToXml(List<IDictionary<string, dynamic>> data)
        {
            convertDataHelper = new ConvertDataHelper(CreateFormaters());
            string xml = convertDataHelper.ToXml(data, settings.XmlRootElement, settings.XmlElement, mapper);
            if (!settings.XmlFileToFTP)
                fileHelpers.WriteTextToFile(settings.XmlFilePath, xml, false, settings.TimeStamp);
            else
                fileHelpers.UploadToFTP(xml, settings.FtpServer, settings.FtpPort, settings.FtpUsername, settings.FtpPassword, settings.XmlFilePath, settings.FtpConnectionEncryption, settings.FTPSslProtocols, settings.FTPEncryptionMode, settings.TimeStamp);
        }

        /// <summary>
        /// Write data to json file
        /// </summary>
        /// <param name="data"></param>
        public void ToJson(List<IDictionary<string, dynamic>> data)
        {
            convertDataHelper = new ConvertDataHelper(CreateFormaters());
            string json = convertDataHelper.ToJson(data);
            if (!settings.JsonFileToFTP)
                fileHelpers.WriteTextToFile(settings.JsonFilePath, json, false, settings.TimeStamp);
            else
                fileHelpers.UploadToFTP(json, settings.FtpServer, settings.FtpPort, settings.FtpUsername, settings.FtpPassword, settings.JsonFilePath, settings.FtpConnectionEncryption, settings.FTPSslProtocols, settings.FTPEncryptionMode, settings.TimeStamp);
        }

        /// <summary>
        /// Write data to csv file
        /// </summary>
        /// <param name="data"></param>
        public void ToCsv(List<IDictionary<string, dynamic>> data)
        {
            convertDataHelper = new ConvertDataHelper(CreateFormaters());
            string csv = convertDataHelper.ToCsv(data, settings.CsvFileHeader ?? false, settings.CsvDelimenter);
            if (!settings.CsvFileToFTP)
                fileHelpers.WriteTextToFile(settings.CsvFilePath, csv, false, settings.TimeStamp);
            else
                fileHelpers.UploadToFTP(csv, settings.FtpServer, settings.FtpPort, settings.FtpUsername, settings.FtpPassword, settings.CsvFilePath, settings.FtpConnectionEncryption, settings.FTPSslProtocols, settings.FTPEncryptionMode, settings.TimeStamp);
        }

        /// <summary>
        /// Write data to csv file
        /// </summary>
        /// <param name="data"></param>
        public void ToFixedLenght(List<IDictionary<string, dynamic>> data)
        {
            convertDataHelper = new ConvertDataHelper(CreateFormaters());
            string fl = convertDataHelper.ToFixedLenght(data, settings.FixedLenghtFileHeader ?? false, settings.FixedLengths ?? null, settings.FixedLenghtAlignRight ?? false);
            if (!settings.FixedLenghtFileToFTP)
                fileHelpers.WriteTextToFile(settings.FixedLenghtFilePath, fl, false, settings.TimeStamp);
            else
                fileHelpers.UploadToFTP(fl, settings.FtpServer, settings.FtpPort, settings.FtpUsername, settings.FtpPassword, settings.FixedLenghtFilePath, settings.FtpConnectionEncryption, settings.FTPSslProtocols, settings.FTPEncryptionMode, settings.TimeStamp);
        }

        /// <summary>
        /// Write data to html file
        /// </summary>
        /// <param name="data"></param>
        public void ToHtml(List<IDictionary<string, dynamic>> data)
        {
            convertDataHelper = new ConvertDataHelper(CreateFormaters());
            string fl = convertDataHelper.ToHtml(data, settings.HtmlHeader ?? false, settings.HtmlTitle, settings.HtmlSortRows, settings.Htmlcss);
            if (!settings.HtmlFileToFTP)
                fileHelpers.WriteTextToFile(settings.HtmlFilePath, fl, false, settings.TimeStamp);
            else
                fileHelpers.UploadToFTP(fl, settings.FtpServer, settings.FtpPort, settings.FtpUsername, settings.FtpPassword, settings.HtmlFilePath, settings.FtpConnectionEncryption, settings.FTPSslProtocols, settings.FTPEncryptionMode, settings.TimeStamp);
        }


        #region Private Methods
        /// <summary>
        /// constract a final url 
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="extenstion"></param>
        /// <returns></returns>
        private string constractUrl(string baseUrl, string extenstion)
        {
            if (extenstion == "") return baseUrl;
            //   if (baseUrl.EndsWith("/") || extenstion.StartsWith("/"))
            return baseUrl + extenstion;
            //  else
            //     return baseUrl + "/"+extenstion;
        }

        /// <summary>
        /// Using a SettingsModel (CultureInfo, Formater) return a Dictionary(string, Formater)
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        private Dictionary<string, Formater> CreateFormaters()
        {
            Dictionary<string, Formater> formaters = new Dictionary<string, Formater>();
            if (settings == null || settings.Formater == null) return formaters;

            foreach (string key in settings.Formater.Keys)
            {
                Formater f = new Formater();
                f.CultureInfoDescription = settings.CultureInfo;
                f.Format = settings.Formater[key];
                formaters.Add(key, f);
            }
            return formaters;
        }

        #endregion

    }
}
