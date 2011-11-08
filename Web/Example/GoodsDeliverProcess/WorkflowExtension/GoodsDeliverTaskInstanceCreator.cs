using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FireWorkflow.Net.Engine;
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

            String sn = (String)processInstance.getProcessInstanceVariable("sn");
            taskInst.Sn = sn;

            String customerName = (String)processInstance.getProcessInstanceVariable("customerName");
            taskInst.CustomerName = customerName;

            String goodsName = (String)processInstance.getProcessInstanceVariable("goodsName");
            taskInst.GoodsName = goodsName;

            long quantity = (long)processInstance.getProcessInstanceVariable("quantity");
            taskInst.Quantity = quantity;

            //taskInst.BizInfo=

            return taskInst;
        }
    }
}
