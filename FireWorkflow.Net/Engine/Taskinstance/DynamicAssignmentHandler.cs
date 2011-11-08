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
using FireWorkflow.Net.Engine.Impl;

namespace FireWorkflow.Net.Engine.Taskinstance
{
    /// <summary>
    /// 动态任务分配句柄，用于指定后续环节的操作员。
    /// </summary>
    public class DynamicAssignmentHandler : IAssignmentHandler
    {
        /// <summary>获取或设置工作项是否需要签收</summary>
        public Boolean IsNeedClaim { get; set; }

        /// <summary>获取或设置操作员Id列表</summary>
        public List<String> ActorIdsList { get; set; }

        /// <summary>
        /// 实现任务分配工作，该方法一般的实现逻辑是：
        /// 首先根据performerName查询出所有的操作员，可以把performerName当作角色名称。
        /// 然后调用asignable.asignToActor(String actorId,Boolean needSign)或者
        /// asignable.asignToActor(String actorId)或者asignable.asignToActorS(List actorIds)
        /// 进行任务分配。
        /// </summary>
        /// <param name="asignable">IAssignable实现类，在FireWorkflow中实际上就是TaskInstance对象。</param>
        /// <param name="performerName">角色名称</param>
        public void assign(IAssignable asignable, String performerName)// throws EngineException, KernelException 
        {
            if (ActorIdsList == null || ActorIdsList.Count == 0)
            {
                TaskInstance taskInstance = (TaskInstance)asignable;
                throw new EngineException(taskInstance.ProcessInstanceId, taskInstance.WorkflowProcess, taskInstance.TaskId,
                    "actorIdsList can not be empty");
            }

            List<IWorkItem> workItems = asignable.assignToActors(ActorIdsList);

            ITaskInstance taskInst = (ITaskInstance)asignable;
            //如果不需要签收，这里自动进行签收，（FormTask的strategy="all"或者=any并且工作项数量为1） 
            if (!IsNeedClaim)
            {
                if (FormTaskEnum.ALL==taskInst.AssignmentStrategy || (FormTaskEnum.ANY==taskInst.AssignmentStrategy && ActorIdsList.Count == 1))
                {
                    for (int i = 0; i < workItems.Count; i++)
                    {
                        IWorkItem wi = workItems[i];
                        wi.claim();
                    }
                }
            }
        }
    }
}
