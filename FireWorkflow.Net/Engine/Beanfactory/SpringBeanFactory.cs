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
using Spring.Context;
using Spring.Context.Support;

namespace FireWorkflow.Net.Engine.Beanfactory
{
    /// <summary>用Spring 的IOC容器作为Fire Workflow 的BeanFactory</summary>
    public class SpringBeanFactory : IBeanFactory
    {
        IApplicationContext springBeanFactory ;//= ContextRegistry.GetContext();// = new XmlApplicationContext(@"E:\Visual Studio 2010\Projects\FireWorkflow.Net\FireWorkflow.Net\FireflowContext.xml");

        public Object GetBean(String beanName)
        {
            if (springBeanFactory == null)
            {
                Type type = Type.GetType(beanName);
                if (type != null) return Activator.CreateInstance(type, null);
                return null;
            }
            return springBeanFactory.GetObject(beanName);
        }

        public void setBeanFactory(IApplicationContext arg0)// throws BeansException 
        {
            springBeanFactory = arg0;
        }


        public object GetBean(string beanName, params object[] args)
        {
            if (springBeanFactory == null)
            {
                Type type = Type.GetType(beanName);
                if (type != null) return Activator.CreateInstance(type, args);
                return null;
            }
            return springBeanFactory.GetObject(beanName);
        }

    }
}
