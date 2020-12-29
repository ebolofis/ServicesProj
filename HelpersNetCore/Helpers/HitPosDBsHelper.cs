using HitHelpersNetCore.Models;
using HitHelpersNetCore.Models.PmsDBModels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace HitHelpersNetCore.Helpers
{
    public class HitPosDBsHelper
    {
        private readonly ILogger<HitPosDBsHelper> logger;

        public HitPosDBsHelper(ILogger<HitPosDBsHelper> _logger)
        {
            logger = _logger;
        }

        /// <summary>
        /// Returns a dictionary with models for HitPos dbs connections
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, DBModel> GetHitPosDBs()
        {
            Dictionary<string, DBModel> result = new Dictionary<string, DBModel>();
            try
            {
                ConfigHelper configHelper = new ConfigHelper();

                MainConfigurationModel mainConfig = configHelper.ReadHitServiceCoreConfig();

                if (mainConfig != null)
                {
                    //Geting data from HitServiceCore configuration
                    string hitPosConn = mainConfig.config.config["HitPosDB"];
                    if (!string.IsNullOrWhiteSpace(hitPosConn))
                    {
                        DBModel tmpDB = new DBModel();
                        SqlConnectionStringBuilder sql = new SqlConnectionStringBuilder(hitPosConn);
                        tmpDB.ConnectionString = hitPosConn;
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
