using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace HitServicesCore.Models.SharedModels
{
    public class MainDescriptorWithAssemplyModel : ClassDescriptorWithNameSpaceModel
    {
        /// <summary>
        /// Assemply of the class type)
        /// </summary>
        public Assembly assembly { get; set; }

        /// <summary>
        /// Unique guid as id. generated from plugin author and it is the same for all releases 
        /// </summary>
        public Guid plugIn_Id { get; set; }

        /// <summary>
        /// Unique name for plugin. 
        /// If not exists on a plugin dll then the plugin will not be loaded from main project (HitServicesCore)
        /// </summary>
        public string plugIn_Name { get; set; }

        /// <summary>
        /// Description for plugin
        /// </summary>
        public string plugIn_Description { get; set; }

        /// <summary>
        /// Plugin version
        /// </summary>
        public string plugIn_Version { get; set; }

    }
}
