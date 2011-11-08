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

namespace FireWorkflow.Net.Kernel.Impl
{
    /// <summary>
    /// token的生命周期开始与一个synchronizer（包括startnode 和 endnode)，结束于另一个synchronizer
    /// </summary>
    public class Token : IToken
    {
        //20090908  transient
        private Dictionary<String, IProcessInstance> contextInfo = new Dictionary<String, IProcessInstance>();

        /// <summary>
        /// 通过alive标志来判断nodeinstance是否要fire
        /// </summary>
        public Boolean IsAlive { get; set; }

        public IProcessInstance ProcessInstance
        {
            get { return (IProcessInstance)this.contextInfo[EngineConstant.CURRENT_PROCESS_INSTANCE]; }
            set
            {
                this.contextInfo.Add(EngineConstant.CURRENT_PROCESS_INSTANCE, value);
                if (value != null)
                {
                    this.ProcessInstanceId = value.Id;
                }
                else
                {
                    this.ProcessInstanceId = null;
                }
            }
        }

        public String ProcessInstanceId { get; set; }
        public String NodeId { get; set; }
        public Int32 Value { get; set; }

        public String Id { get; set; }
        public Int32 StepNumber { get; set; }

        /// <summary>
        /// 获得前驱Activity的Id,如果有多个，则用"&"分割
        /// </summary>
        public String FromActivityId { get; set; }

        /// <summary>
        /// @date 20090908 
        /// 返回Engine的当前上下文信息，如WorkflowSession,等。
        /// 这些信息不保存到数据库 
        /// </summary>
        public Dictionary<string, IProcessInstance> ContextInfo { get { return this.contextInfo; } }
    }

}
