using System;
using System.Collections.Generic;
using Core.Interfaces;

namespace Core
{
    public abstract class ServiceLocatorBase
    { 
        protected IDictionary<Type, IService> services;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceLocatorBase"/> class.
        /// </summary>
        internal ServiceLocatorBase()
        {
            services = new Dictionary<Type, IService>();
        }

        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="System.Exception">Service  + typeof(T) +  is not registered!</exception>
        public T GetService<T>()
        {
            try
            {
                return (T)services[typeof(T)];
            }
            catch (KeyNotFoundException)
            {
                throw new Exception("Service " + typeof(T) + " is not registered!");
            }
        }

        /// <summary>
        /// Adds the service.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="service">The service.</param>
        protected void AddService<T>(IService service)
        {
            if (service is T)
            {
                services.Add(typeof(T), service);

            }
            else
            {
                throw new Exception("Service " + service + " have not implemented interface: " + typeof(T));
            }
        }

        /// <summary>
        /// Initializes the services.
        /// </summary>
        /// <exception cref="System.Exception">Service don't have Init() method!</exception>
        public void InitServices()
        {
            foreach (IService service in services.Values)
                 service.Init();
        }
        
        /// <summary>
        /// Dispose the services
        /// </summary>
        public void Dispose()
        {
        }
    }
}
