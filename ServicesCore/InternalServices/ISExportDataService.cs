using HitCustomAnnotations.Classes;
using HitHelpersNetCore.Helpers;
using HitHelpersNetCore.Interfaces;
using Microsoft.AspNetCore.Builder;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using HitServicesCore.Models;
using System.IO;
using HitServicesCore.Models.IS_Services;
using HitServicesCore.Helpers;
using HitServicesCore.MainLogic.Flows;

namespace HitServicesCore.InternalServices
{
    [SchedulerAnnotation("2582590e-7314-4077-bc92-748710db9ef5", "ExportDataService", "Service to export data based on IS_Services\\ExportData directory and all jsons included on it", "1.0.1.0")]
    public class ISExportDataService : ServiceExecutions
    {

        public ISExportDataService() : base()
        {

        }

        public override void Start(Guid _serviceId)
        {
            //Instance for intenal services helper
            IS_ServicesHelper isServicesHlp = new IS_ServicesHelper();

            //List of internal services based on save to table 
            List<ISExportDataModel> exportDataServices = isServicesHlp.GetExportdataFromJsonFiles();

            //get service based on serviceId guid
            ISExportDataModel currentService = exportDataServices.Find(f => f.serviceId == _serviceId);

            if (currentService != null)
            {
                ExportDataFlows flow = new ExportDataFlows(currentService);
                flow.ExportData();
            }
        }
    }
}
