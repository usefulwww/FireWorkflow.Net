
using System;
using System.Collections.Generic;
using System.Text;
using FireWorkflow.Net.Engine.Taskinstance;

namespace FireWorkflow.Net.Engine.Impl
{

	public class WorkItemHelper
	{
		public WorkItemHelper()
		{
		}
		
		
        public static IWorkItem withdraw(IWorkItem w)
        {
            if (w.CurrentWorkflowSession == null)
            {
                new EngineException(w.TaskInstance.ProcessInstanceId,
                        TaskInstanceHelper.getWorkflowProcess(w.TaskInstance), w.TaskInstance.TaskId,
                        "The current workflow session is null.");
            }
//            if (w.RuntimeContext == null)
//            {
//                new EngineException(w.TaskInstance.ProcessInstanceId,
//                        TaskInstanceHelper.getWorkflowProcess(w.TaskInstance), w.TaskInstance.TaskId,
//                        "The current runtime context is null.");
//            }
            ITaskInstanceManager taskInstanceMgr = RuntimeContextFactory.getRuntimeContext().TaskInstanceManager;
            return taskInstanceMgr.withdrawWorkItem(w);
        }


        public static void reject(IWorkItem w)
        {
            reject(w,w.Comments);
        }

        public static void reject(IWorkItem w,String comments)
        {
            if (w.CurrentWorkflowSession == null)
            {
                new EngineException(w.TaskInstance.ProcessInstanceId,
                        TaskInstanceHelper.getWorkflowProcess(w.TaskInstance), w.TaskInstance.TaskId,
                        "The current workflow session is null.");
            }
//            if (w.RuntimeContext == null)
//            {
//                new EngineException(w.TaskInstance.ProcessInstanceId,
//                        TaskInstanceHelper.getWorkflowProcess(w.TaskInstance), w.TaskInstance.TaskId,
//                        "The current runtime context is null.");
//            }
            ITaskInstanceManager taskInstanceMgr = RuntimeContextFactory.getRuntimeContext().TaskInstanceManager;
            taskInstanceMgr.rejectWorkItem(w, comments);
        }

        /// <summary>
        /// <para>结束当前WorkItem；并由工作流引擎根据流程定义决定下一步操作。引擎的执行规则如下</para>
        /// <para>1、工作流引擎首先判断该WorkItem对应的TaskInstance是否可以结束。</para>
        /// <para>   如果TaskInstance的assignment策略为ANY，或者，assignment策略为ALL且它所有的WorkItem都已经完成</para>
        /// <para>   则结束当前TaskInstance</para>
        /// <para>2、判断TaskInstance对应的ActivityInstance是否可以结束。如果ActivityInstance的complete strategy为ANY，</para>
        /// <para>   或者，complete strategy为ALL且他的所有的TaskInstance都已经结束，则结束当前ActivityInstance</para>
        /// <para>3、根据流程定义，启动下一个Activity，并创建相关的TaskInstance和WorkItem</para>
        /// </summary>
        public static void complete(IWorkItem w)
        {
            complete(w,null, w.Comments);
        }
        /// <summary>
        /// <para>结束当前WorkItem；并由工作流引擎根据流程定义决定下一步操作。引擎的执行规则如下</para>
        /// <para>1、工作流引擎首先判断该WorkItem对应的TaskInstance是否可以结束。</para>
        /// <para>   如果TaskInstance的assignment策略为ANY，或者，assignment策略为ALL且它所有的WorkItem都已经完成</para>
        /// <para>   则结束当前TaskInstance</para>
        /// <para>2、判断TaskInstance对应的ActivityInstance是否可以结束。如果ActivityInstance的complete strategy为ANY，</para>
        /// <para>   或者，complete strategy为ALL且他的所有的TaskInstance都已经结束，则结束当前ActivityInstance</para>
        /// <para>3、根据流程定义，启动下一个Activity，并创建相关的TaskInstance和WorkItem</para>
        /// </summary>
        /// <param name="comments">备注信息</param>
        public static void complete(IWorkItem w,String comments)
        {
            complete(w,null, comments);
        }


        /// <summary>
        /// <para>结束当前WorkItem；并由工作流引擎根据流程定义决定下一步操作。引擎的执行规则如下</para>
        /// <para>1、工作流引擎首先判断该WorkItem对应的TaskInstance是否可以结束。</para>
        /// <para>   如果TaskInstance的assignment策略为ANY，或者，assignment策略为ALL且它所有的WorkItem都已经完成</para>
        /// <para>   则结束当前TaskInstance</para>
        /// <para>2、判断TaskInstance对应的ActivityInstance是否可以结束。如果ActivityInstance的complete strategy为ANY，</para>
        /// <para>   或者，complete strategy为ALL且他的所有的TaskInstance都已经结束，则结束当前ActivityInstance</para>
        /// <para>3、根据流程定义，启动下一个Activity，并创建相关的TaskInstance和WorkItem</para>
        /// </summary>
        /// <param name="dynamicAssignmentHandler">通过动态分配句柄指定下一个环节的操作者。</param>
        /// <param name="comments">备注信息</param>
        public static void complete(IWorkItem w,DynamicAssignmentHandler dynamicAssignmentHandler, String comments)
        {
            if (w.CurrentWorkflowSession == null)
            {
                new EngineException(w.TaskInstance.ProcessInstanceId,
                        TaskInstanceHelper.getWorkflowProcess(w.TaskInstance), w.TaskInstance.TaskId,
                        "The current workflow session is null.");
            }
//            if (w.RuntimeContext == null)
//            {
//                new EngineException(w.TaskInstance.ProcessInstanceId,
//                        TaskInstanceHelper.getWorkflowProcess(w.TaskInstance), w.TaskInstance.TaskId,
//                        "The current runtime context is null.");
//            }

            if (w.State != WorkItemEnum.RUNNING)
            {
                ITaskInstance thisTaskInst = w.TaskInstance;
                //			System.out.println("WorkItem的当前状态为"+w.State+"，不可以执行complete操作。");
                throw new EngineException(thisTaskInst.ProcessInstanceId, TaskInstanceHelper.getWorkflowProcess(thisTaskInst), thisTaskInst.TaskId,
                        "Complete work item failed . The state of the work item [id=" + w.Id + "] is " + w.State);
            }

            if (dynamicAssignmentHandler != null)
            {
                w.CurrentWorkflowSession.setDynamicAssignmentHandler(dynamicAssignmentHandler);
            }
            ITaskInstanceManager taskInstanceManager = RuntimeContextFactory.getRuntimeContext().TaskInstanceManager;
            taskInstanceManager.completeWorkItem(w, null, comments);
        }

        public static IWorkItem reassignTo(IWorkItem w,String actorId)
        {
            return reassignTo( w,actorId, w.Comments);
        }

        public static IWorkItem reassignTo(IWorkItem w,String actorId, String comments)
        {
            if (w.CurrentWorkflowSession == null)
            {
                new EngineException(w.TaskInstance.ProcessInstanceId,
                        TaskInstanceHelper.getWorkflowProcess(w.TaskInstance), w.TaskInstance.TaskId,
                        "The current workflow session is null.");
            }
//            if (w.RuntimeContext == null)
//            {
//                new EngineException(w.TaskInstance.ProcessInstanceId,
//                        TaskInstanceHelper.getWorkflowProcess(w.TaskInstance), w.TaskInstance.TaskId,
//                        "The current runtime context is null.");
//            }

            ITaskInstanceManager manager = RuntimeContextFactory.getRuntimeContext().TaskInstanceManager;
            return manager.reasignWorkItemTo(w, actorId, comments);
        }

        /// <summary>签收</summary>
        /// <returns></returns>
        public static IWorkItem claim(IWorkItem w)
        {
            if (w.CurrentWorkflowSession == null)
            {
                new EngineException(w.TaskInstance.ProcessInstanceId,
                        TaskInstanceHelper.getWorkflowProcess(w.TaskInstance), w.TaskInstance.TaskId,
                        "The current workflow session is null.");
            }
//            if (w.RuntimeContext == null)
//            {
//                new EngineException(w.TaskInstance.ProcessInstanceId,
//                        TaskInstanceHelper.getWorkflowProcess(w.TaskInstance), w.TaskInstance.TaskId,
//                        "The current runtime context is null.");
//            }

            ITaskInstanceManager taskInstanceMgr = RuntimeContextFactory.getRuntimeContext().TaskInstanceManager;
            IWorkItem newWorkItem = taskInstanceMgr.claimWorkItem(w.Id, w.TaskInstance.Id);

            if (newWorkItem != null)
            {
                w.State = newWorkItem.State;
                w.ClaimedTime = newWorkItem.ClaimedTime;

                //((IRuntimeContextAware)newWorkItem).RuntimeContext = w.RuntimeContext;
                ((IWorkflowSessionAware)newWorkItem).CurrentWorkflowSession = w.CurrentWorkflowSession;
            }
            else
            {
                w.State = WorkItemEnum.CANCELED;
            }

            return newWorkItem;
        }

        public static void jumpTo(IWorkItem w,String activityId)
        {
            jumpTo(w,activityId, null, w.Comments);
        }

        public static void jumpTo(IWorkItem w,String activityId, String comments)
        {
            jumpTo(w,activityId, null, comments);
        }

        public static void jumpTo(IWorkItem w,String targetActivityId, DynamicAssignmentHandler dynamicAssignmentHandler, String comments)
        {
            if (w.CurrentWorkflowSession == null)
            {
                new EngineException(w.TaskInstance.ProcessInstanceId,
                        TaskInstanceHelper.getWorkflowProcess(w.TaskInstance), w.TaskInstance.TaskId,
                        "The current workflow session is null.");
            }
//            if (w.RuntimeContext == null)
//            {
//                new EngineException(w.TaskInstance.ProcessInstanceId,
//                        TaskInstanceHelper.getWorkflowProcess(w.TaskInstance), w.TaskInstance.TaskId,
//                        "The current runtime context is null.");
//            }
            if (dynamicAssignmentHandler != null)
            {
                w.CurrentWorkflowSession.setDynamicAssignmentHandler(dynamicAssignmentHandler);
            }
            ITaskInstanceManager taskInstanceManager = RuntimeContextFactory.getRuntimeContext().TaskInstanceManager;
            taskInstanceManager.completeWorkItemAndJumpTo(w, targetActivityId, comments);
        }

        public static void jumpToEx(IWorkItem w,String targetActivityId, DynamicAssignmentHandler dynamicAssignmentHandler, String comments)
        {
            if (w.CurrentWorkflowSession == null)
            {
                new EngineException(w.TaskInstance.ProcessInstanceId,
                        TaskInstanceHelper.getWorkflowProcess(w.TaskInstance), w.TaskInstance.TaskId,
                        "The current workflow session is null.");
            }
//            if (w.RuntimeContext == null)
//            {
//                new EngineException(w.TaskInstance.ProcessInstanceId,
//                        TaskInstanceHelper.getWorkflowProcess(w.TaskInstance), w.TaskInstance.TaskId,
//                        "The current runtime context is null.");
//            }
            if (dynamicAssignmentHandler != null)
            {
                w.CurrentWorkflowSession.setDynamicAssignmentHandler(dynamicAssignmentHandler);
            }
            ITaskInstanceManager taskInstanceManager = RuntimeContextFactory.getRuntimeContext().TaskInstanceManager;
            taskInstanceManager.completeWorkItemAndJumpToEx(w, targetActivityId, comments);
        }

	}
}
