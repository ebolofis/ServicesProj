using HitServicesCore.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HitServicesCore.Models.SharedModels
{
    public class DIDescriptorWithTypeModel : ClassDescriptorWithNameSpaceModel
    {
        /// <summary>
        /// Load the class
        /// </summary>
        public Type classType { get; set; }

        /// <summary>
        /// Enumerator for adding the class to DI as Singleton, Scope or Transient from custom annotation AddClassesToContainer
        /// </summary>
        public ServicesAddTypeEnum scope { get; set; }

        /// <summary>
        /// class Description from custom annotation AddClassesToContainer
        /// </summary>
        public string di_ClassDescription { get; set; }
    }
}
