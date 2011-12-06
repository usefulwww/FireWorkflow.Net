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
using FireWorkflow.Net.Model;
using FireWorkflow.Net.Model.Io;

namespace FireWorkflow.Net.Engine.Definition
{
	/// <summary>
	/// Description of WorkflowDefinitionHelper.
	/// </summary>
	public class WorkflowDefinitionHelper
	{
		public WorkflowDefinitionHelper()
		{
		}
		private static Dictionary<string,WorkflowProcess> dic_wp = new Dictionary<string,WorkflowProcess>();
		
		  /// <summary>获取业务流程对象</summary>
        public static WorkflowProcess getWorkflowProcess( IWorkflowDefinition wdf)// throws RuntimeException
        {
        	WorkflowProcess workflowProcess=null;
        	if (!dic_wp.ContainsKey(wdf.Id))
            {
                if (wdf.ProcessContent != null && !String.IsNullOrEmpty(wdf.ProcessContent.Trim()))
                {
                    Dom4JFPDLParser parser = new Dom4JFPDLParser();
                    workflowProcess = parser.parse(wdf.ProcessContent);
                    dic_wp[wdf.Id]= workflowProcess;
                }
            }
            workflowProcess.Sn = wdf.Id;
            return workflowProcess;
        }

        public static void setWorkflowProcess(IWorkflowDefinition wdf,WorkflowProcess workflowProcess)
        {
            wdf.ProcessId = workflowProcess.Id;
            wdf.Name = workflowProcess.Name;
            wdf.DisplayName = workflowProcess.DisplayName;
            wdf.Description = workflowProcess.Description;

            Dom4JFPDLSerializer ser = new Dom4JFPDLSerializer();

            wdf.ProcessContent = ser.serialize(workflowProcess);
        }
    
	}
}
