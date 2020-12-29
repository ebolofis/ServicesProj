using HitCustomAnnotations.Classes;
using HitHelpersNetCore.Helpers;
using HitHelpersNetCore.Interfaces;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using AutoMapper;
using HitServicesCore.Models;
using System.IO;
using HitServicesCore.Models.IS_Services;
using HitServicesCore.MainLogic.Flows;
using HitServicesCore.Helpers;

namespace HitServicesCore.InternalServices
{
    [SchedulerAnnotation("6cf39393-9e3d-4ec2-bd92-5be81b2eaadc", "SaveToTableService", "Service to save data from a database to another based on IS_Services\\SaveToTable directory and all jsons included on it", "1.0.1.0")]
    public class ISSaveToTableService : ServiceExecutions
    {

        public ISSaveToTableService() : base()
        {

        }

        public override void Start(Guid _serviceId)
        {
            //Instance for intenal services helper
            IS_ServicesHelper isServicesHlp = new IS_ServicesHelper();

            //List of internal services based on save to table 
            List<ISSaveToTableModel> saveToTableServices = isServicesHlp.GetSaveToTableFromJsonFiles();

            //get service based on serviceId guid
            ISSaveToTableModel currentService = saveToTableServices.Find(f => f.serviceId == _serviceId);

            if (currentService != null)
            {
                SaveDataToDBFlow flow = new SaveDataToDBFlow(currentService);
                flow.SaveDataToDB();
            }
        }
    }
}
