using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HitServicesCore.Models
{
    /// <summary>
    /// Model fo general info about project
    /// </summary>
    public class SystemInfo
    {
        /// <summary>
        /// Version no
        /// </summary>
        public string version { get; set; }

        /// <summary>
        /// Base executable path
        /// </summary>
        public string rootPath { get; set; }

        /// <summary>
        /// Path to search for plugins to load
        /// </summary>
        public string pluginPath { get; set; }

        /// <summary>
        /// Paath to search fo plugin files to load
        /// </summary>
        public string pluginFilePath { get; set; }

    }
}
