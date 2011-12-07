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
    public class TaskInstance : ITaskInstance/*, IAssignable*/
    {
        /// <summary>工作流总线</summary>
//        public RuntimeContext RuntimeContext { get; set; }

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

        //public IWorkflowSession CurrentWorkflowSession { get; set; }

       

        public TaskInstance()
        {
            this.State = TaskInstanceStateEnum.INITIALIZED;
            this.Suspended = false;
            this.CanBeWithdrawn = true;
        }

//        public TaskInstance(ProcessInstance workflowProcessInsatnce)
//        {
//            this.State = TaskInstanceStateEnum.INITIALIZED;
//            this.Suspended = false;
//            this.CanBeWithdrawn = true;
//            this.processInsatance = workflowProcessInsatnce;
//        }



        

        
 
    }
}
