<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebDemo._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script type="text/javascript">
        var saveSelectPath = function(node, ml01) {
            var path = node.getPath();
            var w = Pmenu.getWidth();
            Ext.net.DirectMethods.SaveSelectPath(path, ml01, node.id, w);
        }
        var loadPage = function(tabPanel, node) {
            var tab = tabPanel.getItem(node.id);
            if (!tab) {
                tab = tabPanel.add(new Ext.Panel({
                    id: node.id,
                    autoLoad: {
                        showMask: true,
                        scripts: true,
                        url: node.attributes.href,
                        mode: 'iframe',
                        maskMsg: '加载' + node.text + '...'
                    },
                    listeners: {
                        deactivate: {
                            fn: function(el) {
                                if (this.sWin && this.sWin.isVisible()) {
                                    this.sWin.hide();
                                }
                            }
                        }
                    },
                    closable: true
                }));
                tab.title = node.text;
            }
            tabPanel.setActiveTab(tab);
        }
        var refreshTime = function(vstatus) {
            vstatus.showBusy();
            Ext.net.DirectMethods.RefreshTime({
                success: function(result) {
                    vstatus.setText("");
                    vstatus.setIcon();
                },
                failure: function(errorMsg) {
                    vstatus.setText("与服务器断开连接。");
                    vstatus.setIcon();
                }
            });
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <ext:ResourceManager ID="ResourceManager1" runat="server" />
    <ext:Viewport ID="ViewPort1" runat="server" Layout="BorderLayout">
        <Items>
             <ext:Toolbar ID="Toolbar1" runat="server" Region="North" Height="80">
                <Items>
                    <ext:Button ID="btnExit" runat="server" Text="退出系统">
                        <DirectEvents>
                            <Click OnEvent="btnExit_Click">
                                <EventMask ShowMask="true" Msg="正在退出..." MinDelay="500" />
                            </Click>
                        </DirectEvents>
                    </ext:Button>
                    <ext:ToolbarTextItem ID="username" runat="server" />
                </Items>
            </ext:Toolbar>
            <ext:Panel ID="Pmenu" runat="server" Region="West" Layout="AccordionLayout" Title="菜单"
                Width="175">
                <Items>
                    <ext:TreePanel ID="TreePanel1" runat="server" AutoHeight="true" Border="false" RootVisible="false">
                        <Root>
                            <ext:TreeNode Text="Concertos">
                                <Nodes>
                                    <ext:TreeNode Text="业务" Expanded="true">
                                        <Nodes>
                                            <ext:TreeNode Text="创建任务" Href="Example/WorkflowExtension/AddWorkItemList.aspx" Leaf="true" />
                                            <ext:TreeNode Text="待办工作" Href="Example/WorkflowExtension/MyWorkItem.aspx" Leaf="true" />
                                            <ext:TreeNode Text="已办工作" Href="Example/WorkflowExtension/MyHaveDoneWorkItem.aspx" Leaf="true" />
                                            <ext:TreeNode Text="进度查询" Href="Example/WorkflowExtension/InstancesDataViewerBean.aspx" Leaf="true" />
                                        </Nodes>
                                    </ext:TreeNode>
                                    <ext:TreeNode Text="流程管理" Expanded="true">
                                        <Nodes>
                                            <ext:TreeNode Text="流程管理" Href="AddWorkflowProcess.aspx" Leaf="true" />
                                        </Nodes>
                                    </ext:TreeNode>
                                </Nodes>
                            </ext:TreeNode>
                        </Root>
                        <Listeners>
                            <Click Handler="if(node.attributes.href){e.stopEvent();loadPage(#{Pages}, node);}" />
                        </Listeners>
                    </ext:TreePanel>
                </Items>
            </ext:Panel>
            <ext:TabPanel ID="Pages" runat="server" Region="Center">
                <Items>
                    <ext:Panel ID="Home" runat="server" Icon="House" Title="Home">
                        <AutoLoad Url="~/Home.aspx" Mode="IFrame" MaskMsg="加载首页...">
                        </AutoLoad>
                    </ext:Panel>
                </Items>
            </ext:TabPanel>
            <ext:Panel ID="Panel1" runat="server" Title="East" Region="East" Collapsible="true" Split="true"
                MinWidth="225" Width="225" Layout="FitLayout">
                <Items>
                    <ext:TabPanel ID="TabPanel1" runat="server" ActiveTabIndex="1" TabPosition="Bottom" Border="false">
                        <Items>
                            <ext:Panel ID="Panel2" runat="server" Title="Tab 1" Border="false" Padding="6" Html="East Tab 1" />
                            <ext:Panel ID="Panel3" runat="server" Title="Tab 2" Closable="true" Border="false" Padding="6" Html="East Tab 2" />
                        </Items>
                    </ext:TabPanel>
                </Items>
            </ext:Panel>
            <ext:StatusBar ID="statusb" runat="server" Region="South" Split="true" Collapsible="true">
                <Items>
                    <ext:ToolbarFill ID="ToolbarFill1" runat="server" />
                    <ext:ToolbarTextItem ID="zxsl" runat="server" Text=" " />
                    <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server" />
                    <ext:ToolbarTextItem ID="clock" runat="server" Text=" " />
                </Items>
            </ext:StatusBar>
           
        </Items>
    </ext:Viewport>
    </form>
</body>
</html>
