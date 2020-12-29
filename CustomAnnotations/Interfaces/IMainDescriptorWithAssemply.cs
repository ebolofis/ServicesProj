using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace HitCustomAnnotations.Interfaces
{
    public interface IMainDescriptorWithAssemply : IMainDescriptor, IClassDescriptor
    {
        /// <summary>
        /// Assemply of the class type)
        /// </summary>
        Assembly assembly { get; }

    }
}
