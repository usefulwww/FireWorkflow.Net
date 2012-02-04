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
using FireWorkflow.Net.Engine;
using FireWorkflow.Net.Engine.Impl;
using FireWorkflow.Net.Engine.Condition;
using FireWorkflow.Net.Kernel;
using FireWorkflow.Net.Kernel.Event;
using FireWorkflow.Net.Kernel.Impl;
using FireWorkflow.Net.Kernel.Plugin;
using FireWorkflow.Net.Model;
using FireWorkflow.Net.Model.Net;
using FireWorkflow.Net.Base;

namespace FireWorkflow.Net.Engine.Condition
{
    /// <summary>
    /// 实现条件表达式的解析。
    /// </summary>
    public class ConditionResolver : IConditionResolver, IRuntimeContextAware
    {
        public RuntimeContext RuntimeContext { get; set; }

        /// <summary>
        /// 解析条件表达式。条件表达是必须是一个值为Boolean类型的EL表达式
        /// </summary>
        /// <param name="vars">变量列表</param>
        /// <param name="elExpression">条件表达式</param>
        /// <returns>返回条件表达式的计算结果</returns>
        public Boolean resolveBooleanExpression(Dictionary<String, Object> vars, String elExpression)//throws Exception
        {
            Expressions expressions = new Expressions(typeof(bool), elExpression, "GetResolveBooleanExpression", vars);
            return expressions.Evaluate<bool>("GetResolveBooleanExpression", vars);
        }

    }
}
