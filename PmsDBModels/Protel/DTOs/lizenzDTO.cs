using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace PmsDBModels.Protel.DTOs
{
    [Table("lizenz")]
    public class lizenzDTO
    {
        [Key]
        public int mpehotel { get; set; } //(int, not null)
        
        public int mpegroup { get; set; } //(int, not null)
        
        public int mpelic { get; set; } //(int, not null)
        
        public int mpehq { get; set; } //(int, not null)
        
        public int mpehqcl { get; set; } //(int, not null)
        
        public int mpepool { get; set; } //(int, not null)
        
        public int demo { get; set; } //(int, not null)
        
        public int smartlic { get; set; } //(int, not null)
        
        public string smartopt { get; set; } //(varchar(250), not null)
        
        public int kdnr { get; set; } //(int, not null)
        
        public int lang { get; set; } //(int, not null)
        
        public int lizenz { get; set; } //(int, not null)
        
        public string hotel { get; set; } //(varchar(100), not null)

        [Column("short")]
        public string shortName { get; set; } //(varchar(100), not null)
        
        public string homepage { get; set; } //(varchar(200), not null)
        
        public string haendler { get; set; } //(varchar(100), not null)
        
        public string hotelno { get; set; } //(varchar(100), not null)
        
        public string hotelno2 { get; set; } //(varchar(100), not null)
        
        public string start { get; set; } //(varchar(8), not null)
        
        public string ablauf { get; set; } //(varchar(8), not null)
        
        public int zimmer { get; set; } //(int, not null)
        
        public int work_id { get; set; } //(int, not null)
        
        public int code { get; set; } //(int, not null)
        
        public int location { get; set; } //(int, not null)
        
        public int rtcolor { get; set; } //(int, not null)
        
        public int type { get; set; } //(int, not null)
        
        public int class0 { get; set; } //(int, not null)
        
        public int class1 { get; set; } //(int, not null)
        
        public int class2 { get; set; } //(int, not null)
        
        public int class3 { get; set; } //(int, not null)
        
        public int class4 { get; set; } //(int, not null)
        
        public int inet { get; set; } //(int, not null)
        
        public int _del { get; set; } //(int, not null)

    }
}
