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

namespace FireWorkflow.Net.Kernel
{
    /// <summary>
    /// 
    /// </summary>
    public class TokenFrom
    {
        /// <summary>前驱activityid的分隔符为</summary>
        public const String FROM_ACTIVITY_ID_SEPARATOR = "&";
        public const String FROM_START_NODE = "FROM_START_NODE";
    }

    /// <summary>
    /// token的生命周期开始与一个synchronizer（包括startnode 和 endnode)，结束于另一个synchronizer
    /// </summary>
    public interface IToken
    {
        /// <summary>获取或设置流程实例对象</summary>
        IProcessInstance ProcessInstance { get; set; }
        //public abstract void setProcessInstance(IProcessInstance inst);

        /// <summary>获取或设置流程实例对象ID</summary>
        String ProcessInstanceId { get; set; }
        //public abstract void setProcessInstanceId(String id);

        String NodeId { get; set; }
        //public abstract void setNodeId(String nodeId);

        Int32 Value { get; set; }
        //public abstract void setValue(Int32 v);

        /// <summary>
        /// 通过alive标志来判断nodeinstance是否要fire
        /// </summary>
        Boolean IsAlive { get; set; }
        //public abstract void.IsAlive=Boolean b;

        String Id { get; set; }
        //public abstract void setId(String id);

        Int32 StepNumber { get; set; }
        //public abstract void setStepNumber(Int32 i);

        /// <summary>
        /// 获得前驱Activity的Id,如果有多个，则用"&"分割
        /// </summary>
        String FromActivityId { get; set; }
        //public abstract void setFromActivityId(String s);

        /// <summary>
        /// @date 20090908 
        /// 返回Engine的当前上下文信息，如WorkflowSession,等。
        /// 这些信息不保存到数据库 
        /// </summary>
        Dictionary<String, IProcessInstance> ContextInfo { get; }
    }

}
