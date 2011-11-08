using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebDemo.Example.LoanProcess.Persistence
{
    [Serializable]
    public class LoanInfo
    {
        public String Id { get; set; }
        public String Sn { get; set; }
        public String ApplicantName { get; set; }
        public String ApplicantId { get; set; }
        public String Address { get; set; }
        public Int32 Salary { get; set; }
        public Int32 LoanValue { get; set; }
        public String ReturnDate { get; set; }
        public String Loanteller { get; set; }
        public DateTime AppInfoInputDate { get; set; }
        public Boolean SalaryIsReal { get; set; }
        public Boolean CreditStatus { get; set; }
        public Boolean RiskFlag { get; set; }
        public String RiskEvaluator { get; set; }
        public DateTime RiskInfoInputDate { get; set; }
        public Boolean Decision { get; set; }
        public String LendMoneyInfo { get; set; }
        public String LendMoneyOfficer { get; set; }
        public DateTime LendMoneyInfoInputTime { get; set; }
        public String RejectInfo { get; set; }
        public DateTime RejectInfoInputTime { get; set; }
        public String ExaminerList { get; set; }
        public String ApproverList { get; set; }
        public String OpponentList { get; set; }

        public LoanInfo()
        {
            String sn = DateTime.Now.ToString("yyyyMMdd-HHmmss");
            this.Sn = sn;
        }
    }
}
