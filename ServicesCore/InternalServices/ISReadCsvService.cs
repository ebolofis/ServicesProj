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
using HitServicesCore.MainLogic.Flows;
using HitServicesCore.Helpers;

namespace HitServicesCore.InternalServices
{
    [SchedulerAnnotation("4f83485b-89b6-46c2-8612-d369f6388a52", "readCsvService", "Service to read data from a csv file and import them to a database based on IS_Services\\ReadCsv directory and all jsons included on it", "1.0.1.0")]
    public class ISReadCsvService : ServiceExecutions
    {
        public ISReadCsvService() : base()
        {

        }

        public override void Start(Guid _serviceId)
        {
            //Instance for intenal services helper
            IS_ServicesHelper isServicesHlp = new IS_ServicesHelper();

            //List of internal services based on save to table 
            List<ISReadFromCsvModel> readFromCsvServices = isServicesHlp.GetReadFromCsvFromJsonFiles();

            //get service based on serviceId guid
            ISReadFromCsvModel currentService = readFromCsvServices.Find(f => f.serviceId == _serviceId);

            if (currentService != null)
            {
                ReadCsvFlows flow = new ReadCsvFlows(currentService);
                flow.ReadFromCsv();
            }

        }
    }
}
