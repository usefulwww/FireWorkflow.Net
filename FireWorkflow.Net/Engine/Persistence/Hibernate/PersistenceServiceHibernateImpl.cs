using System;
using System.Collections.Generic;
using System.Text;
using FireWorkflow.Net.Engine;
using FireWorkflow.Net.Engine.Impl;
using FireWorkflow.Net.Engine.Definition;
using FireWorkflow.Net.Kernel;

namespace FireWorkflow.Net.Engine.Persistence.Hibernate
{
    /**
 * The hibernate implementation of persistence service
 * 
 * @author 非也,nychen2000@163.com
 * 
 */
    public class PersistenceServiceHibernateImpl : IPersistenceService
    {

        #region IRuntimeContextAware 成员
        protected RuntimeContext rtCtx = null;
        public void setRuntimeContext(RuntimeContext ctx)
        {
            this.rtCtx = ctx;
        }

        public RuntimeContext getRuntimeContext()
        {
            return this.rtCtx;
        }

        #endregion

        /******************************************************************************/
        /************                                                        **********/
        /************            Process instance 相关的持久化方法            **********/
        /************            Persistence methods for process instance    **********/
        /************                                                        **********/
        /******************************************************************************/
        /**
         * 插入或者更新ProcessInstance 。<br/>
         * Save or update processinstance. 
         * If the processInstance.id is null then insert a new process instance record
         * and genarate a new id for it (save operation)
         * otherwise update the existent one.
         * 
         * @param processInstance
         */
        public void saveOrUpdateProcessInstance(IProcessInstance processInstance) {
           // this.getHibernateTemplate().saveOrUpdate(processInstance);
        }

        //    public void saveProcessInstance(IProcessInstance processInstance){ throw new NotImplementedException(); }
        //    
        //    public void updateProcessInstance(IProcessInstance processInstance){ throw new NotImplementedException(); }

        /**
         * 通过ID获得“活的”ProcessInstance对象。<br>
         * “活的”是指ProcessInstance.state=INITIALIZED Or ProcessInstance.state=STARTED Or ProcessInstance=SUSPENDED的流程实例
         * @param id processInstance.id
         * @return process instance
         */
        public IProcessInstance findAliveProcessInstanceById(String id) { throw new NotImplementedException(); }


        /**
         * 通过ID获得ProcessInstance对象。
         * (Engine没有引用到该方法，提供给业务系统使用，20090303)
         * @param id processInstance.id
         * @return process instance
         */
        public IProcessInstance findProcessInstanceById(String id) { throw new NotImplementedException(); }


        /**
         * 查找并返回同一个业务流程的所有实例
         * (Engine没有引用到该方法，提供给业务系统使用，20090303)
         * @param processId The id of the process definition.
         * @return A list of processInstance
         */
        public List<IProcessInstance> findProcessInstancesByProcessId(String processId) { throw new NotImplementedException(); }


        /**
         * 查找并返回同一个指定版本业务流程的所有实例
         * (Engine没有引用到该方法，提供给业务系统使用，20090303)
         * @param processId The id of the process definition.
         * @return A list of processInstance
         */
        public List<IProcessInstance> findProcessInstancesByProcessIdAndVersion(String processId, int version) { throw new NotImplementedException(); }

        /**
         * 计算活动的子流程实例的数量
         * @param taskInstanceId 父TaskInstance的Id
         * @return
         */
        public int getAliveProcessInstanceCountForParentTaskInstance(String taskInstanceId) { throw new NotImplementedException(); }


        /**
         * 终止流程实例。将流程实例、活动的TaskInstance、活动的WorkItem的状态设置为CANCELED；并删除所有的token
         * @param processInstanceId
         */
        public void abortProcessInstance(ProcessInstance processInstance) { throw new NotImplementedException(); }

        /**
         * 挂起流程实例
         * @param processInstance
         */
        public void suspendProcessInstance(ProcessInstance processInstance) { throw new NotImplementedException(); }

        /**
         * 恢复流程实例
         * @param processInstance
         */
        public void restoreProcessInstance(ProcessInstance processInstance) { throw new NotImplementedException(); }





        /******************************************************************************/
        /************                                                        **********/
        /************            task instance 相关的持久化方法               **********/
        /************            Persistence methods for task instance       **********/
        /************                                                        **********/
        /******************************************************************************/
        /**
         * 插入或者更新TaskInstance。<br/>
         * Save or update task instance. If the taskInstance.id is null then insert a new task instance record
         * and generate a new id for it { throw new NotImplementedException(); }
         * otherwise update the existent one. 
         * @param taskInstance
         */
        public void saveOrUpdateTaskInstance(ITaskInstance taskInstance) { throw new NotImplementedException(); }

        /**
         * 终止TaskInstance。将任务实例及其所有的“活的”WorkItem变成Canceled状态。<br/>
         * "活的"WorkItem 是指状态等于INITIALIZED、STARTED或者SUSPENDED的WorkItem.
         * @param taskInstanceId
         */
        public void abortTaskInstance(TaskInstance taskInstance) { throw new NotImplementedException(); }

        /**
         * 返回“活的”TaskInstance。<br/>
         * “活的”是指TaskInstance.state=INITIALIZED Or TaskInstance.state=STARTED 。
         * @param id
         * @return
         */
        public ITaskInstance findAliveTaskInstanceById(String id) { throw new NotImplementedException(); }

        /**
         * 获得activity的“活的”TaskInstance的数量<br/>
         * “活的”是指TaskInstance.state=INITIALIZED Or TaskInstance.state=STARTED 。
         * @param processInstanceId
         * @param activityId
         * @return
         */
        public int getAliveTaskInstanceCountForActivity(String processInstanceId, String activityId) { throw new NotImplementedException(); }

        /**
         * 返回某个Task已经结束的TaskInstance的数量。<br/>
         * “已经结束”是指TaskInstance.state=COMPLETED。
         * @param processInstanceId
         * @param taskId
         * @return
         */
        public int getCompletedTaskInstanceCountForTask(String processInstanceId, String taskId) { throw new NotImplementedException(); }


        /**
         * Find the task instance by id
         * (Engine没有引用到该方法，提供给业务系统使用，20090303)
         * @param id
         * @return
         */
        public ITaskInstance findTaskInstanceById(String id) { throw new NotImplementedException(); }

        /**
         * 查询流程实例的所有的TaskInstance,如果activityId不为空，则返回该流程实例下指定环节的TaskInstance<br/>
         * (Engine没有引用到该方法，提供给业务系统使用，20090303)
         * @param processInstanceId  the id of the process instance
         * @param activityId  if the activityId is null, then return all the taskinstance of the processinstance{ throw new NotImplementedException(); }
         * @return
         */
        public List<ITaskInstance> findTaskInstancesForProcessInstance(String processInstanceId, String activityId) { throw new NotImplementedException(); }


        /**
         * 查询出同一个stepNumber的所有TaskInstance实例
         * @param processInstanceId
         * @param stepNumber
         * @return
         */
        public List<ITaskInstance> findTaskInstancesForProcessInstanceByStepNumber(String processInstanceId, Int32 stepNumber) { throw new NotImplementedException(); }


        /**
         * 调用数据库自身的机制所定TaskInstance实例。<br/>
         * 该方法主要用于工单的签收操作，在签收之前先锁定与之对应的TaskInstance。
         * @param taskInstanceId
         * @return
         */
        public void lockTaskInstance(String taskInstanceId) { throw new NotImplementedException(); }


        /******************************************************************************/
        /************                                                        **********/
        /************            workItem 相关的持久化方法                    **********/
        /************            Persistence methods for workitem            **********/
        /************                                                        **********/
        /******************************************************************************/
        /**
         * 插入或者更新WorkItem<br/>
         * save or update workitem
         * @param workitem
         */
        public void saveOrUpdateWorkItem(IWorkItem workitem) { throw new NotImplementedException(); }




        /**
         * 返回任务实例的所有"活的"WorkItem的数量。<br>
         * "活的"WorkItem 是指状态等于INITIALIZED、STARTED或者SUSPENDED的WorkItem。
         * @param taskInstanceId
         * @return
         */
        public Int32 getAliveWorkItemCountForTaskInstance(String taskInstanceId) { throw new NotImplementedException(); }

        /**
         * 查询任务实例的所有"已经结束"WorkItem。<br>
         * 
         * 所以必须有关联条件WorkItem.state=IWorkItem.COMPLTED 
         *
         * @param taskInstanceId 任务实例Id
         * @return
         */
        public List<IWorkItem> findCompletedWorkItemsForTaskInstance(String taskInstanceId) { throw new NotImplementedException(); }

        /**
         * 查询某任务实例的所有WorkItem
         * @param taskInstanceId
         * @return
         */
        public List<IWorkItem> findWorkItemsForTaskInstance(String taskInstanceId) { throw new NotImplementedException(); }


        /**
         * 删除处于初始化状态的workitem。
         * 此方法用于签收Workitem时，删除其他Actor的WorkItem
         * @param taskInstanceId
         */
        public void deleteWorkItemsInInitializedState(String taskInstanceId) { throw new NotImplementedException(); }


        /**
         * Find workItem by id
         * (Engine没有引用到该方法，提供给业务系统使用，20090303)
         * @param id
         * @return
         */
        public IWorkItem findWorkItemById(String id) { throw new NotImplementedException(); }


        /**
         *
         * Find all workitems for task
         * (Engine没有引用到该方法，提供给业务系统使用，20090303)
         * @param taskid
         * @return
         */
        public List<IWorkItem> findWorkItemsForTask(String taskid) { throw new NotImplementedException(); }


        /**
         * 根据操作员的Id返回其待办工单。如果actorId==null，则返回系统所有的待办任务<br/>
         * 待办工单是指状态等于INITIALIZED或STARTED工单<br/>
         * (Engine没有引用到该方法，提供给业务系统使用，20090303)
         * @param actorId
         * @return
         */
        public List<IWorkItem> findTodoWorkItems(String actorId) { throw new NotImplementedException(); }

        /**
         * 查找操作员在某个流程实例中的待办工单。
         * 如果processInstanceId为空，则等价于调用findTodoWorkItems(String actorId)
         * 待办工单是指状态等于INITIALIZED或STARTED工单<br/>
         * (Engine没有引用到该方法，提供给业务系统使用，20090303)
         * @param actorId
         * @param processInstanceId
         * @return
         */
        public List<IWorkItem> findTodoWorkItems(String actorId, String processInstanceId) { throw new NotImplementedException(); }

        /**
         * 查找操作员在某个流程某个任务上的待办工单。
         * actorId，processId，taskId都可以为空（null或者""）,为空的条件将被忽略
         * 待办工单是指状态等于INITIALIZED或STARTED工单<br/>
         * (Engine没有引用到该方法，提供给业务系统使用，20090303)
         * @param actorId
         * @param processId
         * @param taskId
         * @return
         */
        public List<IWorkItem> findTodoWorkItems(String actorId, String processId, String taskId) { throw new NotImplementedException(); }

        /**
         * 根据操作员的Id返回其已办工单。如果actorId==null，则返回系统所有的已办任务
         * 已办工单是指状态等于COMPLETED或CANCELED的工单<br/>
         * (Engine没有引用到该方法，提供给业务系统使用，20090303)
         * @param actorId
         * @return
         */
        public List<IWorkItem> findHaveDoneWorkItems(String actorId) { throw new NotImplementedException(); }

        /**
         * 查找操作员在某个流程实例中的已办工单。
         * 如果processInstanceId为空，则等价于调用findHaveDoneWorkItems(String actorId)
         * 已办工单是指状态等于COMPLETED或CANCELED的工单<br/>
         * (Engine没有引用到该方法，提供给业务系统使用，20090303)
         * @param actorId
         * @param processInstanceId
         * @return
         */
        public List<IWorkItem> findHaveDoneWorkItems(String actorId, String processInstanceId) { throw new NotImplementedException(); }

        /**
         * 查找操作员在某个流程某个任务上的已办工单。
         * actorId，processId，taskId都可以为空（null或者""）,为空的条件将被忽略
         * 已办工单是指状态等于COMPLETED或CANCELED的工单<br/>
         * (Engine没有引用到该方法，提供给业务系统使用，20090303)
         * @param actorId
         * @param processId
         * @param taskId
         * @return
         */
        public List<IWorkItem> findHaveDoneWorkItems(String actorId, String processId, String taskId) { throw new NotImplementedException(); }



        /*************************Persistence methods for joinpoint*********************/
        /**
         * Save joinpoint
         *
         * @param joinPoint
         */
        //    public void saveOrUpdateJoinPoint(IJoinPoint joinPoint){ throw new NotImplementedException(); }

        /**
         * Find the joinpoint id
         * (Engine没有引用到该方法，提供给业务系统使用，20090303)
         * @param id
         * @return
         */
        //    public IJoinPoint findJoinPointById(String id){ throw new NotImplementedException(); }

        /**
         * Find all the joinpoint of the process instance, and the synchronizerId of the joinpoint must equals to the seconds argument.
         * @param processInstanceId
         * @param synchronizerId if the synchronizerId is null ,then all the joinpoint of the process instance will be returned.
         * @return
         */
        //    public List<IJoinPoint> findJoinPointsForProcessInstance(String processInstanceId, String synchronizerId){ throw new NotImplementedException(); }


        /******************************************************************************/
        /************                                                        **********/
        /************            token 相关的持久化方法                       **********/
        /************            Persistence methods for token               **********/
        /************                                                        **********/
        /******************************************************************************/
        /**
         * Save token
         * @param token
         */
        public void saveOrUpdateToken(IToken token) { throw new NotImplementedException(); }

        /**
         * 统计流程任意节点的活动Token的数量。对于Activity节点，该数量只能取值1或者0，大于1表明有流程实例出现异常。
         * @param processInstanceId
         * @param nodeId
         * @return
         */
        public int getAliveTokenCountForNode(String processInstanceId, String nodeId) { throw new NotImplementedException(); }

        /**
         * 查找到状态为Dead的token
         * @param id
         * @return
         */
        //    public IToken findDeadTokenById(String id){ throw new NotImplementedException(); }

        /**
         * (Engine没有引用到该方法，提供给业务系统使用，20090303)
         * @param id
         * @return
         */
        public IToken findTokenById(String id) { throw new NotImplementedException(); }

        /**
         * Find all the tokens for process instance ,and the nodeId of the token must equals to the second argument.
         * @param processInstanceId the id of the process instance
         * @param nodeId if the nodeId is null ,then return all the tokens of the process instance.
         * @return
         */
        public List<IToken> findTokensForProcessInstance(String processInstanceId, String nodeId) { throw new NotImplementedException(); }

        /**
         * 删除某个节点的所有token
         * @param processInstanceId
         * @param nodeId
         */
        public void deleteTokensForNode(String processInstanceId, String nodeId) { throw new NotImplementedException(); }

        /**
         * 删除某些节点的所有token
         * @param processInstanceId
         * @param nodeIdsList
         */
        public void deleteTokensForNodes(String processInstanceId, List<String> nodeIdsList) { throw new NotImplementedException(); }

        /**
         * 删除token
         * @param token
         */
        public void deleteToken(IToken token) { throw new NotImplementedException(); }

        /******************************************************************************/
        /************                                                        **********/
        /************            存取流程定义文件 相关的持久化方法             **********/
        /************            Persistence methods for workflow definition **********/
        /************                                                        **********/
        /******************************************************************************/
        /**
         * Save or update the workflow definition. The version will be increased automatically when insert a new record.<br>
         * 保存流程定义，如果同一个ProcessId的流程定义已经存在，则版本号自动加1。
         * @param workflowDef
         */
        public void saveOrUpdateWorkflowDefinition(WorkflowDefinition workflowDef) { throw new NotImplementedException(); }

        /**
         * Find the workflow definition by id .
         * 根据纪录的ID返回流程定义
         * @param id
         * @return
         */
        public WorkflowDefinition findWorkflowDefinitionById(String id) { throw new NotImplementedException(); }

        /**
         * Find workflow definition by workflow process id and version<br>
         * 根据ProcessId和版本号返回流程定义
         * @param processId
         * @param version
         * @return
         */
        public WorkflowDefinition findWorkflowDefinitionByProcessIdAndVersionNumber(String processId, int version) { throw new NotImplementedException(); }

        /**
         * Find the latest version of the workflow definition.<br>
         * 根据processId返回最新版本的有效流程定义
         * @param processId the workflow process id 
         * @return
         */
        public WorkflowDefinition findTheLatestVersionOfWorkflowDefinitionByProcessId(String processId) { throw new NotImplementedException(); }

        /**
         * Find all the workflow definitions for the workflow process id.<br>
         * 根据ProcessId 返回所有版本的流程定义
         * @param processId
         * @return
         */
        public List<WorkflowDefinition> findWorkflowDefinitionsByProcessId(String processId) { throw new NotImplementedException(); }

        /**
         * Find all of the latest version of workflow definitions.<br>
         * 返回系统中所有的最新版本的有效流程定义
         * @return
         */
        public List<WorkflowDefinition> findAllTheLatestVersionsOfWorkflowDefinition() { throw new NotImplementedException(); }

        /**
         * Find the latest version number <br>
         * 返回最新的有效版本号
         * @param processId
         * @return the version number ,null if there is no workflow definition stored in the DB.
         */
        public int findTheLatestVersionNumber(String processId) { throw new NotImplementedException(); }

        /**
         * 返回最新版本号,
         * @param processId
         * @return
         */
        public int findTheLatestVersionNumberIgnoreState(String processId) { throw new NotImplementedException(); }



        /********************************process instance trace info **********************/
        public void saveOrUpdateProcessInstanceTrace(ProcessInstanceTrace processInstanceTrace) { throw new NotImplementedException(); }
        public List<String> findProcessInstanceTraces(String processInstanceId) { throw new NotImplementedException(); }






        //    protected RuntimeContext rtCtx = null;

        //    public void setRuntimeContext(RuntimeContext ctx) {
        //        this.rtCtx = ctx;
        //    }

        //    public RuntimeContext getRuntimeContext() {
        //        return this.rtCtx;
        //    }

        //    /**
        //     * Save processInstance
        //     * @param processInstance
        //     */
        //    public void saveOrUpdateProcessInstance(IProcessInstance processInstance) {

        //        this.getHibernateTemplate().saveOrUpdate(processInstance);
        //    }



        //    /**
        //     * Save joinpoint
        //     * @param joinPoint
        //     */
        ////    public void saveOrUpdateJoinPoint(IJoinPoint joinPoint) {
        ////        this.getHibernateTemplate().saveOrUpdate(joinPoint);
        ////    }

        //    /* (non-Javadoc)
        //     * @see org.fireflow.engine.persistence.IPersistenceService#saveTaskInstance(org.fireflow.engine.ITaskInstance)
        //     */
        //    public void saveOrUpdateTaskInstance(ITaskInstance taskInstance) {
        //        this.getHibernateTemplate().saveOrUpdate(taskInstance);
        //    }
        ////
        //    /* (non-Javadoc)
        //     * @see org.fireflow.engine.persistence.IPersistenceService#saveWorkItem(org.fireflow.engine.IWorkItem)
        //     */

        //    public void saveOrUpdateWorkItem(IWorkItem workitem) {
        //        this.getHibernateTemplate().saveOrUpdate(workitem);
        //    }

        //    public void saveOrUpdateToken(IToken token) {
        //        this.getHibernateTemplate().saveOrUpdate(token);
        //    }

        ////    public List<IJoinPoint> findJoinPointsForProcessInstance(final String processInstanceId, final String synchronizerId) {
        ////        List result = (List) this.getHibernateTemplate().execute(new HibernateCallback() {
        ////
        ////            public Object doInHibernate(Session arg0) throws HibernateException, SQLException {
        ////                Criteria criteria = arg0.createCriteria(JoinPoint.class);
        ////                criteria.add(Expression.eq("processInstanceId", processInstanceId));
        ////                if (synchronizerId != null && !synchronizerId.trim().Equals("")) {
        ////                    criteria.add(Expression.eq("synchronizerId", synchronizerId));
        ////                }
        ////                return criteria.list();
        ////            }
        ////        });
        ////
        ////        return result;
        ////
        ////    }
        //    public Int32 getAliveTokenCountForNode(final String processInstanceId, final String nodeId) {
        //        Int32 result = (Int32) this.getHibernateTemplate().execute(new HibernateCallback() {

        //            public Object doInHibernate(Session arg0) throws HibernateException, SQLException {

        //                Criteria criteria = arg0.createCriteria(Token.class);

        //                criteria.add(Expression.eq("processInstanceId", processInstanceId));

        //                criteria.add(Expression.eq("nodeId", nodeId));

        //                criteria.add(Expression.eq("alive", java.lang.Boolean.TRUE));

        //                ProjectionList prolist = Projections.projectionList();
        //                prolist.add(Projections.rowCount());
        //                criteria.setProjection(prolist);

        //                return criteria.uniqueResult();
        //            }
        //        });
        //        return result;
        //    }

        //    public Int32 getCompletedTaskInstanceCountForTask(final String processInstanceId, final String taskId) {
        //        Int32 result = (Int32) this.getHibernateTemplate().execute(new HibernateCallback() {

        //            public Object doInHibernate(Session arg0) throws HibernateException, SQLException {

        //                Criteria criteria = arg0.createCriteria(TaskInstance.class);
        //                criteria.add(Expression.eq("taskId", taskId.trim()));
        //                criteria.add(Expression.eq("processInstanceId", processInstanceId));

        //                Criterion cri2 = Expression.eq("state", new Int32(ITaskInstance.COMPLETED));

        //                criteria.add(cri2);

        //                ProjectionList prolist = Projections.projectionList();
        //                prolist.add(Projections.rowCount());
        //                criteria.setProjection(prolist);

        //                return criteria.uniqueResult();
        //            }
        //        });
        //        return result;
        //    }

        //    public Int32 getAliveTaskInstanceCountForActivity(final String processInstanceId, final String activityId) {
        //        Int32 result = (Int32) this.getHibernateTemplate().execute(new HibernateCallback() {

        //            public Object doInHibernate(Session arg0) throws HibernateException, SQLException {

        //                Criteria criteria = arg0.createCriteria(TaskInstance.class);
        //                criteria.add(Expression.eq("processInstanceId", processInstanceId.trim()));

        //                criteria.add(Expression.eq("activityId", activityId.trim()));

        //                Criterion cri1 = Expression.eq("state", new Int32(ITaskInstance.INITIALIZED));
        //                Criterion cri2 = Expression.eq("state", new Int32(ITaskInstance.RUNNING));
        ////                Criterion cri3 = Expression.eq("state", new Int32(ITaskInstance.SUSPENDED));
        //                Criterion cri_or = Expression.or(cri1, cri2);
        ////                Criterion cri_or = Expression.or(cri_tmp, cri3);

        //                criteria.add(cri_or);

        //                ProjectionList prolist = Projections.projectionList();
        //                prolist.add(Projections.rowCount());
        //                criteria.setProjection(prolist);

        //                return criteria.uniqueResult();
        //            }
        //        });
        //        return result;
        //    }

        //    /**
        //     * 获得同一个Token的所有状态为Initialized的TaskInstance
        //     * @param tokenId
        //     * @return
        //     */
        //    public List<ITaskInstance> findInitializedTaskInstancesListForToken(final String processInstanceId, final String tokenId) {
        //        List result = (List) this.getHibernateTemplate().execute(new HibernateCallback() {

        //            public Object doInHibernate(Session arg0) throws HibernateException, SQLException {

        //                Criteria criteria = arg0.createCriteria(TaskInstance.class);
        //                criteria.add(Expression.eq("processInstanceId", processInstanceId));

        //                criteria.add(Expression.eq("tokenId", tokenId.trim()));

        //                criteria.add(Expression.eq("state", new Int32(0)));

        //                return (List<ITaskInstance>) criteria.list();
        //            }
        //        });
        //        return result;
        //    }

        //    public List<ITaskInstance> findTaskInstancesForProcessInstance(final java.lang.String processInstanceId,
        //            final String activityId) {
        //        List result = (List) this.getHibernateTemplate().execute(new HibernateCallback() {

        //            public Object doInHibernate(Session arg0) throws HibernateException, SQLException {

        //                Criteria criteria = arg0.createCriteria(TaskInstance.class);
        //                criteria.add(Expression.eq("processInstanceId", processInstanceId.trim()));
        //                if (activityId != null && !activityId.trim().Equals("")) {
        //                    criteria.add(Expression.eq("activityId", activityId.trim()));
        //                }
        //                return (List<ITaskInstance>) criteria.list();
        //            }
        //        });
        //        return result;
        //    }

        //    public List<ITaskInstance> findTaskInstancesForProcessInstanceByStepNumber(final String processInstanceId, final Int32 stepNumber) {
        //        List result = (List) this.getHibernateTemplate().execute(new HibernateCallback() {

        //            public Object doInHibernate(Session arg0) throws HibernateException, SQLException {

        //                Criteria criteria = arg0.createCriteria(TaskInstance.class);
        //                criteria.add(Expression.eq("processInstanceId", processInstanceId.trim()));

        //                if (stepNumber != null) {
        //                    criteria.add(Expression.eq("stepNumber", stepNumber));
        //                }
        //                return (List<ITaskInstance>) criteria.list();
        //            }
        //        });
        //        return result;
        //    }
        //    public void lockTaskInstance(String taskInstanceId){
        //        this.getHibernateTemplate().get(TaskInstance.class, taskInstanceId,LockMode.UPGRADE);
        //    }
        //    /*
        //    public List<ITaskInstance> findTaskInstancesForProcessInstanceByFromActivityId(final String processInstanceId, final String fromActivityId) {
        //    List result = (List) this.getHibernateTemplate().execute(new HibernateCallback() {

        //    public Object doInHibernate(Session arg0) throws HibernateException, SQLException {

        //    Criteria criteria = arg0.createCriteria(TaskInstance.class);
        //    criteria.add(Expression.eq("processInstanceId", processInstanceId.trim()));

        //    if (fromActivityId != null && !fromActivityId.trim().Equals("")) {
        //    criteria.add(Expression.eq("fromActivityId", fromActivityId.trim()));
        //    }
        //    return (List<ITaskInstance>) criteria.list();
        //    }
        //    });
        //    return result;
        //    }
        //     */
        ////    public IToken findDeadTokenById(final String id){
        ////       IToken result = (IToken) this.getHibernateTemplate().execute(new HibernateCallback() {
        ////
        ////            public Object doInHibernate(Session arg0) throws HibernateException, SQLException {
        ////
        ////                Criteria criteria = arg0.createCriteria(Token.class);
        ////
        ////
        ////                criteria.add(Expression.eq("id", id));
        ////
        ////                criteria.add(Expression.eq("alive",java.lang.Boolean.FALSE));
        ////
        ////                return criteria.uniqueResult();
        ////            }
        ////        });
        ////        return result;
        ////    }
        //    public IToken findTokenById(String id) {
        //        return (IToken) this.getHibernateTemplate().get(Token.class, id);
        //    }

        //    public void deleteTokensForNodes(final String processInstanceId, final List nodeIdsList) {
        //        this.getHibernateTemplate().execute(new HibernateCallback() {

        //            public Object doInHibernate(Session arg0) throws HibernateException, SQLException {
        //                String hql = "delete from org.fireflow.kernel.impl.Token  where processInstanceId=:processInstanceId and nodeId in (:nodeId)";
        //                Query query = arg0.createQuery(hql);
        //                query.setString("processInstanceId", processInstanceId);
        //                query.setParameterList("nodeId", nodeIdsList);
        //                return query.executeUpdate();
        //            }
        //        });
        //    }

        //    public void deleteTokensForNode(final String processInstanceId, final String nodeId) {
        //        this.getHibernateTemplate().execute(new HibernateCallback() {

        //            public Object doInHibernate(Session arg0) throws HibernateException, SQLException {
        //                String hql = "delete from org.fireflow.kernel.impl.Token  where processInstanceId=:processInstanceId and nodeId=:nodeId";
        //                Query query = arg0.createQuery(hql);
        //                query.setString("processInstanceId", processInstanceId);
        //                query.setString("nodeId", nodeId);
        //                return query.executeUpdate();
        //            }
        //        });
        //    }

        //    public void deleteToken(IToken token) {
        //        this.getHibernateTemplate().delete(token);
        //    }

        //    /**
        //     * 
        //     */
        //    public List<IToken> findTokensForProcessInstance(final String processInstanceId, final String nodeId) {
        //        List result = (List) this.getHibernateTemplate().execute(new HibernateCallback() {

        //            public Object doInHibernate(Session arg0) throws HibernateException, SQLException {

        //                Criteria criteria = arg0.createCriteria(Token.class);

        //                criteria.add(Expression.eq("processInstanceId", processInstanceId));
        //                if (nodeId != null && !nodeId.trim().Equals("")) {
        //                    criteria.add(Expression.eq("nodeId", nodeId));
        //                }

        //                return (List<IToken>) criteria.list();
        //            }
        //        });
        //        return result;
        //    }

        ////    public void updateWorkItem(IWorkItem workItem) {
        ////        //在hibernate中，update操作无需处理？
        ////        Session session = (Session) RuntimeContext.getInstance().getCurrentDBSession();
        ////
        ////        session.update(workItem);
        ////    }

        ////    public void updateTaskInstance(ITaskInstance taskInstance) {
        ////        //在hibernate中，update操作无需处理？
        ////        Session session = (Session) RuntimeContext.getInstance().getCurrentDBSession();
        ////
        ////        session.update(taskInstance);
        ////
        ////    }
        //    public IWorkItem findWorkItemById(String id) {
        //        return (IWorkItem) this.getHibernateTemplate().get(WorkItem.class, id);
        //    }

        //    public ITaskInstance findAliveTaskInstanceById(final String id) {
        //        ITaskInstance result = (ITaskInstance) this.getHibernateTemplate().execute(new HibernateCallback() {

        //            public Object doInHibernate(Session arg0) throws HibernateException, SQLException {
        //                Criteria criteria = arg0.createCriteria(TaskInstance.class);


        //                Criterion cri1 = Expression.eq("state", new Int32(ITaskInstance.INITIALIZED));
        //                Criterion cri2 = Expression.eq("state", new Int32(ITaskInstance.RUNNING));
        //                Criterion cri_or = Expression.or(cri1, cri2);

        //                Criterion cri0 = Expression.eq("id", id);
        //                Criterion cri_and = Expression.and(cri0, cri_or);
        //                criteria.add(cri_and);

        //                return criteria.uniqueResult();
        //            }
        //        });
        //        return result;
        //    }

        //    public ITaskInstance findTaskInstanceById(String id) {
        //        return (ITaskInstance) this.getHibernateTemplate().get(TaskInstance.class, id);
        //    }


        //    /*
        //    public List<IWorkItem> findWorkItemsForTaskInstance(final String taskInstanceId) {
        //    List result = (List) this.getHibernateTemplate().execute(new HibernateCallback() {

        //    public Object doInHibernate(Session arg0) throws HibernateException, SQLException {
        //    Criteria criteria = arg0.createCriteria(WorkItem.class);
        //    criteria.add(Expression.eq("taskInstance.id", taskInstanceId));
        //    List<IWorkItem> _result = criteria.list();

        //    return _result;
        //    }
        //    });
        //    return result;

        //    }
        //     */
        ////    public List<IWorkItem> findAliveWorkItemsWithoutJoinForTaskInstance(final String taskInstanceId) {
        ////        List result = (List) this.getHibernateTemplate().execute(new HibernateCallback() {
        ////
        ////            public Object doInHibernate(Session arg0) throws HibernateException, SQLException {
        ////                String hql = "From org.fireflow.engine.impl.WorkItem m Where m.taskInstance.id=:taskInstanceId And (m.state=0 Or m.state=1 Or m.state=3)";
        ////                Query query = arg0.createQuery(hql);
        ////
        ////                query.setString("taskInstanceId", taskInstanceId);
        ////
        ////                return query.list();
        ////            }
        ////        });
        ////        System.out.println("===================================");
        ////        return result;
        ////    }
        //    public void abortTaskInstance(final TaskInstance taskInstance) {
        //        this.getHibernateTemplate().execute(new HibernateCallback() {

        //            public Object doInHibernate(Session arg0) throws HibernateException, SQLException {
        //                DateTime now = rtCtx.getCalendarService().getSysDate();
        //                //首先Cancel TaskInstance
        //                taskInstance.setState(ITaskInstance.CANCELED);
        //                taskInstance.setEndTime(now);
        //                taskInstance.setCanBeWithdrawn(Boolean.FALSE);
        //                arg0.update(taskInstance);


        //                String hql = "Update org.fireflow.engine.impl.WorkItem m set m.state=:state ,m.endTime=:endTime Where m.taskInstance.id=:taskInstanceId And (m.state=0 Or m.state=1)";
        //                Query query = arg0.createQuery(hql);
        //                query.setInteger("state", IWorkItem.CANCELED);
        //                query.setDate("endTime", now);
        //                query.setString("taskInstanceId", taskInstance.getId());

        //                query.executeUpdate();

        //                return null;
        //            }
        //        });
        //    }

        //    public Int32 getAliveWorkItemCountForTaskInstance(final String taskInstanceId) {
        //        Object result = (Object) this.getHibernateTemplate().execute(new HibernateCallback() {

        //            public Object doInHibernate(Session arg0) throws HibernateException, SQLException {
        //                String hql = "select count(*) From org.fireflow.engine.impl.WorkItem m Where m.taskInstance.id=:taskInstanceId And (m.state=0 Or m.state=1 Or m.state=3)";
        //                Query query = arg0.createQuery(hql);
        //                query.setString("taskInstanceId", taskInstanceId);

        //                return query.uniqueResult();
        //            }
        //        });
        //        if (result is Int32)return (Int32)result;
        //        else{
        //            return new Int32(((Long)result).intValue());
        //        }
        //    }

        //    public List<IWorkItem> findCompletedWorkItemsForTaskInstance(final String taskInstanceId) {
        //        List result = (List) this.getHibernateTemplate().execute(new HibernateCallback() {

        //            public Object doInHibernate(Session arg0) throws HibernateException, SQLException {
        //                String hql = "From org.fireflow.engine.impl.WorkItem m Where m.taskInstance.id=:taskInstanceId And m.state=:state";
        //                Query query = arg0.createQuery(hql);
        //                query.setString("taskInstanceId", taskInstanceId);
        //                query.setInteger("state", IWorkItem.COMPLETED);
        //                return query.list();
        //            }
        //        });
        ////        System.out.println("===================================");
        //        return result;
        //    }

        //    public List<IWorkItem> findWorkItemsForTaskInstance(final String taskInstanceId){
        //        List result = (List) this.getHibernateTemplate().execute(new HibernateCallback() {

        //            public Object doInHibernate(Session arg0) throws HibernateException, SQLException {
        //                Criteria criteria = arg0.createCriteria(WorkItem.class);
        //                criteria.createAlias("taskInstance", "taskInstance");
        //                criteria.add(Expression.eq("taskInstance.id", taskInstanceId));

        //                return criteria.list();
        //            }
        //        });
        //        return result;    	
        //    }

        //    public List<IWorkItem> findWorkItemsForTask(final String taskid) {
        //        List result = (List) this.getHibernateTemplate().execute(new HibernateCallback() {

        //            public Object doInHibernate(Session arg0) throws HibernateException, SQLException {
        //                Criteria criteria = arg0.createCriteria(WorkItem.class);

        //                criteria.createAlias("taskInstance", "taskInstance");
        //                criteria.add(Expression.eq("taskInstance.taskId", taskid));
        //                List<IWorkItem> _result = criteria.list();

        //                return _result;
        //            }
        //        });
        //        return result;
        //    }

        ////    public List<IToken> findTokens(IProcessInstance processInstance) {
        ////        Session session = (Session) RuntimeContext.getInstance().getCurrentDBSession();
        ////        Criteria criteria = session.createCriteria(Token.class);
        ////
        ////        criteria.add(Expression.eq("processInstance.id", processInstance.getId()));
        ////
        ////        return (List<IToken>) criteria.list();
        ////    }
        //    public List<IProcessInstance> findProcessInstancesByProcessId(final String processId) {
        //        List result = (List) this.getHibernateTemplate().execute(new HibernateCallback() {

        //            public Object doInHibernate(Session arg0) throws HibernateException, SQLException {
        //                Criteria criteria = arg0.createCriteria(ProcessInstance.class);

        //                criteria.add(Expression.eq("processId", processId));

        //                criteria.addOrder(Order.asc("createdTime"));

        //                List<IProcessInstance> _result = criteria.list();

        //                return _result;
        //            }
        //        });
        //        return result;
        //    }

        //    public List<IProcessInstance> findProcessInstancesByProcessIdAndVersion(final String processId, final Int32 version) {
        //        List result = (List) this.getHibernateTemplate().execute(new HibernateCallback() {

        //            public Object doInHibernate(Session arg0) throws HibernateException, SQLException {
        //                Criteria criteria = arg0.createCriteria(ProcessInstance.class);

        //                criteria.add(Expression.eq("processId", processId));
        //                criteria.add(Expression.eq("version", version));
        //                criteria.addOrder(Order.asc("createdTime"));                
        //                List<IProcessInstance> _result = criteria.list();

        //                return _result;
        //            }
        //        });
        //        return result;
        //    }

        //    public IProcessInstance findProcessInstanceById(String id) {
        //        return (IProcessInstance) this.getHibernateTemplate().get(ProcessInstance.class, id);
        //    }

        //    public IProcessInstance findAliveProcessInstanceById(final String id) {
        //        IProcessInstance result = (IProcessInstance) this.getHibernateTemplate().execute(new HibernateCallback() {

        //            public Object doInHibernate(Session arg0) throws HibernateException, SQLException {
        //                Criteria criteria = arg0.createCriteria(ProcessInstance.class);


        //                Criterion cri1 = Expression.eq("state", new Int32(IProcessInstance.INITIALIZED));
        //                Criterion cri2 = Expression.eq("state", new Int32(IProcessInstance.RUNNING));
        ////                Criterion cri3 = Expression.eq("state", new Int32(IProcessInstance.SUSPENDED));
        //                Criterion cri_or = Expression.or(cri1, cri2);
        ////                Criterion cri_or = Expression.or(cri_tmp, cri3);

        //                Criterion cri0 = Expression.eq("id", id);
        //                Criterion cri_and = Expression.and(cri0, cri_or);
        //                criteria.add(cri_and);

        //                return criteria.uniqueResult();
        //            }
        //        });
        //        return result;
        //    }

        ////    public IJoinPoint findJoinPointById(String id) {
        ////        return (IJoinPoint) this.getHibernateTemplate().get(JoinPoint.class, id);
        ////    }
        //    public void saveOrUpdateWorkflowDefinition(WorkflowDefinition workflowDef) {
        //        if (workflowDef.getId() == null || workflowDef.getId().Equals("")) {
        //            Int32 latestVersion = findTheLatestVersionNumberIgnoreState(workflowDef.getProcessId());
        //            if (latestVersion != null) {
        //                workflowDef.setVersion(new Int32(latestVersion.intValue() + 1));
        //            } else {
        //                workflowDef.setVersion(new Int32(1));
        //            }
        //        }
        //        this.getHibernateTemplate().saveOrUpdate(workflowDef);
        //    }

        //    public Int32 findTheLatestVersionNumber(final String processId) {
        //        //取得当前最大的发布状态为有效的version值
        //        Int32 result = (Int32) this.getHibernateTemplate().execute(new HibernateCallback() {

        //            public Object doInHibernate(Session arg0) throws HibernateException, SQLException {
        //                Query q = arg0.createQuery("select max(m.version) from WorkflowDefinition m where m.processId=:processId and m.state=:state");
        //                q.setString("processId", processId);
        //                q.setBoolean("state", Boolean.TRUE);
        //                Object obj = q.uniqueResult();
        //                if (obj != null) {
        //                    Int32 latestVersion = (Int32) obj;
        //                    return latestVersion;
        //                } else {
        //                    return null;
        //                }
        //            }
        //        });
        //        return result;
        //    }

        //    public Int32 findTheLatestVersionNumberIgnoreState(final String processId){
        //        Int32 result = (Int32) this.getHibernateTemplate().execute(new HibernateCallback() {
        //            public Object doInHibernate(Session arg0) throws HibernateException, SQLException {
        //                Query q = arg0.createQuery("select max(m.version) from WorkflowDefinition m where m.processId=:processId ");
        //                q.setString("processId", processId);
        //                Object obj = q.uniqueResult();
        //                if (obj != null) {
        //                    Int32 latestVersion = (Int32) obj;
        //                    return latestVersion;
        //                } else {
        //                    return null;
        //                }
        //            }
        //        });
        //        return result;    	
        //    }

        //    public WorkflowDefinition findWorkflowDefinitionById(String id) {
        //        return (WorkflowDefinition) this.getHibernateTemplate().get(WorkflowDefinition.class, id);
        //    }

        //    public WorkflowDefinition findWorkflowDefinitionByProcessIdAndVersionNumber(final String processId, final int version) {
        //        WorkflowDefinition workflowDef = (WorkflowDefinition) this.getHibernateTemplate().execute(new HibernateCallback() {
        //            public Object doInHibernate(Session arg0) throws HibernateException, SQLException {
        //                Criteria c = arg0.createCriteria(WorkflowDefinition.class);
        //                c.add(Expression.eq("processId", processId));
        //                c.add(Expression.eq("version", version));
        //                return (WorkflowDefinition) c.uniqueResult();
        //            }
        //        });
        //        return workflowDef;
        //    }

        //    public WorkflowDefinition findTheLatestVersionOfWorkflowDefinitionByProcessId(String processId) {
        //        Int32 latestVersion = this.findTheLatestVersionNumber(processId);
        //        return this.findWorkflowDefinitionByProcessIdAndVersionNumber(processId, latestVersion);
        //    }

        //    public List<WorkflowDefinition> findWorkflowDefinitionsByProcessId(final String processId) {
        //        List result = (List) this.getHibernateTemplate().execute(new HibernateCallback() {

        //            public Object doInHibernate(Session arg0) throws HibernateException, SQLException {
        //                Criteria c = arg0.createCriteria(WorkflowDefinition.class);
        //                c.add(Expression.eq("processId", processId));
        //                return c.list();
        //            }
        //        });

        //        return result;
        //    }

        //    public List<WorkflowDefinition> findAllTheLatestVersionsOfWorkflowDefinition() {
        //        List result = (List) this.getHibernateTemplate().execute(new HibernateCallback() {

        //            public Object doInHibernate(Session arg0) throws HibernateException, SQLException {
        //                String hql = "select distinct model.processId from WorkflowDefinition model ";
        //                Query query = arg0.createQuery(hql);
        //                List processIdList = query.list();
        //                List _result = new Vector<WorkflowDefinition>();
        //                for (int i = 0; i < processIdList.Count; i++) {
        //                    WorkflowDefinition wfDef = findTheLatestVersionOfWorkflowDefinitionByProcessId((String) processIdList.get(i));
        //                    _result.add(wfDef);
        //                }
        //                return _result;
        //            }
        //        });
        //        return result;
        //    }

        //    public List<IWorkItem> findTodoWorkItems(final String actorId) {
        //        return findTodoWorkItems(actorId, null);
        //    }

        //    public List<IWorkItem> findTodoWorkItems(final String actorId, final String processInstanceId) {

        //        List result = (List) this.getHibernateTemplate().execute(new HibernateCallback() {

        //            public Object doInHibernate(Session arg0) throws HibernateException, SQLException {
        //                Criteria criteria = arg0.createCriteria(WorkItem.class);

        //                Criterion cri1 = Expression.eq("state", new Int32(IWorkItem.INITIALIZED));
        //                Criterion cri2 = Expression.eq("state", new Int32(IWorkItem.RUNNING));
        //                Criterion cri_or = Expression.or(cri1, cri2);

        //                if (actorId != null && !actorId.trim().Equals("")) {
        //                    Criterion cri0 = Expression.eq("actorId", actorId);
        //                    Criterion cri_and = Expression.and(cri0, cri_or);
        //                    criteria.add(cri_and);
        //                } else {
        //                    criteria.add(cri_or);
        //                }

        //                criteria.createAlias("taskInstance", "taskInstance");
        //                if (processInstanceId != null && !processInstanceId.trim().Equals("")) {
        //                    criteria.add(Expression.eq("taskInstance.processInstanceId", processInstanceId));
        //                }

        //                return criteria.list();
        //            }
        //        });
        //        return result;
        //    }

        //    public List<IWorkItem> findTodoWorkItems(final String actorId, final String processId, final String taskId) {
        //        List result = (List) this.getHibernateTemplate().execute(new HibernateCallback() {

        //            public Object doInHibernate(Session arg0) throws HibernateException, SQLException {
        //                Criteria criteria = arg0.createCriteria(WorkItem.class);


        //                Criterion cri1 = Expression.eq("state", new Int32(IWorkItem.INITIALIZED));
        //                Criterion cri2 = Expression.eq("state", new Int32(IWorkItem.RUNNING));
        //                Criterion cri_or = Expression.or(cri1, cri2);

        //                if (actorId != null && !actorId.trim().Equals("")) {
        //                    Criterion cri0 = Expression.eq("actorId", actorId);
        //                    Criterion cri_and = Expression.and(cri0, cri_or);
        //                    criteria.add(cri_and);
        //                } else {
        //                    criteria.add(cri_or);
        //                }

        //                criteria.createAlias("taskInstance", "taskInstance");
        //                if (processId != null && !processId.trim().Equals("")) {
        //                    criteria.add(Expression.eq("taskInstance.processId", processId));
        //                }

        //                if (taskId != null && !taskId.trim().Equals("")) {
        //                    criteria.add(Expression.eq("taskInstance.taskId", taskId));
        //                }
        //                return criteria.list();


        //            }
        //        });
        //        return result;
        //    }

        //    public List<IWorkItem> findHaveDoneWorkItems(final String actorId) {
        //        return findHaveDoneWorkItems(actorId, null);
        //    }

        //    public List<IWorkItem> findHaveDoneWorkItems(final String actorId, final String processInstanceId) {

        //        List result = (List) this.getHibernateTemplate().execute(new HibernateCallback() {

        //            public Object doInHibernate(Session arg0) throws HibernateException, SQLException {
        //                Criteria criteria = arg0.createCriteria(WorkItem.class);

        //                Criterion cri1 = Expression.eq("state", new Int32(IWorkItem.COMPLETED));
        //                Criterion cri2 = Expression.eq("state", new Int32(IWorkItem.CANCELED));
        //                Criterion cri_or = Expression.or(cri1, cri2);

        //                if (actorId != null && !actorId.trim().Equals("")) {
        //                    Criterion cri0 = Expression.eq("actorId", actorId);
        //                    Criterion cri_and = Expression.and(cri0, cri_or);
        //                    criteria.add(cri_and);
        //                } else {
        //                    criteria.add(cri_or);
        //                }

        //                criteria.createAlias("taskInstance", "taskInstance");
        //                if (processInstanceId != null && !processInstanceId.trim().Equals("")) {
        //                    criteria.add(Expression.eq("taskInstance.processInstanceId", processInstanceId));
        //                }

        //                return criteria.list();
        //            }
        //        });
        //        return result;
        //    }

        //    public List<IWorkItem> findHaveDoneWorkItems(final String actorId, final String processId, final String taskId) {
        //        List result = (List) this.getHibernateTemplate().execute(new HibernateCallback() {

        //            public Object doInHibernate(Session arg0) throws HibernateException, SQLException {
        //                Criteria criteria = arg0.createCriteria(WorkItem.class);


        //                Criterion cri1 = Expression.eq("state", new Int32(IWorkItem.COMPLETED));
        //                Criterion cri2 = Expression.eq("state", new Int32(IWorkItem.CANCELED));
        //                Criterion cri_or = Expression.or(cri1, cri2);

        //                if (actorId != null && !actorId.trim().Equals("")) {
        //                    Criterion cri0 = Expression.eq("actorId", actorId);
        //                    Criterion cri_and = Expression.and(cri0, cri_or);
        //                    criteria.add(cri_and);
        //                } else {
        //                    criteria.add(cri_or);
        //                }

        //                criteria.createAlias("taskInstance", "taskInstance");
        //                if (processId != null && !processId.trim().Equals("")) {
        //                    criteria.add(Expression.eq("taskInstance.processId", processId));
        //                }

        //                if (taskId != null && !taskId.trim().Equals("")) {
        //                    criteria.add(Expression.eq("taskInstance.taskId", taskId));
        //                }
        //                return criteria.list();


        //            }
        //        });
        //        return result;
        //    }

        //    public void deleteWorkItemsInInitializedState(final String taskInstanceId) {
        //        this.getHibernateTemplate().execute(new HibernateCallback() {

        //            public Object doInHibernate(Session arg0) throws HibernateException, SQLException {
        //                String hql = "delete from org.fireflow.engine.impl.WorkItem  where taskInstance.id=? and state=0";
        //                Query query = arg0.createQuery(hql);
        //                query.setString(0, taskInstanceId);
        //                return query.executeUpdate();
        //            }
        //        });
        //    }

        //    public Int32 getAliveProcessInstanceCountForParentTaskInstance(final String taskInstanceId) {
        //        Int32 result = (Int32) this.getHibernateTemplate().execute(new HibernateCallback() {

        //            public Object doInHibernate(Session arg0) throws HibernateException, SQLException {
        //                Criteria criteria = arg0.createCriteria(ProcessInstance.class);
        //                criteria.add(Expression.eq("parentTaskInstanceId", taskInstanceId));

        //                Criterion cri1 = Expression.eq("state", new Int32(IProcessInstance.INITIALIZED));
        //                Criterion cri2 = Expression.eq("state", new Int32(IProcessInstance.RUNNING));
        ////                Criterion cri3 = Expression.eq("state", new Int32(IProcessInstance.SUSPENDED));
        //                Criterion cri_or = Expression.or(cri1, cri2);
        ////                Criterion cri_or = Expression.or(cri_tmp, cri3);

        //                criteria.add(cri_or);

        //                ProjectionList prolist = Projections.projectionList();
        //                prolist.add(Projections.rowCount());
        //                criteria.setProjection(prolist);

        //                return criteria.uniqueResult();
        //            }
        //        });
        //        return result;
        //    }

        //    public void suspendProcessInstance(final ProcessInstance processInstance) {
        //        this.getHibernateTemplate().execute(new HibernateCallback() {

        //            public Object doInHibernate(Session arg0) throws HibernateException, SQLException {
        //                processInstance.setSuspended(Boolean.TRUE);
        //                arg0.update(processInstance);

        //                String hql1 = "Update org.fireflow.engine.impl.TaskInstance m Set m.suspended=:suspended Where m.processInstanceId=:processInstanceId And (m.state=0 Or m.state=1)";
        //                Query query1 = arg0.createQuery(hql1);
        //                query1.setBoolean("suspended", Boolean.TRUE);
        //                query1.setString("processInstanceId", processInstance.getId());
        //                query1.executeUpdate();

        //                return null;
        //            }
        //        });
        //    }

        //    public void restoreProcessInstance(final ProcessInstance processInstance) {
        //        this.getHibernateTemplate().execute(new HibernateCallback() {

        //            public Object doInHibernate(Session arg0) throws HibernateException, SQLException {
        //                processInstance.setSuspended(Boolean.FALSE);
        //                arg0.update(processInstance);

        //                String hql1 = "Update org.fireflow.engine.impl.TaskInstance m Set m.suspended=:suspended Where m.processInstanceId=:processInstanceId And (m.state=0 Or m.state=1)";
        //                Query query1 = arg0.createQuery(hql1);
        //                query1.setBoolean("suspended", Boolean.FALSE);
        //                query1.setString("processInstanceId", processInstance.getId());
        //                query1.executeUpdate();

        //                return null;
        //            }
        //        });
        //    }

        //    public void abortProcessInstance(final ProcessInstance processInstance) {
        //        this.getHibernateTemplate().execute(new HibernateCallback() {

        //            public Object doInHibernate(Session arg0) throws HibernateException, SQLException {
        //                DateTime now = rtCtx.getCalendarService().getSysDate();
        //                processInstance.setState(IProcessInstance.CANCELED);
        //                processInstance.setEndTime(now);
        //                arg0.update(processInstance);

        //                String hql1 = "Update org.fireflow.engine.impl.TaskInstance as m set m.state=:state,m.endTime=:endTime,m.canBeWithdrawn=:canBewithdrawn Where m.processInstanceId=:processInstanceId And (m.state=0 Or m.state=1)";
        //                Query query1 = arg0.createQuery(hql1);
        //                query1.setInteger("state", ITaskInstance.CANCELED);
        //                query1.setDate("endTime", now);
        //                query1.setBoolean("canBewithdrawn", Boolean.FALSE);
        //                query1.setString("processInstanceId", processInstance.getId());
        //                query1.executeUpdate();


        //                String hql2 = "Update org.fireflow.engine.impl.WorkItem as m set m.state=:state,m.endTime=:endTime Where m.taskInstance in (From org.fireflow.engine.impl.TaskInstance n  Where n.processInstanceId=:processInstanceId)   And (m.state=0 Or m.state=1)";
        //                Query query2 = arg0.createQuery(hql2);
        //                query2.setInteger("state", IWorkItem.CANCELED);
        //                query2.setDate("endTime", now);
        //                query2.setString("processInstanceId", processInstance.getId());
        //                query2.executeUpdate();

        //                String hql3 = "Delete org.fireflow.kernel.impl.Token where processInstanceId=:processInstanceId";
        //                Query query3 = arg0.createQuery(hql3);
        //                query3.setString("processInstanceId", processInstance.getId());
        //                query3.executeUpdate();

        //                return null;
        //            }
        //        });
        //    }
        //    /*
        //    public List<IWorkItem> findHaveDoneWorkItems(final String processInstanceId,final String activityId){
        //    if (processInstanceId==null || processInstanceId.trim().Equals("")||
        //    activityId==null || activityId.trim().Equals("")){
        //    return null;
        //    }
        //    Object result = this.getHibernateTemplate().execute(new HibernateCallback() {
        //    public Object doInHibernate(Session arg0) throws HibernateException, SQLException {
        //    String hql = " from org.fireflow.engine.impl.WorkItem as model where model.taskInstance.processInstance.id=? and model.activityId=? and model.state=2";
        //    Query query = arg0.createQuery(hql);
        //    query.setString(0, processInstanceId);
        //    query.setString(1, activityId);
        //    return query.list();
        //    }
        //    });

        //    return (List)result;
        //    }
        //     */

        //    public void saveOrUpdateProcessInstanceTrace(ProcessInstanceTrace processInstanceTrace) {
        //        this.getHibernateTemplate().saveOrUpdate(processInstanceTrace);
        //    }
        //    public List findProcessInstanceTraces(final String processInstanceId){
        //        List l = (List)this.getHibernateTemplate().execute(new HibernateCallback() {
        //            public Object doInHibernate(Session arg0)
        //                    throws HibernateException, SQLException {
        //                String hql = "From org.fireflow.engine.impl.ProcessInstanceTrace as m Where m.processInstanceId=:processInstanceId Order by m.stepNumber,m.minorNumber";
        //                Query q = arg0.createQuery(hql);
        //                q.setString("processInstanceId", processInstanceId);

        //                return q.list();
        //            }
        //        });

        //        return l;
        //    }


    }
}
