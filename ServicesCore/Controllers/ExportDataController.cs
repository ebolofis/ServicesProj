using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HitServicesCore.Enums;
using HitServicesCore.Helpers;
using HitServicesCore.Models;
using HitServicesCore.Models.IS_Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HitServicesCore.Controllers
{
    public class ExportDataController : Controller
    {
        private readonly ILogger<ExportDataController> logger;
        private readonly SystemInfo sysInfo;
        private object lockJsons = new object();


        public ExportDataController(SystemInfo _sysInfo, ILogger<ExportDataController> _logger)
        {
            sysInfo = _sysInfo;
            logger = _logger;
        }
        public IActionResult Index()
        {
            return View();
        }


        public void UpdateExistingExportDataScript(ISExportDataModel updatedmodel)
        {
            updatedmodel.ClassType = "Job";
            updatedmodel.serviceType = HangFireServiceTypeEnum.ExportData;
            if (updatedmodel.serviceVersion == null) updatedmodel.serviceVersion = 1;
            try
            {
                IS_ServicesHelper serviceshelper = new IS_ServicesHelper();
                List<ISExportDataModel> model = serviceshelper.GetExportdataFromJsonFiles();
                model = model.Where(x => x.serviceName != updatedmodel.serviceName).ToList();
                model.Add(updatedmodel);

                serviceshelper.SaveExportDataJsons(model);
            }
            catch (Exception e)
            {
                logger.LogError("Failed to update existing file with name =" + updatedmodel.serviceName);
                logger.LogError("Error:" + Convert.ToString(e));
            }
            return;
        }


        public void CreateNewExportDataFile(ISExportDataModel model)
        {
            try
            {
                model.serviceVersion = 1;
                IS_ServicesHelper serviceshelper = new IS_ServicesHelper();
                List<ISExportDataModel> list = serviceshelper.GetExportdataFromJsonFiles();
                list.Add(model);
                serviceshelper.SaveExportDataJsons(list);
            }
            catch (Exception e)
            {
                logger.LogError("Failed to write new file with name =" + model.serviceName);
                logger.LogError("Error:" + Convert.ToString(e));
            }
            return;
        }
    }
}
