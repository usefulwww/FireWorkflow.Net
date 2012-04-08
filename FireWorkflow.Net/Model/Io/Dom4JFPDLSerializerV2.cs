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
			//XmlDocument inventoryDoc = new XmlDocument();
			
			
			//XmlElement root = xmldoc.DocumentElement;
			//XmlElement root = xmldoc.CreateElement(FPDL_NS_PREFIX,WORKFLOW_PROCESS,FPDL_URI);
			//xmldoc.InsertBefore(xmldoc.CreateXmlDeclaration("1.0", "utf-8", "yes"),root);
			xmldoc.AppendChild(xmldoc.CreateXmlDeclaration("1.0", "utf-8", "yes"));
			var dtype = xmldoc.CreateDocumentType(FPDL_NS_PREFIX+":"+WORKFLOW_PROCESS,PUBLIC_ID,SYSTEM_ID,string.Empty);
			xmldoc.AppendChild(dtype);
			workflowProcessToDom(xmldoc,workflowProcess);
			
			XmlWriter writer = XmlWriter.Create(swout);
			if (writer != null)
			{
				xmldoc.Save(writer);
				writer.Close();
			}
			swout.Position = 0;
		}

		public string serialize(WorkflowProcess workflowProcess)
		{
			//XDocument inventoryDoc =new XDocument(new XDeclaration("1.0", "utf-8", "yes"),workflowProcessToDom(workflowProcess));
			//XmlDocument inventoryDoc = new XmlDocument();
			
			//XmlElement root = xmldoc.DocumentElement;
			//XmlElement root = xmldoc.CreateElement(FPDL_NS_PREFIX,WORKFLOW_PROCESS,FPDL_URI);
			//xmldoc.InsertBefore(xmldoc.CreateXmlDeclaration("1.0", "utf-8", "yes"),root);
			xmldoc.AppendChild(xmldoc.CreateXmlDeclaration("1.0", "utf-8", "yes"));
			var dtype = xmldoc.CreateDocumentType(FPDL_NS_PREFIX+":"+WORKFLOW_PROCESS,PUBLIC_ID,SYSTEM_ID,string.Empty);
			xmldoc.AppendChild(dtype);
			workflowProcessToDom(xmldoc,workflowProcess);
			
			return xmldoc.OuterXml;
		}

		XmlDocument xmldoc = new XmlDocument();
		
		private XmlElement CreateElement(string name )
		{
			XmlElement el = xmldoc.CreateElement(FPDL_NS_PREFIX,name,FPDL_URI);
			return el;
		}
		
		private void workflowProcessToDom(XmlDocument xdoc,WorkflowProcess workflowProcess)
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
			
			

			XmlElement root = CreateElement(WORKFLOW_PROCESS);
			root.SetAttribute(ID,workflowProcess.Id);
			root.SetAttribute(NAME,workflowProcess.Name);
			root.SetAttribute(DISPLAY_NAME,workflowProcess.DisplayName);
			root.SetAttribute(RESOURCE_FILE,workflowProcess.ResourceFile);
			root.SetAttribute(RESOURCE_MANAGER,workflowProcess.ResourceManager);
			XmlElement desc = CreateElement(DESCRIPTION);
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
			writeDataFields(root,workflowProcess.DataFields);
			writeStartNode(root,workflowProcess.StartNode);
			writeTasks(root,workflowProcess.Tasks);
			writeActivities(root,workflowProcess.Activities);
			writeSynchronizers(root,workflowProcess.Synchronizers);
			writeEndNodes(root,workflowProcess.EndNodes);
			writeTransitions(root,workflowProcess.Transitions);
			writeLoops(root,workflowProcess.Loops);
			writeEventListeners(root,workflowProcess.EventListeners);
			writeExtendedAttributes(root,workflowProcess.ExtendedAttributes);

			xdoc.AppendChild( root);
		}


		#region 序列化Dictionary＜string, string＞类型数据

		private void writeEventListeners(XmlElement parent,List<EventListener> eventListeners)
		{
			if (eventListeners == null || eventListeners.Count <= 0) { return ; }

//			XElement eventListenersElm = new XElement(xN + EVENT_LISTENERS);
//
//			foreach (EventListener listener in eventListeners)
//			{
//				eventListenersElm.Add(
//					new XElement(xN + EVENT_LISTENER, new XAttribute(CLASS_NAME, listener.ClassName))
//				);
//			}
			
			//XmlDocument xmldoc = new XmlDocument();
			XmlElement eventListenersElm = CreateElement(EVENT_LISTENERS);
			foreach (EventListener listener in eventListeners)
			{
				XmlElement elm = CreateElement(EVENT_LISTENER);
				elm.SetAttribute(CLASS_NAME, listener.ClassName);
				eventListenersElm.AppendChild(elm);
			}
			parent.AppendChild( eventListenersElm);
		}

		/// <summary>序列化Dictionary＜string, string＞类型数据</summary>
		/// <param name="extendedAttributes"></param>
		/// <param name="parent"></param>
		private void writeExtendedAttributes(XmlElement parent,Dictionary<string, string> extendedAttributes)
		{
			if (extendedAttributes == null || extendedAttributes.Count <= 0)
			{
				return ;
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
			
			//XmlDocument xmldoc = new XmlDocument();
			XmlElement extendedAttributesElement = CreateElement(EXTENDED_ATTRIBUTES);
			foreach (String key in extendedAttributes.Keys)
			{
				XmlElement elm = CreateElement(EXTENDED_ATTRIBUTE);
				elm.SetAttribute(NAME, key);
				elm.SetAttribute(VALUE, extendedAttributes[key]);
				extendedAttributesElement.AppendChild(elm);
			}
			parent.AppendChild( extendedAttributesElement);
		}
		#endregion

		#region DataField
		private void writeDataFields(XmlElement parent,List<DataField> dataFields)
		{
			if (dataFields == null || dataFields.Count <= 0)
			{
				return ;
			}
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
			//XmlDocument xmldoc = new XmlDocument();
			XmlElement dataFieldsElement =  CreateElement(DATA_FIELDS);
			foreach (DataField dataField in dataFields)
			{
				XmlElement elm = CreateElement(DATA_FIELD);
				elm.SetAttribute(ID, dataField.Id);
				elm.SetAttribute(NAME, dataField.Name);
				elm.SetAttribute(DISPLAY_NAME, dataField.DisplayName);
				elm.SetAttribute(DATA_TYPE, dataField.DataType.ToString());
				elm.SetAttribute(INITIAL_VALUE, dataField.InitialValue);
				XmlElement elm1=CreateElement(DESCRIPTION);
				elm1.InnerText=dataField.Description;
				writeExtendedAttributes(elm1,dataField.ExtendedAttributes);
				elm.AppendChild(elm1);
			}
			parent.AppendChild( dataFieldsElement);
		}
		#endregion

		#region StartNode
		private void writeStartNode(XmlElement parent,StartNode startNode)
		{
			if (startNode == null) { return ; }

//			XmlElement dataFieldsElement = new XmlElement(
//				xN + START_NODE,
//				new XAttribute(ID, startNode.Id),
//				new XAttribute(NAME, startNode.Name),
//				new XAttribute(DISPLAY_NAME, startNode.DisplayName),
//				new XElement(xN + DESCRIPTION, startNode.Description),
//				writeExtendedAttributes(startNode.ExtendedAttributes)
//			);
			//XmlDocument xmldoc = new XmlDocument();
			XmlElement dataFieldsElement = CreateElement(START_NODE);
			dataFieldsElement.SetAttribute(ID, startNode.Id);
			dataFieldsElement.SetAttribute(NAME, startNode.Name);
			dataFieldsElement.SetAttribute(DISPLAY_NAME, startNode.DisplayName);
			XmlElement elm= CreateElement(DESCRIPTION);
			elm.InnerText=startNode.Description;
			writeExtendedAttributes(elm,startNode.ExtendedAttributes);
			dataFieldsElement.AppendChild(elm);
			parent.AppendChild( dataFieldsElement);
		}
		#endregion

		#region Tasks
		private void writeTasks(XmlElement parent,List<Task> tasks)
		{
			if(tasks==null|| tasks.Count<1)
				return ;
//			XElement tasksElement = new XElement(xN + TASKS);
//
//			foreach (Task item in tasks)
//			{
//				tasksElement.Add(writeTask(item));
//			}
			//XmlDocument xmldoc = new XmlDocument();
			XmlElement tasksElement = CreateElement(TASKS);
			foreach (Task item in tasks)
			{
				writeTask(tasksElement,item);
			}
			parent.AppendChild( tasksElement);
		}

		private void writeTask(XmlElement parent,Task task)
		{
			if(task==null)
				return;
//			XElement taskElement = new XElement(
//				xN + TASK,
//				new XAttribute(ID, task.Id),
//				new XAttribute(NAME, task.Name),
//				new XAttribute(DISPLAY_NAME, task.DisplayName),
//				new XAttribute(TYPE, task.TaskType.ToString())//,
//				//new XElement(xN + DESCRIPTION, startNode.Description),
//			);
			//XmlDocument xmldoc = new XmlDocument();
			XmlElement taskElement = CreateElement(TASK);
			taskElement.SetAttribute(ID, task.Id);
			taskElement.SetAttribute(NAME, task.Name);
			taskElement.SetAttribute(DISPLAY_NAME, task.DisplayName);
			taskElement.SetAttribute(TYPE, task.TaskType.ToString());

			TaskTypeEnum type = task.TaskType;
			if (task is FormTask)
			{
				writePerformer(taskElement,((FormTask)task).Performer);
				

				taskElement.SetAttribute(COMPLETION_STRATEGY, ((FormTask)task).AssignmentStrategy.ToString());
				taskElement.SetAttribute(DEFAULT_VIEW, ((FormTask)task).DefaultView.ToString());

				writeForm(taskElement,EDIT_FORM, ((FormTask)task).EditForm);
				writeForm(taskElement,VIEW_FORM, ((FormTask)task).ViewForm);
				writeForm(taskElement,LIST_FORM, ((FormTask)task).ListForm);
				
			}
			else if (task is ToolTask)
			{
				writeApplication(taskElement,((ToolTask)task).Application);
				//taskElement.Add(new XAttribute(EXECUTION), ((ToolTask)task).Execution.ToString()));
			}
			else if (task is SubflowTask)
			{
				writeSubWorkflowProcess(taskElement,((SubflowTask)task).SubWorkflowProcess);
			}

			taskElement.SetAttribute(PRIORITY, task.Priority.ToString());

			writeDuration(taskElement,task.Duration);

			XmlElement elm_desc = CreateElement(DESCRIPTION);
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

			writeEventListeners(taskElement,task.EventListeners);
			writeExtendedAttributes(taskElement,task.ExtendedAttributes);
			
			parent.AppendChild( taskElement);
		}

		private void writePerformer(XmlElement parent,Participant participant)
		{
			if (participant == null) { return; }
//			XElement participantElement = new XElement(
//				xN + PERFORMER,
//				new XAttribute(NAME, participant.Name),
//				new XAttribute(DISPLAY_NAME, participant.DisplayName),
//				new XAttribute(ASSIGNMENT_TYPE, participant.AssignmentType.ToString()), //201004 add lwz 参与者通过业务接口实现默认获取用户
//				new XElement(xN + DESCRIPTION, participant.Description),
//				new XElement(xN + ASSIGNMENT_HANDLER, participant.AssignmentHandler)
//			);
			//XmlDocument xmldoc = new XmlDocument();
			XmlElement participantElement = CreateElement(PERFORMER);
			participantElement.SetAttribute(NAME, participant.Name);
			participantElement.SetAttribute(DISPLAY_NAME, participant.DisplayName);
			participantElement.SetAttribute(ASSIGNMENT_TYPE, participant.AssignmentType.ToString()); //201004 add lwz 参与者通过业务接口实现默认获取用户
			XmlElement elm1 = CreateElement(DESCRIPTION);
			elm1.InnerText = participant.Description;
			XmlElement elm2 = CreateElement(ASSIGNMENT_HANDLER);
			elm2.InnerText = participant.AssignmentHandler;
			participantElement.AppendChild(elm1);
			participantElement.AppendChild(elm2);
			parent.AppendChild( participantElement);
		}

		private void writeForm(XmlElement parent,String formName, Form form)
		{
			if (form == null) { return ; }
//			XElement editFormElement = new XElement(
//				xN + formName,
//				new XAttribute(NAME, form.Name),
//				new XAttribute(DISPLAY_NAME, form.DisplayName),
//				new XElement(xN + DESCRIPTION, form.Description),
//				new XElement(xN + URI, form.Uri)
//			);
			//XmlDocument xmldoc = new XmlDocument();
			XmlElement editFormElement = CreateElement(formName);
			editFormElement.SetAttribute(NAME, form.Name);
			editFormElement.SetAttribute(DISPLAY_NAME, form.DisplayName);
			XmlElement elm1 = CreateElement(DESCRIPTION);
			elm1.InnerText = form.Description;
			XmlElement elm2 = CreateElement(URI);
			elm2.InnerText = form.Uri;
			editFormElement.AppendChild(elm1);
			editFormElement.AppendChild(elm2);
			parent.AppendChild( editFormElement);
		}

		private void writeApplication(XmlElement parent,Application application)
		{
			if (application == null) { return ; }
//			XElement applicationElement = new XElement(
//				xN + APPLICATION,
//				new XAttribute(NAME, application.Name),
//				new XAttribute(DISPLAY_NAME, application.DisplayName),
//				new XElement(xN + DESCRIPTION, application.Description),
//				new XElement(xN + HANDLER, application.Handler)
//			);
			//XmlDocument xmldoc = new XmlDocument();
			XmlElement applicationElement = CreateElement(APPLICATION);
			applicationElement.SetAttribute(NAME, application.Name);
			applicationElement.SetAttribute(DISPLAY_NAME, application.DisplayName);
			XmlElement elm1 = CreateElement(DESCRIPTION);
			elm1.InnerText = application.Description;
			XmlElement elm2 = CreateElement(HANDLER);
			elm2.InnerText = application.Handler;
			applicationElement.AppendChild(elm1);
			applicationElement.AppendChild(elm2);
			parent.AppendChild( applicationElement);
		}

		private void writeSubWorkflowProcess(XmlElement parent,SubWorkflowProcess subWorkflowProcess)
		{
			if (subWorkflowProcess == null) { return ; }
//			XElement subflowElement = new XElement(
//				xN + SUB_WORKFLOW_PROCESS,
//				new XAttribute(NAME, subWorkflowProcess.Name),
//				new XAttribute(DISPLAY_NAME, subWorkflowProcess.DisplayName),
//				new XElement(xN + DESCRIPTION, subWorkflowProcess.Description),
//				new XElement(xN + WORKFLOW_PROCESS_ID, subWorkflowProcess.WorkflowProcessId)
//			);
			//XmlDocument xmldoc = new XmlDocument();
			XmlElement subflowElement = CreateElement(SUB_WORKFLOW_PROCESS);
			subflowElement.SetAttribute(NAME, subWorkflowProcess.Name);
			subflowElement.SetAttribute(DISPLAY_NAME, subWorkflowProcess.DisplayName);
			XmlElement elm1 = CreateElement(DESCRIPTION);
			elm1.InnerText = subWorkflowProcess.Description;
			XmlElement elm2 = CreateElement(WORKFLOW_PROCESS_ID);
			elm2.InnerText = subWorkflowProcess.WorkflowProcessId;
			subflowElement.AppendChild(elm1);
			subflowElement.AppendChild(elm2);
			parent.AppendChild( subflowElement);
		}

		private void writeDuration(XmlElement parent,Duration duration)
		{
			if (duration == null) { return ; }

//			XElement durationElement = new XElement(
//				xN + DURATION,
//				new XAttribute(VALUE, duration.Value.ToString()),
//				new XAttribute(UNIT, duration.Unit.ToString()),
//				new XAttribute(IS_BUSINESS_TIME, duration.IsBusinessTime.ToString())
//			);
			//XmlDocument xmldoc = new XmlDocument();
			XmlElement durationElement = CreateElement(DURATION);
			durationElement.SetAttribute(VALUE, duration.Value.ToString());
			durationElement.SetAttribute(UNIT, duration.Unit.ToString());
			durationElement.SetAttribute(IS_BUSINESS_TIME, duration.IsBusinessTime.ToString());
			parent.AppendChild( durationElement);
		}
		#endregion

		#region Activitie
		private void writeActivities(XmlElement parent,List<Activity> activities)
		{
			if (activities == null || activities.Count <= 0) { return ; }

			//XElement activitiesElement = new XElement(xN + ACTIVITIES);
			//XmlDocument xmldoc = new XmlDocument();
			XmlElement activitiesElement = CreateElement(ACTIVITIES);
			foreach (Activity item in activities)
			{
				writeActivity(activitiesElement,item);
			}
			parent.AppendChild(activitiesElement);
			//return activitiesElement;
		}

		private void writeActivity(XmlElement parent,Activity activity)
		{
			if (activity == null) { return ; }

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
			
			//XmlDocument xmldoc = new XmlDocument();
			XmlElement activityElement = CreateElement(ACTIVITY);
			activityElement.SetAttribute(ID, activity.Id);
			activityElement.SetAttribute(NAME, activity.Name);
			activityElement.SetAttribute(DISPLAY_NAME, activity.DisplayName);
			activityElement.SetAttribute(COMPLETION_STRATEGY, activity.CompletionStrategy.ToString());
			XmlElement elm = CreateElement( DESCRIPTION);
			elm.InnerText = activity.Description;
			activityElement.AppendChild(elm);
			writeEventListeners(activityElement,activity.EventListeners);
			writeExtendedAttributes(activityElement,activity.ExtendedAttributes);
			writeTasks(activityElement,activity.InlineTasks);
			writeTaskRefs(activityElement,activity.TaskRefs);
			parent.AppendChild(activityElement);
		}

		private void writeTaskRefs(XmlElement parent,List<TaskRef> taskRefs)
		{
			if(taskRefs==null || taskRefs.Count<1)
				return ;
//			XElement taskRefsElement = new XElement(xN + TASKREFS);
//
//			foreach (TaskRef taskRef in taskRefs)
//			{
//				taskRefsElement.Add(new XElement(xN + TASKREF, new XAttribute(REFERENCE, taskRef.ReferencedTask.Id)));
//			}
			//XmlDocument xmldoc = new XmlDocument();
			XmlElement taskRefsElement = CreateElement(TASKREFS);
			foreach (TaskRef taskRef in taskRefs) {
				XmlElement elm = CreateElement(TASKREF);
				elm.SetAttribute (REFERENCE,taskRef.ReferencedTask.Id);
				taskRefsElement.AppendChild(elm);
			}
			parent.AppendChild( taskRefsElement);
		}
		#endregion

		#region Synchronizer
		private void writeSynchronizers(XmlElement parent,List<Synchronizer> synchronizers)
		{
			if (synchronizers == null || synchronizers.Count <= 0) { return ; }
//			XElement synchronizersElement = new XElement(xN + SYNCHRONIZERS);
//
//			foreach (Synchronizer item in synchronizers)
//			{
//				synchronizersElement.Add(writeSynchronizer(item));
//			}
			//XmlDocument xmldoc = new XmlDocument();
			XmlElement synchronizersElement = CreateElement(SYNCHRONIZERS);
			foreach(Synchronizer item in synchronizers){
				writeSynchronizer(synchronizersElement,item);
			}
			parent.AppendChild( synchronizersElement);
		}

		private void writeSynchronizer(XmlElement parent,Synchronizer synchronizer)
		{
			if (synchronizer == null) { return ; }

//			XElement synchronizerElement = new XElement(
//				xN + SYNCHRONIZER,
//				new XAttribute(ID, synchronizer.Id),
//				new XAttribute(NAME, synchronizer.Name),
//				new XAttribute(DISPLAY_NAME, synchronizer.DisplayName),
//				new XElement(xN + DESCRIPTION, synchronizer.Description),
//				writeExtendedAttributes(synchronizer.ExtendedAttributes)
//			);
			//XmlDocument xmldoc = new XmlDocument();
			XmlElement synchronizerElement = CreateElement(SYNCHRONIZER);
			synchronizerElement.SetAttribute(ID, synchronizer.Id);
			synchronizerElement.SetAttribute(NAME, synchronizer.Name);
			synchronizerElement.SetAttribute(DISPLAY_NAME, synchronizer.DisplayName);
			XmlElement elm = CreateElement(DESCRIPTION);
			elm.InnerText = synchronizer.Description;
			synchronizerElement.AppendChild(elm);
			writeExtendedAttributes(synchronizerElement,synchronizer.ExtendedAttributes);
			
			parent.AppendChild( synchronizerElement);
		}
		#endregion

		#region EndNode
		private void writeEndNodes(XmlElement parent,List<EndNode> endNodes)
		{
			if (endNodes == null || endNodes.Count <= 0) { return ; }
//			XElement endNodesElement = new XElement(xN + END_NODES);
//
//			foreach (EndNode item in endNodes)
//			{
//				endNodesElement.Add(writeEndNode(item));
//			}
			//XmlDocument xmldoc = new XmlDocument();
			XmlElement endNodesElement = CreateElement(END_NODES);
			foreach (EndNode item in endNodes) {
				writeEndNode(endNodesElement,item);
			}
			parent.AppendChild( endNodesElement);
		}

		private void writeEndNode(XmlElement parent,EndNode endNode)
		{
			if (endNode == null) { return ; }

//			XElement endNodeElement = new XElement(
//				xN + END_NODE,
//				new XAttribute(ID, endNode.Id),
//				new XAttribute(NAME, endNode.Name),
//				new XAttribute(DISPLAY_NAME, endNode.DisplayName),
//				new XElement(xN + DESCRIPTION, endNode.Description),
//				writeExtendedAttributes(endNode.ExtendedAttributes)
//			);
			//XmlDocument xmldoc = new XmlDocument();
			XmlElement endNodeElement = CreateElement(END_NODE);
			endNodeElement.SetAttribute(ID, endNode.Id);
			endNodeElement.SetAttribute(NAME, endNode.Name);
			endNodeElement.SetAttribute(DISPLAY_NAME, endNode.DisplayName);
			XmlElement elm = CreateElement(DESCRIPTION);
			elm.InnerText = endNode.Description;
			endNodeElement.AppendChild(elm);
			writeExtendedAttributes(endNodeElement,endNode.ExtendedAttributes);
			
			parent.AppendChild( endNodeElement);
		}
		#endregion

		#region Transitions
		private void writeTransitions(XmlElement parent,List<Transition> transitions)
		{
			if (transitions == null || transitions.Count <= 0) { return ; }

//			XElement transitionsElement = new XElement(xN + TRANSITIONS);
//
//			foreach (Transition item in transitions)
//			{
//				transitionsElement.Add(writeTransition(item));
//			}
			//XmlDocument xmldoc = new XmlDocument();
			XmlElement transitionsElement = CreateElement(TRANSITIONS);
			foreach(Transition item in transitions){
				writeTransition(transitionsElement,item);
			}
			parent.AppendChild( transitionsElement);
		}

		private void writeTransition(XmlElement parent,Transition transition)
		{
			if (transition == null) { return ; }

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
			XmlElement transitionElement = CreateElement(TRANSITION);
			transitionElement.SetAttribute(ID, transition.Id);
			transitionElement.SetAttribute(FROM, transition.FromNode.Id);
			transitionElement.SetAttribute(TO, transition.ToNode.Id);
			transitionElement.SetAttribute(NAME, transition.Name);
			transitionElement.SetAttribute(DISPLAY_NAME, transition.DisplayName);
			XmlElement elm = CreateElement(CONDITION);
			elm.InnerText = transition.Condition;
			transitionElement.AppendChild(elm);
			writeExtendedAttributes(transitionElement,transition.ExtendedAttributes);
			
			parent.AppendChild( transitionElement);
		}
		#endregion

		#region Loops
		private void writeLoops(XmlElement parent,List<Loop> loops)
		{
			if (loops == null || loops.Count <= 0) { return ; }
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
			//XmlDocument xmldoc = new XmlDocument();
			XmlElement transitionsElement = CreateElement(LOOPS);
			foreach (Loop loop in loops)
			{
				XmlElement transitionElement = CreateElement(LOOP);
				transitionElement.SetAttribute(ID, loop.Id);
				transitionElement.SetAttribute(FROM, loop.FromNode.Id);
				transitionElement.SetAttribute(TO, loop.ToNode.Id);
				transitionElement.SetAttribute(NAME, loop.Name);
				transitionElement.SetAttribute(DISPLAY_NAME, loop.DisplayName);
				XmlElement elm = CreateElement(CONDITION);
				elm.InnerText = loop.Condition;
				transitionsElement.AppendChild(elm);
				writeExtendedAttributes(transitionElement,loop.ExtendedAttributes);
				
			}
			parent.AppendChild( transitionsElement);
		}
		#endregion

	}
}
