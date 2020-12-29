using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace PmsDBModels.Protel.DTOs
{
    [Table("leist")]
    public class leistDTO
    {
        public int buchnr { get; set; } //(int, not null)
        
        public int kundennr { get; set; } //(int, not null)
        
        public int station { get; set; } //(int, not null)
        
        public int ifc { get; set; } //(int, not null)

        [Column("ref")]
        [Key]
        public int id { get; set; } //(int, not null)
        
        public int reginvref { get; set; } //(int, not null)
        
        public int origin { get; set; } //(int, not null)
        
        public int grpref { get; set; } //(int, not null)
        
        public string grptext { get; set; } //(varchar(40), not null)
        
        public string grpztext { get; set; } //(varchar(40), not null)
        
        public int wkz { get; set; } //(int, not null)
        
        public int localcurr { get; set; } //(int, not null)
        
        public int tan { get; set; } //(int, not null)
        
        public DateTime datum { get; set; } //(datetime, not null)
        
        public DateTime rdatum { get; set; } //(datetime, not null)
        
        public string uhrzeit { get; set; } //(varchar(10), not null)
        
        public string bediener { get; set; } //(varchar(25), not null)
        
        public string umbtext { get; set; } //(varchar(50), not null)
        
        public int kasse { get; set; } //(int, not null)
        
        public decimal epreis { get; set; } //(decimal(19,2), not null)
        
        public decimal wpreis { get; set; } //(decimal(19,2), not null)
        
        public decimal r2preis { get; set; } //(decimal(19,2), not null)
        
        public decimal sintaxed { get; set; } //(decimal(19,2), not null)
        
        public int sininvno { get; set; } //(int, not null)
        
        public int anzahl { get; set; } //(int, not null)
        
        public string text { get; set; } //(varchar(40), not null)
        
        public string zustext { get; set; } //(varchar(40), not null)
        
        public string telno { get; set; } //(varchar(50), not null)
        
        public string zimmer { get; set; } //(varchar(10), not null)
        
        public string gast { get; set; } //(varchar(30), not null)
        
        public int rechnung { get; set; } //(int, not null)
        
        public int arout { get; set; } //(int, not null)
        
        public int arinvoice { get; set; } //(int, not null)
        
        public decimal mwstsatz { get; set; } //(decimal(19,4), not null)
        
        public int vatno { get; set; } //(int, not null)
        
        public decimal tax1 { get; set; } //(decimal(10,6), not null)
        
        public decimal tax2 { get; set; } //(decimal(10,6), not null)
        
        public decimal tax3 { get; set; } //(decimal(10,6), not null)
        
        public int taxexcode { get; set; } //(int, not null)
        
        public int taxexmode { get; set; } //(int, not null)
        
        public int taxedtaa { get; set; } //(int, not null)
        
        public int taxedref { get; set; } //(int, not null)
        
        public int rkz { get; set; } //(int, not null)
        
        public string cc { get; set; } //(varchar(70), not null)
        
        public string cc_holder { get; set; } //(varchar(30), not null)
        
        public string cc_valid { get; set; } //(varchar(5), not null)
        
        public decimal kurs { get; set; } //(decimal(19,6), not null)
        
        public int ukto { get; set; } //(int, not null)
        
        public int article { get; set; } //(int, not null)
        
        public int postdetail { get; set; } //(int, not null)
        
        public int imkasab { get; set; } //(int, not null)
        
        public int beleg { get; set; } //(int, not null)
        
        public int kaskz { get; set; } //(int, not null)
        
        public int hotkto { get; set; } //(int, not null)
        
        public int inhist { get; set; } //(int, not null)
        
        public int deposit { get; set; } //(int, not null)
        
        public DateTime deposituse { get; set; } //(datetime, not null)
        
        public int infibu { get; set; } //(int, not null)
        
        public int fiburef { get; set; } //(int, not null)
        
        public int kasbuch { get; set; } //(int, not null)
        
        public int noncom { get; set; } //(int, not null)
        
        public string voidreason { get; set; } //(varchar(250), not null)
        
        public int voidref { get; set; } //(int, not null)
        
        public int voidinv { get; set; } //(int, not null)
        
        public int source { get; set; } //(int, not null)
        
        public int market { get; set; } //(int, not null)
        
        public int discount { get; set; } //(int, not null)
        
        public decimal discountp { get; set; } //(decimal(19,2), not null)
        
        public int sorq { get; set; } //(int, not null)
        
        public int poq { get; set; } //(int, not null)
        
        public int voucher { get; set; } //(int, not null)
        
        public int withdrawl { get; set; } //(int, not null)
        
        public int taxexemp { get; set; } //(int, not null)
        
        public int package { get; set; } //(int, not null)
        
        public int orgtaa { get; set; } //(int, not null)
        
        public int stable { get; set; } //(int, not null)
        
        public int splitref { get; set; } //(int, not null)
        
        public string string1 { get; set; } //(varchar(50), not null)
        
        public string user1 { get; set; } //(varchar(50), not null)
        
        public string user2 { get; set; } //(varchar(50), not null)
        
        public string eftreceipt { get; set; } //(varchar(50), not null)
        
        public string eftauthcd { get; set; } //(varchar(30), not null)
        
        public string efttrack { get; set; } //(varchar(250), not null)
        
        public string eftinvrec { get; set; } //(varchar(2048), not null)
        
        public string eftrec1 { get; set; } //(varchar(1024), not null)
        
        public string eftrec2 { get; set; } //(varchar(1024), not null)
        
        public string ccenc { get; set; } //(varchar(70), not null)
        
        public string ifcinfo { get; set; } //(varchar(200), not null)
        
        public int b_werk { get; set; } //(int, not null)
        
        public int b_merk { get; set; } //(int, not null)
        
        public int b_kst { get; set; } //(int, not null)
        
        public string b_ukto { get; set; } //(varchar(8), not null)
        
        public string b_kstart { get; set; } //(varchar(7), not null)
        
        public int b_anz { get; set; } //(int, not null)
        
        public int b_pnr { get; set; } //(int, not null)
        
        public int _del { get; set; } //(int, not null)
    }
}
