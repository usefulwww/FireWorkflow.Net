/* Copyright 2009 无忧lwz0721@gmail.com
 * @author 无忧lwz0721@gmail.com
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using FireWorkflow.Net.Engine;
using FireWorkflow.Net.Engine.Impl;
using FireWorkflow.Net.Engine.Definition;
using FireWorkflow.Net.Kernel;
using FireWorkflow.Net.Kernel.Impl;
using FireWorkflow.Net.Engine.Persistence;
using FireWorkflow.Net.Model;

namespace FireWorkflow.Net.Persistence.SqlServerDAL
{
    public class SqlDataReaderToInfo
    {
        /// <summary>
        /// 返回ProcessInstance，共14个字段
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static ProcessInstance GetProcessInstance(IDataReader dr)
        {
            ProcessInstance processInstance = new ProcessInstance();

            processInstance.Id=Convert.ToString(dr["id"]);
            processInstance.ProcessId=Convert.ToString(dr["process_id"]); 
            processInstance.Version=Convert.ToInt32(dr["version"]); 
            processInstance.Name=Convert.ToString(dr["name"]); 
            processInstance.DisplayName=Convert.ToString(dr["display_name"]); 

            processInstance.State= (ProcessInstanceEnum)Convert.ToInt32(dr["state"]); 
            processInstance.Suspended=Convert.ToInt32(dr["suspended"]) == 1; 
            processInstance.CreatorId=Convert.ToString(dr["creator_id"]); 
            if (!(dr["created_time"] is DBNull)) processInstance.CreatedTime=Convert.ToDateTime(dr["created_time"]); 
            if (!(dr["started_time"] is DBNull)) processInstance.StartedTime=Convert.ToDateTime(dr["started_time"]); 

            if (!(dr["expired_time"] is DBNull)) processInstance.ExpiredTime=Convert.ToDateTime(dr["expired_time"]); 
            if (!(dr["end_time"] is DBNull)) processInstance.EndTime=Convert.ToDateTime(dr["end_time"]); 

            processInstance.ParentProcessInstanceId=Convert.ToString(dr["parent_processinstance_id"]);
            processInstance.ParentTaskInstanceId=Convert.ToString(dr["parent_taskinstance_id"]);

            return processInstance;
        }

        /// <summary>
        /// 返回taskinstance (共21个字段)
        /// </summary>
        /// <param name="rs"></param>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        public static ITaskInstance GetTaskInstance(IDataReader dr)
        {
            ITaskInstance taskInstance = new TaskInstance();
            taskInstance.Id=Convert.ToString(dr["id"]);
            // 20090922 wmj2003 没有给biz_type赋值 是否需要给基于jdbc的数据增加 setBizType()方法？
            taskInstance.TaskId=Convert.ToString(dr["task_id"]);
            taskInstance.ActivityId=Convert.ToString(dr["activity_id"]);
            taskInstance.Name=Convert.ToString(dr["name"]);

            taskInstance.DisplayName=Convert.ToString(dr["display_name"]);
            taskInstance.State=(TaskInstanceStateEnum)Convert.ToInt32(dr["state"]);
            taskInstance.Suspended=Convert.ToInt32(dr["suspended"]) == 1 ? true : false;
            taskInstance.TaskType=(TaskTypeEnum)Enum.Parse(typeof(TaskTypeEnum),Convert.ToString(dr["task_type"]));
            if (!(dr["created_time"] is DBNull)) taskInstance.CreatedTime=Convert.ToDateTime(dr["created_time"]);

            if (!(dr["started_time"] is DBNull)) taskInstance.StartedTime = Convert.ToDateTime(dr["started_time"]);
            if (!(dr["end_time"] is DBNull)) taskInstance.EndTime = Convert.ToDateTime(dr["end_time"]);
            if (!(dr["expired_time"] is DBNull)) taskInstance.ExpiredTime = Convert.ToDateTime(dr["expired_time"]);

            taskInstance.AssignmentStrategy=(FormTaskEnum)Enum.Parse(typeof(FormTaskEnum), Convert.ToString((dr["assignment_strategy"])));
            taskInstance.ProcessInstanceId=Convert.ToString(dr["processinstance_id"]);
            taskInstance.ProcessId=Convert.ToString(dr["process_id"]);

            taskInstance.Version=Convert.ToInt32(dr["version"]);
            taskInstance.TargetActivityId=Convert.ToString(dr["target_activity_id"]);
            taskInstance.FromActivityId=Convert.ToString(dr["from_activity_id"]);
            taskInstance.StepNumber=Convert.ToInt32(dr["step_number"]);
            taskInstance.CanBeWithdrawn=Convert.ToInt32(dr["can_be_withdrawn"]) == 1 ? true : false;
            taskInstance.BizInfo = Convert.ToString(dr["biz_info"]);

            return taskInstance;
        }


        /// <summary>
        /// 返回WorkItem 共8个字段
        /// </summary>
        public static WorkItem GetWorkItem(IDataReader dr)
        {
            WorkItem workItem = new WorkItem();
            workItem.Id=Convert.ToString(dr["id"]);
            workItem.State= (WorkItemEnum)Convert.ToInt32(dr["state"]);
            if (!(dr["created_time"] is DBNull)) workItem.CreatedTime=Convert.ToDateTime(dr["created_time"]);
            if (!(dr["claimed_time"] is DBNull)) workItem.ClaimedTime=Convert.ToDateTime(dr["claimed_time"]);
            if (!(dr["end_time"] is DBNull)) workItem.EndTime=Convert.ToDateTime(dr["end_time"]);
            workItem.ActorId=Convert.ToString(dr["actor_id"]);
            workItem.TaskInstanceId=Convert.ToString(dr["taskinstance_id"]);
            workItem.Comments=Convert.ToString(dr["comments"]);
            return workItem;
        }

        public static Token GetToken(IDataReader dr)
        {
            Token token = new Token();
            token.Id=Convert.ToString(dr["id"]);
            token.IsAlive=Convert.ToInt32(dr["alive"]) == 1 ? true : false;
            token.Value=Convert.ToInt32(dr["value"]);
            token.NodeId=Convert.ToString(dr["node_id"]);
            token.ProcessInstanceId=Convert.ToString(dr["processinstance_id"]);
            token.StepNumber=Convert.ToInt32(dr["step_number"]);
            token.FromActivityId=Convert.ToString(dr["from_activity_id"]);

            return token;
        }

        public static WorkflowDefinition GetWorkflowDefinition(IDataReader dr)
        {
            WorkflowDefinition workFlowDefinition = new WorkflowDefinition();
            workFlowDefinition.Id=Convert.ToString(dr["id"]);
            workFlowDefinition.DefinitionType=Convert.ToString(dr["definition_type"]);
            workFlowDefinition.ProcessId=Convert.ToString(dr["process_id"]);
            workFlowDefinition.Name=Convert.ToString(dr["name"]);
            workFlowDefinition.DisplayName=Convert.ToString(dr["display_name"]);

            workFlowDefinition.Description=Convert.ToString(dr["description"]);
            workFlowDefinition.Version=Convert.ToInt32(dr["version"]);
            workFlowDefinition.State=Convert.ToInt32(dr["state"]) == 1 ? true : false;
            workFlowDefinition.UploadUser=Convert.ToString(dr["upload_user"]);
            if (!(dr["upload_time"] is DBNull)) workFlowDefinition.UploadTime=Convert.ToDateTime(dr["upload_time"]);

            workFlowDefinition.PublishUser=Convert.ToString(dr["publish_user"]);
            if (!(dr["publish_time"] is DBNull)) workFlowDefinition.PublishTime=Convert.ToDateTime(dr["publish_time"]);
            // 读取blob大字段
            workFlowDefinition.ProcessContent=Convert.ToString(dr["process_content"]);
            return workFlowDefinition;
        }

        public static ProcessInstanceTrace GetProcessInstanceTrace(IDataReader dr)
        {
                 ProcessInstanceTrace processInstanceTrace = new ProcessInstanceTrace(); 
  
                 processInstanceTrace.Id=Convert.ToString(dr["id"]);
                 processInstanceTrace.ProcessInstanceId=Convert.ToString(dr["processinstance_id"]);
                 processInstanceTrace.StepNumber=Convert.ToInt32(dr["step_number"]);
                 processInstanceTrace.MinorNumber=Convert.ToInt32(dr["minor_number"]);
                 processInstanceTrace.Type = (ProcessInstanceTraceEnum)Enum.Parse( typeof(ProcessInstanceTraceEnum), Convert.ToString(dr["type"]));
  
                 processInstanceTrace.EdgeId=Convert.ToString(dr["edge_id"]);
                 processInstanceTrace.FromNodeId=Convert.ToString(dr["from_node_id"]);
                 processInstanceTrace.ToNodeId=Convert.ToString(dr["to_node_id"]);
  
                 return processInstanceTrace; 
  
         }

        public static ProcessInstanceVar GetProcessInstanceVar(IDataReader dr)
        {
            ProcessInstanceVar processInstanceVar = new ProcessInstanceVar();
            ProcessInstanceVarPk pk = new ProcessInstanceVarPk();
            pk.ProcessInstanceId = Convert.ToString(dr["processinstance_id"]);// (rs.getString("processinstance_id"));
            pk.Name = Convert.ToString(dr["name"]); //(rs.getString("name"));
            processInstanceVar.VarPrimaryKey=pk;

            String valueStr = Convert.ToString(dr["value"]); //rs.getString("value");
            Object valueObj = GetProcessInstanceVarObject(valueStr);
            processInstanceVar.ValueType = valueObj.GetType().Name;
            processInstanceVar.Value = valueObj;

            return processInstanceVar;
        }

        public static Object GetProcessInstanceVarObject(String value)
    	{
    		if (value == null)
    			return null;
    		int index = value.IndexOf("#");
    		if (index == -1)
    		{
    			return null;
    		}
    		String type = value.Substring(0, index);
    		String strValue = value.Substring(index + 1);
    		if (type=="String")
    		{
    			return strValue;
    		}
    		if (String.IsNullOrEmpty(strValue.Trim()))
    		{
    			return null;
    		}
    		if (type=="Int32")
    		{
    			return Int32.Parse(strValue);
    		}
    		else if (type=="Int64")
    		{
                return Int64.Parse(strValue);
    		}
    		else if (type=="Single")
    		{
    			return float.Parse(strValue);
    		}
    		else if (type=="Double")
    		{
    			return Double.Parse(strValue);
    		}
    		else if (type=="Boolean")
    		{
    			return Boolean.Parse(strValue);
    		}
    		else if (type=="DateTime")
    		{
    			return DateTime.Parse(strValue);
    		}
    		else
    		{
    			throw new Exception("Fireflow不支持数据类型" + type);
    		}
    	}
    }
}
