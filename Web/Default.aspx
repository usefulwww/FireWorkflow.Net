<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebDemo._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" />

    <script type="text/javascript">
        var saveSelectPath = function(node, ml01) {
            var path = node.getPath();
            var w = Pmenu.getWidth();
            Coolite.AjaxMethods.SaveSelectPath(path, ml01, node.id, w);
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
            Coolite.AjaxMethods.RefreshTime({
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

    <ext:ViewPort ID="ViewPort1" runat="server">
        <Body>
            <ext:BorderLayout ID="BorderLayout1" runat="server">
                <North Margins-Bottom="3">
                    <ext:Toolbar runat="server" ID="ctl141">
                        <Items>
                            <ext:Button ID="btnExit" runat="server" Text="退出系统">
                                <AjaxEvents>
                                    <Click OnEvent="btnExit_Click">
                                        <EventMask ShowMask="true" Msg="正在退出..." MinDelay="500" />
                                    </Click>
                                </AjaxEvents>
                            </ext:Button>
                            <ext:ToolbarTextItem ID="username" runat="server" />
                        </Items>
                    </ext:Toolbar>
                </North>
                <West Collapsible="true" Split="true">
                    <ext:Panel ID="Pmenu" runat="server" Title="菜单" Width="175">
                        <Body>
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
                        </Body>
                    </ext:Panel>
                </West>
                <Center>
                    <ext:TabPanel ID="Pages" runat="server" ActiveTabIndex="0" EnableTabScroll="true" DeferredRender="false">
                        <Tabs>
                            <ext:Tab ID="Home" runat="server" Icon="House" Title="Home" AutoScroll="true">
                                <AutoLoad ShowMask="true" Scripts="true" Url="~/Home.aspx" Mode="IFrame" MaskMsg="加载首页...">
                                </AutoLoad>
                            </ext:Tab>
                        </Tabs>
                        <Plugins>
                            <ext:TabCloseMenu ID="TabCloseMenu1" runat="server" CloseTabText="关闭" CloseOtherTabsText="关闭除此之外的全部" />
                        </Plugins>
                    </ext:TabPanel>
                </Center>
                <South Margins-Top="3">
                    <ext:StatusBar ID="statusb" runat="server" DefaultText="">
                        <Items>
                            <ext:ToolbarFill ID="ToolbarFill1" runat="server" />
                            <ext:ToolbarTextItem ID="zxsl" runat="server" Text=" " />
                            <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server" />
                            <ext:ToolbarTextItem ID="clock" runat="server" Text=" " />
                        </Items>
                    </ext:StatusBar>
                </South>
            </ext:BorderLayout>
        </Body>
    </ext:ViewPort>
    </form>
</body>
</html>
