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
    /// 任务实例事件监听接口
    /// </summary>
    public interface ITaskInstanceEventListener
    {
        /// <summary>
        /// 响应任务实例的事件。通过e.getEventType区分事件的类型。
        /// </summary>
        /// <param name="e">任务实例的事件。</param>
        void onTaskInstanceEventFired(TaskInstanceEvent e);// throws EngineException;
    }
}
