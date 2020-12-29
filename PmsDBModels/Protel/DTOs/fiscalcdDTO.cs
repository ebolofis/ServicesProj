using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace PmsDBModels.Protel.DTOs
{
    [Table("fiscalcd")]
    public class fiscalcdDTO
    {
        [Column("ref")]
        [Key]
        public int id { get; set; } //(int, not null)
        
        public int mpehotel { get; set; } //(int, not null)
        
        public DateTime validfrom { get; set; } //(datetime, not null)
        
        public DateTime validto { get; set; } //(datetime, not null)
        
        public int counter { get; set; } //(int, not null)
        
        public string code { get; set; } //(varchar(10), not null)
        
        public string text { get; set; } //(varchar(40), not null)
        
        public int clxcode { get; set; } //(int, not null)
        
        public int clxcode2 { get; set; } //(int, not null)
        
        public int clx { get; set; } //(int, not null)
        
        public int eod { get; set; } //(int, not null)
        
        public int def { get; set; } //(int, not null)
        
        public int sin { get; set; } //(int, not null)
        
        public int form { get; set; } //(int, not null)
        
        public int extcounter { get; set; } //(int, not null)
        
        public int _del { get; set; } //(int, not null)

}
}
