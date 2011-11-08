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
 @Revision to .NET 无忧 lwz0721@gmail.com  2010-02
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using System.Xml.Serialization;
using System.Text;
using FireWorkflow.Net.Model;
using FireWorkflow.Net.Model.Net;
using FireWorkflow.Net.Model.Resource;

namespace FireWorkflow.Net.Model.Io
{
	/// <summary>
	/// FPDL解析器，将一个xml格式的fpdl流程定义文件解析成WorkflowProcess对象。
	/// </summary>
	public class Dom4JFPDLParser : IFPDLParser
	{
		
		/// <summary>
		/// 将字符串解析成为一个WorkflowProcess对象。
		/// </summary>
		/// <param name="srin">字符串</param>
		/// <returns>返回WorkflowProcess对象</returns>
		public override WorkflowProcess parse(string srin)
		{
			WorkflowProcess wp=new WorkflowProcess("");
			//.net 2.0
//        	if(!string.IsNullOrEmpty(srin)){
//				byte[] pcontent = System.Text.Encoding.Default.GetBytes(process_content.Value.ToString());
//				using (Stream inStream = new MemoryStream(pcontent))
//				{
//					if (inStream != null)
//					{
//						workflowProcess = parse(inStream);
//					}else{
//						//throw new IOException("没有填写流程定义内容!");
//					}
//				}
//			}
			XElement xele = XElement.Parse(srin,LoadOptions.PreserveWhitespace);
			wp = parse(xele);
			return wp;
		}
		/// <summary>
		/// 将输入流解析成为一个WorkflowProcess对象。
		/// </summary>
		/// <param name="srin">输入流</param>
		/// <returns>返回WorkflowProcess对象</returns>
		public override WorkflowProcess parse(Stream srin)
		{
			if (srin == null) return null;
			try
			{
				//XmlDocument document = new XmlDocument();
				//document.Load(srin);
				XmlReaderSettings xrs = new XmlReaderSettings();
				xrs.ProhibitDtd = false;
				//xrs.ValidationType = ValidationType.DTD;
				string filedtd = "";
				HttpContext context = HttpContext.Current;
				if (context == null)
				{
					filedtd = AppDomain.CurrentDomain.BaseDirectory;
				}
				else
				{
					filedtd = context.Server.MapPath("~/");
				}
				//if (!string.IsNullOrEmpty(filedtd) && File.Exists(filedtd))
				//    xrs.Schemas.Add(null, filedtd);

				XElement xele = XElement.Load(System.Xml.XmlReader.Create(srin, xrs, filedtd));
				WorkflowProcess wp = parse(xele);
				return wp;
			}
			catch
			{
				throw;
			}
		}

		private string GetAttributeValue(XElement xele, string name)
		{
			XName xname = XName.Get(name);
			XAttribute xAttribute = xele.Attribute(xname);
			if (xAttribute == null) return String.Empty;
			return xAttribute.Value;
		}
		private string GetElementValue(XElement xele, string name)
		{
			XName xname = XName.Get(name);
			XElement xElement = xele.Element(xname);
			if (xElement == null) return String.Empty;
			return xElement.Value;
		}

		private IEnumerable<XElement> GetXElements(XElement xele, String name)
		{
			var partNos = from item in xele.Descendants("{" + FPDL_URI + "}" + name) select item;
			return partNos;
		}

		private WorkflowProcess parse(XElement xele)
		{
			WorkflowProcess wp = new WorkflowProcess(GetAttributeValue(xele, NAME));
			wp.Sn = System.Guid.NewGuid().ToString();
			wp.TaskInstanceCreator = GetAttributeValue(xele, TASK_INSTANCE_CREATOR);
			wp.FormTaskInstanceRunner = GetAttributeValue(xele, FORM_TASK_INSTANCE_RUNNER);
			wp.ToolTaskInstanceRunner = GetAttributeValue(xele, TOOL_TASK_INSTANCE_RUNNER);
			wp.SubflowTaskInstanceRunner = GetAttributeValue(xele, SUBFLOW_TASK_INSTANCE_RUNNER);
			wp.FormTaskInstanceCompletionEvaluator = GetAttributeValue(xele, FORM_TASK_INSTANCE_COMPLETION_EVALUATOR);
			wp.ToolTaskInstanceCompletionEvaluator = GetAttributeValue(xele, TOOL_TASK_INSTANCE_COMPLETION_EVALUATOR);
			wp.SubflowTaskInstanceCompletionEvaluator = GetAttributeValue(xele, SUBFLOW_TASK_INSTANCE_COMPLETION_EVALUATOR);
			wp.DisplayName = GetAttributeValue(xele, DISPLAY_NAME);
			wp.ResourceFile = GetAttributeValue(xele, RESOURCE_FILE);
			wp.ResourceManager = GetAttributeValue(xele, RESOURCE_MANAGER);

			wp.Description = GetElementValue(xele, "{" + FPDL_URI + "}" + DESCRIPTION);

			loadDataFields(wp, xele.Element("{" + FPDL_URI + "}" + DATA_FIELDS));
			loadStartNode(wp, xele.Element("{" + FPDL_URI + "}" + START_NODE));
			loadTasks(wp, wp.Tasks, xele.Element("{" + FPDL_URI + "}" + TASKS));
			loadActivities(wp, xele.Element("{" + FPDL_URI + "}" + ACTIVITIES));
			loadSynchronizers(wp, xele.Element("{" + FPDL_URI + "}" + SYNCHRONIZERS));
			loadEndNodes(wp, xele.Element("{" + FPDL_URI + "}" + END_NODES));
			loadTransitions(wp, xele.Element("{" + FPDL_URI + "}" + TRANSITIONS));
			loadLoops(wp, xele.Element("{" + FPDL_URI + "}" + LOOPS));

			loadEventListeners(wp.EventListeners, xele.Element("{" + FPDL_URI + "}" + EVENT_LISTENERS));
			loadExtendedAttributes(wp.ExtendedAttributes, xele.Element("{" + FPDL_URI + "}" + EXTENDED_ATTRIBUTES));

			return wp;
		}

		#region 加载Dictionary＜string, string＞类型数据  和  监听数据
		/// <summary>
		/// 加载Dictionary＜string, string＞类型数据
		/// </summary>
		/// <param name="extendedAttributes"></param>
		/// <param name="element"></param>
		private void loadExtendedAttributes(Dictionary<String, String> extendedAttributes, XElement xElement)
		{
			if (xElement == null) { return; }
			extendedAttributes.Clear();
			var partNos = GetXElements(xElement, EXTENDED_ATTRIBUTE);
			foreach (XElement node in partNos)
			{
				extendedAttributes.Add(GetAttributeValue(node, NAME), GetAttributeValue(node, VALUE));
			}
		}
		/// <summary>
		/// 加载监听数据
		/// </summary>
		/// <param name="listeners"></param>
		/// <param name="element"></param>
		private void loadEventListeners(List<EventListener> listeners, XElement xElement)
		{
			if (xElement == null) { return; }
			listeners.Clear();

			var partNos = GetXElements(xElement, EVENT_LISTENER);

			foreach (XElement node in partNos)
			{
				EventListener listener = new EventListener();
				listener.ClassName = GetAttributeValue(node, CLASS_NAME);//((XmlElement)node).GetAttribute(CLASS_NAME);

				listeners.Add(listener);
			}
		}
		#endregion

		#region Activities
		private void loadActivities(WorkflowProcess wp, XElement xElement)
		{
			if (xElement == null) { return; }
			wp.Activities = new List<Activity>();

			var partNos = GetXElements(xElement, ACTIVITY);
			foreach (XElement node in partNos)
			{
				Activity a = createActivitie(wp, node);
				if (a != null) wp.Activities.Add(a);
			}
		}
		private Activity createActivitie(WorkflowProcess wp, XElement xElement)
		{
			if (xElement == null) { return null; }

			Activity activity = new Activity(wp, GetAttributeValue(xElement, NAME));
			activity.Sn = Guid.NewGuid().ToString();
			activity.DisplayName = GetAttributeValue(xElement, DISPLAY_NAME);
			activity.CompletionStrategy = (FormTaskEnum)Enum.Parse(typeof(FormTaskEnum), GetAttributeValue(xElement, COMPLETION_STRATEGY), true);


			activity.Description = GetElementValue(xElement, "{" + FPDL_URI + "}" + DESCRIPTION);

			loadEventListeners(activity.EventListeners, xElement.Element("{" + FPDL_URI + "}" + EVENT_LISTENERS));
			loadExtendedAttributes(activity.ExtendedAttributes, xElement.Element("{" + FPDL_URI + "}" + EXTENDED_ATTRIBUTES));
			loadTasks(activity, activity.InlineTasks, xElement.Element("{" + FPDL_URI + "}" + TASKS));
			loadTaskRefs((WorkflowProcess)activity.ParentElement, activity, activity.TaskRefs, xElement.Element("{" + FPDL_URI + "}" + TASKREFS));

			return activity;
		}

		private void loadTaskRefs(WorkflowProcess workflowProcess, IWFElement parent, List<TaskRef> taskRefs, XElement xElement)
		{
			if (xElement == null) { return; }
			taskRefs.Clear();

			var partNos = GetXElements(xElement, TASKREF);
			foreach (XElement node in partNos)
			{
				String taskId = GetAttributeValue(node, REFERENCE);
				Task task = (Task)workflowProcess.findWFElementById(taskId);
				if (task != null)
				{
					TaskRef taskRef = new TaskRef(parent, task);
					taskRef.Sn = Guid.NewGuid().ToString();
					taskRefs.Add(taskRef);
				}
			}
		}
		#endregion

		#region Tasks

		private void loadTasks(IWFElement parent, List<Task> tasks, XElement xElement)
		{
			if (xElement == null) { return; }
			tasks.Clear();
			var partNos = GetXElements(xElement, TASK);
			foreach (XElement node in partNos)
			{
				Task task = createTask(parent, node);
				if (task != null) tasks.Add(task);
			}
		}

		private T GetEnum<T>(String senum)
		{
			if (String.IsNullOrEmpty(senum))
			{
				return default(T);
			}
			return (T)Enum.Parse(typeof(T), senum, true);
		}

		private Task createTask(IWFElement parent, XElement xElement)
		{
			Task task = null;
			TaskTypeEnum type = (TaskTypeEnum)Enum.Parse(typeof(TaskTypeEnum), GetAttributeValue(xElement, TYPE), true);
			switch (type)
			{
				case TaskTypeEnum.FORM:
					task = new FormTask(parent, GetAttributeValue(xElement, NAME));
					((FormTask)task).AssignmentStrategy = (FormTaskEnum)Enum.Parse(typeof(FormTaskEnum), GetAttributeValue(xElement, COMPLETION_STRATEGY), true);
					((FormTask)task).DefaultView = (DefaultViewEnum)Enum.Parse(typeof(DefaultViewEnum), GetAttributeValue(xElement, DEFAULT_VIEW), true);
					break;

				case TaskTypeEnum.TOOL:
					task = new ToolTask(parent, GetAttributeValue(xElement, NAME));
					break;

				case TaskTypeEnum.SUBFLOW:
					task = new SubflowTask(parent, GetAttributeValue(xElement, NAME));
					break;

					default: return null;
			}

			task.Sn = Guid.NewGuid().ToString();
			task.DisplayName = GetAttributeValue(xElement, DISPLAY_NAME);
			task.TaskInstanceCreator = GetAttributeValue(xElement, TASK_INSTANCE_CREATOR);
			task.TaskInstanceRunner = GetAttributeValue(xElement, TASK_INSTANCE_RUNNER);
			task.TaskInstanceCompletionEvaluator = GetAttributeValue(xElement, TASK_INSTANCE_COMPLETION_EVALUATOR);
			task.LoopStrategy = GetEnum<LoopStrategyEnum>(GetAttributeValue(xElement, LOOP_STRATEGY));

			int priority = 0;
			try { priority = Int32.Parse(GetAttributeValue(xElement, PRIORITY)); }
			catch { }
			task.Priority = priority;
			if (task is FormTask)
			{
				((FormTask)task).Performer = createPerformer(xElement.Element("{" + FPDL_URI + "}" + PERFORMER));

				((FormTask)task).EditForm = createForm(xElement.Element("{" + FPDL_URI + "}" + EDIT_FORM));
				((FormTask)task).ViewForm = createForm(xElement.Element("{" + FPDL_URI + "}" + VIEW_FORM));
				((FormTask)task).ListForm = createForm(xElement.Element("{" + FPDL_URI + "}" + LIST_FORM));
			}

			if (task is ToolTask)
			{
				((ToolTask)task).Application = createApplication(xElement.Element("{" + FPDL_URI + "}" + APPLICATION));
			}
			if (task is SubflowTask)
			{
				((SubflowTask)task).SubWorkflowProcess = createSubWorkflowProcess(xElement.Element("{" + FPDL_URI + "}" + SUB_WORKFLOW_PROCESS));
			}

			task.Description = GetElementValue(xElement, "{" + FPDL_URI + "}" + DESCRIPTION);

			task.Duration = createDuration(xElement.Element("{" + FPDL_URI + "}" + DURATION));

			loadExtendedAttributes(task.ExtendedAttributes, xElement.Element("{" + FPDL_URI + "}" + EXTENDED_ATTRIBUTES));
			loadEventListeners(task.EventListeners, xElement.Element("{" + FPDL_URI + "}" + EVENT_LISTENERS));

			return task;
		}
		private Participant createPerformer(XElement xElement)
		{
			if (xElement == null) { return null; }
			Participant part = new Participant(GetAttributeValue(xElement, NAME));
			part.DisplayName = GetAttributeValue(xElement, DISPLAY_NAME);

			//201004 add lwz 参与者通过业务接口实现默认获取用户
			String sAssignmentType = GetAttributeValue(xElement, ASSIGNMENT_TYPE);
			AssignmentTypeEnum assignmentType;
			if (String.IsNullOrEmpty(sAssignmentType)) { assignmentType = AssignmentTypeEnum.Handler; }
            else assignmentType = (AssignmentTypeEnum)Enum.Parse(typeof(AssignmentTypeEnum), sAssignmentType, true);
			part.AssignmentType = assignmentType;

			part.Description = GetElementValue(xElement, "{" + FPDL_URI + "}" + DESCRIPTION);
			part.AssignmentHandler = GetElementValue(xElement, "{" + FPDL_URI + "}" + ASSIGNMENT_HANDLER);

			return part;
		}

		private Form createForm(XElement xElement)
		{
			if (xElement == null) { return null; }
			Form form = new Form(GetAttributeValue(xElement, NAME));
			form.DisplayName = GetAttributeValue(xElement, DISPLAY_NAME);

			form.Description = GetElementValue(xElement, "{" + FPDL_URI + "}" + DESCRIPTION);
			form.Uri = GetElementValue(xElement, "{" + FPDL_URI + "}" + URI);

			return form;
		}

		private Application createApplication(XElement xElement)
		{
			if (xElement == null) { return null; }
			Application app = new Application(GetAttributeValue(xElement, APPLICATION));
			app.DisplayName = GetAttributeValue(xElement, DISPLAY_NAME);

			app.Description = GetElementValue(xElement, "{" + FPDL_URI + "}" + DESCRIPTION);
			app.Handler = GetElementValue(xElement, "{" + FPDL_URI + "}" + HANDLER);

			return app;
		}

		private SubWorkflowProcess createSubWorkflowProcess(XElement xElement)
		{
			if (xElement == null)
			{
				return null;
			}

			SubWorkflowProcess subFlow = new SubWorkflowProcess(GetAttributeValue(xElement, NAME));
			subFlow.DisplayName = GetAttributeValue(xElement, DISPLAY_NAME);

			subFlow.Description = GetElementValue(xElement, "{" + FPDL_URI + "}" + DESCRIPTION);
			subFlow.WorkflowProcessId = GetElementValue(xElement, "{" + FPDL_URI + "}" + WORKFLOW_PROCESS_ID);

			return subFlow;
		}

		private Duration createDuration(XElement xElement)
		{
			if (xElement == null) { return null; }

			String sValue = GetAttributeValue(xElement, VALUE);
			String sIsBusTime = GetAttributeValue(xElement, IS_BUSINESS_TIME);
			Boolean isBusinessTime = true;
			int value = 1;
			if (sValue != null)
			{
				try
				{
					value = Int32.Parse(sValue);
					isBusinessTime = Boolean.Parse(sIsBusTime);
				}
				catch (Exception)
				{
					return null;
				}
			}
			Duration duration = new Duration(value, (UnitEnum)Enum.Parse(typeof(UnitEnum), GetAttributeValue(xElement, UNIT), true));
			duration.IsBusinessTime = isBusinessTime;
			return duration;
		}
		#endregion

		#region DataFields
		private void loadDataFields(WorkflowProcess wp, XElement xElement)
		{
			if (xElement == null) { return; }
			List<DataField> dataFields = wp.DataFields;
			dataFields.Clear();
			var partNos = GetXElements(xElement, DATA_FIELD);
			foreach (XElement node in partNos)
			{
				dataFields.Add(createDataField(wp, node));
			}
		}

		private DataField createDataField(WorkflowProcess wp, XElement xElement)
		{
			if (xElement == null) { return null; }
			String sdataType = GetAttributeValue(xElement, DATA_TYPE);
			DataTypeEnum dataType;
			if (String.IsNullOrEmpty(sdataType)) { dataType = DataTypeEnum.STRING; }
			else dataType = (DataTypeEnum)Enum.Parse(typeof(DataTypeEnum), sdataType, true);

			DataField dataField = new DataField(wp, GetAttributeValue(xElement, NAME), dataType);

			dataField.Sn = Guid.NewGuid().ToString();

			dataField.DisplayName = GetAttributeValue(xElement, DISPLAY_NAME);
			dataField.InitialValue = GetAttributeValue(xElement, INITIAL_VALUE);

			dataField.Description = GetElementValue(xElement, "{" + FPDL_URI + "}" + DESCRIPTION);
			loadExtendedAttributes(dataField.ExtendedAttributes, xElement.Element("{" + FPDL_URI + "}" + EXTENDED_ATTRIBUTES));

			return dataField;
		}
		#endregion

		#region StartNode
		private void loadStartNode(WorkflowProcess wp, XElement xElement)
		{
			if (xElement == null) { return; }

			StartNode startNode = new StartNode(wp);
			startNode.Name = GetAttributeValue(xElement, NAME);
			startNode.Sn = Guid.NewGuid().ToString();
			startNode.DisplayName = GetAttributeValue(xElement, DISPLAY_NAME);

			startNode.Description = GetElementValue(xElement, "{" + FPDL_URI + "}" + DESCRIPTION);
			loadExtendedAttributes(startNode.ExtendedAttributes, xElement.Element("{" + FPDL_URI + "}" + EXTENDED_ATTRIBUTES));

			wp.StartNode = startNode;
		}
		#endregion

		#region Synchronizers
		private void loadSynchronizers(WorkflowProcess wp, XElement xElement)
		{
			if (xElement == null) { return; }
			List<Synchronizer> synchronizers = wp.Synchronizers;
			synchronizers.Clear();
			var partNos = GetXElements(xElement, SYNCHRONIZER);
			foreach (XElement node in partNos)
			{
				synchronizers.Add(createSynchronizer(wp, node));
			}
		}
		private Synchronizer createSynchronizer(WorkflowProcess wp, XElement xElement)
		{
			if (xElement == null) { return null; }

			Synchronizer synchronizer = new Synchronizer(wp, GetAttributeValue(xElement, NAME));
			synchronizer.Sn = Guid.NewGuid().ToString();
			synchronizer.DisplayName = GetAttributeValue(xElement, DISPLAY_NAME);

			synchronizer.Description = GetElementValue(xElement, "{" + FPDL_URI + "}" + DESCRIPTION);
			loadExtendedAttributes(synchronizer.ExtendedAttributes, xElement.Element("{" + FPDL_URI + "}" + EXTENDED_ATTRIBUTES));

			return synchronizer;
		}
		#endregion

		#region EndNodes
		private void loadEndNodes(WorkflowProcess wp, XElement xElement)
		{
			if (xElement == null) { return; }
			List<EndNode> endNodes = wp.EndNodes;
			endNodes.Clear();
			var partNos = GetXElements(xElement, END_NODE);
			foreach (XElement node in partNos)
			{
				endNodes.Add(createEndNode(wp, node));
			}
		}
		private EndNode createEndNode(WorkflowProcess wp, XElement xElement)
		{
			if (xElement == null) { return null; }

			EndNode endNode = new EndNode(wp, GetAttributeValue(xElement, NAME));
			endNode.Sn = Guid.NewGuid().ToString();
			endNode.DisplayName = GetAttributeValue(xElement, DISPLAY_NAME);

			endNode.Description = GetElementValue(xElement, "{" + FPDL_URI + "}" + DESCRIPTION);
			loadExtendedAttributes(endNode.ExtendedAttributes, xElement.Element("{" + FPDL_URI + "}" + EXTENDED_ATTRIBUTES));

			return endNode;
		}
		#endregion

		#region Transitions
		private void loadTransitions(WorkflowProcess wp, XElement xElement)
		{
			if (xElement == null) { return; }
			List<Transition> transitions = wp.Transitions;
			transitions.Clear();
			var partNos = GetXElements(xElement, TRANSITION);
			foreach (XElement node in partNos)
			{
				Transition transition = createTransition(wp, node);
				transitions.Add(transition);

				Node fromNode = transition.FromNode;
				Node toNode = transition.ToNode;
				if (fromNode != null && (fromNode is Activity))
				{
					((Activity)fromNode).LeavingTransition = transition;
				}
				else if (fromNode != null && (fromNode is Synchronizer))
				{
					((Synchronizer)fromNode).LeavingTransitions.Add(
						transition);
				}
				if (toNode != null && (toNode is Activity))
				{
					((Activity)toNode).EnteringTransition = transition;
				}
				else if (toNode != null && (toNode is Synchronizer))
				{
					((Synchronizer)toNode).EnteringTransitions.Add(transition);
				}
			}
		}
		private Transition createTransition(WorkflowProcess wp, XElement xElement)
		{
			if (xElement == null) { return null; }
			String fromNodeId = GetAttributeValue(xElement, FROM);
			String toNodeId = GetAttributeValue(xElement, TO);
			Node fromNode = (Node)wp.findWFElementById(fromNodeId);
			Node toNode = (Node)wp.findWFElementById(toNodeId);

			Transition transition = new Transition(wp, GetAttributeValue(xElement, NAME), fromNode, toNode);
			transition.Sn = Guid.NewGuid().ToString();

			transition.DisplayName = GetAttributeValue(xElement, DISPLAY_NAME);

			transition.Description = GetElementValue(xElement, "{" + FPDL_URI + "}" + DESCRIPTION);
			loadExtendedAttributes(transition.ExtendedAttributes, xElement.Element("{" + FPDL_URI + "}" + EXTENDED_ATTRIBUTES));
			transition.Condition = GetElementValue(xElement, "{" + FPDL_URI + "}" + CONDITION);

			return transition;
		}
		#endregion

		#region Loops
		private void loadLoops(WorkflowProcess wp, XElement xElement)
		{
			if (xElement == null) return;

			List<Loop> loops = wp.Loops;
			loops.Clear();
			var partNos = GetXElements(xElement, LOOP);
			foreach (XElement node in partNos)
			{
				Loop loop = createLoop(wp, node);
				loops.Add(loop);

				Synchronizer fromNode = (Synchronizer)loop.FromNode;
				Synchronizer toNode = (Synchronizer)loop.ToNode;

				fromNode.LeavingLoops.Add(loop);
				toNode.EnteringLoops.Add(loop);
			}
		}

		private Loop createLoop(WorkflowProcess wp, XElement xElement)
		{
			if (xElement == null) { return null; }

			String fromNodeId = GetAttributeValue(xElement, FROM);
			String toNodeId = GetAttributeValue(xElement, TO);
			Synchronizer fromNode = (Synchronizer)wp.findWFElementById(fromNodeId);
			Synchronizer toNode = (Synchronizer)wp.findWFElementById(toNodeId);

			Loop loop = new Loop(wp, GetAttributeValue(xElement, NAME), fromNode, toNode);
			loop.Sn = Guid.NewGuid().ToString();

			loop.DisplayName = GetAttributeValue(xElement, DISPLAY_NAME);


			loop.Description = GetElementValue(xElement, "{" + FPDL_URI + "}" + DESCRIPTION);
			loadExtendedAttributes(loop.ExtendedAttributes, xElement.Element("{" + FPDL_URI + "}" + EXTENDED_ATTRIBUTES));
			loop.Condition = GetElementValue(xElement, "{" + FPDL_URI + "}" + CONDITION);

			return loop;
		}
		#endregion

	}
}
