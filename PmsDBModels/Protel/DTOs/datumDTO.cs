using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace PmsDBModels.Protel.DTOs
{
    [Table("datum")]
    public class datumDTO
    {
        [Key]
        public int mpehotel { get; set; } //(int, not null)
        
        public int protdatum { get; set; } //(int, not null)
        
        public DateTime pdate { get; set; } //(datetime, not null)
        
        public int dummyj { get; set; } //(int, not null)
        
        public int day { get; set; } //(int, not null)
        
        public int month { get; set; } //(int, not null)
        
        public int year { get; set; } //(int, not null)
    }

}
