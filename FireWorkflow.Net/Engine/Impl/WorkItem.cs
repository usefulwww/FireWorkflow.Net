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
using FireWorkflow.Net.Engine;
using FireWorkflow.Net.Engine.Taskinstance;
using FireWorkflow.Net.Kernel;
using FireWorkflow.Net.Model;
using FireWorkflow.Net.Model.Net;

namespace FireWorkflow.Net.Engine.Impl
{

    [Serializable]
    public class WorkItem : IWorkItem, IRuntimeContextAware, IWorkflowSessionAware
    {
        public String ActorId { get; set; }
        public String Id { get; set; }
        public WorkItemEnum State { get; set; }
        public DateTime CreatedTime { get; set; }

        /// <summary>签收时间</summary>
        public DateTime? ClaimedTime { get; set; }
        /// <summary>结束时间</summary>
        public DateTime? EndTime { get; set; }
        public String Comments { get; set; }
        public ITaskInstance TaskInstance { get; set; }

        public string Name { get { return TaskInstance.Name; } }//lwz 2010-3-3 add
        public string DisplayName { get { return TaskInstance.DisplayName; } }//lwz 2010-3-3 add
        public String ProcessInstanceId { get { return TaskInstance.ProcessInstanceId; } }//lwz 2010-3-3 add
        public String BizInfo { get { return TaskInstance.BizInfo; } }//lwz 2010-3-3 add
        /// <summary>返回对应的流程的Id</summary>
        public String ProcessId { get { return TaskInstance.ProcessId; } }

        /// <summary>返回流程的版本</summary>
        public Int32 Version { get { return TaskInstance.Version; } }

        /// <summary>added by wangmj 20090922 供springjdbc实现类使用</summary>
        public String TaskInstanceId { get; set; }

        protected RuntimeContext _runtimeContext;
        public RuntimeContext RuntimeContext
        {
            get { return _runtimeContext; }
            set
            {
                _runtimeContext = value;
                if (this.TaskInstance != null)
                {
                    ((IRuntimeContextAware)TaskInstance).RuntimeContext = _runtimeContext;
                }
            }
        }

        protected IWorkflowSession _workflowSession = null;
        public IWorkflowSession CurrentWorkflowSession
        {
            get { return this._workflowSession; }
            set
            {
                this._workflowSession = value;
                if (this.TaskInstance != null)
                {
                    ((IWorkflowSessionAware)TaskInstance).CurrentWorkflowSession = this._workflowSession;
                }
            }
        }

        public WorkItem()
        {
        }

        public WorkItem(TaskInstance taskInstance)
        {
            this.TaskInstance = taskInstance;
        }

        public WorkItem(WorkItemEnum state, DateTime createdTime, DateTime? signedTm,
                DateTime? endTime, String comments, TaskInstance taskInstance)
        {
            this.State = state;
            this.CreatedTime = createdTime;
            this.ClaimedTime = signedTm;
            this.EndTime = endTime;
            this.Comments = comments;
            this.TaskInstance = taskInstance;
        }

        public IWorkItem withdraw()
        {
            if (this._workflowSession == null)
            {
                new EngineException(this.TaskInstance.ProcessInstanceId,
                        this.TaskInstance.WorkflowProcess, this.TaskInstance.TaskId,
                        "The current workflow session is null.");
            }
            if (this.RuntimeContext == null)
            {
                new EngineException(this.TaskInstance.ProcessInstanceId,
                        this.TaskInstance.WorkflowProcess, this.TaskInstance.TaskId,
                        "The current runtime context is null.");
            }
            ITaskInstanceManager taskInstanceMgr = this.RuntimeContext.TaskInstanceManager;
            return taskInstanceMgr.withdrawWorkItem(this);
        }


        public void reject()
        {
            reject(this.Comments);
        }

        public void reject(String comments)
        {
            if (this._workflowSession == null)
            {
                new EngineException(this.TaskInstance.ProcessInstanceId,
                        this.TaskInstance.WorkflowProcess, this.TaskInstance.TaskId,
                        "The current workflow session is null.");
            }
            if (this.RuntimeContext == null)
            {
                new EngineException(this.TaskInstance.ProcessInstanceId,
                        this.TaskInstance.WorkflowProcess, this.TaskInstance.TaskId,
                        "The current runtime context is null.");
            }
            ITaskInstanceManager taskInstanceMgr = this.RuntimeContext.TaskInstanceManager;
            taskInstanceMgr.rejectWorkItem(this, comments);
        }

        /// <summary>
        /// <para>结束当前WorkItem；并由工作流引擎根据流程定义决定下一步操作。引擎的执行规则如下</para>
        /// <para>1、工作流引擎首先判断该WorkItem对应的TaskInstance是否可以结束。</para>
        /// <para>   如果TaskInstance的assignment策略为ANY，或者，assignment策略为ALL且它所有的WorkItem都已经完成</para>
        /// <para>   则结束当前TaskInstance</para>
        /// <para>2、判断TaskInstance对应的ActivityInstance是否可以结束。如果ActivityInstance的complete strategy为ANY，</para>
        /// <para>   或者，complete strategy为ALL且他的所有的TaskInstance都已经结束，则结束当前ActivityInstance</para>
        /// <para>3、根据流程定义，启动下一个Activity，并创建相关的TaskInstance和WorkItem</para>
        /// </summary>
        public void complete()
        {
            complete(null, this.Comments);
        }
        /// <summary>
        /// <para>结束当前WorkItem；并由工作流引擎根据流程定义决定下一步操作。引擎的执行规则如下</para>
        /// <para>1、工作流引擎首先判断该WorkItem对应的TaskInstance是否可以结束。</para>
        /// <para>   如果TaskInstance的assignment策略为ANY，或者，assignment策略为ALL且它所有的WorkItem都已经完成</para>
        /// <para>   则结束当前TaskInstance</para>
        /// <para>2、判断TaskInstance对应的ActivityInstance是否可以结束。如果ActivityInstance的complete strategy为ANY，</para>
        /// <para>   或者，complete strategy为ALL且他的所有的TaskInstance都已经结束，则结束当前ActivityInstance</para>
        /// <para>3、根据流程定义，启动下一个Activity，并创建相关的TaskInstance和WorkItem</para>
        /// </summary>
        /// <param name="comments">备注信息</param>
        public void complete(String comments)
        {
            complete(null, comments);
        }


        /// <summary>
        /// <para>结束当前WorkItem；并由工作流引擎根据流程定义决定下一步操作。引擎的执行规则如下</para>
        /// <para>1、工作流引擎首先判断该WorkItem对应的TaskInstance是否可以结束。</para>
        /// <para>   如果TaskInstance的assignment策略为ANY，或者，assignment策略为ALL且它所有的WorkItem都已经完成</para>
        /// <para>   则结束当前TaskInstance</para>
        /// <para>2、判断TaskInstance对应的ActivityInstance是否可以结束。如果ActivityInstance的complete strategy为ANY，</para>
        /// <para>   或者，complete strategy为ALL且他的所有的TaskInstance都已经结束，则结束当前ActivityInstance</para>
        /// <para>3、根据流程定义，启动下一个Activity，并创建相关的TaskInstance和WorkItem</para>
        /// </summary>
        /// <param name="dynamicAssignmentHandler">通过动态分配句柄指定下一个环节的操作者。</param>
        /// <param name="comments">备注信息</param>
        public void complete(DynamicAssignmentHandler dynamicAssignmentHandler, String comments)
        {
            if (this._workflowSession == null)
            {
                new EngineException(this.TaskInstance.ProcessInstanceId,
                        this.TaskInstance.WorkflowProcess, this.TaskInstance.TaskId,
                        "The current workflow session is null.");
            }
            if (this.RuntimeContext == null)
            {
                new EngineException(this.TaskInstance.ProcessInstanceId,
                        this.TaskInstance.WorkflowProcess, this.TaskInstance.TaskId,
                        "The current runtime context is null.");
            }

            if (this.State != WorkItemEnum.RUNNING)
            {
                TaskInstance thisTaskInst = (TaskInstance)this.TaskInstance;
                //			System.out.println("WorkItem的当前状态为"+this.State+"，不可以执行complete操作。");
                throw new EngineException(thisTaskInst.ProcessInstanceId, thisTaskInst.WorkflowProcess, thisTaskInst.TaskId,
                        "Complete work item failed . The state of the work item [id=" + this.Id + "] is " + this.State);
            }

            if (dynamicAssignmentHandler != null)
            {
                this._workflowSession.setDynamicAssignmentHandler(dynamicAssignmentHandler);
            }
            ITaskInstanceManager taskInstanceManager = this.RuntimeContext.TaskInstanceManager;
            taskInstanceManager.completeWorkItem(this, null, comments);
        }

        public IWorkItem reassignTo(String actorId)
        {
            return reassignTo(actorId, this.Comments);
        }

        public IWorkItem reassignTo(String actorId, String comments)
        {
            if (this._workflowSession == null)
            {
                new EngineException(this.TaskInstance.ProcessInstanceId,
                        this.TaskInstance.WorkflowProcess, this.TaskInstance.TaskId,
                        "The current workflow session is null.");
            }
            if (this.RuntimeContext == null)
            {
                new EngineException(this.TaskInstance.ProcessInstanceId,
                        this.TaskInstance.WorkflowProcess, this.TaskInstance.TaskId,
                        "The current runtime context is null.");
            }

            ITaskInstanceManager manager = this.RuntimeContext.TaskInstanceManager;
            return manager.reasignWorkItemTo(this, actorId, comments);
        }

        /// <summary>签收</summary>
        /// <returns></returns>
        public IWorkItem claim()
        {
            if (this._workflowSession == null)
            {
                new EngineException(this.TaskInstance.ProcessInstanceId,
                        this.TaskInstance.WorkflowProcess, this.TaskInstance.TaskId,
                        "The current workflow session is null.");
            }
            if (this.RuntimeContext == null)
            {
                new EngineException(this.TaskInstance.ProcessInstanceId,
                        this.TaskInstance.WorkflowProcess, this.TaskInstance.TaskId,
                        "The current runtime context is null.");
            }

            ITaskInstanceManager taskInstanceMgr = RuntimeContext.TaskInstanceManager;
            IWorkItem newWorkItem = taskInstanceMgr.claimWorkItem(this.Id, this.TaskInstance.Id);

            if (newWorkItem != null)
            {
                this.State = newWorkItem.State;
                this.ClaimedTime = newWorkItem.ClaimedTime;

                ((IRuntimeContextAware)newWorkItem).RuntimeContext = this.RuntimeContext;
                ((IWorkflowSessionAware)newWorkItem).CurrentWorkflowSession = this._workflowSession;
            }
            else
            {
                this.State = WorkItemEnum.CANCELED;
            }

            return newWorkItem;
        }

        public void jumpTo(String activityId)
        {
            jumpTo(activityId, null, this.Comments);
        }

        public void jumpTo(String activityId, String comments)
        {
            jumpTo(activityId, null, comments);
        }

        public void jumpTo(String targetActivityId, DynamicAssignmentHandler dynamicAssignmentHandler, String comments)
        {
            if (this._workflowSession == null)
            {
                new EngineException(this.TaskInstance.ProcessInstanceId,
                        this.TaskInstance.WorkflowProcess, this.TaskInstance.TaskId,
                        "The current workflow session is null.");
            }
            if (this.RuntimeContext == null)
            {
                new EngineException(this.TaskInstance.ProcessInstanceId,
                        this.TaskInstance.WorkflowProcess, this.TaskInstance.TaskId,
                        "The current runtime context is null.");
            }
            if (dynamicAssignmentHandler != null)
            {
                this._workflowSession.setDynamicAssignmentHandler(dynamicAssignmentHandler);
            }
            ITaskInstanceManager taskInstanceManager = this.RuntimeContext.TaskInstanceManager;
            taskInstanceManager.completeWorkItemAndJumpTo(this, targetActivityId, comments);
        }

        public void jumpToEx(String targetActivityId, DynamicAssignmentHandler dynamicAssignmentHandler, String comments)
        {
            if (this._workflowSession == null)
            {
                new EngineException(this.TaskInstance.ProcessInstanceId,
                        this.TaskInstance.WorkflowProcess, this.TaskInstance.TaskId,
                        "The current workflow session is null.");
            }
            if (this.RuntimeContext == null)
            {
                new EngineException(this.TaskInstance.ProcessInstanceId,
                        this.TaskInstance.WorkflowProcess, this.TaskInstance.TaskId,
                        "The current runtime context is null.");
            }
            if (dynamicAssignmentHandler != null)
            {
                this._workflowSession.setDynamicAssignmentHandler(dynamicAssignmentHandler);
            }
            ITaskInstanceManager taskInstanceManager = this.RuntimeContext.TaskInstanceManager;
            taskInstanceManager.completeWorkItemAndJumpToEx(this, targetActivityId, comments);
        }


    }
}
