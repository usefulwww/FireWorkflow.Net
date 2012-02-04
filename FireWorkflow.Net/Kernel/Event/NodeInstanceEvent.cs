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
using FireWorkflow.Net.Base;

namespace FireWorkflow.Net.Kernel.Event
{
    public enum NodeInstanceEventEnum
    {
        NULL = -1,
        NODEINSTANCE_TOKEN_ENTERED = 1,
        NODEINSTANCE_FIRED = 2,
        NODEINSTANCE_COMPLETED = 3,
        NODEINSTANCE_LEAVING = 4
    }

    /// <summary>node监听器</summary>
    public class NodeInstanceEvent : EventObject
    {
        private NodeInstanceEvent() : base(null)
        {
            EventType = NodeInstanceEventEnum.NULL;
        }
        public NodeInstanceEvent(Object source) : base(source)
        {
            EventType = NodeInstanceEventEnum.NULL;
        }

        public IToken Token { get; set; }

        public NodeInstanceEventEnum EventType { get; set; }

    }
}
