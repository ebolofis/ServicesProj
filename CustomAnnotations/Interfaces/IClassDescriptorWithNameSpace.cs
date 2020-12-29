using System;
using System.Collections.Generic;
using System.Text;

namespace HitCustomAnnotations.Interfaces
{
    public interface IClassDescriptorWithNameSpace : IClassDescriptor
    {
        /// <summary>
        /// Full name space of the class
        /// </summary>
        string fullNameSpace { get; }
    }
}
