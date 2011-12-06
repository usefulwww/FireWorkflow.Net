
using System;
using FireWorkflow.Net.Engine.Persistence;
using FireWorkflow.Net.Engine;
using System.Collections.Generic;
using FireWorkflow.Net.Kernel;

namespace FireWorkflow.Net.Persistence.NHibernateDAL
{

	public class PersistenceServiceDAL: IPersistenceService
	{
		public PersistenceServiceDAL()
		{
		}
		
		public RuntimeContext RuntimeContext {get;set;}
		
		public bool SaveOrUpdateProcessInstance(IProcessInstance processInstance)
		{
			throw new NotImplementedException();
		}
		
		public IProcessInstance FindAliveProcessInstanceById(string id)
		{
			throw new NotImplementedException();
		}
		
		public IProcessInstance FindProcessInstanceById(string id)
		{
			throw new NotImplementedException();
		}
		
		public IList<IProcessInstance> FindProcessInstancesByProcessId(string processId)
		{
			throw new NotImplementedException();
		}
		
		public IList<IProcessInstance> FindProcessInstancesByProcessIdAndVersion(string processId, int version)
		{
			throw new NotImplementedException();
		}
		
		public int GetAliveProcessInstanceCountForParentTaskInstance(string taskInstanceId)
		{
			throw new NotImplementedException();
		}
		
		public bool AbortProcessInstance(IProcessInstance processInstance)
		{
			throw new NotImplementedException();
		}
		
		public bool SuspendProcessInstance(IProcessInstance processInstance)
		{
			throw new NotImplementedException();
		}
		
		public bool RestoreProcessInstance(IProcessInstance processInstance)
		{
			throw new NotImplementedException();
		}
		
		public IList<IProcessInstanceVar> FindProcessInstanceVariable(string processInstanceId)
		{
			throw new NotImplementedException();
		}
		
		public IProcessInstanceVar FindProcessInstanceVariable(string processInstanceId, string name)
		{
			throw new NotImplementedException();
		}
		
		public bool UpdateProcessInstanceVariable(IProcessInstanceVar var)
		{
			throw new NotImplementedException();
		}
		
		public bool SaveProcessInstanceVariable(IProcessInstanceVar var)
		{
			throw new NotImplementedException();
		}
		
		public bool SaveOrUpdateTaskInstance(ITaskInstance taskInstance)
		{
			throw new NotImplementedException();
		}
		
		public bool AbortTaskInstance(ITaskInstance taskInstance)
		{
			throw new NotImplementedException();
		}
		
		public ITaskInstance FindAliveTaskInstanceById(string id)
		{
			throw new NotImplementedException();
		}
		
		public int GetAliveTaskInstanceCountForActivity(string processInstanceId, string activityId)
		{
			throw new NotImplementedException();
		}
		
		public int GetCompletedTaskInstanceCountForTask(string processInstanceId, string taskId)
		{
			throw new NotImplementedException();
		}
		
		public ITaskInstance FindTaskInstanceById(string id)
		{
			throw new NotImplementedException();
		}
		
		public IList<ITaskInstance> FindTaskInstancesForProcessInstance(string processInstanceId, string activityId)
		{
			throw new NotImplementedException();
		}
		
		public IList<ITaskInstance> FindTaskInstancesForProcessInstanceByStepNumber(string processInstanceId, int stepNumber)
		{
			throw new NotImplementedException();
		}
		
		public bool LockTaskInstance(string taskInstanceId)
		{
			throw new NotImplementedException();
		}
		
		public bool SaveOrUpdateWorkItem(IWorkItem workitem)
		{
			throw new NotImplementedException();
		}
		
		public int GetAliveWorkItemCountForTaskInstance(string taskInstanceId)
		{
			throw new NotImplementedException();
		}
		
		public IList<IWorkItem> FindCompletedWorkItemsForTaskInstance(string taskInstanceId)
		{
			throw new NotImplementedException();
		}
		
		public IList<IWorkItem> FindWorkItemsForTaskInstance(string taskInstanceId)
		{
			throw new NotImplementedException();
		}
		
		public bool DeleteWorkItemsInInitializedState(string taskInstanceId)
		{
			throw new NotImplementedException();
		}
		
		public IWorkItem FindWorkItemById(string id)
		{
			throw new NotImplementedException();
		}
		
		public IList<IWorkItem> FindWorkItemsForTask(string taskid)
		{
			throw new NotImplementedException();
		}
		
		public IList<IWorkItem> FindTodoWorkItems(string actorId)
		{
			throw new NotImplementedException();
		}
		
		public IList<IWorkItem> FindTodoWorkItems(string actorId, string processInstanceId)
		{
			throw new NotImplementedException();
		}
		
		public IList<IWorkItem> FindTodoWorkItems(string actorId, string processId, string taskId)
		{
			throw new NotImplementedException();
		}
		
		public IList<IWorkItem> FindHaveDoneWorkItems(string actorId)
		{
			throw new NotImplementedException();
		}
		
		public IList<IWorkItem> FindHaveDoneWorkItems(string actorId, string processInstanceId)
		{
			throw new NotImplementedException();
		}
		
		public IList<IWorkItem> FindHaveDoneWorkItems(string actorId, string processId, string taskId)
		{
			throw new NotImplementedException();
		}
		
		public bool SaveOrUpdateToken(IToken token)
		{
			throw new NotImplementedException();
		}
		
		public int GetAliveTokenCountForNode(string processInstanceId, string nodeId)
		{
			throw new NotImplementedException();
		}
		
		public IToken FindTokenById(string id)
		{
			throw new NotImplementedException();
		}
		
		public IList<IToken> FindTokensForProcessInstance(string processInstanceId, string nodeId)
		{
			throw new NotImplementedException();
		}
		
		public bool DeleteTokensForNode(string processInstanceId, string nodeId)
		{
			throw new NotImplementedException();
		}
		
		public bool DeleteTokensForNodes(string processInstanceId, IList<string> nodeIdsList)
		{
			throw new NotImplementedException();
		}
		
		public bool DeleteToken(IToken token)
		{
			throw new NotImplementedException();
		}
		
		public bool SaveOrUpdateWorkflowDefinition(IWorkflowDefinition workflowDef)
		{
			throw new NotImplementedException();
		}
		
		public IWorkflowDefinition FindWorkflowDefinitionById(string id)
		{
			throw new NotImplementedException();
		}
		
		public IWorkflowDefinition FindWorkflowDefinitionByProcessIdAndVersionNumber(string processId, int version)
		{
			throw new NotImplementedException();
		}
		
		public IWorkflowDefinition FindTheLatestVersionOfWorkflowDefinitionByProcessId(string processId)
		{
			throw new NotImplementedException();
		}
		
		public IList<IWorkflowDefinition> FindWorkflowDefinitionsByProcessId(string processId)
		{
			throw new NotImplementedException();
		}
		
		public IList<IWorkflowDefinition> FindAllTheLatestVersionsOfWorkflowDefinition()
		{
			throw new NotImplementedException();
		}
		
		public int FindTheLatestVersionNumber(string processId)
		{
			throw new NotImplementedException();
		}
		
		public int FindTheLatestVersionNumberIgnoreState(string processId)
		{
			throw new NotImplementedException();
		}
		
		public bool SaveOrUpdateProcessInstanceTrace(IProcessInstanceTrace processInstanceTrace)
		{
			throw new NotImplementedException();
		}
		
		public IList<IProcessInstanceTrace> FindProcessInstanceTraces(string processInstanceId)
		{
			throw new NotImplementedException();
		}
		
		public int GetTodoWorkItemsCount(string actorId, string publishUser)
		{
			throw new NotImplementedException();
		}
		
		public IList<IWorkItem> FindTodoWorkItems(string actorId, string publishUser, int pageSize, int pageNumber)
		{
			throw new NotImplementedException();
		}
		
		public int GetHaveDoneWorkItemsCount(string actorId, string publishUser)
		{
			throw new NotImplementedException();
		}
		
		public IList<IWorkItem> FindHaveDoneWorkItems(string actorId, string publishUser, int pageSize, int pageNumber)
		{
			throw new NotImplementedException();
		}
		
		public int GetProcessInstanceCountByCreatorId(string creatorId, string publishUser)
		{
			throw new NotImplementedException();
		}
		
		public IList<IProcessInstance> FindProcessInstanceListByCreatorId(string creatorId, string publishUser, int pageSize, int pageNumber)
		{
			throw new NotImplementedException();
		}
		
		public int GetProcessInstanceCountByPublishUser(string publishUser)
		{
			throw new NotImplementedException();
		}
		
		public IList<IProcessInstance> FindProcessInstanceListByPublishUser(string publishUser, int pageSize, int pageNumber)
		{
			throw new NotImplementedException();
		}
	}
}
