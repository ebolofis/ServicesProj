using System;
using System.Collections.Generic;
using System.Text;

namespace HitHelpersNetCore.Models.SharedModels
{
    public class ConfigDescriptorModel
    {
        /// <summary>
        /// The full class name of the class
        /// </summary>
        public string fullClassName { get; set; }

        /// <summary>
        /// The assembly file f the class
        /// </summary>
        public Type confFile { get; set; }
    }
}
