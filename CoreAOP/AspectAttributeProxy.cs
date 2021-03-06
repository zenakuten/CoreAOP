﻿namespace CoreAOP
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Proxy class used to proxy calls to service targets, invoking IAspects as needed
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AspectAttributeProxy<T> : DispatchProxy 
    {
        private T _service;
        private Dictionary<MethodInfo, List<IAspect>> _aspectCache = new Dictionary<MethodInfo, List<IAspect>>();

        /// <summary>
        /// Create an AspectProxy for an instance of T
        /// </summary>
        /// <param name="service"></param>
        /// <returns>instance of T</returns>
        public T Create(IServiceCollection services, T service)
        {
            //get all IAspects assigned to the type
            var typeAttributes = new List<IAspect>();
            foreach (var serviceInterface in service.GetType().GetInterfaces())
            {
                var serviceAttributes = serviceInterface.GetCustomAttributes(typeof(IAspect), true);
                serviceAttributes.ToList().ForEach(sa => { if(!typeAttributes.Contains(sa as IAspect)) typeAttributes.Add(sa as IAspect);});
            }

            //cache all aspects for each method so we don't have look them up during invoke
            Dictionary<MethodInfo, List<IAspect>> cache = new Dictionary<MethodInfo, List<IAspect>>();
            foreach (var serviceInterface in service.GetType().GetInterfaces())
            {
                var methods = serviceInterface.GetMethods();
                foreach (var methodInfo in methods)
                {
                    //merge all attributes declared at the type level with 
                    //attributes on the method
                    var methodAttributes = methodInfo.GetCustomAttributes(typeof(IAspect), true).ToList();
                    typeAttributes.Except(methodAttributes).ToList().ForEach(ta => methodAttributes.Add(ta));

                    if (methodAttributes.Any())
                    {
                        List<IAspect> aspects = new List<IAspect>();
                        methodAttributes.ToList().ForEach(ma => aspects.Add(ma as IAspect));
                        cache[methodInfo] = aspects;
                    }
                }
            }

            //call OnCreate for this type 
            typeAttributes.ForEach(aspect => aspect.OnCreate(services.GetServiceType(service.GetType())));

            //create the proxy using dispatchproxy, assign service and cache to it
            T proxy = Create<T, AspectAttributeProxy<T>>();
            (proxy as AspectAttributeProxy<T>)._service = service;
            (proxy as AspectAttributeProxy<T>)._aspectCache = cache;
            return proxy;
        }

        /// <summary>
        /// Call IAspect methods during invoke of proxied method
        /// </summary>
        /// <param name="targetMethod"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            object retval = null;

            //get cached list of aspects on the type, call their enter methods
            var aspects = _aspectCache[targetMethod];
            aspects.ForEach(aspect => args = aspect.OnEnter(targetMethod, args));

            try
            {
                retval = targetMethod.Invoke(_service, args);
            }
            catch (TargetInvocationException ex)
            {
                aspects.ForEach(aspect => aspect.OnException(targetMethod, ex.InnerException));
                throw (ex.InnerException);
            }

            aspects.ForEach( aspect => retval = aspect.OnExit(targetMethod, args, retval));

            return retval;
        }
    }
}
