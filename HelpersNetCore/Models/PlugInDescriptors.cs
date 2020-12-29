using HitHelpersNetCore.Models.SharedModels;
using HitServicesCore.Models.SharedModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace HitHelpersNetCore.Models
{
    public class PlugInDescriptors
    {
        /// <summary>
        /// Main Descriptor. If exists on any plugin class then the plugin will be loaded else passed
        /// </summary>
        public MainDescriptorWithAssemplyModel mainDescriptor { get; set; }

        /// <summary>
        /// class for configuration helper. Inherited by the abstract class AbstractConfigurationHelper
        /// </summary>
        public ConfigDescriptorModel configClass { get; set; }

        /// <summary>
        /// Initialer for database. Executed by button from Back Office
        /// </summary>
        public InitialerDescriptorModel initialerDescriptor { get; set; }

        //public MainConfigurationModel mainConfiguration { get; set; }

        /// <summary>
        /// List of services to added on Hang Fire
        /// </summary>
        public List<ServiceDescriptorWithTypeModel> serviceDescriptor { get; set; }

        /// <summary>
        /// List of classes to added on DI on startUp for main project HitServicesCore
        /// </summary>
        public List<DIDescriptorWithTypeModel> dIDescriptor { get; set; }
    }
}
