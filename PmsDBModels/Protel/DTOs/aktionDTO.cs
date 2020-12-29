using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace PmsDBModels.Protel.DTOs
{
    [Table("aktion")]
    public class aktionDTO
    {
        [Key]
        public DateTime tstamp { get; set; } //(datetime, not null)
        
        public DateTime datum { get; set; } //(datetime, not null)
        
        public string zeit { get; set; } //(varchar(10), not null)
        
        public int sysstart { get; set; } //(int, not null)
        
        public int memusage { get; set; } //(int, not null)
        
        public int mpehotel { get; set; } //(int, not null)
        
        public string bediener { get; set; } //(varchar(40), not null)
        
        public string kasse { get; set; } //(varchar(30), not null)
        
        public string fkt { get; set; } //(varchar(50), not null)
        
        public string aktion { get; set; } //(varchar(50), not null)
        
        public string zimmernr { get; set; } //(varchar(20), not null)
        
        public int kdnr { get; set; } //(int, not null)
        
        public int station { get; set; } //(int, not null)
        
        public string gastname { get; set; } //(varchar(80), not null)
        
        public int buchnr { get; set; } //(int, not null)
        
        public int bktnr { get; set; } //(int, not null)
        
        public string text { get; set; } //(varchar(200), not null)
        
        public string xdebug { get; set; } //(varchar(50), not null)
    }
}
