<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddWorkItemList.aspx.cs" Inherits="WebDemo.Example.WorkflowExtension.AddWorkItemList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../resources/css/list.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" />

    <script type="text/javascript">

        var loadExample = function(href) {
            //开窗
            WindowView.autoLoad.url = href; WindowView.reload(true); WindowView.show();
        }
        var selectionChaged = function(dv, nodes) {
            if (nodes.length > 0) {
                var url = nodes[0].getAttribute("ext:url");

                loadExample(url);
            }
        }

        var viewClick = function(dv, e) {
            var group = e.getTarget("h2", 3, true);
            if (group) {
                group.up("div").toggleClass("collapsed");
            }
        }
    </script>

    <ext:Store ID="Store1" runat="server" AutoLoad="true" SerializationMode="Complex" OnRefreshData="RefreshHomeTabData">
        <Proxy>
            <ext:DataSourceProxy />
        </Proxy>
        <AjaxEventConfig Method="GET" />
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="id" />
                    <ext:RecordField Name="title" />
                    <ext:RecordField Name="samples" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:ViewPort ID="ViewPort1" runat="server">
        <Body>
            <ext:FitLayout ID="FitLayout1" runat="server">
                <ext:Panel ID="ImagePanel" runat="server" Cls="images-view" AutoHeight="true" Border="false">
                    <Body>
                        <ext:FitLayout ID="FitLayout2" runat="server">
                            <ext:DataView ID="DataView1" IDMode="Ignore" runat="server" StoreID="Store1" SingleSelect="true" OverClass="x-view-over"
                                ItemSelector="div.thumb-wrap" AutoHeight="true" EmptyText="No examples to display">
                                <Template ID="Template1" runat="server">
                                        <div id="sample-ct">
                                            <tpl for=".">
                                                <div>
                                                    <a name="{id}"></a>
                                                    <h2><div>{title}</div></h2>
                                                    <dl>
                                                        <tpl for="samples">
                                                            <div class="thumb-wrap" ext:url="{url}">
                                                                <img src="{imgUrl}" title="{name}" />
                                                                <div>
                                                                    <H4>{name}</H4>
                                                                    <P>{descr}</P>
                                                                </div>
                                                            </div>
                                                        </tpl>
                                                        <div style="clear:left"></div>
                                                     </dl>
                                                </div>
                                            </tpl>
                                        </div>
                                </Template>
                                <Listeners>
                                    <SelectionChange Fn="selectionChaged" />
                                    <ContainerClick Fn="viewClick" />
                                </Listeners>
                            </ext:DataView>
                        </ext:FitLayout>
                    </Body>
                </ext:Panel>
            </ext:FitLayout>
        </Body>
    </ext:ViewPort>
    <ext:Window ID="WindowView" runat="server" Width="650" Height="450" Collapsible="True" Maximizable="True"
        Constrain="True" ShowOnLoad="false" Icon="Help" Title="创建任务" Modal="True">
        <CustomConfig>
            <ext:ConfigItem Mode="Value" Name="maximized" Value="true" />
        </CustomConfig>
        <AutoLoad Url="" Mode="IFrame" MaskMsg="加载。。。" ShowMask="true" />
        <Listeners>
            <Hide Handler="this.clearContent();" />
        </Listeners>
    </ext:Window>
    
    
    </form>
</body>
</html>
