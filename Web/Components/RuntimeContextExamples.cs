using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using FireWorkflow.Net.Engine;

namespace WebDemo.Components
{
    public class RuntimeContextExamples
    {
        public static RuntimeContext GetRuntimeContext()
        {
            return  RuntimeContextFactory.getRuntimeContext();
        }
    }
}
