using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HitServicesCore.Models.SharedModels
{
    public class InitialerDescriptorModel //: ClassDescriptorWithNameSpaceModel
    {
        /// <summary>
        /// PlugIn version for last initialser. Saved on db and executed until last one
        /// </summary>
        public string dbVersion { get; set; }

        /// <summary>
        /// The last version on the instalation pc that executed as update.
        /// </summary>
        public string latestUpdate { get; set; }

        /// <summary>
        /// The date that the last version on the instalation pc executed as update
        /// </summary>
        public DateTime latestUpdateDate { get; set; }

        /// <summary>
        /// Full class name space
        /// </summary>
        public string fullNameSpace { get; set; }

        /// <summary>
        /// Real file name for the dll to use it to create the instance of class by fullNameSpace
        /// </summary>
        public string assemblyFileName { get; set; }

        /// <summary>
        /// Plug In Guid where the initializer belongs
        /// </summary>
        public Guid plugIn_Id { get; set; }
    }
}
