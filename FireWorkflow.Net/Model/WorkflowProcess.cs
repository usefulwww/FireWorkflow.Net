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
using FireWorkflow.Net.Model.Net;

namespace FireWorkflow.Net.Model
{
    /// <summary>
    /// <para>业务流程。</para>
    /// 这是Fire workflow工作流模型的最顶层元素。
    /// </summary>
    public class WorkflowProcess : AbstractWFElement
    {
        #region 属性

        #region 子元素

        /// <summary>获取或设置流程数据项，运行时转换为流程变量进行存储。</summary>
        public List<DataField> DataFields { get; set; }

        /// <summary>获取或设置全局Task。</summary>
        public List<Task> Tasks { get; set; }

        /// <summary>获取或设置流程环节</summary>
        public List<Activity> Activities { get; set; }

        /// <summary>获取或设置转移</summary>
        public List<Transition> Transitions { get; set; }

        /// <summary>获取或设置循环</summary>
        public List<Loop> Loops = new List<Loop>();

        /// <summary>获取或设置同步器</summary>
        public List<Synchronizer> Synchronizers { get; set; }

        /// <summary>获取或设置开始节点</summary>
        public StartNode StartNode { get; set; }

        /// <summary>获取或设置结束节点</summary>
        public List<EndNode> EndNodes { get; set; }

        #endregion

        #region 其他属性

        /// <summary>获取或设置资源文件（在1.0中暂时未使用）</summary>
        public String ResourceFile { get; set; }

        /// <summary>获取或设置资源管理器（在1.0中暂时未使用）</summary>
        public String ResourceManager { get; set; }

        /// <summary>
        /// 获取或设置本流程全局的任务实例创建器。
        /// 如果没有设置，引擎将使用DefaultTaskInstanceCreator来创建TaskInstance。
        /// </summary>
        public String TaskInstanceCreator { get; set; }

        /// <summary>
        /// 获取或设置本流程全局的FormTask Instance运行器。
        /// 如果没有设置，引擎将使用DefaultFormTaskInstanceRunner来运行TaskInstance。
        /// </summary>
        public String FormTaskInstanceRunner { get; set; }

        /// <summary>
        /// 获取或设置本流程全局的ToolTask Instance运行器。
        /// 如果没有设置，引擎将使用DefaultToolTaskInstanceRunner来运行TaskInstance。
        /// </summary>
        public String ToolTaskInstanceRunner { get; set; }


        /// <summary>
        /// 获取或设置本流程全局的SubflowTask Instance运行器。
        /// 如果没有设置，引擎将使用DefaultSubflowTaskInstanceRunner来运行TaskInstance。
        /// </summary>
        public String SubflowTaskInstanceRunner { get; set; }


        /// <summary>
        /// 获取或设置本流程全局的FormTask Instance 终结评价器，用于告诉引擎该实例是否可以结束。<br/>
        /// 如果没有设置，引擎使用缺省实现DefaultFormTaskInstanceCompletionEvaluator。
        /// </summary>
        public String FormTaskInstanceCompletionEvaluator { get; set; }

        /// <summary>
        /// 获取或设置本流程全局的ToolTask Instance 终结评价器，用于告诉引擎该实例是否可以结束。<br/>
        /// 如果没有设置，引擎使用缺省实现DefaultToolTaskInstanceCompletionEvaluator。
        /// </summary>
        public String ToolTaskInstanceCompletionEvaluator { get; set; }

        /// <summary>
        /// 获取或设置本流程全局的SubflowTask Instance 终结评价器，用于告诉引擎该实例是否可以结束。<br/>
        /// 如果没有设置，引擎使用缺省实现DefaultSubflowTaskInstanceCompletionEvaluator。
        /// </summary>
        public String SubflowTaskInstanceCompletionEvaluator { get; set; }

        //    private int version = 1;//version在流程定义中不需要，只有在流程存储中需要，每次updatge数据库，都需要增加Version值

        #endregion

        #endregion

        #region 构造函数
        public WorkflowProcess(String name)
            : base(null, name)
        {
            this.DataFields = new List<DataField>();
            this.Tasks = new List<Task>();
            this.Activities = new List<Activity>();
            this.Transitions = new List<Transition>();
            this.Loops = new List<Loop>();
            this.Synchronizers = new List<Synchronizer>();
            this.EndNodes = new List<EndNode>();
        }
        #endregion

        #region 方法
        /// <summary>通过ID查找该流程中的任意元素</summary>
        /// <param name="id">元素的Id</param>
        /// <returns>流程元素，如：Activity,Task,Synchronizer等等</returns>
        public IWFElement findWFElementById(String id)
        {
            if (this.Id.Equals(id))
            {
                return this;
            }

            List<Task> tasksList = this.Tasks;
            for (int i = 0; i < tasksList.Count; i++)
            {
                Task task = (Task)tasksList[i];
                if (task.Id.Equals(id))
                {
                    return task;
                }
            }

            List<Activity> activityList = this.Activities;
            for (int i = 0; i < activityList.Count; i++)
            {
                Activity activity = activityList[i];
                if (activity.Id.Equals(id))
                {
                    return activity;
                }
                List<Task> taskList = activity.getTasks();
                for (int j = 0; j < taskList.Count; j++)
                {
                    Task task = taskList[j];
                    if (task.Id.Equals(id))
                    {
                        return task;
                    }
                }
            }
            if (this.StartNode.Id.Equals(id))
            {
                return this.StartNode;
            }
            List<Synchronizer> synchronizerList = this.Synchronizers;
            for (int i = 0; i < synchronizerList.Count; i++)
            {
                Synchronizer synchronizer = synchronizerList[i];
                if (synchronizer.Id.Equals(id))
                {
                    return synchronizer;
                }
            }

            List<EndNode> endNodeList = this.EndNodes;
            for (int i = 0; i < endNodeList.Count; i++)
            {
                EndNode endNode = endNodeList[i];
                if (endNode.Id.Equals(id))
                {
                    return endNode;
                }
            }

            List<Transition> transitionList = this.Transitions;
            for (int i = 0; i < transitionList.Count; i++)
            {
                Transition transition = transitionList[i];
                if (transition.Id.Equals(id))
                {
                    return transition;
                }
            }

            List<DataField> dataFieldList = this.DataFields;
            for (int i = 0; i < dataFieldList.Count; i++)
            {
                DataField dataField = dataFieldList[i];
                if (dataField.Id.Equals(id))
                {
                    return dataField;
                }
            }

            List<Loop> loopList = this.Loops;
            for (int i = 0; i < loopList.Count; i++)
            {
                Loop loop = loopList[i];
                if (loop.Id.Equals(id))
                {
                    return loop;
                }
            }
            return null;
        }

        /// <summary>通过Id查找任意元素的序列号</summary>
        /// <param name="id">流程元素的id</param>
        /// <returns>流程元素的序列号</returns>
        public String findSnById(String id)
        {
            IWFElement elem = this.findWFElementById(id);
            if (elem != null)
            {
                return elem.Sn;
            }
            return null;
        }

        /// <summary>验证workflow process是否完整正确。</summary>
        /// <returns>null表示流程正确；否则表示流程错误，返回值是错误原因</returns>
        public String validate()
        {
            String errHead = "Workflow process is invalid：";
            if (this.StartNode == null)
            {
                return errHead + "must have one start node";
            }
            if (this.StartNode.LeavingTransitions.Count == 0)
            {
                return errHead + "start node must have leaving transitions.";
            }


            List<Activity> activities = this.Activities;
            for (int i = 0; i < activities.Count; i++)
            {
                Activity activity = activities[i];
                String theName = (String.IsNullOrEmpty(activity.DisplayName)) ? activity.Name : activity.DisplayName;
                if (activity.EnteringTransition == null)
                {
                    return errHead + "activity[" + theName + "] must have entering transition.";
                }
                if (activity.LeavingTransition == null)
                {
                    return errHead + "activity[" + theName + "] must have leaving transition.";
                }

                //check tasks
                List<Task> taskList = activity.getTasks();
                for (int j = 0; j < taskList.Count; j++)
                {
                    Task task = (Task)taskList[j];
                    if (task.TaskType==TaskTypeEnum.FORM)
                    {
                        FormTask formTask = (FormTask)task;
                        if (formTask.Performer == null)
                        {
                            return errHead + "FORM-task[id=" + task.Id + "] must has a performer.";
                        }
                    }
                    else if (task.TaskType==TaskTypeEnum.TOOL)
                    {
                        ToolTask toolTask = (ToolTask)task;
                        if (toolTask.Application == null)
                        {
                            return errHead + "TOOL-task[id=" + task.Id + "] must has a application.";
                        }
                    }
                    else if (task.TaskType==TaskTypeEnum.SUBFLOW)
                    {
                        SubflowTask subflowTask = (SubflowTask)task;
                        if (subflowTask.SubWorkflowProcess == null)
                        {
                            return errHead + "SUBFLOW-task[id=" + task.Id + "] must has a subflow.";
                        }
                    }
                    else
                    {
                        return errHead + " unknown task type of task[" + task.Id + "]";
                    }
                }
            }

            List<Synchronizer> synchronizers = this.Synchronizers;
            for (int i = 0; i < synchronizers.Count; i++)
            {
                Synchronizer synchronizer = synchronizers[i];
                String theName = (synchronizer.DisplayName == null || synchronizer.DisplayName.Equals("")) ? synchronizer.Name : synchronizer.DisplayName;
                if (synchronizer.EnteringTransitions.Count == 0)
                {
                    return errHead + "synchronizer[" + theName + "] must have entering transition.";
                }
                if (synchronizer.LeavingTransitions.Count == 0)
                {
                    return errHead + "synchronizer[" + theName + "] must have leaving transition.";
                }
            }

            List<EndNode> endnodes = this.EndNodes;
            for (int i = 0; i < endnodes.Count; i++)
            {
                EndNode endnode = endnodes[i];
                String theName = (endnode.DisplayName == null || endnode.DisplayName.Equals("")) ? endnode.Name : endnode.DisplayName;
                if (endnode.EnteringTransitions.Count == 0)
                {
                    return errHead + "end node[" + theName + "] must have entering transition.";
                }
            }

            List<Transition> transitions = this.Transitions;
            for (int i = 0; i < transitions.Count; i++)
            {
                Transition transition = transitions[i];
                String theName = (transition.DisplayName == null || transition.DisplayName.Equals("")) ? transition.Name : transition.DisplayName;
                if (transition.FromNode == null)
                {
                    return errHead + "transition[" + theName + "] must have from node.";

                }
                if (transition.ToNode == null)
                {
                    return errHead + "transition[" + theName + "] must have to node.";
                }
            }

            //check datafield 不再需要 DataType不肯能为空
            //List<DataField> dataFieldList = this.DataFields;
            //for (int i = 0; i < dataFieldList.Count; i++)
            //{
            //    DataField df = dataFieldList[i];
            //    if (df.DataType == DataTypeEnum. null)
            //    {
            //        return errHead + "unknown data type of datafield[" + df.Id + "]";
            //    }
            //}

            return null;
        }

        /// <summary>
        /// 判断是否可以从from节点到达to节点
        /// </summary>
        /// <param name="fromNodeId">from节点的id</param>
        /// <param name="toNodeId">to节点的id</param>
        public Boolean isReachable(String fromNodeId, String toNodeId)
        {
            if (fromNodeId == null || toNodeId == null)
            {
                return false;
            }
            if (fromNodeId.Equals(toNodeId))
            {
                return true;
            }
            List<Node> reachableList = this.getReachableNodes(fromNodeId);

            for (int j = 0; reachableList != null && j < reachableList.Count; j++)
            {
                Node node = reachableList[j];
                if (node.Id.Equals(toNodeId))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>判断两个Activity是否在同一个执行线上</summary>
        /// <returns>true表示在同一个执行线上，false表示不在同一个执行线上</returns>
        public Boolean isInSameLine(String activityId1, String activityId2)
        {
            Node node1 = (Node)this.findWFElementById(activityId1);
            Node node2 = (Node)this.findWFElementById(activityId2);
            if (node1 == null || node2 == null) return false;
            List<Node> connectableNodes4Activity1 = new List<Node>();
            connectableNodes4Activity1.Add(node1);
            connectableNodes4Activity1.AddRange(getReachableNodes(activityId1));
            connectableNodes4Activity1.AddRange(getEnterableNodes(activityId1));

            List<Node> connectableNodes4Activity2 = new List<Node>();
            connectableNodes4Activity2.Add(node2);
            connectableNodes4Activity2.AddRange(getReachableNodes(activityId2));
            connectableNodes4Activity2.AddRange(getEnterableNodes(activityId2));
            /*
            System.out.println("===Inside WorkflowProcess.isInSameLine()::connectableNodes4Activity1.Count="+connectableNodes4Activity1.Count);
            System.out.println("===Inside WorkflowProcess.isInSameLine()::connectableNodes4Activity2.Count="+connectableNodes4Activity2.Count);
            System.out.println("-----------------------activity1--------------");
            for (int i=0;i<connectableNodes4Activity1.Count;i++){
                Node node = (Node)connectableNodes4Activity1[i];
                System.out.println("node.id of act1 is "+node.Id);
            }
        
            System.out.println("---------------------activity2--------------------");
            for (int i=0;i<connectableNodes4Activity2.Count;i++){
                Node node = (Node)connectableNodes4Activity2[i];
                System.out.println("node.id of act2 is "+node.Id);
            }
             */

            if (connectableNodes4Activity1.Count != connectableNodes4Activity2.Count)
            {
                return false;
            }

            for (int i = 0; i < connectableNodes4Activity1.Count; i++)
            {
                Node node = (Node)connectableNodes4Activity1[i];
                Boolean find = false;
                for (int j = 0; j < connectableNodes4Activity2.Count; j++)
                {
                    Node tmpNode = (Node)connectableNodes4Activity2[j];
                    if (node.Id.Equals(tmpNode.Id))
                    {
                        find = true;
                        break;
                    }
                }
                if (!find) return false;
            }
            return true;
        }

        /// <summary>获取可以到达的节点</summary>
        /// <param name="nodeId"></param>
        /// <returns></returns>
        public List<Node> getReachableNodes(String nodeId)
        {
            List<Node> reachableNodesList = new List<Node>();
            Node node = (Node)this.findWFElementById(nodeId);
            if (node is Activity)
            {
                Activity activity = (Activity)node;
                Transition leavingTransition = activity.LeavingTransition;
                if (leavingTransition != null)
                {
                    Node toNode = (Node)leavingTransition.ToNode;
                    if (toNode != null)
                    {
                        reachableNodesList.Add(toNode);
                        reachableNodesList.AddRange(getReachableNodes(toNode.Id));
                    }
                }
            }
            else if (node is Synchronizer)
            {
                Synchronizer synchronizer = (Synchronizer)node;
                List<Transition> leavingTransitions = synchronizer.LeavingTransitions;
                for (int i = 0; leavingTransitions != null && i < leavingTransitions.Count; i++)
                {
                    Transition leavingTransition = (Transition)leavingTransitions[i];
                    if (leavingTransition != null)
                    {
                        Node toNode = (Node)leavingTransition.ToNode;
                        if (toNode != null)
                        {
                            reachableNodesList.Add(toNode);
                            reachableNodesList.AddRange(getReachableNodes(toNode.Id));
                        }

                    }
                }
            }

            //剔除重复节点 
            List<Node> tmp = new List<Node>();
            Boolean alreadyInTheList = false;
            for (int i = 0; i < reachableNodesList.Count; i++)
            {
                Node nodeTmp = (Node)reachableNodesList[i];
                alreadyInTheList = false;
                for (int j = 0; j < tmp.Count; j++)
                {
                    Node nodeTmp2 = (Node)tmp[j];
                    if (nodeTmp2.Id.Equals(nodeTmp.Id))
                    {
                        alreadyInTheList = true;
                        break;
                    }
                }
                if (!alreadyInTheList)
                {
                    tmp.Add(nodeTmp);
                }
            }
            reachableNodesList = tmp;
            return reachableNodesList;
        }

        /// <summary>获取进入的节点(activity 或者synchronizer)</summary>
        /// <param name="nodeId"></param>
        /// <returns></returns>
        public List<Node> getEnterableNodes(String nodeId)
        {
            List<Node> enterableNodesList = new List<Node>();
            Node node = (Node)this.findWFElementById(nodeId);
            if (node is Activity)
            {
                Activity activity = (Activity)node;
                Transition enteringTransition = activity.EnteringTransition;
                if (enteringTransition != null)
                {
                    Node fromNode = (Node)enteringTransition.FromNode;
                    if (fromNode != null)
                    {
                        enterableNodesList.Add(fromNode);
                        enterableNodesList.AddRange(getEnterableNodes(fromNode.Id));
                    }
                }
            }
            else if (node is Synchronizer)
            {
                Synchronizer synchronizer = (Synchronizer)node;
                List<Transition> enteringTransitions = synchronizer.EnteringTransitions;
                for (int i = 0; enteringTransitions != null && i < enteringTransitions.Count; i++)
                {
                    Transition enteringTransition = (Transition)enteringTransitions[i];
                    if (enteringTransition != null)
                    {
                        Node fromNode = (Node)enteringTransition.FromNode;
                        if (fromNode != null)
                        {
                            enterableNodesList.Add(fromNode);
                            enterableNodesList.AddRange(getEnterableNodes(fromNode.Id));
                        }

                    }
                }
            }

            //剔除重复节点
            //TODO mingjie.mj 20091018 改为使用集合是否更好?
            List<Node> tmp = new List<Node>();
            Boolean alreadyInTheList = false;
            for (int i = 0; i < enterableNodesList.Count; i++)
            {
                Node nodeTmp = (Node)enterableNodesList[i];
                alreadyInTheList = false;
                for (int j = 0; j < tmp.Count; j++)
                {
                    Node nodeTmp2 = (Node)tmp[j];
                    if (nodeTmp2.Id.Equals(nodeTmp.Id))
                    {
                        alreadyInTheList = true;
                        break;
                    }
                }
                if (!alreadyInTheList)
                {
                    tmp.Add(nodeTmp);
                }
            }
            enterableNodesList = tmp;
            return enterableNodesList;
        }
        #endregion
    }
}
