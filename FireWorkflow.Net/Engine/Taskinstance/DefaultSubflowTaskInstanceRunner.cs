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
                throw new EngineException(processInstance, taskInstance.Activity,
                        "DefaultSubflowTaskInstanceRunner：TaskInstance的任务类型错误，只能为SUBFLOW类型");
            }
            Task task = taskInstance.Task;
            SubWorkflowProcess Subflow = ((SubflowTask)task).SubWorkflowProcess;

            WorkflowDefinition subWorkflowDef = runtimeContext.DefinitionService.GetTheLatestVersionOfWorkflowDefinition(Subflow.WorkflowProcessId);
            if (subWorkflowDef == null)
            {
                WorkflowProcess parentWorkflowProcess = taskInstance.WorkflowProcess;
                throw new EngineException(taskInstance.ProcessInstanceId, parentWorkflowProcess, taskInstance.TaskId,
                        "系统中没有Id为" + Subflow.WorkflowProcessId + "的流程定义");
            }
            WorkflowProcess subWorkflowProcess = subWorkflowDef.getWorkflowProcess();

            if (subWorkflowProcess == null)
            {
                WorkflowProcess parentWorkflowProcess = taskInstance.WorkflowProcess;
                throw new EngineException(taskInstance.ProcessInstanceId, parentWorkflowProcess, taskInstance.TaskId,
                        "系统中没有Id为" + Subflow.WorkflowProcessId + "的流程定义");
            }

            IPersistenceService persistenceService = runtimeContext.PersistenceService;
            //更改任务的状态和开始时间
            ((TaskInstance)taskInstance).State = TaskInstanceStateEnum.RUNNING;
            ((TaskInstance)taskInstance).StartedTime = runtimeContext.CalendarService.getSysDate();
            persistenceService.SaveOrUpdateTaskInstance(taskInstance);

            IProcessInstance subProcessInstance = currentSession.createProcessInstance(subWorkflowProcess.Name, taskInstance);

            //初始化流程变量,从父实例获得初始值
            Dictionary<String, Object> processVars = ((TaskInstance)taskInstance).AliveProcessInstance.ProcessInstanceVariables;
            List<DataField> datafields = subWorkflowProcess.DataFields;
            for (int i = 0; datafields != null && i < datafields.Count; i++)
            {
                DataField df = (DataField)datafields[i];
                if (df.DataType == DataTypeEnum.STRING)
                {
                    if (processVars[df.Name] != null && (processVars[df.Name] is String))
                    {
                        subProcessInstance.setProcessInstanceVariable(df.Name, processVars[df.Name]);
                    }
                    else if (df.InitialValue != null)
                    {
                        subProcessInstance.setProcessInstanceVariable(df.Name, df.InitialValue);
                    }
                    else
                    {
                        subProcessInstance.setProcessInstanceVariable(df.Name, "");
                    }
                }
                else if (df.DataType == DataTypeEnum.INTEGER)
                {
                    if (processVars[df.Name] != null && (processVars[df.Name] is Int32))
                    {
                        subProcessInstance.setProcessInstanceVariable(df.Name, processVars[df.Name]);
                    }
                    else if (df.InitialValue != null)
                    {
                        try
                        {
                            Int32 intValue = Int32.Parse(df.InitialValue);
                            subProcessInstance.setProcessInstanceVariable(df.Name, intValue);
                        }
                        catch { }
                    }
                    else
                    {
                        subProcessInstance.setProcessInstanceVariable(df.Name, (Int32)0);
                    }
                }
                else if (df.DataType == DataTypeEnum.FLOAT)
                {
                    if (processVars[df.Name] != null && (processVars[df.Name] is float))
                    {
                        subProcessInstance.setProcessInstanceVariable(df.Name, processVars[df.Name]);
                    }
                    else if (df.InitialValue != null)
                    {
                        float floatValue = float.Parse(df.InitialValue);
                        subProcessInstance.setProcessInstanceVariable(df.Name, floatValue);
                    }
                    else
                    {
                        subProcessInstance.setProcessInstanceVariable(df.Name, (float)0);
                    }
                }
                else if (df.DataType == DataTypeEnum.BOOLEAN)
                {
                    if (processVars[df.Name] != null && (processVars[df.Name] is Boolean))
                    {
                        subProcessInstance.setProcessInstanceVariable(df.Name, processVars[df.Name]);
                    }
                    else if (df.InitialValue != null)
                    {
                        Boolean booleanValue = Boolean.Parse(df.InitialValue);
                        subProcessInstance.setProcessInstanceVariable(df.Name, booleanValue);
                    }
                    else
                    {
                        subProcessInstance.setProcessInstanceVariable(df.Name, false);
                    }
                }
                else if (df.DataType == DataTypeEnum.DATETIME)
                {
                    //TODO 需要完善一下 （ 父子流程数据传递——时间类型的数据还未做传递-不知道为什么？）
                    //wmj2003 20090925 补充上了
                    if (processVars[df.Name] != null && (processVars[df.Name] is DateTime))
                    {
                        subProcessInstance.setProcessInstanceVariable(df.Name, processVars[df.Name]);
                    }
                    else if (df.InitialValue != null)
                    {
                        try
                        {
                            DateTime dateTmp = DateTime.Parse(df.InitialValue);
                            subProcessInstance.setProcessInstanceVariable(df.Name, dateTmp);
                        }
                        catch
                        {
                            subProcessInstance.setProcessInstanceVariable(df.Name, null);
                        }
                    }
                }
                //TODO 应将下面这句删除！这里还需要吗？应该直接subProcessInstance.run()就可以了。
                runtimeContext.PersistenceService.SaveOrUpdateProcessInstance(subProcessInstance);
                subProcessInstance.run();
            }
        }
    }
}
