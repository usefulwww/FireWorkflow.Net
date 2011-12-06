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
    public partial class MyHaveDoneWorkItem : System.Web.UI.Page
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
            IList<IWorkItem> iwis = RuntimeContextExamples.GetRuntimeContext().PersistenceService.FindHaveDoneWorkItems(this.User.Identity.Name);
            Sdate.DataSource = iwis;
            Sdate.DataBind();
        }
    }
}
