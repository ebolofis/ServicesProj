using HitServicesCore.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HitServicesCore.Models
{
    public class SchedulerServiceModel
    {
        /// <summary>
        /// unique guid made from plugin for each service class
        /// </summary>
        public Guid serviceId { get; set; }

        /// <summary>
        /// Service name
        /// </summary>
        public string serviceName { get; set; }

        /// <summary>
        /// Full class name (with namespace)
        /// </summary>
        public string classFullName { get; set; }

        /// <summary>
        /// Real name for dll file
        /// </summary>
        public string assemblyFileName { get; set; }

        /// <summary>
        /// Description for service
        /// </summary>
        public string description { get; set; }

        /// <summary>
        /// true class is active, false not active
        /// </summary>
        public bool isActive { get; set; }

        /// <summary>
        /// Execution time in cron
        /// </summary>
        public string schedulerTime { get; set; }

        /// <summary>
        /// Description for scheduler execution time
        /// </summary>
        public string schedulerDescr { get; set; }

        /// <summary>
        /// Service current version
        /// </summary>
        public string serviceVersion { get; set; }

        /// <summary>
        /// Type of service
        /// 0 => comes from plugin
        /// 1 => Incorporated Services (Export Data)
        /// 2 => Incorporated Services (Run Sql Script)
        /// 3 => Incorporated Services (Save To Table)
        /// 4 => Incorporated Services (Read From CSV)
        /// </summary>
        public HangFireServiceTypeEnum serviceType { get; set; } = HangFireServiceTypeEnum.Plugin;
    }
}
