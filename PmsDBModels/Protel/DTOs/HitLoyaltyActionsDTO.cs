using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace PmsDBModels.Protel.DTOs
{
    [Table("hit_loyalty_actions")]
    public class HitLoyaltyActionsDTO
    {
        /// <summary>
        /// Record Id
        /// </summary>
        [Key]
        public long Id { get; set; }

        /// <summary>
        /// Profile Id (Hit_Loyalty_Kunden.kdnr)
        /// </summary>
        public long Kdnr { get; set; }

        /// <summary>
        /// Inserted date
        /// </summary>
        public DateTime ActionDate { get; set; }

        /// <summary>
        /// Type
        /// 1   => REWARD
        /// 2   => CONSUMPTION
        /// 3   => INITIAL POINTS
        /// 4   => GIFT
        /// 5   => RETURN
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// Points
        /// </summary>
        public long Points { get; set; }

        /// <summary>
        /// Comments
        /// </summary>
        public string Notes { get; set; }
    }

}
