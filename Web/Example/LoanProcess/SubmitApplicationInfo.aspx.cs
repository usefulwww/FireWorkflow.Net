using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using FireWorkflow.Net.Engine;
using FireWorkflow.Net.Engine.Impl;
using FireWorkflow.Net.Kernel;
using WebDemo.Components;
using WebDemo.Example.LoanProcess.Persistence;

namespace WebDemo.Example.LoanProcess
{
    public partial class SubmitApplicationInfo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                loanteller.Text = this.User.Identity.Name;
                appInfoInputDate.Text = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            }
        }

        public void Save_Click(object sender, EventArgs e)
        {
            LoanInfoDAO loanInfoDAO = new LoanInfoDAO();
            LoanInfo loanInfo=new LoanInfo()
            {
                ApplicantName=applicantName.Text,
                ApplicantId=applicantId.Text,
                Address=address.Text,
                Salary=int.Parse(salary.Text),
                LoanValue=int.Parse(loanValue.Text),
                ReturnDate = returnDate.SelectedDate.ToString("yyyy-MM-dd"),
                Loanteller=loanteller.Text,
                AppInfoInputDate=DateTime.Parse(appInfoInputDate.Text)
            };

            // 一、执行业务业务操作，保存业务数据
            loanInfoDAO.attachDirty(loanInfo);

            // 二、开始执行流程操作
           // IWorkflowSession workflowSession = RuntimeContextExamples.GetRuntimeContext().getWorkflowSession();

            try
            {
                // 1、创建流程实例
                IProcessInstance procInst =
                    ProcessInstanceHelper.createProcessInstance("LoanProcess", this.User.Identity.Name);
                // 2、设置流程变量/业务属性字段
                ProcessInstanceHelper.setProcessInstanceVariable(procInst,"sn", loanInfo.Sn);// 设置流水号
                ProcessInstanceHelper.setProcessInstanceVariable(procInst,"applicantName", loanInfo.ApplicantName);//贷款人姓名
                ProcessInstanceHelper.setProcessInstanceVariable(procInst,"loanValue", loanInfo.LoanValue);// 贷款数额

                // 3、启动流程实例,run()方法启动实例并创建第一个环节实例、分派任务
                ProcessInstanceHelper.run(procInst);
            }
            catch
            {
                throw;
            }
        }
    }
}
