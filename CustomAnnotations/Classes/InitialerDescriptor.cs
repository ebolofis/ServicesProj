using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace HitCustomAnnotations.Interfaces
{
    public class InitialerDescriptor : IInitialerDescriptor
    {
        /// <summary>
        /// PlugIn version for last initialser. Saved on db and executed until last one
        /// </summary>
        public string dbVersion { get; private set; }

        ///// <summary>
        ///// File name (dll name)
        ///// </summary>
        //public string fileName { get; private set; }

        ///// <summary>
        ///// Path where dll exists
        ///// </summary>
        //public string path { get; private set; }

        /// <summary>
        /// start running the initilazer
        /// </summary>
        public void Start(string lastUpdatedVarsion, IApplicationBuilder _app)
        {
            throw new NotImplementedException();
        }
    }
}
