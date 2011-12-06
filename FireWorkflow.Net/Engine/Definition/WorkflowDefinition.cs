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
using System.IO;
using System.Text;
using FireWorkflow.Net.Engine;
using FireWorkflow.Net.Model;
using FireWorkflow.Net.Model.Io;

namespace FireWorkflow.Net.Engine.Definition
{
	/// <summary>
	/// 流程定义对象
	/// 映射到表T_FF_DF_WORKFLOWDEF
	/// </summary>
	public class WorkflowDefinition : IWorkflowDefinition
	{
		public WorkflowDefinition()
		{
			DefinitionType = FPDL_PROCESS;
		}
		
		public const String FPDL_PROCESS = "FPDL";
		public const String XPDL_PROCESS = "XPDL";//从未用到
		public const String BPEL_PROCESS = "BPEL";//从未用到

		#region 属性
		/// <summary>获取或设置主键</summary>
		public String Id { get; set; }
		/// <summary>获取或设置流程id</summary>
		public String ProcessId { get; set; }
		/// <summary>获取或设置流程英文名称</summary>
		public String Name { get; set; }
		/// <summary>获取或设置流程显示名称</summary>
		public String DisplayName { get; set; }
		/// <summary>获取或设置流程业务说明</summary>
		public String Description { get; set; }
		/// <summary>获取或设置版本号</summary>
		public Int32 Version { get; set; }
		/// <summary>获取或设置是否发布，1=已经发布,0未发布</summary>
		public Boolean State { get; set; }
		/// <summary>获取或设置上载到数据库的操作员</summary>
		public String UploadUser { get; set; }
		/// <summary>获取或设置上载到数据库的时间</summary>
		public DateTime UploadTime { get; set; }
		/// <summary>获取或设置发布人</summary>
		public String PublishUser { get; set; }
		/// <summary>获取或设置发布时间</summary>
		public DateTime PublishTime { get; set; }
		/// <summary>获取或设置定义文件的语言类型，fpdl,xpdl,bepl...</summary>
		public String DefinitionType { get; set; }
		/// <summary>获取或设置流程定义文件的内容。</summary>
		public String ProcessContent { get; set; }//
		#endregion

	}
}
