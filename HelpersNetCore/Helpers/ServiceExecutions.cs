using Hangfire;
using System;
using System.Collections.Generic;
using System.Text;

namespace HitHelpersNetCore.Helpers
{
    [DisableConcurrentExecution(0)]
    [AutomaticRetry(Attempts = 1, LogEvents = true, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
    public abstract class ServiceExecutions
    {
        /// <summary>
        /// Starting a service as hang fire service
        /// </summary>
        /// <param name="_serviceId"></param>
        public abstract void Start(Guid _serviceId);
    }
}
