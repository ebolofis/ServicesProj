using System;
using System.Collections.Generic;
using System.Text;

namespace HitHelpersNetCore.Models.PmsDBModels
{
    /// <summary>
    /// Describe a connection string to a DB (Protel, POS, HitPOS, ERMIS, other)
    /// </summary>
    public class DBModel
    {
        /// <summary>
        /// Unique DB id: Server-DB
        /// </summary>
        public string DBId { get; set; }

        /// <summary>
        /// Server name
        /// </summary>
        public string Server { get; set; }

        /// <summary>
        /// DataBase name
        /// </summary>
        public string Db { get; set; }

        /// <summary>
        /// User schema
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// User Password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// The entire ConnectionString: Server=server;Database=db;User id=user;Password=password
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Guid from usersToDatabases.xml (only for pos)
        /// </summary>
        public string Guid { get; set; }
    }
}
