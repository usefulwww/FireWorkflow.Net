using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FireWorkflow.Net.Engine;
using FireWorkflow.Net.Engine.Impl;
using WebDemo.Example.WorkflowExtension;

namespace WebDemo.Example.GoodsDeliverProcess.WorkflowExtension
{
    public class GoodsDeliverTaskInstance : TaskInstance,IExampleTaskInstance
    {
        public String Sn { get; set; }
        public String GoodsName { get; set; }
        public long Quantity { get; set; }
        public String CustomerName { get; set; }


        #region IExampleTaskInstance 成员

        public override string BizInfo
        {
            get
            {
                StringBuilder sbuf = new StringBuilder();

                sbuf.Append("\t\t订单号:").Append(Sn)
                        .Append("\t\t商品名称:").Append(GoodsName)
                        .Append("\t\t数量:").Append(Quantity)
                        .Append("\t\t客户姓名:").Append(CustomerName);
                return sbuf.ToString();
            }
            set { }
        }

        public List<IWorkItem> WorkItems { get; set; }

        #endregion
    }
}
