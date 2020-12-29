using HitHelpersNetCore.Models;
using HitHelpersNetCore.Models.PmsDBModels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace HitHelpersNetCore.Helpers
{
    public class ProtelDBsHelper
    {
        private readonly ILogger<ProtelDBsHelper> logger;

        public ProtelDBsHelper(ILogger<ProtelDBsHelper> _logger)
        {
            logger = _logger;
        }

        /// <summary>
        /// Returns a dictionary with models for Protel dbs connections
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, DBModel> GetProtelDBs()
        {
            Dictionary<string, DBModel> result = new Dictionary<string, DBModel>();
            try
            {
                ConfigHelper configHelper = new ConfigHelper();
                MainConfigurationModel mainConfig = configHelper.ReadHitServiceCoreConfig();
                if (mainConfig != null)
                {
                    List<string> protelsDBs = new List<string>();
                    protelsDBs = mainConfig.config.config["ProtelDBs"];
                    foreach (string item in protelsDBs)
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
