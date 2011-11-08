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
    /// <summary>流程实例事件监听接口</summary>
    public interface IProcessInstanceEventListener
    {
        /// <summary>
        /// <para>响应流程实例的事件。通过e.getEventType来判断事件的类型。</para>
        /// <para>流程实例有两种事件：<para/>
        /// <para>BEFORE_PROCESS_INSTANCE_RUN (= 2):在即将启动流程实例的时候触发的事件<para/>
        /// AFTER_PROCESS_INSTANCE_COMPLETE (= 7):在流程实例结束后触发的事件
        /// </summary>
        /// <param name="e">流程实例事件</param>
        void onProcessInstanceEventFired(ProcessInstanceEvent e);// throws EngineException;
    }
}
