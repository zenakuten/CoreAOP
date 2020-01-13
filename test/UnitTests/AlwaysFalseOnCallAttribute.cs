using System;
using System.Collections.Generic;
using System.Text;

namespace CoreAOP.UnitTests
{
    using System.Reflection;
    public class AlwaysFalseOnCallAttribute : AspectAttribute
    {
        public override object[] OnEnter(MethodInfo mi, object[] args)
        {
            if(args.Length > 0 && args[0].GetType() == typeof(bool))
            {
                args[0] = false;
            }

            return args;
        }
    }
}
