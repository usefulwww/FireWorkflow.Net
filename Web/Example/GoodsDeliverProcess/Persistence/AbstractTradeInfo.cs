using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebDemo.Example.GoodsDeliverProcess.Persistence
{
    [Serializable]
    public abstract class AbstractTradeInfo
    {
        public String Id { get; set; }
        public String Sn { get; set; }
        public String GoodsName { get; set; }
        public String GoodsType { get; set; }
        public long Quantity { get; set; }
        public Double UnitPrice { get; set; }
        public Double Amount { get; set; }
        public String CustomerName { get; set; }
        public String CustomerMobile { get; set; }
        public String CustomerPhoneFax { get; set; }
        public String CustomerAddress { get; set; }
        public String State { get; set; }
        public DateTime PayedTime { get; set; }
        public DateTime DeliveredTime { get; set; }

        public AbstractTradeInfo()
        {
        }
    }
}
