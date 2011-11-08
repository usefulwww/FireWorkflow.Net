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

namespace FireWorkflow.Net.Model.Net
{
    /// <summary>环节。</summary>
    public class Activity : Node
    {
        #region 属性
        /// <summary>获取或设置环节的输入转移Transition。一个环节有且只有一个输入Transition</summary>
        public Transition EnteringTransition { get; set; }//输入弧

        /// <summary>获取或设置输出转移Transition。一个环节有且只有一个输出Transition</summary>
        public Transition LeavingTransition { get; set; }//输出弧

        /// <summary>对全局Task的引用的列表</summary>
        public List<TaskRef> TaskRefs { get; set; }

        /// <summary>局部的task列表</summary>
        public List<Task> InlineTasks { get; set; }

        /// <summary>
        /// <para>返回环节的结束策略，取值为ALL或者ANY，缺省值为ALL</para>
        /// <para>如果取值为ALL,则只有其所有任务实例结束了，环节实例才可以结束。</para>
        /// <para>如果取值为ANY，则只要任何一个任务实例结束后，环节实例就可以结束。</para>
        /// 环节实例的结束操作仅执行一遍，因此后续任务实例的结束不会触发环节实例的结束操作再次执行。
        /// </summary>
        public FormTaskEnum CompletionStrategy { get; set; }
        #endregion
        
        #region 构造函数
        public Activity()
        {
            this.TaskRefs = new List<TaskRef>();
            this.InlineTasks = new List<Task>();
            this.CompletionStrategy = FormTaskEnum.ALL;
        }
        public Activity(WorkflowProcess workflowProcess, String name)
            : base(workflowProcess, name)
        {
            this.TaskRefs = new List<TaskRef>();
            this.InlineTasks = new List<Task>();
            this.CompletionStrategy = FormTaskEnum.ALL;
        }
        #endregion

        /// <summary>
        /// 返回该环节所有的Task。
        /// 这些Task是inlineTask列表和taskRef列表解析后的所有的Task的和。
        /// </summary>
        public List<Task> getTasks()
        {
            List<Task> tasks = new List<Task>();
            tasks.AddRange(this.InlineTasks);
            for (int i = 0; i < this.TaskRefs.Count; i++)
            {
                TaskRef taskRef = TaskRefs[i];
                tasks.Add(taskRef.ReferencedTask);
            }
            return tasks;
        }

        

        
    }
}
