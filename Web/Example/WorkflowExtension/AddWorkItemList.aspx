<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddWorkItemList.aspx.cs" Inherits="WebDemo.Example.WorkflowExtension.AddWorkItemList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../resources/css/list.css" rel="stylesheet" type="text/css" />
    
     <script type="text/javascript">
         var loadExample = function(href) {
             //开窗
             WindowView.autoLoad.url = href; WindowView.reload(true); WindowView.show();
         
         }
         var selectionChanged = function(dv, nodes) {
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
    
</head>
<body>
    <form id="form1" runat="server">
    <ext:ResourceManager ID="ResourceManager1" runat="server" />

    <ext:Store ID="Store1" runat="server">        
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="id" />
                        <ext:RecordField Name="title" />
                        <ext:RecordField Name="samples" IsComplex="true" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        
        <ext:Panel 
            ID="DashBoardPanel" 
            runat="server" 
            Cls="items-view" 
            Layout="fit"
            AutoHeight="true"
            Width="800" 
            Border="false">

            <Items>
                <ext:DataView 
                    ID="Dashboard"
                    runat="server" 
                    StoreID="Store1" 
                    SingleSelect="true"
                    OverClass="x-view-over" 
                    ItemSelector="div.thumb-wrap" 
                    AutoHeight="true" 
                    EmptyText="No items to display">
                    <Template ID="Template1" runat="server">
                        <Html>
							<div id="items-ct">
								<tpl for=".">
									<div class="group-header">
										<a name="{id}"></a>
                                        <h2><div>{title}</div></h2>
										<dl>
											<tpl for="samples">
												<div id="{Id}" class="thumb-wrap" ext:url="{url}">
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
						</Html>
                    </Template>
                    <Listeners>
                        <SelectionChange Fn="selectionChanged" />
                        <ContainerClick Fn="viewClick" />
                    </Listeners>
                </ext:DataView>
            </Items>
        </ext:Panel>
    <ext:Window ID="WindowView" runat="server" Width="650" Height="450" Collapsible="True"
        Maximizable="True" Constrain="True" Hidden="true" Icon="Help" Title="创建任务"
        Modal="True">
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
