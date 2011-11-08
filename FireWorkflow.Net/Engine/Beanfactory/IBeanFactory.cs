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

namespace FireWorkflow.Net.Engine.Beanfactory
{
    /// <summary>
    /// <para>Engine把创建bean实例的工作委派给该服务。</para>
    /// <para>Engine在如下情况下需要获得相关Bean的实例(未全部枚举)。</para>
    /// <para>1)Tool类型的Task,Engine通过该服务获得ApplicationHandler的实例然后调用其方法IApplicationHandler.execute(ITaskInstance taskInstace)</para>
    /// <para>2)Engine在触发事件时，需要获得相关Listener的实例</para>
    /// <para>3)在分配工作项的时候需要获得IAssignmentHandler的实例。</para>
    /// </summary>
    public interface IBeanFactory
    {
        /// <summary>
        /// 根据bean的名字返回bean的实例
        /// </summary>
        /// <param name="beanName">bean name具体含义是什么由IBeanFactory的实现类来决定</param>
        /// <returns></returns>
        Object GetBean(String beanName);

        
        /// <summary>
        /// 根据bean的名字返回bean的实例
        /// </summary>
        /// <param name="beanName">bean name具体含义是什么由IBeanFactory的实现类来决定</param>
        /// <param name="args">与要调用构造函数的参数数量、顺序和类型匹配的参数数组。如果 args 为空数组或 nullNothingnullptrnull 引用（在 Visual Basic 中为 Nothing），则调用不带任何参数的构造函数（默认构造函数）。</param>
        /// <returns></returns>
        Object GetBean(string beanName, params Object[] args);
    }
}
