using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Coolite.Ext.Web;
using FireWorkflow.Net.Engine;
using FireWorkflow.Net.Engine.Persistence;
using FireWorkflow.Net.Kernel;
using WebDemo.Components;
using WebDemo.Example.LoanProcess.Persistence;

namespace WebDemo.Example.LoanProcess
{
    public partial class ApproveInfo : System.Web.UI.Page
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
                    String sn = (String)wi.TaskInstance.AliveProcessInstance.getProcessInstanceVariable("sn");
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
                        salaryIsReal.Text = ti.SalaryIsReal ? "属实" : "不属实";
                        creditStatus.Text = ti.CreditStatus ? "合格" : "不合格";
                        riskEvaluator.Text = ti.RiskEvaluator;
                        riskInfoInputDate.Text = ti.RiskInfoInputDate.ToString("yyyy-MM-dd hh:mm:ss");
                    }
                    ApproveInfoDAO aid = new ApproveInfoDAO();
                    WebDemo.Example.LoanProcess.Persistence.ApproveInfo approveInfo=aid.findBySnAndUserId(sn, this.User.Identity.Name);
                    if (approveInfo != null)
                    {
                        comments.Text = approveInfo.Detail;
                        my_decision.SelectedItem.Value = approveInfo.Decision.ToString();
                    }
                }
            }
        }

        public void Save_Click(object sender, AjaxEventArgs e)
        {
            string workItemId = HWorkItemId.Value.ToString();
            IWorkflowSession wflsession = RuntimeContextExamples.GetRuntimeContext().getWorkflowSession();
            IWorkItem wi = wflsession.findWorkItemById(workItemId);
            String sn = (String)wi.TaskInstance.AliveProcessInstance.getProcessInstanceVariable("sn");

            ApproveInfoDAO aid = new ApproveInfoDAO();
            WebDemo.Example.LoanProcess.Persistence.ApproveInfo approveInfo = aid.findBySnAndUserId(sn, this.User.Identity.Name);
            if (approveInfo == null)
            {
                approveInfo = new WebDemo.Example.LoanProcess.Persistence.ApproveInfo();
                approveInfo.Sn = sn;
                approveInfo.Approver = this.User.Identity.Name;
            }
            approveInfo.Detail = comments.Text;
            approveInfo.Decision = my_decision.SelectedItem.Value == "True";


            //3、保存业务数据
            aid.attachDirty(approveInfo);
            //4、完成工单
            try
            {
                if (wi != null)
                {
                    if (wi.ActorId == this.User.Identity.Name)
                    {
                        wi.complete(comments.Text);
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
