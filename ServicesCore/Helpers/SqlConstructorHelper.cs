using HitServicesCore.Models.IS_Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HitServicesCore.Helpers
{
    public class SqlConstructorHelper
    {
        CultureInfo CultureInfo = CultureInfo.CreateSpecificCulture("en-us");


        /// <summary>
        /// Construct an Insert statement.  
        ///  If data=null (recommended) then return insert statement with parameters instead of values, Else return insert statement with values (NOT recommended for binary columns).  
        ///  
        /// </summary>
        /// <param name="tableInfo">table info</param>
        /// <param name="data">row's data. Every key in the dictionary must correspont to a column in tableInfo</param>
        /// <param name="returnIdentity">if true then return the last identity</param>
        /// <param name="SqlKeyModel">info for encrypted columns</param>
        /// <returns></returns>
        public string InsertStatment(DbTableModel tableInfo, IDictionary<string, dynamic> data = null, bool returnIdentity = true, SqlKeyModel sqlEncrypt = null)
        {
            bool encrypt = false;
            List<DBColumnModel> columns = tableInfo.Columns.Where(x => x.AutoIncrement == false).ToList();
            StringBuilder sql = new StringBuilder();
            if (sqlEncrypt != null && sqlEncrypt.EncryptedColumns != null && sqlEncrypt.EncryptedColumns.Count > 0)
            {
                encrypt = true;
                sql.Append(@"OPEN SYMMETRIC KEY " + sqlEncrypt.SymmetricKey + @"
                    DECRYPTION BY CERTIFICATE " + sqlEncrypt.Certificate + @"
                    with PASSWORD = '" + sqlEncrypt.Password + "';" + @"
                 ");
            }
            sql.Append("INSERT INTO [" + tableInfo.TableName + "] (");
            sql.Append(String.Join(", ", columns.Select(x => "[" + x.ColumnName + "]").ToArray()));
            sql.Append(") VALUES (");

            if (data == null)
            {
                //create insert statement with parameters instead of values
                sql.Append(String.Join(", ", columns.Select(x =>
                {
                    if (encrypt && sqlEncrypt.EncryptedColumns.FirstOrDefault(str => x.ColumnName.ToUpper() == str.ToUpper()) != null)
                        return "EncryptByKey(Key_GUID('" + sqlEncrypt.SymmetricKey + "'), @" + x.ColumnName + ")";
                    else
                        return "@" + x.ColumnName;
                }).ToArray()));
            }
            else
            {
                //create insert statement with  values
                int i = 0;
                foreach (DBColumnModel column in columns)
                {
                    if (encrypt && sqlEncrypt.EncryptedColumns.FirstOrDefault(str => column.ColumnName.ToUpper() == str.ToUpper()) != null)
                        sql.Append("EncryptByKey(Key_GUID('" + sqlEncrypt.SymmetricKey + "'),'" + SqlToString(column.DataType, data[column.ColumnName]) + "')");
                    else
                        sql.Append(SqlToString(column.DataType, data[column.ColumnName]));
                    i++;
                    if (i < columns.Count) sql.Append(",");
                }
            }
            sql.Append(");");
            if (!returnIdentity) sql.Append("select SCOPE_IDENTITY() ");
            return sql.ToString();
        }


        /// <summary>
        /// Construct an Update statement.  
        ///  If data=null (recommended) then return update statement with parameters instead of values, Else return update statement with values (NOT recommended for binary columns). 
        ///   Return number of raws affected.
        /// </summary>
        /// <param name="tableInfo">table info</param>
        /// <param name="data">row's data. Every key in the dictionary must correspont to a column in tableInfo</param>
        /// <param name="SqlKeyModel">info for encrypted columns</param>
        /// <returns>Return number of raws affected.</returns>
        public string UpdateStatment(DbTableModel tableInfo, IDictionary<string, dynamic> data = null, SqlKeyModel sqlEncrypt = null)
        {
            bool encrypt = false;
            List<DBColumnModel> noKeyColumns = tableInfo.Columns.Where(x => x.PrimaryKey == false).ToList();
            List<DBColumnModel> keyColumns = tableInfo.Columns.Where(x => x.PrimaryKey == true).ToList();
            int i = 0;

            StringBuilder sql = new StringBuilder();
            if (sqlEncrypt != null && sqlEncrypt.EncryptedColumns != null && sqlEncrypt.EncryptedColumns.Count > 0)
            {
                encrypt = true;
                sql.Append(@"OPEN SYMMETRIC KEY " + sqlEncrypt.SymmetricKey + @"
                    DECRYPTION BY CERTIFICATE " + sqlEncrypt.Certificate + @"
                    with PASSWORD = '" + sqlEncrypt.Password + "';" + @"
                 ");
            }

            if (keyColumns == null || keyColumns.Count() == 0)
                throw new Exception("Table [" + tableInfo.TableName + "] does NOT contain Primary Key. Unable to update.");

            sql.Append("UPDATE [" + tableInfo.TableName + "] SET ");
            foreach (DBColumnModel column in noKeyColumns)
            {
                sql.Append("[" + column.ColumnName + "] = ");
                if (data == null)
                {
                    if (encrypt && sqlEncrypt.EncryptedColumns.FirstOrDefault(str => column.ColumnName.ToUpper() == str.ToUpper()) != null)
                        sql.Append("EncryptByKey(Key_GUID('" + sqlEncrypt.SymmetricKey + "'), @" + column.ColumnName + ")");
                    else
                        sql.Append("@" + column.ColumnName);
                }
                else
                {
                    if (encrypt && sqlEncrypt.EncryptedColumns.FirstOrDefault(str => column.ColumnName.ToUpper() == str.ToUpper()) != null)
                        sql.Append("EncryptByKey(Key_GUID('" + sqlEncrypt.SymmetricKey + "'),'" + SqlToString(column.DataType, data[column.ColumnName]) + "')");
                    else
                        sql.Append(SqlToString(column.DataType, data[column.ColumnName]));
                }
                i++;
                if (i < noKeyColumns.Count) sql.Append(", ");
            }
            sql.Append(" WHERE ");

            //where clause
            i = 0;
            foreach (DBColumnModel column in keyColumns)
            {
                sql.Append("[" + column.ColumnName + "] = ");
                if (data == null)
                {
                    sql.Append("@" + column.ColumnName);
                }
                else
                {
                    sql.Append(SqlToString(column.DataType, data[column.ColumnName]));
                }
                i++;
                if (i < keyColumns.Count) sql.Append(" AND ");
            }

            sql.Append("; select @@ROWCOUNT");

            return sql.ToString();
        }


        /// <summary>
        /// Construct query like : select count(*) from  table WHERE id=...   
        ///  If data=null (recommended) then return update statement with parameters instead of values, Else return update statement with values (NOT recommended for binary columns).
        /// </summary>
        /// <param name="tableInfo">table info</param>
        /// <param name="data">row's data. Every key in the dictionary must correspont to a column in tableInfo</param>
        /// <returns></returns>
        public string SelectCount(DbTableModel tableInfo, IDictionary<string, dynamic> data = null)
        {
            List<DBColumnModel> keyColumns = tableInfo.Columns.Where(x => x.PrimaryKey == true).ToList();
            int i = 0;

            if (keyColumns == null || keyColumns.Count() == 0)
                throw new Exception("Table [" + tableInfo.TableName + "] does NOT contain Primary Key. Unable to construct select count query.");

            StringBuilder sql = new StringBuilder("select count(*) from  [" + tableInfo.TableName + "] WHERE ");

            //where clause
            i = 0;
            foreach (DBColumnModel column in keyColumns)
            {
                sql.Append("[" + column.ColumnName + "] = ");
                if (data == null)
                {
                    sql.Append("@" + column.ColumnName);
                }
                else
                {
                    sql.Append(SqlToString(column.DataType, data[column.ColumnName]));
                }
                i++;
                if (i < keyColumns.Count) sql.Append(" AND ");
            }

            sql.Append("");

            return sql.ToString();
        }

        /// <summary>
        /// Convert a value to string based on sql datatype
        /// </summary>
        /// <param name="dataType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public string SqlToString(string dataType, dynamic value)
        {
            if (value == null) return "null";

            if (dataType == "decimal" ||
                dataType == "money" ||
                dataType == "numeric" ||
                dataType == "float")
                return value.ToString("F6", CultureInfo);

            if (dataType == "date")
                return "'" + value.ToString("yyyy-MM-dd") + "'";

            if (dataType == "datetime2" ||
                dataType == "datetime" ||
                dataType == "smalldatetime")
                return "'" + value.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo) + "'";

            if (dataType == "varbinary" ||
               dataType == "binary" ||
               dataType == "image")
                return Encoding.UTF8.GetString(value);// <-- ?????

            return "'" + value.ToString() + "'";

        }
    }
}
