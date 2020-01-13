using System;
using System.Collections.Generic;
using System.Text;

namespace CoreAOP.UnitTests
{
    using System.Reflection;
    public class AlwaysFalseOnExitAttribute : AspectAttribute
    {
        public override object OnExit(MethodInfo mi, object[] args, object retval)
        {
            if(retval.GetType() == typeof(bool))
                return false;

            return retval;
        }
    }
}
