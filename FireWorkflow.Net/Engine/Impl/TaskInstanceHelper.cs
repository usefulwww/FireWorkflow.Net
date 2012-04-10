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
using FireWorkflow.Net.Engine.Definition;
using FireWorkflow.Net.Engine.Persistence;
using FireWorkflow.Net.Engine.Taskinstance;
using FireWorkflow.Net.Kernel;
using FireWorkflow.Net.Model;
using FireWorkflow.Net.Model.Net;

namespace FireWorkflow.Net.Engine.Impl
{
	/// <summary>
	/// Description of
	/// </summary>
	public class TaskInstanceHelper
	{
		public TaskInstanceHelper()
		{
		}
		
		/// <summary>
		/// 将TaskInstance分配给编号为actorId的操作员。即系统只创建一个WorkItem，并分配给编号为actorId的操作员
		/// 该WorkItem需要签收
		/// </summary>
		/// <param name="actorId">操作员Id</param>
		/// <returns>返回创建的WorkItem</returns>
		public static IWorkItem assignToActor(ITaskInstance t,String id)// throws EngineException, KernelException
		{
			ITaskInstanceManager taskInstanceMgr = RuntimeContextFactory.getRuntimeContext().TaskInstanceManager;
			IWorkflowSession workflowSession = RuntimeContextFactory.getRuntimeContext().getWorkflowSession(t.ProcessInstanceId);
			WorkItem wi = taskInstanceMgr.createWorkItem(workflowSession, getAliveProcessInstance(t), t, id);
			return wi;
		}

		/// <summary>
		/// 将TaskInstance分配给列表中的操作员。即创建N个WorkItem，每个操作员一个WorkItem，并且这些WorkItem都需要签收。
		/// 最终由那个操作员执行该任务实例，是由Task的分配策略决定的。
		/// 如果分配策略为ALL,即会签的情况，则所有的操作员都要完成相应的工单。
		/// 如果分配策略为ANY，则最先签收的那个操作员完成其工单和任务实例，其他操作员的工单被删除。
		/// </summary>
		/// <param name="actorIds">操作员Id</param>
		/// <returns>返回创建的WorkItem列表</returns>
		public static IList<IWorkItem> assignToActors(ITaskInstance t,IList<String> ids)// throws EngineException, KernelException
		{
			IWorkflowSession currWorkflowSession = RuntimeContextFactory.getRuntimeContext().getWorkflowSession(t.ProcessInstanceId);
			//task应该有一个标志(asignToEveryone)，表明asign的规则
			IList<IWorkItem> workItemList = new List<IWorkItem>();
			for (int i = 0; ids != null && i < ids.Count; i++)
			{
				ITaskInstanceManager taskInstanceMgr = RuntimeContextFactory.getRuntimeContext().TaskInstanceManager;
				WorkItem wi = taskInstanceMgr.createWorkItem(currWorkflowSession, getAliveProcessInstance(t), t, ids[i]);
				//wi.CurrentWorkflowSession = t.CurrentWorkflowSession;
				workItemList.Add(wi);
			}
			return workItemList;
		}


		public /*final*/ static void start(ITaskInstance t)
		{
			IWorkflowSession currWorkflowSession = RuntimeContextFactory.getRuntimeContext().getWorkflowSession(t.ProcessInstanceId);
			
			ITaskInstanceManager taskInstanceMgr = RuntimeContextFactory.getRuntimeContext().TaskInstanceManager;
			taskInstanceMgr.startTaskInstance(currWorkflowSession, getAliveProcessInstance(t), t);
			//        taskInstanceMgr.startTaskInstance(this);
		}

		public static void complete(ITaskInstance t,IActivityInstance targetActivityInstance)
		{
			IWorkflowSession currWorkflowSession = RuntimeContextFactory.getRuntimeContext().getWorkflowSession(t.ProcessInstanceId);
			ITaskInstanceManager taskInstanceMgr = RuntimeContextFactory.getRuntimeContext().TaskInstanceManager;
			taskInstanceMgr.completeTaskInstance(currWorkflowSession, getAliveProcessInstance(t), t, targetActivityInstance);
			//        taskInstanceMgr.completeTaskInstance(this, targetActivityInstance);
		}
		
		/// <summary>挂起 </summary>
		public static void suspend(ITaskInstance t)
		{
			if (t.State == TaskInstanceStateEnum.COMPLETED || t.State == TaskInstanceStateEnum.CANCELED)
			{
				throw new EngineException(getAliveProcessInstance(t), getTask(t), "The task instance can not be suspended,the state of this task instance is " + t.State);
			}
			if (t.Suspended)
			{
				return;
			}
			t.Suspended = true;
			IPersistenceService persistenceService = RuntimeContextFactory.getRuntimeContext().PersistenceService;
			persistenceService.SaveOrUpdateTaskInstance(t);
		}

		/// <summary>
		/// 从挂起状态恢复到挂起前的状态
		/// fireflow.engine.EngineException
		/// </summary>
		public static void restore(ITaskInstance t)
		{
			if (t.State == TaskInstanceStateEnum.COMPLETED || t.State == TaskInstanceStateEnum.CANCELED)
			{
				throw new EngineException(getAliveProcessInstance(t), getTask(t), "The task instance can not be restored,the state of this task instance is " + t.State);
			}
			if (!t.Suspended)
			{
				return;
			}
			t.Suspended = false;
			IPersistenceService persistenceService = RuntimeContextFactory.getRuntimeContext().PersistenceService;
			persistenceService.SaveOrUpdateTaskInstance(t);
		}

		/// <summary>
		/// <para>将该TaskInstance中止，并且使得当前流程实例按照流程定义往下流转。</para>
		/// <para>与该TaskInstance相关的WorkItem都被置为Canceled状态。</para>
		/// 如果该TaskInstance的状态已经是Completed或者Canceled，则抛出异常。
		/// </summary>
		public static void abort(ITaskInstance t)
		{
			abort(t,null);

		}

		/// <summary>
		/// 将该TaskInstance中止，并且使得当前流程实例跳转到targetActivityId指定的环节。
		/// 与该TaskInstance相关的WorkItem都被置为Canceled状态。
		/// 如果该TaskInstance的状态已经是Completed或者Canceled，则抛出异常。
		/// </summary>
		/// <param name="targetActivityId"></param>
		public static void abort(ITaskInstance t,String targetActivityId)
		{
			abort(t,targetActivityId, null);

		}

		/// <summary>
		/// <para>将该TaskInstance中止，并且使得当前流程实例跳转到targetActivityId指定的环节。该环节任务的操作人从dynamicAssignmentHandler获取。</para>
		/// <para>当前环节和目标环节必须在同一条“执行线”上</para>
		/// <para>与该TaskInstance相关的WorkItem都被置为Canceled状态。</para>
		/// 如果该TaskInstance的状态已经是Completed或者Canceled，则抛出异常。
		/// </summary>
		/// <param name="targetActivityId"></param>
		/// <param name="dynamicAssignmentHandler"></param>
		public static void abort(ITaskInstance t,String targetActivityId, DynamicAssignmentHandler dynamicAssignmentHandler)
		{
			IWorkflowSession currWorkflowSession = RuntimeContextFactory.getRuntimeContext().getWorkflowSession(t.ProcessInstanceId);
			
			//if (t.CurrentWorkflowSession == null)
			//{
			//    new EngineException(t.ProcessInstanceId,
			//                        getWorkflowProcess(t), t.TaskId,
			//                        "The current workflow session is null.");
			//}
//			if (t.RuntimeContext == null)
//			{
//				new EngineException(t.ProcessInstanceId,
//				                    getWorkflowProcess(t), t.TaskId,
//				                    "The current runtime context is null.");
//			}

			if ((t.State == TaskInstanceStateEnum.COMPLETED) ||
			    (t.State == TaskInstanceStateEnum.CANCELED))
			{
				throw new EngineException(t.ProcessInstanceId,getWorkflowProcess(t),
				                          t.TaskId,
				                          "Abort task instance failed . The state of the task instance [id=" + t.Id + "] is " + t.State);
			}

			if (dynamicAssignmentHandler != null)
			{
				currWorkflowSession.setDynamicAssignmentHandler(dynamicAssignmentHandler);
			}


			ITaskInstanceManager taskInstanceMgr = RuntimeContextFactory.getRuntimeContext().TaskInstanceManager;
			taskInstanceMgr.abortTaskInstance(currWorkflowSession, getAliveProcessInstance(t), t, targetActivityId);

		}

		/// <summary>
		/// <para>将该TaskInstance中止，并且使得当前流程实例跳转到targetActivityId指定的环节。该环节任务的操作人从dynamicAssignmentHandler获取。</para>
		/// <para>当前环节和目标环节 可以不在 同一条“执行线”上</para>
		/// <para>与该TaskInstance相关的WorkItem都被置为Canceled状态。</para>
		/// 如果该TaskInstance的状态已经是Completed或者Canceled，则抛出异常。
		/// </summary>
		/// <param name="targetActivityId"></param>
		/// <param name="dynamicAssignmentHandler"></param>
		public static void abortEx(ITaskInstance t,String targetActivityId, DynamicAssignmentHandler dynamicAssignmentHandler)
		{
			IWorkflowSession currWorkflowSession = RuntimeContextFactory.getRuntimeContext().getWorkflowSession(t.ProcessInstanceId);
			//if (t.CurrentWorkflowSession == null)
			//{
			//    new EngineException(t.ProcessInstanceId,
			//                        getWorkflowProcess(t), t.TaskId,
			//                        "The current workflow session is null.");
			//}
//			if (t.RuntimeContext == null)
//			{
//				new EngineException(t.ProcessInstanceId,
//				                    getWorkflowProcess(t), t.TaskId,
//				                    "The current runtime context is null.");
//			}

			if ((t.State == TaskInstanceStateEnum.COMPLETED) || (t.State == TaskInstanceStateEnum.CANCELED))
			{
				throw new EngineException(t.ProcessInstanceId,getWorkflowProcess(t),
				                          t.TaskId,
				                          "Abort task instance failed . The state of the task instance [id=" + t.Id + "] is " + t.State);
			}

			if (dynamicAssignmentHandler != null)
			{
				currWorkflowSession.setDynamicAssignmentHandler(dynamicAssignmentHandler);
			}
			ITaskInstanceManager taskInstanceMgr = RuntimeContextFactory.getRuntimeContext().TaskInstanceManager;
			taskInstanceMgr.abortTaskInstanceEx(currWorkflowSession, getAliveProcessInstance(t), t, targetActivityId);
		}
		
		private static Dictionary<string,IProcessInstance> processInsatances = new Dictionary<string, IProcessInstance>();
		public static IProcessInstance getAliveProcessInstance(ITaskInstance t)
		{
			
			if (!processInsatances.ContainsKey(t.Id))
			{
				//if (t.RuntimeContext != null)
				//{
				IPersistenceService persistenceService = RuntimeContextFactory.getRuntimeContext().PersistenceService;
				//this.processInsatance = persistenceService.FindAliveProcessInstanceById(this.ProcessInstanceId); //解决流程结束任务未完成无法继续问题。
				processInsatances[t.Id] = persistenceService.FindProcessInstanceById(t.ProcessInstanceId);//获取存在的流程
				//}
			}
			//if (processInsatances.ContainsKey(t.Id)&&processInsatances[t.Id]!=null)
			//{
			//if (t.CurrentWorkflowSession != null)
			//{
			//    processInsatances[t.Id].CurrentWorkflowSession = t.CurrentWorkflowSession;
			//}
//				if (t.RuntimeContext != null)
//				{
//					processInsatances[t.Id].RuntimeContext = t.RuntimeContext;
//				}
			//}
			return processInsatances[t.Id];
			
		}
		
		///FIXME 用静态Map缓存数据
		public static Task getTask(ITaskInstance t)
		{
			
			//if (t.RuntimeContext == null) return null; //System.out.println("====Inside taskInstance this.RuntimeContext is null");
			IDefinitionService definitionService = RuntimeContextFactory.getRuntimeContext().DefinitionService;
			if (definitionService == null) return null;//System.out.println("====Inside taskInstance definitionService is null");
			IWorkflowDefinition workflowDef = definitionService.GetWorkflowDefinitionByProcessIdAndVersionNumber(t.ProcessId, t.Version);
			if (workflowDef == null)
			{
				return null;
			}
			return (Task)WorkflowDefinitionHelper.getWorkflowProcess(workflowDef).findWFElementById(t.TaskId);
			
		}
		
		///FIXME 用静态Map缓存数据
		public static Activity getActivity(ITaskInstance t)
		{
			
			IWorkflowDefinition workflowDef = RuntimeContextFactory.getRuntimeContext().DefinitionService.GetWorkflowDefinitionByProcessIdAndVersionNumber(t.ProcessId, t.Version);
			if (workflowDef == null)
			{
				return null;
			}
			return (Activity)WorkflowDefinitionHelper.getWorkflowProcess(workflowDef).findWFElementById(t.ActivityId);
			
		}
		
		///FIXME 用静态Map缓存数据
		public static WorkflowProcess getWorkflowProcess(ITaskInstance t)
		{
			
			IWorkflowDefinition workflowDef = RuntimeContextFactory.getRuntimeContext().DefinitionService.GetWorkflowDefinitionByProcessIdAndVersionNumber(t.ProcessId, t.Version);
			if (workflowDef == null)
			{
				return null;
			}
			return WorkflowDefinitionHelper.getWorkflowProcess(workflowDef);
			
		}
		
	}
}
