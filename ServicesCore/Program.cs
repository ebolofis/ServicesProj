using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.WindowsServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;

namespace HitServicesCore
{
    public class Program
    {
        public static string CurrentPath { get; set; }
        public static string AppName { get; set; }

        public static IConfiguration Configuration { get; set; }

        public static void Main(string[] args)
        {
            CurrentPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            System.IO.Directory.SetCurrentDirectory(CurrentPath);

            AppName = Assembly.GetEntryAssembly().GetName().Name;

            var isService = !(Debugger.IsAttached || args.Contains("--console"));
            var webHostArgs = args.Where(arg => arg != "--console").ToArray();

            var ps = new List<string>() { CurrentPath, "Config", "NLog.config" };
            var logpath = Path.GetFullPath(Path.Combine(ps.ToArray()));
            var logger = NLog.Web.NLogBuilder.ConfigureNLog(logpath).GetCurrentClassLogger();

            try
            {
                //CreateHostBuilder(args).Build().Run();

                ConfigurationBuilder();
                var webBuilder = CreateWebHostBuilder(webHostArgs).Build();

                IWebHostEnvironment env = (IWebHostEnvironment)webBuilder.Services.GetService(typeof(IWebHostEnvironment));

                StartLogging(logger, env);

                if (isService)
                {
                    webBuilder.RunAsService();
                }
                else
                {
                    webBuilder.Run();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(" >>>---->>  ERROR (for more info see log files): " + ex.Message);
                //NLog: catch setup errors
                logger.Error(Convert.ToString(ex));
            }
            finally
            {
                logger.Warn($" ======== {AppName} Stopping ======== ");
                logger.Warn("");
                LogManager.Shutdown();
            }

            
        }

        private static void StartLogging(Logger logger, IWebHostEnvironment env)
        {
            Console.WriteLine("Starting Logger...");
            logger.Info("");
            logger.Info("");
            logger.Info("*****************************************");
            logger.Info("*                                       *");
            logger.Info($"*  {AppName}  Started                   ");
            logger.Info("*                                       *");
            logger.Info("*****************************************");
            logger.Info("");
            System.Diagnostics.Debug.WriteLine("Application Started");
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            logger.Info("Version: " + fvi.FileVersion);
            logger.Info("Urls: " + Configuration["urls"]);
            Console.WriteLine("Urls: " + Configuration["urls"]);
            logger.Info("Environment: " + env.EnvironmentName);
            logger.Info("Current Path: " + CurrentPath);
            logger.Info("");
        }


        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>

          WebHost.CreateDefaultBuilder(args)
               //.ConfigureServices(services => services.AddAutofac())
              // .UseServiceProviderFactory(new AutofacServiceProviderFactory())
              .UseStartup<Startup>()
              .ConfigureLogging(logging =>
              {
                  logging.ClearProviders();
                  logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
              })
              .ConfigureKestrel(serverOptions =>
              {
                  Console.WriteLine("Configuring Kestrel...");
              //    serverOptions.ConfigureHttpsDefaults(listenOptions =>
              //    {

              //        // certificate is an X509Certificate2
              //        string outputCertificateFile = Configuration["CertificateFile"];
              //        string outputCertificatePassword = Configuration["CertificatePassword"];
              //        if (!string.IsNullOrWhiteSpace(outputCertificateFile))
              //        {
              //            Console.WriteLine("Loading Certificate...");
              //            var cerPath = Path.GetFullPath(Path.Combine(CurrentPath, "WebConfig", outputCertificateFile));
              //            if (!new FileInfo(cerPath).Exists) throw new Exception($"Certificate {outputCertificateFile} not found");
              //            var tlsCertificate = new X509Certificate2(cerPath, outputCertificatePassword);
              //            listenOptions.ServerCertificate = tlsCertificate;
              //        }
              //    });
              })
             .UseConfiguration(Configuration) //<--- get port from config
             .UseNLog(); // NLog: setup NLog for Dependency injection
                         //   .UseWebRoot(Path.Combine(AppContext.BaseDirectory, "wwwroot")); 


        /// <summary>
        /// Build Configuration
        /// </summary>
        private static void ConfigurationBuilder()
        {
            Console.WriteLine("Building Configuration...");
            var ps1 = new List<string>() { CurrentPath, "Config", "appsettings.json" }; // Config/appsettings.json
//            var ps2 = new List<string>() { CurrentPath, "Config", "HitServiceCore.json" }; // Config/settings.json
            var appsettingspath = Path.GetFullPath(Path.Combine(ps1.ToArray()));
            //            var settingsspath = Path.GetFullPath(Path.Combine(ps2.ToArray()));
            var builder = new ConfigurationBuilder()
                     .SetBasePath(CurrentPath)
                     .AddJsonFile(appsettingspath, optional: false, reloadOnChange: true);
//                     .AddJsonFile(settingsspath, optional: false, reloadOnChange: true);

            Configuration = builder.Build();

        }

    //    public static IHostBuilder CreateHostBuilder(string[] args) =>
    //        Host.CreateDefaultBuilder(args)
    //            .ConfigureWebHostDefaults(webBuilder =>
    //            {
    //                webBuilder.UseStartup<Startup>();
    //            });
    }
}