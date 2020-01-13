﻿namespace CoreAOP
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Base class for aspect attributes
    /// </summary>
    public abstract class AspectAttribute : Attribute, IAspect
    {
        public virtual void OnCreate(Type createdType)
        {
        }

        public virtual void OnEnter(MethodInfo mi, object[] args)
        {
        }

        public virtual void OnException(MethodInfo mi, Exception ex)
        {
        }

        public virtual object OnExit(MethodInfo mi, object[] args, object retval)
        {
            return retval;
        }
    }
}
