using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Coolite.Ext.Web;
using FireWorkflow.Net.Engine;
using FireWorkflow.Net.Engine.Impl;
using FireWorkflow.Net.Engine.Persistence;
using FireWorkflow.Net.Kernel;
using WebDemo.Components;
using WebDemo.Example.LoanProcess.Persistence;

namespace WebDemo.Example.LoanProcess
{
    public partial class RejectInfo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                if (this.Request.QueryString["WorkItemId"] != null)
                {
                    string workItemId = this.Request.QueryString["WorkItemId"];
                    IWorkflowSession wflsession = RuntimeContextExamples.GetRuntimeContext().getWorkflowSession();
                    IWorkItem wi = wflsession.findWorkItemById(workItemId);
                    String sn = (String)ProcessInstanceHelper.getProcessInstanceVariable(TaskInstanceHelper.getAliveProcessInstance(wi.TaskInstance),"sn");
                    LoanInfoDAO lid = new LoanInfoDAO();
                    LoanInfo ti = lid.findBySn(sn);
                    if (ti != null)
                    {
                        HWorkItemId.Value = workItemId;
                        applicantName.Text = ti.ApplicantName;
                        applicantId.Text = ti.ApplicantId;
                        address.Text = ti.Address;
                        salary.Text = ti.Salary.ToString();
                        loanValue.Text = ti.LoanValue.ToString();
                        returnDate.Text = ti.ReturnDate;
                        loanteller.Text = ti.Loanteller;
                        appInfoInputDate.Text = ti.AppInfoInputDate.ToString("yyyy-MM-dd hh:mm:ss");
                        salaryIsReal.Text = ti.SalaryIsReal ? "属实" : "不属实";
                        creditStatus.Text = ti.CreditStatus ? "合格" : "不合格";
                        riskEvaluator.Text = ti.RiskEvaluator;
                        riskInfoInputDate.Text = ti.RiskInfoInputDate.ToString("yyyy-MM-dd hh:mm:ss");
                        rejectInfoInputTime.Text = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                    }
                }
            }
        }

        public void Save_Click(object sender, AjaxEventArgs e)
        {
            string workItemId = HWorkItemId.Value.ToString();
            IWorkflowSession wflsession = RuntimeContextExamples.GetRuntimeContext().getWorkflowSession();
            IWorkItem wi = wflsession.findWorkItemById(workItemId);

            String sn = (String)ProcessInstanceHelper.getProcessInstanceVariable(TaskInstanceHelper.getAliveProcessInstance(wi.TaskInstance),"sn");
            LoanInfoDAO lid = new LoanInfoDAO();
            LoanInfo loanInfo = lid.findBySn(sn);
            loanInfo.RejectInfoInputTime = DateTime.Parse(rejectInfoInputTime.Text);//lendMoneyInfo
            loanInfo.RejectInfo = comments.Text;

            //4、完成工单
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
