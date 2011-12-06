using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FireWorkflow.Net.Engine;
using FireWorkflow.Net.Engine.Impl;
using FireWorkflow.Net.Engine.Taskinstance;

namespace WebDemo.Example.GoodsDeliverProcess.WorkflowExtension
{
    public class SMSHandler : IApplicationHandler
    {
        #region IApplicationHandler 成员

        public void execute(ITaskInstance taskInstance)
        {
        	IProcessInstance processInstance = TaskInstanceHelper.getAliveProcessInstance(taskInstance);
            String goodsName = (String)ProcessInstanceHelper.getProcessInstanceVariable(processInstance,"goodsName");
            String customerName = (String)ProcessInstanceHelper.getProcessInstanceVariable(processInstance,"customerName");
            //这里为模拟。在服务器上弹出消息框。
            System.Windows.Forms.MessageBox.Show("FireflowExample模拟调用后台程序：" + customerName + "你好，你购买的" + goodsName + "即将送货，请注意查收。");
        }

        #endregion
    }
}
