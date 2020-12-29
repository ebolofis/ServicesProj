using System;
using System.Collections.Generic;
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
    public class SaveToTableController : Controller
    {
        private readonly ILogger<SaveToTableController> logger;

        public SaveToTableController( ILogger<SaveToTableController> _logger)
        {
            logger = _logger;
        }
        public IActionResult Index()
        {
            return View();
        }

        public void CreateNewSaveToTableFile(ISSaveToTableModel newmodel)
        {
            try
            {
                newmodel.serviceVersion = 1;
                IS_ServicesHelper serviceshelper = new IS_ServicesHelper();
                List<ISSaveToTableModel> res = serviceshelper.GetSaveToTableFromJsonFiles();
                res.Add(newmodel);
                serviceshelper.SaveSaveToTableJsons(res);
            }
            catch (Exception e)
            {
                logger.LogError("Exception new file could not be created");
                logger.LogError("Error:" + Convert.ToString(e));
            }
            return;
        }
        public void UpdateSaveToTableFile(ISSaveToTableModel updatedmodel)
        {

            updatedmodel.ClassType = "Job";
            updatedmodel.serviceType = HangFireServiceTypeEnum.SaveToTable;
            if (updatedmodel.serviceVersion == null) updatedmodel.serviceVersion = 1;
            try
            {
                IS_ServicesHelper serviceshelper = new IS_ServicesHelper();
                List<ISSaveToTableModel> model = serviceshelper.GetSaveToTableFromJsonFiles();
                model = model.Where(x => x.serviceName != updatedmodel.serviceName).ToList();
                model.Add(updatedmodel);

                serviceshelper.SaveSaveToTableJsons(model);
            }
            catch (Exception e)
            {
                logger.LogError("Failed to update existing file with name =" + updatedmodel.serviceName);
                logger.LogError("Error:" + Convert.ToString(e));
            }
            return;
        }
    }
}
