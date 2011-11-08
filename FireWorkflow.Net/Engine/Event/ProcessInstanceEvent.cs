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
    public enum ProcessInstanceEventEnum
    {
        /// <summary>在即将启动流程实例的时候触发的事件</summary>
        BEFORE_PROCESS_INSTANCE_RUN = 2,

        /// <summary>在流程实例结束后触发的事件</summary>
        AFTER_PROCESS_INSTANCE_COMPLETE = 7
    }

    /// <summary>流程实例事件</summary>
    public class ProcessInstanceEvent
    {
        /// <summary>返回触发事件的流程实例</summary>
        public IProcessInstance Source { get; set; }

        /// <summary>返回事件类型，取值为BEFORE_PROCESS_INSTANCE_RUN或者AFTER_PROCESS_INSTANCE_COMPLETE</summary>
        public ProcessInstanceEventEnum EventType { get; set; }
    }
}
