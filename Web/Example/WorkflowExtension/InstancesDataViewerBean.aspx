<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InstancesDataViewerBean.aspx.cs" Inherits="WebDemo.Example.WorkflowExtension.InstancesDataViewerBean" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" />
    <ext:Store ID="Sdate" runat="server" OnRefreshData="Sdate_Refresh">
        <Reader>
            <ext:JsonReader ReaderID="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="WorkflowProcessId" />
                    <ext:RecordField Name="ProcessId" />
                    <ext:RecordField Name="Version" />
                    <ext:RecordField Name="DisplayName" />
                    <ext:RecordField Name="State" />
                    <ext:RecordField Name="CreatorId" />
                    <ext:RecordField Name="CreatedTime" Type="Date" />
                    <ext:RecordField Name="StartedTime" Type="Date" />
                    <ext:RecordField Name="ExpiredTime" Type="Date" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="Store1" runat="server" OnRefreshData="Store1_Refresh">
        <Reader>
            <ext:JsonReader ReaderID="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="StepNumber" />
                    <ext:RecordField Name="DisplayName" />
                    <ext:RecordField Name="BizInfo" />
                    <ext:RecordField Name="State" />
                    <ext:RecordField Name="TaskType" />
                    <ext:RecordField Name="CreatedTime" Type="Date" />
                    <ext:RecordField Name="StartedTime" Type="Date" />
                    <ext:RecordField Name="EndTime" Type="Date" />
                    <ext:RecordField Name="ExpiredTime" Type="Date" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="Store2" runat="server" OnRefreshData="Store2_Refresh">
        <Reader>
            <ext:JsonReader ReaderID="Id">
                <Fields>
                    <ext:RecordField Name="Name" />
                    <ext:RecordField Name="ValueType" />
                    <ext:RecordField Name="StringValue" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Hidden ID="HId" runat="server">
    </ext:Hidden>

    <script type="text/javascript">

        var DisplayName = function(value, metadata, record, rowIndex, colIndex, store) {
            return record.data.ProcessId + '/' + record.data.DisplayName;
        }

        var State = function(value) {
            switch (value) {
                case "INITIALIZED": return "初始化中";
                case "RUNNING": return "运行中";
                case "COMPLETED": return "已经结束";
                case "CANCELED": return "被撤销";
            }
        }

        var setRaw = function(response, result, expander, type, action, params) {
            expander.bodyContent[params.id] = result.extraParamsResponse.content;

            var row = expander.grid.view.getRow(params.index);
            var body = Ext.DomQuery.selectNode('tr div.x-grid3-row-body', row);
            body.innerHTML = result.extraParamsResponse.content;

            //For example we will cache rows with an even index
            if (isEven(params.index)) {
                expander.grid.store.getById(params.id).cached = true;
            }
        }
        var isEven = function(num) {
            return !(num % 2);
        }

    </script>

    <ext:ViewPort ID="ViewPort1" runat="server" HideBorders="true">
        <Body>
            <ext:BorderLayout ID="BorderLayout1" runat="server">
                <Center>
                    <ext:GridPanel ID="mpgList" runat="server" StoreID="Sdate" ClicksToEdit="1" StripeRows="true" AutoExpandColumn="DisplayName"
                        Title="流程实例">
                        <ColumnModel ID="ColumnModel1" runat="server">
                            <Columns>
                                <ext:CommandColumn Width="55">
                                    <Commands>
                                        <ext:GridCommand Icon="Zoom" CommandName="View" Text="查看" />
                                    </Commands>
                                </ext:CommandColumn>
                                <ext:Column Sortable="true" DataIndex="DisplayName" Header="流程显示名称">
                                    <Renderer Fn="DisplayName" />
                                </ext:Column>
                                <ext:Column Width="55px" Sortable="true" DataIndex="Version" Header="版本号">
                                </ext:Column>
                                <ext:Column Width="60px" Sortable="true" DataIndex="State" Header="状态">
                                    <Renderer Fn="State" />
                                </ext:Column>
                                <ext:Column Width="120px" Sortable="true" DataIndex="CreatorId" Header="创建者">
                                </ext:Column>
                                <ext:Column Width="120px" Sortable="true" DataIndex="CreatedTime" Header="创建时间">
                                    <Renderer Fn="Ext.util.Format.dateRenderer('Y-m-d h:i:s')" />
                                </ext:Column>
                                <ext:Column Width="120px" Sortable="true" DataIndex="StartedTime" Header="启动时间">
                                    <Renderer Fn="Ext.util.Format.dateRenderer('Y-m-d h:i:s')" />
                                </ext:Column>
                                <ext:Column Width="120px" Sortable="true" DataIndex="ExpiredTime" Header="到期时间">
                                    <Renderer Fn="Ext.util.Format.dateRenderer('Y-m-d h:i:s')" />
                                </ext:Column>
                            </Columns>
                        </ColumnModel>
                        <SelectionModel>
                            <ext:RowSelectionModel ID="RowSelectionModel4" runat="server" SingleSelect="true">
                                <Listeners>
                                    <RowSelect Handler="#{HId}.setValue(record.data.Id); 
                                                        if(#{Panel1}.isVisible()){
                                                            if(#{Tab1}.isVisible()){ #{Store1}.reload(); #{Store2}.removeAll();}
                                                            if(#{Tab2}.isVisible()){ #{Store2}.reload(); #{Store1}.removeAll();}
                                                        }" />
                                </Listeners>
                            </ext:RowSelectionModel>
                        </SelectionModel>
                        <SaveMask ShowMask="true" />
                        <LoadMask ShowMask="true" />
                        <Listeners>
                            <Command Handler="WindowView.autoLoad.url='../../WorkflowProcessView.aspx?WorkflowProcessId='+record.data.WorkflowProcessId+'&ProcessInstanceId='+record.data.Id; WindowView.reload(true); WindowView.show();" />
                        </Listeners>
                    </ext:GridPanel>
                </Center>
                <South Collapsible="true" Split="true" >
                    <ext:Panel ID="Panel1" runat="server" Height="200" Title="流程信息" HideBorders="true">
                        <Body>
                            <ext:FitLayout ID="FitLayout2" runat="server">
                                <ext:TabPanel ID="TabPanel1" runat="server" ActiveTabIndex="0" TabPosition="Bottom">
                                    <Tabs>
                                        <ext:Tab ID="Tab1" runat="server" Title="Task实例" HideBorders="true">
                                            <Body>
                                                <ext:FitLayout ID="FitLayout1" runat="server">
                                                    <ext:GridPanel ID="GridPanel1" runat="server" StoreID="Store1" ClicksToEdit="1" StripeRows="true" AutoExpandColumn="BizInfo"
                                                        Collapsible="true">
                                                        <ColumnModel ID="ColumnModel2" runat="server">
                                                            <Columns>
                                                                <ext:Column Width="45px" Sortable="true" DataIndex="StepNumber" Header="步骤" />
                                                                <ext:Column Width="80px" Sortable="true" DataIndex="DisplayName" Header="流程名称" />
                                                                <ext:Column Sortable="true" DataIndex="BizInfo" Header="业务说明" />
                                                                <ext:Column Width="70px" Sortable="true" DataIndex="State" Header="状态">
                                                                    <Renderer Fn="State" />
                                                                </ext:Column>
                                                                <ext:Column Width="50px" Sortable="true" DataIndex="TaskType" Header="类型" />
                                                                <ext:Column Width="120px" Sortable="true" DataIndex="CreatedTime" Header="创建时间">
                                                                    <Renderer Fn="Ext.util.Format.dateRenderer('Y-m-d h:i:s')" />
                                                                </ext:Column>
                                                                <ext:Column Width="120px" Sortable="true" DataIndex="StartedTime" Header="启动时间">
                                                                    <Renderer Fn="Ext.util.Format.dateRenderer('Y-m-d h:i:s')" />
                                                                </ext:Column>
                                                                <ext:Column Width="120px" Sortable="true" DataIndex="EndTime" Header="结束时间">
                                                                    <Renderer Fn="Ext.util.Format.dateRenderer('Y-m-d h:i:s')" />
                                                                </ext:Column>
                                                                <ext:Column Width="120px" Sortable="true" DataIndex="ExpiredTime" Header="到期时间">
                                                                    <Renderer Fn="Ext.util.Format.dateRenderer('Y-m-d h:i:s')" />
                                                                </ext:Column>
                                                            </Columns>
                                                        </ColumnModel>
                                                        <LoadMask ShowMask="true" />
                                                        <View>
                                                            <ext:GridView ID="GridView1" runat="server" ForceFit="true" />
                                                        </View>
                                                        <SelectionModel>
                                                            <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" />
                                                        </SelectionModel>
                                                        <Plugins>
                                                            <ext:RowExpander ID="RowExpander" runat="server">
                                                                <AjaxEvents>
                                                                    <BeforeExpand OnEvent="BeforeExpand" Success="setRaw(response, result, el, type, action, extraParams);"
                                                                        Before="return !params[1].cached;">
                                                                        <EventMask ShowMask="true" MinDelay="1000" Target="CustomTarget" CustomTarget="={GridPanel1.body}" />
                                                                        <ExtraParams>
                                                                            <ext:Parameter Name="id" Value="params[1].id" Mode="Raw" />
                                                                            <ext:Parameter Name="index" Value="params[3]" Mode="Raw" />
                                                                        </ExtraParams>
                                                                    </BeforeExpand>
                                                                </AjaxEvents>
                                                            </ext:RowExpander>
                                                        </Plugins>
                                                    </ext:GridPanel>
                                                </ext:FitLayout>
                                            </Body>
                                            <Listeners>
                                                <Activate Handler="if(#{Store1}.getCount()<= 0) #{Store1}.reload();" />
                                            </Listeners>
                                        </ext:Tab>
                                        <ext:Tab ID="Tab2" runat="server" Title="流程变量" HideBorders="true">
                                            <Body>
                                                <ext:FitLayout ID="FitLayout3" runat="server">
                                                    <ext:GridPanel ID="GridPanel2" runat="server" StoreID="Store2" ClicksToEdit="1" StripeRows="true" AutoExpandColumn="StringValue">
                                                        <ColumnModel ID="ColumnModel3" runat="server">
                                                            <Columns>
                                                                <ext:Column Width="100px" Sortable="true" DataIndex="Name" Header="名称" />
                                                                <ext:Column Width="60px" Sortable="true" DataIndex="ValueType" Header="类型" />
                                                                <ext:Column Sortable="true" DataIndex="StringValue" Header="值">
                                                                </ext:Column>
                                                            </Columns>
                                                        </ColumnModel>
                                                        <LoadMask ShowMask="true" />
                                                    </ext:GridPanel>
                                                </ext:FitLayout>
                                            </Body>
                                            <Listeners>
                                                <Activate Handler="if(#{Store2}.getCount()<= 0) #{Store2}.reload();" />
                                            </Listeners>
                                        </ext:Tab>
                                    </Tabs>
                                </ext:TabPanel>
                            </ext:FitLayout>
                        </Body>
                    </ext:Panel>
                </South>
            </ext:BorderLayout>
        </Body>
    </ext:ViewPort>
    
    <ext:Window ID="WindowView" runat="server" Width="650" Height="450" Collapsible="True" Maximizable="true" Constrain="True"
        ShowOnLoad="false" Icon="Help" Title="流程" BodyStyle="padding: 5px;"  Modal="True">
        <AutoLoad Url="" Mode="IFrame" MaskMsg="加载流程。。。" ShowMask="true" />
    </ext:Window>
    </form>
</body>
</html>
