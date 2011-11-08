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
using FireWorkflow.Net.Engine.Definition;
using FireWorkflow.Net.Engine.Persistence;
using FireWorkflow.Net.Engine.Taskinstance;
using FireWorkflow.Net.Model;
using FireWorkflow.Net.Model.Net;
using FireWorkflow.Net.Kernel;

namespace FireWorkflow.Net.Engine.Impl
{
    [Serializable]
    public class TaskInstance : ITaskInstance, IAssignable, IRuntimeContextAware, IWorkflowSessionAware
    {
        /// <summary>工作流总线</summary>
        public RuntimeContext RuntimeContext { get; set; }

        /// <summary>返回或设置任务实例的Id</summary>
        public String Id { get; set; }

        /// <summary>返回或设置对应的任务Id</summary>
        public String TaskId { get; set; }

        /// <summary>返回或设置任务Name</summary>
        public String Name { get; set; }

        /// <summary>返回或设置任务显示名</summary>
        public String DisplayName { get; set; }

        /// <summary>当前任务实例中的业务信息</summary>
        public virtual String BizInfo { get; set; }

        /// <summary>返回或设置对应的流程实例Id</summary>
        public String ProcessInstanceId { get; set; }

        /// <summary>返回或设置对应的流程的Id</summary>
        public String ProcessId { get; set; }

        /// <summary>返回或设置流程的版本</summary>
        public Int32 Version { get; set; }

        /// <summary>返回或设置任务实例创建的时间</summary>
        public DateTime? CreatedTime { get; set; }

        /// <summary>返回或设置任务实例启动的时间</summary>
        public DateTime? StartedTime { get; set; }

        /// <summary>返回或设置任务实例结束的时间</summary>
        public DateTime? EndTime { get; set; }

        /// <summary>返回或设置任务实例到期日期</summary>
        public DateTime? ExpiredTime { get; set; }

        /// <summary>返回或设置任务实例的状态，取值为：INITIALIZED(已初始化），STARTED(已启动),COMPLETED(已结束),CANCELD(被取消)</summary>
        public TaskInstanceStateEnum State { get; set; }

        /// <summary>返回或设置任务实例的分配策略，取值为 org.fireflow.model.Task.ALL或者org.fireflow.model.Task.ANY</summary>
        public FormTaskEnum AssignmentStrategy { get; set; }

        /// <summary>返回或设置任务实例所属的环节的Id</summary>
        public String ActivityId { get; set; }

        /// <summary>返回或设置任务类型，取值为TaskTypeEnum:FORM,TOOL,SUBFLOW,DUMMY</summary>
        public TaskTypeEnum TaskType { get; set; }

        /// <summary>当执行JumpTo和LoopTo操作时，返回或设置目标Activity 的Id</summary>
        public String TargetActivityId { get; set; }

        /// <summary>返回或设置</summary>
        public Int32 StepNumber { get; set; }

        /// <summary>返回或设置</summary>
        public Boolean Suspended { get; set; }

        //	private Set workItems = new HashSet(0);

        /// <summary>返回或设置</summary>
        public String FromActivityId { get; set; }

        /// <summary>返回或设置</summary>
        public Boolean CanBeWithdrawn { get; set; }

        public IWorkflowSession CurrentWorkflowSession { get; set; }

        private IProcessInstance processInsatance = null;

        public TaskInstance()
        {
            this.State = TaskInstanceStateEnum.INITIALIZED;
            this.Suspended = false;
            this.CanBeWithdrawn = true;
        }

        public TaskInstance(ProcessInstance workflowProcessInsatnce)
        {
            this.State = TaskInstanceStateEnum.INITIALIZED;
            this.Suspended = false;
            this.CanBeWithdrawn = true;
            this.processInsatance = workflowProcessInsatnce;
        }

        public Boolean IsSuspended()
        {
            return this.Suspended;
        }

        public IProcessInstance AliveProcessInstance
        {
            get
            {
                if (this.processInsatance == null)
                {
                    if (this.RuntimeContext != null)
                    {
                        IPersistenceService persistenceService = this.RuntimeContext.PersistenceService;
                        //this.processInsatance = persistenceService.FindAliveProcessInstanceById(this.ProcessInstanceId); //解决流程结束任务未完成无法继续问题。
                        this.processInsatance = persistenceService.FindProcessInstanceById(this.ProcessInstanceId);//获取存在的流程
                        
                    }
                }
                if (this.processInsatance != null)
                {
                    if (this.CurrentWorkflowSession != null)
                    {
                        ((IWorkflowSessionAware)this.processInsatance).CurrentWorkflowSession = this.CurrentWorkflowSession;
                    }
                    if (this.RuntimeContext != null)
                    {
                        ((IRuntimeContextAware)this.processInsatance).RuntimeContext = this.RuntimeContext;
                    }

                }
                return this.processInsatance;
            }
        }

        public Task Task// throws EngineException 
        {
            get
            {
                if (this.RuntimeContext == null) return null; //System.out.println("====Inside taskInstance this.RuntimeContext is null");
                IDefinitionService definitionService = this.RuntimeContext.DefinitionService;
                if (definitionService == null) return null;//System.out.println("====Inside taskInstance definitionService is null");
                WorkflowDefinition workflowDef = definitionService.GetWorkflowDefinitionByProcessIdAndVersionNumber(this.ProcessId, this.Version);
                if (workflowDef == null)
                {
                    return null;
                }
                return (Task)workflowDef.getWorkflowProcess().findWFElementById(this.TaskId);
            }
        }

        public Activity Activity
        {
            get
            {
                WorkflowDefinition workflowDef = this.RuntimeContext.DefinitionService.GetWorkflowDefinitionByProcessIdAndVersionNumber(this.ProcessId, this.Version);
                if (workflowDef == null)
                {
                    return null;
                }
                return (Activity)workflowDef.getWorkflowProcess().findWFElementById(this.ActivityId);
            }
        }

        public WorkflowProcess WorkflowProcess
        {
            get
            {
                WorkflowDefinition workflowDef = this.RuntimeContext.DefinitionService.GetWorkflowDefinitionByProcessIdAndVersionNumber(this.ProcessId, this.Version);
                if (workflowDef == null)
                {
                    return null;
                }
                return workflowDef.getWorkflowProcess();
            }
        }

        public IWorkItem assignToActor(String id)// throws EngineException, KernelException 
        {
            ITaskInstanceManager taskInstanceMgr = this.RuntimeContext.TaskInstanceManager;
            WorkItem wi = taskInstanceMgr.createWorkItem(this.CurrentWorkflowSession, this.AliveProcessInstance, this, id);
            return wi;
        }

        public List<IWorkItem> assignToActors(List<String> ids)// throws EngineException, KernelException 
        {
            //task应该有一个标志(asignToEveryone)，表明asign的规则
            List<IWorkItem> workItemList = new List<IWorkItem>();
            for (int i = 0; ids != null && i < ids.Count; i++)
            {
                ITaskInstanceManager taskInstanceMgr = this.RuntimeContext.TaskInstanceManager;
                WorkItem wi = taskInstanceMgr.createWorkItem(this.CurrentWorkflowSession, this.AliveProcessInstance, this, ids[i]);
                wi.CurrentWorkflowSession = this.CurrentWorkflowSession;
                workItemList.Add(wi);
            }
            return workItemList;
        }


        public /*final*/ void start()
        {
            ITaskInstanceManager taskInstanceMgr = this.RuntimeContext.TaskInstanceManager;
            taskInstanceMgr.startTaskInstance(this.CurrentWorkflowSession, this.AliveProcessInstance, this);
            //        taskInstanceMgr.startTaskInstance(this);
        }

        public void complete(IActivityInstance targetActivityInstance)
        {
            ITaskInstanceManager taskInstanceMgr = this.RuntimeContext.TaskInstanceManager;
            taskInstanceMgr.completeTaskInstance(this.CurrentWorkflowSession, this.AliveProcessInstance, this, targetActivityInstance);
            //        taskInstanceMgr.completeTaskInstance(this, targetActivityInstance);
        }

        public void suspend()
        {
            if (this.State == TaskInstanceStateEnum.COMPLETED || this.State == TaskInstanceStateEnum.CANCELED)
            {
                throw new EngineException(this.AliveProcessInstance, this.Task, "The task instance can not be suspended,the state of this task instance is " + this.State);
            }
            if (this.IsSuspended())
            {
                return;
            }
            this.Suspended = true;
            IPersistenceService persistenceService = this.RuntimeContext.PersistenceService;
            persistenceService.SaveOrUpdateTaskInstance(this);
        }

        public void restore()
        {
            if (this.State == TaskInstanceStateEnum.COMPLETED || this.State == TaskInstanceStateEnum.CANCELED)
            {
                throw new EngineException(this.AliveProcessInstance, this.Task, "The task instance can not be restored,the state of this task instance is " + this.State);
            }
            if (!this.IsSuspended())
            {
                return;
            }
            this.Suspended = false;
            IPersistenceService persistenceService = this.RuntimeContext.PersistenceService;
            persistenceService.SaveOrUpdateTaskInstance(this);
        }


        /* (non-Javadoc) 
          * @see org.fireflow.engine.ITaskInstance#abort() 
          */
        public void abort()
        {
            abort(null);

        }

        /* (non-Javadoc) 
         * @see org.fireflow.engine.ITaskInstance#abort(java.lang.String) 
         */
        public void abort(String targetActivityId)
        {
            abort(targetActivityId, null);

        }

        /* (non-Javadoc) 
         * @see org.fireflow.engine.ITaskInstance#abort(java.lang.String, org.fireflow.engine.taskinstance.DynamicAssignmentHandler) 
         */
        public void abort(String targetActivityId, DynamicAssignmentHandler dynamicAssignmentHandler)
        {

            if (this.CurrentWorkflowSession == null)
            {
                new EngineException(this.ProcessInstanceId,
                                this.WorkflowProcess, this.TaskId,
                                "The current workflow session is null.");
            }
            if (this.RuntimeContext == null)
            {
                new EngineException(this.ProcessInstanceId,
                                this.WorkflowProcess, this.TaskId,
                                "The current runtime context is null.");
            }

            if ((this.State == TaskInstanceStateEnum.COMPLETED) ||
                            (this.State == TaskInstanceStateEnum.CANCELED))
            {
                throw new EngineException(this.ProcessInstanceId, this.WorkflowProcess,
                        this.TaskId,
                        "Abort task instance failed . The state of the task instance [id=" + this.Id + "] is " + this.State);
            }

            if (dynamicAssignmentHandler != null)
            {
                this.CurrentWorkflowSession.setDynamicAssignmentHandler(dynamicAssignmentHandler);
            }


            ITaskInstanceManager taskInstanceMgr = this.RuntimeContext.TaskInstanceManager;
            taskInstanceMgr.abortTaskInstance(this.CurrentWorkflowSession, this.AliveProcessInstance, this, targetActivityId);

        }

        /* (non-Javadoc) 
         * @see org.fireflow.engine.ITaskInstance#abortEx(java.lang.String, org.fireflow.engine.taskinstance.DynamicAssignmentHandler) 
         */
        public void abortEx(String targetActivityId, DynamicAssignmentHandler dynamicAssignmentHandler)
        {

            if (this.CurrentWorkflowSession == null)
            {
                new EngineException(this.ProcessInstanceId,
                                this.WorkflowProcess, this.TaskId,
                                "The current workflow session is null.");
            }
            if (this.RuntimeContext == null)
            {
                new EngineException(this.ProcessInstanceId,
                                this.WorkflowProcess, this.TaskId,
                                "The current runtime context is null.");
            }

            if ((this.State == TaskInstanceStateEnum.COMPLETED) || (this.State == TaskInstanceStateEnum.CANCELED))
            {
                throw new EngineException(this.ProcessInstanceId, this.WorkflowProcess,
                        this.TaskId,
                        "Abort task instance failed . The state of the task instance [id=" + this.Id + "] is " + this.State);
            }

            if (dynamicAssignmentHandler != null)
            {
                this.CurrentWorkflowSession.setDynamicAssignmentHandler(dynamicAssignmentHandler);
            }
            ITaskInstanceManager taskInstanceMgr = this.RuntimeContext.TaskInstanceManager;
            taskInstanceMgr.abortTaskInstanceEx(this.CurrentWorkflowSession, this.AliveProcessInstance, this, targetActivityId);
        }
    }
}
