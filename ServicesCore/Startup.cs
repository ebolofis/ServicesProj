using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using HitServicesCore.Helpers;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Reflection;
using HitHelpersNetCore.Helpers;
using HitServicesCore.Models;
using System.Text.Json;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using PmsDBModels.Classes;
using Hangfire;
using HitServicesCore.Filters;
using HitHelpersNetCore.Models;
using Hangfire.MemoryStorage;
using HitServicesCore.Models.SharedModels;
using HitCustomAnnotations.Interfaces;
using HitHelpersNetCore.Interfaces;
using Hangfire.Common;
using System.Runtime.Remoting;
using AutoMapper;
using Hangfire.Logging;
using HitHelpersNetCore.Classes;
using System.Threading;

//using NLog;

namespace HitServicesCore
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services
                .AddRazorPages()
                .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
            
            services.AddControllersWithViews().AddRazorRuntimeCompilation();

            services.AddMvc(options => options.EnableEndpointRouting = false)
                .ConfigureApplicationPartManager(ConfigureApplicationParts)
                .SetCompatibilityVersion(CompatibilityVersion.Latest)
                .AddJsonOptions(o =>
                {
                    o.JsonSerializerOptions.PropertyNamingPolicy = null;
                    o.JsonSerializerOptions.DictionaryKeyPolicy = null;
                });

            services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );
            services.AddSingleton<DIHelper>();

            SmtpModel mod = new SmtpModel();
            //Add system info model
            SystemInfo sysInf = InitializeSystemInfo();
            services.AddSingleton<SystemInfo>(sysInf);

            //Add all informations about plugins
            services.AddSingleton<List<PlugInDescriptors>>();

            //All Plugins and Main project configurations 
            services.AddSingleton<List<MainConfigurationModel>>();

            //List of all available services to run under Hang Fire
            services.AddSingleton<List<SchedulerServiceModel>>();

            //Class for logins users
            services.AddSingleton<LoginsUsers>();

            //Add class for adding plugins
            services.AddSingleton<PluginsMvcLoader>();

            services.AddScoped<EmailHelper>();

            //Adds new class for pms connections and models

            //services.AddSingleton<PmsConnection>(InitializePmsconnection());

            //Add class to add plugin annotated classes to container
            services.AddSingleton<PlugInAnnotationedClassesHelper>();

            //Add classes from plugins to DI
            PlugInAnnotationedClassesHelper plg = new PlugInAnnotationedClassesHelper(sysInf);
            plg.AddClassesToDI_OnStartUp(services);

            //Add main configuration
            //ManageConfiguration conf = InitializePluginDescriptors();
            services.AddScoped<ManageConfiguration>();

            services.AddScoped<LoginFilter>();

            //Instance to get Protel DBs from configuration file
            services.AddSingleton<ProtelDBsHelper>();

            //Instance to get WebPos DBs from configuration file
            services.AddSingleton<WebPosDBsHelper>();
            //Instance to get Smtp Configuration
            services.AddSingleton<SmtpHelper>();

            //Instance to get HitPos DBs from configuration file
            services.AddSingleton<HitPosDBsHelper>();

            //Instance to get Protel Hotels from database
            services.AddSingleton<ProtelHelper>();

            //Instance to get Ermis DBs from configuration file
            services.AddSingleton<ErmisDBsHelper>();
            

            //Load all automappers.. Get dll that has a class inherited from Profile
            List<string> addAutoMapperFiles = GetAllAutomapperPlugins(sysInf);

            foreach (string item in addAutoMapperFiles)
                services.AddAutoMapper(Assembly.LoadFrom(item));

            //services.AddSingleton<IApplicationBuilder>();
            //services.AddSingleton<IMapper>();

            //Add hangfire memory instance
            services.AddHangfire(config =>
                            config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                            .UseSimpleAssemblyNameTypeSerializer()
                            .UseDefaultTypeSerializer()
                            .UseMemoryStorage());
            services.AddHangfireServer();

            services.AddSingleton<HangFire_ManageServices>();
            services.AddSingleton<InitializerHelper>();


            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(60);
            });

            //Helper to manage all services with hangFire
            //services.AddSingleton<AddServicesOnStartUp>();

        }

        /// <summary>
        /// Check if a dll contains a class inherited the Profile
        /// </summary>
        /// <param name="sysInf"></param>
        /// <returns></returns>
        private List<string> GetAllAutomapperPlugins(SystemInfo sysInf)
        {
            List<string> result = new List<string>();

            if (!Directory.Exists(sysInf.pluginPath))
                return result;

            string addFile;

            string[] plugIns = Directory.GetDirectories(sysInf.pluginPath);
            foreach (string plgPath in plugIns)
            {
                addFile = "";
                var assemblyFiles = Directory.GetFiles(plgPath, "*.dll", SearchOption.AllDirectories);
                foreach (var assemblyFile in assemblyFiles)
                {
                    Assembly assembly = Assembly.LoadFrom(assemblyFile);
                    foreach (Type item in assembly.GetExportedTypes())
                    {
                        if (item.BaseType.Name == "Profile")
                        {
                            addFile = item.Assembly.Location;
                            var fld = result.Find(f => f == addFile);
                            if (fld == null)
                                result.Add(addFile);
                            continue;
                        }
                    }
                }


            }

            return result;
        }

        /// <summary>
        /// Initialize system info model to get information about HitServiceCore such as version, root path....
        /// </summary>
        /// <returns></returns>
        private SystemInfo InitializeSystemInfo()
        {
            SystemInfo result = new SystemInfo();

            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);

            result.version = fvi.FileVersion;
            result.rootPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            result.pluginPath = Path.Combine(result.rootPath, "Plugins");
            result.pluginFilePath = Path.Combine(result.rootPath, "PluginFiles");
            return result;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, SystemInfo sysInfo,
            PluginsMvcLoader pluginsMvcLoader, List<MainConfigurationModel> diConfigs, List<PlugInDescriptors> plgDescr,
            ApplicationPartManager apm, ILogger<Startup> logger, List<SchedulerServiceModel> scheduledServices,
            IBackgroundJobClient backgroundJobClient, IRecurringJobManager recurringJobManager, 
            IServiceProvider serviceProvider, DIHelper diHelper)
        {
            DIHelper.AppBuilder = app; // diHelper.AppBuilder

            //Load classes from plugin to PlugInDescriptor list
            pluginsMvcLoader.LoadMvcPlugins(apm);


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseSession();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapDefaultControllerRoute();
                endpoints.MapRazorPages();
            });
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Login}/{action=Index}");
            });

            //For Hangfire dashboard UI
            var options = new DashboardOptions { AppPath = "/Plugins" };
            app.UseHangfireDashboard("/Scheduler", options);

            //Add jobs to hangfire. 
            //BackRounfJob can be null because first time we will load all services and not run FireAndForget method
            HangFire_ManageServices hangHlp = new HangFire_ManageServices(scheduledServices, recurringJobManager, null);
            hangHlp.LoadServices();


            /// <summary>
            /// Run Threads
            /// </summary>
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                /* run your code here */

                /*Check if all active services exists on hang fire*/
                CheckActiveServicesEnabledOnHangHire checkHangfireServices = new CheckActiveServicesEnabledOnHangHire(/*recurringJobManager, scheduledServices,*/ hangHlp, plgDescr, diConfigs);
                checkHangfireServices.CheckServicesOnhangFire();

            }).Start();

        }

        private void ConfigureApplicationParts(ApplicationPartManager apm)
        {

            //string configPath = Path.GetFullPath(Path.Combine(new string[] { rootPath, "Config", "appSettings.json" }));
            ///*Use HitHelpersNetCore dll to convert config file to dictionary*/
            //MainconfigurationHelper hlpConfig = new MainconfigurationHelper(configPath);

            //string pluginPath = Path.Combine(rootPath, "Plugins");
            //pluginHlp = new GetPluginsHelper(pluginPath, crons);

        }
    }
}
