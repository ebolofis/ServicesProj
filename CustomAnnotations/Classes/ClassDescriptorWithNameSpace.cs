using System;
using System.Collections.Generic;
using System.Text;

namespace HitCustomAnnotations.Interfaces
{
    public class ClassDescriptorWithNameSpace : IClassDescriptorWithNameSpace
    {
        /// <summary>
        /// Full name space
        /// </summary>
        public string fullNameSpace { get; private set; }

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
