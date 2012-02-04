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
using FireWorkflow.Net.Model.Net;
using FireWorkflow.Net.Kernel;
using FireWorkflow.Net.Kernel.Plugin;
using FireWorkflow.Net.Kernel.Event;

namespace FireWorkflow.Net.Kernel.Impl
{
    public class LoopInstance : EdgeInstance, ILoopInstance, IPlugable
    {
        public const String Extension_Target_Name = "FireWorkflow.Net.Kernel.LoopInstance";
        public static List<String> Extension_Point_Names = new List<String>();
        public const String Extension_Point_LoopInstanceEventListener = "LoopInstanceEventListener";

        static LoopInstance()
        {
            Extension_Point_Names.Add(Extension_Point_LoopInstanceEventListener);
            //        Extension_Point_Names.add(Extension_Point_ConditionEvaluator);
        }

        //private 

        private Loop loop = null;

        public LoopInstance(Loop lp)
        {
            this.loop = lp;
        }

        public override String Id { get { return this.loop.Id; } }

        public override int Weight
        {
            get
            {
                if (weight == 0)
                {
                    if (LeavingNodeInstance is SynchronizerInstance)
                    {
                        weight = ((SynchronizerInstance)this.LeavingNodeInstance).Volume;
                    }
                    else if (LeavingNodeInstance is StartNodeInstance)
                    {
                        weight = ((StartNodeInstance)this.LeavingNodeInstance).Volume;
                    }
                    else if (LeavingNodeInstance is EndNodeInstance)
                    {
                        weight = ((EndNodeInstance)this.LeavingNodeInstance).Volume;
                    }
                }
                return weight;
            }
        }

        public override Boolean take(IToken token)
        {
            Boolean oldAlive = token.IsAlive;

            EdgeInstanceEvent e = new EdgeInstanceEvent(this);
            e.Token=token;
            e.EventType = EdgeInstanceEventEnum.ON_TAKING_THE_TOKEN;

            for (int i = 0; this.eventListeners != null && i < this.eventListeners.Count; i++)
            {
                IEdgeInstanceEventListener listener = this.eventListeners[i];
                listener.onEdgeInstanceEventFired(e);
            }


            Boolean newAlive = token.IsAlive;

            if (!newAlive)
            {//循环条件不满足，则恢复token的alive标示
                token.IsAlive = oldAlive;
                return newAlive;
            }
            else
            {//否则流转到下一个节点
                INodeInstance nodeInst = this.LeavingNodeInstance;
                token.Value = this.Weight;
                nodeInst.fire(token);//触发同步器节点
                return newAlive;
            }
        }

        public Loop Loop { get { return loop; } }

        public void setLoop(Loop arg0)
        {
            this.loop = arg0;
        }

        public String ExtensionTargetName { get { return LoopInstance.Extension_Target_Name; } }

        public List<String> ExtensionPointNames { get { return LoopInstance.Extension_Point_Names; } }

        public void registExtension(IKernelExtension extension)
        {
            if (!Extension_Target_Name.Equals(extension.ExtentionTargetName))
            {
                return;
            }
            if (Extension_Point_LoopInstanceEventListener.Equals(extension.ExtentionPointName))
            {
                if (extension is IEdgeInstanceEventListener)
                {
                    this.eventListeners.Add((IEdgeInstanceEventListener)extension);
                }
                else
                {
                    throw new Exception("Error:When construct the TransitionInstance,the extension MUST be a instance of ITransitionInstanceEventListener");
                }
            }
        }

    }

}
