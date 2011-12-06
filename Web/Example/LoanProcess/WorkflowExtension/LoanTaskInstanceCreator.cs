using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using FireWorkflow.Net.Engine;
using FireWorkflow.Net.Engine.Impl;
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
            taskInstance.Sn=(String)ProcessInstanceHelper.getProcessInstanceVariable(processInstance,"sn");
            taskInstance.ApplicantName=(String)ProcessInstanceHelper.getProcessInstanceVariable(processInstance,"applicantName");
            taskInstance.LoanValue=(int)ProcessInstanceHelper.getProcessInstanceVariable(processInstance,"loanValue");
            taskInstance.RiskFlag=(Boolean)ProcessInstanceHelper.getProcessInstanceVariable(processInstance,"RiskFlag");
            taskInstance.Decision=(Boolean)ProcessInstanceHelper.getProcessInstanceVariable(processInstance,"Decision");

            return taskInstance;
        }

        #endregion
    }
}
