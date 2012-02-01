using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;

namespace WebDemo.Example.LoanProcess.Persistence
{
    [Serializable]
    public class ApproveInfo
    {
        public String Id { get; set; }
        public String Sn { get; set; }
        public String Approver { get; set; }
        public Boolean Decision { get; set; }
        public String Detail { get; set; }
    }
}
