namespace CoreAOP
{
    using System;
    using System.Reflection;

    /// <summary>
    ///  IAspect interface
    /// </summary>
    public interface IAspect
    {
        /// <summary>
        /// Called during object creation
        /// </summary>
        /// <param name="createdType">type being created</param>
        void OnCreate(Type createdType);

        /// <summary>
        /// Called on method entry 
        /// </summary>
        /// <param name="mi">method being called</param>
        void OnEnter(MethodInfo mi, object[] args);

        /// <summary>
        /// Called on method exit 
        /// </summary>
        /// <param name="mi">method being called</param>
        object OnExit(MethodInfo mi, object[] args, object retval);

        /// <summary>
        ///  Called on method exception 
        /// </summary>
        /// <param name="mi">method being called</param>
        /// <param name="ex">exception thrown</param>
        /// <returns>If true, the exception is considered handled, if false the exception will be thrown</returns>
        void OnException(MethodInfo mi, Exception ex);
    }
}
