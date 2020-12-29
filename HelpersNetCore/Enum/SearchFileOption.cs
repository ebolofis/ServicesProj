using System;
using System.Collections.Generic;
using System.Text;

namespace HitHelpersNetCore.Enum
{
    /// <summary>
    /// Options on how to search files
    /// </summary>
    public enum SearchFileOption
    {
        /// <summary>
        /// Search files only to the top directory
        /// </summary>
        TopDirectoryOnly = 0,
        /// <summary>
        /// Search files to the top directory and to any sub-folder
        /// </summary>
        AllDirectories = 1
    }
}
