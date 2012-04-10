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
using System.Runtime.Serialization;
using System.Text;

using FireWorkflow.Net.Engine.Impl;

namespace FireWorkflow.Net.Engine
{
	public interface IProcessInstanceTrace
	{
		string Id { get; set; }
		string ProcessInstanceId { get; set; }
		int StepNumber { get; set; }
		int MinorNumber { get; set; }
		ProcessInstanceTraceEnum Type { get; set; }
		[DataMemberAttribute()]
		string EdgeId { get; set; }
		[DataMemberAttribute()]
		string FromNodeId { get; set; }
		[DataMemberAttribute()]
		string ToNodeId { get; set; }
	}
}
