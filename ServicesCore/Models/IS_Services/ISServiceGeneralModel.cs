using HitServicesCore.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HitServicesCore.Models.IS_Services
{
    /// <summary>
    /// General model for all IS services.
    /// </summary>
    public class ISServiceGeneralModel
    {
        /// <summary>
        /// Full class name to get the annotations for active fields.
        /// </summary>
        public string FullClassName { get; set; }

        /// <summary>
        /// Class Type 'Job' or 'Controller'
        /// </summary>
        public string ClassType { get; set; }

        /// <summary>
        /// Description for service
        /// </summary>
        public string ClassDescription { get; set; }

        /*For HitServiceCore classes*/
        /// <summary>
        /// unique guid made from plugin for each service class
        /// </summary>
        public Guid serviceId { get; set; }

        /// <summary>
        /// Service name
        /// </summary>
        public string serviceName { get; set; }

        /// <summary>
        /// Type of service
        /// 0 => comes from plugin
        /// 1 => Incorporated Services (Export Data)
        /// 2 => Incorporated Services (Run Sql Script)
        /// 3 => Incorporated Services (Save To Table)
        /// 4 => Incorporated Services (Read From CSV)
        /// </summary>
        public HangFireServiceTypeEnum serviceType { get; set; }

        /// <summary>
        /// emails to send after succes or failure. Emails separated by ;
        /// </summary>
        public string sendEmailTo { get; set; }

        /// <summary>
        /// Send email after success action on email list
        /// </summary>
        public bool sendEmailOnSuccess { get; set; }

        /// <summary>
        /// Send email after failure action on email list
        /// </summary>
        public bool sendEmailOnFailure { get; set; }

        /// <summary>
        /// Current service version
        /// </summary>
        public long? serviceVersion { get; set; }

    }
}
