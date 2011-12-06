<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LendMoneyInfo.aspx.cs" Inherits="WebDemo.Example.LoanProcess.LendMoneyInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ResourceManager ID="ResourceManager1" runat="server" />
    <ext:Hidden ID="HWorkItemId" runat="server" />
    <ext:Panel ID="Panel1" runat="server" BodyStyle="padding:5px 5px 0" Width="350" ButtonAlign="Center"
        Border="false">
        <Items>
            <ext:FormLayout ID="FormLayout1" runat="server" LabelWidth="120">
                <Anchors>
                    <ext:Anchor Horizontal="100%">
                        <ext:Label ID="applicantName" runat="server" FieldLabel="申请人姓名" />
                    </ext:Anchor>
                    <ext:Anchor Horizontal="100%">
                        <ext:Label ID="applicantId" runat="server" FieldLabel="申请人身份证号码" />
                    </ext:Anchor>
                    <ext:Anchor Horizontal="100%">
                        <ext:Label ID="address" runat="server" FieldLabel="申请人家庭住址" />
                    </ext:Anchor>
                    <ext:Anchor Horizontal="100%">
                        <ext:Label ID="salary" runat="server" FieldLabel="申请人年收入(元)" />
                    </ext:Anchor>
                    <ext:Anchor Horizontal="100%">
                        <ext:Label ID="loanValue" runat="server" FieldLabel="贷款额" />
                    </ext:Anchor>
                    <ext:Anchor Horizontal="100%">
                        <ext:Label runat="server" ID="returnDate" FieldLabel="还贷日期" />
                    </ext:Anchor>
                    <ext:Anchor Horizontal="100%">
                        <ext:Label ID="loanteller" runat="server" FieldLabel="信贷员" />
                    </ext:Anchor>
                    <ext:Anchor Horizontal="100%">
                        <ext:Label ID="appInfoInputDate" runat="server" FieldLabel="信息录入时间" />
                    </ext:Anchor>
                    <ext:Anchor Horizontal="100%">
                        <ext:Label ID="salaryIsReal" runat="server" FieldLabel="收入状况是否属实" />
                    </ext:Anchor>
                    <ext:Anchor Horizontal="100%">
                        <ext:Label ID="creditStatus" runat="server" FieldLabel="信用状况是否合格" />
                    </ext:Anchor>
                    <ext:Anchor Horizontal="100%">
                        <ext:Label ID="riskEvaluator" runat="server" FieldLabel="评估人" />
                    </ext:Anchor>
                    <ext:Anchor Horizontal="100%">
                        <ext:Label ID="riskInfoInputDate" runat="server" FieldLabel="评估信息录入时间" />
                    </ext:Anchor>
                    <ext:Anchor Horizontal="100%">
                        <ext:Label ID="lendMoneyOfficer" runat="server" FieldLabel="放款操作人" />
                    </ext:Anchor>
                    <ext:Anchor Horizontal="100%">
                        <ext:Label ID="lendMoneyInfoInputTime" runat="server" FieldLabel="放款信息录入时间人" />
                    </ext:Anchor>
                    <ext:Anchor Horizontal="100%">
                        <ext:TextArea ID="comments" runat="server" FieldLabel="放款意见" />
                    </ext:Anchor>
                </Anchors>
            </ext:FormLayout>
        </Items>
        <Buttons>
            <ext:Button ID="Button1" runat="server" Text="完成">
                <DirectEvents>
                    <Click OnEvent="Save_Click" Success="parent.Ext.WindowMgr.hideAll();">
                        <EventMask ShowMask="true" Msg="完成请稍等..." MinDelay="100" />
                    </Click>
                </DirectEvents>
            </ext:Button>
            <ext:Button ID="Button2" runat="server" Text="取消">
                <Listeners>
                    <Click Handler="parent.Ext.WindowMgr.hideAll();" />
                </Listeners>
            </ext:Button>
        </Buttons>
    </ext:Panel>
    </form>
</body>
</html>
