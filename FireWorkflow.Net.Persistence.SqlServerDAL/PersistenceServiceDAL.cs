/* Copyright 2009 无忧lwz0721@gmail.com
 * @author 无忧lwz0721@gmail.com
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using FireWorkflow.Net.Engine;
using FireWorkflow.Net.Engine.Impl;
using FireWorkflow.Net.Engine.Definition;
using FireWorkflow.Net.Kernel;
using FireWorkflow.Net.Kernel.Impl;
using FireWorkflow.Net.Engine.Persistence;

namespace FireWorkflow.Net.Persistence.SqlServerDAL
{
    public class PersistenceServiceDAL : IPersistenceService
    {
        private string dbtype = "sqlserver";
        string connectionString = "Data Source=localhost\\sqlexpress;Initial Catalog=FireWorkflowExample;User Id=sa;Password=123456;";
        public PersistenceServiceDAL()
        {
            dbtype = ConfigurationManager.AppSettings["dbtype"];
            switch (dbtype)
            {
                case "sqlserver":
                    connectionString = ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString;
                    break;
                case "oracle":
                    connectionString = ConfigurationManager.ConnectionStrings["OracleServer"].ConnectionString;
                    break;
                default:
                    connectionString = ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString;
                    break;
            }
        }
        
        public PersistenceServiceDAL(String connName)
        {
            if (String.IsNullOrEmpty(connName)) throw new Exception("没有配置数据库连接字符串。");
            connectionString = ConfigurationManager.ConnectionStrings[connName].ConnectionString;
            if (String.IsNullOrEmpty(connectionString)) throw new Exception("没有配置数据库连接字符串。");
        }

        #region IRuntimeContextAware 成员
        public RuntimeContext RuntimeContext { get; set; }
        public void setRuntimeContext(RuntimeContext ctx)
        {
            this.RuntimeContext = ctx;
        }

        #endregion

        /******************************************************************************/
        /************                                                        **********/
        /************            Process instance 相关的持久化方法            **********/
        /************            Persistence methods for process instance    **********/
        /************                                                        **********/
        /******************************************************************************/
        /// <summary>
        /// 插入或者更新ProcessInstance流程实例
        /// </summary>
        public bool SaveOrUpdateProcessInstance(IProcessInstance processInstance)
        {
            if (String.IsNullOrEmpty(processInstance.Id))
            {
                ((ProcessInstance)processInstance).Id = Guid.NewGuid().ToString().Replace("-", "");
                string insert = "INSERT INTO T_FF_RT_PROCESSINSTANCE (" +
                    "ID, PROCESS_ID, VERSION, NAME, DISPLAY_NAME, " +
                    "STATE, SUSPENDED, CREATOR_ID, CREATED_TIME, STARTED_TIME, " +
                    "EXPIRED_TIME, END_TIME, PARENT_PROCESSINSTANCE_ID, PARENT_TASKINSTANCE_ID" +
                    ") VALUES(@1,@2,@3,@4,@5, @6,@7,@8,@9,@10, @11,@12,@13,@14)";
                SqlParameter[] insertParms = { 
    				SqlServerHelper.NewSqlParameter("@1", SqlDbType.VarChar, 50, processInstance.Id), 
    				SqlServerHelper.NewSqlParameter("@2", SqlDbType.VarChar, 100, processInstance.ProcessId), 
    				SqlServerHelper.NewSqlParameter("@3", SqlDbType.Int, processInstance.Version), 
    				SqlServerHelper.NewSqlParameter("@4", SqlDbType.VarChar, 100, processInstance.Name), 
    				SqlServerHelper.NewSqlParameter("@5", SqlDbType.VarChar, 128, processInstance.DisplayName), 
    				SqlServerHelper.NewSqlParameter("@6", SqlDbType.Int, (int)processInstance.State), 
    				SqlServerHelper.NewSqlParameter("@7", SqlDbType.SmallInt, SqlServerHelper.OraBit(processInstance.Suspended)), 
    				SqlServerHelper.NewSqlParameter("@8", SqlDbType.VarChar, 50, processInstance.CreatorId), 
    				SqlServerHelper.NewSqlParameter("@9", SqlDbType.DateTime, 11, processInstance.CreatedTime), 
    				SqlServerHelper.NewSqlParameter("@10", SqlDbType.DateTime, 11, processInstance.StartedTime), 
    				SqlServerHelper.NewSqlParameter("@11", SqlDbType.DateTime, 11, processInstance.ExpiredTime), 
    				SqlServerHelper.NewSqlParameter("@12", SqlDbType.DateTime, 11, processInstance.EndTime), 
    				SqlServerHelper.NewSqlParameter("@13", SqlDbType.VarChar, 50, processInstance.ParentProcessInstanceId), 
    				SqlServerHelper.NewSqlParameter("@14", SqlDbType.VarChar, 50, processInstance.ParentTaskInstanceId)
    			};
                if (SqlServerHelper.ExecuteNonQuery(connectionString, CommandType.Text, insert, insertParms) != 1)
                    return false;
                else return true;
            }
            else
            {
                string update = "UPDATE T_FF_RT_PROCESSINSTANCE SET " +
                     "PROCESS_ID=@2, VERSION=@3, NAME=@4, DISPLAY_NAME=@5, STATE=@6, " +
                     "SUSPENDED=@7, CREATOR_ID=@8, CREATED_TIME=@9, STARTED_TIME=@10, EXPIRED_TIME=@11, " +
                     "END_TIME=@12, PARENT_PROCESSINSTANCE_ID=@13, PARENT_TASKINSTANCE_ID=@14 WHERE ID=@1";
                SqlParameter[] updateParms = { 
    				SqlServerHelper.NewSqlParameter("@2", SqlDbType.VarChar, 100, processInstance.ProcessId), 
    				SqlServerHelper.NewSqlParameter("@3", SqlDbType.Int, processInstance.Version), 
    				SqlServerHelper.NewSqlParameter("@4", SqlDbType.VarChar, 100, processInstance.Name), 
    				SqlServerHelper.NewSqlParameter("@5", SqlDbType.VarChar, 128, processInstance.DisplayName), 
    				SqlServerHelper.NewSqlParameter("@6", SqlDbType.Int, (int)processInstance.State), 
    				SqlServerHelper.NewSqlParameter("@7", SqlDbType.SmallInt, SqlServerHelper.OraBit(processInstance.Suspended)), 
    				SqlServerHelper.NewSqlParameter("@8", SqlDbType.VarChar, 50, processInstance.CreatorId), 
    				SqlServerHelper.NewSqlParameter("@9", SqlDbType.DateTime, 11, processInstance.CreatedTime), 
    				SqlServerHelper.NewSqlParameter("@10", SqlDbType.DateTime, 11, processInstance.StartedTime), 
    				SqlServerHelper.NewSqlParameter("@11", SqlDbType.DateTime, 11, processInstance.ExpiredTime), 
    				SqlServerHelper.NewSqlParameter("@12", SqlDbType.DateTime, 11, processInstance.EndTime), 
    				SqlServerHelper.NewSqlParameter("@13", SqlDbType.VarChar, 50, processInstance.ParentProcessInstanceId), 
    				SqlServerHelper.NewSqlParameter("@14", SqlDbType.VarChar, 50, processInstance.ParentTaskInstanceId),
    				SqlServerHelper.NewSqlParameter("@1", SqlDbType.VarChar, 50, processInstance.Id)
    			};
                if (SqlServerHelper.ExecuteNonQuery(connectionString, CommandType.Text, update, updateParms) != 1)
                    return false;
                else return true;
            }
        }

        /// <summary>
        /// 通过ID获得“活的”ProcessInstance对象。
        /// “活的”是指ProcessInstance.state=INITIALIZED Or ProcessInstance.state=STARTED Or ProcessInstance=SUSPENDED的流程实例
        /// </summary>
        public IProcessInstance FindAliveProcessInstanceById(String id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                String select = String.Format("SELECT * FROM T_FF_RT_PROCESSINSTANCE WHERE ID=@1 and  state in ({0},{1})",
                    (int)ProcessInstanceEnum.INITIALIZED,
                    (int)ProcessInstanceEnum.RUNNING);
                SqlDataReader reader = null;
                try
                {
                    reader = SqlServerHelper.ExecuteReader(connection, CommandType.Text, select,
                        SqlServerHelper.NewSqlParameter("@1", SqlDbType.VarChar, 50, id)
                        );
                    while (reader.Read())
                    {
                        IProcessInstance info = SqlDataReaderToInfo.GetProcessInstance(reader);
                        return info;
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            return null;
        }

        /// <summary>
        /// 通过ID获得ProcessInstance对象。
        /// (Engine没有引用到该方法，提供给业务系统使用，20090303)
        /// </summary>
        public IProcessInstance FindProcessInstanceById(String id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                String select = "SELECT * FROM T_FF_RT_PROCESSINSTANCE WHERE ID=@1";
                SqlDataReader reader = null;
                try
                {
                    reader = SqlServerHelper.ExecuteReader(connection, CommandType.Text, select,
                        SqlServerHelper.NewSqlParameter("@1", SqlDbType.VarChar, 50, id)
                        );
                    while (reader.Read())
                    {
                        IProcessInstance info = SqlDataReaderToInfo.GetProcessInstance(reader);
                        return info;
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            return null;
        }

        /// <summary>
        /// 获得操作员发起的工作流实例总数量
        /// publishUser如果为null，获取全部
        /// </summary>
        /// <param name="creatorId">操作员主键</param>
        /// <param name="publishUser">流程定义发布者</param>
        /// <returns></returns>
        public Int32 GetProcessInstanceCountByCreatorId(String creatorId, String publishUser)
        {
            return 0;
        }

        /// <summary>
        /// 获得工作流发布者发起的所有流程定义的工作流实例列表（分页）
        /// </summary>
        /// <param name="publishUser">工作流发布者</param>
        /// <param name="pageSize">每页显示的条数</param>
        /// <param name="pageNumber">当前页数</param>
        /// <returns></returns>
        public IList<IProcessInstance> FindProcessInstanceListByPublishUser(String publishUser, int pageSize, int pageNumber)
        {
            return FindProcessInstanceListByCreatorId("", publishUser, pageSize, pageNumber);
        }

        /// <summary>
        /// 获得操作员发起的工作流实例列表(运行中)（分页）
        /// publishUser如果为null，获取全部
        /// </summary>
        /// <param name="creatorId">操作员主键</param>
        /// <param name="publishUser">流程定义发布者</param>
        /// <param name="pageSize">每页显示的条数</param>
        /// <param name="pageNumber">当前页数</param>
        /// <returns></returns>
        public IList<IProcessInstance> FindProcessInstanceListByCreatorId(String creatorId, String publishUser, int pageSize, int pageNumber)
        {
            int sum = 0;
            IList<IProcessInstance> _IProcessInstances = new List<IProcessInstance>();

            QueryField queryField = new QueryField();
            queryField.Add(new QueryFieldInfo("a.creator_id", CSharpType.String, creatorId));
            queryField.Add(new QueryFieldInfo("b.publish_user", CSharpType.String, publishUser));
            QueryInfo queryInfo = SqlServerHelper.GetFormatQuery(queryField);

            SqlConnection conn = new SqlConnection(connectionString);
            SqlDataReader reader = null;
            try
            {
                reader = SqlServerHelper.ExecuteReader(conn, pageNumber, pageSize, out sum,
                    "T_FF_RT_PROCESSINSTANCE a,t_ff_df_workflowdef b",
                    "a.*,b.publish_user",
                    String.Format("a.process_id=b.process_id and a.version=b.version {0}", queryInfo.QueryStringAnd),
                    "a.created_time desc",
                    queryInfo.ListQueryParameters == null ? null : queryInfo.ListQueryParameters.ToArray());

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        IProcessInstance _IProcessInstance = SqlDataReaderToInfo.GetProcessInstance(reader);
                        _IProcessInstances.Add(_IProcessInstance);
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (reader != null) reader.Close();
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
            return _IProcessInstances;
        }

        /// <summary>
        /// 查找并返回同一个业务流程的所有实例
        /// (Engine没有引用到该方法，提供给业务系统使用，20090303)
        /// </summary>
        /// <param name="processId">The id of the process definition.</param>
        /// <returns>A list of processInstance</returns>
        public IList<IProcessInstance> FindProcessInstancesByProcessId(String processId)
        {
            IList<IProcessInstance> infos = new List<IProcessInstance>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string select = "select * from t_ff_rt_processinstance where process_id=@1 order by created_time";
                SqlDataReader reader = null;
                try
                {
                    reader = SqlServerHelper.ExecuteReader(connection, CommandType.Text, select,
                        SqlServerHelper.NewSqlParameter("@1", SqlDbType.VarChar, 100, processId)
                        );
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            IProcessInstance info = SqlDataReaderToInfo.GetProcessInstance(reader);
                            infos.Add(info);
                        }
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            return infos;
        }

        /// <summary>
        /// 查找并返回同一个指定版本业务流程的所有实例
        /// (Engine没有引用到该方法，提供给业务系统使用，20090303)
        /// </summary>
        /// <param name="processId">The id of the process definition.</param>
        /// <param name="version">版本号</param>
        /// <returns>A list of processInstance</returns>
        public IList<IProcessInstance> FindProcessInstancesByProcessIdAndVersion(String processId, int version)
        {
            IList<IProcessInstance> infos = new List<IProcessInstance>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string select = "select * from t_ff_rt_processinstance where process_id=@1 and version=@2 order by created_time";
                SqlDataReader reader = null;
                try
                {
                    reader = SqlServerHelper.ExecuteReader(connection, CommandType.Text, select,
                        SqlServerHelper.NewSqlParameter("@1", SqlDbType.VarChar, 100, processId),
                        SqlServerHelper.NewSqlParameter("@2", SqlDbType.Int, version)
                        );
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            IProcessInstance info = SqlDataReaderToInfo.GetProcessInstance(reader);
                            infos.Add(info);
                        }
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            return infos;
        }

        /// <summary>
        /// 计算活动的子流程实例的数量
        /// </summary>
        /// <param name="taskInstanceId">父TaskInstance的Id</param>
        /// <returns></returns>
        public int GetAliveProcessInstanceCountForParentTaskInstance(String taskInstanceId)
        {
            String select = String.Format("select count(*) from t_ff_rt_processinstance where parent_taskinstance_id=@1 and state in({0},{1})",
                (int)ProcessInstanceEnum.INITIALIZED, (int)ProcessInstanceEnum.RUNNING);
            SqlParameter[] selectParms = { 
				SqlServerHelper.NewSqlParameter("@1", SqlDbType.VarChar, 50, taskInstanceId)
		    };

            return SqlServerHelper.ExecuteInt32(this.connectionString, CommandType.Text, select, selectParms);
        }

        /// <summary>
        /// 终止流程实例。将流程实例、活动的TaskInstance、活动的WorkItem的状态设置为CANCELED；并删除所有的token
        /// </summary>
        public bool AbortProcessInstance(IProcessInstance processInstance)
        {
            SqlTransaction transaction = SqlServerHelper.GetSqlTransaction(this.connectionString);
            try
            {
                // 更新流程状态，设置为canceled
                DateTime now = this.RuntimeContext.CalendarService.getSysDate();
                String processSql = "update t_ff_rt_processinstance set state=" + (int)ProcessInstanceEnum.CANCELED
                        + ",end_time=@1 where id=@2 ";
                int count = SqlServerHelper.ExecuteNonQuery(transaction, CommandType.Text, processSql,
                    SqlServerHelper.NewSqlParameter("@1", SqlDbType.DateTime, 11, now),
                    SqlServerHelper.NewSqlParameter("@2", SqlDbType.VarChar, 50, processInstance.Id)
                    );
                if (count <= 0)
                {
                    transaction.Rollback();
                    return false;
                }


                // 更新所有的任务实例状态为canceled
                String taskSql = " update t_ff_rt_taskinstance set state=" + (int)TaskInstanceStateEnum.CANCELED
                        + ",end_time=@1,can_be_withdrawn=0 " + "  where processinstance_id=@2 and (state=0 or state=1)";
                count = SqlServerHelper.ExecuteNonQuery(transaction, CommandType.Text, taskSql,
                    SqlServerHelper.NewSqlParameter("@1", SqlDbType.DateTime, 11, now),
                    SqlServerHelper.NewSqlParameter("@2", SqlDbType.VarChar, 50, processInstance.Id)
                    );
                if (count <= 0)
                {
                    transaction.Rollback();
                    return false;
                }

                // 更新所有工作项的状态为canceled
                String workItemSql = " update t_ff_rt_workitem set state="
                        + (int)WorkItemEnum.CANCELED
                        + ",end_time=@1  "
                        + " where taskinstance_id in (select a.id  from t_ff_rt_taskinstance a,t_ff_rt_workitem b where a.id=b.taskinstance_id and a.processinstance_id=@2 ) and (state=0 or state=1) ";
                count = SqlServerHelper.ExecuteNonQuery(transaction, CommandType.Text, workItemSql,
                    SqlServerHelper.NewSqlParameter("@1", SqlDbType.DateTime, 11, now),
                    SqlServerHelper.NewSqlParameter("@2", SqlDbType.VarChar, 50, processInstance.Id)
                    );
                if (count <= 0)
                {
                    transaction.Rollback();
                    return false;
                }

                // 删除所有的token
                String tokenSql = " delete from t_ff_rt_token where processinstance_id=@1  ";
                count = SqlServerHelper.ExecuteNonQuery(transaction, CommandType.Text, tokenSql,
                    SqlServerHelper.NewSqlParameter("@1", SqlDbType.VarChar, 50, processInstance.Id)
                    );
                if (count <= 0)
                {
                    transaction.Rollback();
                    return false;
                }

                // 数据库操作成功后，更新对象的状态
                processInstance.State = ProcessInstanceEnum.CANCELED;

                transaction.Commit();
                return true;

            }
            catch
            {
                transaction.Rollback();
                return false;
            }
            finally
            {
                if (transaction.Connection.State != ConnectionState.Closed)
                {
                    transaction.Connection.Close();
                    transaction.Dispose();
                }
            }
        }

        /// <summary>
        /// 挂起流程实例
        /// </summary>
        public bool SuspendProcessInstance(IProcessInstance processInstance)
        {
            SqlTransaction transaction = SqlServerHelper.GetSqlTransaction(this.connectionString);
            try
            {

                String sql = " update t_ff_rt_processinstance set suspended=1 where id=@1 ";
                int count = SqlServerHelper.ExecuteNonQuery(transaction, CommandType.Text, sql,
                    SqlServerHelper.NewSqlParameter("@1", SqlDbType.VarChar, 50, processInstance.Id)
                    );
                if (count <= 0)
                {
                    transaction.Rollback();
                    return false;
                }

                // 挂起对应的TaskInstance
                String sql2 = " update t_ff_rt_taskinstance set suspended=1 where processinstance_id=@1 ";
                count = SqlServerHelper.ExecuteNonQuery(transaction, CommandType.Text, sql2,
                    SqlServerHelper.NewSqlParameter("@1", SqlDbType.VarChar, 50, processInstance.Id)
                    );
                if (count <= 0)
                {
                    transaction.Rollback();
                    return false;
                }

                processInstance.Suspended = true;

                transaction.Commit();
                return true;

            }
            catch
            {
                transaction.Rollback();
                return false;
            }
            finally
            {
                if (transaction.Connection.State != ConnectionState.Closed)
                {
                    transaction.Connection.Close();
                    transaction.Dispose();
                }
            }
        }

        /// <summary>
        /// 恢复流程实例
        /// </summary>
        public bool RestoreProcessInstance(IProcessInstance processInstance)
        {
            SqlTransaction transaction = SqlServerHelper.GetSqlTransaction(this.connectionString);
            try
            {
                String sql = " update t_ff_rt_processinstance set suspended=0 where id=@1 ";
                int count = SqlServerHelper.ExecuteNonQuery(transaction, CommandType.Text, sql,
                    SqlServerHelper.NewSqlParameter("@1", SqlDbType.VarChar, 50, processInstance.Id)
                    );
                if (count <= 0)
                {
                    transaction.Rollback();
                    return false;
                }

                // 恢复对应的TaskInstance
                String sql2 = " update t_ff_rt_taskinstance set suspended=0 where processinstance_id=@1";
                count = SqlServerHelper.ExecuteNonQuery(transaction, CommandType.Text, sql2,
                    SqlServerHelper.NewSqlParameter("@1", SqlDbType.VarChar, 50, processInstance.Id)
                    );
                if (count <= 0)
                {
                    transaction.Rollback();
                    return false;
                }

                processInstance.Suspended = false;

                transaction.Commit();
                return true;

            }
            catch
            {
                transaction.Rollback();
                return false;
            }
            finally
            {
                if (transaction.Connection.State != ConnectionState.Closed)
                {
                    transaction.Connection.Close();
                    transaction.Dispose();
                }
            }
        }

        /*****************************************************************************/
        /************                                                        **********/
        /************            task instance 相关的持久化方法               **********/
        /************            Persistence methods for task instance       **********/
        /************                                                        **********/
        /******************************************************************************/

        /// <summary>
        /// <para>插入或者更新TaskInstance。</para>
        /// <para>Save or update task instance. If the taskInstance.id is null then insert a new task instance record</para>
        /// <para>and generate a new id for it { throw new NotImplementedException(); }</para>
        /// <para>otherwise update the existent one. </para>
        /// </summary>
        public bool SaveOrUpdateTaskInstance(ITaskInstance taskInstance)
        {
            if (String.IsNullOrEmpty(taskInstance.Id))
            {
                taskInstance.Id = Guid.NewGuid().ToString().Replace("-", "");
                string insert = "INSERT INTO T_FF_RT_TASKINSTANCE (" +
                "ID, BIZ_TYPE, TASK_ID, ACTIVITY_ID, NAME, " +
                "DISPLAY_NAME, STATE, SUSPENDED, TASK_TYPE, CREATED_TIME, " +
                "STARTED_TIME, EXPIRED_TIME, END_TIME, ASSIGNMENT_STRATEGY, PROCESSINSTANCE_ID, " +
                "PROCESS_ID, VERSION, TARGET_ACTIVITY_ID, FROM_ACTIVITY_ID, STEP_NUMBER, " +
                "CAN_BE_WITHDRAWN, BIZ_INFO )VALUES(@1, @2, @3, @4, @5, @6, @7, @8, @9, @10, @11, @12, @13, @14, @15, @16, @17, @18, @19, @20, @21, @22)";
                SqlParameter[] insertParms = { 
    				SqlServerHelper.NewSqlParameter("@1", SqlDbType.VarChar, 50, taskInstance.Id), 
    				SqlServerHelper.NewSqlParameter("@2", SqlDbType.VarChar, 250, taskInstance.GetType().Name), 
    				SqlServerHelper.NewSqlParameter("@3", SqlDbType.VarChar, 300, taskInstance.TaskId), 
    				SqlServerHelper.NewSqlParameter("@4", SqlDbType.VarChar, 200, taskInstance.ActivityId), 
    				SqlServerHelper.NewSqlParameter("@5", SqlDbType.VarChar, 100, taskInstance.Name), 
    				SqlServerHelper.NewSqlParameter("@6", SqlDbType.VarChar, 128, taskInstance.DisplayName), 
    				SqlServerHelper.NewSqlParameter("@7", SqlDbType.Int, (int)taskInstance.State), 
    				SqlServerHelper.NewSqlParameter("@8", SqlDbType.SmallInt, SqlServerHelper.OraBit(taskInstance.Suspended)), 
    				SqlServerHelper.NewSqlParameter("@9", SqlDbType.VarChar, 10, taskInstance.TaskType), 
    				SqlServerHelper.NewSqlParameter("@10", SqlDbType.DateTime, 11, taskInstance.CreatedTime), 
    				SqlServerHelper.NewSqlParameter("@11", SqlDbType.DateTime, 11, taskInstance.StartedTime), 
    				SqlServerHelper.NewSqlParameter("@12", SqlDbType.DateTime, 11, taskInstance.ExpiredTime), 
    				SqlServerHelper.NewSqlParameter("@13", SqlDbType.DateTime, 11, taskInstance.EndTime), 
    				SqlServerHelper.NewSqlParameter("@14", SqlDbType.VarChar, 10, taskInstance.AssignmentStrategy), 
    				SqlServerHelper.NewSqlParameter("@15", SqlDbType.VarChar, 50, taskInstance.ProcessInstanceId), 
    				SqlServerHelper.NewSqlParameter("@16", SqlDbType.VarChar, 100, taskInstance.ProcessId), 
    				SqlServerHelper.NewSqlParameter("@17", SqlDbType.Int, taskInstance.Version), 
    				SqlServerHelper.NewSqlParameter("@18", SqlDbType.VarChar, 100, taskInstance.TargetActivityId), 
    				SqlServerHelper.NewSqlParameter("@19", SqlDbType.VarChar, 600, taskInstance.FromActivityId), 
    				SqlServerHelper.NewSqlParameter("@20", SqlDbType.Int, taskInstance.StepNumber), 
    				SqlServerHelper.NewSqlParameter("@21", SqlDbType.SmallInt, SqlServerHelper.OraBit(taskInstance.CanBeWithdrawn)),
    				SqlServerHelper.NewSqlParameter("@22", SqlDbType.VarChar, 500, taskInstance.BizInfo)
    			};
                if (SqlServerHelper.ExecuteNonQuery(connectionString, CommandType.Text, insert, insertParms) != 1)
                    return false;
                else return true;
            }
            else
            {
                string update = "UPDATE T_FF_RT_TASKINSTANCE SET " +
                "BIZ_TYPE=@2, TASK_ID=@3, ACTIVITY_ID=@4, NAME=@5, DISPLAY_NAME=@6, " +
                "STATE=@7, SUSPENDED=@8, TASK_TYPE=@9, CREATED_TIME=@10, STARTED_TIME=@11, " +
                "EXPIRED_TIME=@12, END_TIME=@13, ASSIGNMENT_STRATEGY=@14, PROCESSINSTANCE_ID=@15, PROCESS_ID=@16, " +
                "VERSION=@17, TARGET_ACTIVITY_ID=@18, FROM_ACTIVITY_ID=@19, STEP_NUMBER=@20, CAN_BE_WITHDRAWN=@21, BIZ_INFO=@22" +
                " WHERE ID=@1";
                SqlParameter[] updateParms = { 
    				SqlServerHelper.NewSqlParameter("@2", SqlDbType.VarChar, 250, taskInstance.GetType().Name), 
    				SqlServerHelper.NewSqlParameter("@3", SqlDbType.VarChar, 300, taskInstance.TaskId), 
    				SqlServerHelper.NewSqlParameter("@4", SqlDbType.VarChar, 200, taskInstance.ActivityId), 
    				SqlServerHelper.NewSqlParameter("@5", SqlDbType.VarChar, 100, taskInstance.Name), 
    				SqlServerHelper.NewSqlParameter("@6", SqlDbType.VarChar, 128, taskInstance.DisplayName), 
    				SqlServerHelper.NewSqlParameter("@7", SqlDbType.Int, (int)taskInstance.State), 
    				SqlServerHelper.NewSqlParameter("@8", SqlDbType.SmallInt, SqlServerHelper.OraBit(taskInstance.Suspended)), 
    				SqlServerHelper.NewSqlParameter("@9", SqlDbType.VarChar, 10, taskInstance.TaskType), 
    				SqlServerHelper.NewSqlParameter("@10", SqlDbType.DateTime, 11, taskInstance.CreatedTime), 
    				SqlServerHelper.NewSqlParameter("@11", SqlDbType.DateTime, 11, taskInstance.StartedTime), 
    				SqlServerHelper.NewSqlParameter("@12", SqlDbType.DateTime, 11, taskInstance.ExpiredTime), 
    				SqlServerHelper.NewSqlParameter("@13", SqlDbType.DateTime, 11, taskInstance.EndTime), 
    				SqlServerHelper.NewSqlParameter("@14", SqlDbType.VarChar, 10, taskInstance.AssignmentStrategy), 
    				SqlServerHelper.NewSqlParameter("@15", SqlDbType.VarChar, 50, taskInstance.ProcessInstanceId), 
    				SqlServerHelper.NewSqlParameter("@16", SqlDbType.VarChar, 100, taskInstance.ProcessId), 
    				SqlServerHelper.NewSqlParameter("@17", SqlDbType.Int, taskInstance.Version), 
    				SqlServerHelper.NewSqlParameter("@18", SqlDbType.VarChar, 100, taskInstance.TargetActivityId), 
    				SqlServerHelper.NewSqlParameter("@19", SqlDbType.VarChar, 600, taskInstance.FromActivityId), 
    				SqlServerHelper.NewSqlParameter("@20", SqlDbType.Int, taskInstance.StepNumber), 
    				SqlServerHelper.NewSqlParameter("@21", SqlDbType.SmallInt, SqlServerHelper.OraBit(taskInstance.CanBeWithdrawn)),
    				SqlServerHelper.NewSqlParameter("@22", SqlDbType.VarChar, 500, taskInstance.BizInfo),
    				SqlServerHelper.NewSqlParameter("@1", SqlDbType.VarChar, 50, taskInstance.Id)
    			};
                if (SqlServerHelper.ExecuteNonQuery(connectionString, CommandType.Text, update, updateParms) != 1)
                    return false;
                else return true;
            }
        }

        /// <summary>
        /// 终止TaskInstance。将任务实例及其所有的“活的”WorkItem变成Canceled状态。
        /// "活的"WorkItem 是指状态等于INITIALIZED、STARTED或者SUSPENDED的WorkItem.
        /// </summary>
        public bool AbortTaskInstance(ITaskInstance taskInstance)
        {
            SqlTransaction transaction = SqlServerHelper.GetSqlTransaction(connectionString);
            try
            {
                String sql = "update t_ff_rt_taskinstance set state=" 
                	+ (int)TaskInstanceStateEnum.CANCELED
                	+ " ,end_time=@1 where id=@2 and (state=0 or state=1)";
                int count = SqlServerHelper.ExecuteNonQuery(transaction, CommandType.Text, sql,
                    SqlServerHelper.NewSqlParameter("@1", SqlDbType.DateTime, 11, this.RuntimeContext.CalendarService.getSysDate()),
                    SqlServerHelper.NewSqlParameter("@2", SqlDbType.VarChar, 50, taskInstance.Id)
                    );
                if (count <= 0)
                {
                    transaction.Rollback();
                    return false;
                }


                // 将与之关联的WorkItem取消掉
                String workItemSql = " update t_ff_rt_workitem set state=" + (int)WorkItemEnum.CANCELED + ",end_time=@1  "
                        + " where taskinstance_id =@2 ";
                count = SqlServerHelper.ExecuteNonQuery(transaction, CommandType.Text, workItemSql,
                    SqlServerHelper.NewSqlParameter("@1", SqlDbType.DateTime, 11, this.RuntimeContext.CalendarService.getSysDate()),
                    SqlServerHelper.NewSqlParameter("@2", SqlDbType.VarChar, 50, taskInstance.Id)
                    );
                if (count <= 0)
                {
                    transaction.Rollback();
                    return false;
                }

                taskInstance.State = TaskInstanceStateEnum.CANCELED;

                transaction.Commit();
                return true;

            }
            catch
            {
                transaction.Rollback();
                return false;
            }
            finally
            {
                if (transaction.Connection.State != ConnectionState.Closed)
                {
                    transaction.Connection.Close();
                    transaction.Dispose();
                }
            }
        }

        /// <summary>
        /// 返回“活的”TaskInstance。
        /// “活的”是指TaskInstance.state=INITIALIZED Or TaskInstance.state=STARTED 。
        /// </summary>
        public ITaskInstance FindAliveTaskInstanceById(String id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                String select = "select * from t_ff_rt_taskinstance where id=@1 and  (state=" + (int)TaskInstanceStateEnum.INITIALIZED
                        + " or state=" + (int)TaskInstanceStateEnum.RUNNING + " )";
                SqlDataReader reader = null;
                try
                {
                    reader = SqlServerHelper.ExecuteReader(connection, CommandType.Text, select,
                        SqlServerHelper.NewSqlParameter("@1", SqlDbType.VarChar, 50, id)
                        );
                    while (reader.Read())
                    {
                        ITaskInstance info = SqlDataReaderToInfo.GetTaskInstance(reader);
                        return info;
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            return null;
        }

        /// <summary>
        /// 获得activity的“活的”TaskInstance的数量
        /// “活的”是指TaskInstance.state=INITIALIZED Or TaskInstance.state=STARTED 。
        /// </summary>
        public int GetAliveTaskInstanceCountForActivity(String processInstanceId, String activityId)
        {
            String select = "select count(*) from T_FF_RT_TASKINSTANCE where " + " (state=" + (int)TaskInstanceStateEnum.INITIALIZED
                + " or state=" + (int)TaskInstanceStateEnum.RUNNING + ")" + " and activity_id=@1 and processinstance_id=@2";
            SqlParameter[] selectParms = { 
				SqlServerHelper.NewSqlParameter("@1", SqlDbType.VarChar, 200, activityId), 
				SqlServerHelper.NewSqlParameter("@2", SqlDbType.VarChar, 50, processInstanceId), 
		    };

            return SqlServerHelper.ExecuteInt32(this.connectionString, CommandType.Text, select, selectParms);
        }

        /// <summary>
        /// 返回某个Task已经结束的TaskInstance的数量
        /// “已经结束”是指TaskInstance.state=COMPLETED。
        /// </summary>
        public int GetCompletedTaskInstanceCountForTask(String processInstanceId, String taskId)
        {
            String select = "select count(*) from T_FF_RT_TASKINSTANCE where state=" + (int)TaskInstanceStateEnum.COMPLETED
                        + " and task_id=@1 and processinstance_id=@2 ";
            SqlParameter[] selectParms = { 
				SqlServerHelper.NewSqlParameter("@1", SqlDbType.VarChar, 300, taskId), 
				SqlServerHelper.NewSqlParameter("@2", SqlDbType.VarChar, 50, processInstanceId), 
		    };

            return SqlServerHelper.ExecuteInt32(this.connectionString, CommandType.Text, select, selectParms);
        }

        /// <summary>
        /// Find the task instance by id
        /// (Engine没有引用到该方法，提供给业务系统使用，20090303)
        /// </summary>
        public ITaskInstance FindTaskInstanceById(String id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                String select = "select * from t_ff_rt_taskinstance where id=@1";
                SqlDataReader reader = null;
                try
                {
                    reader = SqlServerHelper.ExecuteReader(connection, CommandType.Text, select,
                        SqlServerHelper.NewSqlParameter("@1", SqlDbType.VarChar, 50, id)
                        );
                    while (reader.Read())
                    {
                        ITaskInstance info = SqlDataReaderToInfo.GetTaskInstance(reader);
                        return info;
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            return null;
        }

        /// <summary>
        /// 查询流程实例的所有的TaskInstance,如果activityId不为空，则返回该流程实例下指定环节的TaskInstance
        /// (Engine没有引用到该方法，提供给业务系统使用，20090303)
        /// </summary>
        public IList<ITaskInstance> FindTaskInstancesForProcessInstance(String processInstanceId, String activityId)
        {
            IList<ITaskInstance> infos = new List<ITaskInstance>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string select;
                SqlParameter[] selectParms;
                if (String.IsNullOrEmpty(activityId))
                {
                    select = "select * from t_ff_rt_taskinstance where processinstance_id=@1 order by step_number,end_time";
                    selectParms = new SqlParameter[]{ 
        				SqlServerHelper.NewSqlParameter("@1", SqlDbType.VarChar, 50, processInstanceId)
        		    };
                }
                else
                {
                    select = "select * from t_ff_rt_taskinstance where processinstance_id=@1 and activity_id=@2 order by step_number,end_time";
                    selectParms = new SqlParameter[]{  
        				SqlServerHelper.NewSqlParameter("@1", SqlDbType.VarChar, 50, processInstanceId), 
				        SqlServerHelper.NewSqlParameter("@2", SqlDbType.VarChar, 200, activityId)
        		    };
                }
                SqlDataReader reader = null;
                try
                {
                    reader = SqlServerHelper.ExecuteReader(connection, CommandType.Text, select, selectParms);
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            ITaskInstance info = SqlDataReaderToInfo.GetTaskInstance(reader);
                            infos.Add(info);
                        }
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            return infos;
        }

        /// <summary>
        /// 查询出同一个stepNumber的所有TaskInstance实例
        /// </summary>
        public IList<ITaskInstance> FindTaskInstancesForProcessInstanceByStepNumber(String processInstanceId, Int32 stepNumber)
        {
            IList<ITaskInstance> infos = new List<ITaskInstance>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string select;
                SqlParameter[] selectParms;
                if (stepNumber < 0)
                {
                    select = "select * from t_ff_rt_taskinstance where processinstance_id=@1 order by step_number,end_time";
                    selectParms = new SqlParameter[]{ 
        				SqlServerHelper.NewSqlParameter("@1", SqlDbType.VarChar, 50, processInstanceId)
        		    };
                }
                else
                {
                    select = "select * from t_ff_rt_taskinstance where processinstance_id=@1 and step_number=@2 order by step_number,end_time";
                    selectParms = new SqlParameter[]{  
        				SqlServerHelper.NewSqlParameter("@1", SqlDbType.VarChar, 50, processInstanceId), 
				        SqlServerHelper.NewSqlParameter("@2", SqlDbType.Int,stepNumber)
        		    };
                }
                SqlDataReader reader = null;
                try
                {
                    reader = SqlServerHelper.ExecuteReader(connection, CommandType.Text, select, selectParms);
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            ITaskInstance info = SqlDataReaderToInfo.GetTaskInstance(reader);
                            infos.Add(info);
                        }
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            return infos;
        }

        /// <summary>
        /// 调用数据库自身的机制所定TaskInstance实例。
        /// 该方法主要用于工单的签收操作，在签收之前先锁定与之对应的TaskInstance。
        /// </summary>
        public bool LockTaskInstance(String taskInstanceId)
        {



            if (SqlServerHelper.ExecuteNonQuery(connectionString, CommandType.Text, "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED SELECT * FROM t_ff_rt_taskinstance ROWLOCK WHERE id =@1",
                SqlServerHelper.NewSqlParameter("@1", SqlDbType.VarChar, 50, taskInstanceId)) != 1)
            //if (SqlServerHelper.ExecuteNonQuery(connectionString, CommandType.Text, "select * from t_ff_rt_taskinstance where id=@1 for update",
            //    SqlServerHelper.NewSqlParameter("@1", SqlDbType.VarChar, 50, taskInstanceId)) != 1)
                return false;
            else return true;
        }


        /******************************************************************************/
        /************                                                        **********/
        /************            workItem 相关的持久化方法                    **********/
        /************            Persistence methods for workitem            **********/
        /************                                                        **********/
        /******************************************************************************/

        /// <summary>
        /// 插入或者更新WorkItem
        /// </summary>
        public bool SaveOrUpdateWorkItem(IWorkItem workitem)
        {
            if (String.IsNullOrEmpty(workitem.Id))
            {
                ((WorkItem)workitem).Id = Guid.NewGuid().ToString().Replace("-", ""); ;

                string insert = "INSERT INTO T_FF_RT_WORKITEM (" +
                    "ID, STATE, CREATED_TIME, CLAIMED_TIME, END_TIME, " +
                    "ACTOR_ID, COMMENTS, TASKINSTANCE_ID )VALUES(@1, @2, @3, @4, @5, @6, @7, @8)";
                SqlParameter[] insertParms = { 
    				SqlServerHelper.NewSqlParameter("@1", SqlDbType.VarChar, 50, workitem.Id), 
    				SqlServerHelper.NewSqlParameter("@2", SqlDbType.Int, (int)workitem.State), 
    				SqlServerHelper.NewSqlParameter("@3", SqlDbType.DateTime, 11, workitem.CreatedTime), 
    				SqlServerHelper.NewSqlParameter("@4", SqlDbType.DateTime, 11, workitem.ClaimedTime), 
    				SqlServerHelper.NewSqlParameter("@5", SqlDbType.DateTime, 11, workitem.EndTime), 
    				SqlServerHelper.NewSqlParameter("@6", SqlDbType.VarChar, 50, workitem.ActorId), 
    				SqlServerHelper.NewSqlParameter("@7", SqlDbType.VarChar, 1024, workitem.Comments), 
    				SqlServerHelper.NewSqlParameter("@8", SqlDbType.VarChar, 50, workitem.TaskInstance.Id)
    			};
                if (SqlServerHelper.ExecuteNonQuery(connectionString, CommandType.Text, insert, insertParms) != 1)
                    return false;
                else return true;
            }
            else
            {
                string update = "UPDATE T_FF_RT_WORKITEM SET " +
                    "STATE=@2, CREATED_TIME=@3, CLAIMED_TIME=@4, END_TIME=@5, ACTOR_ID=@6, " +
                    "COMMENTS=@7, TASKINSTANCE_ID=@8" +
                    " WHERE ID=@1";
                SqlParameter[] updateParms = { 
    				SqlServerHelper.NewSqlParameter("@2", SqlDbType.Int, (int)workitem.State), 
    				SqlServerHelper.NewSqlParameter("@3", SqlDbType.DateTime, 11, workitem.CreatedTime), 
    				SqlServerHelper.NewSqlParameter("@4", SqlDbType.DateTime, 11, workitem.ClaimedTime), 
    				SqlServerHelper.NewSqlParameter("@5", SqlDbType.DateTime, 11, workitem.EndTime), 
    				SqlServerHelper.NewSqlParameter("@6", SqlDbType.VarChar, 50, workitem.ActorId), 
    				SqlServerHelper.NewSqlParameter("@7", SqlDbType.VarChar, 1024, workitem.Comments), 
    				SqlServerHelper.NewSqlParameter("@8", SqlDbType.VarChar, 50, workitem.TaskInstance.Id),
    				SqlServerHelper.NewSqlParameter("@1", SqlDbType.VarChar, 50, workitem.Id)
    			};
                if (SqlServerHelper.ExecuteNonQuery(connectionString, CommandType.Text, update, updateParms) != 1)
                    return false;
                else return true;
            }
        }

        /// <summary>
        ///  返回任务实例的所有"活的"WorkItem的数量。
        ///  "活的"WorkItem 是指状态等于INITIALIZED、STARTED或者SUSPENDED的WorkItem。
        /// </summary>
        public Int32 GetAliveWorkItemCountForTaskInstance(String taskInstanceId)
        {
            String select = "select count(*) from t_ff_rt_workitem where taskinstance_id=@1 and (state in (0,1,3))";
            SqlParameter[] selectParms = { 
				SqlServerHelper.NewSqlParameter("@1", SqlDbType.VarChar, 50, taskInstanceId),
		    };
            return SqlServerHelper.ExecuteInt32(this.connectionString, CommandType.Text, select, selectParms);
        }

        /// <summary>
        /// 查询任务实例的所有"已经结束"WorkItem。
        /// 所以必须有关联条件WorkItem.state=IWorkItem.COMPLTED
        /// </summary>
        /// <param name="taskInstanceId">任务实例Id</param>
        public IList<IWorkItem> FindCompletedWorkItemsForTaskInstance(String taskInstanceId)
        {
            IList<IWorkItem> infos = new List<IWorkItem>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string select = "select * from t_ff_rt_workitem where taskinstance_id=@1 and state=" + (int)WorkItemEnum.COMPLETED;
                SqlDataReader reader = null;
                try
                {
                    reader = SqlServerHelper.ExecuteReader(connection, CommandType.Text, select,
                        SqlServerHelper.NewSqlParameter("@1", SqlDbType.VarChar, 50, taskInstanceId)
                        );
                    if (reader != null)
                    {
                        ITaskInstance iTaskInstance = FindTaskInstanceById(taskInstanceId);
                        while (reader.Read())
                        {
                            IWorkItem info = SqlDataReaderToInfo.GetWorkItem(reader);
                            ((WorkItem)info).TaskInstance = iTaskInstance;
                            infos.Add(info);
                        }
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            return infos;
        }

        /// <summary>
        /// 查询某任务实例的所有WorkItem
        /// </summary>
        public IList<IWorkItem> FindWorkItemsForTaskInstance(String taskInstanceId)
        {
            IList<IWorkItem> infos = new List<IWorkItem>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string select = "select * from t_ff_rt_workitem where taskinstance_id=@1";
                SqlDataReader reader = null;
                try
                {
                    reader = SqlServerHelper.ExecuteReader(connection, CommandType.Text, select,
                        SqlServerHelper.NewSqlParameter("@1", SqlDbType.VarChar, 50, taskInstanceId)
                        );
                    if (reader != null)
                    {
                        ITaskInstance iTaskInstance = FindTaskInstanceById(taskInstanceId);
                        while (reader.Read())
                        {
                            IWorkItem info = SqlDataReaderToInfo.GetWorkItem(reader);
                            ((WorkItem)info).TaskInstance = iTaskInstance;
                            infos.Add(info);
                        }
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            return infos;
        }

        /// <summary>
        /// 删除处于初始化状态的workitem。
        /// 此方法用于签收Workitem时，删除其他Actor的WorkItem
        /// </summary>
        public bool DeleteWorkItemsInInitializedState(String taskInstanceId)
        {
            String delete = "delete from t_ff_rt_workitem where taskinstance_id=@1 and  state=" + (int)WorkItemEnum.INITIALIZED;
            SqlParameter[] deleteParms = { 
                 SqlServerHelper.NewSqlParameter("@1", SqlDbType.VarChar, 50, taskInstanceId)
			};
            if (SqlServerHelper.ExecuteNonQuery(connectionString, CommandType.Text, delete, deleteParms) <= 0)
                return false;
            else return true;
        }

        /// <summary>
        /// Find workItem by id
        /// (Engine没有引用到该方法，提供给业务系统使用，20090303)
        /// </summary>
        public IWorkItem FindWorkItemById(String id)
        {
            WorkItem workItem;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                String select = " select * from t_ff_rt_workitem where id=@1 ";
                SqlDataReader reader = null;
                try
                {
                    reader = SqlServerHelper.ExecuteReader(connection, CommandType.Text, select,
                        SqlServerHelper.NewSqlParameter("@1", SqlDbType.VarChar, 50, id)
                        );
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            workItem = SqlDataReaderToInfo.GetWorkItem(reader);
                            ITaskInstance iTaskInstance = FindTaskInstanceById(workItem.TaskInstanceId);
                            if (iTaskInstance != null)
                            {
                                workItem.TaskInstance = iTaskInstance;
                            }
                            return workItem;
                        }
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            return null;
        }

        /// <summary>
        /// Find all workitems for task
        /// (Engine没有引用到该方法，提供给业务系统使用，20090303)
        /// </summary>
        public IList<IWorkItem> FindWorkItemsForTask(String taskid)
        {
            IList<IWorkItem> infos = new List<IWorkItem>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string select = "select a.*,b.* from t_ff_rt_workitem a,t_ff_rt_taskinstance b where a.taskinstance_id=b.id and b.task_id=@1";
                SqlDataReader reader = null;
                try
                {
                    reader = SqlServerHelper.ExecuteReader(connection, CommandType.Text, select,
                        SqlServerHelper.NewSqlParameter("@1", SqlDbType.VarChar, 300, taskid)
                        );
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            IWorkItem info = SqlDataReaderToInfo.GetWorkItem(reader);
                            ITaskInstance iTaskInstance = SqlDataReaderToInfo.GetTaskInstance(reader); //FindTaskInstanceById(((WorkItem)info).TaskInstanceId);
                            ((WorkItem)info).TaskInstance = iTaskInstance;
                            infos.Add(info);
                        }
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            return infos;
        }

        /// <summary>
        /// 根据操作员的Id返回其待办工单。如果actorId==null，则返回系统所有的待办任务
        /// 待办工单是指状态等于INITIALIZED或STARTED工单
        /// (Engine没有引用到该方法，提供给业务系统使用，20090303)
        /// </summary>
        public IList<IWorkItem> FindTodoWorkItems(String actorId)
        {
            return FindTodoWorkItems(actorId, String.Empty);
        }

        /// <summary>
        /// 查找操作员在某个流程实例中的待办工单。
        /// 如果processInstanceId为空，则等价于调用findTodoWorkItems(String actorId)
        /// 待办工单是指状态等于INITIALIZED或STARTED工单
        /// (Engine没有引用到该方法，提供给业务系统使用，20090303)
        /// </summary>
        public IList<IWorkItem> FindTodoWorkItems(String actorId, String processInstanceId)
        {
            QueryField queryField = new QueryField();
            queryField.Add(new QueryFieldInfo("actor_id", CSharpType.String, actorId));
            queryField.Add(new QueryFieldInfo("a.taskinstance_id", CSharpType.String, processInstanceId));
            QueryInfo queryInfo = SqlServerHelper.GetFormatQuery(queryField);

            IList<IWorkItem> infos = new List<IWorkItem>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string select = string.Format("select a.*,b.* from t_ff_rt_workitem a,t_ff_rt_taskinstance b where a.taskinstance_id=b.id and a.state in ({0},{1}){2} ORDER BY A.CREATED_TIME DESC",
                    (int)WorkItemEnum.INITIALIZED, (int)WorkItemEnum.RUNNING, queryInfo.QueryStringAnd);

                SqlDataReader reader = null;
                try
                {
                    reader = SqlServerHelper.ExecuteReader(connection, CommandType.Text, select, ((List<SqlParameter>)queryInfo.ListQueryParameters).ToArray());
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            IWorkItem info = SqlDataReaderToInfo.GetWorkItem(reader);
                            ITaskInstance iTaskInstance = SqlDataReaderToInfo.GetTaskInstance(reader); //FindTaskInstanceById(((WorkItem)info).TaskInstanceId);
                            ((WorkItem)info).TaskInstance = iTaskInstance;
                            infos.Add(info);
                        }
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            return infos;
        }

        /// <summary>
        /// 查找操作员在某个流程某个任务上的待办工单。
        /// actorId，processId，taskId都可以为空（null或者""）,为空的条件将被忽略
        /// 待办工单是指状态等于INITIALIZED或STARTED工单
        /// (Engine没有引用到该方法，提供给业务系统使用，20090303)
        /// </summary>
        public IList<IWorkItem> FindTodoWorkItems(String actorId, String processId, String taskId)
        {
            QueryField queryField = new QueryField();
            queryField.Add(new QueryFieldInfo("actor_id", CSharpType.String, actorId));
            queryField.Add(new QueryFieldInfo("process_id", CSharpType.String, processId));
            queryField.Add(new QueryFieldInfo("task_id", CSharpType.String, taskId));
            QueryInfo queryInfo = SqlServerHelper.GetFormatQuery(queryField);

            IList<IWorkItem> infos = new List<IWorkItem>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string select = String.Format(
                    "select a.*,b.* from t_ff_rt_workitem a,t_ff_rt_taskinstance b where a.taskinstance_id=b.id and a.state in ({0},{1}){2}",
                    (int)WorkItemEnum.INITIALIZED,
                    (int)WorkItemEnum.RUNNING,
                    queryInfo.QueryStringAnd);

                SqlDataReader reader = null;
                try
                {
                    reader = SqlServerHelper.ExecuteReader(connection, CommandType.Text, select, ((List<SqlParameter>)queryInfo.ListQueryParameters).ToArray());
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            IWorkItem info = SqlDataReaderToInfo.GetWorkItem(reader);
                            ITaskInstance iTaskInstance = SqlDataReaderToInfo.GetTaskInstance(reader);//FindTaskInstanceById(((WorkItem)info).TaskInstanceId);
                            ((WorkItem)info).TaskInstance = iTaskInstance;
                            infos.Add(info);
                        }
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            return infos;
        }

        /// <summary>
        /// 根据操作员的Id返回其已办工单。如果actorId==null，则返回系统所有的已办任务
        /// 已办工单是指状态等于COMPLETED或CANCELED的工单
        /// (Engine没有引用到该方法，提供给业务系统使用，20090303)
        /// </summary>
        public IList<IWorkItem> FindHaveDoneWorkItems(String actorId)
        {
            return FindHaveDoneWorkItems(actorId, string.Empty);
        }

        /// <summary>
        /// 查找操作员在某个流程实例中的已办工单。
        /// 如果processInstanceId为空，则等价于调用findHaveDoneWorkItems(String actorId)
        /// 已办工单是指状态等于COMPLETED或CANCELED的工单
        /// (Engine没有引用到该方法，提供给业务系统使用，20090303)
        /// </summary>
        public IList<IWorkItem> FindHaveDoneWorkItems(String actorId, String processInstanceId)
        {
            QueryField queryField = new QueryField();
            queryField.Add(new QueryFieldInfo("actor_id", CSharpType.String, actorId));
            queryField.Add(new QueryFieldInfo("processinstance_id", CSharpType.String, processInstanceId));
            QueryInfo queryInfo = SqlServerHelper.GetFormatQuery(queryField);

            IList<IWorkItem> infos = new List<IWorkItem>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                String select = String.Format(
                    "select a.*,b.* from t_ff_rt_workitem a,t_ff_rt_taskinstance b where a.taskinstance_id=b.id and a.state in ({0},{1}){2}",
                    (int)WorkItemEnum.COMPLETED,
                    (int)WorkItemEnum.CANCELED,
                    queryInfo.QueryStringAnd);
                SqlDataReader reader = null;
                try
                {
                    reader = SqlServerHelper.ExecuteReader(connection, CommandType.Text, select, ((List<SqlParameter>)queryInfo.ListQueryParameters).ToArray());
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            IWorkItem info = SqlDataReaderToInfo.GetWorkItem(reader);
                            ITaskInstance iTaskInstance = SqlDataReaderToInfo.GetTaskInstance(reader); //FindTaskInstanceById(((WorkItem)info).TaskInstanceId);
                            ((WorkItem)info).TaskInstance = iTaskInstance;
                            infos.Add(info);
                        }
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            return infos;
        }

        /// <summary>
        /// 查找操作员在某个流程某个任务上的已办工单。
        /// actorId，processId，taskId都可以为空（null或者""）,为空的条件将被忽略
        ///  已办工单是指状态等于COMPLETED或CANCELED的工单
        ///  (Engine没有引用到该方法，提供给业务系统使用，20090303)
        /// </summary>
        public IList<IWorkItem> FindHaveDoneWorkItems(String actorId, String processId, String taskId)
        {
            QueryField queryField = new QueryField();
            queryField.Add(new QueryFieldInfo("actor_id", CSharpType.String, actorId));
            queryField.Add(new QueryFieldInfo("process_id", CSharpType.String, processId));
            queryField.Add(new QueryFieldInfo("task_id", CSharpType.String, taskId));
            QueryInfo queryInfo = SqlServerHelper.GetFormatQuery(queryField);

            IList<IWorkItem> infos = new List<IWorkItem>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                String select = String.Format(
                    "select a.*,b.* from t_ff_rt_workitem a,t_ff_rt_taskinstance b where a.taskinstance_id=b.id and a.state in ({0},{1}){2}",
                    (int)WorkItemEnum.COMPLETED,
                    (int)WorkItemEnum.CANCELED,
                    queryInfo.QueryStringAnd);

                SqlDataReader reader = null;
                try
                {
                    reader = SqlServerHelper.ExecuteReader(connection, CommandType.Text, select, ((List<SqlParameter>)queryInfo.ListQueryParameters).ToArray());
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            IWorkItem info = SqlDataReaderToInfo.GetWorkItem(reader);
                            ITaskInstance iTaskInstance = SqlDataReaderToInfo.GetTaskInstance(reader); //FindTaskInstanceById(((WorkItem)info).TaskInstanceId);
                            ((WorkItem)info).TaskInstance = iTaskInstance;
                            infos.Add(info);
                        }
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            return infos;
        }

        /******************************************************************************/
        /************                                                        **********/
        /************            token 相关的持久化方法                       **********/
        /************            Persistence methods for token               **********/
        /************                                                        **********/
        /******************************************************************************/


        public bool SaveOrUpdateToken(IToken token)
        {
            if (String.IsNullOrEmpty(token.Id))
            {
                ((Token)token).Id = Guid.NewGuid().ToString().Replace("-", "");
                string insert = "INSERT INTO T_FF_RT_TOKEN (" +
                    "ID, ALIVE, VALUE, NODE_ID, PROCESSINSTANCE_ID, " +
                    "STEP_NUMBER, FROM_ACTIVITY_ID )VALUES(@1, @2, @3, @4, @5, @6, @7)";
                SqlParameter[] insertParms = { 
    				SqlServerHelper.NewSqlParameter("@1", SqlDbType.VarChar, 50, token.Id), 
    				SqlServerHelper.NewSqlParameter("@2", SqlDbType.SmallInt, SqlServerHelper.OraBit(token.IsAlive)), 
    				SqlServerHelper.NewSqlParameter("@3", SqlDbType.Int, token.Value), 
    				SqlServerHelper.NewSqlParameter("@4", SqlDbType.VarChar, 200, token.NodeId), 
    				SqlServerHelper.NewSqlParameter("@5", SqlDbType.VarChar, 50, token.ProcessInstanceId), 
    				SqlServerHelper.NewSqlParameter("@6", SqlDbType.Int, token.StepNumber), 
    				SqlServerHelper.NewSqlParameter("@7", SqlDbType.VarChar, 100, token.FromActivityId)
    			};
                if (SqlServerHelper.ExecuteNonQuery(connectionString, CommandType.Text, insert, insertParms) != 1)
                    return false;
                else return true;
            }
            else
            {
                string update = "UPDATE T_FF_RT_TOKEN SET " +
                    "ALIVE=@2, VALUE=@3, NODE_ID=@4, PROCESSINSTANCE_ID=@5, STEP_NUMBER=@6, " +
                    "FROM_ACTIVITY_ID=@7" +
                    " WHERE ID=@1";
                SqlParameter[] updateParms = { 
					SqlServerHelper.NewSqlParameter("@2", SqlDbType.SmallInt, SqlServerHelper.OraBit(token.IsAlive)), 
					SqlServerHelper.NewSqlParameter("@3", SqlDbType.Int, token.Value), 
					SqlServerHelper.NewSqlParameter("@4", SqlDbType.VarChar, 200, token.NodeId), 
					SqlServerHelper.NewSqlParameter("@5", SqlDbType.VarChar, 50, token.ProcessInstanceId), 
					SqlServerHelper.NewSqlParameter("@6", SqlDbType.Int, token.StepNumber), 
					SqlServerHelper.NewSqlParameter("@7", SqlDbType.VarChar, 100, token.FromActivityId),
					SqlServerHelper.NewSqlParameter("@1", SqlDbType.VarChar, 50, token.Id)
				};
                if (SqlServerHelper.ExecuteNonQuery(connectionString, CommandType.Text, update, updateParms) != 1)
                    return false;
                else return true;
            }
        }

        /// <summary>
        /// 统计流程任意节点的活动Token的数量。对于Activity节点，该数量只能取值1或者0，大于1表明有流程实例出现异常。
        /// </summary>
        public int GetAliveTokenCountForNode(String processInstanceId, String nodeId)
        {
            String select = String.Format("select count(*) from T_FF_RT_TOKEN where alive=1 and processinstance_id=@1 and node_id =@2",
                ProcessInstanceEnum.INITIALIZED, ProcessInstanceEnum.RUNNING);
            SqlParameter[] selectParms = { 
				SqlServerHelper.NewSqlParameter("@1", SqlDbType.VarChar, 50, processInstanceId), 
				SqlServerHelper.NewSqlParameter("@2", SqlDbType.VarChar, 200, nodeId)
		    };
            return SqlServerHelper.ExecuteInt32(this.connectionString, CommandType.Text, select, selectParms);
        }

        /// <summary>
        /// (Engine没有引用到该方法，提供给业务系统使用，20090303)
        /// </summary>
        public IToken FindTokenById(String id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                String select = "select * from t_ff_rt_token where id=@1";
                SqlDataReader reader = null;
                try
                {
                    reader = SqlServerHelper.ExecuteReader(connection, CommandType.Text, select,
                        SqlServerHelper.NewSqlParameter("@1", SqlDbType.VarChar, 50, id)
                        );
                    while (reader.Read())
                    {
                        IToken info = SqlDataReaderToInfo.GetToken(reader);
                        return info;
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            return null;
        }

        /// <summary>
        /// Find all the tokens for process instance ,and the nodeId of the token must equals to the second argument.
        /// </summary>
        /// <param name="processInstanceId">the id of the process instance</param>
        /// <param name="nodeId">if the nodeId is null ,then return all the tokens of the process instance.</param>
        /// <returns></returns>
        public IList<IToken> FindTokensForProcessInstance(String processInstanceId, String nodeId)
        {
            if (String.IsNullOrEmpty(processInstanceId)) return null;
            QueryField queryField = new QueryField();
            queryField.Add(new QueryFieldInfo("processinstance_id", CSharpType.String, processInstanceId));
            queryField.Add(new QueryFieldInfo("node_id", CSharpType.String, nodeId));
            QueryInfo queryInfo = SqlServerHelper.GetFormatQuery(queryField);

            IList<IToken> infos = new List<IToken>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string select = String.Format("select * from t_ff_rt_token {0}", queryInfo.QueryStringWhere);
                SqlDataReader reader = null;
                try
                {
                    reader = SqlServerHelper.ExecuteReader(connection, CommandType.Text, select, ((List<SqlParameter>)queryInfo.ListQueryParameters).ToArray());
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            IToken info = SqlDataReaderToInfo.GetToken(reader);
                            infos.Add(info);
                        }
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            return infos;
        }

        /// <summary>
        /// 删除某个节点的所有token
        /// </summary>
        public bool DeleteTokensForNode(String processInstanceId, String nodeId)
        {
            String delete = "delete from t_ff_rt_token where processinstance_id = @1 and node_id=@2 ";
            SqlParameter[] deleteParms = { 
				SqlServerHelper.NewSqlParameter("@1", SqlDbType.VarChar, 50, processInstanceId),
				SqlServerHelper.NewSqlParameter("@2", SqlDbType.VarChar, 200, nodeId)
			};
            if (SqlServerHelper.ExecuteNonQuery(connectionString, CommandType.Text, delete, deleteParms) != 1)
                return false;
            else return true;
        }

        /// <summary>
        /// 删除某些节点的所有token
        /// </summary>
        public bool DeleteTokensForNodes(String processInstanceId, IList<String> nodeIdsList)
        {
            SqlTransaction transaction = SqlServerHelper.GetSqlTransaction(connectionString);
            try
            {
                String delete = "delete from t_ff_rt_token where processinstance_id = @1 and node_id=@2";
                SqlParameter[] deleteParms = { 
    				SqlServerHelper.NewSqlParameter("@1", SqlDbType.VarChar, 50, processInstanceId),
    				SqlServerHelper.NewSqlParameter("@2", SqlDbType.VarChar, 200, "")
    			};
                foreach (String item in nodeIdsList)
                {
                    deleteParms[1].Value = item;
                    SqlServerHelper.ExecuteNonQuery(transaction, CommandType.Text, delete, deleteParms);
                }
                transaction.Commit();
                return true;
            }
            catch
            {
                transaction.Rollback();
                return false;
            }
            finally
            {
                if (transaction.Connection.State != ConnectionState.Closed)
                {
                    transaction.Connection.Close();
                    transaction.Dispose();
                }
            }
        }

        /// <summary>
        /// 删除token
        /// </summary>
        public bool DeleteToken(IToken token)
        {
            string delete = "delete from t_ff_rt_token where id=@1";
            SqlParameter[] deleteParms = { 
				SqlServerHelper.NewSqlParameter("@1", SqlDbType.VarChar, 50, token.Id)
			};
            if (SqlServerHelper.ExecuteNonQuery(connectionString, CommandType.Text, delete, deleteParms) <= 0)
                return false;
            else return true;
        }

        /******************************************************************************/
        /************                                                        **********/
        /************            存取流程定义文件 相关的持久化方法             **********/
        /************            Persistence methods for workflow definition **********/
        /************                                                        **********/
        /******************************************************************************/

        /// <summary>
        /// Save or update the workflow definition. The version will be increased automatically when insert a new record.
        /// 保存流程定义，如果同一个ProcessId的流程定义已经存在，则版本号自动加1。
        /// </summary>
        public bool SaveOrUpdateWorkflowDefinition(IWorkflowDefinition workflowDef)
        {

            if (String.IsNullOrEmpty(workflowDef.Id))
            {
                Int32 latestVersion = FindTheLatestVersionNumberIgnoreState(workflowDef.ProcessId);
                if (latestVersion > 0)
                {
                    workflowDef.Version = latestVersion + 1;
                }
                else
                {
                    workflowDef.Version = 1;
                }
                workflowDef.Id = Guid.NewGuid().ToString("N");
                string insert = "INSERT INTO T_FF_DF_WORKFLOWDEF (" +
                    "ID, DEFINITION_TYPE, PROCESS_ID, NAME, DISPLAY_NAME, " +
                    "DESCRIPTION, VERSION, STATE, UPLOAD_USER, UPLOAD_TIME, " +
                    "PUBLISH_USER, PUBLISH_TIME, PROCESS_CONTENT )VALUES(@1, @2, @3, @4, @5, @6, @7, @8, @9, @10, @11, @12, @13)";

                SqlParameter[] insertParms = { 
					SqlServerHelper.NewSqlParameter("@1", SqlDbType.VarChar, 50, workflowDef.Id), 
					SqlServerHelper.NewSqlParameter("@2", SqlDbType.VarChar, 50, workflowDef.DefinitionType), 
					SqlServerHelper.NewSqlParameter("@3", SqlDbType.VarChar, 100, workflowDef.ProcessId), 
					SqlServerHelper.NewSqlParameter("@4", SqlDbType.VarChar, 100, workflowDef.Name), 
					SqlServerHelper.NewSqlParameter("@5", SqlDbType.VarChar, 128, workflowDef.DisplayName), 
					SqlServerHelper.NewSqlParameter("@6", SqlDbType.VarChar, 1024, workflowDef.Description), 
					SqlServerHelper.NewSqlParameter("@7", SqlDbType.Int, workflowDef.Version), 
					SqlServerHelper.NewSqlParameter("@8", SqlDbType.SmallInt, SqlServerHelper.OraBit(workflowDef.State) ), 
					SqlServerHelper.NewSqlParameter("@9", SqlDbType.VarChar, 50, workflowDef.UploadUser), 
					SqlServerHelper.NewSqlParameter("@10", SqlDbType.DateTime, 11, workflowDef.UploadTime), 
					SqlServerHelper.NewSqlParameter("@11", SqlDbType.VarChar, 50, workflowDef.PublishUser), 
					SqlServerHelper.NewSqlParameter("@12", SqlDbType.DateTime, 11, workflowDef.PublishTime), 
					SqlServerHelper.NewSqlParameter("@13", SqlDbType.NText, workflowDef.ProcessContent)
				};
                if (SqlServerHelper.ExecuteNonQuery(connectionString, CommandType.Text, insert, insertParms) != 1)
                    return false;
                else return true;
            }
            else
            {
                string update = "UPDATE T_FF_DF_WORKFLOWDEF SET " +
                    "PROCESS_ID=@3, NAME=@4, DISPLAY_NAME=@5, DESCRIPTION=@6, " +
                    "STATE=@8, UPLOAD_USER=@9, UPLOAD_TIME=@10, PROCESS_CONTENT=@13 " +
                    "WHERE ID=@1";
                SqlParameter[] updateParms = { 
    				SqlServerHelper.NewSqlParameter("@3", SqlDbType.VarChar, 100, workflowDef.ProcessId), 
    				SqlServerHelper.NewSqlParameter("@4", SqlDbType.VarChar, 100, workflowDef.Name), 
    				SqlServerHelper.NewSqlParameter("@5", SqlDbType.VarChar, 128, workflowDef.DisplayName), 
    				SqlServerHelper.NewSqlParameter("@6", SqlDbType.VarChar, 1024, workflowDef.Description), 
    				SqlServerHelper.NewSqlParameter("@8", SqlDbType.SmallInt, SqlServerHelper.OraBit(workflowDef.State)), 
    				SqlServerHelper.NewSqlParameter("@9", SqlDbType.VarChar, 50, workflowDef.UploadUser), 
    				SqlServerHelper.NewSqlParameter("@10", SqlDbType.DateTime, 11, workflowDef.UploadTime),
    				SqlServerHelper.NewSqlParameter("@13", SqlDbType.Text,workflowDef.ProcessContent),
    				SqlServerHelper.NewSqlParameter("@1", SqlDbType.VarChar, 50, workflowDef.Id)
    			};
                if (SqlServerHelper.ExecuteNonQuery(connectionString, CommandType.Text, update, updateParms) != 1)
                    return false;
                else return true;
            }
        }

        /// <summary>
        /// Find the workflow definition by id .
        /// 根据纪录的ID返回流程定义
        /// </summary>
        public IWorkflowDefinition FindWorkflowDefinitionById(String id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                String select = "select * from t_ff_df_workflowdef where id=@1";
                SqlDataReader reader = null;
                try
                {
                    reader = SqlServerHelper.ExecuteReader(connection, CommandType.Text, select,
                        SqlServerHelper.NewSqlParameter("@1", SqlDbType.VarChar, 50, id)
                        );
                    while (reader.Read())
                    {
                        WorkflowDefinition info = SqlDataReaderToInfo.GetWorkflowDefinition(reader);
                        return info;
                    }
                }
                catch
                {
                    throw;
                }
                finally
                {
                    reader.Close();
                }
            }
            return null;
        }

        /// <summary>
        /// Find workflow definition by workflow process id and version<br>
        /// 根据ProcessId和版本号返回流程定义
        /// </summary>
        public IWorkflowDefinition FindWorkflowDefinitionByProcessIdAndVersionNumber(String processId, int version)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                String select = "select * from t_ff_df_workflowdef where process_id=@1 and version=@2";
                SqlDataReader reader = null;
                try
                {
                    reader = SqlServerHelper.ExecuteReader(connection, CommandType.Text, select,
                        SqlServerHelper.NewSqlParameter("@1", SqlDbType.VarChar, 100, processId),
                        SqlServerHelper.NewSqlParameter("@2", SqlDbType.Int, version)
                        );
                    while (reader.Read())
                    {
                        WorkflowDefinition info = SqlDataReaderToInfo.GetWorkflowDefinition(reader);
                        return info;
                    }
                }
                catch
                {
                    throw;
                }
                finally
                {
                    reader.Close();
                }
            }
            return null;
        }

        /// <summary>
        /// Find the latest version of the workflow definition.
        /// 根据processId返回最新版本的有效流程定义
        /// </summary>
        /// <param name="processId">the workflow process id </param>
        /// <returns></returns>
        public IWorkflowDefinition FindTheLatestVersionOfWorkflowDefinitionByProcessId(String processId)
        {
            Int32 latestVersion = this.FindTheLatestVersionNumber(processId);
            return this.FindWorkflowDefinitionByProcessIdAndVersionNumber(processId, latestVersion);
        }

        /// <summary>
        /// Find all the workflow definitions for the workflow process id.
        /// 根据ProcessId 返回所有版本的流程定义
        /// </summary>
        public IList<IWorkflowDefinition> FindWorkflowDefinitionsByProcessId(String processId)
        {
            IList<IWorkflowDefinition> infos = new List<IWorkflowDefinition>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string select = "select * from t_ff_df_workflowdef where process_id=@1";
                SqlDataReader reader = null;
                try
                {
                    reader = SqlServerHelper.ExecuteReader(connection, CommandType.Text, select,
                        SqlServerHelper.NewSqlParameter("@1", SqlDbType.VarChar, 100, processId)
                        );
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            WorkflowDefinition info = SqlDataReaderToInfo.GetWorkflowDefinition(reader);
                            infos.Add(info);
                        }
                    }
                }
                catch
                {
                    throw;
                }
                finally
                {
                    reader.Close();
                }
            }
            return infos;
        }

        /// <summary>
        /// Find all of the latest version of workflow definitions.
        /// 返回系统中所有的最新版本的有效流程定义
        /// </summary>
        public IList<IWorkflowDefinition> FindAllTheLatestVersionsOfWorkflowDefinition()
        {
            IList<IWorkflowDefinition> infos = new List<IWorkflowDefinition>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string select = "SELECT * FROM T_FF_DF_WORKFLOWDEF a " +
                    "where version=(select max(version) from T_FF_DF_WORKFLOWDEF b where a.PROCESS_ID=b.PROCESS_ID) ";
                SqlDataReader reader = null;
                try
                {
                    reader = SqlServerHelper.ExecuteReader(connection, CommandType.Text, select, null);
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            WorkflowDefinition info = SqlDataReaderToInfo.GetWorkflowDefinition(reader);
                            infos.Add(info);
                        }
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            return infos;
        }

        /// <summary>
        /// Find the latest version number
        /// 返回最新的有效版本号
        /// </summary>
        /// <returns>the version number ,null if there is no workflow definition stored in the DB.</returns>
        public int FindTheLatestVersionNumber(String processId)
        {
            String select = "select max(version) from t_ff_df_workflowdef where process_id=@1 and state=1";
            SqlParameter[] selectParms = { 
				SqlServerHelper.NewSqlParameter("@1", SqlDbType.VarChar, 100, processId)
		    };
            return SqlServerHelper.ExecuteInt32(this.connectionString, CommandType.Text, select, selectParms);
        }

        /// <summary>
        /// 返回最新版本号,
        /// </summary>
        public int FindTheLatestVersionNumberIgnoreState(String processId)
        {
            String select = "select max(version) from t_ff_df_workflowdef where process_id=@1";
            SqlParameter[] selectParms = { 
				SqlServerHelper.NewSqlParameter("@1", SqlDbType.VarChar, 100, processId)
		    };
            return SqlServerHelper.ExecuteInt32(this.connectionString, CommandType.Text, select, selectParms);
        }



        /********************************process instance trace info **********************/
        public bool SaveOrUpdateProcessInstanceTrace(IProcessInstanceTrace processInstanceTrace)
        {
            if (String.IsNullOrEmpty(processInstanceTrace.Id))
            {
                processInstanceTrace.Id = Guid.NewGuid().ToString().Replace("-", "");
                string insert = "INSERT INTO T_FF_HIST_TRACE (" +
                    "ID, PROCESSINSTANCE_ID, STEP_NUMBER, MINOR_NUMBER, TYPE, " +
                    "EDGE_ID, FROM_NODE_ID, TO_NODE_ID )VALUES(@1, @2, @3, @4, @5, @6, @7, @8)";
                SqlParameter[] insertParms = { 
				SqlServerHelper.NewSqlParameter("@1", SqlDbType.VarChar, 50, processInstanceTrace.Id), 
				SqlServerHelper.NewSqlParameter("@2", SqlDbType.VarChar, 50, processInstanceTrace.ProcessInstanceId), 
				SqlServerHelper.NewSqlParameter("@3", SqlDbType.Int, processInstanceTrace.StepNumber), 
				SqlServerHelper.NewSqlParameter("@4", SqlDbType.Int, processInstanceTrace.MinorNumber), 
				SqlServerHelper.NewSqlParameter("@5", SqlDbType.VarChar, 15, processInstanceTrace.Type.ToString()), 
				SqlServerHelper.NewSqlParameter("@6", SqlDbType.VarChar, 100, processInstanceTrace.EdgeId), 
				SqlServerHelper.NewSqlParameter("@7", SqlDbType.VarChar, 100, processInstanceTrace.FromNodeId), 
				SqlServerHelper.NewSqlParameter("@8", SqlDbType.VarChar, 100, processInstanceTrace.ToNodeId)
			};
                if (SqlServerHelper.ExecuteNonQuery(connectionString, CommandType.Text, insert, insertParms) != 1)
                    return false;
                else return true;
            }
            else
            {
                string update = "UPDATE T_FF_HIST_TRACE SET " +
                    "PROCESSINSTANCE_ID=@2, STEP_NUMBER=@3, MINOR_NUMBER=@4, TYPE=@5, EDGE_ID=@6, " +
                    "FROM_NODE_ID=@7, TO_NODE_ID=@8" +
                    " WHERE ID=@1";
                SqlParameter[] updateParms = { 
				SqlServerHelper.NewSqlParameter("@2", SqlDbType.VarChar, 50, processInstanceTrace.ProcessInstanceId), 
				SqlServerHelper.NewSqlParameter("@3", SqlDbType.Int, processInstanceTrace.StepNumber), 
				SqlServerHelper.NewSqlParameter("@4", SqlDbType.Int, processInstanceTrace.MinorNumber), 
				SqlServerHelper.NewSqlParameter("@5", SqlDbType.VarChar, 15, processInstanceTrace.Type), 
				SqlServerHelper.NewSqlParameter("@6", SqlDbType.VarChar, 100, processInstanceTrace.EdgeId), 
				SqlServerHelper.NewSqlParameter("@7", SqlDbType.VarChar, 100, processInstanceTrace.FromNodeId), 
				SqlServerHelper.NewSqlParameter("@8", SqlDbType.VarChar, 100, processInstanceTrace.ToNodeId),
				SqlServerHelper.NewSqlParameter("@1", SqlDbType.VarChar, 50, processInstanceTrace.Id)
			};
                if (SqlServerHelper.ExecuteNonQuery(connectionString, CommandType.Text, update, updateParms) != 1)
                    return false;
                else return true;
            }
        }


        /********************************process instance trace info **********************/

        /// <summary>
        /// 根据流程实例ID查找流程实例运行轨迹
        /// </summary>
        /// <param name="processInstanceId">流程实例ID</param>
        /// <returns></returns>
        public IList<IProcessInstanceTrace> FindProcessInstanceTraces(String processInstanceId)
        {
            IList<IProcessInstanceTrace> infos = new List<IProcessInstanceTrace>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string select = "select * from t_ff_hist_trace where processinstance_id=@1 order by step_number,minor_number";
                SqlDataReader reader = null;
                try
                {
                    reader = SqlServerHelper.ExecuteReader(connection, CommandType.Text, select,
                        SqlServerHelper.NewSqlParameter("@1", SqlDbType.VarChar, 50, processInstanceId)
                        );
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            ProcessInstanceTrace info = SqlDataReaderToInfo.GetProcessInstanceTrace(reader);
                            infos.Add(info);
                        }
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            return infos;
        }


        public IList<IProcessInstanceVar> FindProcessInstanceVariable(string processInstanceId)
        {
            IList<IProcessInstanceVar> infos = new List<IProcessInstanceVar>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string select = "select * from t_ff_rt_procinst_var where processinstance_id=@1";
                SqlDataReader reader = null;
                try
                {
                    reader = SqlServerHelper.ExecuteReader(connection, CommandType.Text, select,
                        SqlServerHelper.NewSqlParameter("@1", SqlDbType.VarChar, 50, processInstanceId)
                        );
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            ProcessInstanceVar info = SqlDataReaderToInfo.GetProcessInstanceVar(reader);
                            infos.Add(info);
                        }
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            return infos;
        }

        public IProcessInstanceVar FindProcessInstanceVariable(string processInstanceId, string name)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                String select = "select * from t_ff_rt_procinst_var where processinstance_id=@1 and name=@2";
                SqlDataReader reader = null;
                try
                {
                    reader = SqlServerHelper.ExecuteReader(connection, CommandType.Text, select,
                        SqlServerHelper.NewSqlParameter("@1", SqlDbType.VarChar, 50, processInstanceId),
                        SqlServerHelper.NewSqlParameter("@2", SqlDbType.VarChar, 255, name)
                        );
                    while (reader.Read())
                    {
                        ProcessInstanceVar info = SqlDataReaderToInfo.GetProcessInstanceVar(reader);
                        return info;
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            return null;
        }

        public bool UpdateProcessInstanceVariable(IProcessInstanceVar var)
        {
            string update = "UPDATE T_FF_RT_PROCINST_VAR SET " +
                "VALUE=@2" +
                " WHERE PROCESSINSTANCE_ID=@1 AND NAME=@3";
            SqlParameter[] updateParms = { 
				SqlServerHelper.NewSqlParameter("@2", SqlDbType.VarChar, 255, var.ValueType+"#"+var.Value), 
				SqlServerHelper.NewSqlParameter("@1", SqlDbType.VarChar, 50, var.ProcessInstanceId),
				SqlServerHelper.NewSqlParameter("@3", SqlDbType.VarChar, 255, var.Name)
			};
            if (SqlServerHelper.ExecuteNonQuery(connectionString, CommandType.Text, update, updateParms) != 1)
                return false;
            else return true;
        }

        public bool SaveProcessInstanceVariable(IProcessInstanceVar var)
        {
            string insert = "INSERT INTO T_FF_RT_PROCINST_VAR (" +
                   "PROCESSINSTANCE_ID, VALUE, NAME )VALUES(@1, @2, @3)";
            SqlParameter[] insertParms = { 
				SqlServerHelper.NewSqlParameter("@1", SqlDbType.VarChar, 50, var.ProcessInstanceId), 
				SqlServerHelper.NewSqlParameter("@2", SqlDbType.VarChar, 255, var.ValueType+"#"+var.Value), 
				SqlServerHelper.NewSqlParameter("@3", SqlDbType.VarChar, 255, var.Name)
			};
            if (SqlServerHelper.ExecuteNonQuery(connectionString, CommandType.Text, insert, insertParms) != 1)
                return false;
            else return true;
        }

        /******************************** lifw555@gmail.com **********************/

        /// <summary>
        /// 获得操作员所要操作工单的总数量
        /// publishUser如果为null，获取全部
        /// </summary>
        /// <param name="actorId">操作员主键</param>
        /// <param name="publishUser">流程定义发布者</param>
        /// <returns></returns>
        public Int32 GetTodoWorkItemsCount(String actorId, String publishUser)
        {
            return 0;
        }

        /// <summary>
        /// 获得操作员所要操作工单列表（分页）
        /// publishUser如果为null，获取全部
        /// </summary>
        /// <param name="actorId">操作员主键</param>
        /// <param name="publishUser">流程定义发布者</param>
        /// <param name="pageSize">每页显示的条数</param>
        /// <param name="pageNumber">当前页数</param>
        /// <returns></returns>
        public IList<IWorkItem> FindTodoWorkItems(String actorId, String publishUser, int pageSize, int pageNumber)
        {
            return null;
        }

        /// <summary>
        /// 获得操作员完成的工单总数量
        /// publishUser如果为null，获取全部
        /// </summary>
        /// <param name="actorId">操作员主键</param>
        /// <param name="publishUser">流程定义发布者</param>
        /// <returns></returns>
        public Int32 GetHaveDoneWorkItemsCount(String actorId, String publishUser)
        {
            return 0;
        }

        /// <summary>
        /// 获得操作员完成的工单列表（分页）
        /// publishUser如果为null，获取全部
        /// </summary>
        /// <param name="actorId">操作员主键</param>
        /// <param name="publishUser">流程定义发布者</param>
        /// <param name="pageSize">每页显示的条数</param>
        /// <param name="pageNumber">当前页数</param>
        /// <returns></returns>
        public IList<IWorkItem> FindHaveDoneWorkItems(String actorId, String publishUser, int pageSize, int pageNumber)
        {
            return null;
        }



        /// <summary>
        /// 获得工作流发布者发起的所有流程定义的工作流实例总数量
        /// </summary>
        /// <param name="publishUser">工作流发布者</param>
        /// <returns></returns>
        public Int32 GetProcessInstanceCountByPublishUser(String publishUser)
        {
            return 0;
        }



    }
}
