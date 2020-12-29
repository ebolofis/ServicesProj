using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HitHelpersNetCore.Models;
using HitServicesCore.Filters;
using HitServicesCore.Helpers;
using HitServicesCore.Models;
using HitServicesCore.Models.SharedModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace HitServicesCore.Controllers
{
    
    public class PluginsController : Controller
    {
        private readonly List<PlugInDescriptors> plugins;
        private readonly LoginsUsers loginsUsers;

        public PluginsController(ILogger<PluginsController> logger,List<PlugInDescriptors> _plugins,LoginsUsers loginsUsers)
        {
            this.plugins = _plugins;
            this.loginsUsers = loginsUsers;
        }

        [ServiceFilter(typeof(LoginFilter))]
        public IActionResult Index(bool usertype)
        {
            List<PlugInDescriptors> plg = plugins;
            List<MainDescriptorWithAssemplyModel> mainDesc= new List<MainDescriptorWithAssemplyModel>();
            foreach (PlugInDescriptors model in plg)
                mainDesc.Add(model.mainDescriptor);
            return View("Plugins",mainDesc);
        }
    }
}
