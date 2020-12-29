using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace PmsDBModels.Protel.DTOs
{
    [Table("hit_loyalty_classes_benefits")]
    public class HitLoyaltyClassesBenefitsDTO
    {
        /// <summary>
        /// Record Id
        /// </summary>
        [Key]
        public long id { get; set; }

        /// <summary>
        /// class Id belong the benefit
        /// </summary>
        public long classId { get; set; }

        /// <summary>
        /// Order no
        /// </summary>
        public int position { get; set; }

        /// <summary>
        /// Benefit Description
        /// </summary>
        public string description { get; set; }

        /// <summary>
        /// True : Is Active
        /// False: not Active
        /// </summary>
        public bool isactive { get; set; }
    }
}
