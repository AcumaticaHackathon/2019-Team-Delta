<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormTab.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="PM208030.aspx.cs"
    Inherits="Page_PM208030" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPages/FormTab.master" %>
<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%" PrimaryView="Task" TypeName="PX.Objects.PM.TemplateGlobalTaskMaint"
        BorderStyle="NotSet">
        <CallbackCommands>
            <px:PXDSCallbackCommand Name="Cancel" PopupVisible="true" />
            <px:PXDSCallbackCommand CommitChanges="True" Name="Save" PopupVisible="true" />
            <px:PXDSCallbackCommand CommitChanges="True" Name="Delete" PopupVisible="true" ClosePopup="true" />
            <px:PXDSCallbackCommand Name="First" StartNewGroup="True" />
            <px:PXDSCallbackCommand Name="Last" PostData="Self" />
        </CallbackCommands>
    </px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" runat="Server">
    <px:PXFormView ID="form" runat="server" DataSourceID="ds" Style="z-index: 100" Width="100%" DataMember="Task" LinkPage=""
        Caption="Task Summary" FilesIndicator="True" NoteIndicator="True">
        <Template>
            <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="SM" ControlSize="XM" />
            <px:PXSegmentMask ID="edTaskCD" runat="server" DataField="TaskCD" DataSourceID="ds">
            </px:PXSegmentMask>
			<px:PXTextEdit ID="edDescription" runat="server" DataField="Description" />
			<px:PXLayoutRule ID="PXLayoutRule3" runat="server" StartColumn="True" LabelsWidth="SM" ControlSize="XM" />
						
        </Template>
    </px:PXFormView>
</asp:Content>
<asp:Content ID="cont3" ContentPlaceHolderID="phG" runat="Server">
    <px:PXTab ID="tab" runat="server" Width="100%" Height="341px" DataSourceID="ds" DataMember="TaskProperties" LinkPage="">
        <Items>
            <px:PXTabItem Text="Summary">
                <Template>
                    <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="SM" ControlSize="XM" />
                    <px:PXLayoutRule runat="server" StartGroup="True" GroupCaption="Task Properties" />
                    <px:PXDropDown ID="edCompletedPctMethod" runat="server" DataField="CompletedPctMethod" CommitChanges="True" />
                    <px:PXSelector ID="edApproverID" runat="server" DataField="ApproverID" AutoRefresh="True" />
                    <px:PXLayoutRule runat="server" LabelsWidth="SM" ControlSize="XM" />
                    
                    <px:PXLayoutRule runat="server" StartGroup="True" GroupCaption="Billing And Allocation Settings" />
                    <px:PXCheckBox ID="chkBillSeparately" runat="server" DataField="BillSeparately" />                    
                    <px:PXSelector CommitChanges="True" ID="edAllocationID" runat="server" DataField="AllocationID" />
                    <px:PXSelector CommitChanges="True" ID="edBillingID" runat="server" DataField="BillingID" />
                    <px:PXSelector ID="edDefaultBranchID" runat="server" DataField="DefaultBranchID" />
                    <px:PXSelector ID="edRateTableID" runat="server" DataField="RateTableID" DataSourceID="ds" />
                    <px:PXDropDown ID="edBillingOption" runat="server" DataField="BillingOption" />
                    <px:PXSelector ID="edWipAccountGroupID" runat="server" DataField="WipAccountGroupID" />
                    

                    <px:PXLayoutRule runat="server" LabelsWidth="SM" ControlSize="XM" />
                    <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="SM" ControlSize="XM" />
                    <px:PXLayoutRule runat="server" StartGroup="True" GroupCaption="Default Values" />
                    <px:PXSegmentMask ID="edDefaultAccountID" runat="server" DataField="DefaultAccountID" />
                    <px:PXSegmentMask ID="edDefaultSubID" runat="server" DataField="DefaultSubID" />
                     <px:PXSegmentMask ID="PXSegmentMask1" runat="server" DataField="DefaultAccrualAccountID" />
                    <px:PXSegmentMask ID="PXSegmentMask2" runat="server" DataField="DefaultAccrualSubID" />
                    <px:PXSelector ID="edTaxCategoryID" runat="server" DataField="TaxCategoryID" />
                   
                    
                    <px:PXLayoutRule runat="server" LabelsWidth="SM" ControlSize="XM" />
                    <px:PXLayoutRule runat="server" StartGroup="True" GroupCaption="Visibility Settings" />
                    <px:PXLayoutRule ID="PXLayoutRule2" runat="server" Merge="True" />
                    <px:PXCheckBox ID="chkVisibleInGL" runat="server" DataField="VisibleInGL" />
                    <px:PXCheckBox ID="chkVisibleInAP" runat="server" DataField="VisibleInAP" />
                    <px:PXCheckBox ID="chkVisibleInAR" runat="server" DataField="VisibleInAR" />
                    <px:PXCheckBox ID="chkVisibleInSO" runat="server" DataField="VisibleInSO" />
                    <px:PXCheckBox ID="chkVisibleInPO" runat="server" DataField="VisibleInPO" />
                    <px:PXLayoutRule ID="PXLayoutRule5" runat="server" />
                    <px:PXLayoutRule ID="PXLayoutRule4" runat="server" Merge="True" />                
                    <px:PXCheckBox ID="chkVisibleInIN" runat="server" DataField="VisibleInIN" />
                    <px:PXCheckBox ID="chkVisibleInCA" runat="server" DataField="VisibleInCA" />
                    <px:PXCheckBox ID="chkVisibleInCR" runat="server" DataField="VisibleInCR" />
                    <px:PXLayoutRule ID="PXLayoutRule7" runat="server" />
                    <px:PXLayoutRule ID="PXLayoutRule8" runat="server" Merge="True" />
                    <px:PXCheckBox ID="chkVisibleInTA" runat="server" DataField="VisibleInTA" />
                    <px:PXCheckBox ID="chkVisibleInEA" runat="server" DataField="VisibleInEA" />
                    <px:PXLayoutRule ID="PXLayoutRule3" runat="server" />
                </Template>
            </px:PXTabItem>
            <px:PXTabItem Text="Budget">
                <Template>
                    <px:PXGrid ID="grid" runat="server" DataSourceID="ds" Style="z-index: 100" Width="100%" Height="150px" SkinID="DetailsInTab">
                        <Levels>
                            <px:PXGridLevel DataMember="Budget">
                                <RowTemplate>
                                    <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="SM" ControlSize="XM" />
                                    <px:PXSelector ID="edInventoryID" runat="server" DataField="InventoryID" AutoRefresh="True" />
                                    <px:PXSegmentMask ID="edAccountGroupID" runat="server" DataField="AccountGroupID" />
                                     <px:PXSegmentMask ID="edCostCodeID" runat="server" DataField="CostCodeID" />
                                    <px:PXCheckBox ID="chkIsProduction" runat="server" DataField="IsProduction" />
                                    <px:PXTextEdit ID="edDescription" runat="server" DataField="Description" />
                                    <px:PXNumberEdit ID="edQty" runat="server" DataField="Qty" />
                                    <px:PXSelector ID="edUOM" runat="server" DataField="UOM" AutoRefresh="True" />
                                    <px:PXNumberEdit ID="edRate" runat="server" DataField="Rate" />
                                    <px:PXNumberEdit ID="edAmount" runat="server" DataField="Amount" />
                                    <px:PXNumberEdit ID="edRevisedQty" runat="server" DataField="RevisedQty" />
                                    <px:PXNumberEdit ID="edRevisedAmount" runat="server" DataField="RevisedAmount" />
                                    <px:PXNumberEdit ID="edActualQty" runat="server" DataField="ActualQty" Enabled="False" />
                                    <px:PXNumberEdit ID="edActualAmount" runat="server" DataField="ActualAmount" Enabled="False" />
                                </RowTemplate>
                                <Columns>
                                    <px:PXGridColumn DataField="PMAccountGroup__Type" Label="Type" RenderEditorText="True" Width="81px" />
                                    <px:PXGridColumn AutoCallBack="True" DataField="AccountGroupID" Label="Account Group" Width="108px" DisplayMode="Hint" />
                                    <px:PXGridColumn AutoCallBack="True" DataField="InventoryID" Label="Inventory ID" Width="108px" />
                                    <px:PXGridColumn AutoCallBack="True" DataField="CostCodeID" Width="108px" />
                                    <px:PXGridColumn DataField="Description" Width="180px">
                                        <Header Text="Description" />
                                    </px:PXGridColumn>
                                    <px:PXGridColumn DataField="Qty" Label="Qty." TextAlign="Right" Width="81px" AutoCallBack="True" />
                                    <px:PXGridColumn AutoCallBack="True" DataField="UOM" Label="UOM" Width="63px" />
                                    <px:PXGridColumn DataField="Rate" Label="Rate" TextAlign="Right" Width="99px" AutoCallBack="True" />
                                    <px:PXGridColumn DataField="Amount" Label="Amount" TextAlign="Right" Width="81px" />
                                    <px:PXGridColumn DataField="IsProduction" Label="Production" AutoCallBack="True" TextAlign="Center" Type="CheckBox" />
                                </Columns>
                            </px:PXGridLevel>
                        </Levels>
                        <AutoSize Enabled="True" MinHeight="150" />
                        <Mode InitNewRow="True" />
                    </px:PXGrid>
                </Template>
            </px:PXTabItem>
            <px:PXTabItem Text="Recurring Billing">
                <Template>
                    <px:PXGrid ID="GridBillingItems" runat="server" DataSourceID="ds" Width="100%" Height="100%" SkinID="DetailsInTab">
                        <Levels>
                            <px:PXGridLevel DataMember="BillingItems">
                                <RowTemplate>
                                    <px:PXSegmentMask Size="s" ID="edSubMask" runat="server" DataField="SubMask" DataMember="_PMRECBILL_Segments_" />
                                    <px:PXSegmentMask Size="s" ID="edSubID" runat="server" DataField="SubID" />
                                </RowTemplate>
                                <Columns>
                                    <px:PXGridColumn DataField="InventoryID" Width="108px" AutoCallBack="True" />
                                    <px:PXGridColumn DataField="Description" Width="200px" />
                                    <px:PXGridColumn DataField="Amount" TextAlign="Right" Width="81px" />
                                    <px:PXGridColumn DataField="AccountSource" RenderEditorText="True" Width="90px" AutoCallBack="True" />
                                    <px:PXGridColumn DataField="SubMask" Width="108px" RenderEditorText="True" />
                                    <px:PXGridColumn DataField="BranchId" Width="100px" />
                                    <px:PXGridColumn DataField="AccountID" AutoCallBack="True" Width="63px" />
                                    <px:PXGridColumn DataField="SubID" Width="108px" />
                                    <px:PXGridColumn DataField="ResetUsage" RenderEditorText="True" Width="108px" />
                                    <px:PXGridColumn DataField="Included" TextAlign="Right" Width="81px" />
                                    <px:PXGridColumn DataField="UOM" Width="63px" />
                                </Columns>
                            </px:PXGridLevel>
                        </Levels>
                        <AutoSize Enabled="True" />
                    </px:PXGrid>
                </Template>
            </px:PXTabItem>
            <px:PXTabItem Text="Attributes" Visible="True">
                <Template>
                    <px:PXGrid ID="PXGridAnswers" runat="server" DataSourceID="ds" Width="100%" Height="100%" SkinID="DetailsInTab" MatrixMode="True">
                        <Levels>
                            <px:PXGridLevel DataMember="Answers" DataKeyNames="AttributeID,EntityType,EntityID">
                                <Columns>
                                    <px:PXGridColumn DataField="AttributeID" TextAlign="Left" Width="220px" AllowShowHide="False" TextField="AttributeID_description" />
    								<px:PXGridColumn DataField="isRequired" TextAlign="Center" Type="CheckBox" Width="75px" />
                                    <px:PXGridColumn DataField="Value" Width="148px" RenderEditorText="True" />
                                </Columns>
                            </px:PXGridLevel>
                        </Levels>
                        <AutoSize Enabled="True" />
                        <Mode AllowAddNew="False" AllowColMoving="False" AllowDelete="False" />
                    </px:PXGrid>
                </Template>
            </px:PXTabItem>
        </Items>
        <AutoSize Container="Window" Enabled="True" MinHeight="150" />
    </px:PXTab>
</asp:Content>
