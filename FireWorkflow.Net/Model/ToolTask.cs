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
using FireWorkflow.Net.Model.Resource;

namespace FireWorkflow.Net.Model
{
    /// <summary>Tool类型的Task</summary>
    public class ToolTask : Task
    {
        #region 属性
        /// <summary>获取或设置任务自动执行的Application。只有TOOL类型的任务才有Application。</summary>
        public Application Application { get; set; }

        #endregion

        #region 构造函数
        public ToolTask()
        {
            this.TaskType = TaskTypeEnum.TOOL;
        }

        public ToolTask(IWFElement parent, String name)
            : base(parent, name)
        {
            this.TaskType = TaskTypeEnum.TOOL;
        }
        #endregion
    }
}
