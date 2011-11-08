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
    public class WorkflowDefinition : WorkflowDefinitionInfo
    {
        protected WorkflowProcess workflowProcess;

        /// <summary>获取或设置流程定义文件的内容。</summary>
        public String ProcessContent { get; set; }//

        /// <summary>获取业务流程对象</summary>
        public WorkflowProcess getWorkflowProcess()// throws RuntimeException
        {
            if (workflowProcess == null)
            {
                if (ProcessContent != null && !String.IsNullOrEmpty(this.ProcessContent.Trim()))
                {
                    MemoryStream msin = null;

                    try
                    {
                        Dom4JFPDLParser parser = new Dom4JFPDLParser();
                        msin = new MemoryStream(Encoding.UTF8.GetBytes(this.ProcessContent));
                        this.workflowProcess = parser.parse(msin);
                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        if (msin != null) msin.Close();
                    }

                }
            }
            workflowProcess.Sn = this.Id;
            return workflowProcess;
        }

        public void setWorkflowProcess(WorkflowProcess process)// throws  RuntimeException 
        {
            this.workflowProcess = process;

            this.ProcessId = workflowProcess.Id;
            this.Name = workflowProcess.Name;
            this.DisplayName = workflowProcess.DisplayName;
            this.Description = workflowProcess.Description;

            Dom4JFPDLSerializer ser = new Dom4JFPDLSerializer();
            MemoryStream so = new MemoryStream();
            try
            {
                ser.serialize(workflowProcess, so);
                this.ProcessContent = Encoding.UTF8.GetString(so.ToArray());
            }
            catch
            {
                throw;
            }
            finally
            {
                if (so != null) so.Close();
            }
        }
    }
}
