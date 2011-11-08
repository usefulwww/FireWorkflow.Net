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
using FireWorkflow.Net.Engine.Beanfactory;
using FireWorkflow.Net.Engine.Calendar;
using FireWorkflow.Net.Engine.Definition;
using FireWorkflow.Net.Engine.Impl;
using FireWorkflow.Net.Engine.Persistence;
using FireWorkflow.Net.Engine.Taskinstance;
using FireWorkflow.Net.Kernel;
using FireWorkflow.Net.Engine.Condition;

namespace FireWorkflow.Net.Engine
{
    /// <summary>
    /// <para>RuntimeContext是Fire workflow Engine的总线。所有的服务都挂接在这个总线上，并通过这个总线获取。</para>
    /// <para>RuntimeContext也是业务代码调用工作流引擎的入口，通过runtimeContext.getWorkflowSession()获得IWorkflowSession 对象，</para>
    /// <para>然后通过IWorkflowSession调用各种工作流实例对象及其API。</para>
    /// context管理的各种服务
    /// </summary>
    public class RuntimeContext
    {
        /// <summary>是否已经初始化</summary>
        public Boolean IsInitialized { get; set; }
        //context管理的各种服务

        /// <summary>是否打开流程跟踪，如果打开，则会往T_FF_HIST_TRACE表中插入纪录。</summary>
        public Boolean IsEnableTrace { get; set; }

        /// <summary>转移条件表达式解析服务</summary>
        private IConditionResolver _conditionResolver = null;
        /// <summary>设置或获取</summary>
        public IConditionResolver ConditionResolver
        {
            get { return _conditionResolver; }
            set
            {
                this._conditionResolver = value;
                if (this._conditionResolver is IRuntimeContextAware)
                {
                    ((IRuntimeContextAware)this._conditionResolver).RuntimeContext = this;
                }
            }
        }

        /// <summary>实例对象存取服务</summary>
        private IPersistenceService _persistenceService = null;
        /// <summary>实例对象存取服务</summary>
        public IPersistenceService PersistenceService
        {
            get { return this._persistenceService; }
            set
            {
                this._persistenceService = value;
                this._persistenceService.RuntimeContext = this;
            }
        }

        /// <summary>流程定义服务，通过该服务获取流程定义</summary>
        private IDefinitionService _definitionService = null;
        /// <summary>流程定义服务，通过该服务获取流程定义</summary>
        public IDefinitionService DefinitionService
        {
            get { return _definitionService; }
            set
            {
                this._definitionService = value;
                this._definitionService.RuntimeContext = this;
            }
        }

        /// <summary>任务分配处理服务</summary>
        private IAssignmentBusinessHandler _assignmentBusinessHandler = null;
        /// <summary>任务分配处理服务</summary>
        public IAssignmentBusinessHandler AssignmentBusinessHandler
        {
            get { return _assignmentBusinessHandler; }
            set
            {
                this._assignmentBusinessHandler = value;
                this._assignmentBusinessHandler.RuntimeContext = this;
            }
        }

        

        /// <summary>内核管理器</summary>
        private KernelManager _kernelManager = null;
        /// <summary>内核管理器</summary>
        public KernelManager KernelManager
        {
            get { return _kernelManager; }
            set
            {
                this._kernelManager = value;
                //KernelExtensions  Spring.net还没想到方法解决初始化
                this._kernelManager.KernelExtensions.Add("FireWorkflow.Net.Kernel.StartNodeInstance",
                    new List<Kernel.Plugin.IKernelExtension>() { new FireWorkflow.Net.Engine.Kernelextensions.StartNodeInstanceExtension() }
                    );
                this._kernelManager.KernelExtensions.Add("FireWorkflow.Net.Kernel.ActivityInstance",
                    new List<Kernel.Plugin.IKernelExtension>() { new FireWorkflow.Net.Engine.Kernelextensions.ActivityInstanceExtension() }
                    );
                this._kernelManager.KernelExtensions.Add("FireWorkflow.Net.Kernel.SynchronizerInstance",
                    new List<Kernel.Plugin.IKernelExtension>() { new FireWorkflow.Net.Engine.Kernelextensions.SynchronizerInstanceExtension() }
                    );
                this._kernelManager.KernelExtensions.Add("FireWorkflow.Net.Kernel.EndNodeInstance",
                    new List<Kernel.Plugin.IKernelExtension>() { new FireWorkflow.Net.Engine.Kernelextensions.EndNodeInstanceExtension() }
                    );
                this._kernelManager.KernelExtensions.Add("FireWorkflow.Net.Kernel.TransitionInstance",
                    new List<Kernel.Plugin.IKernelExtension>() { new FireWorkflow.Net.Engine.Kernelextensions.TransitionInstanceExtension() }
                    );
                this._kernelManager.KernelExtensions.Add("FireWorkflow.Net.Kernel.LoopInstance",
                    new List<Kernel.Plugin.IKernelExtension>() { new FireWorkflow.Net.Engine.Kernelextensions.LoopInstanceExtension() }
                    );
                this._kernelManager.RuntimeContext = this;
            }
        }

        /// <summary>TaskInstance 管理器，负责TaskInstance的创建、运行、结束。</summary>
        private ITaskInstanceManager _taskInstanceManager = null;
        /// <summary>TaskInstance 管理器，负责TaskInstance的创建、运行、结束。</summary>
        public ITaskInstanceManager TaskInstanceManager
        {
            get { return _taskInstanceManager; }
            set
            {
                this._taskInstanceManager = value;
                this._taskInstanceManager.RuntimeContext = this;
            }
        }

        /// <summary>日历服务</summary>
        private ICalendarService _calendarService = null;
        public ICalendarService CalendarService
        {
            get { return _calendarService; }
            set
            {
                this._calendarService = value;
                this._calendarService.RuntimeContext = this;
            }
        }

        /// <summary>bean工厂，fire workflow默认使用spring作为其实现</summary>
        public IBeanFactory BeanFactory { get; set; }

        public RuntimeContext()
        {
            IsInitialized = false;
            IsEnableTrace = false;
        }

        /// <summary>
        /// 根据bean的name返回bean的实例。<br/>
        /// Fire workflow RuntimeContext将该工作委派给org.fireflow.engine.beanfactory.IBeanFatory
        /// </summary>
        /// <param name="beanName">Bean Name具体指什么是由IBeanFatory的实现类来决定的。</param>
        /// <returns></returns>
        public Object getBeanByName(String beanName)
        {
            if (BeanFactory != null)
            {
                return BeanFactory.GetBean(beanName);
            }
            else
            {
                throw new NullReferenceException("The RuntimeContext's beanFactory  can NOT be null");
            }
        }

        public IWorkflowSession getWorkflowSession()
        {
            return new WorkflowSession(this);
        }

        /// <summary>初始化方法</summary>
        public void initialize()// throws EngineException, KernelException 
        {
            if (!IsInitialized)
            {
                initAllNetInstances();
                IsInitialized = true;
            }
        }

        protected void initAllNetInstances()// throws KernelException
        {
        }
    }

}
