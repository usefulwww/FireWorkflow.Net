using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FireWorkflow.Net.Engine;
using FireWorkflow.Net.Engine.Taskinstance;
using FireWorkflow.Net.Kernel;

//import org.fireflow.engine.IWorkItem;
//import org.fireflow.engine.taskinstance.IAssignable;
//import org.fireflow.engine.taskinstance.IAssignmentHandler;
//import org.fireflow.kernel.KernelException;
//import org.fireflow.security.persistence.User;
//import org.springframework.security.context.SecurityContextHolder;

namespace WebDemo.Example.WorkflowExtension
{
    /// <summary>
    /// 将系统当前用户设置为workitem的操作员。
    /// </summary>
    public class CurrentUserAssignmentHandler : IAssignmentHandler
    {
        public void assign(IAssignable arg0, String arg1)  {

            HttpContext context = HttpContext.Current;
            if (context != null)
            {
                //将当前用户设置为操作员
                IWorkItem wi = arg0.assignToActor(context.User.Identity.Name);

                //假设在目前的两个example中都规定：首个环节信息录入后，立即提交到下一个环节。
                //则需要对新创建的WorkItem执行 claim()和complete()操作。
                wi.claim();
                wi.complete("系统自动提交任务。");
            }
	}
    }
}
