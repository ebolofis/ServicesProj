using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HitServicesCore.Models.IS_Services
{
    /// <summary>
    /// Read a csv file and write to table Model
    /// </summary>
    public class ISReadFromCsvModel : ISServiceGeneralModel
    {
        #region DB
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

        #endregion

        #region SQL Script
        /// <summary>
        /// Sql script to run before import data from csv to table
        /// </summary>
        public string SqlDestPreScript { get; set; }

        #endregion

        #region Csv
        /// <summary>
        /// Path for the csv path. (if path contains &lt;TIMESTAMP&#62; then this is replased by current timespan )
        /// </summary>
        public string CsvFilePath { get; set; }//(if path contains <TIMESTAMP> then this is replased by current timespan )

        /// <summary>
        /// If checked then the 1st line of the file will be header.
        /// </summary>
        public bool? CsvFileHeader { get; set; } = true;

        /// <summary>
        /// List of header's names. If CsvFileHeader is unchecked then headers MUST be declered manually in CsvFileHeaders.
        /// </summary>
        public List<string> CsvFileHeaders { get; set; }

        /// <summary>
        /// Delimeter (ex: ;,- or tab)
        /// </summary>
        public string CsvDelimenter { get; set; } = ";";

        /// <summary>
        /// Encoding the csv file record before insert to database
        /// </summary>
        public string CsvEncoding { get; set; } = ";";

        #endregion

        #region "Helper Methods - Never use in win form"

        /// <summary>
        /// Encoding (Helper Method)
        /// </summary>
        public Encoding Encoding { get; set; }

        #endregion
    }
}
