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

namespace FireWorkflow.Net.Model.Net
{
    /// <summary>同步器</summary>
    public class Synchronizer : Node
    {
        #region 属性
        /// <summary>获取或设置输入转移的列表</summary>
        public virtual List<Transition> EnteringTransitions { get; set; }

        /// <summary>获取或设置输出转移的列表</summary>
        public virtual List<Transition> LeavingTransitions { get; set; }

        /// <summary>获取或设置输入循环的列表</summary>
        public  List<Loop> EnteringLoops { get; set; }

        /// <summary>获取或设置输出循环的列表</summary>
        public  List<Loop> LeavingLoops { get; set; }
        #endregion

        #region 构造函数
        public Synchronizer()
        {
            this.EnteringTransitions = new List<Transition>();
            this.LeavingTransitions = new List<Transition>();
            this.EnteringLoops = new List<Loop>();
            this.LeavingLoops = new List<Loop>();
        }

        public Synchronizer(WorkflowProcess workflowProcess, String name)
            : base(workflowProcess, name)
        {
            this.EnteringTransitions = new List<Transition>();
            this.LeavingTransitions = new List<Transition>();
            this.EnteringLoops = new List<Loop>();
            this.LeavingLoops = new List<Loop>();
            // TODO Auto-generated constructor stub
        }
        #endregion

    }
}
