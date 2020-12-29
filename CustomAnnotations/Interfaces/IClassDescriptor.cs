using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace HitCustomAnnotations.Interfaces
{
    public interface IClassDescriptor
    {
        /// <summary>
        /// File name (dll name)
        /// </summary>
        string fileName { get; }

        /// <summary>
        /// Path where dll exists
        /// </summary>
        string path { get; }

    }
}
