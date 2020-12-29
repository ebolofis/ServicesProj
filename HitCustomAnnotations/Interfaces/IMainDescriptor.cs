using System;
using System.Collections.Generic;
using System.Text;

namespace HitCustomAnnotations.Interfaces
{
    public interface IMainDescriptor //: IClassDescriptor
    {
        /// <summary>
        /// Unique guid as id. generated from plugin author and it is the same for all releases 
        /// </summary>
        Guid plugIn_Id { get;  }

        /// <summary>
        /// Unique name for plugin. Must ends with Plugin to be able the back offece read them
        /// If not exists on a plugin dll then the plugin will not be loaded from main project (HitServicesCore)
        /// </summary>
        string plugIn_Name { get; }

        /// <summary>
        /// Description for plugin
        /// </summary>
        string plugIn_Description { get; }

        /// <summary>
        /// Plugin version
        /// </summary>
        //string plugIn_Version { get; }

        
    }
}
