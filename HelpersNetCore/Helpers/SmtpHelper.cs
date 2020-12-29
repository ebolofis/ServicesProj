using HitHelpersNetCore.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace HitHelpersNetCore.Helpers
{
    public class SmtpHelper
    {
        public Dictionary<string,SmtpModel> _smhelper;
        
        private string CurrentPath;
        private object lockJsons = new object();
        public List<string> ServerNames;

        private readonly ILogger<SmtpHelper> logger;

        /// <summary>
        /// Instance for encryption helper
        /// </summary>
        private readonly EncryptionHelper eh;

        public SmtpHelper(ILogger<SmtpHelper> _logger)
        {
            CurrentPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            System.IO.Directory.SetCurrentDirectory(CurrentPath);
            logger = _logger;
            eh = new EncryptionHelper();
        }


        public void GetConfig()
        {
            var ps = new List<string>() { CurrentPath, "Config", "NLog.config" };
            var logpath = Path.GetFullPath(Path.Combine(ps.ToArray()));
            var logger = NLog.Web.NLogBuilder.ConfigureNLog(logpath).GetCurrentClassLogger();
            string sPath = Path.GetDirectoryName(this.GetType().Assembly.Location);
            try
            {
                lock (lockJsons)
                {
                    var x = "";
                    string sFileName = Path.GetFullPath(Path.Combine(new string[] { sPath, "Config", "smtp.json" }));
                    string sVal = System.IO.File.ReadAllText(sFileName, Encoding.Default);
                    sVal = eh.Decrypt(sVal);
                    Dictionary<string, SmtpModel> smtpData = JsonSerializer.Deserialize<Dictionary<string, SmtpModel>>(sVal);

                    _smhelper = smtpData;
                }

            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
        }


        private static Dictionary<string, dynamic> FillSubDictionary(string jsonFile)
        {
            Dictionary<string, dynamic> dictionary;
            string rawData = File.ReadAllText(jsonFile, Encoding.Default);
            //dictionary = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(rawData);
            dictionary = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, dynamic>>(rawData);
            return dictionary;
        }

        public void SaveSmtps(Dictionary<string,SmtpModel> editedSmtp)
        {
            var ps = new List<string>() { CurrentPath, "Config", "NLog.config" };
            var logpath = Path.GetFullPath(Path.Combine(ps.ToArray()));
            var logger = NLog.Web.NLogBuilder.ConfigureNLog(logpath).GetCurrentClassLogger();
            try
            {
                lock (lockJsons)
                {
                    string json = JsonSerializer.Serialize(editedSmtp);
                    json = eh.Encrypt(json);
                    string configPath = Path.GetFullPath(Path.Combine(new string[] { CurrentPath, "Config", "smtp.json" }));
                    File.WriteAllText(configPath, json, Encoding.Default);

                    _smhelper = editedSmtp;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
        }
    }
}
