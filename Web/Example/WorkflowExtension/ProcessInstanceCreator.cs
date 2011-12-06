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

        public void assign(ITaskInstance taskInstance, string performerName)
        {
            //ITaskInstance taskInstance = (TaskInstance)asignable;
            IProcessInstance processInstance = TaskInstanceHelper.getAliveProcessInstance(taskInstance);

            String creator = processInstance.CreatorId;

            if (creator == null)
            {
            	throw new EngineException(processInstance, TaskInstanceHelper.getTask(taskInstance), "分配工单错误，流程创建者Id为null");
            }

            TaskInstanceHelper.assignToActor(taskInstance,creator);
        }

    }
}
