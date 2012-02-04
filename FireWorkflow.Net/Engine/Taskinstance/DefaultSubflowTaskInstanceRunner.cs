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
using FireWorkflow.Net.Engine.Definition;
using FireWorkflow.Net.Engine.Impl;
using FireWorkflow.Net.Engine.Persistence;
using FireWorkflow.Net.Kernel;
using FireWorkflow.Net.Model;
using FireWorkflow.Net.Model.Resource;


namespace FireWorkflow.Net.Engine.Taskinstance
{
    public class DefaultSubflowTaskInstanceRunner : ITaskInstanceRunner
    {

        public void run(IWorkflowSession currentSession, RuntimeContext runtimeContext,
            IProcessInstance processInstance, ITaskInstance taskInstance)// throws EngineException, KernelException 
        {
            if (taskInstance.TaskType != TaskTypeEnum.SUBFLOW)
            {
                throw new EngineException(processInstance, TaskInstanceHelper.getActivity(taskInstance),
                        "DefaultSubflowTaskInstanceRunner：TaskInstance的任务类型错误，只能为SUBFLOW类型");
            }
            Task task = TaskInstanceHelper.getTask(taskInstance);
            SubWorkflowProcess Subflow = ((SubflowTask)task).SubWorkflowProcess;

            IWorkflowDefinition subWorkflowDef = runtimeContext.DefinitionService.GetTheLatestVersionOfWorkflowDefinition(Subflow.WorkflowProcessId);
            if (subWorkflowDef == null)
            {
                WorkflowProcess parentWorkflowProcess = TaskInstanceHelper.getWorkflowProcess(taskInstance);
                throw new EngineException(taskInstance.ProcessInstanceId, parentWorkflowProcess, taskInstance.TaskId,
                        "系统中没有Id为" + Subflow.WorkflowProcessId + "的流程定义");
            }
            WorkflowProcess subWorkflowProcess = WorkflowDefinitionHelper.getWorkflowProcess(subWorkflowDef);

            if (subWorkflowProcess == null)
            {
                WorkflowProcess parentWorkflowProcess = TaskInstanceHelper.getWorkflowProcess(taskInstance);
                throw new EngineException(taskInstance.ProcessInstanceId, parentWorkflowProcess, taskInstance.TaskId,
                        "系统中没有Id为" + Subflow.WorkflowProcessId + "的流程定义");
            }

            IPersistenceService persistenceService = runtimeContext.PersistenceService;
            //更改任务的状态和开始时间
            taskInstance.State = TaskInstanceStateEnum.RUNNING;
            taskInstance.StartedTime = runtimeContext.CalendarService.getSysDate();
            persistenceService.SaveOrUpdateTaskInstance(taskInstance);

            IProcessInstance subProcessInstance = ProcessInstanceHelper.createProcessInstance(subWorkflowProcess.Name, taskInstance);

            //初始化流程变量,从父实例获得初始值
            Dictionary<String, Object> processVars = ProcessInstanceHelper.getProcessInstanceVariables(TaskInstanceHelper.getAliveProcessInstance(taskInstance));
            List<DataField> datafields = subWorkflowProcess.DataFields;
            for (int i = 0; datafields != null && i < datafields.Count; i++)
            {
                DataField df = (DataField)datafields[i];
                if (df.DataType == DataTypeEnum.STRING)
                {
                    if (processVars[df.Name] != null && (processVars[df.Name] is String))
                    {
                        ProcessInstanceHelper.setProcessInstanceVariable(subProcessInstance,df.Name, processVars[df.Name]);
                    }
                    else if (df.InitialValue != null)
                    {
                        ProcessInstanceHelper.setProcessInstanceVariable(subProcessInstance,df.Name, df.InitialValue);
                    }
                    else
                    {
                        ProcessInstanceHelper.setProcessInstanceVariable(subProcessInstance,df.Name, "");
                    }
                }
                else if (df.DataType == DataTypeEnum.INTEGER)
                {
                    if (processVars[df.Name] != null && (processVars[df.Name] is Int32))
                    {
                        ProcessInstanceHelper.setProcessInstanceVariable(subProcessInstance,df.Name, processVars[df.Name]);
                    }
                    else if (df.InitialValue != null)
                    {
                        try
                        {
                            Int32 intValue = Int32.Parse(df.InitialValue);
                            ProcessInstanceHelper.setProcessInstanceVariable(subProcessInstance,df.Name, intValue);
                        }
                        catch { }
                    }
                    else
                    {
                        ProcessInstanceHelper.setProcessInstanceVariable(subProcessInstance,df.Name, (Int32)0);
                    }
                }
                else if (df.DataType == DataTypeEnum.FLOAT)
                {
                    if (processVars[df.Name] != null && (processVars[df.Name] is float))
                    {
                        ProcessInstanceHelper.setProcessInstanceVariable(subProcessInstance,df.Name, processVars[df.Name]);
                    }
                    else if (df.InitialValue != null)
                    {
                        float floatValue = float.Parse(df.InitialValue);
                        ProcessInstanceHelper.setProcessInstanceVariable(subProcessInstance,df.Name, floatValue);
                    }
                    else
                    {
                        ProcessInstanceHelper.setProcessInstanceVariable(subProcessInstance,df.Name, (float)0);
                    }
                }
                else if (df.DataType == DataTypeEnum.BOOLEAN)
                {
                    if (processVars[df.Name] != null && (processVars[df.Name] is Boolean))
                    {
                        ProcessInstanceHelper.setProcessInstanceVariable(subProcessInstance,df.Name, processVars[df.Name]);
                    }
                    else if (df.InitialValue != null)
                    {
                        Boolean booleanValue = Boolean.Parse(df.InitialValue);
                        ProcessInstanceHelper.setProcessInstanceVariable(subProcessInstance,df.Name, booleanValue);
                    }
                    else
                    {
                        ProcessInstanceHelper.setProcessInstanceVariable(subProcessInstance,df.Name, false);
                    }
                }
                else if (df.DataType == DataTypeEnum.DATETIME)
                {
                    //TODO 需要完善一下 （ 父子流程数据传递——时间类型的数据还未做传递-不知道为什么？）
                    //wmj2003 20090925 补充上了
                    if (processVars[df.Name] != null && (processVars[df.Name] is DateTime))
                    {
                        ProcessInstanceHelper.setProcessInstanceVariable(subProcessInstance,df.Name, processVars[df.Name]);
                    }
                    else if (df.InitialValue != null)
                    {
                        try
                        {
                            DateTime dateTmp = DateTime.Parse(df.InitialValue);
                            ProcessInstanceHelper.setProcessInstanceVariable(subProcessInstance,df.Name, dateTmp);
                        }
                        catch
                        {
                            ProcessInstanceHelper.setProcessInstanceVariable(subProcessInstance,df.Name, null);
                        }
                    }
                }
                //TODO 应将下面这句删除！这里还需要吗？应该直接subProcessInstance.run()就可以了。
                runtimeContext.PersistenceService.SaveOrUpdateProcessInstance(subProcessInstance);
                ProcessInstanceHelper.run(subProcessInstance);
            }
        }
    }
}
