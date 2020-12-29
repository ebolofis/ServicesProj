using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HitServicesCore.Models
{
    public class InitializersLastUpdateModel
    {
        /// <summary>
        /// Plug In Id Guid to find on json file
        /// </summary>
        public Guid plugInId { get; set; }

        /// <summary>
        /// The last version on the instalation pc that executed as update.
        /// </summary>
        public string latestUpdate { get; set; }

        /// <summary>
        /// The date that the last version on the instalation pc executed as update
        /// </summary>
        public DateTime latestUpdateDate { get; set; }
    }
}
