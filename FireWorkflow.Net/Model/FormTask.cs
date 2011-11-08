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
using System.Xml;
using System.Xml.Serialization;
using System.Text;
using FireWorkflow.Net.Model.Resource;

namespace FireWorkflow.Net.Model
{
    /// <summary>表单类型的Task，即人工任务。</summary>
    public class FormTask : Task
    {
        #region 属性
        /// <summary>获取或设置任务的操作员。只有FORM类型的任务才有操作员。</summary>
        public Participant Performer { get; set; }//引用participant

        /// <summary>任获取或设置任务的分配策略，只对FORM类型的任务有意义。取值为FormTask.ANY,FormTask.ALL。</summary>
        public FormTaskEnum AssignmentStrategy { get; set; }

        /// <summary>
        /// <para>获取或设置任务的缺省表单的类型，取值为EDITFORM、VIEWFORM或者LISTFORM。</para>
        /// 只有FORM类型的任务此方法才有意义。该方法的主要作用是方便系统开发，引擎不会用到该方法。
        /// </summary>
        public DefaultViewEnum DefaultView { get; set; }//缺省视图是view form

        /// <summary>可编辑表单</summary>
        public Form EditForm { get; set; }

        /// <summary>只读表单</summary>
        public Form ViewForm { get; set; }

        /// <summary>列表表单</summary>
        public Form ListForm { get; set; }

        //    protected String startMode = MANUAL ;//启动模式，启动模式没有意义，application和subflow自动启动，Form一般情况下签收时启动，如果需要自动启动则在assignable接口中实现。
        #endregion

        #region 构造函数
        public FormTask()
        {
            this.TaskType = TaskTypeEnum.FORM;
            this.AssignmentStrategy = FormTaskEnum.ANY;
            this.DefaultView = DefaultViewEnum.VIEWFORM;
        }

        public FormTask(IWFElement parent, String name)
            : base(parent, name)
        {
            this.TaskType = TaskTypeEnum.FORM;
            this.AssignmentStrategy = FormTaskEnum.ANY;
            this.DefaultView = DefaultViewEnum.VIEWFORM;
        }
        #endregion
    }

    #region 枚举
    /// <summary>
    /// 表单的类型枚举
    /// </summary>
    public enum DefaultViewEnum
    {
        /// <summary>可编辑表单</summary>
        EDITFORM,
        /// <summary>只读表单</summary>
        VIEWFORM,
        /// <summary>列表表单</summary>
        LISTFORM
    }
    /// <summary>
    /// 任务的分配策略枚举
    /// </summary>
    public enum FormTaskEnum
    {
        /// <summary>
        /// 任务分配策略之一：ALL。任务分配给角色中的所有人，只有在所有工单结束结束的情况下，任务实例才结束。
        /// 用于实现会签。
        /// </summary>
        ALL,
        /// <summary>任务分配策略之二：ANY。任何一个操作角签收该任务的工单后，其他人的工单被取消掉。</summary>
        ANY
    }
    #endregion
}
