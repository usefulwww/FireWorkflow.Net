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
 * @Revision 蓝天白云远兮 lyun@nashihou.cn 2011-12
 */
using System;
using System.Collections.Generic;
using FireWorkflow.Net.Engine.Definition;
using FireWorkflow.Net.Engine.Event;
using FireWorkflow.Net.Engine.Persistence;
using FireWorkflow.Net.Kernel;
using FireWorkflow.Net.Kernel.Impl;
using FireWorkflow.Net.Model;

namespace FireWorkflow.Net.Engine.Impl
{

	public class ProcessInstanceHelper
	{
		public ProcessInstanceHelper()
		{
		}
		
		/// <summary>生成joinPoint</summary>
		/// <param name="synchInst"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		public static IJoinPoint createJoinPoint(IProcessInstance p,ISynchronizerInstance synchInst, IToken token)// throws EngineException
		{

			int enterTransInstanceCount = synchInst.EnteringTransitionInstances.Count;
			if (enterTransInstanceCount == 0)//检查流程定义是否合法，同步器节点必须有输入边
			{
				throw new EngineException(p.Id, getWorkflowProcess(p),
				                          synchInst.Synchronizer.Id, "The process definition [" + p.Name + "] is invalid，the synchronizer[" + synchInst.Synchronizer + "] has no entering transition");
			}
			IPersistenceService persistenceService = RuntimeContextFactory.getRuntimeContext().PersistenceService;
			//保存到数据库
			persistenceService.SaveOrUpdateToken(token);

			IJoinPoint resultJoinPoint = null;
			resultJoinPoint = new JoinPoint();
			resultJoinPoint.ProcessInstance=p;
			resultJoinPoint.SynchronizerId=synchInst.Synchronizer.Id;
			if (enterTransInstanceCount == 1)
			{
				// 生成一个不存储到数据库中的JoinPoint
				resultJoinPoint.addValue(token.Value);

				if (token.IsAlive)
				{
					resultJoinPoint.Alive=true;
					resultJoinPoint.FromActivityId=token.FromActivityId;
				}
				resultJoinPoint.StepNumber=token.StepNumber + 1;

				return resultJoinPoint;
			}
			else
			{
				int stepNumber = 0;

				IList<IToken> tokensList_0 = persistenceService.FindTokensForProcessInstance(p.Id, synchInst.Synchronizer.Id);
				Dictionary<String, IToken> tokensMap = new Dictionary<String, IToken>();
				for (int i = 0; i < tokensList_0.Count; i++)
				{
					IToken tmpToken = (IToken)tokensList_0[i];
					String tmpFromActivityId = tmpToken.FromActivityId;
					if (!tokensMap.ContainsKey(tmpFromActivityId))
					{
						tokensMap.Add(tmpFromActivityId, tmpToken);
					}
					else
					{
						//TODO  ====下面的代码有意义吗？===start===wmj2003
						IToken tmpToken2 = (IToken)tokensMap[tmpFromActivityId];
						if (tmpToken2.StepNumber > tmpToken.StepNumber)
						{
							tokensMap[tmpFromActivityId] = tmpToken2;
						}
						//TODO  ====下面的代码有意义吗？===end===wmj2003
					}
				}

				List<IToken> tokensList = new List<IToken>(tokensMap.Values);

				for (int i = 0; i < tokensList.Count; i++)
				{
					IToken _token = (IToken)tokensList[i];
					resultJoinPoint.addValue(_token.Value);
					if (_token.IsAlive)//如果token的状态是alive
					{
						resultJoinPoint.Alive=true;
						String oldFromActivityId = resultJoinPoint.FromActivityId;
						if (String.IsNullOrEmpty(oldFromActivityId))
						{
							resultJoinPoint.FromActivityId=_token.FromActivityId;
						}
						else
						{
							resultJoinPoint.FromActivityId=oldFromActivityId + TokenFrom.FROM_ACTIVITY_ID_SEPARATOR + _token.FromActivityId;
						}
					}
					if (token.StepNumber > stepNumber)
					{
						stepNumber = token.StepNumber;
					}
				}

				resultJoinPoint.StepNumber=stepNumber + 1;

				return resultJoinPoint;
			}
		}

		public static void run(IProcessInstance p)
		{
			if (p.State != ProcessInstanceEnum.INITIALIZED)
			{
				throw new EngineException(p.Id,
				                          getWorkflowProcess(p),
				                          p.ProcessId, "The state of the process instance is " + p.State + ",can not run it ");
			}

			INetInstance netInstance = (INetInstance)RuntimeContextFactory.getRuntimeContext().KernelManager.getNetInstance(p.ProcessId, p.Version);
			if (netInstance == null)
			{
				throw new EngineException(p.Id,
				                          getWorkflowProcess(p),
				                          p.ProcessId, "The net instance for the  workflow process [Id=" + p.ProcessId + "] is Not found");
			}
			//触发事件
			ProcessInstanceEvent pevent = new ProcessInstanceEvent();
			pevent.EventType = ProcessInstanceEventEnum.BEFORE_PROCESS_INSTANCE_RUN;
			pevent.Source=p;
			fireProcessInstanceEvent(p,pevent);

			p.State=ProcessInstanceEnum.RUNNING;
			p.StartedTime=RuntimeContextFactory.getRuntimeContext().CalendarService.getSysDate();
			RuntimeContextFactory.getRuntimeContext().PersistenceService.SaveOrUpdateProcessInstance(p);
			netInstance.run(p);
		}


		public static Object getProcessInstanceVariable(IProcessInstance p,String name)
		{
			if (getProcessInstanceVariables(p) == null)
			{
				//通过数据库查询进行初始化
				IPersistenceService persistenceService = RuntimeContextFactory.getRuntimeContext().PersistenceService;
				IList<IProcessInstanceVar> allVars = persistenceService.FindProcessInstanceVariable(p.Id);
				setProcessInstanceVariables(p,new Dictionary<String, Object>());
				if (allVars != null && allVars.Count != 0)
				{
					foreach (ProcessInstanceVar theVar in allVars)
					{
						getProcessInstanceVariables(p).Add(theVar.VarPrimaryKey.Name, theVar.Value);
					}
				}
			}
			if (getProcessInstanceVariables(p).ContainsKey(name))
				return getProcessInstanceVariables(p)[name];
			else return null;

		}

		public static void setProcessInstanceVariable(IProcessInstance p,String name, Object value)
		{
			IPersistenceService persistenceService = RuntimeContextFactory.getRuntimeContext().PersistenceService;
			if (getProcessInstanceVariables(p) == null)
			{
				//通过数据库查询进行初始化
				IList<IProcessInstanceVar> allVars = persistenceService.FindProcessInstanceVariable(p.Id);
				setProcessInstanceVariables(p,new Dictionary<String, Object>());
				if (allVars != null && allVars.Count != 0)
				{
					foreach (ProcessInstanceVar theVar in allVars)
					{
						getProcessInstanceVariables(p).Add(theVar.VarPrimaryKey.Name, theVar.Value);
					}
				}
			}
			ProcessInstanceVar procInstVar = new ProcessInstanceVar();
			ProcessInstanceVarPk pk = new ProcessInstanceVarPk();
			pk.ProcessInstanceId=p.Id;
			pk.Name=name;
			procInstVar.VarPrimaryKey=pk;
			procInstVar.Value = value.ToString();
			procInstVar.ValueType=value.GetType().Name;
			
			if (getProcessInstanceVariables(p).ContainsKey(name))
			{
				persistenceService.UpdateProcessInstanceVariable(procInstVar);
				getProcessInstanceVariables(p)[name] = value;
			}
			else
			{
				persistenceService.SaveProcessInstanceVariable(procInstVar);
				getProcessInstanceVariables(p).Add(name, value);
			}
		}
		
		/// <summary>
		/// <para>正常结束工作流</para>
		/// <para>1、首先检查有无活动的token,如果有则直接返回，如果没有则结束当前流程</para>
		/// <para>2、执行结束流程的操作，将state的值设置为结束状态</para>
		/// 3、然后检查parentTaskInstanceId是否为null，如果不为null则，调用父taskinstance的complete操作。
		/// </summary>
		public static void complete(IProcessInstance p)
		{
			IList<IToken> tokens = RuntimeContextFactory.getRuntimeContext().PersistenceService.FindTokensForProcessInstance(p.Id, null);
			Boolean canBeCompleted = true;
			for (int i = 0; tokens != null && i < tokens.Count; i++)
			{
				IToken token = tokens[i];
				if (token.IsAlive)
				{
					canBeCompleted = false;
					break;
				}
			}
			if (!canBeCompleted)
			{
				return;
			}

			p.State=ProcessInstanceEnum.COMPLETED;
			//记录结束时间
			p.EndTime=RuntimeContextFactory.getRuntimeContext().CalendarService.getSysDate();
			RuntimeContextFactory.getRuntimeContext().PersistenceService.SaveOrUpdateProcessInstance(p);

			//删除所有的token
			for (int i = 0; tokens != null && i < tokens.Count; i++)
			{
				IToken token = tokens[i];
				RuntimeContextFactory.getRuntimeContext().PersistenceService.DeleteToken(token);
			}

			//触发事件
			ProcessInstanceEvent pevent = new ProcessInstanceEvent();
			pevent.EventType=ProcessInstanceEventEnum.AFTER_PROCESS_INSTANCE_COMPLETE;
			pevent.Source=p;
			fireProcessInstanceEvent(p,pevent);

			if ( !String.IsNullOrEmpty(p.ParentTaskInstanceId.Trim()))
			{
				ITaskInstance taskInstance = RuntimeContextFactory.getRuntimeContext().PersistenceService.FindAliveTaskInstanceById(p.ParentTaskInstanceId);
				//((IRuntimeContextAware)taskInstance).RuntimeContext=p.RuntimeContext;
				((IWorkflowSessionAware)taskInstance).CurrentWorkflowSession = p.CurrentWorkflowSession;
				//((TaskInstance)taskInstance).complete(null);
				TaskInstanceHelper.complete(taskInstance,null);
			}
		}

		public static void abort(IProcessInstance p)
		{
			if (p.State == ProcessInstanceEnum.COMPLETED || p.State == ProcessInstanceEnum.CANCELED)
			{
				throw new EngineException(p, getWorkflowProcess(p), "The process instance can not be aborted,the state of this process instance is " + p.State);
			}
			IPersistenceService persistenceService = RuntimeContextFactory.getRuntimeContext().PersistenceService;
			persistenceService.AbortProcessInstance(p);
		}

		/// <summary>触发process instance相关的事件</summary>
		/// <param name="e"></param>
		protected static void fireProcessInstanceEvent(IProcessInstance p,ProcessInstanceEvent e)
		{
			WorkflowProcess workflowProcess = getWorkflowProcess(p);
			if (workflowProcess == null)
			{
				return;
			}

			List<EventListener> listeners = workflowProcess.EventListeners;
			for (int i = 0; i < listeners.Count; i++)
			{
				EventListener listener = (EventListener)listeners[i];
				Object obj = RuntimeContextFactory.getRuntimeContext().getBeanByName(listener.ClassName);
				if (obj != null)
				{
					((IProcessInstanceEventListener)obj).onProcessInstanceEventFired(e);
				}
			}
		}

		public static void suspend(IProcessInstance p)
		{
			if (p.State == ProcessInstanceEnum.COMPLETED || p.State == ProcessInstanceEnum.CANCELED)
			{
				throw new EngineException(p, getWorkflowProcess(p), "The process instance can not be suspended,the state of this process instance is " + p.State);
			}
			if (p.Suspended != null && (bool)p.Suspended)
			{
				return;
			}
			IPersistenceService persistenceService = RuntimeContextFactory.getRuntimeContext().PersistenceService;
			persistenceService.SuspendProcessInstance(p);
		}

		public static void restore(IProcessInstance p)
		{
			if (p.State == ProcessInstanceEnum.COMPLETED || p.State == ProcessInstanceEnum.CANCELED)
			{
				throw new EngineException(p, getWorkflowProcess(p), "The process instance can not be restored,the state of this process instance is " + p.State);
			}
			if (!(p.Suspended != null && (bool)p.Suspended))
			{
				return;
			}
			IPersistenceService persistenceService = RuntimeContextFactory.getRuntimeContext().PersistenceService;
			persistenceService.RestoreProcessInstance(p);
		}
		
		private static Dictionary<String,WorkflowProcess> _workflowProcess = new Dictionary<string, WorkflowProcess>();
		public static WorkflowProcess getWorkflowProcess(IProcessInstance p)
		{
			if(!_workflowProcess.ContainsKey(p.Id)){
				IWorkflowDefinition workflowDef = RuntimeContextFactory.getRuntimeContext().DefinitionService.GetWorkflowDefinitionByProcessIdAndVersionNumber(p.ProcessId, p.Version);
				WorkflowProcess workflowProcess  = WorkflowDefinitionHelper.getWorkflowProcess(workflowDef);
				if(workflowProcess!=null)
					_workflowProcess[p.Id] = workflowProcess;
				else
					_workflowProcess[p.Id] = new WorkflowProcess("");
			}
			return _workflowProcess[p.Id];
		}
		
		public static String getWorkflowProcessId(IProcessInstance p)
		{
			return getWorkflowProcess(p).Sn;
		}
		
		
		private static Dictionary<String,Dictionary<String, Object>> _processInstanceVariables = new Dictionary<string, Dictionary<string, object>>();
		public static Dictionary<String, Object> getProcessInstanceVariables(IProcessInstance p)
		{
			IPersistenceService persistenceService = RuntimeContextFactory.getRuntimeContext().PersistenceService;
			if (!_processInstanceVariables.ContainsKey(p.Id))
			{
				//通过数据库查询进行初始化
				IList<IProcessInstanceVar> allVars = persistenceService.FindProcessInstanceVariable(p.Id);
				Dictionary<String, Object> pvs = new Dictionary<String, Object>();
				if (allVars != null && allVars.Count != 0)
				{
					foreach (ProcessInstanceVar theVar in allVars)
					{
						pvs.Add(theVar.VarPrimaryKey.Name, theVar.Value);
					}
				}
				_processInstanceVariables[p.Id] = pvs;
			}
			return _processInstanceVariables[p.Id];
			
		}
		
		public static void setProcessInstanceVariables(IProcessInstance p,Dictionary<String, Object> dic)
		{
			_processInstanceVariables[p.Id] = dic;
		}

	}
}
