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
using FireWorkflow.Net.Model.Net;

namespace FireWorkflow.Net.Engine
{
    public enum ProcessInstanceEnum
    {
        /// <summary>初始化状态</summary>
        INITIALIZED = 0,
        /// <summary>运行状态</summary>
        RUNNING = 1,
        /// <summary>已经结束</summary>
        COMPLETED = 7,
        /// <summary>被撤销</summary>
        CANCELED = 9
    }
    /// <summary>
    /// <para>流程实例接口</para>
    /// <para>对流程实例的状态字段作如下规定：小于5的状态为“活动”状态，大于等于5的状态为“非活动”状态。</para>
    /// <para>活动状态包括：INITIALIZED,RUNNING,SUSPENDED</para>
    /// 非活动状态包括：COMPLETED,CANCELED
    /// </summary>
    public interface IProcessInstance/*: IWorkflowSessionAware*/
    {

        /// <summary>主键.</summary>
        String Id { get;set; }

        /// <summary>流程实例的name（与流程定义的name相同）</summary>
        String Name { get;set; }

        /// <summary>流程实例的DisplayName（与流程定义的DisplayName相同）</summary>
        String DisplayName { get;set; }

        /// <summary>流程定义的id</summary>
        String ProcessId { get;set; }

        /// <summary>流程实例的状态</summary>
        ProcessInstanceEnum State { get; set;}

        /// <summary>流程定义的Version</summary>
        Int32 Version { get;set; }

        /// <summary>流程实例创建者ID</summary>
        String CreatorId { get;set; }

        /// <summary>返回流程实例的创建时间</summary>
        /// <returns>流程实例的创建时间</returns>
        DateTime? CreatedTime { get;set; }

        /// <summary>返回流程实例的启动时间，即执行IProcessInstance.run()的时间</summary>
        DateTime? StartedTime { get;set; }

        /// <summary>返回流程实例的结束时间</summary>
        DateTime? EndTime { get;set; }

        /// <summary>返回流程实例的到期时间</summary>
        DateTime? ExpiredTime { get;set; }

//        Dictionary<String, Object> ProcessInstanceVariables { get; set; }

        /// <summary>return the corresponding workflow process.</summary>
//        WorkflowProcess WorkflowProcess { get; }// throws EngineException;

//        String WorkflowProcessId { get; }

        /// <summary>get the parent process instance's id , null if no parent process instance.</summary>
        String ParentProcessInstanceId { get;set; }

        /// <summary>get the parent taskinstance's id ,null if no parent taskinstance.</summary>
        String ParentTaskInstanceId { get; set;}
        
       Boolean? Suspended { get; set; }


    }
}
