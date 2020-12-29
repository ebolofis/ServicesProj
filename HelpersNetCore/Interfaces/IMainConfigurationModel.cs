using HitHelpersNetCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HitHelpersNetCore.Interfaces
{
    public interface IMainConfigurationModel
    {
        Guid? plugInId { get; set; }

        /// <summary>
        /// configuration values
        /// </summary>
        MainConfiguration config { get; set; }

        /// <summary>
        /// Descriptors
        /// </summary>
        MainConfigDescriptors descriptors { get; set; }
    }
}
