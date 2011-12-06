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
using FireWorkflow.Net.Engine.Definition;
using FireWorkflow.Net.Kernel.Impl;
using FireWorkflow.Net.Kernel.Plugin;


namespace FireWorkflow.Net.Kernel
{
    public class KernelManager : IRuntimeContextAware
    {
        /// <summary>工作流总线</summary>
        private RuntimeContext runtimeContext;
        /// <summary>工作流总线</summary>
        public RuntimeContext RuntimeContext
        {
            get { return this.runtimeContext; }
            set
            {
                this.runtimeContext = value;
                if (this.KernelExtensions != null && this.KernelExtensions.Count > 0)
                {
                    foreach (List<IKernelExtension> item in this.KernelExtensions.Values)
                    {
                        for (int i = 0; item != null && i < item.Count; i++)
                        {
                            IKernelExtension extension = (IKernelExtension)item[i];
                            ((IRuntimeContextAware)extension).RuntimeContext = this.runtimeContext;
                            //wangmj? 如何就强制转换成IRuntimeContextAware了？
                            //答：因为IKernelExtension 的实现类，同时也实现了接口IRuntimeContextAware
                        }
                    }
                }
            }
        }

        /// <summary>工作流网实例</summary>
        private Dictionary<String, INetInstance> netInstanceMap = new Dictionary<String, INetInstance>();

        /// <summary>
        /// wangmj spring 初始化的时候将扩展属性注入到这个map中
        /// 内核扩展对象
        /// </summary>
        public Dictionary<String, List<IKernelExtension>> KernelExtensions { get; set; }

        public KernelManager()
        {
            this.KernelExtensions = new Dictionary<String, List<IKernelExtension>>();
        }

        /// <summary>在获取工作流网实例的时候，调用createNetInstance方法，创建实例</summary>
        /// <param name="processId">流程定义ID</param>
        /// <param name="version">流程版本号</param>
        public INetInstance getNetInstance(String processId, Int32 version)
        {
            INetInstance netInstance = (this.netInstanceMap.ContainsKey(processId + "_V_" + version)) ? this.netInstanceMap[processId + "_V_" + version] : null;
            if (netInstance == null)
            {
                //数据流定义在runtimeContext初始化的时候，就被加载了，将流程定义的xml读入到内存中 
                IWorkflowDefinition def = this.RuntimeContext.DefinitionService.GetWorkflowDefinitionByProcessIdAndVersionNumber(processId, version);
                netInstance = this.createNetInstance(def);
            }
            return netInstance;
        }

        /// <summary>清空所有工作流网的实例</summary>
        public void clearAllNetInstance()
        {
            netInstanceMap.Clear();
        }

        /// <summary>创建一个工作流网实例</summary>
        /// <param name="workflowDef"></param>
        /// <returns></returns>
        public INetInstance createNetInstance(IWorkflowDefinition workflowDef)
        {
            if (workflowDef == null) return null;
            WorkflowProcess workflowProcess = null;
            workflowProcess = WorkflowDefinitionHelper.getWorkflowProcess(workflowDef);//解析fpdl 

            //Map nodeInstanceMap = new HashMap();
            if (workflowProcess == null)
            {
                throw new KernelException(null, null, "The WorkflowProcess property of WorkflowDefinition[processId=" + workflowDef.ProcessId + "] is null. ");
            }
            String validateMsg = workflowProcess.validate();//校验工作流定义是否有效 
            if (validateMsg != null)
            {
                throw new KernelException(null, null, validateMsg);
            }
            NetInstance netInstance = new NetInstance(workflowProcess, KernelExtensions);
            //netInstance.setWorkflowProcess(workflowProcess);
            netInstance.Version = workflowDef.Version;//设置版本号
            //map的key的组成规则：流程定义ID_V_版本号 
            netInstanceMap.Add(workflowDef.ProcessId + "_V_" + workflowDef.Version, netInstance);

            //netInstance.setRtCxt(new RuntimeContext());
            return netInstance;
        }

    }
}
