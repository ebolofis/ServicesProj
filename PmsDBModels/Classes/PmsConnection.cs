using System;
using System.Collections.Generic;
using System.Text;

namespace PmsDBModels.Classes
{
    public class PmsConnection
    {
        /// <summary>
        /// Pms Connection String
        /// </summary>
        public string connection;

        public PmsConnection(string _connection)
        {
            connection = _connection;
        }
    }
}
