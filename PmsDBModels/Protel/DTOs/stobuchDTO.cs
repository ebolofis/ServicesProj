using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace PmsDBModels.Protel.DTOs
{
    [Table("stobuch")]
    public class stobuchDTO
    {
        public int mpehotel { get; set; } //(int, not null)

        public int anzahl { get; set; } //(int, not null)

        public DateTime datumvon { get; set; } //(datetime, not null)

        public DateTime datumbis { get; set; } //(datetime, not null)

        public string ziname { get; set; } //(varchar(20), not null)

        public int zimmernr { get; set; } //(int, not null)

        public string katname { get; set; } //(varchar(20), not null)

        public int katnr { get; set; } //(int, not null)

        public int kattyp { get; set; } //(int, not null)

        public int reschar { get; set; } //(int, not null)

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

        public int kundennr { get; set; } //(int, not null)

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

        public string stornozei { get; set; } //(varchar(5), not null)

        public DateTime stornodat { get; set; } //(datetime, not null)

        public string stornotxt { get; set; } //(varchar(150), not null)

        public string stornousr { get; set; } //(varchar(10), not null)

        public string cxl { get; set; } //(varchar(15), not null)

        public int cxlref { get; set; } //(int, not null)

        public string cxlnr { get; set; } //(varchar(20), not null)

        public int oldresstat { get; set; } //(int, not null)

        public int hear { get; set; } //(int, not null)

        public int come { get; set; } //(int, not null)

        public string string1 { get; set; } //(varchar(50), not null)

        public int gender { get; set; } //(int, not null)

        public DateTime not1dat { get; set; } //(datetime, not null)

        public string not1txt { get; set; } //(varchar(150), not null)

        public DateTime not2dat { get; set; } //(datetime, not null)

        public string not2txt { get; set; } //(varchar(150), not null)

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

        public int cctyp { get; set; } //(int, not null)

        public string cc { get; set; } //(varchar(70), not null)

        public string ccexp { get; set; } //(varchar(5), not null)

        public string ccholder { get; set; } //(varchar(50), not null)

        public decimal oppcost { get; set; } //(decimal(19,2), not null)

        [Column("override")]
        public int overrideId { get; set; } //(int, not null)
        
        public int hqbuchnr { get; set; } //(int, not null)
        
        public string idsnumber { get; set; } //(varchar(50), not null)
        
        public int iressource { get; set; } //(int, not null)
        
        public long cloudref { get; set; } //(bigint, not null)
        
        public int pp_xml_createreservation { get; set; } //(int, not null)
        
        public int suite { get; set; } //(int, not null)

    }
}
