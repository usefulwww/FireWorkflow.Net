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
using System.Text;
using FireWorkflow.Net.Engine.Event;

namespace FireWorkflow.Net.Engine.Taskinstance
{
    /// <summary>
    /// 
    /// </summary>
    public class DefaultTaskInstanceEventListener : ITaskInstanceEventListener
    {
        /// <summary>
        /// 响应任务实例的事件。通过e.getEventType区分事件的类型。
        /// </summary>
        /// <param name="e">任务实例的事件。</param>
        public void onTaskInstanceEventFired(TaskInstanceEvent e)// throws EngineException 
        {
            IWorkflowSession session = e.WorkflowSession;
            IProcessInstance proceInst = e.ProcessInstance;
            ITaskInstance taskInst = (ITaskInstance)e.Source;
            IWorkItem wi = e.WorkItem;
            switch (e.EventType)
            {
                case TaskInstanceEventEnum.BEFORE_TASK_INSTANCE_START:
                    beforeTaskInstanceStart(session, proceInst, taskInst);
                    break;
                case TaskInstanceEventEnum.AFTER_WORKITEM_CREATED:
                    afterWorkItemCreated(session, proceInst, taskInst, wi);
                    break;
                case TaskInstanceEventEnum.AFTER_WORKITEM_COMPLETE:
                    afterWorkItemComplete(session, proceInst, taskInst, wi);
                    break;
                case TaskInstanceEventEnum.AFTER_TASK_INSTANCE_COMPLETE:
                    afterTaskInstanceCompleted(session, proceInst, taskInst);
                    break;
                default:
                    break;
            }
        }

        protected void beforeTaskInstanceStart(IWorkflowSession currentSession, IProcessInstance processInstance, ITaskInstance taskInstance)//throws EngineException
        {

        }
        protected void afterTaskInstanceCompleted(IWorkflowSession currentSession,
                IProcessInstance processInstance, ITaskInstance taskInstance)//throws EngineException
        {

        }
        protected void afterWorkItemCreated(IWorkflowSession currentSession, IProcessInstance processInstance, 
            ITaskInstance taskInstance, IWorkItem workItem)//throws EngineException
        {

        }

        protected void afterWorkItemComplete(IWorkflowSession currentSession, IProcessInstance processInstance,
            ITaskInstance taskInstance, IWorkItem workItem)
        {
            //    	System.out.println("---------------------------------after workitem complete!!!!!!!!!!!!!!");
        }
    }
}
