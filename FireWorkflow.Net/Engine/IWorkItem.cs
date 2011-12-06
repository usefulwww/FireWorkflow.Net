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
using System.Text;
using FireWorkflow.Net.Engine.Taskinstance;

namespace FireWorkflow.Net.Engine
{
    /// <summary>
    /// 工作项的状态
    /// </summary>
    public enum WorkItemEnum
    {
        /// <summary>初始化状态</summary>
        INITIALIZED = 0,
        /// <summary>运行状态</summary>
        RUNNING = 1,
        /// <summary>已经结束</summary>
        COMPLETED = 7,
        /// <summary>被撤销</summary>
        CANCELED = 9
    }

    /// <summary>
    /// 工作项对象。<br/>
    /// 对该对象的状态字段作如下规定：小于5的状态为“活动”状态，大于等于5的状态为“非活动”状态。
    /// 活动状态包括：INITIALIZED,RUNNING,SUSPENDED
    /// 非活动状态包括：COMPLETED,CANCELED
    /// </summary>
    public interface IWorkItem:  IWorkflowSessionAware
    {
        /// <summary>返回工作项的Id</summary>
        String Id { get;set; }

        /// <summary>返回工作项的状态</summary>
        WorkItemEnum State { get;set; }

        //lwz 2010-3-3 add begin 方便业务展示
        /// <summary>返回任务实例TaskInstance.Name</summary>
        String Name { get; }

        /// <summary>返回任务实例显示名TaskInstance.DisplayName</summary>
        String DisplayName { get; }
        
        /// <summary>返回对应的流程实例Id</summary>
        String ProcessInstanceId { get; }

        /// <summary>返回对应的流程的Id</summary>
        String ProcessId { get; }

        /// <summary>返回流程的版本</summary>
        Int32 Version { get; }

        /// <summary>当前任务实例中的业务信息</summary>
        String BizInfo { get;}
        //lwz 2010-3-3 add end

        /// <summary>获取或设置备注信息</summary>
        String Comments { get; set; }

        /// <summary>返回创建时间</summary>
        DateTime CreatedTime { get; set;}

        /// <summary>返回签收时间</summary>
        DateTime? ClaimedTime { get;set; }

        /// <summary>返回结束时间</summary>
        DateTime? EndTime { get; set;}

        /// <summary>返回操作员的Id</summary>
        String ActorId { get; set;}

        /// <summary>返回任务实例</summary>
        ITaskInstance TaskInstance { get;set; }

//        /// <summary>
//        /// <para>签收工作项。如果任务实例的分配模式是ANY，则同一个任务实例的其他工作项将被删除。</para>
//        /// <para>如果任务是里的分配模式是ALL，则此操作不影响同一个任务实例的其他工作项的状态。</para>
//        /// <para>如果签收成功，则返回一个新的IWorkItem对象，并且更新当前WorkItem对象的状态修改成RUNNING状态，</para>
//        /// <para>更新ClaimedTime属性值。</para>
//        /// <para>如果签收失败，则返回null，且当前WorkItem的状态被修改为CANCELED</para>
//        /// <para>例如：同一个TaskInstance被分配给Actor_1和Actor_2，且分配模式是ANY，即便Actor_1和Actor_2同时执行</para>
//        /// <para>签收操作，也必然有一个人签收失败。系统对这种竞争性操作进行了同步。</para>
//        /// </summary>
//        /// <returns>如果签收成功，则返回一个新的IWorkItem对象；否则返回null</returns>
//        IWorkItem claim();// throws EngineException, KernelException;
//
//        /// <summary>
//        /// <para>对已经结束的工作项执行取回操作</para>
//        /// <para>只有满足如下约束才能正确执行取回操作：</para>
//        /// <para>1) 下一个Activity只有Form类型的Task,没有Tool类型和Subflow类型的Task</para>
//        /// <para>2) 下一个环节的所有WorkItem还没有被签收，都处于Initialized状态，</para>
//        /// <para>如果在本WorkItem成功执行了jumpTo操作或者loopTo操作，只要满足上述条件，也可以</para>
//        /// <para>成功执行withdraw。</para>
//        /// <para>该方法和IWorkflowSession.withdrawWorkItem(String workItemId)等价。</para>
//        /// </summary>
//        /// <returns>如果取回成功，则创建一个新的WorkItem 并返回该WorkItem</returns>
//        IWorkItem withdraw();//throws EngineException, KernelException;
//
//        /// <summary>
//        /// <para>执行“拒收”操作，可以对已经签收的或者未签收的WorkItem拒收。</para>
//        /// <para>该操作必须满足如下条件：</para>
//        /// <para>1、前驱环节中没有没有Tool类型和Subflow类型的Task；</para>
//        /// <para>2、没有和当前TaskInstance并行的其他TaskInstance；</para>
//        /// 该方法和IWorkflowSession.rejectWorkItem(String workItemId)等价。
//        /// </summary>
//        void reject();//throws EngineException, KernelException;
//
//
//        /// <summary>
//        /// <para>执行“拒收”操作，可以对已经签收的或者未签收的WorkItem拒收。</para>
//        /// <para>该操作必须满足如下条件：</para>
//        /// <para>1、前驱环节中没有没有Tool类型和Subflow类型的Task；</para>
//        /// <para>2、没有合当前TaskInstance并行的其他TaskInstance；</para>
//        /// 该方法和IWorkflowSession.rejectWorkItem(String workItemId,String comments)等价。
//        /// </summary>
//        /// <param name="comments">备注信息，将被写入workItem.comments字段。</param>
//        void reject(String comments);//throws EngineException, KernelException;
//
//        /// <summary>
//        /// <para>结束当前WorkItem；并由工作流引擎根据流程定义决定下一步操作。引擎的执行规则如下</para>
//        /// <para>1、工作流引擎首先判断该WorkItem对应的TaskInstance是否可以结束。</para>
//        /// <para>   如果TaskInstance的assignment策略为ANY，或者，assignment策略为ALL且它所有的WorkItem都已经完成</para>
//        /// <para>   则结束当前TaskInstance</para>
//        /// <para>2、判断TaskInstance对应的ActivityInstance是否可以结束。如果ActivityInstance的complete strategy为ANY，</para>
//        /// <para>   或者，complete strategy为ALL且他的所有的TaskInstance都已经结束，则结束当前ActivityInstance</para>
//        /// <para>3、根据流程定义，启动下一个Activity，并创建相关的TaskInstance和WorkItem</para>
//        /// </summary>
//        void complete();// throws EngineException, KernelException;
//
//        /// <summary>
//        /// <para>结束当前WorkItem；并由工作流引擎根据流程定义决定下一步操作。引擎的执行规则如下</para>
//        /// <para>1、工作流引擎首先判断该WorkItem对应的TaskInstance是否可以结束。</para>
//        /// <para>   如果TaskInstance的assignment策略为ANY，或者，assignment策略为ALL且它所有的WorkItem都已经完成</para>
//        /// <para>   则结束当前TaskInstance</para>
//        /// <para>2、判断TaskInstance对应的ActivityInstance是否可以结束。如果ActivityInstance的complete strategy为ANY，</para>
//        /// <para>   或者，complete strategy为ALL且他的所有的TaskInstance都已经结束，则结束当前ActivityInstance</para>
//        /// <para>3、根据流程定义，启动下一个Activity，并创建相关的TaskInstance和WorkItem</para>
//        /// </summary>
//        /// <param name="comments">备注信息</param>
//        void complete(String comments);//throws EngineException, KernelException;
//
//        /// <summary>
//        /// <para>结束当前WorkItem；并由工作流引擎根据流程定义决定下一步操作。引擎的执行规则如下</para>
//        /// <para>1、工作流引擎首先判断该WorkItem对应的TaskInstance是否可以结束。</para>
//        /// <para>   如果TaskInstance的assignment策略为ANY，或者，assignment策略为ALL且它所有的WorkItem都已经完成</para>
//        /// <para>   则结束当前TaskInstance</para>
//        /// <para>2、判断TaskInstance对应的ActivityInstance是否可以结束。如果ActivityInstance的complete strategy为ANY，</para>
//        /// <para>   或者，complete strategy为ALL且他的所有的TaskInstance都已经结束，则结束当前ActivityInstance</para>
//        /// <para>3、根据流程定义，启动下一个Activity，并创建相关的TaskInstance和WorkItem</para>
//        /// </summary>
//        /// <param name="dynamicAssignmentHandler">通过动态分配句柄指定下一个环节的操作者。</param>
//        /// <param name="comments">备注信息</param>
//        void complete(DynamicAssignmentHandler dynamicAssignmentHandler, String comments);// throws EngineException, KernelException;
//
//        /// <summary>
//        /// <para>结束当前WorkItem，跳转到指定的Activity</para>
//        /// <para>只有满足如下条件的情况下，该方法才能成功执行，否则抛出EngineException，流程状态恢复到调用该方法之前的状态。</para>
//        /// <para>1)当前Activity和即将启动的Acitivty必须在同一个执行线上</para>
//        /// <para>2)当前Task的assignment为Task.ANY。或者当前Task的assignment为Task.ALL(汇签)，且本WorkItem结束后可以使得TaskInstance结束；与之相反的情况是，</para>
//        /// <para>  尚有其他参与汇签的操作者没有完成其工作项，这时engine拒绝跳转操作</para>
//        /// <para>3)当前TaskInstance结束后,可以使得当前的ActivityInstance结束。与之相反的情况是，当前Activity包含了多个Task，且Activity的Complete Strategy是ALL，</para>
//        /// <para>  尚有其他的TaskInstance仍然处于活动状态，这种情况下执行jumpTo操作会被拒绝。</para>
//        /// </summary>
//        /// <param name="targetActivityId">下一个环节的ActivityId</param>
//        void jumpTo(String targetActivityId);// throws EngineException, KernelException;
//
//        /// <summary>
//        /// <para>结束当前WorkItem，跳转到指定的Activity</para>
//        /// <para>只有满足如下条件的情况下，该方法才能成功执行，否则抛出EngineException，流程状态恢复到调用该方法之前的状态。</para>
//        /// <para>1)当前Activity和即将启动的Acitivty必须在同一个执行线上</para>
//        /// <para>2)当前Task的assignment为Task.ANY。或者当前Task的assignment为Task.ALL(汇签)，且本WorkItem结束后可以使得TaskInstance结束；与之相反的情况是，</para>
//        /// <para>  尚有其他参与汇签的操作者没有完成其工作项，这时engine拒绝跳转操作</para>
//        /// <para>3)当前TaskInstance结束后,可以使得当前的ActivityInstance结束。与之相反的情况是，当前Activity包含了多个Task，且Activity的Complete Strategy是ALL，</para>
//        /// <para>  尚有其他的TaskInstance仍然处于活动状态，这种情况下执行jumpTo操作会被拒绝。</para>
//        /// </summary>
//        /// <param name="targetActivityId">下一个环节的id</param>
//        /// <param name="comments">备注信息</param>
//        void jumpTo(String targetActivityId, String comments);// throws EngineException, KernelException;
//
//        /// <summary>
//        /// <para>结束当前WorkItem，跳转到指定的Activity</para>
//        /// <para>只有满足如下条件的情况下，该方法才能成功执行，否则抛出EngineException，流程状态恢复到调用该方法之前的状态。</para>
//        /// <para>1)当前Activity和即将启动的Acitivty必须在同一个执行线上</para>
//        /// <para>2)当前Task的assignment为Task.ANY。或者当前Task的assignment为Task.ALL(汇签)，且本WorkItem结束后可以使得TaskInstance结束；与之相反的情况是，</para>
//        /// <para>  尚有其他参与汇签的操作者没有完成其工作项，这时engine拒绝跳转操作</para>
//        /// <para>3)当前TaskInstance结束后,可以使得当前的ActivityInstance结束。与之相反的情况是，当前Activity包含了多个Task，且Activity的Complete Strategy是ALL，</para>
//        /// <para>  尚有其他的TaskInstance仍然处于活动状态，这种情况下执行jumpTo操作会被拒绝。</para>
//        /// </summary>
//        /// <param name="targetActivityId">下一个环节的id</param>
//        /// <param name="dynamicAssignmentHandler">可以通过该参数指定下一个环节的Actor，如果这个参数不为空，则引擎忽略下一个环节的Task定义中的AssignmentHandler</param>
//        /// <param name="comments">备注信息</param>
//        void jumpTo(String targetActivityId, DynamicAssignmentHandler dynamicAssignmentHandler, String comments);// throws EngineException, KernelException;
//
//        /// <summary>超级自由流 （使用说明参考jumpTo ）</summary>
//        /// <param name="targetActivityId"></param>
//        /// <param name="dynamicAssignmentHandler"></param>
//        /// <param name="comments"></param>
//        void jumpToEx(String targetActivityId, DynamicAssignmentHandler dynamicAssignmentHandler, String comments);
//    
//        /// <summary>
//        /// 将工作项委派给其他人，自己的工作项变成CANCELED状态。返回新创建的工作项。
//        /// </summary>
//        /// <param name="actorId">接受任务的操作员Id</param>
//        /// <returns>新创建的工作项</returns>
//        IWorkItem reassignTo(String actorId);// throws EngineException;
//
//        /// <summary>
//        /// 将工作项委派给其他人，自己的工作项变成CANCELED状态。返回新创建的工作项
//        /// </summary>
//        /// <param name="actorId">接受任务的操作员Id</param>
//        /// <param name="comments">相关的备注信息</param>
//        /// <returns>新创建的工作项</returns>
//        IWorkItem reassignTo(String actorId, String comments);// throws EngineException;

    }
}
