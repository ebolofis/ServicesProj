using System;
using System.Collections.Generic;
using System.Text;

namespace HitHelpersNetCore.Models.PmsDBModels
{
    /// <summary>
    /// Describe a Protel's Hotel
    /// </summary>
    public class ProtelHotelsModel
    {
        /// <summary>
        /// Unique DB id: Server-DB-mpehotel
        /// </summary>
        public string DBId { get; set; }

        /// <summary>
        /// Protel Id
        /// </summary>
        public int Mpehotel { get; set; }

        /// <summary>
        /// Hotel's name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Hotel's short
        /// </summary>
        public string SortName { get; set; }
    }
}
