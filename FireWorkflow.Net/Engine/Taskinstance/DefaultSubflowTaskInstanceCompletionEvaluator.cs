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
    /// 
    /// </summary>
    public class DefaultSubflowTaskInstanceCompletionEvaluator : ITaskInstanceCompletionEvaluator
    {

        /// <summary>
        /// 判断任务实例是否可以终止
        /// </summary>
        /// <param name="currentSession"></param>
        /// <param name="runtimeContext"></param>
        /// <param name="processInstance"></param>
        /// <param name="taskInstance"></param>
        /// <returns>true表示可以终止，false不能终止</returns>
        public Boolean taskInstanceCanBeCompleted(IWorkflowSession currentSession, RuntimeContext runtimeContext,
                IProcessInstance processInstance, ITaskInstance taskInstance)//throws EngineException ,KernelException
        {
            //在Fire Workflow 中，系统默认每个子流程仅创建一个实例，所以当子流程实例完成后，SubflowTaskInstance都可以被completed
            //所以，应该直接返回true;
            return true;

            //如果系统动态创建了多个并发子流程实例，则需要检查是否存在活动的子流程实例，如果存在则返回false，否则返回true。
            //可以用下面的代码实现
            //        IPersistenceService persistenceService = runtimeContext.PersistenceService;
            //        Int32 count = persistenceService.getAliveProcessInstanceCountForParentTaskInstance(taskInstance.Id);
            //        if (count>0){
            //            return false;
            //        }else{
            //            return true;
            //        }

        }

    }
}
