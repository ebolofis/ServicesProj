using System;
using System.Collections.Generic;
using Dapper;
using System.Text;

namespace PmsDBModels.Protel.DTOs
{
    [Table("zahlart")]
    public class zahlartDTO
    {
        public string za { get; set; } //(varchar(10), not null)
        
        public string bez { get; set; } //(varchar(40), not null)
        
        [Key]
        public int zanr { get; set; } //(int, not null)
        
        public int hauptgrp { get; set; } //(int, not null)
        
        public int typ { get; set; } //(int, not null)
        
        public int armode { get; set; } //(int, not null)
        
        public int inkasse { get; set; } //(int, not null)
        
        public int hidden { get; set; } //(int, not null)
        
        public int noar { get; set; } //(int, not null)
        
        public int noarpay { get; set; } //(int, not null)
        
        public int arpayonly { get; set; } //(int, not null)
        
        public int eft { get; set; } //(int, not null)
        
        public int vatno { get; set; } //(int, not null)
        
        public int resstatus { get; set; } //(int, not null)
        
        public int inet { get; set; } //(int, not null)
        
        public int hideinv { get; set; } //(int, not null)
        
        public int foreign2 { get; set; } //(int, not null)
        
        public decimal kurs { get; set; } //(decimal(19,6), not null)
        
        public decimal komm2 { get; set; } //(decimal(19,3), not null)
        
        public int cc { get; set; } //(int, not null)
        
        public int statgrp { get; set; } //(int, not null)
        
        public int module { get; set; } //(int, not null)
        
        public int datevkto { get; set; } //(int, not null)
        
        public string ord1 { get; set; } //(varchar(25), not null)
        
        public string ord2 { get; set; } //(varchar(25), not null)
        
        public string ord3 { get; set; } //(varchar(25), not null)
        
        public string ord4 { get; set; } //(varchar(25), not null)
        
        public string fibukto { get; set; } //(varchar(20), not null)
        
        public string kst1 { get; set; } //(varchar(20), not null)
        
        public string kst2 { get; set; } //(varchar(20), not null)
        
        public string gvkonto { get; set; } //(varchar(20), not null)
        
        public string provision { get; set; } //(varchar(11), not null)
        
        public string flags { get; set; } //(varchar(20), not null)

    }
}
