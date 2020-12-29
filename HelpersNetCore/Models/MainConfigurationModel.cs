using HitHelpersNetCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace HitHelpersNetCore.Models
{
    /// <summary>
    /// Main configuration Model for all plugins and ServiceCore.
    /// When plugInId is empty Guid refers to HitServiceCore else to plugin
    /// </summary>
    public class MainConfigurationModel : IMainConfigurationModel
    {
        /// <summary>
        /// use it to find on a list of plugins and make changes
        /// </summary>
        public Guid? plugInId { get; set; }

        /// <summary>
        /// Full name with namespace for class inherited the abstract
        /// </summary>
        public string configClassName { get; set; }

        /// <summary>
        /// configuration values
        /// </summary>
        public MainConfiguration config { get; set; }

        public MainConfigDescriptors descriptors { get; set; }
    }

    /// <summary>
    /// Configuration File
    /// </summary>
    public class MainConfiguration
    {
        /// <summary>
        /// File name
        /// </summary>
        public string fileName { get; set; }

        /// <summary>
        /// File Path
        /// </summary>
        public string basePath { get; set; }

        /// <summary>
        /// File values
        /// </summary>
        public Dictionary<string, dynamic> config { get; set; }
    }

    /// <summary>
    /// Config descpritions
    /// </summary>
    public class MainConfigDescriptors
    {
        /// <summary>
        /// File name
        /// </summary>
        public string fileName { get; set; }

        /// <summary>
        /// File path
        /// </summary>
        public string basePath { get; set; }

        /// <summary>
        /// File values
        /// </summary>
        public Dictionary<string, List<DescriptorsModel>> descriptions { get; set; }
    }

    /// <summary>
    /// Model for descriptors
    /// </summary>
    public class DescriptorsModel
    {
        /// <summary>
        /// Name of Configuration Value
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Description of Configuration Value
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Typr of Value (string, boolean, datetime, integer, ...)
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// The Default Value when Configuration Load
        /// </summary>
        public string DefaultValue { get; set; }

        /// <summary>
        /// The Api Version that We Create the value 
        /// </summary>
        public string ApiVersion { get; set; }

        /// <summary>
        /// true  => value on ui is not visible
        /// false => value on ui is visible
        /// </summary>
        public string IsHidden { get; set; } = "false";
    }
}
