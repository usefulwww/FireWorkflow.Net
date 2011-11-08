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

namespace FireWorkflow.Net.Engine.Taskinstance
{
    /// <summary>
    /// 任务实例创建器
    /// </summary>
    public interface ITaskInstanceCreator
    {
        /// <summary>
        /// 创建任务实例
        /// </summary>
        /// <param name="currentSession"></param>
        /// <param name="runtimeContxt"></param>
        /// <param name="processInstance"></param>
        /// <param name="task"></param>
        /// <param name="activity"></param>
        /// <returns></returns>
        ITaskInstance createTaskInstance(IWorkflowSession currentSession, RuntimeContext runtimeContxt, IProcessInstance processInstance, Task task, Activity activity);// throws EngineException;
    }
}
