using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


using FireWorkflow.Net.Engine;
using FireWorkflow.Net.Engine.Impl;
using FireWorkflow.Net.Kernel;
using WebDemo.Components;
using WebDemo.Example.GoodsDeliverProcess.Persistence;

namespace WebDemo.Example.GoodsDeliverProcess
{
    public partial class PrepareGoods : System.Web.UI.Page
    {
        TradeInfoDAO tradeInfoDao = new TradeInfoDAO();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                if (this.Request.QueryString["WorkItemId"] != null)
                {
                    string workItemId = this.Request.QueryString["WorkItemId"];
                    //IWorkflowSession wflsession = RuntimeContextExamples.GetRuntimeContext().getWorkflowSessionByWorkItemId(workItemId);
                    IWorkItem wi = RuntimeContextExamples.GetRuntimeContext().PersistenceService.FindWorkItemById(workItemId);
                    String sn = (String)ProcessInstanceHelper.getProcessInstanceVariable(TaskInstanceHelper.getAliveProcessInstance(wi.TaskInstance), "sn");
                    TradeInfo ti=tradeInfoDao.findById(sn);
                    if (ti != null)
                    {
                        HWorkItemId.Value = workItemId;
                        Sn.Text = ti.Sn;
                        GoodsName.Text = ti.GoodsName;
                        UnitPrice.Text = ti.UnitPrice.ToString();
                        Quantity.Text = ti.Quantity.ToString();
                        Amount.Text = ti.Amount.ToString();
                        CustomerName.Text = ti.CustomerName;
                        CustomerMobile.Text = ti.CustomerMobile;
                        CustomerPhoneFax.Text = ti.CustomerPhoneFax;
                    }
                }
            }
        }

        public void Save_Click(object sender, EventArgs e)
        {
            string workItemId = HWorkItemId.Value.ToString();
            //IWorkflowSession wflsession = RuntimeContextExamples.GetRuntimeContext().getWorkflowSessionByWorkItemId(workItemId);
            IWorkItem wi = RuntimeContextExamples.GetRuntimeContext().PersistenceService.FindWorkItemById(workItemId);
            
            try
            {
                if (wi != null)
                {
                    if (wi.ActorId == this.User.Identity.Name)
                    {
                        WorkItemHelper.complete(wi,comments.Text);
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
