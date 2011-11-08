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

namespace FireWorkflow.Net.Engine.Taskinstance
{
    /// <summary>
    /// TaskInstance实现了该节口，用于任务分配
    /// </summary>
    public interface IAssignable
    {
        /// <summary>
        /// 将TaskInstance分配给编号为actorId的操作员。即系统只创建一个WorkItem，并分配给编号为actorId的操作员
        /// 该WorkItem需要签收
        /// </summary>
        /// <param name="actorId">操作员Id</param>
        /// <returns>返回创建的WorkItem</returns>
        IWorkItem assignToActor(String actorId);// throws EngineException,KernelException;

        /// <summary>
        /// 将TaskInstance分配给列表中的操作员。即创建N个WorkItem，每个操作员一个WorkItem，并且这些WorkItem都需要签收。
        /// 最终由那个操作员执行该任务实例，是由Task的分配策略决定的。
        /// 如果分配策略为ALL,即会签的情况，则所有的操作员都要完成相应的工单。
        /// 如果分配策略为ANY，则最先签收的那个操作员完成其工单和任务实例，其他操作员的工单被删除。
        /// </summary>
        /// <param name="actorIds">操作员Id</param>
        /// <returns>返回创建的WorkItem列表</returns>
        List<IWorkItem> assignToActors(List<String> actorIds);// throws EngineException, KernelException;
    }
}
