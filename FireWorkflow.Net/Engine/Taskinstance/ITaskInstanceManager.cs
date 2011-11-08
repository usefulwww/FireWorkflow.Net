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
using FireWorkflow.Net.Kernel;
using FireWorkflow.Net.Engine.Impl;

namespace FireWorkflow.Net.Engine.Taskinstance
{
    /// <summary>任务实例管理器</summary>
    public interface ITaskInstanceManager : IRuntimeContextAware
    {
        /// <summary>
        /// 创建taskinstance实例
        /// </summary>
        /// <param name="token"></param>
        /// <param name="activityInstance"></param>
        void createTaskInstances(IToken token, IActivityInstance activityInstance);// throws EngineException, KernelException;

        /// <summary>
        /// <para>将已经完成的taskinstance实例转移到已办表</para>
        /// （该方法保留在1.0中未使用，暂时保留，20090317）
        /// </summary>
        /// <param name="activityInstance"></param>
        void archiveTaskInstances(IActivityInstance activityInstance);// throws EngineException, KernelException;

        /// <summary>
        /// <para>启动TaskInstance，其状态将从INITIALIZED变成STARTED状态。</para>
        /// <para>对于Tool类型的TaskInstance,将直接调用外部应用程序。</para>
        /// <para>对于Sbuflow类型的TaskInstance，将启动子流程。</para>
        /// 对于Form类型的TaskInstance，仅改变其状态纪录启动时间。
        /// </summary>
        /// <param name="currentSession"></param>
        /// <param name="processInstance"></param>
        /// <param name="taskInstance"></param>
        void startTaskInstance(IWorkflowSession currentSession, IProcessInstance processInstance, ITaskInstance taskInstance);// throws EngineException, KernelException   ;

        /// <summary>
        /// <para>结束TaskInstance以及当前的ActivityInstance，并执行targetActivityInstance环节实例。</para>
        /// 如果targetActivityInstance为null表示由工作流引擎根据流程定义自动流转到下一个环节。
        /// </summary>
        void completeTaskInstance(IWorkflowSession currentSession, IProcessInstance processInstance, ITaskInstance taskInstance, IActivityInstance targetActivityInstance);// throws EngineException, KernelException;

        /// <summary>
        /// 中止task instance。
        /// </summary>
        /// <param name="currentSession"></param>
        /// <param name="processInstance"></param>
        /// <param name="taskInstance"></param>
        /// <param name="targetActivityId"></param>
        void abortTaskInstance(IWorkflowSession currentSession, IProcessInstance processInstance, ITaskInstance taskInstance, String targetActivityId);

        /// <summary>
        /// 中止task instance。
        /// </summary>
        /// <param name="currentSession"></param>
        /// <param name="processInstance"></param>
        /// <param name="taskInstance"></param>
        /// <param name="targetActivityId"></param>
        void abortTaskInstanceEx(IWorkflowSession currentSession, IProcessInstance processInstance, ITaskInstance taskInstance, String targetActivityId);

        /// <summary>根据TaskInstance创建workItem。</summary>
        WorkItem createWorkItem(IWorkflowSession currentSession, IProcessInstance processInstance, ITaskInstance taskInstance, String actorId);// throws EngineException;

        /// <summary>签收WorkItem。</summary>
        IWorkItem claimWorkItem(String workItemId, String taskInstanceId);//throws EngineException, KernelException ;

        /// <summary>结束WorkItem</summary>
        void completeWorkItem(IWorkItem workItem, IActivityInstance targetActivityInstance, String comments);//throws EngineException, KernelException ;

        /// <summary>结束工单并跳转</summary>
        void completeWorkItemAndJumpTo(IWorkItem workItem, String targetActivityId, String comments);// throws EngineException, KernelException ;

        /// <summary>结束工单并跳转（超级）</summary>
        void completeWorkItemAndJumpToEx(IWorkItem workItem, String targetActivityId, String comments);


        /// <summary>撤销刚才执行的Complete动作，系统将创建并返回一个新的Running状态的WorkItem</summary>
        /// <returns>新创建的工作项</returns>
        IWorkItem withdrawWorkItem(IWorkItem workItem);// throws EngineException, KernelException ;

        /// <summary>拒收</summary>
        void rejectWorkItem(IWorkItem workItem, String comments);// throws  EngineException, KernelException ;

        /// <summary>将工作项位派给其他人，自己的工作项变成CANCELED状态。返回新创建的WorkItem.</summary>
        /// <param name="workItem">我的WorkItem</param>
        /// <param name="actorId">被委派的Actor的Id</param>
        /// <param name="comments">备注信息</param>
        /// <returns>新创建的工作项</returns>
        IWorkItem reasignWorkItemTo(IWorkItem workItem, String actorId, String comments);
    }
}
