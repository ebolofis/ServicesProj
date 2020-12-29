using System;
using System.Collections.Generic;
using System.Text;

namespace HitCustomAnnotations.Interfaces
{
    public interface IDIDescriptorWithType 
    {
        /// <summary>
        /// Load the class
        /// </summary>
        Type classType { get; }
    }
}
