/*
 * Copyright 2003-2008 非也
 * All rights reserved. 
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation。
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see http://www.gnu.org/licenses. *
 * @author 非也,nychen2000@163.com
 * @Revision to .NET 无忧 lwz0721@gmail.com 2010-02
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FireWorkflow.Net.Model;
using FireWorkflow.Net.Engine.Impl;

namespace FireWorkflow.Net.Engine.Taskinstance
{
    public class DefaultToolTaskInstanceRunner : ITaskInstanceRunner
    {

        public void run(IWorkflowSession currentSession, RuntimeContext runtimeContext, IProcessInstance processInstance, ITaskInstance taskInstance)// throws EngineException, KernelException 
        {
            if (taskInstance.TaskType != TaskTypeEnum.TOOL)
            {
                throw new EngineException(processInstance, taskInstance.Activity,
                        "DefaultToolTaskInstanceRunner：TaskInstance的任务类型错误，只能为TOOL类型");
            }
            Task task = taskInstance.Task;
            if (task == null)
            {
                WorkflowProcess process = taskInstance.WorkflowProcess;
                throw new EngineException(taskInstance.ProcessInstanceId, process, taskInstance.TaskId,
                        "The Task is null,can NOT start the taskinstance,");
            }
            if (((ToolTask)task).Application == null || ((ToolTask)task).Application.Handler == null)
            {
                WorkflowProcess process = taskInstance.WorkflowProcess;
                throw new EngineException(taskInstance.ProcessInstanceId, process, taskInstance.TaskId,
                        "The task.Application is null or task.Application.Handler is null,can NOT start the taskinstance,");
            }

            Object obj = runtimeContext.getBeanByName(((ToolTask)task).Application.Handler);

            if (obj == null || !(obj is IApplicationHandler))
            {
                WorkflowProcess process = taskInstance.WorkflowProcess;
                throw new EngineException(taskInstance.ProcessInstanceId, process, taskInstance.TaskId,
                        "Run tool task instance error! Not found the instance of " + ((ToolTask)task).Application.Handler + " or the instance not implements IApplicationHandler");
            }

            try
            {
                ((IApplicationHandler)obj).execute(taskInstance);
            }
            catch (Exception )
            {//TODO wmj2003 对tool类型的task抛出的错误应该怎么处理？ 这个时候引擎会如何？整个流程是否还可以继续？
                throw new EngineException(processInstance, taskInstance.Activity,
                        "DefaultToolTaskInstanceRunner：TaskInstance的任务执行失败！");
            }

            ITaskInstanceManager taskInstanceManager = runtimeContext.TaskInstanceManager;
            taskInstanceManager.completeTaskInstance(currentSession, processInstance, taskInstance, null);
        }
    }
}
