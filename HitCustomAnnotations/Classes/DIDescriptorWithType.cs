using HitServicesCore.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace HitCustomAnnotations.Interfaces
{
    public class DIDescriptorWithType : IDIDescriptorWithType
    {
        /// <summary>
        /// Load the class
        /// </summary>
        public Type classType { get; private set; }

        /// <summary>
        /// Enumerator for adding the class to DI as Singleton, Scope or Transient from custom annotation AddClassesToContainer
        /// </summary>
        public ServicesAddTypeEnum scope { get; private set; }

        /// <summary>
        /// class Description from custom annotation AddClassesToContainer
        /// </summary>
        public string di_ClassDescription { get; private set; }

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
