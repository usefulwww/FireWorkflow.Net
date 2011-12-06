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
using FireWorkflow.Net.Engine.Persistence;
using FireWorkflow.Net.Engine.Impl;
using FireWorkflow.Net.Kernel;
using FireWorkflow.Net.Kernel.Event;
using FireWorkflow.Net.Kernel.Impl;

namespace FireWorkflow.Net.Engine.Kernelextensions
{
    public class EndNodeInstanceExtension : SynchronizerInstanceExtension
    {
        /// <summary>获取扩展目标名称</summary>
        public override String ExtentionTargetName { get { return EndNodeInstance.Extension_Target_Name; } }

        /// <summary>获取扩展点名称</summary>
        public override String ExtentionPointName { get { return EndNodeInstance.Extension_Point_NodeInstanceEventListener; } }

        /// <summary>节点实例监听器</summary>
        public override void onNodeInstanceEventFired(NodeInstanceEvent e)
        {
            //同步器节点的监听器触发条件，是在离开这个节点的时候
            if (e.EventType == NodeInstanceEventEnum.NODEINSTANCE_LEAVING)
            {
                ISynchronizerInstance syncInst = (ISynchronizerInstance)e.getSource();
                IPersistenceService persistenceService = this.RuntimeContext.PersistenceService;
                //删除同步器节点的token
                persistenceService.DeleteTokensForNode(e.Token.ProcessInstanceId, syncInst.Synchronizer.Id);
            }
            //如果节点实例结束，就触发
            if (e.EventType == NodeInstanceEventEnum.NODEINSTANCE_COMPLETED)
            {
                // 执行ProcessInstance的complete操作
                IToken tk = e.Token;
                ProcessInstance currentProcessInstance = (ProcessInstance)tk.ProcessInstance;
                ProcessInstanceHelper.complete(currentProcessInstance);
            }
        }
    }
}
