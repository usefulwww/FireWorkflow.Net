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
using System.Xml;
using System.Xml.Serialization;
using System.Text;

namespace FireWorkflow.Net.Model
{
    /// <summary>工作流任务</summary>
    public abstract class Task : AbstractWFElement
    {
        #region 属性
        /// <summary>获取或设置任务类型,取值为FORM,TOOL,SUBFLOW,DUMMY(保留)，缺省值为FORM</summary>
        public TaskTypeEnum TaskType { get; set; }

        /// <summary>获取或设置任务的完成期限</summary>
        public Duration Duration { get; set; }

        /// <summary>获取或设置任务优先级别(1.0暂时没有用到)</summary>
        public int Priority { get; set; }

        /// <summary>获取或设置循环情况下任务执行策略，取值为REDO、SKIP和NONE,</summary>
        public LoopStrategyEnum LoopStrategy { get; set; }

        /// <summary>获取或设置任务实例创建器。如果没有设置，则使用所在流程的全局任务实例创建器。</summary>
        public String TaskInstanceCreator { get; set; }

        /// <summary>获取或设置任务实例运行器，如果没有设置，则使用所在流程的全局的任务实例运行器</summary>
        public String TaskInstanceRunner { get; set; }

        /// <summary>获取或设置任务实例的终结评价器，用于告诉引擎，该实例是否可以结束。如果没有设置，则使用所在流程的全局的任务实例终结评价器。</summary>
        public String TaskInstanceCompletionEvaluator { get; set; }
        #endregion

        #region 构造函数
        public Task()
        {
            this.TaskType = TaskTypeEnum.FORM;
            this.Priority = 1;
            this.LoopStrategy = LoopStrategyEnum.REDO;
        }

        public Task(IWFElement parent, String name) : base(parent, name)
        {
            this.TaskType = TaskTypeEnum.FORM;
            this.Priority = 1;
            this.LoopStrategy = LoopStrategyEnum.REDO;
        }
        #endregion

        #region 方法
        public override String ToString()
        {
            return "Task[id='" + this.Id + ", name='" + this.Name + "']";
        }
        #endregion
    }
    #region 枚举 TaskTypeEnum,LoopStrategyEnum
    /// <summary>
    /// 任务枚举
    /// </summary>
    public enum TaskTypeEnum
    {
        /// <summary>任务类型之二 ：TOOL类型，即工具类型任务，该任务自动调用java代码完成特定的工作。</summary>
        TOOL,
        /// <summary>任务类型之三：SUBFLOW类型，即子流程任务</summary>
        SUBFLOW,
        /// <summary>任务类型之一：FORM类型，最常见的一类任务，代表该任务需要操作员填写相关的表单。</summary>
        FORM,
        /// <summary>任务类型之四：DUMMY类型，该类型暂时没有用到，保留。</summary>
        DUMMY
    }

    /// <summary>
    /// 循环情况下任务执行策略枚举
    /// </summary>
    public enum LoopStrategyEnum
    {
        /// <summary>
        /// 循环情况下，任务分配指示之一：重做<br />
        /// 对于Tool类型和Subflow类型的task会重新执行一遍
        /// 对于Form类型的Task，重新执行一遍，且将该任务实例分配给最近一次完成同一任务的操作员。
        /// </summary>
        REDO,
        /// <summary>
        /// 循环情况下，任务分配指示之二：忽略<br />
        /// 循环的情况下该任务将被忽略，即在流程实例的生命周期里，仅执行一遍。
        /// </summary>
        SKIP,
        /// <summary>
        /// 循环的情况下，任务分配指示之三：无<br/>
        /// 对于Tool类型和Subflow类型的task会重新执行一遍，和REDO效果一样的。<br/>
        /// 对于Form类型的Task，重新执行一遍，且工作流引擎仍然调用Performer属性的AssignmentHandler分配任务
        /// </summary>
        NONE
    }
    #endregion
}
