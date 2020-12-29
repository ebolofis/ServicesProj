using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HitServicesCore.Models.IS_Services
{
    /// <summary>
    /// Class representing a DB Table 
    /// </summary>
    public class DbTableModel
    {
        /// <summary>
        /// Table Name
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// List of columns
        /// </summary>
        public List<DBColumnModel> Columns { get; set; }
    }

    /// <summary>
    /// Class representing a DB Table Column
    /// </summary>
    public class DBColumnModel
    {
        /// <summary>
        /// column's position (1,2,3,...)
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// Column Name
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// Column Type
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        /// Column Max Length
        /// </summary>
        public int MaxLength { get; set; }

        /// <summary>
        /// Column Precision
        /// </summary>
        public double Precision { get; set; }

        /// <summary>
        /// Column Scale
        /// </summary>
        public double Scale { get; set; }

        /// <summary>
        /// true if column is Key
        /// </summary>
        public bool PrimaryKey { get; set; }

        /// <summary>
        /// true if column is nullable
        /// </summary>
        public bool Nullable { get; set; }

        /// <summary>
        /// true if column is AutoIncrement 
        /// </summary>
        public bool AutoIncrement { get; set; }
    }
}
