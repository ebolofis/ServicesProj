using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace PmsDBModels.Protel.DTOs
{
    [Table("hit_loyalty_classes")]
    public class HitLoyaltyClassesDTO
    {
        /// <summary>
        /// Record Id
        /// </summary>
        [Key]
        public long Id { get; set; }

        /// <summary>
        /// Class Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// min poits to get the class
        /// </summary>
        public long Threshold { get; set; }

        /// <summary>
        /// class image
        /// </summary>
        public byte[]? Image { get; set; }

        /// <summary>
        /// Class color
        /// </summary>
        public string rgb { get; set; }

        /// <summary>
        /// discount for pos
        /// </summary>
        public int? fnbdiscount { get; set; }

        /// <summary>
        /// min stays to change class
        /// </summary>
        public int? minStay { get; set; }

        /// <summary>
        /// max stays to change class
        /// </summary>
        public int? maxStay { get; set; }

        /// <summary>
        /// min overnights to change class
        /// </summary>
        public int? minOverNights { get; set; }

        /// <summary>
        /// max overnights to change class
        /// </summary>
        public int? maxOverNights { get; set; }

        /// <summary>
        /// Total revenues to change class
        /// </summary>
        public decimal? Revenues { get; set; }

        /// <summary>
        /// On Revenues include Arrangment
        /// </summary>
        public bool? RevArrang { get; set; }

        /// <summary>
        /// On Revenues include F&B
        /// </summary>
        public bool? RevFB { get; set; }

        /// <summary>
        /// On Revenues include Extras
        /// </summary>
        public bool? RevExtras { get; set; }

        /// <summary>
        /// Available on Profile type
        /// 0   => Customers
        /// 1   => Travel Agnets
        /// 2   => Companies
        /// 3   => All
        /// </summary>
        public int? availableTo { get; set; }

        /// <summary>
        /// Calculate points if for Travel Agent profile
        /// </summary>
        public bool? IsTravelAgent { get; set; }

        /// <summary>
        /// Calculate points if for Group profile
        /// </summary>
        public bool? IsGroup { get; set; }

        /// <summary>
        /// Calculate points if for company profile
        /// </summary>
        public bool? IsCompany { get; set; }

        /// <summary>
        /// If profile downgrade class with class is from current
        /// </summary>
        public int? DownGradeClassId { get; set; }

        /// <summary>
        /// Comments
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// Make profile as vip if get this class
        /// </summary>
        public int VipId { get; set; }
    }
}
