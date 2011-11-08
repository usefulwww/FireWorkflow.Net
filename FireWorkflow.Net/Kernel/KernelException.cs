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
using FireWorkflow.Net.Engine;

namespace FireWorkflow.Net.Kernel
{
    public class KernelException : Exception
    {

        private const long serialVersionUID = -7219349319943347690L;

        /// <summary>抛出异常的流程实例的Id</summary>
        public String ProcessInstanceId { get; set; }

        /// <summary>抛出异常的流程定义的Id</summary>
        public String ProcessId { get; set; }

        /// <summary>抛出异常的流程的名称</summary>
        public String ProcessName { get; set; }

        /// <summary>抛出异常的流程的显示名称</summary>
        public String ProcessDisplayName { get; set; }

        /// <summary>抛出异常的流程元素的Id</summary>
        public String WorkflowElementId { get; set; }

        /// <summary>抛出异常的流程元素的名称</summary>
        public String WorkflowElementName { get; set; }

        /// <summary>抛出异常的流程元素的显示名称</summary>
        public String WorkflowElementDisplayName { get; set; }

        public KernelException(IProcessInstance processInstance, IWFElement workflowElement, String errMsg)
            : base(errMsg)
        {
            if (processInstance != null)
            {
                this.ProcessInstanceId = processInstance.Id;
                this.ProcessId=processInstance.ProcessId;
                this.ProcessName=processInstance.Name;
                this.ProcessDisplayName=processInstance.DisplayName;
            }
            if (workflowElement != null)
            {
                this.WorkflowElementId=workflowElement.Id;
                this.WorkflowElementName=workflowElement.Name;
                this.WorkflowElementDisplayName=workflowElement.DisplayName;
            }
            // TODO Auto-generated constructor stub
        }
    }
}
