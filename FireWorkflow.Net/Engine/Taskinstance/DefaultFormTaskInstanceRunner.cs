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
using FireWorkflow.Net.Engine;
using FireWorkflow.Net.Engine.Beanfactory;
using FireWorkflow.Net.Engine.Impl;
using FireWorkflow.Net.Engine.Persistence;
using FireWorkflow.Net.Kernel;
using FireWorkflow.Net.Model;
using FireWorkflow.Net.Model.Resource;


namespace FireWorkflow.Net.Engine.Taskinstance
{
    public class DefaultFormTaskInstanceRunner : ITaskInstanceRunner
    {

        public void run(IWorkflowSession currentSession, RuntimeContext runtimeContext, IProcessInstance processInstance, ITaskInstance taskInstance)// throws EngineException, KernelException 
        {
            if (taskInstance.TaskType!= TaskTypeEnum.FORM)//!Task.FORM.Equals(taskInstance.TaskType))
            {
                throw new EngineException(processInstance, taskInstance.Activity,
                        "DefaultFormTaskInstanceRunner：TaskInstance的任务类型错误，只能为FORM类型");
            }

            DynamicAssignmentHandler dynamicAssignmentHandler = ((WorkflowSession)currentSession).consumeCurrentDynamicAssignmentHandler();
            FormTask task = (FormTask)taskInstance.Task;
            // performer(id,name,type,handler)
            Participant performer = task.Performer;
            if (performer == null || performer.AssignmentHandler.Trim().Equals(""))
            {
                throw new EngineException(processInstance, taskInstance.Activity,
                        "流程定义错误，Form类型的 task必须指定performer及其AssignmentHandler");
            }
            assign(currentSession, processInstance, runtimeContext, taskInstance, task, performer, dynamicAssignmentHandler);
        }

        /// <summary>分配， 按照当前任务的参与者插入工单</summary>
        /// <param name="currentSession"></param>
        /// <param name="processInstance"></param>
        /// <param name="runtimeContext"></param>
        /// <param name="taskInstance"></param>
        /// <param name="formTask"></param>
        /// <param name="part"></param>
        /// <param name="dynamicAssignmentHandler"></param>
        protected void assign(IWorkflowSession currentSession, IProcessInstance processInstance, RuntimeContext runtimeContext, ITaskInstance taskInstance, FormTask formTask, Participant part, DynamicAssignmentHandler dynamicAssignmentHandler)// throws EngineException, KernelException 
        {
            //如果有指定的Actor，则按照指定的Actor分配任务
            if (dynamicAssignmentHandler != null)
            {
                dynamicAssignmentHandler.assign((IAssignable)taskInstance, part.Name);
            }
            else
            {
                IPersistenceService persistenceService = runtimeContext.PersistenceService;
                List<ITaskInstance> taskInstanceList = persistenceService.FindTaskInstancesForProcessInstance(taskInstance.ProcessInstanceId, taskInstance.ActivityId);
                ITaskInstance theLastCompletedTaskInstance = null;

                for (int i = 0; taskInstanceList != null && i < taskInstanceList.Count; i++)
                {
                    ITaskInstance tmp = (ITaskInstance)taskInstanceList[i];
                    if (tmp.Id.Equals(taskInstance.Id)) continue;
                    if (!tmp.TaskId.Equals(taskInstance.TaskId)) continue;
                    if (tmp.State != TaskInstanceStateEnum.COMPLETED) continue;
                    if (theLastCompletedTaskInstance == null)
                    {
                        theLastCompletedTaskInstance = tmp;
                    }
                    else
                    {
                        if (theLastCompletedTaskInstance.StepNumber < tmp.StepNumber)
                        {
                            theLastCompletedTaskInstance = tmp;
                        }
                    }
                }

                //如果是循环且LoopStrategy==REDO，则分配个上次完成该工作的操作员
                if (theLastCompletedTaskInstance != null && (LoopStrategyEnum.REDO==formTask.LoopStrategy || currentSession.isInWithdrawOrRejectOperation()))
                {
                    List<IWorkItem> workItemList = persistenceService.FindCompletedWorkItemsForTaskInstance(theLastCompletedTaskInstance.Id);
                    ITaskInstanceManager taskInstanceMgr = runtimeContext.TaskInstanceManager;
                    for (int k = 0; k < workItemList.Count; k++)
                    {
                        IWorkItem completedWorkItem = (IWorkItem)workItemList[k];

                        IWorkItem newFromWorkItem = taskInstanceMgr.createWorkItem(currentSession, processInstance, taskInstance, completedWorkItem.ActorId);
                        newFromWorkItem.claim();//并自动签收
                    }
                }
                else
                {
                    IBeanFactory beanFactory = runtimeContext.BeanFactory;
                    //从spring中获取到对应任务的Performer，创建工单
                    //201004 add lwz 参与者通过业务接口实现默认获取用户
                    switch (part.AssignmentType)
                    {
                        case AssignmentTypeEnum.Current:
                            runtimeContext.AssignmentBusinessHandler.assignCurrent(
                                currentSession, processInstance, (IAssignable)taskInstance);
                            break;
                        case AssignmentTypeEnum.Role:
                            runtimeContext.AssignmentBusinessHandler.assignRole(
                                currentSession, processInstance, (IAssignable)taskInstance, part.PerformerValue);
                            break;
                        case AssignmentTypeEnum.Agency:
                            runtimeContext.AssignmentBusinessHandler.assignAgency(
                                currentSession, processInstance, (IAssignable)taskInstance, part.PerformerValue);
                            break;
                        case AssignmentTypeEnum.Fixed:
                            runtimeContext.AssignmentBusinessHandler.assignFixed(
                                currentSession, processInstance, (IAssignable)taskInstance, part.PerformerValue);
                            break;
                        case AssignmentTypeEnum.Superiors:
                            runtimeContext.AssignmentBusinessHandler.assignSuperiors(
                                currentSession, processInstance, (IAssignable)taskInstance);
                            break;
                        default:
                            IAssignmentHandler assignmentHandler = (IAssignmentHandler)beanFactory.GetBean(part.AssignmentHandler);
                            //modified by wangmj 20090904
                            ((IAssignmentHandler)assignmentHandler).assign((IAssignable)taskInstance, part.PerformerValue);
                            break;
                    }
                }
            }
        }
    }
}
