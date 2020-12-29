using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HitServicesCore.Models.IS_Services
{
    public class SqlKeyModel
    {
        /// <summary>
        /// name of symetric key
        /// </summary>
        public string SymmetricKey { get; set; }

        /// <summary>
        /// name of certificate
        /// </summary>
        public string Certificate { get; set; }

        /// <summary>
        /// password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// list of encrypted columns
        /// </summary>
        public List<string> EncryptedColumns { get; set; }
    }
}
