using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Caching;
using Coolite.Ext.Web;

namespace WebDemo.Example.WorkflowExtension
{
    public partial class AddWorkItemList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void RefreshHomeTabData(object sender, StoreRefreshDataEventArgs e)
        {
            var data = this.Page.Cache["ExamplesGroups"] as List<ExampleGroup>;

            if (data == null)
            {
                data = new List<ExampleGroup>();

                ExampleGroup eg = new ExampleGroup { id = "workitem", title = "任务列表" };

                eg.samples.Add(new
                {
                    name = "收款",
                    descr = "创建收款业务，开启商场送货流程。",
                    url = this.ResolveUrl("~/Example/GoodsDeliverProcess/Payment.aspx"),
                    imgUrl = this.ResolveUrl("~/resources/images/list/Process/TheGoodsDeliverProcess.gif")
                });

                eg.samples.Add(new
                {
                    name = "贷款",
                    descr = "创建贷款业务，开启某银行贷款流程。",
                    url = this.ResolveUrl("~/Example/LoanProcess/SubmitApplicationInfo.aspx"),
                    imgUrl = this.ResolveUrl("~/resources/images/list/Process/TheLoanProcess.gif")
                });



                data.Add(eg);


                //UIHelpers.FindExamples(new System.IO.DirectoryInfo(HttpContext.Current.Server.MapPath("~/Examples/")), 1, 3, data);
                this.Page.Cache.Add("ExamplesGroups", data, null, DateTime.Now.AddHours(1), System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
            }

            this.Store1.DataSource = data;
            this.Store1.DataBind();
        }
    }

    public class ExampleGroup
    {
        private List<object> examples;

        public string id { get; set; }

        public string title { get; set; }

        public List<object> samples
        {
            get
            {
                if (this.examples == null)
                {
                    this.examples = new List<object>();
                }
                return examples;
            }
        }
    }
}
