<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddWorkflowProcess.aspx.cs" Inherits="WebDemo.AddWorkflowProcess"
    ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ResourceManager ID="ResourceManager1" runat="server" />
    <ext:Store ID="Sdate" runat="server">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="ProcessId" />
                    <ext:RecordField Name="Name" />
                    <ext:RecordField Name="DisplayName" />
                    <ext:RecordField Name="Description" />
                    <ext:RecordField Name="Version" />
                    <ext:RecordField Name="State" />
                    <ext:RecordField Name="UploadUser" />
                    <ext:RecordField Name="UploadTime" Type="Date" />
                    <ext:RecordField Name="PublishUser" />
                    <ext:RecordField Name="PublishTime" Type="Date" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Hidden ID="HProcessId" runat="server" />
    <ext:ViewPort ID="ViewPort1" runat="server">
        <Items>
            
                <ext:Panel ID="Panel1" runat="server" Height="300" Border="false" HideBorders="true" Title="流程管理" Layout="Fit">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <ext:Button ID="ToolbarButton1" runat="server" Text="添加">
                                    <Listeners>
                                        <Click Handler="#{FormPanel1}.getForm().reset();#{HProcessId}.setValue();#{WorkflowEdit}.setTitle('添加流程'); #{WorkflowEdit}.show();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="ToolbarButton2" runat="server" Text="修改">
                                <DirectEvents>
                                <Click OnEvent="update_Click">
                                    <EventMask ShowMask="true" Msg="请稍等..." MinDelay="500" />
                                </Click>
                                </DirectEvents>
                                </ext:Button>
                                
                                <ext:Button ID="ToolbarButton3" runat="server" Text="查询">
                                    <DirectEvents>
                                        <Click OnEvent="query_Click">
                                            <EventMask ShowMask="true" Msg="正在查询数据请稍等..." MinDelay="100" />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Items>
                        
                            <ext:GridPanel ID="mpgList" runat="server" StoreID="Sdate" ClicksToEdit="1" StripeRows="true" Layout="Fit">
                                <ColumnModel ID="ColumnModel1" runat="server">
                                    <Columns>
                                        <ext:Column Width="80px" Sortable="true" DataIndex="ProcessId" Header="流程ID">
               
                                        </ext:Column>
                                        <ext:Column Width="90px" Sortable="true" DataIndex="Name" Header="英文名称">
                                        </ext:Column>
                                        <ext:Column Width="120px" Sortable="true" DataIndex="DisplayName" Header="流程显示名称">
                                        </ext:Column>
                                        <ext:Column Width="60px" Sortable="true" DataIndex="Version" Header="版本号">
                                        </ext:Column>
                                        <ext:CommandColumn Width="80">
                                            <Commands>
                                                <ext:GridCommand Icon="Zoom" CommandName="View" Text="查看流程" />
                                            </Commands>
                                        </ext:CommandColumn>

                                        <ext:CheckColumn Width="70px" Sortable="true" DataIndex="State" Header="是否发布" />
                                        <ext:Column Width="80px" Sortable="true" DataIndex="UploadUser" Header="修改者">
                                        </ext:Column>
                                        <ext:Column Width="120px" Sortable="true" DataIndex="UploadTime" Header="修改时间">
                                            <Renderer Fn="Ext.util.Format.dateRenderer('y-m-d h:i:s')" />
                                        </ext:Column>
                                        <ext:Column Width="80px" Sortable="true" DataIndex="PublishUser" Header="发布人">
                                        </ext:Column>
                                        <ext:Column Width="120px" Sortable="true" DataIndex="PublishTime" Header="发布时间">
                                            <Renderer Fn="Ext.util.Format.dateRenderer('y-m-d h:i:s')" />
                                        </ext:Column>
                                    </Columns>
                                </ColumnModel>
                                <SelectionModel>
                                    <ext:RowSelectionModel ID="RowSelectionModel1" SingleSelect="true" runat="server">
                                    </ext:RowSelectionModel>
                                </SelectionModel>
                                <SaveMask ShowMask="true" />
                                <LoadMask ShowMask="true" />
                                <Listeners>
                                    <Command Handler="WindowView.autoLoad.url='WorkflowProcessView.aspx?WorkflowProcessId='+record.data.Id; WindowView.reload(true); WindowView.show();" />
                                </Listeners>
                            </ext:GridPanel>
                    </Items>
                </ext:Panel>
        </Items>
    </ext:ViewPort>
    <ext:Window ID="WorkflowEdit" runat="server" Icon="CalendarSelectWeek" Title="添加流程" AutoHeight="true" Parent="true"
        BodyStyle="padding:10px 10px" Width="500px"  Hidden="true" Constrain="true" ConstrainHeader="true"
        Modal="True" Shadow="Sides" Floating="False" Footer="False">
        <Items>
           <ext:FormPanel ID="FormPanel1" runat="server" Border="false" MonitorValid="true" BodyStyle="background-color:transparent;" Layout="Fit">
                <Items>
                    <ext:FormLayout ID="FormLayout1" runat="server" LabelWidth="80"><Anchors>
                        <ext:Anchor Horizontal="100%">
                            <ext:FileUploadField ID="BasicField" runat="server" Width="350" Icon="Attach" FieldLabel="上传流程文件" />
                        </ext:Anchor>
                        <ext:Anchor Horizontal="100%">
                            <ext:ComboBox ID="state" runat="server" Width="50px" Editable="false" FieldLabel="是否发布">
                                <Items>
                                    <ext:ListItem Text="未发布" Value="False" />
                                    <ext:ListItem Text="已经发布" Value="True" />
                                </Items>
                                <SelectedItem Value="False" />
                            </ext:ComboBox>
                        </ext:Anchor>
                        </Anchors>
                    </ext:FormLayout>
                </Items>
            </ext:FormPanel>
        </Items>
        <Buttons>
            <ext:Button ID="Bppok" runat="server" Icon="CalendarSelectWeek" Text="保存">
                <DirectEvents>
                    <Click OnEvent="ok_Click" Timeout="240000" Before="Ext.Msg.wait('正在上传你的流程文件，请稍等...', '上传流程文件');" 
                        Failure="Ext.Msg.show({ title: 'Error', msg: '上传没有成功', minWidth: 200, modal: true, icon: Ext.Msg.ERROR, buttons: Ext.Msg.OK });">

                    </Click>
                </DirectEvents>
            </ext:Button>
        </Buttons>
    </ext:Window>
    
    <ext:Window ID="WindowView" runat="server" Width="650" Height="450" Collapsible="True"
        Maximizable="true" Hidden="true" Icon="Help" Title="流程" BodyStyle="padding: 5px;" Constrain="True" Modal="True" >
        <AutoLoad Url="WorkflowProcessView.aspx" Mode="IFrame" MaskMsg="加载流程。。。" ShowMask="true" />
    </ext:Window>
    </form>
</body>
</html>
