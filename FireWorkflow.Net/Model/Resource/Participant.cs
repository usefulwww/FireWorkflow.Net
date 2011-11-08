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

namespace FireWorkflow.Net.Model.Resource
{
    //201004 add lwz 参与者通过业务接口实现默认获取用户
    public enum AssignmentTypeEnum
    {
        /// <summary>通过AssignmentHandler实现</summary>
        Handler,
        /// <summary>通过默认获取当前创建者</summary>
        Current,
        /// <summary>通过默认获取角色用户</summary>
        Role,
        /// <summary>通过默认获取机构用户</summary>
        Agency,
        /// <summary>获取PerformerValue来启动固定用户任务</summary>
        Fixed,
        /// <summary>通过默认获取创建者的上级领导用户</summary>
        Superiors,
    }
    /// <summary>参与者。</summary>
    public class Participant : AbstractResource
    {
        /// <summary>
        /// 任务分配句柄的类名。<br/>
        /// Fire workflow引擎调用该句柄获得真正的操作者ID。
        /// </summary>
        public String AssignmentHandler { get; set; }

        /// <summary>
        /// 默认设定值，如角色名，机构名，用户名,多值逗号分割
        /// </summary>
        public String PerformerValue { get; set; } //201004 add lwz 参与者通过业务接口实现默认获取用户

        /// <summary>
        /// 实现类型
        /// </summary>
        public AssignmentTypeEnum AssignmentType { get; set; } //201004 add lwz 参与者通过业务接口实现默认获取用户

        public Participant(String name)
        {
            this.Name = name;
        }
    }
}
