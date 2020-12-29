using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace PmsDBModels.Protel.DTOs
{
    [Table("hit_loyalty_cards")]
    public class HitLoyaltyCardsDTO
    {
        /// <summary>
        /// Record Id
        /// </summary>
        [Key]
        public long Id { get; set; }
        
        /// <summary>
        /// Profile kdnr (hit_loyalty_kunden.kdnr)
        /// </summary>
        public long Kdnr { get; set; }

        /// <summary>
        /// card number
        /// </summary>
        public string CardNum { get; set; }

        /// <summary>
        /// Issue date
        /// </summary>
        public DateTime IssueDate { get; set; }

        /// <summary>
        /// Expiration date
        /// </summary>
        public DateTime ExpDate { get; set; }

        /// <summary>
        /// Card class
        /// </summary>
        public long Class { get; set; }

        /// <summary>
        /// Card status
        /// 1   => Active
        /// 0   => Not Active
        /// </summary>
        public int Status { get; set; }
    }
}
