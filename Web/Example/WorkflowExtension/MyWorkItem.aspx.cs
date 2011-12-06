using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Ext.Net;
using FireWorkflow.Net.Engine;
using FireWorkflow.Net.Engine.Definition;
using FireWorkflow.Net.Engine.Impl;
using FireWorkflow.Net.Model;
using FireWorkflow.Net.Model.Io;
using WebDemo.Components;

namespace WebDemo.Example.WorkflowExtension
{
    public partial class MyWorkItem : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Sdate_Refresh(null, null);
            }
        }

        public void query_Click(object sender, EventArgs e)
        {
            Sdate_Refresh(null, null);
        }

        public void Sdate_Refresh(object sender, StoreRefreshDataEventArgs e)
        {
            List<IWorkItem> iwis = RuntimeContextExamples.GetRuntimeContext().getWorkflowSession().findMyTodoWorkItems(this.User.Identity.Name);
            Sdate.DataSource = iwis;
            Sdate.DataBind();
        }

        [DirectMethod]
        public bool claim(String workItemId)
        {
            IWorkflowSession wflsession = RuntimeContextExamples.GetRuntimeContext().getWorkflowSession();
            IWorkItem wi = wflsession.findWorkItemById(workItemId);

            try
            {
                if (wi != null)
                {
                    if (wi.ActorId == this.User.Identity.Name)
                    {
                        WorkItemHelper.claim(wi);
                        Sdate_Refresh(null, null);
                        return true;
                    }
                    else return false;
                }
            }
            catch
            {
                throw;
            }
            return false;
        }

        [DirectMethod]
        public void complete(String workItemId)
        {
            IWorkflowSession wflsession = RuntimeContextExamples.GetRuntimeContext().getWorkflowSession();
            IWorkItem wi = wflsession.findWorkItemById(workItemId);

            try
            {
                if (wi != null)
                {
                    if (wi.ActorId == this.User.Identity.Name)
                    {
                    	if (TaskInstanceHelper.getTask(wi.TaskInstance) is FormTask)
                        {
                    		String formUri = this.ResolveUrl(((FormTask)TaskInstanceHelper.getTask(wi.TaskInstance)).EditForm.Uri + "?WorkItemId=" + workItemId);

                            WindowView.AutoLoad.Url = formUri;
                            WindowView.AutoLoad.Mode = LoadMode.IFrame;
                            WindowView.AutoLoad.NoCache = true;
                            WindowView.LoadContent();

                            //WindowView.Reload();
                            //WindowView.AutoLoad.SaveViewState();
                            WindowView.Show();
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }

    }
}
