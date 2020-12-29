using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace PmsDBModels.Protel.DTOs
{
    [Table("kunden")]
    public class kundenDTO
    {
        [Key]
        public int kdnr { get; set; } //(int, not null)

        public long cloudref { get; set; } //(bigint, not null)

        public int mpehotel { get; set; } //(int, not null)

        public int hotelkdnr { get; set; } //(int, not null)

        public string extref { get; set; } //(varchar(15), not null)

        public string member { get; set; } //(varchar(50), not null)

        public int outlsync { get; set; } //(int, not null)

        public DateTime outldate { get; set; } //(datetime, not null)

        public string passwd { get; set; } //(varchar(15), not null)

        public int typ { get; set; } //(int, not null)

        public string name1 { get; set; } //(varchar(80), not null)

        public string name2 { get; set; } //(varchar(80), not null)

        public string vorname { get; set; } //(varchar(50), not null)

        public string ehepartner { get; set; } //(varchar(80), not null)

        public DateTime ehegeb { get; set; } //(datetime, not null)

        public string strasse { get; set; } //(varchar(80), not null)

        public string strasse2 { get; set; } //(varchar(80), not null)

        public string strasse3 { get; set; } //(varchar(80), not null)

        public string plz { get; set; } //(varchar(17), not null)

        public string pfplz { get; set; } //(varchar(17), not null)

        public string postfach { get; set; } //(varchar(17), not null)

        public string ort { get; set; } //(varchar(50), not null)

        public string land { get; set; } //(varchar(80), not null)

        public int landkz { get; set; } //(int, not null)

        public int regionkz { get; set; } //(int, not null)

        public int gender { get; set; } //(int, not null)

        public string abteil { get; set; } //(varchar(80), not null)

        public string region { get; set; } //(varchar(80), not null)

        public int nat { get; set; } //(int, not null)

        public int marketing { get; set; } //(int, not null)

        public int sprache { get; set; } //(int, not null)

        public int vip { get; set; } //(int, not null)

        public DateTime gebdat { get; set; } //(datetime, not null)

        public string anrede { get; set; } //(varchar(40), not null)

        public string begr { get; set; } //(varchar(70), not null)

        public string telefonnr { get; set; } //(varchar(50), not null)

        public string funktel { get; set; } //(varchar(50), not null)

        public string email { get; set; } //(varchar(75), not null)

        public string twitter { get; set; } //(varchar(80), not null)

        public string homepage { get; set; } //(varchar(80), not null)

        public int cctypn { get; set; } //(int, not null)

        public string ccexp { get; set; } //(varchar(10), not null)

        public string cc { get; set; } //(varchar(35), not null)

        public string kfz { get; set; } //(varchar(30), not null)

        public string faxnr { get; set; } //(varchar(50), not null)

        public string telex { get; set; } //(varchar(50), not null)

        public string gdsident { get; set; } //(varchar(64), not null)

        public string resname { get; set; } //(varchar(80), not null)

        public string iata { get; set; } //(varchar(9), not null)

        public string contract { get; set; } //(varchar(20), not null)

        public string resvorn { get; set; } //(varchar(50), not null)

        public string resanr { get; set; } //(varchar(40), not null)

        public string restel { get; set; } //(varchar(100), not null)

        public string respanr { get; set; } //(varchar(70), not null)

        public string rechname { get; set; } //(varchar(80), not null)

        public string rechvorn { get; set; } //(varchar(50), not null)

        public string rechanr { get; set; } //(varchar(40), not null)

        public string rechtel { get; set; } //(varchar(100), not null)

        public string rechpanr { get; set; } //(varchar(70), not null)

        public string titel { get; set; } //(varchar(20), not null)

        public string firmenname { get; set; } //(varchar(50), not null)

        public string firmennam2 { get; set; } //(varchar(50), not null)

        public string beruf { get; set; } //(varchar(50), not null)

        public int prof { get; set; } //(int, not null)

        public int wunschzi { get; set; } //(int, not null)

        public string zimmerattr { get; set; } //(varchar(50), not null)

        public string gfeat { get; set; } //(varchar(50), not null)

        public int preiscode { get; set; } //(int, not null)

        public int comcode { get; set; } //(int, not null)

        public int comtax1 { get; set; } //(int, not null)

        public int comtax2 { get; set; } //(int, not null)

        public int deleted { get; set; } //(int, not null)

        public int kredit { get; set; } //(int, not null)

        public decimal kreditbet { get; set; } //(decimal(19,2), not null)

        public int intcredit { get; set; } //(int, not null)

        public decimal intcredita { get; set; } //(decimal(19,2), not null)

        public int statement { get; set; } //(int, not null)

        public int sonderp { get; set; } //(int, not null)

        public decimal sonderp1 { get; set; } //(decimal(7,2), not null)

        public decimal sonderp2 { get; set; } //(decimal(7,2), not null)

        public decimal sonderp3 { get; set; } //(decimal(7,2), not null)

        public decimal sonderp4 { get; set; } //(decimal(7,2), not null)

        public string bemerkung { get; set; } //(varchar(250), not null)

        public string bemrest { get; set; } //(varchar(250), not null)

        public int aufenth { get; set; } //(int, not null)

        public int naechte { get; set; } //(int, not null)

        public int noshows { get; set; } //(int, not null)

        public int stornos { get; set; } //(int, not null)

        public int aufenth_vj { get; set; } //(int, not null)

        public int naechte_vj { get; set; } //(int, not null)

        public int noshows_vj { get; set; } //(int, not null)

        public int stornos_vj { get; set; } //(int, not null)

        public DateTime letzterauf { get; set; } //(datetime, not null)

        public DateTime firststay { get; set; } //(datetime, not null)

        public DateTime erfasst { get; set; } //(datetime, not null)

        public string erfassttim { get; set; } //(varchar(10), not null)

        public string erfasstusr { get; set; } //(varchar(50), not null)

        public decimal letzterpr { get; set; } //(decimal(19,2), not null)

        public string letzteszi { get; set; } //(varchar(10), not null)

        public string fibudeb { get; set; } //(varchar(20), not null)

        public decimal logis { get; set; } //(decimal(19,2), not null)

        public decimal fb { get; set; } //(decimal(19,2), not null)

        public decimal extras { get; set; } //(decimal(19,2), not null)

        public decimal logis_vj { get; set; } //(decimal(19,2), not null)

        public decimal fb_vj { get; set; } //(decimal(19,2), not null)

        public decimal extras_vj { get; set; } //(decimal(19,2), not null)

        public int transfer { get; set; } //(int, not null)

        public int mailing { get; set; } //(int, not null)

        [Column("protected")]
        public int protectedId { get; set; } //(int, not null)
        
        public DateTime changed { get; set; } //(datetime, not null)
        
        public string changedby { get; set; } //(varchar(50), not null)
        
        public string changetime { get; set; } //(varchar(10), not null)
        
        public DateTime merged { get; set; } //(datetime, not null)
        
        public string mergetime { get; set; } //(varchar(10), not null)
        
        public int tosales { get; set; } //(int, not null)
        
        public string passnr { get; set; } //(varchar(30), not null)
        
        public string issued { get; set; } //(varchar(50), not null)
        
        public DateTime issuedate { get; set; } //(datetime, not null)
        
        public int discount { get; set; } //(int, not null)
        
        public int doctype { get; set; } //(int, not null)
        
        public DateTime docvalid { get; set; } //(datetime, not null)
        
        public string vatno { get; set; } //(varchar(30), not null)
        
        public string flags { get; set; } //(varchar(20), not null)
        
        public string afmno { get; set; } //(varchar(30), not null)
        
        public string gebort { get; set; } //(varchar(50), not null)
        
        public int gebland { get; set; } //(int, not null)
        
        public int mstatus { get; set; } //(int, not null)
        
        public int mahncode { get; set; } //(int, not null)
        
        public int masteracc { get; set; } //(int, not null)
        
        public int masterdeb { get; set; } //(int, not null)
        
        public int ismstdeb { get; set; } //(int, not null)
        
        public int extend { get; set; } //(int, not null)
        
        public DateTime spfrom1 { get; set; } //(datetime, not null)
        
        public DateTime spfrom2 { get; set; } //(datetime, not null)
        
        public DateTime spfrom3 { get; set; } //(datetime, not null)
        
        public DateTime spfrom4 { get; set; } //(datetime, not null)
        
        public DateTime spto1 { get; set; } //(datetime, not null)
        
        public DateTime spto2 { get; set; } //(datetime, not null)
        
        public DateTime spto3 { get; set; } //(datetime, not null)
        
        public DateTime spto4 { get; set; } //(datetime, not null)
        
        public decimal sp1p1 { get; set; } //(decimal(7,2), not null)
        
        public decimal sp2p1 { get; set; } //(decimal(7,2), not null)
        
        public decimal sp3p1 { get; set; } //(decimal(7,2), not null)
        
        public decimal sp4p1 { get; set; } //(decimal(7,2), not null)
        
        public decimal sp1p2 { get; set; } //(decimal(7,2), not null)
        
        public decimal sp2p2 { get; set; } //(decimal(7,2), not null)
        
        public decimal sp3p2 { get; set; } //(decimal(7,2), not null)
        
        public decimal sp4p2 { get; set; } //(decimal(7,2), not null)
        
        public decimal sp1p3 { get; set; } //(decimal(7,2), not null)
        
        public decimal sp2p3 { get; set; } //(decimal(7,2), not null)
        
        public decimal sp3p3 { get; set; } //(decimal(7,2), not null)
        
        public decimal sp4p3 { get; set; } //(decimal(7,2), not null)
        
        public decimal sp1p4 { get; set; } //(decimal(7,2), not null)
        
        public decimal sp2p4 { get; set; } //(decimal(7,2), not null)
        
        public decimal sp3p4 { get; set; } //(decimal(7,2), not null)
        
        public decimal sp4p4 { get; set; } //(decimal(7,2), not null)
        
        public string user00 { get; set; } //(varchar(20), not null)
        
        public string user01 { get; set; } //(varchar(20), not null)
        
        public int user02 { get; set; } //(int, not null)
        
        public int user03 { get; set; } //(int, not null)
        
        public int taxexemp { get; set; } //(int, not null)
        
        public int voidfee { get; set; } //(int, not null)
        
        public int depcode { get; set; } //(int, not null)
        
        public int laundry { get; set; } //(int, not null)
        
        public int posrcd { get; set; } //(int, not null)
        
        public int emptybfee { get; set; } //(int, not null)
        
        public string shortname { get; set; } //(varchar(80), not null)
        
        public string mastername { get; set; } //(varchar(80), not null)
        
        public string accountname { get; set; } //(varchar(80), not null)
        
        public string otapara { get; set; } //(varchar(51), not null)
        
        public int numcontacts { get; set; } //(int, not null)
        
        public int numaccounts { get; set; } //(int, not null)
        
        public int addcleansed { get; set; } //(int, not null)
        
        public string cpssrc { get; set; } //(varchar(30), not null)
        
        public string cpsid { get; set; } //(varchar(30), not null)
        
        public int _del { get; set; } //(int, not null)

    }
}
