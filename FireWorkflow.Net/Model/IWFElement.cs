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
using FireWorkflow.Net.Base;

namespace FireWorkflow.Net.Model
{
    /// <summary>
    /// <para>工作流元素的抽象接口，工作流元素主要包括:</para>
    /// <para>1)业务流程 WorkflowProcess，这是顶层元素</para>
    /// <para>2)任务(Task)</para>
    /// <para>3)开始节点(StartNode)、结束节点(EndNode)、同步器(Synchronizer)、环节(Activity)</para>
    /// <para>4)转移(Transition)和循环(Loop)</para>
    /// <para>5)流程数据项(DataField)</para>
    /// </summary>
    public interface IWFElement
    {
        /// <summary>获取或设置元素序列号，请不要在业务代码里面使用该属性的信息。因为这个属性的值是变化的。</summary>
        String Sn { get; set; }

        /// <summary>
        /// <para>获取工作流元素的Id</para>
        /// 工作流元素的Id采用“父Id.自身Name”的方式组织。
        /// </summary>
        String Id { get; }

        /// <summary>获取或设置父元素</summary>
        IWFElement ParentElement { get; set; }

        /// <summary>获取或设置名称，不为空</summary>
        String Name { get; set; }

        /// <summary>获取或设置显示名称</summary>
        String DisplayName { get; set; }

        /// <summary>获取或设置描述</summary>
        String Description { get; set; }

        /// <summary>获取或设置事件监听器</summary>
        List<EventListener> EventListeners { get; set; }

        /// <summary>获取或设置扩展属性</summary>
        Dictionary<String, String> ExtendedAttributes { get; set; }
    }
}
