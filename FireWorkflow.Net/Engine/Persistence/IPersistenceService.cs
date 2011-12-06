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
using FireWorkflow.Net.Engine;
using FireWorkflow.Net.Engine.Impl;
using FireWorkflow.Net.Engine.Definition;
using FireWorkflow.Net.Kernel;

namespace FireWorkflow.Net.Engine.Persistence
{
    /// <summary>
    /// 数据存储接口，
    /// (目前该接口的方法还不够，下一步增加方法，把hibernate的QBC和QBE直接集成进来。)<br/>
    /// 约定：以下划线开头的方法只提供给引擎本身使用，这些方法都经过特定优化。
    /// 例如增加分区查询条件。
    /// </summary>
    public interface IPersistenceService : IRuntimeContextAware
    {
        /******************************************************************************/
        /************                                                        **********/
        /************            Process instance 相关的持久化方法            **********/
        /************            Persistence methods for process instance    **********/
        /************                                                        **********/
        /******************************************************************************/

        /// <summary>
        /// 插入或者更新ProcessInstance 。同时也会保存或更新流程变量<br/>
        /// Save or update processinstance. 
        /// If the processInstance.id is null then insert a new process instance record
        /// and genarate a new id for it (save operation);
        /// otherwise update the existent one.
        /// </summary>
        /// <param name="processInstance"></param>
        bool SaveOrUpdateProcessInstance(IProcessInstance processInstance);

        /// <summary>
        /// 通过ID获得“活的”ProcessInstance对象。
        /// “活的”是指ProcessInstance.state=INITIALIZED Or ProcessInstance.state=STARTED Or ProcessInstance=SUSPENDED的流程实例
        /// </summary>
        /// <param name="id">processInstance.id</param>
        /// <returns>process instance</returns>
        IProcessInstance FindAliveProcessInstanceById(String id);

        /// <summary>
        /// 通过ID获得ProcessInstance对象。
        /// (Engine没有引用到该方法，提供给业务系统使用，20090303)
        /// </summary>
        /// <param name="id">processInstance.id</param>
        /// <returns>process instance</returns>
        IProcessInstance FindProcessInstanceById(String id);

        /// <summary>
        /// 查找并返回同一个业务流程的所有实例
        /// (Engine没有引用到该方法，提供给业务系统使用，20090303)
        /// </summary>
        /// <param name="processId">The id of the process definition.</param>
        /// <returns>A list of processInstance</returns>
        IList<IProcessInstance> FindProcessInstancesByProcessId(String processId);

        /// <summary>
        /// 查找并返回同一个指定版本业务流程的所有实例
        /// (Engine没有引用到该方法，提供给业务系统使用，20090303)
        /// </summary>
        /// <param name="processId">The id of the process definition.</param>
        /// <param name="version">A list of processInstance</param>
        /// <returns></returns>
        IList<IProcessInstance> FindProcessInstancesByProcessIdAndVersion(String processId, Int32 version);

        /// <summary>
        /// 计算活动的子流程实例的数量
        /// </summary>
        /// <param name="taskInstanceId">父TaskInstance的Id</param>
        /// <returns></returns>
        Int32 GetAliveProcessInstanceCountForParentTaskInstance(String taskInstanceId);

        /// <summary>
        /// 终止流程实例。将流程实例、活动的TaskInstance、活动的WorkItem的状态设置为CANCELED；并删除所有的token
        /// </summary>
        /// <param name="processInstance"></param>
        bool AbortProcessInstance(IProcessInstance processInstance);

        /// <summary>
        /// 挂起流程实例
        /// </summary>
        /// <param name="processInstance"></param>
        bool SuspendProcessInstance(IProcessInstance processInstance);

        /// <summary>
        /// 恢复流程实例
        /// </summary>
        /// <param name="processInstance"></param>
        bool RestoreProcessInstance(IProcessInstance processInstance);

        /******************************************************************************/
        /************                                                        **********/
        /************            与流程变量相关的持久化方法                    **********/
        /************            Persistence methods for process instance    **********/
        /************                                                        **********/
        /******************************************************************************/

        /// <summary>查询流程实例的所有变量</summary>
        /// <param name="processInstanceId">流程实例的Id</param>
        /// <returns>流程实例的所有变量</returns>
        IList<IProcessInstanceVar> FindProcessInstanceVariable(String processInstanceId);

        IProcessInstanceVar FindProcessInstanceVariable(String processInstanceId, String name);

        bool UpdateProcessInstanceVariable(IProcessInstanceVar var);

        bool SaveProcessInstanceVariable(IProcessInstanceVar var);

        /******************************************************************************/
        /************                                                        **********/
        /************            task instance 相关的持久化方法               **********/
        /************            Persistence methods for task instance       **********/
        /************                                                        **********/
        /******************************************************************************/

        /// <summary>
        /// 插入或者更新TaskInstance。<br/>
        /// Save or update task instance. If the taskInstance.id is null then insert a new task instance record
        /// and generate a new id for it ;
        /// otherwise update the existent one. 
        /// </summary>
        /// <param name="taskInstance"></param>
        bool SaveOrUpdateTaskInstance(ITaskInstance taskInstance);

        /// <summary>
        /// 终止TaskInstance。将任务实例及其所有的“活的”WorkItem变成Canceled状态。<br/>
        /// "活的"WorkItem 是指状态等于INITIALIZED、STARTED或者SUSPENDED的WorkItem.
        /// </summary>
        /// <param name="taskInstance"></param>
        bool AbortTaskInstance(ITaskInstance taskInstance);

        /// <summary>
        /// 返回“活的”TaskInstance。<br/>
        /// “活的”是指TaskInstance.state=INITIALIZED Or TaskInstance.state=STARTED 。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ITaskInstance FindAliveTaskInstanceById(String id);

        /// <summary>
        /// 获得activity的“活的”TaskInstance的数量<br/>
        /// “活的”是指TaskInstance.state=INITIALIZED Or TaskInstance.state=STARTED 。
        /// </summary>
        /// <param name="processInstanceId"></param>
        /// <param name="activityId"></param>
        /// <returns></returns>
        Int32 GetAliveTaskInstanceCountForActivity(String processInstanceId, String activityId);

        /// <summary>
        /// 返回某个Task已经结束的TaskInstance的数量。<br/>
        /// “已经结束”是指TaskInstance.state=COMPLETED。
        /// </summary>
        /// <param name="processInstanceId"></param>
        /// <param name="taskId"></param>
        /// <returns></returns>
        Int32 GetCompletedTaskInstanceCountForTask(String processInstanceId, String taskId);

        /// <summary>
        /// Find the task instance by id
        /// (Engine没有引用到该方法，提供给业务系统使用，20090303)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ITaskInstance FindTaskInstanceById(String id);

        /// <summary>
        /// 查询流程实例的所有的TaskInstance,如果activityId不为空，则返回该流程实例下指定环节的TaskInstance<br/>
        /// (Engine没有引用到该方法，提供给业务系统使用，20090303)
        /// </summary>
        /// <param name="processInstanceId">the id of the process instance</param>
        /// <param name="activityId">if the activityId is null, then return all the taskinstance of the processinstance;</param>
        /// <returns></returns>
        IList<ITaskInstance> FindTaskInstancesForProcessInstance(String processInstanceId, String activityId);

        /// <summary>
        /// 查询出同一个stepNumber的所有TaskInstance实例
        /// </summary>
        /// <param name="processInstanceId"></param>
        /// <param name="stepNumber"></param>
        /// <returns></returns>
        IList<ITaskInstance> FindTaskInstancesForProcessInstanceByStepNumber(String processInstanceId, Int32 stepNumber);

        /// <summary>
        /// 调用数据库自身的机制所定TaskInstance实例。<br/>
        /// 该方法主要用于工单的签收操作，在签收之前先锁定与之对应的TaskInstance。
        /// </summary>
        /// <param name="taskInstanceId"></param>
        bool LockTaskInstance(String taskInstanceId);


        /******************************************************************************/
        /************                                                        **********/
        /************            workItem 相关的持久化方法                    **********/
        /************            Persistence methods for workitem            **********/
        /************                                                        **********/
        /******************************************************************************/

        /// <summary>
        /// 插入或者更新WorkItem<br/>
        /// save or update workitem
        /// </summary>
        /// <param name="workitem"></param>
        bool SaveOrUpdateWorkItem(IWorkItem workitem);

        /// <summary>
        /// 返回任务实例的所有"活的"WorkItem的数量。
        /// "活的"WorkItem 是指状态等于INITIALIZED、STARTED或者SUSPENDED的WorkItem。
        /// </summary>
        /// <param name="taskInstanceId"></param>
        /// <returns></returns>
        Int32 GetAliveWorkItemCountForTaskInstance(String taskInstanceId);

        /// <summary>
        /// 查询任务实例的所有"已经结束"WorkItem。
        /// 所以必须有关联条件WorkItem.state=IWorkItem.COMPLTED 
        /// </summary>
        /// <param name="taskInstanceId">任务实例Id</param>
        /// <returns></returns>
        IList<IWorkItem> FindCompletedWorkItemsForTaskInstance(String taskInstanceId);

        /// <summary>
        /// 查询某任务实例的所有WorkItem
        /// </summary>
        /// <param name="taskInstanceId"></param>
        /// <returns></returns>
        IList<IWorkItem> FindWorkItemsForTaskInstance(String taskInstanceId);

        /// <summary>
        /// 删除处于初始化状态的workitem。
        /// 此方法用于签收Workitem时，删除其他Actor的WorkItem
        /// </summary>
        /// <param name="taskInstanceId"></param>
        bool DeleteWorkItemsInInitializedState(String taskInstanceId);

        /// <summary>
        /// Find workItem by id
        /// (Engine没有引用到该方法，提供给业务系统使用，20090303)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IWorkItem FindWorkItemById(String id);

        /// <summary>
        /// Find all workitems for task
        /// (Engine没有引用到该方法，提供给业务系统使用，20090303)
        /// </summary>
        /// <param name="taskid"></param>
        /// <returns></returns>
        IList<IWorkItem> FindWorkItemsForTask(String taskid);

        /// <summary>
        /// 根据操作员的Id返回其待办工单。如果actorId==null，则返回系统所有的待办任务<br/>
        /// 待办工单是指状态等于INITIALIZED或STARTED工单<br/>
        /// (Engine没有引用到该方法，提供给业务系统使用，20090303)
        /// </summary>
        /// <param name="actorId"></param>
        /// <returns></returns>
        IList<IWorkItem> FindTodoWorkItems(String actorId);

        /// <summary>
        /// 查找操作员在某个流程实例中的待办工单。
        /// 如果processInstanceId为空，则等价于调用findTodoWorkItems(String actorId)
        /// 待办工单是指状态等于INITIALIZED或STARTED工单<br/>
        /// (Engine没有引用到该方法，提供给业务系统使用，20090303)
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="processInstanceId"></param>
        /// <returns></returns>
        IList<IWorkItem> FindTodoWorkItems(String actorId, String processInstanceId);

        /// <summary>
        /// 查找操作员在某个流程某个任务上的待办工单。
        /// actorId，processId，taskId都可以为空（null或者""）,为空的条件将被忽略
        /// 待办工单是指状态等于INITIALIZED或STARTED工单<br/>
        /// (Engine没有引用到该方法，提供给业务系统使用，20090303)
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="processId"></param>
        /// <param name="taskId"></param>
        /// <returns></returns>
        IList<IWorkItem> FindTodoWorkItems(String actorId, String processId, String taskId);

        /// <summary>
        /// 根据操作员的Id返回其已办工单。如果actorId==null，则返回系统所有的已办任务
        /// 已办工单是指状态等于COMPLETED或CANCELED的工单<br/>
        /// (Engine没有引用到该方法，提供给业务系统使用，20090303)
        /// </summary>
        /// <param name="actorId"></param>
        /// <returns></returns>
        IList<IWorkItem> FindHaveDoneWorkItems(String actorId);

        /// <summary>
        /// 查找操作员在某个流程实例中的已办工单。
        /// 如果processInstanceId为空，则等价于调用findHaveDoneWorkItems(String actorId)
        /// 已办工单是指状态等于COMPLETED或CANCELED的工单<br/>
        /// (Engine没有引用到该方法，提供给业务系统使用，20090303)
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="processInstanceId"></param>
        /// <returns></returns>
        IList<IWorkItem> FindHaveDoneWorkItems(String actorId, String processInstanceId);

        /// <summary>
        /// 查找操作员在某个流程某个任务上的已办工单。
        /// actorId，processId，taskId都可以为空（null或者""）,为空的条件将被忽略
        /// 已办工单是指状态等于COMPLETED或CANCELED的工单<br/>
        /// (Engine没有引用到该方法，提供给业务系统使用，20090303)
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="processId"></param>
        /// <param name="taskId"></param>
        /// <returns></returns>
        IList<IWorkItem> FindHaveDoneWorkItems(String actorId, String processId, String taskId);

        /******************************************************************************/
        /************                                                        **********/
        /************            token 相关的持久化方法                       **********/
        /************            Persistence methods for token               **********/
        /************                                                        **********/
        /******************************************************************************/

        /// <summary>Save token</summary>
        /// <param name="token"></param>
        bool SaveOrUpdateToken(IToken token);

        /// <summary>统计流程任意节点的活动Token的数量。对于Activity节点，该数量只能取值1或者0，大于1表明有流程实例出现异常。</summary>
        Int32 GetAliveTokenCountForNode(String processInstanceId, String nodeId);

        /// <summary>(Engine没有引用到该方法，提供给业务系统使用，20090303)</summary>
        IToken FindTokenById(String id);

        /// <summary>
        /// Find all the tokens for process instance ,and the nodeId of the token must Equals to the second argument.
        /// </summary>
        /// <param name="processInstanceId">the id of the process instance</param>
        /// <param name="nodeId">if the nodeId is null ,then return all the tokens of the process instance.</param>
        /// <returns></returns>
        IList<IToken> FindTokensForProcessInstance(String processInstanceId, String nodeId);

        /// <summary>删除某个节点的所有token</summary>
        bool DeleteTokensForNode(String processInstanceId, String nodeId);

        /// <summary>删除某些节点的所有token</summary>
        bool DeleteTokensForNodes(String processInstanceId, IList<String> nodeIdsList);

        /// <summary>删除token</summary>
        bool DeleteToken(IToken token);

        /******************************************************************************/
        /************                                                        **********/
        /************            存取流程定义文件 相关的持久化方法             **********/
        /************            Persistence methods for workflow definition **********/
        /************                                                        **********/
        /******************************************************************************/

        /// <summary>
        /// Save or update the workflow definition. The version will be increased automatically when insert a new record.
        /// 保存流程定义，如果同一个ProcessId的流程定义已经存在，则版本号自动加1。
        /// </summary>
        bool SaveOrUpdateWorkflowDefinition(IWorkflowDefinition workflowDef);

        /// <summary>
        /// Find the workflow definition by id .
        /// 根据纪录的ID返回流程定义
        /// </summary>
        IWorkflowDefinition FindWorkflowDefinitionById(String id);

        /// <summary>
        /// Find workflow definition by workflow process id and version
        /// 根据ProcessId和版本号返回流程定义
        /// </summary>
        IWorkflowDefinition FindWorkflowDefinitionByProcessIdAndVersionNumber(String processId, int version);

        /// <summary>
        /// Find the latest version of the workflow definition.
        /// 根据processId返回最新版本的有效流程定义
        /// </summary>
        /// <param name="processId">the workflow process id</param>
        IWorkflowDefinition FindTheLatestVersionOfWorkflowDefinitionByProcessId(String processId);

        /// <summary>
        /// Find all the workflow definitions for the workflow process id.
        /// 根据ProcessId 返回所有版本的流程定义
        /// </summary>
        IList<IWorkflowDefinition> FindWorkflowDefinitionsByProcessId(String processId);

        /// <summary>
        /// Find all of the latest version of workflow definitions.
        /// 返回系统中所有的最新版本的有效流程定义
        /// </summary>
        IList<IWorkflowDefinition> FindAllTheLatestVersionsOfWorkflowDefinition();

        /// <summary>
        /// Find the latest version number 
        /// 返回最新的有效版本号
        /// </summary>
        /// <param name="processId"></param>
        /// <returns>the version number ,null if there is no workflow definition stored in the DB.</returns>
        Int32 FindTheLatestVersionNumber(String processId);

        /// <summary>返回最新版本号</summary>
        Int32 FindTheLatestVersionNumberIgnoreState(String processId);



        /********************************process instance trace info **********************/
        bool SaveOrUpdateProcessInstanceTrace(IProcessInstanceTrace processInstanceTrace);


        /********************************process instance trace info **********************/

        /// <summary>
        /// 根据流程实例ID查找流程实例运行轨迹
        /// </summary>
        /// <param name="processInstanceId">流程实例ID</param>
        /// <returns></returns>
        IList<IProcessInstanceTrace> FindProcessInstanceTraces(String processInstanceId);

        /******************************** lifw555@gmail.com **********************/

        /// <summary>
        /// 获得操作员所要操作工单的总数量
        /// publishUser如果为null，获取全部
        /// </summary>
        /// <param name="actorId">操作员主键</param>
        /// <param name="publishUser">流程定义发布者</param>
        /// <returns></returns>
        Int32 GetTodoWorkItemsCount(String actorId, String publishUser);

        /// <summary>
        /// 获得操作员所要操作工单列表（分页）
        /// publishUser如果为null，获取全部
        /// </summary>
        /// <param name="actorId">操作员主键</param>
        /// <param name="publishUser">流程定义发布者</param>
        /// <param name="pageSize">每页显示的条数</param>
        /// <param name="pageNumber">当前页数</param>
        /// <returns></returns>
        IList<IWorkItem> FindTodoWorkItems(String actorId, String publishUser, int pageSize, int pageNumber);

        /// <summary>
        /// 获得操作员完成的工单总数量
        /// publishUser如果为null，获取全部
        /// </summary>
        /// <param name="actorId">操作员主键</param>
        /// <param name="publishUser">流程定义发布者</param>
        /// <returns></returns>
        Int32 GetHaveDoneWorkItemsCount(String actorId, String publishUser);

        /// <summary>
        /// 获得操作员完成的工单列表（分页）
        /// publishUser如果为null，获取全部
        /// </summary>
        /// <param name="actorId">操作员主键</param>
        /// <param name="publishUser">流程定义发布者</param>
        /// <param name="pageSize">每页显示的条数</param>
        /// <param name="pageNumber">当前页数</param>
        /// <returns></returns>
        IList<IWorkItem> FindHaveDoneWorkItems(String actorId, String publishUser, int pageSize, int pageNumber);

        /// <summary>
        /// 获得操作员发起的工作流实例总数量
        /// publishUser如果为null，获取全部
        /// </summary>
        /// <param name="creatorId">操作员主键</param>
        /// <param name="publishUser">流程定义发布者</param>
        /// <returns></returns>
        Int32 GetProcessInstanceCountByCreatorId(String creatorId, String publishUser);

        /// <summary>
        /// 获得操作员发起的工作流实例列表（分页）
        /// publishUser如果为null，获取全部
        /// </summary>
        /// <param name="creatorId">操作员主键</param>
        /// <param name="publishUser">流程定义发布者</param>
        /// <param name="pageSize">每页显示的条数</param>
        /// <param name="pageNumber">当前页数</param>
        /// <returns></returns>
        IList<IProcessInstance> FindProcessInstanceListByCreatorId(String creatorId, String publishUser, int pageSize, int pageNumber);

        /// <summary>
        /// 获得工作流发布者发起的所有流程定义的工作流实例总数量
        /// </summary>
        /// <param name="publishUser">工作流发布者</param>
        /// <returns></returns>
        Int32 GetProcessInstanceCountByPublishUser(String publishUser);

        /// <summary>
        /// 获得工作流发布者发起的所有流程定义的工作流实例列表（分页）
        /// </summary>
        /// <param name="publishUser">工作流发布者</param>
        /// <param name="pageSize">每页显示的条数</param>
        /// <param name="pageNumber">当前页数</param>
        /// <returns></returns>
        IList<IProcessInstance> FindProcessInstanceListByPublishUser(String publishUser, int pageSize, int pageNumber);

    }
}
