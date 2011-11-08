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
using System.Text;

namespace FireWorkflow.Net.Model.Io
{
    public abstract class FPDLNames
    {
        /// <summary><para>Namespace prefix to use for XSD elements</para>xsd</summary>
        public const String XSD_NS_PREFIX = "xsd";

        /// <summary><para>The XSD namespace URI. </para>http://www.w3.org/2001/XMLSchema</summary>
        public const String XSD_URI = "http://www.w3.org/2001/XMLSchema";

        /// <summary><para>Namespace prefix to use for XSD elements.</para>xsi</summary>
        public const String XSI_NS_PREFIX = "xsi";

        /// <summary><para>The XSD namespace URI.</para>http://www.w3.org/2001/XMLSchema-instance</summary>
        public const String XSI_URI = "http://www.w3.org/2001/XMLSchema-instance";

        /// <summary>schemaLocation</summary>
        public const String XSI_SCHEMA_LOCATION = "schemaLocation";

        /// <summary><para>Namespace prefix to use for FPDL elements.</para>fpdl</summary>
        public const String FPDL_NS_PREFIX = "fpdl";

        /// <summary><para>The XPDL namespace URI.</para>http://www.fireflow.org/Fireflow_Process_Definition_Language</summary>
        public const String FPDL_URI = "http://www.fireflow.org/Fireflow_Process_Definition_Language";

        /// <summary>-//Nieyun Chen//ProcessDefinition//CN</summary>
        public const String PUBLIC_ID = "-//Nieyun Chen//ProcessDefinition//CN";

        /// <summary>FireFlow_Process_Definition_Language.dtd</summary>
        public const String SYSTEM_ID = "FireFlow_Process_Definition_Language.dtd";

        /// <summary><para>The FPDL schema URI.</para>http://www.fireflow.org/2007/fpdl1.0 fpdl-1.0.xsd</summary>
        public const String FPDL_SCHEMA_LOCATION =
            "http://www.fireflow.org/2007/fpdl1.0 fpdl-1.0.xsd";

        /// <summary><para>Unique identifier.</para>Id</summary>
        public const String ID = "Id";

        /// <summary><para>Entity name.</para>Name</summary>
        public const String NAME = "Name";

        /// <summary><para>Tag which defines a brief description of an element. </para>Description</summary>
        public const String DESCRIPTION = "Description";

        /// <summary>DisplayName</summary>
        public const String DISPLAY_NAME = "DisplayName";

        /// <summary>ExtendedAttributes</summary>
        public const String EXTENDED_ATTRIBUTES = "ExtendedAttributes";
        /// <summary>ExtendedAttribute</summary>
        public const String EXTENDED_ATTRIBUTE = "ExtendedAttribute";
        /// <summary>Value</summary>
        public const String VALUE = "Value";

        /// <summary>Version</summary>
        public const String VERSION = "Version";

        //    String PARTICIPANTS = "Participants";
        //    String PARTICIPANT = "Participant";
        //    String PARTICIPANT_TYPE = "ParticipantType";
        /// <summary>AssignmentHandler</summary>
        public const String ASSIGNMENT_HANDLER = "AssignmentHandler";


        //    public const String EVENT_TYPES = "EventTypes";
        //    public const String EVENT_TYPE = "EventType";
        //    public const String EVENT = "Event";

        //    public const String APPLICATIONS = "Applications";
        /// <summary>Application</summary>
        public const String APPLICATION = "Application";
        /// <summary>Handler</summary>
        public const String HANDLER = "Handler";

        //    public const String WORKFLOW_PROCESSES = "WorkflowProcesses";
        /// <summary>WorkflowProcess</summary>
        public const String WORKFLOW_PROCESS = "WorkflowProcess";


        /// <summary>Priority</summary>
        public const String PRIORITY = "Priority";

        /// <summary>Duration</summary>
        public const String DURATION = "Duration";

        /// <summary>StartNode</summary>
        public const String START_NODE = "StartNode";
        /// <summary>EndNode</summary>
        public const String END_NODE = "EndNode";
        /// <summary>EndNodes</summary>
        public const String END_NODES = "EndNodes";

        /// <summary>Activities</summary>
        public const String ACTIVITIES = "Activities";
        /// <summary>Activity</summary>
        public const String ACTIVITY = "Activity";


        /// <summary>Synchronizers</summary>
        public const String SYNCHRONIZERS = "Synchronizers";
        /// <summary>Synchronizer</summary>
        public const String SYNCHRONIZER = "Synchronizer";

        /// <summary>Tasks</summary>
        public const String TASKS = "Tasks";
        /// <summary>Task</summary>
        public const String TASK = "Task";


        /// <summary>TaskRefs</summary>
        public const String TASKREFS = "TaskRefs";
        /// <summary>TaskRef</summary>
        public const String TASKREF = "TaskRef";
        /// <summary>Reference</summary>
        public const String REFERENCE = "Reference";

        /// <summary>SubFlow</summary>
        public const String SUBFLOW = "SubFlow";

        /// <summary>DataFields</summary>
        public const String DATA_FIELDS = "DataFields";
        /// <summary>DataField</summary>
        public const String DATA_FIELD = "DataField";
        /// <summary>InitialValue</summary>
        public const String INITIAL_VALUE = "InitialValue";
        /// <summary>Length</summary>
        public const String LENGTH = "Length";

        /// <summary>Performer</summary>
        public const String PERFORMER = "Performer";
        /// <summary>StartMode</summary>
        public const String START_MODE = "StartMode";
        /// <summary>FinishMode</summary>
        public const String FINISH_MODE = "FinishMode";
        /// <summary>Manual</summary>
        public const String MANUAL = "Manual";
        /// <summary>Automatic</summary>
        public const String AUTOMATIC = "Automatic";

        /// <summary>Transitions</summary>
        public const String TRANSITIONS = "Transitions";
        /// <summary>Transition</summary>
        public const String TRANSITION = "Transition";
        /// <summary>From</summary>
        public const String FROM = "From";
        /// <summary>To</summary>
        public const String TO = "To";

        /// <summary>Loops</summary>
        public const String LOOPS = "Loops";
        /// <summary>Loop</summary>
        public const String LOOP = "Loop";
        /// <summary>LoopStrategy</summary>
        public const String LOOP_STRATEGY = "LoopStrategy";


        /// <summary>Condition</summary>
        public const String CONDITION = "Condition";

        /// <summary>Type</summary>
        public const String TYPE = "Type";
        /// <summary>DataType</summary>
        public const String DATA_TYPE = "DataType";

        public const String ASSIGNMENT_TYPE = "AssignmentType";

        /// <summary>namespace</summary>
        public const String NAMESPACE = "namespace";

        /// <summary>ExceptionName</summary>
        public const String EXCEPTION_NAME = "ExceptionName";

        /// <summary>ResourceFile</summary>
        public const String RESOURCE_FILE = "ResourceFile";
        /// <summary>ResourceManager</summary>
        public const String RESOURCE_MANAGER = "ResourceManager";

        /// <summary>CompletionStrategy</summary>
        public const String COMPLETION_STRATEGY = "CompletionStrategy";
        /// <summary>DefaultView</summary>
        public const String DEFAULT_VIEW = "DefaultView";
        /// <summary>Execution</summary>
        public const String EXECUTION = "Execution";
        /// <summary>EditForm</summary>
        public const String EDIT_FORM = "EditForm";
        /// <summary>ViewForm</summary>
        public const String VIEW_FORM = "ViewForm";
        /// <summary>ListForm</summary>
        public const String LIST_FORM = "ListForm";
        /// <summary>Uri</summary>
        public const String URI = "Uri";
        /// <summary>Unit</summary>
        public const String UNIT = "Unit";
        /// <summary>IsBusinessTime</summary>
        public const String IS_BUSINESS_TIME = "IsBusinessTime";

        /// <summary>SubWorkflowProcess</summary>
        public const String SUB_WORKFLOW_PROCESS = "SubWorkflowProcess";
        /// <summary>WorkflowProcessId</summary>
        public const String WORKFLOW_PROCESS_ID = "WorkflowProcessId";

        /// <summary>EventListeners</summary>
        public const String EVENT_LISTENERS = "EventListeners";
        /// <summary>EventListener</summary>
        public const String EVENT_LISTENER = "EventListener";
        /// <summary>ClassName</summary>
        public const String CLASS_NAME = "ClassName";

        /// <summary>TaskInstanceCreator</summary>
        public const String TASK_INSTANCE_CREATOR = "TaskInstanceCreator";
        /// <summary>TaskInstanceRunner</summary>
        public const String TASK_INSTANCE_RUNNER = "TaskInstanceRunner";
        /// <summary>TaskInstanceCompletionEvaluator</summary>
        public const String TASK_INSTANCE_COMPLETION_EVALUATOR = "TaskInstanceCompletionEvaluator";

        /// <summary>FormTaskInstanceRunner</summary>
        public const String FORM_TASK_INSTANCE_RUNNER = "FormTaskInstanceRunner";
        /// <summary>ToolTaskInstanceRunner</summary>
        public const String TOOL_TASK_INSTANCE_RUNNER = "ToolTaskInstanceRunner";
        /// <summary>SubflowTaskInstanceRunner</summary>
        public const String SUBFLOW_TASK_INSTANCE_RUNNER = "SubflowTaskInstanceRunner";

        /// <summary>FormTaskInstanceCompletionEvaluator</summary>
        public const String FORM_TASK_INSTANCE_COMPLETION_EVALUATOR = "FormTaskInstanceCompletionEvaluator";
        /// <summary>ToolTaskInstanceCompletionEvaluator</summary>
        public const String TOOL_TASK_INSTANCE_COMPLETION_EVALUATOR = "ToolTaskInstanceCompletionEvaluator";
        /// <summary>SubflowTaskInstanceCompletionEvaluator</summary>
        public const String SUBFLOW_TASK_INSTANCE_COMPLETION_EVALUATOR = "SubflowTaskInstanceCompletionEvaluator";

        //Namespace XSD_NS = new Namespace(XSD_NS_PREFIX, XSD_URI);
        //Namespace XSI_NS = new Namespace(XSI_NS_PREFIX, XSI_URI);
        //Namespace FPDL_NS = new Namespace(FPDL_NS_PREFIX, FPDL_URI);
    }
}
