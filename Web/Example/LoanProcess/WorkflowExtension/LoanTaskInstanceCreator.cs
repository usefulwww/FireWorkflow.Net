using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FireWorkflow.Net.Engine;
using FireWorkflow.Net.Engine.Taskinstance;
using FireWorkflow.Net.Model;
using FireWorkflow.Net.Model.Net;

namespace WebDemo.Example.LoanProcess.WorkflowExtension
{
    public class LoanTaskInstanceCreator : ITaskInstanceCreator
    {
        #region ITaskInstanceCreator 成员

        public ITaskInstance createTaskInstance(IWorkflowSession currentSession, RuntimeContext runtimeContxt, IProcessInstance processInstance, Task task, Activity activity)
        {
            LoanTaskInstance taskInstance = new LoanTaskInstance();
            taskInstance.Sn=(String)processInstance.getProcessInstanceVariable("sn");
            taskInstance.ApplicantName=(String)processInstance.getProcessInstanceVariable("applicantName");
            taskInstance.LoanValue=(int)processInstance.getProcessInstanceVariable("loanValue");
            taskInstance.RiskFlag=(Boolean)processInstance.getProcessInstanceVariable("RiskFlag");
            taskInstance.Decision=(Boolean)processInstance.getProcessInstanceVariable("Decision");

            return taskInstance;
        }

        #endregion
    }
}
