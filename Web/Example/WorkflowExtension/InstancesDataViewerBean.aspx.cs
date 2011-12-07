using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using FireWorkflow.Net.Model;
using FireWorkflow.Net.Model.Io;
using FireWorkflow.Net.Engine.Definition;
using FireWorkflow.Net.Engine;
using FireWorkflow.Net.Engine.Impl;
using FireWorkflow.Net.Engine.Persistence;

using WebDemo.Components;
using Ext.Net;

namespace WebDemo.Example.WorkflowExtension
{
    public partial class InstancesDataViewerBean : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Sdate_Refresh(null, null);
            }
        }


        public void Sdate_Refresh(object sender, StoreRefreshDataEventArgs e)
        {
            IPersistenceService ips = RuntimeContextExamples.GetRuntimeContext().PersistenceService;
            string username = this.User.Identity.Name;
            if (username == "admin") 
            	username = "";
            IList<IProcessInstance> pis = ips.FindProcessInstanceListByCreatorId(username, "", 100, 0);
//            foreach (IProcessInstance item in pis)
//            {
                //((ProcessInstance)item).RuntimeContext = ips.RuntimeContext;
//            }
            Sdate.DataSource = pis;
            Sdate.DataBind();
        }

        public void Store1_Refresh(object sender, StoreRefreshDataEventArgs e)
        {
            IPersistenceService ips = RuntimeContextExamples.GetRuntimeContext().PersistenceService;

            Store1.DataSource = ips.FindTaskInstancesForProcessInstanceByStepNumber(HId.Value.ToString(),-1);
            Store1.DataBind();
        }
        public void Store2_Refresh(object sender, StoreRefreshDataEventArgs e)
        {
            IPersistenceService ips = RuntimeContextExamples.GetRuntimeContext().PersistenceService;

            Store2.DataSource = ips.FindProcessInstanceVariable(HId.Value.ToString());
            Store2.DataBind();
        }

        protected void BeforeExpand(object sender, DirectEventArgs e)
        {
            IPersistenceService ips = RuntimeContextExamples.GetRuntimeContext().PersistenceService;
            IList<IWorkItem> IWorkItems = ips.FindWorkItemsForTaskInstance(e.ExtraParams["id"]);
            StringBuilder sb = new StringBuilder();
            foreach (IWorkItem item in IWorkItems)
            {
                if (sb.Length > 0) sb.Append("<br />");
                sb.AppendFormat("<pre>操作者:{0}\t 状态:{1}\t 开始时间:{2}\t 签收时间:{3}\t 结束时间:{4}\r\n\t完成说明:{5}</pre>",
                    item.ActorId, GetStateToString(item.State), item.CreatedTime, item.ClaimedTime, item.EndTime, item.Comments);
            }
            e.ExtraParamsResponse["content"] = sb.ToString();
        }

        public String GetStateToString (WorkItemEnum wie)
        {
            switch (wie)
            {
                case WorkItemEnum.INITIALIZED: return "初始化中";
                case WorkItemEnum.RUNNING: return "运行中";
                case WorkItemEnum.COMPLETED: return "已经结束";
                case WorkItemEnum.CANCELED: return "被撤销";
                default: return "";
            }
        } 
    }
}
