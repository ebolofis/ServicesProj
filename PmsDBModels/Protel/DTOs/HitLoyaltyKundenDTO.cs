using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace PmsDBModels.Protel.DTOs
{
    [Table("hit_loyalty_kunden")]
    public class HitLoyaltyKundenDTO
    {
        /// <summary>
        /// profile id from kunden.kdnr
        /// </summary>
        [Key]
        [Required]
        public long Kdnr { get; set; }

        /// <summary>
        /// Total Loyalty points
        /// </summary>
        public long Points { get; set; }

        /// <summary>
        /// Rewarded Loyalty points
        /// </summary>
        public long Reward { get; set; }

        /// <summary>
        /// Profile password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// class id
        /// </summary>
        public Nullable<long> Class { get; set; }

        /// <summary>
        /// count of cards
        /// </summary>
        public long CardCount { get; set; }

        /// <summary>
        /// customer has change class
        /// </summary>
        public bool classChanged { get; set; }

        /// <summary>
        /// allow receive emails
        /// </summary>
        public bool ReceiveMails { get; set; }
    }
}
