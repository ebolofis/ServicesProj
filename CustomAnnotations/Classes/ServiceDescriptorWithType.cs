using System;
using System.Collections.Generic;
using System.Text;

namespace HitCustomAnnotations.Interfaces
{
    public class ServiceDescriptorWithType : IServiceDescriptorWithType
    {
        /// <summary>
        /// Load the class
        /// </summary>
        public Type classType { get; private set; }

        /// <summary>
        /// Service file version
        /// </summary>
        public string serviceVersion { get; private set; }

        /// <summary>
        /// Unique guid as id. generated from service author and it is the same for all releases (from custom annotation SchedulerAnnotation)
        /// </summary>
        public Guid serviceId { get; private set; }

        /// <summary>
        /// Service name (from custom annotation SchedulerAnnotation)
        /// </summary>
        public string seriveName { get; private set; }

        /// <summary>
        /// service description (from custom annotation SchedulerAnnotation)
        /// </summary>
        public string serviceDescription { get; private set; }

        /// <summary>
        /// File name (dll name)
        /// </summary>
        public string fileName { get; private set; }

        /// <summary>
        /// Path where dll exists
        /// </summary>
        public string path { get; private set; }
    }
}
