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
using FireWorkflow.Net.Kernel;
using FireWorkflow.Net.Kernel.Event;
using FireWorkflow.Net.Kernel.Plugin;

namespace FireWorkflow.Net.Kernel.Impl
{
    public abstract class AbstractNodeInstance : INodeInstance, IPlugable
    {
        public List<ITransitionInstance> LeavingTransitionInstances { get; set; }

        public List<ITransitionInstance> EnteringTransitionInstances { get; set; }

        public List<INodeInstanceEventListener> EventListeners { get; set; }

        public List<ILoopInstance> LeavingLoopInstances { get; set; }

        public List<ILoopInstance> EnteringLoopInstances { get; set; }

        public AbstractNodeInstance()
        {
            this.LeavingTransitionInstances = new List<ITransitionInstance>();
            this.EnteringTransitionInstances = new List<ITransitionInstance>();
            this.EventListeners = new List<INodeInstanceEventListener>();
            this.LeavingLoopInstances = new List<ILoopInstance>();
            this.EnteringLoopInstances = new List<ILoopInstance>();
        }


        public virtual void AddLeavingTransitionInstance(ITransitionInstance transitionInstance)
        {
            LeavingTransitionInstances.Add(transitionInstance);
        }

        public virtual void AddEnteringTransitionInstance(ITransitionInstance transitionInstance)
        {
            this.EnteringTransitionInstances.Add(transitionInstance);
        }

        public virtual void AddLeavingLoopInstance(ILoopInstance loopInstance)
        {
            LeavingLoopInstances.Add(loopInstance);
        }

        public virtual void AddEnteringLoopInstance(ILoopInstance loopInstance)
        {
            this.EnteringLoopInstances.Add(loopInstance);
        }

        /// <summary>
        /// 20090914 增加统一的触发方法，实现类中根据事件的不同而进行触发
        /// </summary>
        /// <param name="neevent"></param>
        public virtual void fireNodeEvent(NodeInstanceEvent neevent)
        {
            for (int i = 0; i < this.EventListeners.Count; i++)
            {
                INodeInstanceEventListener listener = this.EventListeners[i];
                listener.onNodeInstanceEventFired(neevent);
            }
        }

        public virtual string Id
        {
            get { throw new NotImplementedException(); }
        }

        public virtual void fire(IToken token)
        {
            throw new NotImplementedException();
        }

        public virtual string ExtensionTargetName
        {
            get { throw new NotImplementedException(); }
        }

        public virtual List<string> ExtensionPointNames
        {
            get { throw new NotImplementedException(); }
        }

        public virtual void registExtension(IKernelExtension extension)
        {
            throw new NotImplementedException();
        }
    }
}
