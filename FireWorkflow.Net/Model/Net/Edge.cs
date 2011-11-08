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

namespace FireWorkflow.Net.Model.Net
{
    /// <summary>工作流网的边。</summary>
    public class Edge : AbstractWFElement
    {
        #region 属性
        /// <summary>
        /// 获取或设置转移(或者循环)的源节点。
        /// 转移的源节点可以是StartNode、 Activity或者Synchronizer。
        /// 循环的源节点必须是Synchronizer或者EndNode，同时循环的目标节点必须是循环源节点的前驱。
        /// </summary>
        public Node FromNode { get; set; }

        /// <summary>
        /// 获取或设置转移(或者循环)的目标节点。
        /// 转移的终止目标可以是EndNode、 Activity或者Synchronizer。
        /// 循环的目标节点必须是Synchronizer或者StartNode。
        /// </summary>
        public Node ToNode { get; set; }

        /// <summary>返回转移(或者循环)的启动条件，转移（循环）启动条件是一个EL表达式</summary>
        public String Condition { get; set; }
        #endregion

        #region 构造函数
        public Edge()
        {

        }

        public Edge(WorkflowProcess workflowProcess, String name)
            : base(workflowProcess, name)
        {
        }
        #endregion

    }
}
