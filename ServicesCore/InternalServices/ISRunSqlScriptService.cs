using HitCustomAnnotations.Classes;
using HitHelpersNetCore.Helpers;
using HitHelpersNetCore.Interfaces;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using HitServicesCore.MainLogic.Flows;
using AutoMapper;
using HitServicesCore.Models;
using HitServicesCore.Models.IS_Services;
using System.IO;
using NLog;
using HitServicesCore.Helpers;

namespace HitServicesCore.InternalServices
{
    [SchedulerAnnotation("8e477a21-8853-47e1-be86-9b6de10e9718", "RunSqlScriptService", "Service to execute sql scripts based on IS_Services\\SqlScripts directory and all jsons included on it", "1.0.1.0")]
    public class ISRunSqlScriptService : ServiceExecutions
    {
        public ISRunSqlScriptService() : base()
        {

        }

        public override void Start(Guid _serviceId)
        {
            IApplicationBuilder _app = DIHelper.AppBuilder;
            if (_app == null)
            {

                //throw new Exception("Application builder is Empty");
            }
            //variable to get from HitServiceCore Sigletons
            var services = _app.ApplicationServices;
            
            //Instance for intenal services helper
            IS_ServicesHelper isServicesHlp = new IS_ServicesHelper();

            //List of internal services based on run sql script 
            List<ISRunSqlScriptsModel> runSqlServices = isServicesHlp.GetRunSqlScriptsFromJsonFiles();
            
            //get service based on serviceId guid
            ISRunSqlScriptsModel currentService = runSqlServices.Find(f => f.serviceId == _serviceId);

            //Found (not null) and execute code from flow
            if (currentService != null)
            {
                SQLFlows sqlFlow = new SQLFlows(currentService);
                sqlFlow.RunScript(currentService.SqlScript, currentService.Custom1DB);
            }

        }
    }
}
