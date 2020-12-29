using Hangfire;
using Hangfire.Common;
using HitCustomAnnotations.Interfaces;
using HitHelpersNetCore.Helpers;
using HitHelpersNetCore.Interfaces;
using HitHelpersNetCore.Models;
using HitServicesCore.InternalServices;
using HitServicesCore.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace HitServicesCore.Helpers
{
    public class HangFire_ManageServices
    {
        /// <summary>
        /// List of plugins with jobs to added on hangfire
        /// </summary>
        private List<SchedulerServiceModel> hangFireServices;

        /// <summary>
        /// Instance for HangFile
        /// </summary>
        private IRecurringJobManager hangFire;

        /// <summary>
        /// Backround Job client
        /// </summary>
        private IBackgroundJobClient jobClient;

        /// <summary>
        /// root path
        /// </summary>
        private string CurrentPath;

        /// <summary>
        /// Instance for encryption helper
        /// </summary>
        private readonly EncryptionHelper eh;

        /// <summary>
        /// Lock read, write json files
        /// </summary>
        private object lockJsons = new object();

        private ILogger<HangFire_ManageServices> logger;

        public HangFire_ManageServices(List<SchedulerServiceModel> _hangFireServices,
            IRecurringJobManager _hangFire, IBackgroundJobClient _jobClient)
        {
            hangFireServices = _hangFireServices;
            hangFire = _hangFire;
            jobClient = _jobClient;
            CurrentPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            Directory.SetCurrentDirectory(CurrentPath);
            eh = new EncryptionHelper();
            CheckLogger();
        }

        private void CheckLogger()
        {
            if (logger == null)
            {
                IApplicationBuilder _app = DIHelper.AppBuilder;
                if (_app != null)
                {
                    //variable to get from HitServiceCore Sigletons
                    var services = _app.ApplicationServices;//diHelper.Services();//
                    logger = services.GetService<ILogger<HangFire_ManageServices>>();
                }
            }
        }

        /// <summary>
        /// Add Jobs to HangFire
        /// </summary>
        /// <param name="logger"></param>
        public void AddServicesToHangFire(/*NLog.Logger logger*/)
        {
            try
            {
                CheckLogger();
                foreach (SchedulerServiceModel item in hangFireServices)
                {
                    if (item.isActive)
                    {
                        //Create Class
                        Type LoadType = Type.GetType(item.classFullName + ", " + item.assemblyFileName);

                        if (LoadType == null) 
                            logger.LogError(">>>>>>> Class :" + item.description + " not found !!!");
                        
                        //create Instance of Class
                        object instance = Activator.CreateInstance(LoadType);

                        //Get Method Start
                        MethodInfo method = LoadType.GetMethod("Start");

                        //Pass Parameters. All classes implement the Interface IServiceExecutions. Methodf start has one parameter for Service Id as Guid
                        var hfjob = new Job(LoadType, method, new object[1] { item.serviceId });

                        //Add To HangFire
                        hangFire.AddOrUpdate(item.serviceName + " (" + item.serviceId.ToString() + ")", hfjob, item.schedulerTime, TimeZoneInfo.Local);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
            }
        }

        /// <summary>
        /// Load Services to IRecurringJobManager
        /// </summary>
        public void LoadServices()
        {
            AddServicesToHangFire();
        }

        /// <summary>
        /// Removes all Jobs from HangFire and Load them again with changes
        /// </summary>
        private void ReloadHangFireServices(/*NLog.Logger logger*/)
        {
            foreach (SchedulerServiceModel item in hangFireServices)
            {
                if (!item.isActive)
                    hangFire.RemoveIfExists(item.serviceName + " (" + item.serviceId.ToString() + ")");
            }
            AddServicesToHangFire();
        }

        /// <summary>
        /// Saves all scheduler jobs to file and reload the DI List of hang fire jobs
        /// </summary>
        /// <param name="jobs"></param>
        /// <returns></returns>
        public bool SaveSchedulersJobs(List<SchedulerServiceModel> jobs)
        {
            bool result = true;
            CheckLogger();
            try
            {
                lock (lockJsons)
                {
                    //Gets the file name to save hang fire jobs
                    string sFileName = Path.GetFullPath(Path.Combine(new string[] { CurrentPath, "Config", "scheduler.json" }));

                    //File not exists so creates one
                    if (!File.Exists(sFileName))
                        File.WriteAllText(sFileName, " ");

                    //Save changes to file
                    string sVal = System.Text.Json.JsonSerializer.Serialize(jobs);
                    sVal = eh.Encrypt(sVal);
                    File.WriteAllText(sFileName, sVal);

                    //Check all changed jobs with the loaded
                    foreach (SchedulerServiceModel item in jobs)
                    {
                        SchedulerServiceModel tmp = FillEmptyValues(item);

                        var fld = hangFireServices.Find(f => f.serviceId == item.serviceId);
                        //changed job not exists so add it to loaded
                        if (fld == null)
                            hangFireServices.Add(tmp);
                        //changed job exists so change it to loaded  
                        else
                            fld = tmp;
                    }
                    //Reload service to main project
                    ReloadHangFireServices(/*logger*/);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                result = false;
            }
            return result;
        }

        /// <summary>
        /// Check if any value is empty to fill it from json file
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private SchedulerServiceModel FillEmptyValues(SchedulerServiceModel model)
        {
            var fld = hangFireServices.Find(f => f.serviceId == model.serviceId);
            if (fld != null)
            {
                if (string.IsNullOrWhiteSpace(model.assemblyFileName))
                    model.assemblyFileName = fld.assemblyFileName;
                if (string.IsNullOrWhiteSpace(model.classFullName))
                    model.classFullName = fld.classFullName;
                if (string.IsNullOrWhiteSpace(model.description))
                    model.description = fld.description;
                if (string.IsNullOrWhiteSpace(model.serviceVersion))
                    model.serviceVersion = fld.serviceVersion;
                if (string.IsNullOrWhiteSpace(model.serviceName))
                    model.serviceName = fld.serviceName;
            }
            return model;
        }

        /// <summary>
        /// Get's a job from DI list based on Service Id and executed it
        /// </summary>
        /// <param name="serviceId"></param>
        public string FireAndForget(Guid serviceId)
        {
            try
            {
                CheckLogger();
                ////Find thee service based on Guid Service Id
                var fld = hangFireServices.Find(f => f.serviceId == serviceId);

                //Not found on list so return
                if (fld == null)
                {
                    logger.LogInformation("Service with Id " + serviceId, ToString() + " not found");

                    return "Service with Id " + serviceId + " not found";
                }
                //Load Assemply Type
                Type LoadType = Type.GetType(fld.classFullName + ", " + fld.assemblyFileName);

                //Could not load class
                if (LoadType == null)
                {
                    logger.LogError(">>>>>>> Class :" + fld.description + " not found !!!");
                    return "Error on Fire And Forget  >>>>>>> Class :" + fld.description + " not found !!!";
                }

                //Create instance of IServiceExecutions (Implement interface for all services)
                ServiceExecutions instance = (ServiceExecutions)Activator.CreateInstance(LoadType);

                logger.LogInformation("Executing (Fire-and-Forget) Job: " + fld.ToString() + "...");
                
                //Add Job to Backround With parameter the Service Id as Guyid
                BackgroundJob.Enqueue(() => instance.Start(fld.serviceId));

                return "OK";
            }
            catch (Exception ex)
            {
                logger.LogError("Error on Fire And Forget  " + Convert.ToString(ex));
                return "Error on Fire And Forget  " + Convert.ToString(ex);
            }
        }
    }
}
