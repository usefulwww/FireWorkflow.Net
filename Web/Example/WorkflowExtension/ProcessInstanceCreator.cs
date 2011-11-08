using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FireWorkflow.Net.Engine;
using FireWorkflow.Net.Engine.Impl;
using FireWorkflow.Net.Engine.Taskinstance;
using FireWorkflow.Net.Kernel;

namespace WebDemo.Example.WorkflowExtension
{
    public class ProcessInstanceCreator : IAssignmentHandler
    {

        public void assign(IAssignable asignable, string performerName)
        {
            TaskInstance taskInstance = (TaskInstance)asignable;
            IProcessInstance processInstance = taskInstance.AliveProcessInstance;

            String creator = processInstance.CreatorId;

            if (creator == null)
            {
                throw new EngineException(processInstance, taskInstance.Task, "分配工单错误，流程创建者Id为null");
            }

            asignable.assignToActor(creator);
        }

    }
}
