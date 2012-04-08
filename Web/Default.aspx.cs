using System;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

using FireWorkflow.Net.Engine;
using FireWorkflow.Net.Model;
using FireWorkflow.Net.Model.Io;

//using System.Linq;










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
		
		private void test(){
			// Create the XmlDocument.
			//XmlDocument doc = new XmlDocument();
			//string xmlData = "<book xmlns:bk='urn:samples'></book>";
			//doc.Load(new StringReader(xmlData));
			
			
			// Create a new element and add it to the document.
//			XmlElement elem = doc.CreateElement("bk", "genre", "urn:samples");
//			elem.InnerText = "fantasy";
//			elem.SetAttribute("Id","1");
			
			//doc.Save(Response.Output);
			//Response.Flush();
			
//			<?xml version="1.0" encoding="utf-8"?>
//			<book xmlns:bk="urn:samples">
//			  <bk:genre Id="1">fantasy</bk:genre>
//			</book>
			
			//=================================================
			
//			<fpdl:StartNode Id="LoanProcess.START_NODE" Name="START_NODE" DisplayName="">
//		        <fpdl:ExtendedAttributes>
//		            <fpdl:ExtendedAttribute Name="FIRE_FLOW.bounds.height" Value="20"/>
//		            <fpdl:ExtendedAttribute Name="FIRE_FLOW.bounds.width" Value="20"/>
//		            <fpdl:ExtendedAttribute Name="FIRE_FLOW.bounds.x" Value="52"/>
//		            <fpdl:ExtendedAttribute Name="FIRE_FLOW.bounds.y" Value="15"/>
//		        </fpdl:ExtendedAttributes>
//		    </fpdl:StartNode>
			XmlDocument doc = new XmlDocument();
			XmlElement root = doc.CreateElement(FPDLNames.FPDL_NS_PREFIX,FPDLNames.WORKFLOW_PROCESS,FPDLNames.FPDL_URI);

			XmlElement start_node = doc.CreateElement(FPDLNames.FPDL_NS_PREFIX, FPDLNames.START_NODE,FPDLNames.FPDL_URI);
			start_node.SetAttribute(FPDLNames.ID,"LoanProcess.START_NODE");
			start_node.SetAttribute(FPDLNames.NAME,FPDLNames.START_NODE);
			start_node.SetAttribute(FPDLNames.DISPLAY_NAME,"");
			
			XmlElement start_node_exts = doc.CreateElement(FPDLNames.FPDL_NS_PREFIX, FPDLNames.EXTENDED_ATTRIBUTES,FPDLNames.FPDL_URI);
			XmlElement start_node_ext1 = doc.CreateElement(FPDLNames.FPDL_NS_PREFIX, FPDLNames.EXTENDED_ATTRIBUTE,FPDLNames.FPDL_URI);
			start_node_ext1.SetAttribute("FIRE_FLOW.bounds.height","20");
			XmlElement start_node_ext2 = doc.CreateElement(FPDLNames.FPDL_NS_PREFIX, FPDLNames.EXTENDED_ATTRIBUTE,FPDLNames.FPDL_URI);
			start_node_ext2.SetAttribute("FIRE_FLOW.bounds.width","20");
			XmlElement start_node_ext3 = doc.CreateElement(FPDLNames.FPDL_NS_PREFIX, FPDLNames.EXTENDED_ATTRIBUTE,FPDLNames.FPDL_URI);
			start_node_ext3.SetAttribute("FIRE_FLOW.bounds.x","52");
			XmlElement start_node_ext4 = doc.CreateElement(FPDLNames.FPDL_NS_PREFIX, FPDLNames.EXTENDED_ATTRIBUTE,FPDLNames.FPDL_URI);
			start_node_ext4.SetAttribute("FIRE_FLOW.bounds.y","15");
			start_node_exts.AppendChild(start_node_ext1);
			start_node_exts.AppendChild(start_node_ext2);
			start_node_exts.AppendChild(start_node_ext3);
			start_node_exts.AppendChild(start_node_ext4);
			start_node.AppendChild(start_node_exts);
			root.AppendChild(start_node);
			doc.AppendChild(root);

			doc.Save(Response.Output);
			
			
			XmlElement start_node_w = doc.DocumentElement[FPDLNames.FPDL_NS_PREFIX+":"+FPDLNames.START_NODE];
			Response.Write(start_node_w.GetAttribute(FPDLNames.ID));

			Response.Flush();
		}
	
	}


	
}
