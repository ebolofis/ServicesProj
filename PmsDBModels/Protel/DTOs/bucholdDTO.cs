using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace PmsDBModels.Protel.DTOs
{
    [Table("buchold")]
    public class bucholdDTO
    {
        public int mpehotel { get; set; } //(int, not null)
        
        public DateTime datumvon { get; set; } //(datetime, not null)
        
        public DateTime datumbis { get; set; } //(datetime, not null)
        
        public string ziname { get; set; } //(varchar(20), not null)
        
        public int zimmernr { get; set; } //(int, not null)
        
        public string katname { get; set; } //(varchar(20), not null)
        
        public int katnr { get; set; } //(int, not null)
        
        public int kattyp { get; set; } //(int, not null)
        
        public int orgkatnr { get; set; } //(int, not null)
        
        public string ptypname { get; set; } //(varchar(15), not null)
        
        public int preistypgr { get; set; } //(int, not null)
        
        public int preistyp { get; set; } //(int, not null)
        
        public int wkz { get; set; } //(int, not null)
        
        public decimal preis { get; set; } //(decimal(19,2), not null)
        
        public int anzerw { get; set; } //(int, not null)
        
        public int anzkin1 { get; set; } //(int, not null)
        
        public int anzkin2 { get; set; } //(int, not null)
        
        public int anzkin3 { get; set; } //(int, not null)
        
        public int anzkin4 { get; set; } //(int, not null)
        
        public int zbett { get; set; } //(int, not null)
        
        public int kbett { get; set; } //(int, not null)
        
        public string markname { get; set; } //(varchar(30), not null)
        
        public int market { get; set; } //(int, not null)
        
        public string sourname { get; set; } //(varchar(30), not null)
        
        public int source { get; set; } //(int, not null)
        
        public DateTime resdat { get; set; } //(datetime, not null)
        
        public string resuser { get; set; } //(varchar(50), not null)
        
        public decimal logis { get; set; } //(decimal(19,2), not null)
        
        public decimal fb { get; set; } //(decimal(19,2), not null)
        
        public decimal extras { get; set; } //(decimal(19,2), not null)
        
        public int kundennr { get; set; } //(int, not null)
        
        public int knatcode { get; set; } //(int, not null)
        
        public int kregion { get; set; } //(int, not null)
        
        public int firmennr { get; set; } //(int, not null)
        
        public int gruppennr { get; set; } //(int, not null)
        
        public int reisenr { get; set; } //(int, not null)
        
        public int sourcenr { get; set; } //(int, not null)
        
        public string kname { get; set; } //(varchar(80), not null)
        
        public string fname { get; set; } //(varchar(80), not null)
        
        public string gname { get; set; } //(varchar(80), not null)
        
        public string rname { get; set; } //(varchar(80), not null)
        
        public string sname { get; set; } //(varchar(80), not null)
        
        [Key]
        public int buchnr { get; set; } //(int, not null)
        
        public int grpmaster { get; set; } //(int, not null)
        
        public int sharenr { get; set; } //(int, not null)
        
        public string kontname { get; set; } //(varchar(50), not null)
        
        public int kontinnr { get; set; } //(int, not null)
        
        public string sex { get; set; } //(varchar(5), not null)
        
        public string hear { get; set; } //(varchar(11), not null)
        
        public int hearnr { get; set; } //(int, not null)
        
        public string come { get; set; } //(varchar(11), not null)
        
        public int comenr { get; set; } //(int, not null)
        
        public int k1rech { get; set; } //(int, not null)
        
        public int k2rech { get; set; } //(int, not null)
        
        public int k3rech { get; set; } //(int, not null)
        
        public int k4rech { get; set; } //(int, not null)
        
        public int k5rech { get; set; } //(int, not null)
        
        public int k6rech { get; set; } //(int, not null)
        
        public int zart1 { get; set; } //(int, not null)
        
        public int zart2 { get; set; } //(int, not null)
        
        public int zart3 { get; set; } //(int, not null)
        
        public int zart4 { get; set; } //(int, not null)
        
        public int zart5 { get; set; } //(int, not null)
        
        public int zart6 { get; set; } //(int, not null)
        
        public int hqbuchnr { get; set; } //(int, not null)
        
        public int oldresstat { get; set; } //(int, not null)
        
        public DateTime oldgldbis { get; set; } //(datetime, not null)
        
        public string resmove { get; set; } //(varchar(50), not null)
        
        public string string1 { get; set; } //(varchar(50), not null)
        
        public string hisremark { get; set; } //(varchar(250), not null)
        
        public string arrtp { get; set; } //(varchar(30), not null)
        
        public string deptp { get; set; } //(varchar(30), not null)
        
        public int gender { get; set; } //(int, not null)
        
        public DateTime not1dat { get; set; } //(datetime, not null)
        
        public string not1txt { get; set; } //(varchar(150), not null)
        
        public DateTime not2dat { get; set; } //(datetime, not null)
        
        public string not2txt { get; set; } //(varchar(150), not null)
        
        public string user00 { get; set; } //(varchar(20), not null)
        
        public int value1 { get; set; } //(int, not null)
        
        public int value2 { get; set; } //(int, not null)
        
        public string usrstr1 { get; set; } //(varchar(20), not null)
        
        public string usrstr2 { get; set; } //(varchar(20), not null)
        
        public int cino { get; set; } //(int, not null)
        
        public string cino1 { get; set; } //(varchar(20), not null)
        
        public string crsnumber { get; set; } //(varchar(50), not null)
        
        public int mcdactive { get; set; } //(int, not null)
        
        public int ccomcode { get; set; } //(int, not null)
        
        public int gcomcode { get; set; } //(int, not null)
        
        public int tcomcode { get; set; } //(int, not null)
        
        public int scomcode { get; set; } //(int, not null)
        
        public int pickup { get; set; } //(int, not null)
        
        public decimal oppcost { get; set; } //(decimal(19,2), not null)

        [Column("override")]
        public int overrideId { get; set; } //(int, not null)
        
        public string idsnumber { get; set; } //(varchar(50), not null)
        
        public int iressource { get; set; } //(int, not null)
        
        public long cloudref { get; set; } //(bigint, not null)
        
        //public int pp_xml_createreservation { get; set; } //(int, not null)
        
        public int suite { get; set; } //(int, not null)

    }
}
