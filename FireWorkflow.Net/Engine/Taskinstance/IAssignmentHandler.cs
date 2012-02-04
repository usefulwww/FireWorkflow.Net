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
//using System.Linq;
using System.Text;

namespace FireWorkflow.Net.Engine.Taskinstance
{
    /// <summary>
    /// <para>任务分配处理程序，工作流系统将真正的任务分配工作交给该处理程序完成。</para>
    /// <para>所有的FORM类型的Task都需要设置其Performer属性，Performer属性实际上是一个Participant对象，</para>
    /// 由该对象提供IAssignmentHandler实现类。
    /// </summary>
    public interface IAssignmentHandler
    {
    	//FIXME ITaskInstance 已经去掉IAssignable实现
        /// <summary>
        /// <para>实现任务分配工作，该方法一般的实现逻辑是：</para>
        /// <para>首先根据performerName查询出所有的操作员，可以把performerName当作角色名称。</para>
        /// <para>然后调用asignable.asignToActor(String actorId,Boolean needSign)或者</para>
        /// <para>asignable.asignToActor(String actorId)或者asignable.asignToActorS(List actorIds)</para>
        /// 进行任务分配。
        /// </summary>
        /// <param name="asignable">IAssignable实现类，在FireWorkflow中实际上就是TaskInstance对象。</param>
        /// <param name="performerName">角色名称</param>
        void assign(ITaskInstance taskInstance, String performerName);// throws EngineException, KernelException;

        //后续版本实现。。。
        //    public void assign(IWorkflowSession workflowSession, IProcessInstance processInstance, 
        //            IAssignable asignable, String performerName)throws EngineException,KernelException;
    }
}
