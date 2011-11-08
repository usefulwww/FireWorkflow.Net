/*
 * Copyright 2010 无忧
 * All rights reserved. 
 * 
 * 201004 add lwz 参与者通过业务接口实现默认获取用户
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FireWorkflow.Net.Engine.Taskinstance
{
    /// <summary>
    /// <para>任务分配处理程序，工作流系统将真正的任务分配工作交给该处理程序完成。</para>
    /// <para>所有的FORM类型的Task都需要设置其Performer属性，Performer属性实际上是一个Participant对象，</para>
    /// 由该对象提供IAssignmentHandler实现类。
    /// </summary>
    public interface IAssignmentBusinessHandler : IRuntimeContextAware
    {
        /// <summary>
        /// <para>实现任务分配工作，该方法一般的实现逻辑是：</para>
        /// <para>获取创建操作员。</para>
        /// <para>然后调用asignable.asignToActor(String actorId,Boolean needSign)或者</para>
        /// <para>asignable.asignToActor(String actorId)或者asignable.asignToActorS(List actorIds)</para>
        /// 进行任务分配。
        /// </summary>
        /// <param name="asignable">IAssignable实现类，在FireWorkflow中实际上就是TaskInstance对象。</param>
        void assignCurrent(IWorkflowSession workflowSession, IProcessInstance processInstance, IAssignable asignable);// throws EngineException, KernelException;

        /// <summary>
        /// <para>实现任务分配工作，该方法一般的实现逻辑是：</para>
        /// <para>首先根据performerName当作角色名称查询出所有的操作员。</para>
        /// <para>然后调用asignable.asignToActor(String actorId,Boolean needSign)或者</para>
        /// <para>asignable.asignToActor(String actorId)或者asignable.asignToActorS(List actorIds)</para>
        /// 进行任务分配。
        /// </summary>
        /// <param name="asignable">IAssignable实现类，在FireWorkflow中实际上就是TaskInstance对象。</param>
        /// <param name="performerName">角色名称</param>
        void assignRole(IWorkflowSession workflowSession, IProcessInstance processInstance, IAssignable asignable, String performerName);// throws EngineException, KernelException;


        /// <summary>
        /// <para>实现任务分配工作，该方法一般的实现逻辑是：</para>
        /// <para>首先根据performerName当作机构名称查询出所有的操作员。</para>
        /// <para>然后调用asignable.asignToActor(String actorId,Boolean needSign)或者</para>
        /// <para>asignable.asignToActor(String actorId)或者asignable.asignToActorS(List actorIds)</para>
        /// 进行任务分配。
        /// </summary>
        /// <param name="asignable">IAssignable实现类，在FireWorkflow中实际上就是TaskInstance对象。</param>
        /// <param name="performerName">机构名称</param>
        void assignAgency(IWorkflowSession workflowSession, IProcessInstance processInstance, IAssignable asignable, String performerName);


        /// <summary>
        /// <para>实现任务分配工作，该方法一般的实现逻辑是：</para>
        /// <para>首先根据performerName当固定用户，并可以辅助实现代办。</para>
        /// <para>然后调用asignable.asignToActor(String actorId,Boolean needSign)或者</para>
        /// <para>asignable.asignToActor(String actorId)或者asignable.asignToActorS(List actorIds)</para>
        /// 进行任务分配。
        /// </summary>
        /// <param name="asignable">IAssignable实现类，在FireWorkflow中实际上就是TaskInstance对象。</param>
        /// <param name="performerName">用户名称</param>
        void assignFixed(IWorkflowSession workflowSession, IProcessInstance processInstance, IAssignable asignable, String performerName);


        /// <summary>
        /// <para>实现任务分配工作，该方法一般的实现逻辑是：</para>
        /// <para>创建者的上级领导用户</para>
        /// <para>然后调用asignable.asignToActor(String actorId,Boolean needSign)或者</para>
        /// <para>asignable.asignToActor(String actorId)或者asignable.asignToActorS(List actorIds)</para>
        /// 进行任务分配。
        /// </summary>
        /// <param name="asignable">IAssignable实现类，在FireWorkflow中实际上就是TaskInstance对象。</param>
        void assignSuperiors(IWorkflowSession workflowSession, IProcessInstance processInstance, IAssignable asignable);
    }
}