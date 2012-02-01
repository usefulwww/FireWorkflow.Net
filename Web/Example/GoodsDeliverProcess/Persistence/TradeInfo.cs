using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;

namespace WebDemo.Example.GoodsDeliverProcess.Persistence
{
    [Serializable]
    public class TradeInfo : AbstractTradeInfo
    {
        public TradeInfo()
        {
            String sn = DateTime.Now.ToString("yyyyMMdd-HHmmss");
            this.Sn=sn;
        }
    }
}
