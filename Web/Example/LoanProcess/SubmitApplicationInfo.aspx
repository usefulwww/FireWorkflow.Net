<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SubmitApplicationInfo.aspx.cs" Inherits="WebDemo.Example.LoanProcess.SubmitApplicationInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ResourceManager ID="ResourceManager1" runat="server" />
    <ext:Panel ID="Panel1" runat="server" BodyStyle="padding:5px 5px 0" Width="350" ButtonAlign="Center" Border="false">
        <Items>
            <ext:FormLayout ID="FormLayout1" runat="server" LabelWidth="110">
                <Anchors >
                    <ext:Anchor Horizontal="100%">
                        <ext:TextField ID="applicantName" runat="server" FieldLabel="申请人姓名" />
                    </ext:Anchor>
                    <ext:Anchor Horizontal="100%">
                        <ext:TextField ID="applicantId" runat="server" FieldLabel="申请人身份证号码" />
                    </ext:Anchor>
                    <ext:Anchor Horizontal="100%">
                        <ext:TextField ID="address" runat="server" FieldLabel="申请人家庭住址" />
                    </ext:Anchor>
                    <ext:Anchor Horizontal="100%">
                        <ext:NumberField ID="salary" runat="server" FieldLabel="申请人年收入(元)">
                        </ext:NumberField>
                    </ext:Anchor>
                    <ext:Anchor Horizontal="100%">
                        <ext:NumberField ID="loanValue" runat="server" FieldLabel="贷款额">
                        </ext:NumberField>
                    </ext:Anchor>
                    <ext:Anchor Horizontal="100%">
                        <ext:DateField runat="server" ID="returnDate" Vtype="daterange" FieldLabel="还贷日期" Format="yyyy-MM-dd">
                            </ext:DateField>
                    </ext:Anchor>
                    <ext:Anchor Horizontal="100%">
                        <ext:Label ID="loanteller" runat="server" FieldLabel="信贷员" />
                    </ext:Anchor>
                    <ext:Anchor Horizontal="100%">
                        <ext:Label ID="appInfoInputDate" runat="server" FieldLabel="信息录入时间" />
                    </ext:Anchor>
                </Anchors>
            </ext:FormLayout>
        </Items>
        <Buttons>
            <ext:Button ID="Button1" runat="server" Text="Save">
                <DirectEvents>
                    <Click OnEvent="Save_Click" Success="parent.Ext.WindowMgr.hideAll();">
                        <EventMask ShowMask="true" Msg="正在保存数据请稍等..." MinDelay="100" />
                    </Click>
                </DirectEvents>
            </ext:Button>
        </Buttons>
    </ext:Panel>
    </form>
</body>
</html>
