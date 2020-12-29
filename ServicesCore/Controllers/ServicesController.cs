using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HitServicesCore.Filters;
using HitServicesCore.Helpers;
using HitServicesCore.Models;
using Microsoft.AspNetCore.Mvc;

namespace HitServicesCore.Controllers
{

    public class ServicesController : Controller
    {
        private readonly List<SchedulerServiceModel> scheduledTasks;
        private readonly HangFire_ManageServices hangfire;
        public ServicesController(List<SchedulerServiceModel> _scheduledTasks, HangFire_ManageServices _hangfire)
        {
            this.scheduledTasks = _scheduledTasks;
            this.hangfire = _hangfire;
        }

        [ServiceFilter(typeof(LoginFilter))]
        public IActionResult Index(string error)
        {
            if (error == null) error = "";
            scheduledTasks.OrderBy(x => x.serviceName);
            ViewBag.ScheduledTasks = scheduledTasks;
            ViewBag.error = error;
            return View();
        }

        [ServiceFilter(typeof(LoginFilter))]
        public IActionResult Redirect()
        {
            scheduledTasks.OrderBy(x => x.serviceName);
            ViewBag.ScheduledTasks = scheduledTasks;
            return View("Index");
        }

        [ServiceFilter(typeof(LoginFilter))]
        [HttpPost]
        public async Task<ActionResult> FireAndForget(ServiceId model)
        {
            var res = hangfire.FireAndForget(new Guid(model.serviceId));
            return Ok(res);
        }
        public class ServiceId
        {
           public  string serviceId { get; set; }
        }


        [ServiceFilter(typeof(LoginFilter))]
        public IActionResult ChangeStatusToActive(string serviceId)
        {
            SchedulerServiceModel currentEditedService = scheduledTasks.Where(x => x.serviceId == new Guid(serviceId)).FirstOrDefault();
            scheduledTasks.Remove(currentEditedService);
            currentEditedService.isActive = true;
            scheduledTasks.Add(currentEditedService);
           
            hangfire.SaveSchedulersJobs(scheduledTasks);
            return RedirectToAction("Index", "Services");
        }
        [ServiceFilter(typeof(LoginFilter))]
        public IActionResult ChangeStatusToInActive(string serviceId)
        {
            SchedulerServiceModel currentEditedService = scheduledTasks.Where(x => x.serviceId == new Guid(serviceId)).FirstOrDefault();
            scheduledTasks.Remove(currentEditedService);
            scheduledTasks.Add(currentEditedService);
            currentEditedService.isActive = false;
            hangfire.SaveSchedulersJobs(scheduledTasks);
            return RedirectToAction("Index", "Services");
        }

    }
}
