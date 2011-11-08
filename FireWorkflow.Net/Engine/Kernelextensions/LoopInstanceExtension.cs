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
using FireWorkflow.Net.Engine.Condition;
using FireWorkflow.Net.Engine.Impl;
using FireWorkflow.Net.Kernel;
using FireWorkflow.Net.Kernel.Event;
using FireWorkflow.Net.Kernel.Plugin;
using FireWorkflow.Net.Kernel.Impl;

namespace FireWorkflow.Net.Engine.Kernelextensions
{
    public class LoopInstanceExtension : IKernelExtension, IEdgeInstanceEventListener, IRuntimeContextAware
    {
        public RuntimeContext RuntimeContext { get; set; }

        /// <summary>获取扩展目标名称</summary>
        public String ExtentionTargetName { get { return LoopInstance.Extension_Target_Name; } }

        /// <summary>获取扩展点名称</summary>
        public String ExtentionPointName { get { return LoopInstance.Extension_Point_LoopInstanceEventListener; } }

        private Boolean determineTheAliveOfToken(Dictionary<String, Object> vars, String condition)
        {
            // TODO通过计算transition上的表达式来确定alive的值
            IConditionResolver elResolver = this.RuntimeContext.ConditionResolver;
            Boolean b = elResolver.resolveBooleanExpression(vars, condition);
            return b;
        }

        public void calculateTheAliveValue(IToken token, String condition)
        {
            if (!token.IsAlive)
            {
                return;// 如果token是dead状态，表明synchronizer的joinpoint是dead状态，不需要重新计算。
            }

            // 1、如果没有循环条件，默认为false
            if (condition == null || condition.Trim().Equals(""))
            {
                token.IsAlive=false;
                return;
            }
            // 3、计算EL表达式
            try
            {
                Boolean alive = determineTheAliveOfToken(token.ProcessInstance.ProcessInstanceVariables, condition);
                token.IsAlive = alive;
            }
            catch (Exception ex)
            {
                throw new EngineException(token.ProcessInstanceId, token.ProcessInstance.WorkflowProcess, token.NodeId, ex.Message);
            }
        }

        /// <summary>节点实例监听器</summary>
        public void onEdgeInstanceEventFired(EdgeInstanceEvent e)
        {
            if (e.EventType == EdgeInstanceEventEnum.ON_TAKING_THE_TOKEN)
            {
                IToken token = e.Token;
                // 计算token的alive值
                ILoopInstance transInst = (ILoopInstance)e.getSource();
                String condition = transInst.Loop.Condition;

                calculateTheAliveValue(token, condition);

                if (this.RuntimeContext.IsEnableTrace && token.IsAlive)
                {
                    ProcessInstanceTrace trace = new ProcessInstanceTrace();
                    trace.ProcessInstanceId=e.Token.ProcessInstanceId;
                    trace.StepNumber=e.Token.StepNumber + 1;
                    trace.Type=ProcessInstanceTraceEnum.LOOP_TYPE;
                    trace.FromNodeId=transInst.Loop.FromNode.Id;
                    trace.ToNodeId=transInst.Loop.ToNode.Id;
                    trace.EdgeId=transInst.Loop.Id;
                    //TODO wmj2003 一旦token从当前边上经过，那么就保存流程运行轨迹,这里应该是insert
                    RuntimeContext.PersistenceService.SaveOrUpdateProcessInstanceTrace(trace);
                }
            }
        }
    }
}
