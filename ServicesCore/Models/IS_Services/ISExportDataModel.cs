using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HitServicesCore.Models.IS_Services
{
    /// <summary>
    /// Export data Model
    /// </summary>
    public class ISExportDataModel : ISServiceGeneralModel
    {
        #region DB

        // <summary>
        /// CustomDB1 connection string 
        /// </summary>
        public string Custom1DB { get; set; } = "server = <SERVER>; user id = <USER>; password = <PASSWORD>; database = <DB>;";

        /// <summary>
        /// Transaction
        /// </summary>
        public string DBTimeout { get; set; } = "60";

        #endregion

        #region Sql Scripts

        public Dictionary<string, string> SqlParameters { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// Sql Script Path to execute
        /// </summary>
        public string SqlScript { get; set; }



        #endregion

        #region Export To File
        /// <summary>
        /// CultureInfo: ex: el-GR, en-us or null
        /// </summary>
        public string CultureInfo { get; set; } = "en-us";

        /// <summary>
        /// Formatter for every column. Key: the name of column, Value: format ex: yyyy-MM-dd HH:mm:ss (2018-12-31 23:10:20), F2 (1230.12), N2 (1,230.12) 
        /// </summary>
        public Dictionary<string, string> Formater { get; set; }

        /// <summary>
        /// TimeStampt's format (ex:yyyyMMddHHmm). Is replaces the &lt;TIMESTAMP&#62; into files paths
        /// </summary>
        public string TimeStamp { get; set; } = "yyyyMMddHHmmss";

        /// <summary>
        /// The exported  file will be sent to an FTP Server
        /// </summary>
        public bool ExportedFileToFTP { get; set; }

        #endregion

        #region Xml
        /// <summary>
        /// Path for the xml path. (if path contains &lt;TIMESTAMP&#62; then this is replased by current timespan )
        /// </summary>
        public string XmlFilePath { get; set; }//(if path contains <TIMESTAMP> then this is replased by current timespan )

        /// <summary>
        /// RootElement for the xml  (ex: Receipts)
        /// </summary>
        public string XmlRootElement { get; set; }

        /// <summary>
        /// Every-Line-Element for the xml(ex: Receipt)
        /// </summary>
        public string XmlElement { get; set; }

        /// <summary>
        /// The exported  file will be sent to an FTP Server
        /// </summary>
        public bool XmlFileToFTP { get; set; }
        #endregion

        #region Json
        /// <summary>
        /// Path for the json path. (if path contains &lt;TIMESTAMP&#62; then this is replased by current timespan )
        /// </summary>
        public string JsonFilePath { get; set; }//(if path contains <TIMESTAMP> then this is replased by current timespan )

        /// <summary>
        /// The exported  file will be sent to an FTP Server
        /// </summary>
        public bool JsonFileToFTP { get; set; }
        #endregion

        #region Csv
        /// <summary>
        /// Path for the csv path. (if path contains &lt;TIMESTAMP&#62; then this is replased by current timespan )
        /// </summary>
        public string CsvFilePath { get; set; }//(if path contains <TIMESTAMP> then this is replased by current timespan )

        /// <summary>
        /// If checked then the 1st line of the file will be header.
        /// </summary>
        public bool? CsvFileHeader { get; set; } = true;

        /// <summary>
        /// Delimeter (ex: ;,- or tab)
        /// </summary>
        public string CsvDelimenter { get; set; } = ";";

        /// <summary>
        /// The exported  file will be sent to an FTP Server
        /// </summary>
        public bool CsvFileToFTP { get; set; }

        #endregion

        #region Fixed lenght
        /// <summary>
        /// If checked then the 1st line of the file will be header.
        /// </summary>
        public bool? FixedLenghtFileHeader { get; set; } = true;

        /// <summary>
        /// If checked then columns are aligned to the right.
        /// </summary>
        public bool? FixedLenghtAlignRight { get; set; } = true;

        /// <summary>
        /// Lengths for every column (ex: 10,20,8,23)
        /// </summary>
        public List<int?> FixedLengths { get; set; }

        /// <summary>
        /// Path for the FixedLenght path. (if path contains &lt;TIMESTAMP&#62; then this is replased by current timespan )
        /// </summary>
        public string FixedLenghtFilePath { get; set; }//(if path contains <TIMESTAMP> then this is replased by current timespan )

        /// <summary>
        /// The exported  file will be sent to an FTP Server
        /// </summary>
        public bool FixedLenghtFileToFTP { get; set; }

        #endregion

        #region Html

        /// <summary>
        /// Path for the Html path. (if path contains &lt;TIMESTAMP&#62; then this is replased by current timespan )
        /// </summary>
        public string HtmlFilePath { get; set; }//(if path contains <TIMESTAMP> then this is replased by current timespan )

        /// <summary>
        /// If checked then the table will contain header.
        /// </summary>
        public bool? HtmlHeader { get; set; } = true;

        /// <summary>
        /// Html's title
        /// </summary>
        public string HtmlTitle { get; set; }

        /// <summary>
        /// css for html
        /// </summary>
        public bool HtmlSortRows { get; set; } = true;


        /// <summary>
        /// css for html
        /// </summary>
        public string Htmlcss { get; set; } = @"
body{
    padding: 0; 
    border: 0; 
    margin: 0;
}
#hittable {
    font-family:  ""Trebuchet MS"",Arial, Helvetica, sans-serif;
    border - collapse: collapse;
    width: 100 %;
    padding: 0; 
    margin: 0;
   }

#hittable td, #hittable th {
        border: 1px solid #ddd;
        padding: 8px;
  }

#hittable tr:nth-child(even){background-color: #f2f2f2;}

#hittable tr:hover {background-color: #ddd;}

#hittable th {
    padding-top: 12px;
    padding-bottom: 12px;
    text-align: left;
    background-color: #4CAF50;
    color: white;
}";

        /// <summary>
        /// The exported  file will be sent to an FTP Server
        /// </summary>
        public bool HtmlFileToFTP { get; set; }

        #endregion

        #region Api client
        /// <summary>
        /// api url to call
        /// </summary>
        public string RestServerUrl { get; set; }

        /// <summary>
        /// Authentication Header . Format: Username:Password  
        /// </summary>
        public string RestServerAuthenticationHeader { get; set; }

        /// <summary>
        /// Authentication Type  (Basic or OAuth2)
        /// </summary>
        public string RestServerAuthenticationType { get; set; } = "Basic";

        /// <summary>
        /// HttpMethod: Get,Post  
        /// </summary>
        public string RestServerHttpMethod { get; set; }

        /// <summary>
        /// mediaType: application/json or application/xml 
        /// </summary>
        public string RestServerMediaType { get; set; }

        /// <summary>
        /// CustomHeaders the Rest Server may require
        /// </summary>
        public Dictionary<string, string> RestServerCustomHeaders { get; set; }

        #endregion

        #region Pdf

        /// <summary>
        /// Path for the Pdf path. (if path contains &lt;TIMESTAMP&#62; then this is replased by current timespan )
        /// </summary>
        public string PdfFilePath { get; set; }//(if path contains <TIMESTAMP> then this is replased by current timespan )

        /// <summary>
        /// Html's title
        /// </summary>
        public string PdfTitle { get; set; }

        /// <summary>
        /// css for html
        /// </summary>
        public string Pdfcss { get; set; }

        /// <summary>
        /// The exported  file will be sent to an FTP Server
        /// </summary>
        public bool PdfFileToFTP { get; set; }
        #endregion

        #region Ftp
        /// <summary>
        /// Ftp Server
        /// </summary>
        public string FtpServer { get; set; }

        /// <summary>
        /// Ftp Port
        /// </summary>
        public int FtpPort { get; set; }

        /// <summary>
        /// Ftp Username
        /// </summary>
        public string FtpUsername { get; set; }

        /// <summary>
        /// Ftp Password
        /// </summary>
        public string FtpPassword { get; set; }

        /// <summary>
        /// Ftp Encryption
        /// </summary>
        public bool FtpConnectionEncryption { get; set; }


        /// <summary>
        /// 0=> None
        /// 1=> Default
        /// </summary>
        public int FTPSslProtocols { get; set; } = 0;

        /// <summary>
        /// 0=> None
        /// 1=> Default
        /// </summary>
        public int FTPEncryptionMode { get; set; } = 0;

        #endregion
    }
}
