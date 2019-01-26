<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormView.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="EP205010.aspx.cs"
	Inherits="Page_EP205010" Title="Untitled Page" %>
<%@ Register TagPrefix="px" Namespace="PX.Web.UI" Assembly="PX.Web.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3b136cac2f602b8e" %>

<%@ MasterType VirtualPath="~/MasterPages/FormView.master" %>
<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <px:PXDataSource ID="ds" runat="server" AutoCallBack="True" Visible="True" Width="100%" PrimaryView="AssigmentMap" TypeName="PX.Objects.EP.EPAssignmentMapMaint"
        PageLoadBehavior="InsertRecord">
        <CallbackCommands>
            <px:PXDSCallbackCommand CommitChanges="True" Name="Save" />
            <px:PXDSCallbackCommand Name="Up" Visible="False" DependOnGrid="topGrid" />
            <px:PXDSCallbackCommand Name="Down" Visible="False" DependOnGrid="topGrid" />
			<px:PXDSCallbackCommand Name="DeleteRoute" Visible="false" />
			<px:PXDSCallbackCommand Name="AddRule" Visible="false" />
			<px:PXDSCallbackCommand DependOnGrid="bottomGrid" Name="ConditionUp" Visible="False" />
			<px:PXDSCallbackCommand DependOnGrid="bottomGrid" Name="ConditionDown" Visible="False" />
			<px:PXDSCallbackCommand DependOnGrid="bottomGrid" Name="ConditionInsert" Visible="False" />
        </CallbackCommands>
        <DataTrees>
            <px:PXTreeDataMember TreeKeys="RuleID" TreeView="NodesTree" />
            <px:PXTreeDataMember TreeKeys="Key" TreeView="CacheTree" />
			<px:PXTreeDataMember TreeView="_EPCompanyTree_Tree_" TreeKeys="WorkgroupID" />
			<px:PXTreeDataMember TreeKeys="Key" TreeView="EntityItems" />
        </DataTrees>
    </px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" runat="Server">
	<px:PXFormView runat="server" ID="mapForm" DataSourceID="ds" Width="100%" DataMember="AssigmentMap" 
        Caption="Assignment Rules Summary" NoteIndicator="True" FilesIndicator="True" ActivityIndicator="True" ActivityField="NoteActivity"
        DefaultControlID="edName">
        <Template>
            <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="S" ControlSize="XM" />
            <px:PXSelector ID="edAssignmentMapID" runat="server" DataField="AssignmentMapID" TextField="Name" NullText="<NEW>" DataSourceID="ds" CommitChanges="true"/>
            <px:PXTextEdit CommitChanges="True" ID="edName" runat="server" DataField="Name" />
			<px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="S" ControlSize="XM" />
			<px:PXDropDown CommitChanges="True" ID="edGraphType" runat="server" DataField="GraphType" AutoRefresh="true" />
        </Template>
    </px:PXFormView>
	    <px:PXSplitContainer runat="server" ID="sp0" SplitterPosition="270"  Style="background-color: #eeeeee;">
        <AutoSize Enabled="true" Container="Window" />
        <Template1>
			<px:PXTreeView ID="tree" runat="server" DataSourceID="ds" Height="500px" PopulateOnDemand="True" ShowRootNode="False"
					ExpandDepth="1" AutoRepaint="true" Caption="Rules" AllowCollapse="true" SelectFirstNode="True"
					CommitChanges="true" MatrixMode="true" SyncPosition="True" SyncPositionWithGraph="True" KeepPosition="True">
				<ToolBarItems>
					<px:PXToolBarButton DisplayStyle="Image" Tooltip="Add Rule">
						<AutoCallBack Command="AddRule" Enabled="True" Target="ds" />
						<Images Normal="main@AddNew" />
					</px:PXToolBarButton>
					<px:PXToolBarButton Text="Up" DisplayStyle="Image" Tooltip="Move Node Up">
						<AutoCallBack Command="Up" Enabled="True" Target="ds" />
						<Images Normal="main@ArrowUp" />
					</px:PXToolBarButton>
					<px:PXToolBarButton Text="Down" DisplayStyle="Image" Tooltip="Move Node Down">
						<AutoCallBack Command="Down" Enabled="True" Target="ds" />
						<Images Normal="main@ArrowDown" />
					</px:PXToolBarButton>
					<px:PXToolBarButton Tooltip="Delete Node" ImageKey="Remove">
						<AutoCallBack Command="DeleteRoute" Target="ds" />
						<Images Normal="main@RecordDel" />
					</px:PXToolBarButton>
				</ToolBarItems>
				<AutoCallBack Target="formRuleType" Command="Refresh" Enabled="True" />
				<AutoSize Enabled="True" MinHeight="300" />
				<DataBindings>
					<px:PXTreeItemBinding DataMember="NodesTree" TextField="Name" ValueField="RuleID" ImageUrlField="Icon" />
				</DataBindings>
			</px:PXTreeView>
		</Template1>
        <Template2>
			<px:PXTab ID="tab" runat="server" Height="126px" Style="z-index: 100;" Width="100%">
				<AutoSize Enabled="True" Container="Parent" MinHeight="180"></AutoSize>
				<Activity HighlightColor="" SelectedColor="" Width="" Height=""></Activity>
				<Items>
					<px:PXTabItem Text="Conditions">
						<Template>
							<px:PXFormView ID="formRuleType" runat="server" Width="100%" DataMember="CurrentNode" DataSourceID="ds" FilesIndicator="false" NoteIndicator="false" SyncPosition="true" SkinID="Transparent">
								<Template>
									<px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="SM" ControlSize="XM" />
									<px:PXTextEdit CommitChanges="True" ID="edRuleName" runat="server" DataField="Name" AutoRefresh="true" />
								</Template> 
							</px:PXFormView>

							<px:PXGrid ID="bottomGrid" runat="server" SkinID="Details" DataSourceID="ds" ActionsPosition="Top" Height="110px" Width="100%"
								MatrixMode="true" SyncPositionWithGraph="true" SyncPosition="true" CaptionVisible="false" Style="z-index: 101;" AutoAdjustColumns="true" AllowPaging="false">
								<Levels>
									<px:PXGridLevel DataMember="Rules">
										<Columns>
											<px:PXGridColumn AllowNull="False" DataField="OpenBrackets" AllowSort="False" Type="DropDownList" Width="70px" AutoCallBack="true" />
											<px:PXGridColumn Type="DropDownList" DataField="Entity" AllowSort="False" AutoCallBack="True" Width="130px" AllowResize="true" />
											<px:PXGridColumn AutoCallBack="True" Type="DropDownList" AllowSort="False" DataField="FieldName" Width="130px" AllowResize="true" CommitChanges="True" />
											<px:PXGridColumn AllowNull="False" DataField="Condition" AllowSort="False" Type="DropDownList" AllowResize="true" />
											<px:PXGridColumn DataField="Value" AllowResize="true" AllowSort="False" />
											<px:PXGridColumn DataField="Value2" AllowResize="true" AllowSort="False" />
											<px:PXGridColumn AllowNull="False" DataField="CloseBrackets" AllowSort="False" Type="DropDownList" Width="70px" />
											<px:PXGridColumn AllowNull="False" DataField="Operator" AllowSort="False" Type="DropDownList" Width="70px" />
										</Columns>
									</px:PXGridLevel>
								</Levels>
								<AutoSize Enabled="true" Container="Parent"/>
								<ActionBar>
									<Actions>
										<Search Enabled="False" />
										<EditRecord Enabled="False" />
										<NoteShow Enabled="False" />
										<FilterShow Enabled="False" />
										<FilterSet Enabled="False" />
										<ExportExcel Enabled="False" />
									</Actions>
									<CustomItems>
										<px:PXToolBarButton CommandName="ConditionInsert" CommandSourceID="ds" Text="Insert" />
										<px:PXToolBarButton CommandName="ConditionUp" CommandSourceID="ds" Tooltip="Move Up">
											<Images Normal="main@ArrowUp" />
										</px:PXToolBarButton>
										<px:PXToolBarButton CommandName="ConditionDown" CommandSourceID="ds" Tooltip="Move Down">
											<Images Normal="main@ArrowDown" />
										</px:PXToolBarButton>
									</CustomItems>
								</ActionBar>
								<Mode InitNewRow="True" />
							</px:PXGrid>
						</Template>
					</px:PXTabItem>
					<px:PXTabItem Text="Rule Actions" RepaintOnDemand="false">
						<Template>
							<px:PXFormView ID="PXFormConditions" runat="server" Width="100%" DataMember="CurrentNode" DataSourceID="ds" SkinID="Transparent">
								<Template>
									<px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="SM" ControlSize="XM" />
									<px:PXDropDown CommitChanges="True" ID="edRuleType" runat="server" DataField="RuleType" AllowNull="False"/>
									<px:PXSelector ID="edOwnerID" runat="server" DataField="OwnerID" AutoRefresh="True" AllowEdit="True" CommitChanges="true" />
									<px:PXTreeSelector ID="edOwnerSource" runat="server" CommitChanges="true" DataField="OwnerSource" TreeDataSourceID="ds" PopulateOnDemand="True"
										InitialExpandLevel="0" ShowRootNode="False" MinDropWidth="468" MaxDropWidth="600" AllowEditValue="True"
										AutoRefresh="True" TreeDataMember="EntityItems">
									<DataBindings>
										<px:PXTreeItemBinding TextField="Name" ValueField="Path" ImageUrlField="Icon" ToolTipField="Path" />
									</DataBindings>
									<ButtonImages Normal="main@AddNew" Hover="main@AddNew" Pushed="main@AddNew" />
									</px:PXTreeSelector>
									<px:PXTreeSelector ID="edWorkgroupID" runat="server" DataField="WorkgroupID" TreeDataMember="_EPCompanyTree_Tree_" TreeDataSourceID="ds"
										PopulateOnDemand="True" InitialExpandLevel="0" ShowRootNode="False" CommitChanges="true">
										<DataBindings>
											<px:PXTreeItemBinding TextField="Description" ValueField="Description" />
										</DataBindings>
									</px:PXTreeSelector>
								</Template>
							</px:PXFormView>
						</Template>
					</px:PXTabItem>
				</Items>
			</px:PXTab>
        </Template2>
	</px:PXSplitContainer>
</asp:Content>

