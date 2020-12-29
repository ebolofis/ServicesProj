using HitServicesCore.DTAccess;
using HitServicesCore.Models.IS_Services;
//using Microsoft.Extensions.Logging;
using NLog;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HitServicesCore.MainLogic.Tasks
{
    public class SQLTasks
    {
        /// <summary>
        /// instance for DT
        /// </summary>
        private readonly RunSQLScriptsDT runScriptDT;

        /// <summary>
        /// Instance of Run sql script settings model
        /// </summary>
        private readonly ISRunSqlScriptsModel settings;

        public SQLTasks(ISRunSqlScriptsModel _settings) 
        {
            settings = _settings;
            runScriptDT = new RunSQLScriptsDT();
        }

        /// <summary>
        ///  Run sql script (insert/updates/deletes/creates etc)
        /// </summary>
        /// <param name="sqlScript">sql script to run</param>
        /// <param name="conString">connection string. If null then pick settings.Custom1DB</param>
        public void RunScript(string sqlScript, string conString = null)
        {
            if (conString == null) conString = settings.Custom1DB;
            int timeout = Int32.Parse(settings.DBTimeout);
            //Exec Script to DB
            runScriptDT.RunScript(conString, sqlScript, timeout);
        }

        /// <summary>
        /// Return the results of a select query as IEnumerable(dynamic). (Use AutoMapper to convert dynamic to specific model)
        /// </summary>
        /// <param name="sqlScript">sql script to run</param>
        /// <param name="conString">connection string. If null then pick  settings.Custom1DB </param>
        /// <returns>the reusult of the script</returns>
        public IEnumerable<dynamic> RunSelect(string sqlScript, string conString)
        {
            int timeout = Int32.Parse(settings.DBTimeout);
            //Exec Script to DB
            return runScriptDT.RunSelect(conString, sqlScript, timeout);
        }

        /// <summary>
        ///  Return the results of a select query as IEnumerable(dynamic). (Use AutoMapper to convert dynamic to specific model)
        /// </summary>
        /// <param name="constr">the connection string</param>
        /// <param name="script">the select query to run</param>
        /// <returns></returns>
        public IEnumerable<dynamic> RunSelectMulty(string sqlScript, string conString)
        {
            int timeout = Int32.Parse(settings.DBTimeout);
            //Exec Script to DB
            return runScriptDT.RunSelectMulty(conString, sqlScript, timeout);
        }

        /// <summary>
        /// Return the results of a select query as IEnumerable(dynamic). (Use AutoMapper to convert dynamic to specific model)
        /// </summary>
        /// <param name="sqlScript">sql script to run</param>
        /// <param name="conString">connection string. If null then pick  settings.Custom1DB </param>
        /// <returns>the reusult of the script</returns>
        public List<IEnumerable<dynamic>> RunMultySelect(string sqlScript, string conString = null, int timeout = 0)
        {
            if (conString == null) 
                conString = settings.Custom1DB;
            
            if (timeout == 0) 
                timeout = Int32.Parse(settings.DBTimeout);
            //Exec Script to DB
            return runScriptDT.RunMultipleSelect(conString, sqlScript, timeout);
        }

        /// <summary>
        /// for a DB Table return the List of Columns 
        /// </summary>
        /// <param name="constr">connection string</param>
        /// <param name="tableName">Table Name</param>
        /// <returns>Table info</returns>
        public DbTableModel GetTableInfo(string constr, string tableName, int timeout = 60)
        {
            return runScriptDT.GetTableInfo(constr, tableName, timeout);
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
            runScriptDT.SaveToTable(data, constr, tableinfo, operation, useTransaction, timeout);
        }
    }
}
