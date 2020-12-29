using HitHelpersNetCore.Models;
using HitHelpersNetCore.Models.PmsDBModels;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;

namespace HitHelpersNetCore.Helpers
{
    public class WebPosDBsHelper
    {
        private readonly ILogger<WebPosDBsHelper> logger;

        public WebPosDBsHelper(ILogger<WebPosDBsHelper> _logger)
        {
            logger = _logger;
        }

        /// <summary>
        /// Returns a dictionary with models for WebPos dbs connections
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, DBModel> GetWebPosDBs()
        {
            Dictionary<string, DBModel> result = new Dictionary<string, DBModel>();
            try
            {
                ConfigHelper configHelper = new ConfigHelper();
                
                MainConfigurationModel mainConfig = configHelper.ReadHitServiceCoreConfig();

                string xmlFile;

                if (mainConfig != null)
                {
                    //Geting data from HitServiceCore configuration
                    List<string> webPosDBs = new List<string>();
                    webPosDBs = mainConfig.config.config["PosDBs"];

                    foreach (string item in webPosDBs)
                    {
                        DBModel tmpDB = new DBModel();
                        SqlConnectionStringBuilder sql = new SqlConnectionStringBuilder(item);
                        tmpDB.ConnectionString = item;
                        tmpDB.Db = sql.InitialCatalog;
                        tmpDB.Password = sql.Password;
                        tmpDB.Server = sql.DataSource;
                        tmpDB.User = sql.UserID;
                        tmpDB.DBId = tmpDB.Server.Replace('\\', '_') + "-" + tmpDB.Db;
                        if (!result.ContainsKey(tmpDB.DBId))
                            result.Add(tmpDB.DBId, tmpDB);
                    }

                    //Getting data from usertodatabses
                    webPosDBs = new List<string>();
                    webPosDBs = mainConfig.config.config["PosPaths"];

                    foreach (string item in webPosDBs)
                    {
                        xmlFile = Path.GetFullPath(Path.Combine(new string[] { item, "UsersToDatabases.xml" }));

                        if (File.Exists(xmlFile))
                        {
                            XDocument doc = XDocument.Load(xmlFile);

                            var stores = doc.Descendants().Where(w => w.Name == "Store");

                            foreach (var node in stores)
                            {
                                DBModel tmpDB = new DBModel();
                                foreach (var attrib in node.Attributes())
                                {
                                    tmpDB.Guid = attrib.Name.ToString().ToLower() == "id" ? attrib.Value : tmpDB.Guid;
                                    tmpDB.Server = attrib.Name.ToString().ToLower() == "datasource" ? attrib.Value : tmpDB.Server;
                                    tmpDB.Db = attrib.Name.ToString().ToLower() == "database" ? attrib.Value : tmpDB.Db;
                                    tmpDB.User = attrib.Name.ToString().ToLower() == "databaseusername" ? attrib.Value : tmpDB.User;
                                    tmpDB.Password = attrib.Name.ToString().ToLower() == "databasepassword" ? attrib.Value : tmpDB.Password;
                                }
                                tmpDB.DBId = tmpDB.Server.Replace('\\', '_') + "-" + tmpDB.Db;
                                tmpDB.ConnectionString = "server=" + tmpDB.Server + ";user id=" + tmpDB.User + ";password=" + tmpDB.Password + ";database=" + tmpDB.Db + ";";

                                if (!result.ContainsKey(tmpDB.DBId))
                                    result.Add(tmpDB.DBId, tmpDB);

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
            }

            return result;
        }
    }
}
