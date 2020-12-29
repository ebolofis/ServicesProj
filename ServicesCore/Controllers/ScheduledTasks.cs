using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HitServicesCore.Filters;
using HitServicesCore.Helpers;
using HitServicesCore.Models;
using Microsoft.AspNetCore.Mvc;
using NCrontab;

namespace HitServicesCore.Controllers
{
    public class ScheduledTasks : Controller
    {
        private readonly List<SchedulerServiceModel> scheduledTasks;
        private readonly HangFire_ManageServices hangfire;
        private static string currentServiceId;
        public ScheduledTasks(List<SchedulerServiceModel> _scheduledTasks, HangFire_ManageServices _hangfire)
        {
            this.scheduledTasks = _scheduledTasks;
            this.hangfire = _hangfire;
        }
        [ServiceFilter(typeof(LoginFilter))]
        public IActionResult Index(string serviceId)
        {
            SchedulerServiceModel model = scheduledTasks.Where(x => x.serviceId == new Guid(serviceId)).FirstOrDefault();
            ViewBag.schedulerDescr = model.schedulerDescr;
            ViewBag.occurences = ParseCron(model.schedulerTime);
            if (serviceId == null)
                currentServiceId = Convert.ToString(scheduledTasks[0].serviceId);
            else
                currentServiceId = serviceId;
            ViewBag.ScheduledJob = scheduledTasks.Where(x => x.serviceId == new Guid(serviceId)).FirstOrDefault().description;
            ViewBag.ScheduledTime = scheduledTasks.Where(x => x.serviceId == new Guid(serviceId)).FirstOrDefault().schedulerTime;
            string str = Convert.ToString(ViewBag.ScheduledTime);
            string[] currentTime = str.Split(null); 
            List<string> manualTime= new List<string>();
                foreach (string s in currentTime)
            {
                if (s.Contains("*/"))
                    manualTime.Add(s.Substring(2));
                else
                    manualTime.Add(s);

            }
            ViewBag.currentTime = currentTime;
            ViewBag.manualTime = manualTime;
            
            ViewBag.ScheduledTasks = scheduledTasks;
            return View("ScheduledTasks");
        }

        [HttpPost]
        public async Task<IActionResult> ScheduleJob(schedulerHelper obj)
        {
            SchedulerServiceModel currentEditedService = scheduledTasks.Where(x => x.serviceId == new Guid(currentServiceId)).FirstOrDefault();
            scheduledTasks.Remove(currentEditedService);
            currentEditedService.schedulerTime = obj.stars;
            currentEditedService.schedulerDescr = obj.starsDesc;
            scheduledTasks.Add(currentEditedService);
            hangfire.SaveSchedulersJobs(scheduledTasks);
            return Ok();
        }

        private object ParseCron(string cron)
        {
            var s = CrontabSchedule.Parse(cron);
            var start = DateTime.Now;
            var end = start.AddMonths(25);
            var occurrences = s.GetNextOccurrences(start, end).Take(500).Select(x => x.ToString("ddd, dd MMM yyyy  HH:mm")).ToList();
            return occurrences;
        }
        public class schedulerHelper
        {
            public string stars { get; set; }
            public string starsDesc { get; set; }
        }

    }
}
