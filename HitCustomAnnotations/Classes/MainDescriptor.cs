using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace HitCustomAnnotations.Interfaces
{
    public class MainDescriptor : IMainDescriptor
    {
        /// <summary>
        /// Unique guid as id. generated from plugin author and it is the same for all releases 
        /// </summary>
        public Guid plugIn_Id { get; private set; }

        /// <summary>
        /// Unique name for plugin. 
        /// If not exists on a plugin dll then the plugin will not be loaded from main project (HitServicesCore)
        /// </summary>
        public string plugIn_Name { get; private set; }

        /// <summary>
        /// Description for plugin
        /// </summary>
        public string plugIn_Description { get; private set; }

        /// <summary>
        /// Plugin version
        /// </summary>
        //public string plugIn_Version { get; private set; }

        /// <summary>
        /// File name (dll name)
        /// </summary>
        public string fileName { get; private set; }

        /// <summary>
        /// Path where dll exists
        /// </summary>
        public string path { get; private set; }
        
    }
}
