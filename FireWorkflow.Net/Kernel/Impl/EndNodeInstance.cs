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
using System.Text;
using FireWorkflow.Net.Model;
using FireWorkflow.Net.Model.Net;
using FireWorkflow.Net.Engine;
using FireWorkflow.Net.Engine.Impl;
using FireWorkflow.Net.Kernel;
using FireWorkflow.Net.Kernel.Plugin;
using FireWorkflow.Net.Kernel.Event;

namespace FireWorkflow.Net.Kernel.Impl
{
    public class EndNodeInstance : AbstractNodeInstance, ISynchronizerInstance
    {
        public const String Extension_Target_Name = "FireWorkflow.Net.Kernel.EndNodeInstance";
        public static List<String> Extension_Point_Names = new List<String>();
        public const String Extension_Point_NodeInstanceEventListener = "NodeInstanceEventListener";

        /// <summary>
        /// volume是同步器的容量
        /// </summary>
        public int Volume { get; set; }

        static EndNodeInstance()
        {
            Extension_Point_Names.Add(Extension_Point_NodeInstanceEventListener);
        }
        private EndNode endNode = null;

        public EndNodeInstance()
        {
        }

        public override String Id
        {
            get { return this.endNode.Id; }
        }

        public EndNodeInstance(EndNode endNd)
        {
            this.endNode = endNd;
            this.Volume = this.endNode.EnteringTransitions.Count;
        }

        public int Value { get; set; }

        public IJoinPoint synchronized(IToken tk)
        {
            IJoinPoint joinPoint = null;
            tk.NodeId=this.Synchronizer.Id;
            //log.debug("The weight of the Entering TransitionInstance is " + tk.getValue());
            // 触发TokenEntered事件
            NodeInstanceEvent event1 = new NodeInstanceEvent(this);
            event1.Token=tk;
            event1.EventType=NodeInstanceEventEnum.NODEINSTANCE_TOKEN_ENTERED;
            this.fireNodeEvent(event1);

            //汇聚检查
            joinPoint = ProcessInstanceHelper.createJoinPoint(tk.ProcessInstance,this, tk);// JoinPoint由谁生成比较好？
            int value = (int)joinPoint.Value;

            //log.debug("The volume of " + this.toString() + " is " + volume);
            //log.debug("The value of " + this.toString() + " is " + value);
            if (value > this.Volume)
            {
                KernelException exception = new KernelException(tk.ProcessInstance,
                        this.Synchronizer,
                        "Error:The token count of the synchronizer-instance can NOT be  greater than  it's volumn  ");
                throw exception;
            }
            if (value < this.Volume)
            {// 如果Value小于容量则继续等待其他弧的汇聚。
                return null;
            }
            return joinPoint;
        }

        public override void fire(IToken tk)
        {
            IJoinPoint joinPoint = synchronized(tk);
            if (joinPoint == null) return;
            IProcessInstance processInstance = tk.ProcessInstance;
            NodeInstanceEvent event2 = new NodeInstanceEvent(this);
            event2.Token=tk;
            event2.EventType=NodeInstanceEventEnum.NODEINSTANCE_FIRED;
            this.fireNodeEvent(event2);

            //在此事件监听器中，删除原有的token
            NodeInstanceEvent event4 = new NodeInstanceEvent(this);
            event4.Token=tk;
            event4.EventType=NodeInstanceEventEnum.NODEINSTANCE_LEAVING;
            this.fireNodeEvent(event4);

            //首先必须检查是否有满足条件的循环
            Boolean doLoop = false;//表示是否有满足条件的循环，false表示没有，true表示有。
            if (joinPoint.Alive)
            {
                IToken tokenForLoop = null;

                tokenForLoop = new Token(); // 产生新的token
                tokenForLoop.IsAlive=joinPoint.Alive;
                tokenForLoop.ProcessInstance=processInstance;
                tokenForLoop.StepNumber=joinPoint.StepNumber - 1;
                tokenForLoop.FromActivityId=joinPoint.FromActivityId;

                for (int i = 0; i < this.LeavingLoopInstances.Count; i++)
                {
                    ILoopInstance loopInstance = this.LeavingLoopInstances[i];
                    doLoop = loopInstance.take(tokenForLoop);
                    if (doLoop)
                    {
                        break;
                    }
                }
            }

            if (!doLoop)
            {
                NodeInstanceEvent event3 = new NodeInstanceEvent(this);
                event3.Token=tk;
                event3.EventType=NodeInstanceEventEnum.NODEINSTANCE_COMPLETED;
                this.fireNodeEvent(event3);
            }

            //        NodeInstanceEvent event4 = new NodeInstanceEvent(this);
            //        event4.setToken(tk);
            //        event4.setEventType(NodeInstanceEvent.NODEINSTANCE_LEAVING);
            //        this.fireNodeEvent(event4);
        }

        public override String ExtensionTargetName { get { return Extension_Target_Name; } }

        public override List<String> ExtensionPointNames { get { return Extension_Point_Names; } }

        /*
         * (non-Javadoc)
         *
         * @see org.fireflow.kenel.plugin.IPlugable#registExtension(org.fireflow.kenel.plugin.IKenelExtension)
         */
        public override void registExtension(IKernelExtension extension)
        {
            if (!Extension_Target_Name.Equals(extension.ExtentionTargetName))
            {
                throw new Exception("Error:When construct the EndNodeInstance,the Extension_Target_Name is mismatching");
            }
            if (Extension_Point_NodeInstanceEventListener.Equals(extension.ExtentionPointName))
            {
                if (extension is INodeInstanceEventListener)
                {
                    this.EventListeners.Add((INodeInstanceEventListener)extension);
                }
                else
                {
                    throw new Exception("Error:When construct the EndNodeInstance,the extension MUST be a instance of INodeInstanceEventListener");
                }
            }
        }

        public override String ToString()
        {
            return "EndNodeInstance_4_[" + endNode.Id + "]";
        }


        public Synchronizer Synchronizer { get { return this.endNode; } }
    }

}
