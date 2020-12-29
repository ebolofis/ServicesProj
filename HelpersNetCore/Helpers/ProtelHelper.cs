using HitHelpersNetCore.Models;
using HitHelpersNetCore.Models.PmsDBModels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace HitHelpersNetCore.Helpers
{
    public class ProtelHelper
    {
        private readonly ILogger<ProtelHelper> logger;

        private CheckConnectionsHelper checkConn;

        public ProtelHelper(ILogger<ProtelHelper> _logger)
        {
            logger = _logger;
            checkConn = new CheckConnectionsHelper();
        }

        public Dictionary<string, ProtelHotelsModel> GetProtelHotels()
        {
            Dictionary<string, ProtelHotelsModel> result = new Dictionary<string, ProtelHotelsModel>();
            try
            {
                
                ConfigHelper configHelper = new ConfigHelper();

                MainConfigurationModel mainConfig = configHelper.ReadHitServiceCoreConfig();

                SqlDataAdapter da;

                DataSet ds;

                ProtelHotelsModel protModel;

                string rowID;

                if (mainConfig != null)
                {
                    List<string> protelsDBs = mainConfig.config.config["ProtelDBs"];
                    foreach (string item in protelsDBs)
                    {
                        if (!string.IsNullOrWhiteSpace(checkConn.CheckConnection(item)))
                            continue;

                        SqlConnectionStringBuilder build = new SqlConnectionStringBuilder(item);
                        rowID = build.DataSource.Replace('\\', '_') + "-" + build.InitialCatalog;


                        using (SqlConnection db = new SqlConnection(item))
                        {
                            db.Open();

                            da = new SqlDataAdapter("SELECT l.mpehotel, l.hotel, l.short FROM lizenz AS l WHERE l.mpehq <> 1", db);
                            ds = new DataSet();
                            da.Fill(ds);

                            if (ds.Tables != null && ds.Tables[0].Rows != null)
                            {
                                foreach (DataRow row in ds.Tables[0].Rows)
                                {
                                    protModel = new ProtelHotelsModel();

                                    protModel.DBId = rowID.Replace('\\', '_') + "-" + ((int)row["mpehotel"]).ToString();
                                    protModel.Mpehotel = (int)row["mpehotel"];
                                    protModel.Name = (string)row["hotel"];
                                    protModel.SortName = (string)row["short"];

                                    if (!result.ContainsKey(protModel.DBId))
                                        result.Add(protModel.DBId, protModel);
                                }
                            }

                            if (db.State == System.Data.ConnectionState.Open)
                                db.Close();
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
