﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace CoreAOP.UnitTests
{
    public class TestAspect : IAspect
    {
        public static IAspect handler = null;
        public void OnCreate(Type createdType)
        {
            handler.OnCreate(createdType);
        }

        public void OnEnter(MethodInfo mi)
        {
            handler.OnEnter(mi);
        }

        public void OnException(MethodInfo mi, Exception ex)
        {
            handler.OnException(mi, ex);
        }

        public void OnExit(MethodInfo mi)
        {
            handler.OnExit(mi);
        }
    }
}