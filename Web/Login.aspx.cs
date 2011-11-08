using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebDemo
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtUsername.Text.Trim()))
            {
                //System.Web.Security.FormsAuthentication.RedirectFromLoginPage(tbxUserName.Text, true); 
                System.Web.Security.FormsAuthentication.SetAuthCookie(txtUsername.Text.Trim().ToLower(), false);

                //if(txtUsername.Text.Trim().ToLower()=="admin")
                //    Response.Redirect("~/AddWorkflowProcess.aspx", true);
                //else 
                Response.Redirect("~/Default.aspx", true);
            }
            else
            {
                txtUsername.Text = "";
                txtPassword.Text = "";
            }
        }
    }
}
