using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace HitHelpersNetCore.Helpers
{
    /// <summary>
    /// Class that provides DI container for everyone
    /// </summary>
   public class DIHelper
    {
    
        public DIHelper()
        {
           
        }


        public static IApplicationBuilder AppBuilder { get; set; }

        /// <summary>
        /// DI Container
        /// </summary>
        /// <returns></returns>
        public IServiceProvider Services()
        {
            return AppBuilder.ApplicationServices;
        }
    }
}
