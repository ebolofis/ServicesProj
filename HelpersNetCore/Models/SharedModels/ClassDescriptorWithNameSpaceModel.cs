using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HitServicesCore.Models.SharedModels
{
    public class ClassDescriptorWithNameSpaceModel
    {
        /// <summary>
        /// Full name space
        /// </summary>
        public string fullNameSpace { get; set; }

        /// <summary>
        /// File name (dll name)
        /// </summary>
        public string fileName { get; set; }

        /// <summary>
        /// Path where dll exists
        /// </summary>
        public string path { get; set; }
    }
}
