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
using FireWorkflow.Net.Model;
using FireWorkflow.Net.Engine;

namespace FireWorkflow.Net.Kernel
{

    /// <summary>
    /// ProcessInstance负责和外部运行环境（RuntimeContext)沟通
    /// @author chennieyun
    /// </summary>
    public interface INetInstance
    {
        String Id { get; }

        Int32 Version { get; }

        //TODO 实参-形参如何体现？通过Context?
        /// <summary>启动工作流的实例</summary>
        /// <param name="processInstance"></param>
        void run(IProcessInstance processInstance);//throws KernelException;

        /// <summary>
        /// 结束流程实例，如果流程状态没有达到终态，则直接返回。
        /// @throws RuntimeException
        /// </summary>
        void complete();//throws KernelException;

        WorkflowProcess WorkflowProcess { get; }

        Object getWFElementInstance(String wfElementId);
    }
}
