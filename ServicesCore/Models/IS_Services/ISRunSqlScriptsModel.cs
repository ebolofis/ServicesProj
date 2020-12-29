using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HitServicesCore.Models.IS_Services
{
    /// <summary>
    /// Model to run sql queries
    /// </summary>
    public class ISRunSqlScriptsModel : ISServiceGeneralModel
    {
        #region DB
        /// <summary>
        /// CustomDB1 connection string 
        /// </summary>
        public string Custom1DB { get; set; } = "server = <SERVER>; user id = <USER>; password = <PASSWORD>; database = <DB>;";

        /// <summary>
        /// Transaction
        /// </summary>
        public string DBTimeout { get; set; } = "60";

        #endregion

        #region SQL Script
        /// <summary>
        /// Sql Script to execute. All code is writen here
        /// </summary>
        public string SqlScript { get; set; }

        /// <summary>
        /// Parameters for sql script
        /// </summary>
        public Dictionary<string, string> SqlParameters { get; set; } = new Dictionary<string, string>();

        #endregion
    }
}
