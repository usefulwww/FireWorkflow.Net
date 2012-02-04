// Copyright 2003-2008 非也
// All rights reserved. 
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation。
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program. If not, see http://www.gnu.org/licenses. *
// @author 非也,nychen2000@163.com
// @Revision to .NET 无忧 lwz0721@gmail.com 2010-02
using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using FireWorkflow.Net.Engine;

namespace FireWorkflow.Net.Engine.Definition
{
    /// <summary>
    /// 从关系数据库表T_FF_DF_WORKFLOWDEF中读取流程定义文件，该表保存了同一个流程的各个版本。
    /// 该类用于系统的实施阶段。
    /// </summary>
    public class DefinitionService4DBMS : IDefinitionService
    {
        /// <summary>
        /// 工作流总线
        /// </summary>
        /// <value></value>
        public RuntimeContext RuntimeContext { get; set; }

        #region 实现IDefinitionService
        /// <summary>返回所有流程的最新版本</summary>
        /// <returns></returns>
        public IList<IWorkflowDefinition> GetAllLatestVersionsOfWorkflowDefinition()
        {
            return RuntimeContext.PersistenceService.FindAllTheLatestVersionsOfWorkflowDefinition();
        }


        /// <summary>
        /// 根据流程Id和版本号查找流程定义
        /// </summary>
        /// <param name="processId">流程Id</param>
        /// <param name="version">版本号</param>
        /// <returns></returns>
        public IWorkflowDefinition GetWorkflowDefinitionByProcessIdAndVersionNumber(String processId, Int32 version)
        {
            return RuntimeContext.PersistenceService.FindWorkflowDefinitionByProcessIdAndVersionNumber(processId, version);
        }

        /// <summary>
        /// 通过流程Id查找其最新版本的流程定义
        /// </summary>
        /// <param name="processId">流程Id</param>
        /// <returns></returns>
        public IWorkflowDefinition GetTheLatestVersionOfWorkflowDefinition(String processId)
        {
            return RuntimeContext.PersistenceService.FindTheLatestVersionOfWorkflowDefinitionByProcessId(processId);
        }

        #endregion
    }
}
