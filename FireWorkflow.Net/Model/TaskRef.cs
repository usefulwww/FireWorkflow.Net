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

namespace FireWorkflow.Net.Model
{
    /// <summary>任务引用。用于Activity引用全局的Task。</summary>
    public class TaskRef : AbstractWFElement
    {
        #region 属性
        /// <summary>被引用的Task</summary>
        Task referencedTask = null;
        /// <summary>被引用的Task</summary>
        public Task ReferencedTask { get { return referencedTask; } }

        /// <summary>
        /// <para>TaskRef的name等于被引用的Task的name</para>
        /// AbstractWFElement#Name
        /// </summary>
        public override String Name { get { return referencedTask.Name; } set { } }

        /// <summary>TaskRef的description等于被引用的Task的description</summary>
        public override String Description { get { return referencedTask.Description; } set { } }

        /// <summary>TaskRef的显示名等于被引用的Task的显示名</summary>
        public override String DisplayName { get { return referencedTask.DisplayName; } set { } }
        #endregion

        #region 构造函数
        public TaskRef(IWFElement parent, Task task)
            : base(parent, task.Name)
        {
            referencedTask = task;
        }

        public TaskRef(Task task)
        {
            referencedTask = task;
        }
        #endregion

        public override String ToString()
        {
            return referencedTask.ToString();
        }
    }
}
