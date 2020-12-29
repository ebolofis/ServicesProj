using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HitHelpersNetCore.Helpers;
using HitHelpersNetCore.Models;
using HitServicesCore.Filters;
using HitServicesCore.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HitServicesCore.Controllers
{
    public class InitializeDBController : Controller
    {  private readonly InitializerHelper ihelper;
        private readonly List<PlugInDescriptors> plugins;
        private readonly IApplicationBuilder app;
        ILogger<InitializeDBController> logger;
        DIHelper diHelper;

        public InitializeDBController(InitializerHelper _ihelper, List<PlugInDescriptors> _plugins, DIHelper diHelper, ILogger<InitializeDBController> _logger)
        {
            this.ihelper = _ihelper;
            this.plugins = _plugins;
            this.diHelper = diHelper;
            this.logger = _logger;
        }
        [ServiceFilter(typeof(LoginFilter))]
        public IActionResult InitializeDB(string error)
        {
            if (error != null)
                ViewBag.error = error;
            ViewBag.plugins = plugins;
            return View();
        }

        [ServiceFilter(typeof(LoginFilter))]
        public IActionResult Initialize(string pluginId)
        {
            if(pluginId != null) {
                logger.LogInformation("Initializing DB of PlugIn with id " + pluginId);

                string dbv1 =  plugins.Where(x => x.mainDescriptor.plugIn_Id == new Guid(pluginId)).FirstOrDefault().initialerDescriptor.dbVersion;
                var dbVersion = new Version(dbv1);
                string dbv2 = plugins.Where(x => x.mainDescriptor.plugIn_Id == new Guid(pluginId)).FirstOrDefault().initialerDescriptor.latestUpdate;
                var currVersion =  new Version(dbv2);
                if (currVersion < dbVersion)
                {
                    try 
                    { 
                        //ihelper.RunInitialMethod(new Guid(pluginId), diHelper.AppBuilder);
                        ihelper.RunInitialMethod(new Guid(pluginId), DIHelper.AppBuilder);
                    }
                    catch (Exception e)
                    {
                        string error = e.Message;
                        if(e.InnerException != null)
                            error += " " + e.InnerException.Message;
                        
                        return RedirectToAction("InitializeDB", "InitializeDB", new { error });
                    }
                }
            }
            return RedirectToAction("InitializeDB", "InitializeDB", new { error  = "Success" });
        }
    }
}
