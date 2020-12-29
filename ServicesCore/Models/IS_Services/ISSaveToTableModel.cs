using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HitServicesCore.Models.IS_Services
{
    /// <summary>
    /// Model to get services for saving data from one database to another
    /// </summary>
    public class ISSaveToTableModel : ISServiceGeneralModel
    {
        /// <summary>
        /// Source connection string 
        /// </summary>
        public string SourceDB { get; set; } = "server = <SERVER>; user id = <USER>; password = <PASSWORD>; database = <DB>;";

        /// <summary>
        /// Desctination connection string 
        /// </summary>
        public string DestinationDB { get; set; } = "server = <SERVER>; user id = <USER>; password = <PASSWORD>; database = <DB>;";

        /// <summary>
        /// Table name to save data
        /// </summary>
        public string DestinationDBTableName { get; set; }

        /// <summary>
        /// Type of operation: 0: Insert & Update data, 1: Insert data only, 2: Update data only
        /// </summary>
        public int DBOperation { get; set; }

        /// <summary>
        /// Transaction
        /// </summary>
        public bool DBTransaction { get; set; }

        /// <summary>
        /// Command Time out
        /// </summary>
        public string DBTimeout { get; set; } = "60";

        #region SQL Script
        /// <summary>
        /// Sql Script to execute. All code is writen here
        /// </summary>
        public string SqlScript { get; set; }

        /// <summary>
        /// Parameters for sql script
        /// </summary>
        public Dictionary<string, string> SqlParameters { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// Script ro execute before execute main script
        /// </summary>
        public string SqlDestPreScript { get; set; }
        #endregion
    }
}
