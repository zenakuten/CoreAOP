namespace CoreAOP
{
    using System.Reflection;
    using Microsoft.Extensions.DependencyInjection;

    public class AspectProxy<T> : DispatchProxy
    {
        private IAspect _aspect;
        private T _service;
        public T Create(IServiceCollection services, T service, IAspect aspect)
        {
            aspect.OnCreate(services.GetServiceType(service.GetType()));

            var proxy = Create<T, AspectProxy<T>>();
            (proxy as AspectProxy<T>)._service = service;
            (proxy as AspectProxy<T>)._aspect = aspect;
            return proxy;
        }

        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            object retval = null;
            _aspect.OnEnter(targetMethod, args);

            try
            {
                retval = targetMethod.Invoke(_service, args);
            }
            catch (TargetInvocationException ex)
            {
                _aspect.OnException(targetMethod, ex.InnerException);
                throw (ex.InnerException);
            }

            retval = _aspect.OnExit(targetMethod, args, retval);
            return retval;
        }
    }
}
