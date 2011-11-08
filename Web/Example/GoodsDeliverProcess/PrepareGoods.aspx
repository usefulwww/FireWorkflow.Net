<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrepareGoods.aspx.cs" Inherits="WebDemo.Example.GoodsDeliverProcess.PrepareGoods" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    
    <ext:ScriptManager ID="ScriptManager1" runat="server" />
    <ext:Hidden ID="HWorkItemId" runat="server" />
    <ext:Panel ID="Panel1" runat="server"  BodyStyle="padding:5px 5px 0" Width="350" ButtonAlign="Center" Border="false">
        <Body>
            <ext:FormLayout ID="FormLayout1" runat="server">
                <Anchors>
                    <ext:Anchor Horizontal="100%">
                        <ext:Label ID="Sn" runat="server" FieldLabel="流水号" AllowBlank="false" />
                    </ext:Anchor>
                    <ext:Anchor Horizontal="100%">
                        <ext:Label ID="GoodsName" runat="server" ReadOnly="false" FieldLabel="货品名称" />
                    </ext:Anchor>
                    <ext:Anchor Horizontal="100%">
                        <ext:Label ID="UnitPrice" runat="server" FieldLabel="单价" />
                    </ext:Anchor>
                    <ext:Anchor Horizontal="100%">
                        <ext:Label ID="Quantity" runat="server" FieldLabel="数量" />
                    </ext:Anchor>
                    <ext:Anchor Horizontal="100%">
                        <ext:Label ID="Amount" runat="server" FieldLabel="总价" />
                    </ext:Anchor>
                    <ext:Anchor Horizontal="100%">
                        <ext:Label ID="CustomerName" runat="server" FieldLabel="客户姓名" />
                    </ext:Anchor>
                    <ext:Anchor Horizontal="100%">
                        <ext:Label ID="CustomerMobile" runat="server" FieldLabel="手机" />
                    </ext:Anchor>
                    <ext:Anchor Horizontal="100%">
                        <ext:Label ID="CustomerPhoneFax" runat="server" FieldLabel="电话/传真" />
                    </ext:Anchor>
                    <ext:Anchor Horizontal="100%">
                        <ext:TextArea ID="comments" runat="server" FieldLabel="完成说明">
                        </ext:TextArea>
                    </ext:Anchor>
                </Anchors>
            </ext:FormLayout>
        </Body>
        <Buttons>
            <ext:Button ID="Button1" runat="server" Text="完成">
                <AjaxEvents>
                    <Click OnEvent="Save_Click" Success="parent.Ext.WindowMgr.hideAll();" >
                                            <EventMask ShowMask="true" Msg="完成请稍等..." MinDelay="100" />
                    </Click>
                </AjaxEvents>
            </ext:Button>
            
            <ext:Button ID="Button2" runat="server" Text="取消">
            <Listeners><Click Handler="parent.Ext.WindowMgr.hideAll();" /></Listeners>
            </ext:Button>
            
        </Buttons>
    </ext:Panel>
    </form>
</body>
</html>
