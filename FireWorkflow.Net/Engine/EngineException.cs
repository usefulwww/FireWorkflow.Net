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
using System.Linq;
using System.Text;
using FireWorkflow.Net.Model;
using FireWorkflow.Net.Kernel;

namespace FireWorkflow.Net.Engine
{
    /// <summary>wangmj 引擎exception定义</summary>
    public class EngineException : KernelException
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="processInstance">processInstance 发生异常的流程实例</param>
        /// <param name="workflowElement">workflowElement 发生异常的流程环节或者Task</param>
        /// <param name="errMsg">错误信息</param>
        public EngineException(IProcessInstance processInstance, IWFElement workflowElement, String errMsg)
            :base(processInstance, workflowElement, errMsg)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="processInstanceId">发生异常的流程实例Id</param>
        /// <param name="process">发生异常的流程</param>
        /// <param name="workflowElementId">发生异常的环节或者Task的Id</param>
        /// <param name="errMsg">错误信息</param>
        public EngineException(String processInstanceId, WorkflowProcess process, String workflowElementId, String errMsg)
            : base(null, null, errMsg)
        {
            this.ProcessInstanceId=processInstanceId;
            if (process != null)
            {
                this.ProcessId=process.Id;
                this.ProcessName=process.Name;
                this.ProcessDisplayName=process.DisplayName;

                IWFElement workflowElement = process.findWFElementById(workflowElementId);
                if (workflowElement != null)
                {
                    this.WorkflowElementId=workflowElement.Id;
                    this.WorkflowElementName=workflowElement.Name;
                    this.WorkflowElementDisplayName=workflowElement.DisplayName;
                }
            }
        }
    }
}
