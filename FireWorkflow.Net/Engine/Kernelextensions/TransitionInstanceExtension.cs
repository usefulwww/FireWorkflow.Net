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
using FireWorkflow.Net.Engine;
using FireWorkflow.Net.Engine.Impl;
using FireWorkflow.Net.Engine.Condition;
using FireWorkflow.Net.Kernel;
using FireWorkflow.Net.Kernel.Event;
using FireWorkflow.Net.Kernel.Impl;
using FireWorkflow.Net.Kernel.Plugin;
using FireWorkflow.Net.Model;
using FireWorkflow.Net.Model.Net;

namespace FireWorkflow.Net.Engine.Kernelextensions
{
    public class TransitionInstanceExtension : IKernelExtension,
        IEdgeInstanceEventListener, IRuntimeContextAware
    {
        public RuntimeContext RuntimeContext { get; set; }

        /// <summary>
        /// <para>执行分支判断策略，即设置token的alive属性 首先，如果this.alive==false,则所有的token的Alive属性皆为false</para>
        /// <para>然后，如果在nexttransitionList中有值，则直接执行列表中的tansition</para>
        /// 否则，通过计算Transition的表达式来确定下一个transition,
        /// </summary>
        /// <param name="vars"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        private Boolean determineTheAliveOfToken(Dictionary<String, Object> vars, String condition)
        {
            // TODO通过计算transition上的表达式来确定alive的值
            IConditionResolver elResolver = this.RuntimeContext.ConditionResolver;
            Boolean b = elResolver.resolveBooleanExpression(vars, condition);
            return b;
        }

        /// <summary>计算value值</summary>
        public void calculateTheAliveValue(IToken token, String condition)
        {
            if (!token.IsAlive)
            {
                return;//如果token是dead状态，表明synchronizer的joinpoint是dead状态，不需要重新计算。
            }
            //1、如果没有转移条件，默认为true
            if (condition == null || condition.Trim().Equals(""))
            {
                token.IsAlive=true;
                return;
            }
            //2、default类型的不需要计算其alive值，该值由synchronizer决定
            if (condition.Trim().Equals(ConditionConstant.DEFAULT))
            {
                return;
            }

            //3、计算EL表达式
            try
            {
            	Boolean alive = determineTheAliveOfToken(ProcessInstanceHelper.getProcessInstanceVariables(token.ProcessInstance), condition);
                token.IsAlive=alive;
            }
            catch (Exception ex)
            {
            	throw new EngineException(token.ProcessInstanceId, ProcessInstanceHelper.getWorkflowProcess(token.ProcessInstance), token.NodeId, ex.Message);
            }

        }

        /// <summary>获取扩展目标名称</summary>
        public String ExtentionTargetName { get { return TransitionInstance.Extension_Target_Name; } }

        /// <summary>获取扩展点名称</summary>
        public String ExtentionPointName { get { return TransitionInstance.Extension_Point_TransitionInstanceEventListener; } }

        /// <summary>节点实例监听器</summary>
        public void onEdgeInstanceEventFired(EdgeInstanceEvent e)
        {
            if (e.EventType == EdgeInstanceEventEnum.ON_TAKING_THE_TOKEN)
            {
                IToken token = e.Token;
                ITransitionInstance transInst = (ITransitionInstance)e.getSource();
                String condition = transInst.Transition.Condition;
                calculateTheAliveValue(token, condition);

                if (this.RuntimeContext.IsEnableTrace && token.IsAlive)
                {
                    Transition transition = transInst.Transition;
                    IWFElement fromNode = transition.FromNode;
                    int minorNumber = 1;
                    if (fromNode is Activity)
                    {
                        minorNumber = 2;
                    }
                    else
                    {
                        minorNumber = 1;
                    }

                    ProcessInstanceTrace trace = new ProcessInstanceTrace();
                    trace.ProcessInstanceId=e.Token.ProcessInstanceId;
                    trace.StepNumber=e.Token.StepNumber;
                    trace.Type = ProcessInstanceTraceEnum.TRANSITION_TYPE;
                    trace.FromNodeId=transInst.Transition.FromNode.Id;
                    trace.ToNodeId=transInst.Transition.ToNode.Id;
                    trace.EdgeId=transInst.Transition.Id;
                    trace.MinorNumber=minorNumber;
                    //TODO wmj2003 这里应该是insert。一旦token从当前边上经过，那么就保存流程运行轨迹.
                    RuntimeContext.PersistenceService.SaveOrUpdateProcessInstanceTrace(trace);
                }
            }

        }
    }
}
