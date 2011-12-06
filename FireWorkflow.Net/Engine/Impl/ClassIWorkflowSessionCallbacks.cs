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
using FireWorkflow.Net.Engine.Definition;
using FireWorkflow.Net.Engine.Persistence;
using FireWorkflow.Net.Model;

namespace FireWorkflow.Net.Engine.Impl
{
    #region WorkflowSessionProcessInstance class
    public class WorkflowSessionIProcessInstance : IWorkflowSessionCallback
    {
        public String wfprocessId;
        public String creatorId;
        public String parentProcessInstanceId;
        public String parentTaskInstanceId;

        public WorkflowSessionIProcessInstance(String workflowProcessId, String creatorId, String parentProcessInstanceId, String parentTaskInstanceId)
        {
            this.wfprocessId = workflowProcessId;
            this.creatorId = creatorId;
            this.parentProcessInstanceId = parentProcessInstanceId;
            this.parentTaskInstanceId = parentTaskInstanceId;
        }
        public object doInWorkflowSession(RuntimeContext ctx)
        {

            IWorkflowDefinition workflowDef = ctx.DefinitionService.GetTheLatestVersionOfWorkflowDefinition(wfprocessId);
            WorkflowProcess wfProcess = null;

            wfProcess = WorkflowDefinitionHelper.getWorkflowProcess(workflowDef);

            if (wfProcess == null)
            {
                throw new Exception("Workflow process NOT found,id=[" + wfprocessId + "]");
            }

            IProcessInstance processInstance = new ProcessInstance();
            processInstance.CreatorId=creatorId;
            processInstance.ProcessId=wfProcess.Id;
            processInstance.Version=workflowDef.Version;
            processInstance.DisplayName=wfProcess.DisplayName;
            processInstance.Name=wfProcess.Name;
            processInstance.State=ProcessInstanceEnum.INITIALIZED;
            processInstance.CreatedTime=ctx.CalendarService.getSysDate();
            processInstance.ParentProcessInstanceId=parentProcessInstanceId;
            processInstance.ParentTaskInstanceId=parentTaskInstanceId;

            ctx.PersistenceService.SaveOrUpdateProcessInstance(
                    processInstance);

            // 初始化流程变量
            List<DataField> datafields = wfProcess.DataFields;
            for (int i = 0; datafields != null && i < datafields.Count; i++)
            {
                DataField df = (DataField)datafields[i];
                if (df.DataType == DataTypeEnum.STRING)
                {
                    if (df.InitialValue != null)
                    {
                        ProcessInstanceHelper.setProcessInstanceVariable(processInstance,df.Name, df.InitialValue);
                    }
                    else
                    {
                        ProcessInstanceHelper.setProcessInstanceVariable(processInstance,df.Name, "");
                    }
                }
                else if (df.DataType == DataTypeEnum.INTEGER)
                {
                    if (df.InitialValue != null)
                    {
                        try
                        {
                            Int32 intValue = Int32.Parse(df.InitialValue);
                            ProcessInstanceHelper.setProcessInstanceVariable(processInstance,df.Name, intValue);
                        }
                        catch (Exception )
                        {
                        }
                    }
                    else
                    {
                        ProcessInstanceHelper.setProcessInstanceVariable(processInstance,df.Name, (Int32)0);
                    }
                }
                else if (df.DataType == DataTypeEnum.LONG)
                {
                    if (df.InitialValue != null)
                    {
                        try
                        {
                            long longValue = long.Parse(df.InitialValue);
                            ProcessInstanceHelper.setProcessInstanceVariable(processInstance,df.Name, longValue);
                        }
                        catch (Exception )
                        {
                        }
                    }
                    else
                    {
                        ProcessInstanceHelper.setProcessInstanceVariable(processInstance,df.Name, (long)0);
                    }
                }
                else if (df.DataType == DataTypeEnum.FLOAT)
                {
                    if (df.InitialValue != null)
                    {
                        float floatValue = float.Parse(df.InitialValue);
                        ProcessInstanceHelper.setProcessInstanceVariable(processInstance,df.Name, floatValue);
                    }
                    else
                    {
                        ProcessInstanceHelper.setProcessInstanceVariable(processInstance,df.Name, (float)0);
                    }
                }
                else if (df.DataType == DataTypeEnum.DOUBLE)
                {
                    if (df.InitialValue != null)
                    {
                        Double doubleValue = Double.Parse(df.InitialValue);
                        ProcessInstanceHelper.setProcessInstanceVariable(processInstance,df.Name, doubleValue);
                    }
                    else
                    {
                        ProcessInstanceHelper.setProcessInstanceVariable(processInstance,df.Name, (Double)0);
                    }
                }
                else if (df.DataType == DataTypeEnum.BOOLEAN)
                {
                    if (df.InitialValue != null)
                    {
                        Boolean booleanValue = Boolean.Parse(df.InitialValue);
                        ProcessInstanceHelper.setProcessInstanceVariable(processInstance,df.Name, booleanValue);
                    }
                    else
                    {
                        ProcessInstanceHelper.setProcessInstanceVariable(processInstance,df.Name, false);
                    }
                }
                else if (df.DataType == DataTypeEnum.DATETIME)
                {
                    // TODO 需要完善一下
                    if (df.InitialValue != null
                            && df.DataPattern != null)
                    {
                        try
                        {
                            //SimpleDateFormat dFormat = new SimpleDateFormat(df.DataPattern);
                            DateTime dateTmp = DateTime.Parse(df.InitialValue);
                            ProcessInstanceHelper.setProcessInstanceVariable(processInstance,df.Name, dateTmp);
                        }
                        catch (Exception )
                        {
                            ProcessInstanceHelper.setProcessInstanceVariable(processInstance,df.Name, null);
                            //e.printStackTrace();
                        }
                    }
                    else
                    {
                        ProcessInstanceHelper.setProcessInstanceVariable(processInstance,df.Name, null);
                    }
                }
            }

            ctx.PersistenceService.SaveOrUpdateProcessInstance(
                    processInstance);

            return processInstance;
        }
    }
    #endregion


    #region WorkflowSessionIWorkItem
    public class WorkflowSessionIWorkItem : IWorkflowSessionCallback
    {
        String workItemId;
        public WorkflowSessionIWorkItem(String workItemId)
        {
            this.workItemId = workItemId;
        }

        public Object doInWorkflowSession(RuntimeContext ctx)
        {
            IPersistenceService persistenceService = ctx.PersistenceService;

            return persistenceService.FindWorkItemById(workItemId);
        }
    }
    #endregion




    #region WorkflowSessionITaskInstance
    public class WorkflowSessionITaskInstance : IWorkflowSessionCallback
    {
        String taskInstanceId;
        public WorkflowSessionITaskInstance(String taskInstanceId)
        {
            this.taskInstanceId = taskInstanceId;
        }

        public Object doInWorkflowSession(RuntimeContext ctx)
        {
            IPersistenceService persistenceService = ctx.PersistenceService;
            return persistenceService.FindTaskInstanceById(taskInstanceId);
        }
    }
    #endregion

    #region WorkflowSessionIWorkItems
    public class WorkflowSessionIWorkItems : IWorkflowSessionCallback
    {
        String actorId;
        String processId;
        String taskId;
        char t;
        public WorkflowSessionIWorkItems(String actorId)
        {
            t='1';
            this.actorId = actorId;
        }

        public WorkflowSessionIWorkItems(String actorId, String processId)
        {
            t = '2';
            this.actorId = actorId;
            this.processId = processId;
        }
        public WorkflowSessionIWorkItems(String actorId, String processId, String taskId)
        {
            t = '3';
            this.actorId = actorId;
            this.processId = processId;
            this.taskId = taskId;
        }

        public Object doInWorkflowSession(RuntimeContext ctx)
        {
            switch (t)
            {
                case '2': return ctx.PersistenceService.FindTodoWorkItems(actorId, processId);
                case '3': return ctx.PersistenceService.FindTodoWorkItems(actorId, processId, taskId);
                default: return ctx.PersistenceService.FindTodoWorkItems(actorId);
            }
        }
    }

    #endregion

    #region WorkflowSessionIProcessInstance
    public class WorkflowSessionIProcessInstance1 : IWorkflowSessionCallback
    {
        String id;
        public WorkflowSessionIProcessInstance1(String id)
        {
            this.id = id;
        }

        public Object doInWorkflowSession(RuntimeContext ctx)
        {
            IPersistenceService persistenceService = ctx.PersistenceService;
            return persistenceService.FindProcessInstanceById(id);
        }
    }
    public class WorkflowSessionIProcessInstanceCreateProcessInstance : IWorkflowSessionCallback
    {
        String creatorId;
        WorkflowProcess wfProcess;
        IWorkflowDefinition workflowDef;
        String parentProcessInstanceId;
        String parentTaskInstanceId;

        public WorkflowSessionIProcessInstanceCreateProcessInstance(String creatorId, WorkflowProcess wfProcess, 
            IWorkflowDefinition workflowDef, String parentProcessInstanceId,String parentTaskInstanceId)
        {
            this.creatorId = creatorId;
            this.wfProcess = wfProcess;
            this.workflowDef = workflowDef;
            this.parentProcessInstanceId = parentProcessInstanceId;
            this.parentTaskInstanceId = parentTaskInstanceId;
        }

        public Object doInWorkflowSession(RuntimeContext ctx)
        {

            IProcessInstance processInstance = new ProcessInstance();
            processInstance.CreatorId=creatorId;
            processInstance.ProcessId=wfProcess.Id;
            processInstance.Version=workflowDef.Version;
            processInstance.DisplayName = wfProcess.DisplayName;
            processInstance.Name = wfProcess.Name;
            processInstance.State = ProcessInstanceEnum.INITIALIZED;
            processInstance.CreatedTime=ctx.CalendarService.getSysDate();
            processInstance.ParentProcessInstanceId=parentProcessInstanceId;
            processInstance.ParentTaskInstanceId=parentTaskInstanceId;

            ctx.PersistenceService.SaveOrUpdateProcessInstance(processInstance);

            return processInstance;
        }
    }
    #endregion

    #region WorkflowSessionIProcessInstances
    public class WorkflowSessionIProcessInstances : IWorkflowSessionCallback
    {
        String processId;
        Int32 version;
        char t;
        public WorkflowSessionIProcessInstances(String processId)
        {
            t = '1';
            this.processId = processId;
        }
        public WorkflowSessionIProcessInstances(String processId,Int32 version)
        {
            t = '2';
            this.processId = processId;
            this.version=version; 
        }

        public Object doInWorkflowSession(RuntimeContext ctx)
        {
            switch (t)
            {
                case '2': return ctx.PersistenceService.FindProcessInstancesByProcessIdAndVersion(processId, version);
                //case '3': return ctx.PersistenceService.findProcessInstancesByProcessId(actorId, processId, taskId);
                default: return ctx.PersistenceService.FindProcessInstancesByProcessId(processId);
            }

        }
    }
    #endregion

    #region WorkflowSessionITaskInstances
    public class WorkflowSessionITaskInstances : IWorkflowSessionCallback
    {
        String processInstanceId;
        String activityId;
        public WorkflowSessionITaskInstances(String processInstanceId, String activityId)
        {
            this.processInstanceId = processInstanceId;
            this.activityId = activityId;
        }

        public Object doInWorkflowSession(RuntimeContext ctx)
        {
            IPersistenceService persistenceService = ctx.PersistenceService;
            return persistenceService.FindTaskInstancesForProcessInstance(processInstanceId, activityId);
        }
    }
    #endregion


}
