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
//using System.Linq;
using System.Xml;
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
		//XNamespace xN = FPDL_URI;

		/// <summary>
		/// 将WorkflowProcess对象序列化到一个输出流。
		/// </summary>
		/// <param name="workflowProcess">工作流定义</param>
		/// <param name="swout">输出流</param>
		public override void serialize(WorkflowProcess workflowProcess, Stream swout)
		{
			if (swout == null) return;

//			XDocument inventoryDoc =
//				new XDocument(
//					new XDeclaration("1.0", "utf-8", "yes"),
//					workflowProcessToDom(workflowProcess)
//				);
			XmlDocument inventoryDoc = new XmlDocument();
			inventoryDoc.AppendChild(workflowProcessToDom(workflowProcess));
			
			XmlElement root = inventoryDoc.DocumentElement;
			inventoryDoc.InsertBefore(inventoryDoc.CreateXmlDeclaration("1.0", "utf-8", "yes"),root);
			
			
			XmlWriter writer = XmlWriter.Create(swout);
			if (writer != null)
			{
				inventoryDoc.Save(writer);
				writer.Close();
			}
			swout.Position = 0;
		}

		public string serialize(WorkflowProcess workflowProcess)
		{
			//XDocument inventoryDoc =new XDocument(new XDeclaration("1.0", "utf-8", "yes"),workflowProcessToDom(workflowProcess));
			XmlDocument inventoryDoc = new XmlDocument();
			inventoryDoc.AppendChild(workflowProcessToDom(workflowProcess));
			
			XmlElement root = inventoryDoc.DocumentElement;
			inventoryDoc.InsertBefore(inventoryDoc.CreateXmlDeclaration("1.0", "utf-8", "yes"),root);
			
			return inventoryDoc.ToString();
		}

		private XmlElement workflowProcessToDom(WorkflowProcess workflowProcess)
		{
			//            XNamespace aw = FPDL_URI;
			//            XmlElement root = new XmlElement(
			//                xN + WORKFLOW_PROCESS,
			//                new XAttribute(XNamespace.Xmlns + FPDL_NS_PREFIX, FPDL_URI),
			//                new XAttribute(ID, workflowProcess.Id),
			//                new XAttribute(NAME, workflowProcess.Name),
			//                new XAttribute(DISPLAY_NAME, workflowProcess.DisplayName),
			//                new XAttribute(RESOURCE_FILE, workflowProcess.ResourceFile),
			//                new XAttribute(RESOURCE_MANAGER, workflowProcess.ResourceManager),
			//                new XElement(xN + DESCRIPTION, workflowProcess.Description)
			//                );
			
			XmlDocument xmldoc = new XmlDocument();

			XmlElement root = xmldoc.CreateElement(FPDL_NS_PREFIX,WORKFLOW_PROCESS,FPDL_URI);
			root.SetAttribute(ID,workflowProcess.Id);
			root.SetAttribute(NAME,workflowProcess.Name);
			root.SetAttribute(DISPLAY_NAME,workflowProcess.DisplayName);
			root.SetAttribute(RESOURCE_FILE,workflowProcess.ResourceFile);
			root.SetAttribute(RESOURCE_MANAGER,workflowProcess.ResourceManager);
			XmlElement desc = xmldoc.CreateElement(DESCRIPTION);
			desc.InnerText = workflowProcess.Description;
			root.AppendChild(desc);
			
			if (!String.IsNullOrEmpty(workflowProcess.TaskInstanceCreator))
			{
				root.SetAttribute(TASK_INSTANCE_CREATOR, workflowProcess.TaskInstanceCreator);
			}
			if (!String.IsNullOrEmpty(workflowProcess.FormTaskInstanceRunner))
			{
				root.SetAttribute(FORM_TASK_INSTANCE_RUNNER, workflowProcess.FormTaskInstanceRunner);
			}
			if (!String.IsNullOrEmpty(workflowProcess.ToolTaskInstanceRunner))
			{
				root.SetAttribute(TOOL_TASK_INSTANCE_RUNNER, workflowProcess.ToolTaskInstanceRunner);
			}
			if (!String.IsNullOrEmpty(workflowProcess.SubflowTaskInstanceRunner))
			{
				root.SetAttribute(SUBFLOW_TASK_INSTANCE_RUNNER, workflowProcess.SubflowTaskInstanceRunner);
			}
			if (!String.IsNullOrEmpty(workflowProcess.FormTaskInstanceCompletionEvaluator))
			{
				root.SetAttribute(FORM_TASK_INSTANCE_COMPLETION_EVALUATOR, workflowProcess.FormTaskInstanceCompletionEvaluator);
			}
			if (!String.IsNullOrEmpty(workflowProcess.ToolTaskInstanceCompletionEvaluator))
			{
				root.SetAttribute(TOOL_TASK_INSTANCE_COMPLETION_EVALUATOR, workflowProcess.ToolTaskInstanceCompletionEvaluator);
			}
			if (!String.IsNullOrEmpty(workflowProcess.SubflowTaskInstanceCompletionEvaluator))
			    {
			    	root.SetAttribute(SUBFLOW_TASK_INSTANCE_COMPLETION_EVALUATOR, workflowProcess.SubflowTaskInstanceCompletionEvaluator);
			    }
			    root.AppendChild(writeDataFields(workflowProcess.DataFields));
			    root.AppendChild(writeStartNode(workflowProcess.StartNode));
			    root.AppendChild(writeTasks(workflowProcess.Tasks));
			    root.AppendChild(writeActivities(workflowProcess.Activities));
			    root.AppendChild(writeSynchronizers(workflowProcess.Synchronizers));
			    root.AppendChild(writeEndNodes(workflowProcess.EndNodes));
			    root.AppendChild(writeTransitions(workflowProcess.Transitions));
			    root.AppendChild(writeLoops(workflowProcess.Loops));
			    root.AppendChild(writeEventListeners(workflowProcess.EventListeners));
			    root.AppendChild(writeExtendedAttributes(workflowProcess.ExtendedAttributes));

			    return root;
			}


		#region 序列化Dictionary＜string, string＞类型数据

		private XmlElement writeEventListeners(List<EventListener> eventListeners)
		{
			if (eventListeners == null || eventListeners.Count <= 0) { return null; }

//			XElement eventListenersElm = new XElement(xN + EVENT_LISTENERS);
//
//			foreach (EventListener listener in eventListeners)
//			{
//				eventListenersElm.Add(
//					new XElement(xN + EVENT_LISTENER, new XAttribute(CLASS_NAME, listener.ClassName))
//				);
//			}
			
			XmlDocument xmldoc = new XmlDocument();
			XmlElement eventListenersElm = xmldoc.CreateElement(EVENT_LISTENERS);
			foreach (EventListener listener in eventListeners)
			{
				XmlElement elm = xmldoc.CreateElement(EVENT_LISTENER);
				elm.SetAttribute(CLASS_NAME, listener.ClassName);
				eventListenersElm.AppendChild(elm);
			}
			return eventListenersElm;
		}

		/// <summary>序列化Dictionary＜string, string＞类型数据</summary>
		/// <param name="extendedAttributes"></param>
		/// <param name="parent"></param>
		private XmlElement writeExtendedAttributes(Dictionary<string, string> extendedAttributes)
		{
			if (extendedAttributes == null || extendedAttributes.Count <= 0)
			{
				return null;
			}

//			XElement extendedAttributesElement = new XElement(xN + EXTENDED_ATTRIBUTES);
//
//			foreach (String key in extendedAttributes.Keys)
//			{
//				extendedAttributesElement.Add(
//					new XElement(xN + EXTENDED_ATTRIBUTE,
//				                  new XAttribute(NAME, key),
//				                  new XAttribute(VALUE, extendedAttributes[key])
//				       ));
//			}
			
			XmlDocument xmldoc = new XmlDocument();
			XmlElement extendedAttributesElement = xmldoc.CreateElement(EXTENDED_ATTRIBUTES);
			foreach (String key in extendedAttributes.Keys)
			{
				XmlElement elm = xmldoc.CreateElement(EXTENDED_ATTRIBUTE);
				elm.SetAttribute(NAME, key);
				elm.SetAttribute(VALUE, extendedAttributes[key]);
				extendedAttributesElement.AppendChild(elm);
			}
			return extendedAttributesElement;
		}
		#endregion

		#region DataField
		private XmlElement writeDataFields(List<DataField> dataFields)
		{
//			if (dataFields == null || dataFields.Count <= 0)
//			{
//				return null;
//			}
//			XElement dataFieldsElement = new XElement(xN + DATA_FIELDS);
//
//			foreach (DataField dataField in dataFields)
//			{
//				dataFieldsElement.Add(
//					new XElement(xN + DATA_FIELD,
//					             new XAttribute(ID, dataField.Id),
//					             new XAttribute(NAME, dataField.Name),
//					             new XAttribute(DISPLAY_NAME, dataField.DisplayName),
//					             new XAttribute(DATA_TYPE, dataField.DataType.ToString()),
//					             new XAttribute(INITIAL_VALUE, dataField.InitialValue),
//					             new XElement(xN + DESCRIPTION, dataField.Description),
//					             writeExtendedAttributes(dataField.ExtendedAttributes)
//					            ));
//			}
			XmlDocument xmldoc = new XmlDocument();
			XmlElement dataFieldsElement =  xmldoc.CreateElement(DATA_FIELDS);
			foreach (DataField dataField in dataFields)
			{
				XmlElement elm = xmldoc.CreateElement(DATA_FIELD);
				elm.SetAttribute(ID, dataField.Id);
				elm.SetAttribute(NAME, dataField.Name);
				elm.SetAttribute(DISPLAY_NAME, dataField.DisplayName);
				elm.SetAttribute(DATA_TYPE, dataField.DataType.ToString());
				elm.SetAttribute(INITIAL_VALUE, dataField.InitialValue);
				XmlElement elm1=xmldoc.CreateElement(DESCRIPTION);
				elm1.InnerText=dataField.Description;
				elm1.AppendChild(writeExtendedAttributes(dataField.ExtendedAttributes));
				elm.AppendChild(elm1);
			}
			return dataFieldsElement;
		}
		#endregion

		#region StartNode
		private XmlElement writeStartNode(StartNode startNode)
		{
			if (startNode == null) { return null; }

//			XmlElement dataFieldsElement = new XmlElement(
//				xN + START_NODE,
//				new XAttribute(ID, startNode.Id),
//				new XAttribute(NAME, startNode.Name),
//				new XAttribute(DISPLAY_NAME, startNode.DisplayName),
//				new XElement(xN + DESCRIPTION, startNode.Description),
//				writeExtendedAttributes(startNode.ExtendedAttributes)
//			);
			XmlDocument xmldoc = new XmlDocument();
			XmlElement dataFieldsElement = xmldoc.CreateElement(START_NODE);
			dataFieldsElement.SetAttribute(ID, startNode.Id);
			dataFieldsElement.SetAttribute(NAME, startNode.Name);
			dataFieldsElement.SetAttribute(DISPLAY_NAME, startNode.DisplayName);
			XmlElement elm= xmldoc.CreateElement(DESCRIPTION);
			elm.InnerText=startNode.Description;
			elm.AppendChild(writeExtendedAttributes(startNode.ExtendedAttributes));
			dataFieldsElement.AppendChild(elm);
			return dataFieldsElement;
		}
		#endregion

		#region Tasks
		private XmlElement writeTasks(List<Task> tasks)
		{
//			XElement tasksElement = new XElement(xN + TASKS);
//
//			foreach (Task item in tasks)
//			{
//				tasksElement.Add(writeTask(item));
//			}
			XmlDocument xmldoc = new XmlDocument();
			XmlElement tasksElement = xmldoc.CreateElement(TASKS);
			foreach (Task item in tasks)
			{
				tasksElement.AppendChild(writeTask(item));
			}
			return tasksElement;
		}

		private XmlElement writeTask(Task task)
		{
//			XElement taskElement = new XElement(
//				xN + TASK,
//				new XAttribute(ID, task.Id),
//				new XAttribute(NAME, task.Name),
//				new XAttribute(DISPLAY_NAME, task.DisplayName),
//				new XAttribute(TYPE, task.TaskType.ToString())//,
//				//new XElement(xN + DESCRIPTION, startNode.Description),
//			);
			XmlDocument xmldoc = new XmlDocument();
			XmlElement taskElement = xmldoc.CreateElement(TASK);
			taskElement.SetAttribute(ID, task.Id);
			taskElement.SetAttribute(NAME, task.Name);
			taskElement.SetAttribute(DISPLAY_NAME, task.DisplayName);
			taskElement.SetAttribute(TYPE, task.TaskType.ToString());

			TaskTypeEnum type = task.TaskType;
			if (task is FormTask)
			{
				taskElement.AppendChild(this.writePerformer(((FormTask)task).Performer));

				taskElement.SetAttribute(COMPLETION_STRATEGY, ((FormTask)task).AssignmentStrategy.ToString());
				taskElement.SetAttribute(DEFAULT_VIEW, ((FormTask)task).DefaultView.ToString());

				taskElement.AppendChild(this.writeForm(EDIT_FORM, ((FormTask)task).EditForm));
				taskElement.AppendChild(this.writeForm(VIEW_FORM, ((FormTask)task).ViewForm));
				taskElement.AppendChild(this.writeForm(LIST_FORM, ((FormTask)task).ListForm));
			}
			else if (task is ToolTask)
			{
				taskElement.AppendChild(this.writeApplication(((ToolTask)task).Application));
				//taskElement.Add(new XAttribute(EXECUTION), ((ToolTask)task).Execution.ToString()));
			}
			else if (task is SubflowTask)
			{
				taskElement.AppendChild(this.writeSubWorkflowProcess(((SubflowTask)task).SubWorkflowProcess));
			}

			taskElement.SetAttribute(PRIORITY, task.Priority.ToString());

			taskElement.AppendChild(writeDuration(task.Duration));

			XmlElement elm_desc = xmldoc.CreateElement(DESCRIPTION);
			elm_desc.InnerText=task.Description;
			taskElement.AppendChild(elm_desc);

			if (!String.IsNullOrEmpty(task.TaskInstanceCreator))
			{
				taskElement.SetAttribute(TASK_INSTANCE_CREATOR, task.TaskInstanceCreator);
			}
			if (!String.IsNullOrEmpty(task.TaskInstanceRunner))
			{
				taskElement.SetAttribute(TASK_INSTANCE_RUNNER, task.TaskInstanceRunner);
			}
			if (!String.IsNullOrEmpty(task.TaskInstanceCompletionEvaluator))
			{
				taskElement.SetAttribute(TASK_INSTANCE_COMPLETION_EVALUATOR, task.TaskInstanceCompletionEvaluator);
			}

			taskElement.SetAttribute(LOOP_STRATEGY, task.LoopStrategy.ToString());

			taskElement.AppendChild(writeEventListeners(task.EventListeners));
			taskElement.AppendChild(writeExtendedAttributes(task.ExtendedAttributes));

			return taskElement;
		}

		private XmlElement writePerformer(Participant participant)
		{
			if (participant == null) { return null; }
//			XElement participantElement = new XElement(
//				xN + PERFORMER,
//				new XAttribute(NAME, participant.Name),
//				new XAttribute(DISPLAY_NAME, participant.DisplayName),
//				new XAttribute(ASSIGNMENT_TYPE, participant.AssignmentType.ToString()), //201004 add lwz 参与者通过业务接口实现默认获取用户
//				new XElement(xN + DESCRIPTION, participant.Description),
//				new XElement(xN + ASSIGNMENT_HANDLER, participant.AssignmentHandler)
//			);
			XmlDocument xmldoc = new XmlDocument();
			XmlElement participantElement = xmldoc.CreateElement(PERFORMER);
			participantElement.SetAttribute(NAME, participant.Name);
			participantElement.SetAttribute(DISPLAY_NAME, participant.DisplayName);
			participantElement.SetAttribute(ASSIGNMENT_TYPE, participant.AssignmentType.ToString()); //201004 add lwz 参与者通过业务接口实现默认获取用户
			XmlElement elm1 = xmldoc.CreateElement(DESCRIPTION);
			elm1.InnerText = participant.Description;
			XmlElement elm2 = xmldoc.CreateElement(ASSIGNMENT_HANDLER);
			elm2.InnerText = participant.AssignmentHandler;
			participantElement.AppendChild(elm1);
			participantElement.AppendChild(elm2);
			return participantElement;
		}

		private XmlElement writeForm(String formName, Form form)
		{
			if (form == null) { return null; }
//			XElement editFormElement = new XElement(
//				xN + formName,
//				new XAttribute(NAME, form.Name),
//				new XAttribute(DISPLAY_NAME, form.DisplayName),
//				new XElement(xN + DESCRIPTION, form.Description),
//				new XElement(xN + URI, form.Uri)
//			);
			XmlDocument xmldoc = new XmlDocument();
			XmlElement editFormElement = xmldoc.CreateElement(formName);
			editFormElement.SetAttribute(NAME, form.Name);
			editFormElement.SetAttribute(DISPLAY_NAME, form.DisplayName);
			XmlElement elm1 = xmldoc.CreateElement(DESCRIPTION);
			elm1.InnerText = form.Description;
			XmlElement elm2 = xmldoc.CreateElement(URI);
			elm2.InnerText = form.Uri;
			editFormElement.AppendChild(elm1);
			editFormElement.AppendChild(elm2);
			return editFormElement;
		}

		private XmlElement writeApplication(Application application)
		{
			if (application == null) { return null; }
//			XElement applicationElement = new XElement(
//				xN + APPLICATION,
//				new XAttribute(NAME, application.Name),
//				new XAttribute(DISPLAY_NAME, application.DisplayName),
//				new XElement(xN + DESCRIPTION, application.Description),
//				new XElement(xN + HANDLER, application.Handler)
//			);
			XmlDocument xmldoc = new XmlDocument();
			XmlElement applicationElement = xmldoc.CreateElement(APPLICATION);
			applicationElement.SetAttribute(NAME, application.Name);
			applicationElement.SetAttribute(DISPLAY_NAME, application.DisplayName);
			XmlElement elm1 = xmldoc.CreateElement(DESCRIPTION);
			elm1.InnerText = application.Description;
			XmlElement elm2 = xmldoc.CreateElement(HANDLER);
			elm2.InnerText = application.Handler;
			applicationElement.AppendChild(elm1);
			applicationElement.AppendChild(elm2);
			return applicationElement;
		}

		private XmlElement writeSubWorkflowProcess(SubWorkflowProcess subWorkflowProcess)
		{
			if (subWorkflowProcess == null) { return null; }
//			XElement subflowElement = new XElement(
//				xN + SUB_WORKFLOW_PROCESS,
//				new XAttribute(NAME, subWorkflowProcess.Name),
//				new XAttribute(DISPLAY_NAME, subWorkflowProcess.DisplayName),
//				new XElement(xN + DESCRIPTION, subWorkflowProcess.Description),
//				new XElement(xN + WORKFLOW_PROCESS_ID, subWorkflowProcess.WorkflowProcessId)
//			);
			XmlDocument xmldoc = new XmlDocument();
			XmlElement subflowElement = xmldoc.CreateElement(SUB_WORKFLOW_PROCESS);
			subflowElement.SetAttribute(NAME, subWorkflowProcess.Name);
			subflowElement.SetAttribute(DISPLAY_NAME, subWorkflowProcess.DisplayName);
			XmlElement elm1 = xmldoc.CreateElement(DESCRIPTION);
			elm1.InnerText = subWorkflowProcess.Description;
			XmlElement elm2 = xmldoc.CreateElement(WORKFLOW_PROCESS_ID);
			elm2.InnerText = subWorkflowProcess.WorkflowProcessId;
			subflowElement.AppendChild(elm1);
			subflowElement.AppendChild(elm2);
			return subflowElement;
		}

		private XmlElement writeDuration(Duration duration)
		{
			if (duration == null) { return null; }

//			XElement durationElement = new XElement(
//				xN + DURATION,
//				new XAttribute(VALUE, duration.Value.ToString()),
//				new XAttribute(UNIT, duration.Unit.ToString()),
//				new XAttribute(IS_BUSINESS_TIME, duration.IsBusinessTime.ToString())
//			);
			XmlDocument xmldoc = new XmlDocument();
			XmlElement durationElement = xmldoc.CreateElement(DURATION);
			durationElement.SetAttribute(VALUE, duration.Value.ToString());
			durationElement.SetAttribute(UNIT, duration.Unit.ToString());
			durationElement.SetAttribute(IS_BUSINESS_TIME, duration.IsBusinessTime.ToString());
			return durationElement;
		}
		#endregion

		#region Activitie
		private XmlElement writeActivities(List<Activity> activities)
		{
			if (activities == null || activities.Count <= 0) { return null; }

			//XElement activitiesElement = new XElement(xN + ACTIVITIES);
			XmlDocument xmldoc = new XmlDocument();
			XmlElement activitiesElement = xmldoc.CreateElement(ACTIVITIES);
			foreach (Activity item in activities)
			{
				activitiesElement.AppendChild(writeActivity(item));
			}
			return activitiesElement;
		}

		private XmlElement writeActivity(Activity activity)
		{
			if (activity == null) { return null; }

//			XElement activityElement = new XElement(
//				xN + ACTIVITY,
//				new XAttribute(ID, activity.Id),
//				new XAttribute(NAME, activity.Name),
//				new XAttribute(DISPLAY_NAME, activity.DisplayName),
//				new XAttribute(COMPLETION_STRATEGY, activity.CompletionStrategy.ToString()),
//				new XElement(xN + DESCRIPTION, activity.Description),
//				writeEventListeners(activity.EventListeners),
//				writeExtendedAttributes(activity.ExtendedAttributes),
//				writeTasks(activity.InlineTasks),
//				writeTaskRefs(activity.TaskRefs)
//			);
			
			XmlDocument xmldoc = new XmlDocument();
			XmlElement activityElement = xmldoc.CreateElement(ACTIVITY);
			activityElement.SetAttribute(ID, activity.Id);
			activityElement.SetAttribute(NAME, activity.Name);
			activityElement.SetAttribute(DISPLAY_NAME, activity.DisplayName);
			activityElement.SetAttribute(COMPLETION_STRATEGY, activity.CompletionStrategy.ToString());
			XmlElement elm = xmldoc.CreateElement( DESCRIPTION);
			elm.InnerText = activity.Description;
			activityElement.AppendChild(elm);
			activityElement.AppendChild(writeEventListeners(activity.EventListeners));
			activityElement.AppendChild(writeExtendedAttributes(activity.ExtendedAttributes));
			activityElement.AppendChild(writeTasks(activity.InlineTasks));
			activityElement.AppendChild(writeTaskRefs(activity.TaskRefs));
			return activityElement;
		}

		private XmlElement writeTaskRefs(List<TaskRef> taskRefs)
		{
//			XElement taskRefsElement = new XElement(xN + TASKREFS);
//
//			foreach (TaskRef taskRef in taskRefs)
//			{
//				taskRefsElement.Add(new XElement(xN + TASKREF, new XAttribute(REFERENCE, taskRef.ReferencedTask.Id)));
//			}
			XmlDocument xmldoc = new XmlDocument();
			XmlElement taskRefsElement = xmldoc.CreateElement(TASKREFS);
			foreach (TaskRef taskRef in taskRefs) {
				XmlElement elm = xmldoc.CreateElement(TASKREF);
				elm.SetAttribute (REFERENCE,taskRef.ReferencedTask.Id);
				taskRefsElement.AppendChild(elm);
			}
			return taskRefsElement;
		}
		#endregion

		#region Synchronizer
		private XmlElement writeSynchronizers(List<Synchronizer> synchronizers)
		{
			if (synchronizers == null || synchronizers.Count <= 0) { return null; }
//			XElement synchronizersElement = new XElement(xN + SYNCHRONIZERS);
//
//			foreach (Synchronizer item in synchronizers)
//			{
//				synchronizersElement.Add(writeSynchronizer(item));
//			}
			XmlDocument xmldoc = new XmlDocument();
			XmlElement synchronizersElement = xmldoc.CreateElement(SYNCHRONIZERS);
			foreach(Synchronizer item in synchronizers){
				synchronizersElement.AppendChild(writeSynchronizer(item));
			}
			return synchronizersElement;
		}

		private XmlElement writeSynchronizer(Synchronizer synchronizer)
		{
			if (synchronizer == null) { return null; }

//			XElement synchronizerElement = new XElement(
//				xN + SYNCHRONIZER,
//				new XAttribute(ID, synchronizer.Id),
//				new XAttribute(NAME, synchronizer.Name),
//				new XAttribute(DISPLAY_NAME, synchronizer.DisplayName),
//				new XElement(xN + DESCRIPTION, synchronizer.Description),
//				writeExtendedAttributes(synchronizer.ExtendedAttributes)
//			);
			XmlDocument xmldoc = new XmlDocument();
			XmlElement synchronizerElement = xmldoc.CreateElement(SYNCHRONIZER);
			synchronizerElement.SetAttribute(ID, synchronizer.Id);
			synchronizerElement.SetAttribute(NAME, synchronizer.Name);
			synchronizerElement.SetAttribute(DISPLAY_NAME, synchronizer.DisplayName);
			XmlElement elm = xmldoc.CreateElement(DESCRIPTION);
			elm.InnerText = synchronizer.Description;
			synchronizerElement.AppendChild(elm);
			synchronizerElement.AppendChild(writeExtendedAttributes(synchronizer.ExtendedAttributes));
			
			return synchronizerElement;
		}
		#endregion

		#region EndNode
		private XmlElement writeEndNodes(List<EndNode> endNodes)
		{
			if (endNodes == null || endNodes.Count <= 0) { return null; }
//			XElement endNodesElement = new XElement(xN + END_NODES);
//
//			foreach (EndNode item in endNodes)
//			{
//				endNodesElement.Add(writeEndNode(item));
//			}
			XmlDocument xmldoc = new XmlDocument();
			XmlElement endNodesElement = xmldoc.CreateElement(END_NODES);
			foreach (EndNode item in endNodes) {
				endNodesElement.AppendChild(writeEndNode(item));
			}
			return endNodesElement;
		}

		private XmlElement writeEndNode(EndNode endNode)
		{
			if (endNode == null) { return null; }

//			XElement endNodeElement = new XElement(
//				xN + END_NODE,
//				new XAttribute(ID, endNode.Id),
//				new XAttribute(NAME, endNode.Name),
//				new XAttribute(DISPLAY_NAME, endNode.DisplayName),
//				new XElement(xN + DESCRIPTION, endNode.Description),
//				writeExtendedAttributes(endNode.ExtendedAttributes)
//			);
			XmlDocument xmldoc = new XmlDocument();
			XmlElement endNodeElement = xmldoc.CreateElement(END_NODE);
			endNodeElement.SetAttribute(ID, endNode.Id);
			endNodeElement.SetAttribute(NAME, endNode.Name);
			endNodeElement.SetAttribute(DISPLAY_NAME, endNode.DisplayName);
			XmlElement elm = xmldoc.CreateElement(DESCRIPTION);
			elm.InnerText = endNode.Description;
			endNodeElement.AppendChild(elm);
			endNodeElement.AppendChild(writeExtendedAttributes(endNode.ExtendedAttributes));
			return endNodeElement;
		}
		#endregion

		#region Transitions
		private XmlElement writeTransitions(List<Transition> transitions)
		{
			if (transitions == null || transitions.Count <= 0) { return null; }

//			XElement transitionsElement = new XElement(xN + TRANSITIONS);
//
//			foreach (Transition item in transitions)
//			{
//				transitionsElement.Add(writeTransition(item));
//			}
			XmlDocument xmldoc = new XmlDocument();
			XmlElement transitionsElement = xmldoc.CreateElement(TRANSITIONS);
			foreach(Transition item in transitions){
				transitionsElement.AppendChild(writeTransition(item));
			}
			return transitionsElement;
		}

		private XmlElement writeTransition(Transition transition)
		{
			if (transition == null) { return null; }

//			XElement transitionElement = new XElement(
//				xN + TRANSITION,
//				new XAttribute(ID, transition.Id),
//				new XAttribute(FROM, transition.FromNode.Id),
//				new XAttribute(TO, transition.ToNode.Id),
//				new XAttribute(NAME, transition.Name),
//				new XAttribute(DISPLAY_NAME, transition.DisplayName),
//				new XElement(xN + CONDITION, transition.Condition),
//				writeExtendedAttributes(transition.ExtendedAttributes)
//			);
			
			XmlDocument xmldoc  =new XmlDocument();
			XmlElement transitionElement = xmldoc.CreateElement(TRANSITION);
			transitionElement.SetAttribute(ID, transition.Id);
			transitionElement.SetAttribute(FROM, transition.FromNode.Id);
			transitionElement.SetAttribute(TO, transition.ToNode.Id);
			transitionElement.SetAttribute(NAME, transition.Name);
			transitionElement.SetAttribute(DISPLAY_NAME, transition.DisplayName);
			XmlElement elm = xmldoc.CreateElement(CONDITION);
			elm.InnerText = transition.Condition;
			transitionElement.AppendChild(elm);
			transitionElement.AppendChild(writeExtendedAttributes(transition.ExtendedAttributes));
			return transitionElement;
		}
		#endregion

		#region Loops
		private XmlElement writeLoops(List<Loop> loops)
		{
			if (loops == null || loops.Count <= 0) { return null; }
//			XElement transitionsElement = new XElement(xN + LOOPS);
//
//			foreach (Loop loop in loops)
//			{
//				transitionsElement.Add(new XElement(
//					xN + LOOP,
//					new XAttribute(ID, loop.Id),
//					new XAttribute(FROM, loop.FromNode.Id),
//					new XAttribute(TO, loop.ToNode.Id),
//					new XAttribute(NAME, loop.Name),
//					new XAttribute(DISPLAY_NAME, loop.DisplayName),
//					new XElement(xN + CONDITION, loop.Condition),
//					writeExtendedAttributes(loop.ExtendedAttributes)
//				));
//			}
			XmlDocument xmldoc = new XmlDocument();
			XmlElement transitionsElement = xmldoc.CreateElement(LOOPS);
			foreach (Loop loop in loops)
			{
				XmlElement transitionElement = xmldoc.CreateElement(LOOP);
				transitionElement.SetAttribute(ID, loop.Id);
				transitionElement.SetAttribute(FROM, loop.FromNode.Id);
				transitionElement.SetAttribute(TO, loop.ToNode.Id);
				transitionElement.SetAttribute(NAME, loop.Name);
				transitionElement.SetAttribute(DISPLAY_NAME, loop.DisplayName);
				XmlElement elm = xmldoc.CreateElement(CONDITION);
				elm.InnerText = loop.Condition;
				transitionsElement.AppendChild(elm);
				transitionsElement.AppendChild(writeExtendedAttributes(loop.ExtendedAttributes));
			}
			return transitionsElement;
		}
		#endregion

	}
}
