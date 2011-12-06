<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Payment.aspx.cs" Inherits="WebDemo.Example.GoodsDeliverProcess.Payment" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body style="padding: 10px;">
    <form id="form1" runat="server">
    <ext:ResourceManager ID="ResourceManager1" runat="server" />
    <ext:Panel ID="Panel1" runat="server" BodyStyle="padding:5px 5px 0" Width="350" ButtonAlign="Center" Border="false">
        <Items>
            <ext:FormLayout ID="FormLayout1" runat="server" LabelWidth="60">
                <Anchors >
                    <ext:Anchor Horizontal="100%">
                        <ext:TextField ID="Sn" runat="server" FieldLabel="流水号" AllowBlank="false" />
                    </ext:Anchor>
                    <ext:Anchor Horizontal="100%">
                        <ext:ComboBox ID="GoodsName" runat="server" ReadOnly="false" FieldLabel="货品名称">
                            <Items>
                                <ext:ListItem Text="TCL 电视机" Value="TCL 电视机" />
                                <ext:ListItem Text="长虹 电视机" Value="长虹 电视机" />
                                <ext:ListItem Text="万和 热水器" Value="万和 热水器" />
                                <ext:ListItem Text="方太 抽油烟机" Value="方太 抽油烟机" />
                                <ext:ListItem Text="海尔 洗衣机" Value="海尔 洗衣机" />
                            </Items>
                            <SelectedItem Value="TCL 电视机" />
                        </ext:ComboBox>
                    </ext:Anchor>
                    <ext:Anchor Horizontal="100%">
                        <ext:NumberField ID="UnitPrice" runat="server" FieldLabel="单价">
                        
                        </ext:NumberField>
                    </ext:Anchor>
                    <ext:Anchor Horizontal="100%">
                        <ext:NumberField ID="Quantity" runat="server" FieldLabel="数量">
                        </ext:NumberField>
                    </ext:Anchor>
                    <ext:Anchor Horizontal="100%">
                        <ext:NumberField ID="Amount" runat="server" FieldLabel="总价">
                        </ext:NumberField>
                    </ext:Anchor>
                    <ext:Anchor Horizontal="100%">
                        <ext:TextField ID="CustomerName" runat="server" FieldLabel="客户姓名" />
                    </ext:Anchor>
                    <ext:Anchor Horizontal="100%">
                        <ext:TextField ID="CustomerMobile" runat="server" FieldLabel="手机" />
                    </ext:Anchor>
                    <ext:Anchor Horizontal="100%">
                        <ext:TextField ID="CustomerPhoneFax" runat="server" FieldLabel="电话/传真" />
                    </ext:Anchor>
                </Anchors>
            </ext:FormLayout>
        </Items>
        <Buttons>
            <ext:Button runat="server" Text="Save">
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
