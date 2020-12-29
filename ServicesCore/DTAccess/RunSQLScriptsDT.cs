using Dapper;
using HitServicesCore.Helpers;
using HitServicesCore.Models.IS_Services;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace HitServicesCore.DTAccess
{
    public class RunSQLScriptsDT
    {

        public RunSQLScriptsDT()
        {

        }

        /// <summary>
        /// Execute a script (insers/updates)
        /// </summary>
        /// <param name="constr">the connection string </param>
        /// <param name="path">the sql script to run</param>
        /// <param name="timeout">timeout in sec</param>
        public void RunScript(string constr, string script, int timeout = 60)
        {
            using (IDbConnection db = new SqlConnection(constr))
            {
                db.Execute(script, commandTimeout: timeout);
            }
        }

        /// <summary>
        ///  Return the results of a select query as IEnumerable(dynamic). (Use AutoMapper to convert dynamic to specific model)
        /// </summary>
        /// <param name="constr">the connection string</param>
        /// <param name="script">the select query to run</param>
        /// <returns></returns>
        public IEnumerable<dynamic> RunSelect(string constr, string script, int timeout = 60)
        {
            using (IDbConnection db = new SqlConnection(constr))
            {
                return db.Query(script, commandTimeout: timeout);
            }
        }

        /// <summary>
        ///  Return the results of a select query as IEnumerable(dynamic). (Use AutoMapper to convert dynamic to specific model)
        /// </summary>
        /// <param name="constr">the connection string</param>
        /// <param name="script">the select query to run</param>
        /// <returns></returns>
        public List<IEnumerable<dynamic>> RunSelectMulty(string constr, string script, int timeout = 60)
        {
            List<IEnumerable<dynamic>> result = new List<IEnumerable<dynamic>>();
            using (IDbConnection db = new SqlConnection(constr))
            {
                var m = db.QueryMultiple(script, commandTimeout: timeout);
                while (m.IsConsumed == false)
                    result.Add(m.Read());
            }
            return result;
        }

        /// <summary>
        ///  Return the multiple results of a select query as IEnumerable(dynamic). (Use AutoMapper to convert dynamic to specific model)
        /// </summary>
        /// <param name="constr">the connection string</param>
        /// <param name="script">the select query to run</param>
        /// <returns></returns>
        public List<IEnumerable<dynamic>> RunMultipleSelect(string constr, string scripts, int timeout = 60)
        {
            using (IDbConnection db = new SqlConnection(constr))
            {
                var m = db.QueryMultiple(scripts, commandTimeout: timeout);
                List<IEnumerable<dynamic>> list = new List<IEnumerable<dynamic>>();
                while (m.IsConsumed == false)
                {
                    list.Add(m.Read());
                }
                return list;
            }
        }

        /// <summary>
        /// Insert or Update data to a Data Table
        /// </summary>
        /// <param name="data">data to Insert or Update</param>
        /// <param name="constr">destination connection string</param>
        /// <param name="tableinfo">destination Table</param>
        /// <param name="operation">Operation to perform: 0: Inserts & Updates, 1: Inserts only, 2: Updates only </param>
        /// <param name="useTransaction">true if transaction is used</param>
        public void SaveToTable(List<IDictionary<string, dynamic>> data, string constr, DbTableModel tableinfo, int operation, bool useTransaction, int timeout = 60)
        {
            if (useTransaction)//use Transaction
            {
                using (IDbConnection db = new SqlConnection(constr))
                {
                    using (var scope = new TransactionScope())
                    {
                        SaveToTable(data, tableinfo, operation, db, timeout);
                        scope.Complete(); //commit
                    }
                }
            }
            else //No Transaction
            {
                using (IDbConnection db = new SqlConnection(constr))
                {
                    SaveToTable(data, tableinfo, operation, db, timeout);
                }
            }
        }

        /// <summary>
        /// Insert or Update data to a Data Table (core function)
        /// </summary>
        /// <param name="data">data to Insert or Update</param>
        /// <param name="tableinfo">destination Table</param>
        /// <param name="operation">Operation to perform: 0: Inserts & Updates, 1: Inserts only, 2: Updates only </param>
        /// <param name="db">db connection</param>
        private void SaveToTable(List<IDictionary<string, dynamic>> data, DbTableModel tableinfo, int operation, IDbConnection db, int timeout = 60)
        {
            //1. construct sql statements
            SqlConstructorHelper sqlConstruct = new SqlConstructorHelper();
            string insertSql = sqlConstruct.InsertStatment(tableinfo);     // insert query
            string updateSql = sqlConstruct.UpdateStatment(tableinfo);     // update query
            string countSql = sqlConstruct.SelectCount(tableinfo);         // select count(*)... query
            string lastquery = "";
            int count = 0;
            int inserts = 0;
            int updates = 0;

            //2. run inserts/updates
            foreach (IDictionary<string, dynamic> raw in data)
            {
                try
                {
                    if (operation == 0)// Inserts & Updates
                    {
                        lastquery = updateSql;
                        count = db.Query<int>(updateSql, raw, commandTimeout: timeout).FirstOrDefault();
                        if (count == 0)
                        {
                            lastquery = insertSql;
                            db.Query<int?>(insertSql, raw, commandTimeout: timeout).FirstOrDefault();
                            inserts++;
                        }
                    }
                    if (operation == 1)// Inserts only
                    {
                        lastquery = countSql;
                        count = db.Query<int>(countSql, raw, commandTimeout: timeout).FirstOrDefault();
                        if (count == 0)
                        {
                            lastquery = insertSql;
                            db.Query<int?>(insertSql, raw, commandTimeout: timeout).FirstOrDefault();
                            inserts++;
                        }
                        else
                            count = 0;
                    }
                    if (operation == 2)// Updates only
                    {
                        lastquery = updateSql;
                        count = db.Query<int>(updateSql, raw, commandTimeout: timeout).FirstOrDefault();
                    }
                    updates = updates + count;

                }
                catch (Exception ex)
                {
                    //logger.Error("Error         : " + ex.ToString());
                    //logger.Error(" ");
                    //logger.Error("   Last query : " + lastquery);
                    //logger.Error("   For Object : " + System.Text.Json.JsonSerializer.Serialize(raw));
                    string sMess = "";
                    sMess += "Error         : " + ex.ToString() + "\r\n";
                    sMess += " \r\n";
                    sMess += "   Last query : " + lastquery + " \r\n";
                    sMess += "   For Object : " + System.Text.Json.JsonSerializer.Serialize(raw);
                    throw new Exception(sMess);
                }
            }
            //logger.Info("SaveToTable Summary : " + data.Count() + " total raws to insert/update.  " + updates.ToString() + " raws updated, " + inserts.ToString() + " raws inserted.");
        }

        /// <summary>
        /// for a DB Table return the List of Columns 
        /// </summary>
        /// <param name="constr">connection string</param>
        /// <param name="tableName">Table Name</param>
        /// <returns>Table info</returns>
        public DbTableModel GetTableInfo(string constr, string tableName, int timeout = 60)
        {
            DbTableModel table = new DbTableModel();
            table.TableName = tableName;

            using (IDbConnection db = new SqlConnection(constr))
            {
                table.Columns = db.Query<DBColumnModel>(@"SELECT distinct
                        c.column_id 'Position',
                        c.name 'ColumnName',
                        tt.Name 'DataType',
                        c.max_length 'MaxLength',
                        c.precision 'Precision',
                        c.scale 'Scale',
                        c.is_nullable 'Nullable',
                        ISNULL(i.is_primary_key, 0) 'PrimaryKey',
	                    is_identity 'AutoIncrement'
                    FROM    
                        sys.columns c
                    INNER JOIN sys.tables t ON t.object_id = c.object_id AND t.name = @TableName
					OUTER APPLY(
						SELECT DISTINCT  i.is_primary_key, i.name
						FROM sys.indexes i
						INNER JOIN sys.index_columns ic ON  ic.object_id = i.object_id AND ic.column_id = c.column_id and ic.index_id=i.index_id
						WHERE i.object_id = t.object_id AND i.is_primary_key = 1 and i.type=1
					) i 
					INNER JOIN sys.types tt on tt.user_type_id=c.user_type_id
                  Order By Position", new { TableName = tableName }, commandTimeout: timeout).ToList();

            }
            return table;
        }
    }
}
