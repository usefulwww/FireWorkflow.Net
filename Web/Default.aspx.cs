using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FireWorkflow.Net.Model;
using FireWorkflow.Net.Engine;

namespace WebDemo
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack) username.Text = this.User.Identity.Name;

        }

        protected void btnExit_Click(object sender, EventArgs e)
        {

            //退出登录
            string usercode = User.Identity.Name;
            System.Web.Security.FormsAuthentication.SignOut();
            System.Web.Security.FormsAuthentication.RedirectToLoginPage();

            Response.Redirect("~/Login.aspx", true);
        }
    }


   
}
