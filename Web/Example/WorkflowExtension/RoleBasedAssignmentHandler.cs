using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;

using FireWorkflow.Net.Engine;
using FireWorkflow.Net.Engine.Impl;
using FireWorkflow.Net.Engine.Taskinstance;
using FireWorkflow.Net.Kernel;

namespace WebDemo.Example.WorkflowExtension
{
	/// <summary>
	/// <para>任务分配处理程序，工作流系统将真正的任务分配工作交给该处理程序完成。</para>
	/// <para>所有的FORM类型的Task都需要设置其Performer属性，Performer属性实际上是一个Participant对象，</para>
	/// 由该对象提供IAssignmentHandler实现类。
	/// </summary>
	public class RoleBasedAssignmentHandler : IAssignmentHandler
	{
		#region IAssignmentHandler 成员

		/// <summary>
		/// <para>实现任务分配工作，该方法一般的实现逻辑是：</para>
		/// <para>首先根据performerName查询出所有的操作员，可以把performerName当作角色名称。</para>
		/// <para>然后调用asignable.asignToActor(String actorId,Boolean needSign)或者</para>
		/// <para>asignable.asignToActor(String actorId)或者asignable.asignToActorS(List actorIds)</para>
		/// 进行任务分配。
		/// </summary>
		/// <param name="asignable">IAssignable实现类，在FireWorkflow中实际上就是TaskInstance对象。</param>
		/// <param name="performerName">角色名称</param>
		public void assign(ITaskInstance taskInst, string performerName)
		{
			//ITaskInstance taskInst = (ITaskInstance)asignable;

			String roleName = performerName == null ? "" : performerName.Trim();
			List<String> users = new List<string>();
			Dictionary<string, List<string>> users_dic = new Dictionary<string, List<string>>();
			//仓管岗
			users_dic.Add("WarehouseKeeper", new List<string>() { "warehousekeeper1", "warehousekeeper2" });
			//送货岗
			users_dic.Add("Deliveryman", new List<string>() { "deliveryman1", "deliveryman2", "deliveryman3" });
			//收银岗
			users_dic.Add("Cashier", new List<string>() { "cashier1", "cashier2" });
			//风险核查岗
			users_dic.Add("RiskEvaluator", new List<string>() { "riskevaluator1", "riskevaluator2" });
			//审批岗
			users_dic.Add("Approver", new List<string>() { "approver1", "approver2", "approver3" });
			//放款操作岗
			users_dic.Add("LendMoneyOfficer", new List<string>() { "lendmoneyofficer1" });
			//信贷员
			users_dic.Add("Loanteller", new List<string>() { "loanteller1", "loanteller2" });
			if (users_dic.ContainsKey(roleName))
				users = users_dic[roleName];
			else{
				foreach (List<string> list in users_dic.Values) {
					if (list.Contains(roleName)){
						users = list;
						break;
					}
				}
			}
			
			if (users == null || users.Count <= 0)
			{
				throw new EngineException(taskInst.ProcessInstanceId,
				                          TaskInstanceHelper.getWorkflowProcess(taskInst), taskInst.TaskId, "没有任何用户和角色" + performerName + "相关联，无法分配任务实例[id=" + taskInst.Id + ",任务名称=" + taskInst.DisplayName);
			}
			List<String> userIds = new List<string>();
			foreach (string item in users)
			{
				userIds.Add(item);
			}
			
			TaskInstanceHelper.assignToActors(taskInst,userIds);
		}

		#endregion
	}
}
