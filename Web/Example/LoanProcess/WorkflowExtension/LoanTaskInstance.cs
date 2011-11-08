using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using FireWorkflow.Net.Engine;
using FireWorkflow.Net.Engine.Impl;
using WebDemo.Example.WorkflowExtension;

namespace WebDemo.Example.LoanProcess.WorkflowExtension
{
    public class LoanTaskInstance : TaskInstance, IExampleTaskInstance
    {
        public String Sn { get; set; }
        public String ApplicantName { get; set; }
        public int LoanValue { get; set; }
        public Boolean RiskFlag { get; set; }
        public Boolean Decision { get; set; }


        #region IExampleTaskInstance 成员

        public override string BizInfo
        {
            get
            {
                String riskEvaluateState = "办理中";
                if (this.StepNumber <= 2)
                {
                    riskEvaluateState = "办理中";
                }
                else if (this.StepNumber > 2 && this.RiskFlag == false)
                {
                    riskEvaluateState = "通过";
                }
                else if (this.StepNumber > 2 && this.RiskFlag == true)
                {
                    riskEvaluateState = "不通过";
                }
                String approveFlag = "办理中";
                if (this.StepNumber <= 3)
                {
                    approveFlag = "办理中";
                }
                else if (this.StepNumber > 3 && this.Decision == false)
                {
                    approveFlag = "不通过";
                }
                else if (this.StepNumber > 3 && this.Decision == true)
                {
                    approveFlag = "通过";
                }
                StringBuilder sb = new StringBuilder();
                sb.Append("业务流水号:").Append(this.Sn).Append("    ")
                    .Append("客户姓名:").Append(this.ApplicantName).Append("    ")
                    .Append("贷款金额:").Append(this.LoanValue).Append("    ")
                    .Append("风险核查:").Append(riskEvaluateState).Append("    ")
                    .Append("贷款审批:").Append(approveFlag);
                return sb.ToString();
            }
        }

        public List<IWorkItem> WorkItems { get; set; }

        #endregion
    }
}
