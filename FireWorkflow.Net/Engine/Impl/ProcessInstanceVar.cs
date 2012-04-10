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
//using System.Linq;
using System.Text;

namespace FireWorkflow.Net.Engine.Impl
{
	/// <summary>
	/// 
	/// </summary>
	public class ProcessInstanceVar : IProcessInstanceVar
	{
		/// <summary>
		/// Gets or sets the var primary key.
		/// </summary>
		/// <value>The var primary key.</value>
		//public ProcessInstanceVarPk VarPrimaryKey { get; set; }
		
		public String Id { get; set; }
		
		/// <summary>
		/// 获取或设置变量类型
		/// </summary>
		public String ValueType { get; set; }

		/// <summary>
		/// 获取或设置String形式变量值
		/// </summary>
		public String StringValue { get; set; }

		public String Name { get ;set; }
		
		public String ProcessInstanceId { get;set; }

	}
}
