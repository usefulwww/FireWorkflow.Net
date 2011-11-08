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

namespace FireWorkflow.Net.Engine.Event
{
    /// <summary>
    /// 事件类型
    /// </summary>
    public enum TaskInstanceEventEnum
    {
        /// <summary>
        /// 
        /// </summary>
        NULL=-1,
        /// <summary>在任务实例即将启动时触发的事件</summary>
        BEFORE_TASK_INSTANCE_START = 2,

        /// <summary>当创建工作项之后</summary>
        AFTER_WORKITEM_CREATED = 5,

        /// <summary>当工作项完成之后</summary>
        AFTER_WORKITEM_COMPLETE = 6,

        /// <summary>在任务实例结束时触发的事件</summary>
        AFTER_TASK_INSTANCE_COMPLETE = 7
    }
    /// <summary>任务实例事件</summary>
    public class TaskInstanceEvent
    {
        /// <summary>返回事件类型，取值为BEFORE_TASK_INSTANCE_START或者AFTER_TASK_INSTANCE_COMPLETE</summary>
        public TaskInstanceEventEnum EventType { get; set; }

        /// <summary>返回触发该事件的任务实例</summary>
        public ITaskInstance Source { get; set; }

        public IWorkItem WorkItem { get; set; }

        public IWorkflowSession WorkflowSession { get; set; }

        public IProcessInstance ProcessInstance { get; set; }
    }
}
