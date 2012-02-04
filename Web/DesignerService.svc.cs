using System;
using System.Collections.Generic;
//using System.Linq;
using System.Runtime.Serialization;
//using System.ServiceModel;
using System.Text;
using FireWorkflow.Net.Engine;
using FireWorkflow.Net.Engine.Impl;
using FireWorkflow.Net.Engine.Definition;

namespace WebDemo
{
    // 注意: 如果更改此处的类名 "DesignerService"，也必须更新 Web.config 中对 "DesignerService" 的引用。
    public class DesignerService : IDesignerService
    {
        public String GetWorkflowProcessXml(String id)
        {
            IWorkflowDefinition wd = RuntimeContextFactory.getRuntimeContext().PersistenceService.FindWorkflowDefinitionById(id);
            if (wd != null) return wd.ProcessContent;
            else return "";
        }

        public String GetWorkflowProcessXmlProcessIdOrVersion(String processID, int version)
        {
            if (version <= 0)
            {
                IWorkflowDefinition wd = RuntimeContextFactory.getRuntimeContext().PersistenceService.FindTheLatestVersionOfWorkflowDefinitionByProcessId(processID);
                if (wd != null) return wd.ProcessContent;
            }
            else
            {
                IWorkflowDefinition wd = RuntimeContextFactory.getRuntimeContext().PersistenceService.FindWorkflowDefinitionByProcessIdAndVersionNumber(processID, version);
                if (wd != null) return wd.ProcessContent;
            }
            return "";
        }

        public IList<IProcessInstanceTrace> GetProcessInstanceTraceXml(String processInstanceId)
        {
            IList<IProcessInstanceTrace> pit = RuntimeContextFactory.getRuntimeContext().PersistenceService.FindProcessInstanceTraces(processInstanceId);
            return pit;
        }
    }
}
