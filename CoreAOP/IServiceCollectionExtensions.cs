namespace CoreAOP
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Microsoft.Extensions.DependencyInjection;

    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Find any services which have IAspect attributes and replace them with proxied objects
        /// </summary>
        /// <param name="services"></param>
        public static void AddAspects(this IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();
            for (int i = 0; i < services.Count; i++)
            {
                var descriptor = services[i];
                if (HasAspects(descriptor.ServiceType))
                {
                    var aspectDescriptor = new ServiceDescriptor(descriptor.ServiceType, GetAspectAttributeProxy(provider, services, descriptor.ServiceType, descriptor.ImplementationType));
                    services.RemoveAt(i);
                    services.Insert(i, aspectDescriptor);
                }
            }
        }

        /// <summary>
        /// Applies an IAspect to a service
        /// </summary>
        /// <param name="services"></param>
        /// <param name="serviceType"></param>
        /// <param name="aspectType"></param>
        public static void AddAspect(this IServiceCollection services, Type serviceType, Type aspectType)
        {
            if (!serviceType.IsInterface)
                return;

            var provider = services.BuildServiceProvider();
            var aspectDescriptor = new ServiceDescriptor(serviceType, GetAspectProxy(provider, services, serviceType, aspectType));
            services.Add(aspectDescriptor);
        }

        /// <summary>
        /// Applies an IAspect to a service
        /// </summary>
        /// <param name="services"></param>
        /// <param name="serviceType"></param>
        /// <param name="aspectType"></param>
        public static void AddAspect<TService, TAspect>(this IServiceCollection services)
        {
            AddAspect(services, typeof(TService), typeof(TAspect));
        }

        /// <summary>
        /// Get the ServiceType for an implementation type
        /// </summary>
        /// <param name="services"></param>
        /// <param name="implType"></param>
        /// <returns></returns>
        public static Type GetServiceType(this IServiceCollection services, Type implType)
        {
            Type retval = implType;

            ServiceDescriptor sd = services
                .Where(s => implType.IsAssignableFrom(s.ImplementationType))
                .Union( services.Where(s => implType.IsAssignableFrom(s.ImplementationInstance?.GetType())))
                .FirstOrDefault();

            if (sd != null)
            {
                retval = sd.ServiceType;
            }

            return retval;
        }

        /// <summary>
        /// Check if the type or any properties or methods have IAspect attributes
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns>true if the type, properties, or methods have an IAspect attribute</returns>
        private static bool HasAspects(Type serviceType)
        {
            if (!serviceType.IsInterface)
                return false;

            foreach (var item in serviceType.GetCustomAttributesData())
            {
                if (typeof(IAspect).IsAssignableFrom(item.AttributeType))
                {
                    return true;
                }
            }

            foreach (var propertyInfo in serviceType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                foreach (var item in propertyInfo.GetCustomAttributesData())
                {
                    if (typeof(IAspect).IsAssignableFrom(item.AttributeType))
                    {
                        return true;
                    }
                }
            }

            foreach (var methodInfo in serviceType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                foreach (var item in methodInfo.GetCustomAttributesData())
                {
                    if (typeof(IAspect).IsAssignableFrom(item.AttributeType))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        ///  Create proxy object
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="implementationType"></param>
        /// <returns>AspectAttributeProxy object</returns>
        private static object GetAspectAttributeProxy(ServiceProvider provider, IServiceCollection services, Type serviceType, Type implementationType)
        {
            Type genericProxy = typeof(AspectAttributeProxy<>).MakeGenericType(serviceType);
            object proxyCreator = Activator.CreateInstance(genericProxy);
            object impl = provider.GetService(serviceType);
            object proxy = proxyCreator.GetType().GetMethod(nameof(AspectAttributeProxy<object>.Create)).Invoke(proxyCreator, new object[] { services, impl });
            return proxy;
        }

        /// <summary>
        ///  Create proxy object
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="serviceType"></param>
        /// <param name="aspectType"></param>
        /// <returns>AspectProxy object</returns>
        private static object GetAspectProxy(ServiceProvider provider, IServiceCollection services,  Type serviceType, Type aspectType)
        {
            Type genericProxy = typeof(AspectProxy<>).MakeGenericType(serviceType);
            object proxyCreator = Activator.CreateInstance(genericProxy);
            object serviceImpl = provider.GetService(serviceType);
            object aspectImpl = Activator.CreateInstance(aspectType);
            object proxy = proxyCreator.GetType().GetMethod(nameof(AspectProxy<object>.Create)).Invoke(proxyCreator, new object[] { services, serviceImpl, aspectImpl });
            return proxy;
        }

    }
}
