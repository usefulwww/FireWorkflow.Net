/*--

 Copyright (C) 2002-2003 Anthony Eden.
 All rights reserved.

 Redistribution and use in source and binary forms, with or without
 modification, are permitted provided that the following conditions
 are met:

 1. Redistributions of source code must retain the above copyright
    notice, this list of conditions, and the following disclaimer.

 2. Redistributions in binary form must reproduce the above copyright
    notice, this list of conditions, and the disclaimer that follows
    these conditions in the documentation and/or other materials
    provided with the distribution.

 3. The names "OBE" and "Open Business Engine" must not be used to
    endorse or promote products derived from this software without prior
    written permission.  For written permission, please contact
    me@anthonyeden.com.

 4. Products derived from this software may not be called "OBE" or
    "Open Business Engine", nor may "OBE" or "Open Business Engine"
    appear in their name, without prior written permission from
    Anthony Eden (me@anthonyeden.com).

 In addition, I request (but do not require) that you include in the
 end-user documentation provided with the redistribution and/or in the
 software itself an acknowledgement equivalent to the following:
     "This product includes software developed by
      Anthony Eden (http://www.anthonyeden.com/)."

 THIS SOFTWARE IS PROVIDED ``AS IS'' AND ANY EXPRESSED OR IMPLIED
 WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
 OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 DISCLAIMED.  IN NO EVENT SHALL THE AUTHOR(S) BE LIABLE FOR ANY DIRECT,
 INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
 SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)
 HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT,
 STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING
 IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
 POSSIBILITY OF SUCH DAMAGE.

 For more information on OBE, please see <http://www.openbusinessengine.org/>.
@author Anthony Eden
 updated by nychen2000
 @Revision to .NET 无忧 lwz0721@gmail.com 2010-02
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using FireWorkflow.Net.Model.Net;
using FireWorkflow.Net.Model.Resource;

namespace FireWorkflow.Net.Model.Io
{
    /// <summary>
    /// FPDL序列化器。将WorkflowProcess对象序列化到一个输出流。
    /// </summary>
    public class Dom4JFPDLSerializer : IFPDLSerializer
    {
        XNamespace xN = FPDL_URI;

        /// <summary>
        /// 将WorkflowProcess对象序列化到一个输出流。
        /// </summary>
        /// <param name="workflowProcess">工作流定义</param>
        /// <param name="swout">输出流</param>
        public override void serialize(WorkflowProcess workflowProcess, Stream swout)
        {
            if (swout == null) return;

            XDocument inventoryDoc =
                new XDocument(
                    new XDeclaration("1.0", "utf-8", "yes"),
                    workflowProcessToDom(workflowProcess)
                    );
            XmlWriter writer = XmlWriter.Create(swout);
            if (writer != null)
            {
                inventoryDoc.Save(writer);
                writer.Close();
            }
            swout.Position = 0;
        }

        private XElement workflowProcessToDom(WorkflowProcess workflowProcess)
        {
            XNamespace aw = FPDL_URI;
            XElement root = new XElement(
                xN + WORKFLOW_PROCESS,
                new XAttribute(XNamespace.Xmlns + FPDL_NS_PREFIX, FPDL_URI),
                new XAttribute(ID, workflowProcess.Id),
                new XAttribute(NAME, workflowProcess.Name),
                new XAttribute(DISPLAY_NAME, workflowProcess.DisplayName),
                new XAttribute(RESOURCE_FILE, workflowProcess.ResourceFile),
                new XAttribute(RESOURCE_MANAGER, workflowProcess.ResourceManager),
                new XElement(xN + DESCRIPTION, workflowProcess.Description)
                );


            if (!String.IsNullOrEmpty(workflowProcess.TaskInstanceCreator))
            {
                root.Add(new XAttribute(TASK_INSTANCE_CREATOR, workflowProcess.TaskInstanceCreator));
            }
            if (!String.IsNullOrEmpty(workflowProcess.FormTaskInstanceRunner))
            {
                root.Add(new XAttribute(FORM_TASK_INSTANCE_RUNNER, workflowProcess.FormTaskInstanceRunner));
            }
            if (!String.IsNullOrEmpty(workflowProcess.ToolTaskInstanceRunner))
            {
                root.Add(new XAttribute(TOOL_TASK_INSTANCE_RUNNER, workflowProcess.ToolTaskInstanceRunner));
            }
            if (!String.IsNullOrEmpty(workflowProcess.SubflowTaskInstanceRunner))
            {
                root.Add(new XAttribute(SUBFLOW_TASK_INSTANCE_RUNNER, workflowProcess.SubflowTaskInstanceRunner));
            }
            if (!String.IsNullOrEmpty(workflowProcess.FormTaskInstanceCompletionEvaluator))
            {
                root.Add(new XAttribute(FORM_TASK_INSTANCE_COMPLETION_EVALUATOR, workflowProcess.FormTaskInstanceCompletionEvaluator));
            }
            if (!String.IsNullOrEmpty(workflowProcess.ToolTaskInstanceCompletionEvaluator))
            {
                root.Add(new XAttribute(TOOL_TASK_INSTANCE_COMPLETION_EVALUATOR, workflowProcess.ToolTaskInstanceCompletionEvaluator));
            }
            if (!String.IsNullOrEmpty(workflowProcess.SubflowTaskInstanceCompletionEvaluator))
            {
                root.Add(new XAttribute(SUBFLOW_TASK_INSTANCE_COMPLETION_EVALUATOR, workflowProcess.SubflowTaskInstanceCompletionEvaluator));
            }
            root.Add(writeDataFields(workflowProcess.DataFields));
            root.Add(writeStartNode(workflowProcess.StartNode));
            root.Add(writeTasks(workflowProcess.Tasks));
            root.Add(writeActivities(workflowProcess.Activities));
            root.Add(writeSynchronizers(workflowProcess.Synchronizers));
            root.Add(writeEndNodes(workflowProcess.EndNodes));
            root.Add(writeTransitions(workflowProcess.Transitions));
            root.Add(writeLoops(workflowProcess.Loops));
            root.Add(writeEventListeners(workflowProcess.EventListeners));
            root.Add(writeExtendedAttributes(workflowProcess.ExtendedAttributes));

            return root;
        }


        #region 序列化Dictionary＜string, string＞类型数据

        private XElement writeEventListeners(List<EventListener> eventListeners)
        {
            if (eventListeners == null || eventListeners.Count <= 0) { return null; }

            XElement eventListenersElm = new XElement(xN + EVENT_LISTENERS);

            foreach (EventListener listener in eventListeners)
            {
                eventListenersElm.Add(
                    new XElement(xN + EVENT_LISTENER, new XAttribute(CLASS_NAME, listener.ClassName))
                        );
            }
            return eventListenersElm;
        }

        /// <summary>序列化Dictionary＜string, string＞类型数据</summary>
        /// <param name="extendedAttributes"></param>
        /// <param name="parent"></param>
        private XElement writeExtendedAttributes(Dictionary<string, string> extendedAttributes)
        {
            if (extendedAttributes == null || extendedAttributes.Count <= 0)
            {
                return null;
            }

            XElement extendedAttributesElement = new XElement(xN + EXTENDED_ATTRIBUTES);

            foreach (String key in extendedAttributes.Keys)
            {
                extendedAttributesElement.Add(new XElement(xN + EXTENDED_ATTRIBUTE,
                    new XAttribute(NAME, key),
                    new XAttribute(VALUE, extendedAttributes[key])
                    ));
            }
            return extendedAttributesElement;
        }
        #endregion

        #region DataField
        private XElement writeDataFields(List<DataField> dataFields)
        {
            if (dataFields == null || dataFields.Count <= 0)
            {
                return null;
            }
            XElement dataFieldsElement = new XElement(xN + DATA_FIELDS);

            foreach (DataField dataField in dataFields)
            {
                dataFieldsElement.Add(
                    new XElement(xN + DATA_FIELD,
                        new XAttribute(ID, dataField.Id),
                        new XAttribute(NAME, dataField.Name),
                        new XAttribute(DISPLAY_NAME, dataField.DisplayName),
                        new XAttribute(DATA_TYPE, dataField.DataType.ToString()),
                        new XAttribute(INITIAL_VALUE, dataField.InitialValue),
                        new XElement(xN + DESCRIPTION, dataField.Description),
                        writeExtendedAttributes(dataField.ExtendedAttributes)
                        ));
            }
            return dataFieldsElement;
        }
        #endregion

        #region StartNode
        private XElement writeStartNode(StartNode startNode)
        {
            if (startNode == null) { return null; }

            XElement dataFieldsElement = new XElement(
                xN + START_NODE,
                new XAttribute(ID, startNode.Id),
                new XAttribute(NAME, startNode.Name),
                new XAttribute(DISPLAY_NAME, startNode.DisplayName),
                new XElement(xN + DESCRIPTION, startNode.Description),
                writeExtendedAttributes(startNode.ExtendedAttributes)
                );
            return dataFieldsElement;
        }
        #endregion

        #region Tasks
        private XElement writeTasks(List<Task> tasks)
        {
            XElement tasksElement = new XElement(xN + TASKS);

            foreach (Task item in tasks)
            {
                tasksElement.Add(writeTask(item));
            }
            return tasksElement;
        }

        private XElement writeTask(Task task)
        {
            XElement taskElement = new XElement(
                xN + TASK,
                new XAttribute(ID, task.Id),
                new XAttribute(NAME, task.Name),
                new XAttribute(DISPLAY_NAME, task.DisplayName),
                new XAttribute(TYPE, task.TaskType.ToString())//,
                //new XElement(xN + DESCRIPTION, startNode.Description),
                );

            TaskTypeEnum type = task.TaskType;
            if (task is FormTask)
            {
                taskElement.Add(this.writePerformer(((FormTask)task).Performer));

                taskElement.Add(new XAttribute(COMPLETION_STRATEGY, ((FormTask)task).AssignmentStrategy.ToString()));
                taskElement.Add(new XAttribute(DEFAULT_VIEW, ((FormTask)task).DefaultView.ToString()));

                taskElement.Add(this.writeForm(EDIT_FORM, ((FormTask)task).EditForm));
                taskElement.Add(this.writeForm(VIEW_FORM, ((FormTask)task).ViewForm));
                taskElement.Add(this.writeForm(LIST_FORM, ((FormTask)task).ListForm));
            }
            else if (task is ToolTask)
            {
                taskElement.Add(this.writeApplication(((ToolTask)task).Application));
                //taskElement.Add(new XAttribute(EXECUTION), ((ToolTask)task).Execution.ToString()));
            }
            else if (task is SubflowTask)
            {
                taskElement.Add(this.writeSubWorkflowProcess(((SubflowTask)task).SubWorkflowProcess));
            }

            taskElement.Add(new XAttribute(PRIORITY, task.Priority.ToString()));

            taskElement.Add(writeDuration(task.Duration));

            taskElement.Add(new XElement(xN + DESCRIPTION, task.Description));

            if (!String.IsNullOrEmpty(task.TaskInstanceCreator))
            {
                taskElement.Add(new XAttribute(TASK_INSTANCE_CREATOR, task.TaskInstanceCreator));
            }
            if (!String.IsNullOrEmpty(task.TaskInstanceRunner))
            {
                taskElement.Add(new XAttribute(TASK_INSTANCE_RUNNER, task.TaskInstanceRunner));

            }
            if (!String.IsNullOrEmpty(task.TaskInstanceCompletionEvaluator))
            {
                taskElement.Add(new XAttribute(TASK_INSTANCE_COMPLETION_EVALUATOR, task.TaskInstanceCompletionEvaluator));

            }

            taskElement.Add(new XAttribute(LOOP_STRATEGY, task.LoopStrategy.ToString()));

            taskElement.Add(writeEventListeners(task.EventListeners));
            taskElement.Add(writeExtendedAttributes(task.ExtendedAttributes));

            return taskElement;
        }

        private XElement writePerformer(Participant participant)
        {
            if (participant == null) { return null; }
            XElement participantElement = new XElement(
                xN + PERFORMER,
                new XAttribute(NAME, participant.Name),
                new XAttribute(DISPLAY_NAME, participant.DisplayName),
                new XAttribute(ASSIGNMENT_TYPE, participant.AssignmentType.ToString()), //201004 add lwz 参与者通过业务接口实现默认获取用户
                new XElement(xN + DESCRIPTION, participant.Description),
                new XElement(xN + ASSIGNMENT_HANDLER, participant.AssignmentHandler)
                );
            return participantElement;
        }

        private XElement writeForm(String formName, Form form)
        {
            if (form == null) { return null; }
            XElement editFormElement = new XElement(
                xN + formName,
                new XAttribute(NAME, form.Name),
                new XAttribute(DISPLAY_NAME, form.DisplayName),
                new XElement(xN + DESCRIPTION, form.Description),
                new XElement(xN + URI, form.Uri)
                );
            return editFormElement;
        }

        private XElement writeApplication(Application application)
        {
            if (application == null) { return null; }
            XElement applicationElement = new XElement(
                xN + APPLICATION,
                new XAttribute(NAME, application.Name),
                new XAttribute(DISPLAY_NAME, application.DisplayName),
                new XElement(xN + DESCRIPTION, application.Description),
                new XElement(xN + HANDLER, application.Handler)
                );
            return applicationElement;
        }

        private XElement writeSubWorkflowProcess(SubWorkflowProcess subWorkflowProcess)
        {
            if (subWorkflowProcess == null) { return null; }
            XElement subflowElement = new XElement(
                xN + SUB_WORKFLOW_PROCESS,
                new XAttribute(NAME, subWorkflowProcess.Name),
                new XAttribute(DISPLAY_NAME, subWorkflowProcess.DisplayName),
                new XElement(xN + DESCRIPTION, subWorkflowProcess.Description),
                new XElement(xN + WORKFLOW_PROCESS_ID, subWorkflowProcess.WorkflowProcessId)
                );
            return subflowElement;
        }

        private XElement writeDuration(Duration duration)
        {
            if (duration == null) { return null; }

            XElement durationElement = new XElement(
                xN + DURATION,
                new XAttribute(VALUE, duration.Value.ToString()),
                new XAttribute(UNIT, duration.Unit.ToString()),
                new XAttribute(IS_BUSINESS_TIME, duration.IsBusinessTime.ToString())
                );
            return durationElement;
        }
        #endregion

        #region Activitie
        private XElement writeActivities(List<Activity> activities)
        {
            if (activities == null || activities.Count <= 0) { return null; }

            XElement activitiesElement = new XElement(xN + ACTIVITIES);

            foreach (Activity item in activities)
            {

                activitiesElement.Add(writeActivity(item));
            }
            return activitiesElement;
        }

        private XElement writeActivity(Activity activity)
        {
            if (activity == null) { return null; }

            XElement activityElement = new XElement(
                xN + ACTIVITY,
                new XAttribute(ID, activity.Id),
                new XAttribute(NAME, activity.Name),
                new XAttribute(DISPLAY_NAME, activity.DisplayName),
                new XAttribute(COMPLETION_STRATEGY, activity.CompletionStrategy.ToString()),
                new XElement(xN + DESCRIPTION, activity.Description),
                writeEventListeners(activity.EventListeners),
                writeExtendedAttributes(activity.ExtendedAttributes),
                writeTasks(activity.InlineTasks),
                writeTaskRefs(activity.TaskRefs)
                );
            return activityElement;
        }

        private XElement writeTaskRefs(List<TaskRef> taskRefs)
        {
            XElement taskRefsElement = new XElement(xN + TASKREFS);

            foreach (TaskRef taskRef in taskRefs)
            {
                taskRefsElement.Add(new XElement(xN + TASKREF, new XAttribute(REFERENCE, taskRef.ReferencedTask.Id)));
            }
            return taskRefsElement;
        }
        #endregion

        #region Synchronizer
        private XElement writeSynchronizers(List<Synchronizer> synchronizers)
        {
            if (synchronizers == null || synchronizers.Count <= 0) { return null; }
            XElement synchronizersElement = new XElement(xN + SYNCHRONIZERS);

            foreach (Synchronizer item in synchronizers)
            {
                synchronizersElement.Add(writeSynchronizer(item));
            }
            return synchronizersElement;
        }

        private XElement writeSynchronizer(Synchronizer synchronizer)
        {
            if (synchronizer == null) { return null; }

            XElement synchronizerElement = new XElement(
                xN + SYNCHRONIZER,
                new XAttribute(ID, synchronizer.Id),
                new XAttribute(NAME, synchronizer.Name),
                new XAttribute(DISPLAY_NAME, synchronizer.DisplayName),
                new XElement(xN + DESCRIPTION, synchronizer.Description),
                writeExtendedAttributes(synchronizer.ExtendedAttributes)
                );
            return synchronizerElement;
        }
        #endregion

        #region EndNode
        private XElement writeEndNodes(List<EndNode> endNodes)
        {
            if (endNodes == null || endNodes.Count <= 0) { return null; }
            XElement endNodesElement = new XElement(xN + END_NODES);

            foreach (EndNode item in endNodes)
            {
                endNodesElement.Add(writeEndNode(item));
            }
            return endNodesElement;
        }

        private XElement writeEndNode(EndNode endNode)
        {
            if (endNode == null) { return null; }

            XElement endNodeElement = new XElement(
                xN + END_NODE,
                new XAttribute(ID, endNode.Id),
                new XAttribute(NAME, endNode.Name),
                new XAttribute(DISPLAY_NAME, endNode.DisplayName),
                new XElement(xN + DESCRIPTION, endNode.Description),
                writeExtendedAttributes(endNode.ExtendedAttributes)
                );
            return endNodeElement;
        }
        #endregion

        #region Transitions
        private XElement writeTransitions(List<Transition> transitions)
        {
            if (transitions == null || transitions.Count <= 0) { return null; }

            XElement transitionsElement = new XElement(xN + TRANSITIONS);

            foreach (Transition item in transitions)
            {
                transitionsElement.Add(writeTransition(item));
            }
            return transitionsElement;
        }

        private XElement writeTransition(Transition transition)
        {
            if (transition == null) { return null; }

            XElement transitionElement = new XElement(
                xN + TRANSITION,
                new XAttribute(ID, transition.Id),
                new XAttribute(FROM, transition.FromNode.Id),
                new XAttribute(TO, transition.ToNode.Id),
                new XAttribute(NAME, transition.Name),
                new XAttribute(DISPLAY_NAME, transition.DisplayName),
                new XElement(xN + CONDITION, transition.Condition),
                writeExtendedAttributes(transition.ExtendedAttributes)
                );
            return transitionElement;
        }
        #endregion

        #region Loops
        private XElement writeLoops(List<Loop> loops)
        {
            if (loops == null || loops.Count <= 0) { return null; }
            XElement transitionsElement = new XElement(xN + LOOPS);

            foreach (Loop loop in loops)
            {
                transitionsElement.Add(new XElement(
                    xN + LOOP,
                    new XAttribute(ID, loop.Id),
                    new XAttribute(FROM, loop.FromNode.Id),
                    new XAttribute(TO, loop.ToNode.Id),
                    new XAttribute(NAME, loop.Name),
                    new XAttribute(DISPLAY_NAME, loop.DisplayName),
                    new XElement(xN + CONDITION, loop.Condition),
                    writeExtendedAttributes(loop.ExtendedAttributes)
                    ));
            }
            return transitionsElement;
        }
        #endregion

    }
}
