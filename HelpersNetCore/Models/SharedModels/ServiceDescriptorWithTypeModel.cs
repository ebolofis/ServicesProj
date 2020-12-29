using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HitServicesCore.Models.SharedModels
{
    public class ServiceDescriptorWithTypeModel : ClassDescriptorWithNameSpaceModel 
    {
        /// <summary>
        /// Load the class
        /// </summary>
        public Type classType { get; set; }

        /// <summary>
        /// Service file version
        /// </summary>
        public string serviceVersion { get; set; }

        /// <summary>
        /// Unique guid as id. generated from service author and it is the same for all releases (from custom annotation SchedulerAnnotation)
        /// </summary>
        public Guid serviceId { get; set; }

        /// <summary>
        /// Service name (from custom annotation SchedulerAnnotation)
        /// </summary>
        public string seriveName { get; set; }

        /// <summary>
        /// service description (from custom annotation SchedulerAnnotation)
        /// </summary>
        public string serviceDescription { get; set; }

        /// <summary>
        /// Real file name for the dll to use it to create the instance of class by fullNameSpace
        /// </summary>
        public string assemblyFileName { get; set; }

    }
}
