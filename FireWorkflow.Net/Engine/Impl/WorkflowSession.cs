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
using FireWorkflow.Net.Model;
using FireWorkflow.Net.Engine;
using FireWorkflow.Net.Engine.Taskinstance;
using FireWorkflow.Net.Engine.Definition;
using FireWorkflow.Net.Engine.Persistence;
using FireWorkflow.Net.Kernel;

namespace FireWorkflow.Net.Engine.Impl
{
	public class WorkflowSession : IWorkflowSession, IRuntimeContextAware
	{
		public RuntimeContext RuntimeContext { get; set; }
		protected DynamicAssignmentHandler dynamicAssignmentHandler = null;
		protected Boolean inWithdrawOrRejectOperation = false;
		protected Dictionary<String, Object> attributes = new Dictionary<String, Object>();

		public WorkflowSession(RuntimeContext ctx)
		{
			this.RuntimeContext = ctx;
		}

		public void setCurrentDynamicAssignmentHandler(DynamicAssignmentHandler handler)
		{
			this.dynamicAssignmentHandler = handler;
		}

		public DynamicAssignmentHandler consumeCurrentDynamicAssignmentHandler()
		{
			DynamicAssignmentHandler handler = this.dynamicAssignmentHandler;
			this.dynamicAssignmentHandler = null;
			return handler;
		}


		/// <summary>通过workitem的id查找到该workitem</summary>
		/// <param name="id">workitem id</param>
		/// <returns>Workitem对象</returns>
		public IWorkItem findWorkItemById(String id)
		{
			String workItemId = id;
			try
			{
				return (IWorkItem)this.execute(new WorkflowSessionIWorkItem(id));
			}
			catch
			{
				return null;
			}
		}

		/// <summary>模板方法</summary>
		/// <param name="callbak"></param>
		/// <returns>
		/// <para>返回的对象一般有三种情况：</para>
		/// <para>1、单个工作流引擎API对象（如IProcessInstance,ITaskInstance,IworkItem等）</para>
		/// <para>2、工作流引擎对象的列表、3、null</para>
		/// </returns>
		public Object execute(IWorkflowSessionCallback callback)
		{
			try
			{
				Object result = callback.doInWorkflowSession(RuntimeContext);
				if (result != null)
				{
					if (result is IRuntimeContextAware)
					{
						((IRuntimeContextAware)result).RuntimeContext = this.RuntimeContext;
					}
					if (result is IWorkflowSessionAware)
					{
						((IWorkflowSessionAware)result).CurrentWorkflowSession = this;
					}

					if (result is List<Object>)
					{
						List<Object> l = (List<Object>)result;
						for (int i = 0; i < l.Count; i++)
						{
							Object item = l[i];
							if (item is IRuntimeContextAware)
							{
								((IRuntimeContextAware)item).RuntimeContext = this.RuntimeContext;
							}
							if (item is IWorkflowSessionAware)
							{
								((IWorkflowSessionAware)item).CurrentWorkflowSession = this;
							}
						}
					}
				}
				return result;
			}
			finally
			{

			}
		}

		/// <summary>根据任务实例的Id查找任务实例。</summary>
		/// <param name="id">任务实例的Id</param>
		/// <returns></returns>
		public ITaskInstance findTaskInstanceById(String id)
		{
			String taskInstanceId = id;
			try
			{
				return (ITaskInstance)this.execute(new WorkflowSessionITaskInstance(taskInstanceId));
			}
			catch
			{
				return null;
			}
		}

		public List<IWorkItem> findMyTodoWorkItems(String actorId)
		{
			List<IWorkItem> result = null;
			try
			{
				result = (List<IWorkItem>)this.execute(new WorkflowSessionIWorkItems(actorId));
			}
			catch { }
			return result;
		}

		public List<IWorkItem> findMyTodoWorkItems(String actorId, String processInstanceId)
		{
			List<IWorkItem> result = null;
			try
			{
				result = (List<IWorkItem>)this.execute(new WorkflowSessionIWorkItems(actorId, processInstanceId));
			}
			catch { }
			return result;
		}

		public List<IWorkItem> findMyTodoWorkItems(String actorId, String processId, String taskId)
		{
			List<IWorkItem> result = null;
			try
			{
				result = (List<IWorkItem>)this.execute(new WorkflowSessionIWorkItems(actorId, processId, taskId));

			}
			catch { }
			return result;
		}

		/// <summary>设置当前WorkflowSession是在一个取回或者拒收的操作环境中。</summary>
		/// <param name="b">true表示是在一个取回或者拒收的操作环境中；false表示不是在取回或者拒收的操作环境中</param>
		public void setWithdrawOrRejectOperationFlag(Boolean b)
		{
			this.inWithdrawOrRejectOperation = b;
		}

		public Boolean isInWithdrawOrRejectOperation()
		{
			return this.inWithdrawOrRejectOperation;
		}

		public void setDynamicAssignmentHandler(DynamicAssignmentHandler dynamicAssignmentHandler)
		{
			this.dynamicAssignmentHandler = dynamicAssignmentHandler;
		}

		public IProcessInstance abortProcessInstance(String processInstanceId)
		{
			IProcessInstance processInstance = this.findProcessInstanceById(processInstanceId);
			ProcessInstanceHelper.abort(processInstance);
			return processInstance;
		}

		public IWorkItem claimWorkItem(String workItemId)
		{
			IWorkItem result = null;
			IWorkItem wi = this.findWorkItemById(workItemId);
			result = WorkItemHelper.claim(wi);
			return result;
		}

		public void completeWorkItem(String workItemId)
		{
			IWorkItem wi = this.findWorkItemById(workItemId);
			WorkItemHelper.complete(wi);

		}

		public void completeWorkItem(String workItemId, String comments)
		{
			IWorkItem wi = this.findWorkItemById(workItemId);
			WorkItemHelper.complete(wi,comments);
		}

		public void completeWorkItem(String workItemId, DynamicAssignmentHandler dynamicAssignmentHandler, String comments)
		{
			IWorkItem wi = this.findWorkItemById(workItemId);
			WorkItemHelper.complete(wi,dynamicAssignmentHandler, comments);

		}

		public void completeWorkItemAndJumpTo(String workItemId, String targetActivityId)
		{
			IWorkItem wi = this.findWorkItemById(workItemId);
			WorkItemHelper.jumpTo(wi,targetActivityId);
		}

		public void completeWorkItemAndJumpTo(String workItemId, String targetActivityId, String comments)
		{
			IWorkItem wi = this.findWorkItemById(workItemId);
			WorkItemHelper.jumpTo(wi,targetActivityId, comments);
		}

		public void completeWorkItemAndJumpTo(String workItemId, String targetActivityId,
		                                      DynamicAssignmentHandler dynamicAssignmentHandler, String comments)
		{
			IWorkItem wi = this.findWorkItemById(workItemId);
			WorkItemHelper.jumpTo(wi,targetActivityId, dynamicAssignmentHandler, comments);
		}

		public void completeWorkItemAndJumpToEx(String workItemId, String targetActivityId,
		                                        DynamicAssignmentHandler dynamicAssignmentHandler, String comments)
		{
			IWorkItem wi = this.findWorkItemById(workItemId);
			WorkItemHelper.jumpToEx(wi,targetActivityId, dynamicAssignmentHandler, comments);
		}

		public IProcessInstance findProcessInstanceById(String id)
		{
			try
			{
				return (IProcessInstance)this.execute(new WorkflowSessionIProcessInstance1(id));
			}
			catch { return null; }
		}

		public List<IProcessInstance> findProcessInstancesByProcessId(String processId)
		{
			try
			{
				return (List<IProcessInstance>)this.execute(new WorkflowSessionIProcessInstances(processId));
			}
			catch { return null; }
		}

		public List<IProcessInstance> findProcessInstancesByProcessIdAndVersion(String processId, Int32 version)
		{
			try
			{
				return (List<IProcessInstance>)this.execute(new WorkflowSessionIProcessInstances(processId, version));

			}
			catch { return null; }
		}

		/// <summary>
		/// <para>查询流程实例的所有的TaskInstance,如果activityId不为空，则返回该流程实例下指定环节的TaskInstance</para>
		/// (Engine没有引用到该方法，提供给业务系统使用，20090303)
		/// </summary>
		/// <param name="processInstanceId">the id of the process instance</param>
		/// <param name="activityId">if the activityId is null, then return all the taskinstance of the processinstance;</param>
		/// <returns>符合条件的TaskInstance列表</returns>
		public List<ITaskInstance> findTaskInstancesForProcessInstance(String processInstanceId, String activityId)
		{
			try
			{
				return (List<ITaskInstance>)this.execute(new WorkflowSessionITaskInstances(processInstanceId, activityId));
			}
			catch { return null; }
		}

		/// <summary>将工作项委派给其他人，自己的工作项变成CANCELED状态</summary>
		/// <param name="workItemId">工作项Id</param>
		/// <param name="actorId">接受任务的操作员Id</param>
		/// <returns>新创建的工作项</returns>
		public IWorkItem reasignWorkItemTo(String workItemId, String actorId)
		{
			IWorkItem workItem = this.findWorkItemById(workItemId);
			return WorkItemHelper.reassignTo(workItem,actorId);
		}

		/// <summary>将工作项委派给其他人，自己的工作项变成CANCELED状态。返回新创建的工作项</summary>
		/// <param name="workItemId">工作项Id</param>
		/// <param name="actorId">接受任务的操作员Id</param>
		/// <param name="comments">相关的备注信息</param>
		/// <returns>新创建的工作项</returns>
		public IWorkItem reasignWorkItemTo(String workItemId, String actorId, String comments)
		{
			IWorkItem workItem = this.findWorkItemById(workItemId);
			return WorkItemHelper.reassignTo(workItem,actorId, comments);
		}

		/// <summary>
		/// <para>执行“拒收”操作，可以对已经签收的或者未签收的WorkItem拒收。</para>
		/// <para>该操作必须满足如下条件：</para>
		/// <para>1、前驱环节中没有没有Tool类型和Subflow类型的Task；</para>
		/// <para>2、没有合当前TaskInstance并行的其他TaskInstance；</para>
		/// </summary>
		/// <param name="workItemId">工作项Id</param>
		public void rejectWorkItem(String workItemId)
		{
			IWorkItem workItem = this.findWorkItemById(workItemId);
			WorkItemHelper.reject(workItem);
		}

		/// <summary>
		/// <para>执行“拒收”操作，可以对已经签收的或者未签收的WorkItem拒收。</para>
		/// <para>该操作必须满足如下条件：</para>
		/// <para>1、前驱环节中没有没有Tool类型和Subflow类型的Task；</para>
		/// <para>2、没有合当前TaskInstance并行的其他TaskInstance；</para>
		/// </summary>
		/// <param name="workItemId">工作项Id</param>
		/// <param name="comments">备注信息</param>
		public void rejectWorkItem(String workItemId, String comments)
		{
			IWorkItem workItem = this.findWorkItemById(workItemId);
			WorkItemHelper.reject(workItem,comments);
		}

		/// <summary>恢复被挂起的流程实例</summary>
		/// <param name="processInstanceId">流程实例Id</param>
		public IProcessInstance restoreProcessInstance(String processInstanceId)
		{
			IProcessInstance processInstance = this.findProcessInstanceById(processInstanceId);
			ProcessInstanceHelper.restore(processInstance);
			return processInstance;
		}

		/// <summary>恢复被挂起的TaskInstance</summary>
		/// <param name="taskInstanceId"></param>
		/// <returns></returns>
		public ITaskInstance restoreTaskInstance(String taskInstanceId)
		{
			ITaskInstance taskInst = this.findTaskInstanceById(taskInstanceId);
			TaskInstanceHelper.restore(taskInst);
			//taskInst.restore();
			return taskInst;
		}

		public IProcessInstance suspendProcessInstance(String processInstanceId)
		{
			IProcessInstance processInstance = this.findProcessInstanceById(processInstanceId);
			ProcessInstanceHelper.suspend(processInstance);
			return processInstance;
		}

		public ITaskInstance suspendTaskInstance(String taskInstanceId)
		{
			ITaskInstance taskInst = this.findTaskInstanceById(taskInstanceId);
			//taskInst.suspend();
			TaskInstanceHelper.suspend(taskInst);
			return taskInst;
		}

		public IWorkItem withdrawWorkItem(String workItemId)
		{
			IWorkItem wi = this.findWorkItemById(workItemId);
			return WorkItemHelper.withdraw(wi);
		}

		public void clearAttributes()
		{
			this.attributes.Clear();
		}

		public Object getAttribute(String name)
		{
			return this.attributes[name];
		}

		public void removeAttribute(String name)
		{
			this.attributes.Remove(name);
		}

		public void setAttribute(String name, Object attr)
		{
			this.attributes.Add(name, attr);
		}
	}
}
