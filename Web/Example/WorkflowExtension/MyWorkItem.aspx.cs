using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FireWorkflow.Net.Model;
using FireWorkflow.Net.Model.Io;
using FireWorkflow.Net.Engine.Definition;
using FireWorkflow.Net.Engine;
using Coolite.Ext.Web;
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

        public void query_Click(object sender, AjaxEventArgs e)
        {
            Sdate_Refresh(null, null);
        }

        public void Sdate_Refresh(object sender, StoreRefreshDataEventArgs e)
        {
            List<IWorkItem> iwis = RuntimeContextExamples.GetRuntimeContext().getWorkflowSession().findMyTodoWorkItems(this.User.Identity.Name);
            Sdate.DataSource = iwis;
            Sdate.DataBind();
        }

        [AjaxMethod]
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
                        wi.claim();
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

        [AjaxMethod]
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
                        if (wi.TaskInstance.Task is FormTask)
                        {
                            String formUri = this.ResolveUrl(((FormTask)wi.TaskInstance.Task).EditForm.Uri + "?WorkItemId=" + workItemId);

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
