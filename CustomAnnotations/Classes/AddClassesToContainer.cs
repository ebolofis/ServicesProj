
using HitCustomAnnotations.Interfaces;
using HitServicesCore.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace HitCustomAnnotations.Classes
{
    [AttributeUsage(AttributeTargets.Class |
                    AttributeTargets.Struct,
                       AllowMultiple = true)  // Multiuse attribute.  
    ]
    public class AddClassesToContainer : Attribute, IAddClassesToContainer
    {
        /// <summary>
        /// add class to DI as scope, sigleton or Transient
        /// </summary>
        ServicesAddTypeEnum serviceType;

        /// <summary>
        /// Description for the class
        /// </summary>
        public string description;

        public AddClassesToContainer(ServicesAddTypeEnum _serviceType, string _description)
        {
            serviceType = _serviceType;
            description = _description;
        }

        public ServicesAddTypeEnum GetServiceType()
        {
            return serviceType;
        }

        public string GetDescription()
        {
            return description;
        }

        
    }
}
