using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace PmsDBModels.Protel.DTOs
{
    [Table("buch")]
    public class buchDTO
    {
        [Key]
        public int buchnr { get; set; } //(int, not null)

        public int anzahl { get; set; } //(int, not null)

        public DateTime datumvon { get; set; } //(datetime, not null)

        public DateTime datumbis { get; set; } //(datetime, not null)

        public int zimmernr { get; set; } //(int, not null)

        public int kundennr { get; set; } //(int, not null)

        public int buchstatus { get; set; } //(int, not null)

        public int resstatus { get; set; } //(int, not null)

        public int reschar { get; set; } //(int, not null)

        public int katnr { get; set; } //(int, not null)

        public int modus { get; set; } //(int, not null)

        public int mpehotel { get; set; } //(int, not null)

        public int preistypgr { get; set; } //(int, not null)

        public int preistyp { get; set; } //(int, not null)

        public decimal preis { get; set; } //(decimal(19,2), not null)

        public int chdbok { get; set; } //(int, not null)

        public int prchange { get; set; } //(int, not null)

        public int gruppennr { get; set; } //(int, not null)

        public int reisenr { get; set; } //(int, not null)

        public int firmennr { get; set; } //(int, not null)

        public int sourcenr { get; set; } //(int, not null)

        public int orgkatnr { get; set; } //(int, not null)

        public int anzerw { get; set; } //(int, not null)

        public int anzkin1 { get; set; } //(int, not null)

        public int anzkin2 { get; set; } //(int, not null)

        public int anzkin3 { get; set; } //(int, not null)

        public int anzkin4 { get; set; } //(int, not null)

        public int k1rech { get; set; } //(int, not null)

        public int k2rech { get; set; } //(int, not null)

        public int k3rech { get; set; } //(int, not null)

        public int k4rech { get; set; } //(int, not null)

        public int k5rech { get; set; } //(int, not null)

        public int k6rech { get; set; } //(int, not null)

        public int nr1 { get; set; } //(int, not null)

        public int nr2 { get; set; } //(int, not null)

        public int nr3 { get; set; } //(int, not null)

        public int nr4 { get; set; } //(int, not null)

        public int nr5 { get; set; } //(int, not null)

        public int nr6 { get; set; } //(int, not null)

        public string wg1 { get; set; } //(varchar(60), not null)

        public string wg2 { get; set; } //(varchar(60), not null)

        public string wg3 { get; set; } //(varchar(60), not null)

        public string wg4 { get; set; } //(varchar(60), not null)

        public string wg5 { get; set; } //(varchar(60), not null)

        public string wg6 { get; set; } //(varchar(60), not null)

        public int zart1 { get; set; } //(int, not null)

        public int zart2 { get; set; } //(int, not null)

        public int zart3 { get; set; } //(int, not null)

        public int zart4 { get; set; } //(int, not null)

        public int zart5 { get; set; } //(int, not null)

        public int zart6 { get; set; } //(int, not null)

        public DateTime gebbis { get; set; } //(datetime, not null)

        public int globbnr { get; set; } //(int, not null)

        public DateTime globdvon { get; set; } //(datetime, not null)

        public DateTime globdbis { get; set; } //(datetime, not null)

        public int rechnr1 { get; set; } //(int, not null)

        public int rechnr2 { get; set; } //(int, not null)

        public int rechnr3 { get; set; } //(int, not null)

        public int rechnr4 { get; set; } //(int, not null)

        public int rechnr5 { get; set; } //(int, not null)

        public int rechnr6 { get; set; } //(int, not null)

        public DateTime not1dat { get; set; } //(datetime, not null)

        public string not1txt { get; set; } //(varchar(150), not null)

        public DateTime not2dat { get; set; } //(datetime, not null)

        public string not2txt { get; set; } //(varchar(150), not null)

        public string anzeit { get; set; } //(varchar(5), not null)

        public string abzeit { get; set; } //(varchar(5), not null)

        public int zbett { get; set; } //(int, not null)

        public int kbett { get; set; } //(int, not null)

        public int rmodus { get; set; } //(int, not null)

        public int grpmaster { get; set; } //(int, not null)

        public int rechkp1 { get; set; } //(int, not null)

        public int rechkp2 { get; set; } //(int, not null)

        public int rechkp3 { get; set; } //(int, not null)

        public int rechkp4 { get; set; } //(int, not null)

        public int rechkp5 { get; set; } //(int, not null)

        public int rechkp6 { get; set; } //(int, not null)

        public string resuser { get; set; } //(varchar(50), not null)

        public int umzdurch { get; set; } //(int, not null)

        public DateTime datumopt { get; set; } //(datetime, not null)

        public decimal grundpreis { get; set; } //(decimal(19,2), not null)

        public DateTime resdatum { get; set; } //(datetime, not null)

        public int sharenr { get; set; } //(int, not null)

        public int rechformnr { get; set; } //(int, not null)

        public int market { get; set; } //(int, not null)

        public int source { get; set; } //(int, not null)

        public int waesche { get; set; } //(int, not null)

        public int resfest { get; set; } //(int, not null)

        public int kontinnr { get; set; } //(int, not null)

        public int leistacc { get; set; } //(int, not null)

        [Column("event")]
        public int eventId { get; set; } //(int, not null)

        public string string1 { get; set; } //(varchar(50), not null)

        public string string2 { get; set; } //(varchar(50), not null)

        public DateTime stornodat { get; set; } //(datetime, not null)

        public string stornotxt { get; set; } //(varchar(150), not null)

        public string stornousr { get; set; } //(varchar(10), not null)

        public string stornozei { get; set; } //(varchar(5), not null)

        public int cxl { get; set; } //(int, not null)

        public string cxlnr { get; set; } //(varchar(20), not null)

        public string sex { get; set; } //(varchar(5), not null)

        public int gender { get; set; } //(int, not null)

        public string voucher { get; set; } //(varchar(14), not null)

        public string arrtp { get; set; } //(varchar(30), not null)

        public string deptp { get; set; } //(varchar(30), not null)

        public string cctyp { get; set; } //(varchar(10), not null)

        public string ccexp { get; set; } //(varchar(5), not null)

        public string cc { get; set; } //(varchar(70), not null)

        public string ccchkdg { get; set; } //(varchar(4), not null)

        public string ccholder { get; set; } //(varchar(50), not null)

        public int safenr { get; set; } //(int, not null)

        public int hear { get; set; } //(int, not null)

        public int come { get; set; } //(int, not null)

        public int pickup { get; set; } //(int, not null)

        public int cino { get; set; } //(int, not null)

        public string cino1 { get; set; } //(varchar(20), not null)

        public int color { get; set; } //(int, not null)

        public int color1 { get; set; } //(int, not null)

        public int modified { get; set; } //(int, not null)

        public int oldresstat { get; set; } //(int, not null)

        public decimal orgpreis { get; set; } //(decimal(19,2), not null)

        public int taxexemp { get; set; } //(int, not null)

        public DateTime oldgldbis { get; set; } //(datetime, not null)
        public int value1 { get; set; } //(int, not null)

        public int value2 { get; set; } //(int, not null)

        public string usrstr1 { get; set; } //(varchar(20), not null)

        public string usrstr2 { get; set; } //(varchar(20), not null)

        public DateTime usrdate1 { get; set; } //(datetime, not null)

        public int discount { get; set; } //(int, not null)

        public decimal discval { get; set; } //(decimal(19,2), not null)

        public string promotion { get; set; } //(varchar(20), not null)

        public string booker { get; set; } //(varchar(254), not null)

        public string crsnumber { get; set; } //(varchar(50), not null)

        public int hidertonrc { get; set; } //(int, not null)

        public int mcdactive { get; set; } //(int, not null)

        public int ccomcode { get; set; } //(int, not null)

        public int gcomcode { get; set; } //(int, not null)

        public int tcomcode { get; set; } //(int, not null)

        public int scomcode { get; set; } //(int, not null)

        public int igmpreis { get; set; } //(int, not null)

        public string hqclient { get; set; } //(varchar(500), not null)

        public int hqbuchnr { get; set; } //(int, not null)

        public DateTime resdatumsql { get; set; } //(datetime, not null)

        public int channelnr { get; set; } //(int, not null)

        public string channeldata { get; set; } //(varchar(500), not null)

        public int channelrestorebuchnr { get; set; } //(int, not null)

        public string channelrestoredata { get; set; } //(varchar(500), not null)

        public int modifiedforecast { get; set; } //(int, not null)

        public int channelkatnr { get; set; } //(int, not null)

        public int modifiedmulti { get; set; } //(int, not null)

        public string xmlinfo { get; set; } //(varchar(500), not null)

        public string extinfo { get; set; } //(varchar(500), not null)

        public decimal oppcost { get; set; } //(decimal(19,2), not null)

        [Column("override")]
        public int overrideId { get; set; } //(int, not null)

        public int changeuser { get; set; } //(int, not null)
        
        public string idsnumber { get; set; } //(varchar(50), not null)
        
        public long cloudref { get; set; } //(bigint, not null)
       
        public int notdelete { get; set; } //(int, not null)
       
        public int iressource { get; set; } //(int, not null)
        
        public int suite { get; set; } //(int, not null)
        
        public int _del { get; set; } //(int, not null)
        
        //public int pp_xml_createreservation { get; set; } //(int, not null)
    }
}
