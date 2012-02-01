using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;

using FireWorkflow.Net.Engine;
using FireWorkflow.Net.Engine.Impl;
using FireWorkflow.Net.Engine.Taskinstance;
using FireWorkflow.Net.Model;
using FireWorkflow.Net.Model.Net;

namespace WebDemo.Example.GoodsDeliverProcess.WorkflowExtension
{
    public class GoodsDeliverTaskInstanceCreator : ITaskInstanceCreator
    {

        public ITaskInstance createTaskInstance(IWorkflowSession currentSession,
                RuntimeContext runtimeContxt, IProcessInstance processInstance,
                Task task, Activity activity)
        {
            GoodsDeliverTaskInstance taskInst = new GoodsDeliverTaskInstance();

            String sn = (String)ProcessInstanceHelper.getProcessInstanceVariable(processInstance,"sn");
            taskInst.Sn = sn;

            String customerName = (String)ProcessInstanceHelper.getProcessInstanceVariable(processInstance,"customerName");
            taskInst.CustomerName = customerName;

            String goodsName = (String)ProcessInstanceHelper.getProcessInstanceVariable(processInstance,"goodsName");
            taskInst.GoodsName = goodsName;

            long quantity = (long)ProcessInstanceHelper.getProcessInstanceVariable(processInstance,"quantity");
            taskInst.Quantity = quantity;

            //taskInst.BizInfo=

            return taskInst;
        }
    }
}
