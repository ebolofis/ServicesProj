using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace HitHelpersNetCore.Helpers
{
    public class CheckConnectionsHelper
    {
        public CheckConnectionsHelper()
        {

        }

        /// <summary>
        /// Check if connection string is valid
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public string CheckConnection(string connectionString)
        {
            string result = "";
            try
            {
                using (SqlConnection db = new SqlConnection(connectionString))
                {
                    db.Open();
                    db.Close();
                }
            }
            catch(Exception ex)
            {
                result = ex.Message + (ex.InnerException != null ? "   inner exception : " + ex.InnerException.Message : "");
            }
            return result;
        }

    }
}
