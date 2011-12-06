<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="WebDemo.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>

    <form id="form1" runat="server">
  <ext:ResourceManager ID="ResourceManager1" runat="server" />
        <table>
	<tr>
		<td align="center"><img width="500" height="250"
			src="resources/images/list/Process/TheLoanProcess.gif" /> <br />

		<table cellspacing="0" cellpadding="4" border="1">
			<tr>
				<td colspan="4" style="font-weight: bold">测试用户列表</td>
			</tr>
			<tr>
				<td>角色名称</td>
				<td>用户名</td>
				<td>密码</td>
				<td>中文名</td>
			</tr>
			<tr>
				<td rowspan="2">信贷员</td>
				<td>Loanteller1</td>
				<td>123456</td>
				<td>信贷员1</td>
			</tr>
			<tr>

				<td>Loanteller2</td>
				<td>123456</td>
				<td>信贷员2</td>
			</tr>

			<tr>
				<td rowspan="2">风险审核岗</td>
				<td>RiskEvaluator1</td>
				<td>123456</td>
				<td>风险审核员1</td>
			</tr>
			<tr>

				<td>RiskEvaluator2</td>
				<td>123456</td>
				<td>风险审核员2</td>
			</tr>
			<tr>
				<td rowspan="3">审批岗</td>
				<td>Approver1</td>
				<td>123456</td>
				<td>审批员1</td>
			</tr>
			<tr>

				<td>Approver2</td>
				<td>123456</td>
				<td>审批员2</td>
			</tr>
			<tr>

				<td>Approver3</td>
				<td>123456</td>
				<td>审批员3</td>
			</tr>
			<tr>
				<td>放款操作岗</td>
				<td>LendMoneyOfficer1</td>
				<td>123456</td>
				<td>放款操作员1</td>
			</tr>			
			<tr>
				<td>管理岗</td>
				<td>manager</td>
				<td>123456</td>
				<td>管理员</td>
			</tr>
		</table>
		</td>
		<td align="center"><img
			width="400" height="250"
			src="resources/images/list/Process/TheGoodsDeliverProcess.gif" />

		<br />

		<table cellspacing="0" cellpadding="4" border="1">
			<tr>
				<td colspan="4" style="font-weight: bold">测试用户列表</td>
			</tr>
			<tr>
				<td>角色名称</td>
				<td>用户名</td>
				<td>密码</td>
				<td>中文名</td>
			</tr>
			<tr>
				<td rowspan="2">收银岗</td>
				<td>Cashier1</td>
				<td>123456</td>
				<td>收银员1</td>
			</tr>
			<tr>

				<td>Cashier2</td>
				<td>123456</td>
				<td>收银员2</td>
			</tr>

			<tr>
				<td rowspan="2">仓管岗</td>
				<td>WarehouseKeeper1</td>
				<td>123456</td>
				<td>仓管员1</td>
			</tr>
			<tr>

				<td>WarehouseKeeper2</td>
				<td>123456</td>
				<td>仓管员2</td>
			</tr>
			<tr>
				<td rowspan="3">送货岗</td>
				<td>Deliveryman1</td>
				<td>123456</td>
				<td>送货员1</td>
			</tr>
			<tr>

				<td>Deliveryman2</td>
				<td>123456</td>
				<td>送货员2</td>
			</tr>
			<tr>

				<td>Deliveryman3</td>
				<td>123456</td>
				<td>送货员3</td>
			</tr>
			<tr>
				<td>管理岗</td>
				<td>manager</td>
				<td>123456</td>
				<td>管理员</td>
			</tr>
		</table>

		</td>

	</tr>
</table>
        <ext:Window 
            ID="Window1" 
            runat="server" 
            Height="150" 
            Icon="Lock" 
            Title="Login"
            Width="350"
            BodyStyle="padding:5px;">
            <Items>
                    
                        <ext:TextField  
                            ID="txtUsername" 
                            runat="server" 
                            FieldLabel="用户名" 
                            />
                    
                        <ext:TextField  
                            ID="txtPassword" 
                            runat="server" 
                            InputType="Password" 
                            FieldLabel="密码" 
                            BlankText="Your password is required."
                            />
                   
             </Items>
                   
            
            <Buttons>
                <ext:Button ID="Button1" runat="server" Text="登录" Icon="Accept">
                    <DirectEvents>
                        <Click OnEvent="Button1_Click">
                            <EventMask ShowMask="true" Msg="Verifying..." MinDelay="1000" />
                        </Click>
                    </DirectEvents>
                </ext:Button>
            </Buttons>
        </ext:Window>

    </form>
</body>
</html>
