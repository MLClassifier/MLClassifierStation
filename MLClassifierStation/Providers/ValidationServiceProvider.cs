using System;
using System.Collections.Generic;

namespace MLClassifierStation.Providers
{
    public class ValidationServiceProvider : IServiceProvider
    {
        private Dictionary<Type, object> services = new Dictionary<Type, object>();

        public object GetService(Type serviceType)
        {
            if (services.ContainsKey(serviceType))
                return services[serviceType];

            return null;
        }

        public void AddService(Type serviceType, object service)
        {
            services.Add(serviceType, service);
        }
    }
}