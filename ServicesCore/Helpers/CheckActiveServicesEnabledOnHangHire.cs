using Hangfire;
using Hangfire.Common;
using HitHelpersNetCore.Models;
using HitServicesCore.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace HitServicesCore.Helpers
{
    public class CheckActiveServicesEnabledOnHangHire
    {
        private readonly List<PlugInDescriptors> plugIns;
        private readonly List<MainConfigurationModel> config;
        private readonly HangFire_ManageServices hangFireManager;
        public CheckActiveServicesEnabledOnHangHire(HangFire_ManageServices _hangFireManager,
            List<PlugInDescriptors> _plugIns,
            List<MainConfigurationModel> _config)
        {
            plugIns = _plugIns;
            config = _config;
            hangFireManager = _hangFireManager;
        }

        /// <summary>
        /// Void to check if all active services exists on hangfire.
        /// Run as idevidual thread.
        /// </summary>
        /// <param name="_hangFire"></param>
        /// <param name="_hangFireServices"></param>
        /// <param name="_plugIns"></param>
        /// <param name="_config"></param>
        public void CheckServicesOnhangFire()
        {

            while (true)
            {
                //Get main configuration to check for how time it will be sleeped
                MainConfigHelper mainconfigHlp = new MainConfigHelper(config, plugIns);
                MainConfigurationModel mainConfigModel = mainconfigHlp.ReadHitServiceCoreConfig();
                int sleep = mainConfigModel.config.config["CheckServiceOnScheduler"];
                //int.TryParse(mainConfigModel.config.config["CheckServiceOnScheduler"], out sleep);
                if (sleep < 1)
                    sleep = 1;
                sleep = sleep * 60000;

                //create logger instance
                string CurrentPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                var ps = new List<string>() { CurrentPath, "Config", "NLog.config" };
                var logpath = Path.GetFullPath(Path.Combine(ps.ToArray()));
                var logger = NLog.Web.NLogBuilder.ConfigureNLog(logpath).GetCurrentClassLogger();

                try
                {
                    //Call method from HangFire_ManageServices to add deleted services
                    hangFireManager.AddServicesToHangFire(/*logger*/);
                }
                catch (Exception ex)
                {
                    logger.Error(ex.ToString());
                }

                Thread.Sleep(sleep);
            }
        }

    }
}
