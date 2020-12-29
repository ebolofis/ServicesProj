using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace HitCustomAnnotations.Interfaces
{
    public interface IInitialerDescriptor //: IClassDescriptor
    {
        /// <summary>
        /// PlugIn version for last initialser. Saved on db and executed until last one
        /// </summary>
        string dbVersion { get; }

        /// <summary>
        /// Start running the Initilizer methods
        /// </summary>
        void Start(string lastUpdatedVarsion, IApplicationBuilder _app);
    }
}
