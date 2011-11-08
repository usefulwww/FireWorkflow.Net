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

namespace FireWorkflow.Net.Kernel.Impl
{
    /// <summary>实现edge的公共方法 ，边有两种，一是transition，二是loop。</summary>
    public abstract class EdgeInstance : IEdgeInstance
    {
        public INodeInstance LeavingNodeInstance { get; set; }
        public INodeInstance EnteringNodeInstance { get; set; }
        protected int weight = 0;
        protected List<IEdgeInstanceEventListener> eventListeners = new List<IEdgeInstanceEventListener>();

        public virtual string Id { get { return String.Empty; } }

        public virtual int Weight { get { return 0; } }

        public virtual bool take(IToken token)
        {
            return false;
        }
    }

}
