using FluentFTP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Authentication;
using System.Text;

namespace HitHelpersNetCore.Helpers
{
    /// <summary>
    /// Upload or Download a file to an FTP Server
    /// </summary>
    public class FtpClientHelper : IDisposable
    {
        public string Server { get; set; }
        public int Port { get; set; } = 41;
        public string Username { get; set; }
        public string Password { get; set; }
        public bool FtpConnectionEncryption { get; set; }

        /// <summary>
        /// 0=> None
        /// 1=> Default
        /// </summary>
        public SslProtocols FTPSslProtocols { get; set; }

        /// <summary>
        /// None = 0,
        //  Implicit = 1,
        //  Explicit = 2
        /// </summary>
        public FtpEncryptionMode FTPEncryptionMode { get; set; }

        FtpClient client;

        public FtpClientHelper()
        {

        }


        /// <summary>
        /// Connect to FTP server.
        /// </summary>
        /// <returns></returns>
        public void Connect(string Server, int Port, string Username, string Password, bool FtpConnectionEncryption = false, int FTPSslProtocols = 0, int FTPEncryptionMode = 0)
        {
            this.Server = Server;
            this.Port = Port;
            this.Username = Username;
            this.Password = Password;
            //encryption
            this.FtpConnectionEncryption = FtpConnectionEncryption;
            if (FTPSslProtocols == 0)
                this.FTPSslProtocols = SslProtocols.None;
            else
                this.FTPSslProtocols = SslProtocols.Default;

            this.FTPEncryptionMode = (FtpEncryptionMode)FTPEncryptionMode;

            client = new FtpClient(this.Server);
            client.Port = this.Port;
            client.Credentials = new NetworkCredential(this.Username, this.Password);

            if (this.FtpConnectionEncryption)
            {
                client.SslProtocols = this.FTPSslProtocols;
                client.EncryptionMode = this.FTPEncryptionMode;
                client.DataConnectionType = FtpDataConnectionType.AutoPassive;
                client.DataConnectionEncryption = true;
                client.EnableThreadSafeDataConnections = false;
            }


            // begin connecting to the server
            client.Connect();


        }

        /// <summary>
        /// Disconnect from FTP server
        /// </summary>
        public void Disconnect()
        {
            if (client != null) client.Disconnect();
        }


        /// <summary>
        /// Download a file from ftpsource to localtarget.  
        /// Returns true if succeeded, false if failed or file does not exist. Exceptions are thrown for critical errors.
        /// </summary>
        /// <param name="ftpsource">full ftp path</param>
        /// <param name="localtarget">full local path</param>
        //public bool DownloadFile(string ftpsource, string localtarget, IProgress<double> progress = null)
        //{
        //    if (client == null) throw new Exception("Connect to an FTP server first.");
        //    client.RetryAttempts = 4;
        //    return client.DownloadFile(localtarget, ftpsource, overwrite: true, verifyOptions: FtpVerify.Retry, progress: progress);
        //}
        public FtpStatus DownloadFile(string ftpsource, string localtarget, Action<FtpProgress> progress = null)
        {
            if (client == null) throw new Exception("Connect to an FTP server first.");
            client.RetryAttempts = 4;
            return client.DownloadFile(localtarget, ftpsource, FtpLocalExists.Overwrite, verifyOptions: FtpVerify.Retry, progress: progress);
        }

        /// <summary>
        /// Download a file as stream from ftpsource.  
        /// Returns true if succeeded, false if failed or file does not exist. Exceptions are thrown for critical errors.
        /// </summary>
        /// <param name="ftpsource">full ftp path</param>
        public bool Download(string ftpsource, out Stream outStream, IProgress<double> progress = null)
        {
            if (client == null) throw new Exception("Connect to an FTP server first.");
            outStream = null;
            client.RetryAttempts = 4;
            return client.Download(outStream, ftpsource);
        }


        /// <summary>
        /// Upload a file from localpath to ftpsource.  
        /// Returns true if succeeded, false if failed or file does not exist. Exceptions are thrown for critical errors.
        /// </summary>
        /// <param name="localpath">full local path</param>
        /// <param name="ftpsource">full ftp path</param>
        //public bool UploadFile(string localpath, string ftpsource, IProgress<double> progress = null)
        //{
        //    if (client == null) throw new Exception("Connect to an FTP server first.");
        //    client.RetryAttempts = 3;
        //    return client.UploadFile(localpath, ftpsource, existsMode: FtpExists.Overwrite, verifyOptions: FtpVerify.Retry, progress: progress);
        //}
        public FtpStatus UploadFile(string localpath, string ftpsource, Action<FtpProgress> progress = null)
        {
            if (client == null) throw new Exception("Connect to an FTP server first.");
            client.RetryAttempts = 3;
            return client.UploadFile(localpath, ftpsource, FtpRemoteExists.Overwrite, verifyOptions: FtpVerify.Retry, progress: progress);
        }


        /// <summary>
        /// Upload a file as stream to ftpsource. 
        /// Returns true if succeeded, false if failed or file does not exist. Exceptions are thrown for critical errors.
        /// </summary>
        /// <param name="ftpsource">full ftp path</param>
        //public bool Upload(Stream stream, string ftpsource, IProgress<double> progress = null)
        //{
        //    if (client == null) throw new Exception("Connect to an FTP server first.");
        //    client.RetryAttempts = 3;
        //    return client.Upload(stream, ftpsource);
        //}
        public FtpStatus Upload(Stream stream, string ftpsource, Action<FtpProgress> progress = null)
        {
            if (client == null) throw new Exception("Connect to an FTP server first.");
            client.RetryAttempts = 3;
            return client.Upload(stream, ftpsource);
        }

        /// <summary>
        /// Delete a file to FTP
        /// </summary>
        /// <param name="ftpPath">full file's path to delete</param>
        public void DeleteFile(string ftpPath)
        {
            if (client == null) throw new Exception("Connect to an FTP server first.");
            client.DeleteFile(ftpPath);
        }

        /// <summary>
        /// Check if file exists in the server. Return true if it exists. 
        /// </summary>
        /// <param name="ftpPath">full file's path</param>
        public bool FileExists(string ftpPath)
        {
            if (client == null) throw new Exception("Connect to an FTP server first.");
            return client.FileExists(ftpPath);
        }



        public void Dispose()
        {
            if (client != null)
                client.Dispose();
            client = null;
        }

        /// <summary>
        /// Distractor
        /// </summary>
        ~FtpClientHelper()
        {
            Dispose();
        }
    }
}
