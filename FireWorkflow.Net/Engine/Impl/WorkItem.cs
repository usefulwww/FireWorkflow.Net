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
using FireWorkflow.Net.Engine.Taskinstance;
using FireWorkflow.Net.Kernel;
using FireWorkflow.Net.Model;
using FireWorkflow.Net.Model.Net;

namespace FireWorkflow.Net.Engine.Impl
{

	[Serializable]
	public class WorkItem : IWorkItem
	{
		public String ActorId { get; set; }
		public String Id { get; set; }
		public WorkItemEnum State { get; set; }
		public DateTime CreatedTime { get; set; }

		/// <summary>签收时间</summary>
		public DateTime ClaimedTime { get; set; }
		/// <summary>结束时间</summary>
		public DateTime EndTime { get; set; }
		public String Comments { get; set; }
		public ITaskInstance TaskInstance { get; set; }

		public string Name { get { return TaskInstance.Name; } }//lwz 2010-3-3 add
		public string DisplayName { get { return TaskInstance.DisplayName; } }//lwz 2010-3-3 add
		public String ProcessInstanceId { get { return TaskInstance.ProcessInstanceId; } }//lwz 2010-3-3 add
		public String BizInfo { get { return TaskInstance.BizInfo; } }//lwz 2010-3-3 add
		/// <summary>返回对应的流程的Id</summary>
		public String ProcessId { get { return TaskInstance.ProcessId; } }

		/// <summary>返回流程的版本</summary>
		public Int32 Version { get { return TaskInstance.Version; } }

		/// <summary>added by wangmj 20090922 供springjdbc实现类使用</summary>
		public String TaskInstanceId { get; set; }

		public WorkItem()
		{
		}

		public WorkItem(ITaskInstance taskInstance)
		{
			this.TaskInstance = taskInstance;
		}

		public WorkItem(WorkItemEnum state, DateTime createdTime, DateTime signedTm,
		                DateTime endTime, String comments, ITaskInstance taskInstance)
		{
			this.State = state;
			this.CreatedTime = createdTime;
			this.ClaimedTime = signedTm;
			this.EndTime = endTime;
			this.Comments = comments;
			this.TaskInstance = taskInstance;
		}


	}
}
