﻿using System;
using PX.Data;
using PX.Objects.GL;
using PX.Objects.PO;
using PX.Objects.IN;
using PX.Objects.CM;
using PX.Objects.TX;
using PX.Objects.CS;
using PX.Objects.PM;
using PX.Objects.Common.Discount;

namespace PX.Objects.AP
{
	public partial class APInvoiceEntry
	{
		[Obsolete(Common.Messages.WillBeRemovedInAcumatica2019R1)]
		[PXProjection(typeof(Select2<POLine,
			LeftJoin<POAccrualStatus, On<POAccrualStatus.type, Equal<POAccrualType.order>,
				And<POAccrualStatus.refNoteID, Equal<POLine.orderNoteID>,
				And<POAccrualStatus.lineNbr, Equal<POLine.lineNbr>>>>>>),
			Persistent = false)]
		[PXCacheName(PO.Messages.POLineShort)]
		[Serializable]
		public partial class POLineS : IBqlTable, IAPTranSource, ISortOrder
		{
			#region Selected
			public abstract class selected : PX.Data.IBqlField
			{
			}
			protected bool? _Selected = false;
			[PXBool]
			[PXDefault(false)]
			[PXUIField(DisplayName = "Selected")]
			public virtual bool? Selected
			{
				get
				{
					return _Selected;
				}
				set
				{
					_Selected = value;
				}
			}
			#endregion
			#region BranchID
			public abstract class branchID : PX.Data.IBqlField
			{
			}
			protected Int32? _BranchID;
			[Branch(BqlField = typeof(POLine.branchID))]
			public virtual Int32? BranchID
			{
				get
				{
					return this._BranchID;
				}
				set
				{
					this._BranchID = value;
				}
			}
			#endregion
			#region OrderType
			public abstract class orderType : PX.Data.IBqlField
			{
			}
			protected String _OrderType;
			[PXDBString(2, IsKey = true, IsFixed = true, BqlField = typeof(POLine.orderType))]
			[PXUIField(DisplayName = "Order Type", Visibility = PXUIVisibility.Visible, Visible = false)]
			public virtual String OrderType
			{
				get
				{
					return this._OrderType;
				}
				set
				{
					this._OrderType = value;
				}
			}
			#endregion
			#region OrderNbr
			public abstract class orderNbr : PX.Data.IBqlField
			{
			}
			protected String _OrderNbr;

			[PXDBString(15, IsUnicode = true, IsKey = true, InputMask = "", BqlField = typeof(POLine.orderNbr))]
			[PXUIField(DisplayName = "Order Nbr.", Visibility = PXUIVisibility.Invisible, Visible = false)]
			public virtual String OrderNbr
			{
				get
				{
					return this._OrderNbr;
				}
				set
				{
					this._OrderNbr = value;
				}
			}
			#endregion
			#region LineNbr
			public abstract class lineNbr : PX.Data.IBqlField
			{
			}
			protected Int32? _LineNbr;
			[PXDBInt(IsKey = true, BqlField = typeof(POLine.lineNbr))]
			[PXUIField(DisplayName = "Line Nbr.", Visibility = PXUIVisibility.Visible, Visible = false)]
			public virtual Int32? LineNbr
			{
				get
				{
					return this._LineNbr;
				}
				set
				{
					this._LineNbr = value;
				}
			}
			#endregion
			#region SortOrder
			public abstract class sortOrder : PX.Data.IBqlField
			{
			}
			protected Int32? _SortOrder;
			[PXUIField(DisplayName = AP.APTran.sortOrder.DispalyName, Visible = false, Enabled = false)]
			[PXDBInt(BqlField = typeof(POLine.sortOrder))]
			public virtual Int32? SortOrder
			{
				get
				{
					return this._SortOrder;
				}
				set
				{
					this._SortOrder = value;
				}
			}
			#endregion
			#region InventoryID
			public abstract class inventoryID : PX.Data.IBqlField
			{
			}
			protected Int32? _InventoryID;
			[POLineInventoryItem(Filterable = true, BqlField = typeof(POLine.inventoryID))]
			public virtual Int32? InventoryID
			{
				get
				{
					return this._InventoryID;
				}
				set
				{
					this._InventoryID = value;
				}
			}
			#endregion
			#region LineType
			public abstract class lineType : PX.Data.IBqlField
			{
			}
			protected String _LineType;
			[PXDBString(2, IsFixed = true, BqlField = typeof(POLine.lineType))]
			[POLineTypeList2(typeof(POLine.orderType), typeof(POLine.inventoryID))]
			[PXUIField(DisplayName = "Line Type")]
			public virtual String LineType
			{
				get
				{
					return this._LineType;
				}
				set
				{
					this._LineType = value;
				}
			}
			#endregion

			#region VendorID
			public abstract class vendorID : PX.Data.IBqlField
			{
			}
			protected Int32? _VendorID;
			[POVendor(BqlField = typeof(POLine.vendorID))]
			public virtual Int32? VendorID
			{
				get
				{
					return this._VendorID;
				}
				set
				{
					this._VendorID = value;
				}
			}
			#endregion
			#region OrderDate
			public abstract class orderDate : PX.Data.IBqlField
			{
			}
			protected DateTime? _OrderDate;
			[PXDBDate(BqlField = typeof(POLine.orderDate))]
			public virtual DateTime? OrderDate
			{
				get
				{
					return this._OrderDate;
				}
				set
				{
					this._OrderDate = value;
				}
			}
			#endregion
			#region SubItemID
			public abstract class subItemID : PX.Data.IBqlField
			{
			}
			protected Int32? _SubItemID;
			[SubItem(typeof(POLineS.inventoryID), BqlField = typeof(POLine.subItemID))]
			public virtual Int32? SubItemID
			{
				get
				{
					return this._SubItemID;
				}
				set
				{
					this._SubItemID = value;
				}
			}
			#endregion
			#region SiteID
			public abstract class siteID : PX.Data.IBqlField
			{
			}
			protected Int32? _SiteID;

			[POSiteAvail(typeof(POLineS.inventoryID), typeof(POLineS.subItemID), BqlField = typeof(POLine.siteID))]
			public virtual Int32? SiteID
			{
				get
				{
					return this._SiteID;
				}
				set
				{
					this._SiteID = value;
				}
			}
			#endregion
			#region LotSerialNbr
			public abstract class lotSerialNbr : PX.Data.IBqlField
			{
			}
			protected String _LotSerialNbr;
			[PXDBString(100, IsUnicode = true, BqlField = typeof(POLine.lotSerialNbr))]
			[PXUIField(DisplayName = "Lot Serial Number", Visible = false)]
			public virtual String LotSerialNbr
			{
				get
				{
					return this._LotSerialNbr;
				}
				set
				{
					this._LotSerialNbr = value;
				}
			}
			#endregion

			#region UOM
			public abstract class uOM : PX.Data.IBqlField
			{
			}
			protected String _UOM;
			[INUnit(typeof(POLine.inventoryID), DisplayName = "UOM", BqlField = typeof(POLine.uOM))]
			public virtual String UOM
			{
				get
				{
					return this._UOM;
				}
				set
				{
					this._UOM = value;
				}
			}
			#endregion
			#region OrderQty
			public abstract class orderQty : PX.Data.IBqlField
			{
			}
			protected Decimal? _OrderQty;
			[PXDBQuantity(typeof(POLineS.uOM), typeof(POLineS.baseOrderQty), BqlField = typeof(POLine.orderQty))]
			[PXUIField(DisplayName = "Order Qty.", Visibility = PXUIVisibility.Visible)]
			public virtual Decimal? OrderQty
			{
				get
				{
					return this._OrderQty;
				}
				set
				{
					this._OrderQty = value;
				}
			}
			#endregion
			#region BaseOrderQty
			public abstract class baseOrderQty : PX.Data.IBqlField
			{
			}
			protected Decimal? _BaseOrderQty;
			[PXUIField(DisplayName = "Base Order Qty.", Visible = false, Enabled = false)]
			[PXDBDecimal(6, BqlField = typeof(POLine.baseOrderQty))]
			public virtual Decimal? BaseOrderQty
			{
				get
				{
					return this._BaseOrderQty;
				}
				set
				{
					this._BaseOrderQty = value;
				}
			}
			#endregion
			#region CuryInfoID
			public abstract class curyInfoID : PX.Data.IBqlField
			{
			}
			protected Int64? _CuryInfoID;

			[PXDBLong(BqlField = typeof(POLine.curyInfoID))]
			[CurrencyInfo]
			public virtual Int64? CuryInfoID
			{
				get
				{
					return this._CuryInfoID;
				}
				set
				{
					this._CuryInfoID = value;
				}
			}
			#endregion
			#region CuryUnitCost
			public abstract class curyUnitCost : PX.Data.IBqlField
			{
			}
			protected Decimal? _CuryUnitCost;

			[PXDBCurrency(typeof(Search<CommonSetup.decPlPrcCst>), typeof(POLineS.curyInfoID), typeof(POLineS.unitCost), BqlField = typeof(POLine.curyUnitCost))]
			[PXUIField(DisplayName = "Unit Cost", Visibility = PXUIVisibility.SelectorVisible)]
			public virtual Decimal? CuryUnitCost
			{
				get
				{
					return this._CuryUnitCost;
				}
				set
				{
					this._CuryUnitCost = value;
				}
			}
			#endregion
			#region UnitCost
			public abstract class unitCost : PX.Data.IBqlField
			{
			}
			protected Decimal? _UnitCost;

			[PXDBPriceCost(BqlField = typeof(POLine.unitCost))]
			public virtual Decimal? UnitCost
			{
				get
				{
					return this._UnitCost;
				}
				set
				{
					this._UnitCost = value;
				}
			}
			#endregion

			#region DiscPct
			public abstract class discPct : PX.Data.IBqlField
			{
			}
			protected Decimal? _DiscPct;
			[PXDBDecimal(6, MinValue = -100, MaxValue = 100, BqlField = typeof(POLine.discPct))]
			public virtual Decimal? DiscPct
			{
				get
				{
					return this._DiscPct;
				}
				set
				{
					this._DiscPct = value;
				}
			}
			#endregion
			#region CuryDiscAmt
			public abstract class curyDiscAmt : PX.Data.IBqlField
			{
			}
			protected Decimal? _CuryDiscAmt;
			[PXDBCurrency(typeof(POLineS.curyInfoID), typeof(POLineS.discAmt), BqlField = typeof(POLine.curyDiscAmt))]
			[PXUIField(DisplayName = "Discount Amount")]
			public virtual Decimal? CuryDiscAmt
			{
				get
				{
					return this._CuryDiscAmt;
				}
				set
				{
					this._CuryDiscAmt = value;
				}
			}
			#endregion
			#region DiscAmt
			public abstract class discAmt : PX.Data.IBqlField
			{
			}
			protected Decimal? _DiscAmt;
			[PXDBDecimal(4, BqlField = typeof(POLine.discAmt))]
			public virtual Decimal? DiscAmt
			{
				get
				{
					return this._DiscAmt;
				}
				set
				{
					this._DiscAmt = value;
				}
			}
			#endregion
			#region CuryLineAmt
			public abstract class curyLineAmt : PX.Data.IBqlField
			{
			}
			protected Decimal? _CuryLineAmt;
			[PXDBCurrency(typeof(POLineS.curyInfoID), typeof(POLineS.lineAmt), BqlField = typeof(POLine.curyLineAmt))]
			[PXUIField(DisplayName = "Ext. Cost")]
			public virtual Decimal? CuryLineAmt
			{
				get
				{
					return this._CuryLineAmt;
				}
				set
				{
					this._CuryLineAmt = value;
				}
			}
			#endregion
			#region LineAmt
			public abstract class lineAmt : PX.Data.IBqlField
			{
			}
			protected Decimal? _LineAmt;
			[PXDBDecimal(4, BqlField = typeof(POLine.lineAmt))]
			public virtual Decimal? LineAmt
			{
				get
				{
					return this._LineAmt;
				}
				set
				{
					this._LineAmt = value;
				}
			}
			#endregion

			#region RetainagePct
			public abstract class retainagePct : IBqlField
			{
			}
			[PXDBDecimal(6, MinValue = -100, MaxValue = 100, BqlField = typeof(POLine.retainagePct))]
			public virtual decimal? RetainagePct
			{
				get;
				set;
			}
			#endregion
			#region CuryRetainageAmt
			public abstract class curyRetainageAmt : IBqlField
			{
			}
			[PXDBCurrency(typeof(POLineS.curyInfoID), typeof(POLineS.retainageAmt), BqlField = typeof(POLine.curyRetainageAmt))]
			public virtual decimal? CuryRetainageAmt
			{
				get;
				set;
			}
			#endregion
			#region RetainageAmt
			public abstract class retainageAmt : IBqlField
			{
			}
			[PXDBBaseCury(BqlField = typeof(POLine.retainageAmt))]
			public virtual decimal? RetainageAmt
			{
				get;
				set;
			}
			#endregion

			#region CuryExtCost
			public abstract class curyExtCost : PX.Data.IBqlField
			{
			}
			protected Decimal? _CuryExtCost;
			[PXDBCurrency(typeof(POLineS.curyInfoID), typeof(POLineS.extCost), MinValue = 0.0, BqlField = typeof(POLine.curyExtCost))]
			[PXUIField(DisplayName = "Amount", Visibility = PXUIVisibility.SelectorVisible, Enabled = false)]
			public virtual Decimal? CuryExtCost
			{
				get
				{
					return this._CuryExtCost;
				}
				set
				{
					this._CuryExtCost = value;
				}
			}
			#endregion
			#region ExtCost
			public abstract class extCost : PX.Data.IBqlField
			{
			}
			protected Decimal? _ExtCost;

			[PXDBBaseCury(BqlField = typeof(POLine.extCost))]
			[PXUIField(DisplayName = "Amount")]
			public virtual Decimal? ExtCost
			{
				get
				{
					return this._ExtCost;
				}
				set
				{
					this._ExtCost = value;
				}
			}
			#endregion
			#region GroupDiscountRate
			public abstract class groupDiscountRate : PX.Data.IBqlField
			{
			}
			protected Decimal? _GroupDiscountRate;
			[PXDBDecimal(6, BqlField = typeof(POLine.groupDiscountRate))]
			public virtual Decimal? GroupDiscountRate
			{
				get
				{
					return this._GroupDiscountRate;
				}
				set
				{
					this._GroupDiscountRate = value;
				}
			}
			#endregion
			#region DocumentDiscountRate
			public abstract class documentDiscountRate : PX.Data.IBqlField
			{
			}
			protected Decimal? _DocumentDiscountRate;
			[PXDBDecimal(6, BqlField = typeof(POLine.documentDiscountRate))]
			public virtual Decimal? DocumentDiscountRate
			{
				get
				{
					return this._DocumentDiscountRate;
				}
				set
				{
					this._DocumentDiscountRate = value;
				}
			}
			#endregion
			#region TaxCategoryID
			public abstract class taxCategoryID : PX.Data.IBqlField
			{
			}
			protected String _TaxCategoryID;
			[PXDBString(10, IsUnicode = true, BqlField = typeof(POLine.taxCategoryID))]
			[PXUIField(DisplayName = "Tax Category", Visibility = PXUIVisibility.Visible)]
			public virtual String TaxCategoryID
			{
				get
				{
					return this._TaxCategoryID;
				}
				set
				{
					this._TaxCategoryID = value;
				}
			}
			#endregion
			#region TaxID
			public abstract class taxID : PX.Data.IBqlField
			{
			}
			protected String _TaxID;
			[PXDBString(Tax.taxID.Length, IsUnicode = true, BqlField = typeof(POLine.taxID))]
			[PXUIField(DisplayName = "Tax ID", Visible = false)]
			[PXSelector(typeof(Tax.taxID), DescriptionField = typeof(Tax.descr))]
			public virtual String TaxID
			{
				get
				{
					return this._TaxID;
				}
				set
				{
					this._TaxID = value;
				}
			}
			#endregion
			#region ExpenseAcctID
			public abstract class expenseAcctID : PX.Data.IBqlField
			{
			}
			protected Int32? _ExpenseAcctID;
			[Account(typeof(POLineS.branchID), DisplayName = "Account", Visibility = PXUIVisibility.Visible, Filterable = false, DescriptionField = typeof(Account.description), BqlField = typeof(POLine.expenseAcctID))]
			public virtual Int32? ExpenseAcctID
			{
				get
				{
					return this._ExpenseAcctID;
				}
				set
				{
					this._ExpenseAcctID = value;
				}
			}
			#endregion
			#region ProjectID
			public abstract class projectID : PX.Data.IBqlField
			{
			}
			protected Int32? _ProjectID;
			[ProjectBase(BqlField = typeof(POLine.projectID))]
			public virtual Int32? ProjectID
			{
				get
				{
					return this._ProjectID;
				}
				set
				{
					this._ProjectID = value;
				}
			}
			#endregion
			#region TaskID
			public abstract class taskID : PX.Data.IBqlField
			{
			}
			protected Int32? _TaskID;
			[ActiveOrInPlanningProjectTask(typeof(POLineS.projectID), BatchModule.PO, DisplayName = "Project Task", BqlField = typeof(POLine.taskID))]
			public virtual Int32? TaskID
			{
				get
				{
					return this._TaskID;
				}
				set
				{
					this._TaskID = value;
				}
			}
			#endregion
			#region ExpenseSubID
			public abstract class expenseSubID : PX.Data.IBqlField
			{
			}
			protected Int32? _ExpenseSubID;
			[SubAccount(typeof(POLineS.expenseAcctID), typeof(POLineS.branchID), DisplayName = "Sub.", Visibility = PXUIVisibility.Visible, Filterable = true, BqlField = typeof(POLine.expenseSubID))]
			public virtual Int32? ExpenseSubID
			{
				get
				{
					return this._ExpenseSubID;
				}
				set
				{
					this._ExpenseSubID = value;
				}
			}
			#endregion
			#region POAccrualAcctID
			public abstract class pOAccrualAcctID : IBqlField
			{
			}
			[Account(typeof(POLineS.branchID), DescriptionField = typeof(Account.description), DisplayName = "Accrual Account", Filterable = false, BqlField = typeof(POLine.pOAccrualAcctID))]
			public virtual int? POAccrualAcctID
			{
				get;
				set;
			}
			#endregion
			#region POAccrualSubID
			public abstract class pOAccrualSubID : IBqlField
			{
			}
			[SubAccount(typeof(POLineS.pOAccrualAcctID), typeof(POLineS.branchID), DisplayName = "Accrual Sub.", Filterable = true, BqlField = typeof(POLine.pOAccrualSubID))]
			[PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
			public virtual int? POAccrualSubID
			{
				get;
				set;
			}
			#endregion
			#region TranDesc
			public abstract class tranDesc : PX.Data.IBqlField
			{
			}
			protected String _TranDesc;
			[PXDBString(256, IsUnicode = true, BqlField = typeof(POLine.tranDesc))]
			[PXUIField(DisplayName = "Line Description", Visibility = PXUIVisibility.Visible)]
			public virtual String TranDesc
			{
				get
				{
					return this._TranDesc;
				}
				set
				{
					this._TranDesc = value;
				}
			}
			#endregion
			#region CostCodeID
			public abstract class costCodeID : PX.Data.IBqlField
			{
			}
			protected Int32? _CostCodeID;
			[CostCode(typeof(expenseAcctID), typeof(taskID), GL.AccountType.Expense, BqlField = typeof(POLine.costCodeID))]
			public virtual Int32? CostCodeID
			{
				get
				{
					return this._CostCodeID;
				}
				set
				{
					this._CostCodeID = value;
				}
			}
			#endregion

			#region Cancelled
			public abstract class cancelled : PX.Data.IBqlField
			{
			}
			protected Boolean? _Cancelled;
			[PXDBBool(BqlField = typeof(POLine.cancelled))]
			[PXUIField(DisplayName = "Cancelled", Visibility = PXUIVisibility.Visible)]
			public virtual Boolean? Cancelled
			{
				get
				{
					return this._Cancelled;
				}
				set
				{
					this._Cancelled = value;
				}
			}
			#endregion
			#region Closed
			public abstract class closed : IBqlField
			{
			}
			[PXDBBool(BqlField = typeof(POLine.closed))]
			[PXUIField(DisplayName = "Closed", Visibility = PXUIVisibility.Visible)]
			public virtual bool? Closed
			{
				get;
				set;
			}
			#endregion
			#region Billed
			public abstract class billed : IBqlField
			{
			}
			/// <summary>
			/// A flag that indicates (if set to <c>true</c>) that a PO Line is fully billed.
			/// </summary>
			[PXBool]
			[PXDBCalced(typeof(
				Switch<Case<Where<POLine.completedQty, Greater<POLine.billedQty>>, False>,
				Switch<Case<Where<POLine.completePOLine, Equal<CompletePOLineTypes.quantity>>,
					Switch<Case<Where<POLine.orderQty, LessEqual<decimal0>, Or<Div<Mult<POLine.orderQty, POLine.rcptQtyThreshold>, decimal100>, Greater<POLine.billedQty>>>, False>, True>>,
					Switch<Case<Where<Div<Mult<Add<POLine.curyExtCost, POLine.curyRetainageAmt>, POLine.rcptQtyThreshold>, decimal100>, Greater<POLine.curyBilledAmt>>, False>, True>>>),
				typeof(bool))]
			public virtual bool? Billed
			{
				get;
				set;
			}
			#endregion

			#region POAccrualType
			public abstract class pOAccrualType : IBqlField
			{
			}
			[PXDBString(1, IsFixed = true, BqlField = typeof(POLine.pOAccrualType))]
			public virtual string POAccrualType
			{
				get;
				set;
			}
			#endregion
			#region OrderNoteID
			public abstract class orderNoteID : IBqlField
			{
			}
			[PXDBGuid(BqlField = typeof(POLine.orderNoteID))]
			public virtual Guid? OrderNoteID
			{
				get;
				set;
			}
			#endregion

			#region DiscountID
			public abstract class discountID : PX.Data.IBqlField
			{
			}
			protected String _DiscountID;
			[PXDBString(10, IsUnicode = true, BqlField = typeof(POLine.discountID))]
			[PXSelector(typeof(Search<APDiscount.discountID, Where<APDiscount.bAccountID, Equal<Current<POLineS.vendorID>>, And<APDiscount.type, Equal<DiscountType.LineDiscount>>>>))]
			[PXUIField(DisplayName = "Discount Code", Visible = true, Enabled = false)]
			public virtual String DiscountID
			{
				get
				{
					return this._DiscountID;
				}
				set
				{
					this._DiscountID = value;
				}
			}
			#endregion
			#region DiscountSequenceID
			public abstract class discountSequenceID : PX.Data.IBqlField
			{
			}
			protected String _DiscountSequenceID;
			[PXDBString(10, IsUnicode = true, BqlField = typeof(POLine.discountSequenceID))]
			[PXUIField(DisplayName = "Discount Sequence", Visible = false, Enabled = false)]
			public virtual String DiscountSequenceID
			{
				get
				{
					return this._DiscountSequenceID;
				}
				set
				{
					this._DiscountSequenceID = value;
				}
			}
			#endregion

			#region RefNoteID
			public abstract class refNoteID : IBqlField
			{
			}
			[PXDBGuid(BqlField = typeof(POAccrualStatus.refNoteID))]
			public virtual Guid? RefNoteID
			{
				get;
				set;
			}
			#endregion
			#region BilledUOM
			public abstract class billedUOM : IBqlField
			{
			}
			[PXDBString(6, IsUnicode = true, BqlField = typeof(POAccrualStatus.billedUOM))]
			public virtual string BilledUOM
			{
				get;
				set;
			}
			#endregion
			#region BilledQty
			public abstract class billedQty : IBqlField
			{
			}
			[PXDBQuantity(BqlField = typeof(POAccrualStatus.billedQty))]
			public virtual decimal? BilledQty
			{
				get;
				set;
			}
			#endregion
			#region BaseBilledQty
			public abstract class baseBilledQty : IBqlField
			{
			}
			[PXDBCalced(typeof(IsNull<POAccrualStatus.baseBilledQty, decimal0>), typeof(decimal))]
			[PXQuantity]
			public virtual decimal? BaseBilledQty
			{
				get;
				set;
			}
			#endregion
			#region CuryBilledAmt
			public abstract class curyBilledAmt : IBqlField
			{
			}
			[PXDBCalced(typeof(IsNull<POAccrualStatus.curyBilledAmt, decimal0>), typeof(decimal))]
			[PXBaseCury]
			public virtual decimal? CuryBilledAmt
			{
				get;
				set;
			}
			#endregion
			#region BilledAmt
			public abstract class billedAmt : IBqlField
			{
			}
			[PXDBCalced(typeof(IsNull<POAccrualStatus.billedAmt, decimal0>), typeof(decimal))]
			[PXBaseCury]
			public virtual decimal? BilledAmt
			{
				get;
				set;
			}
			#endregion
			#region CuryBilledCost
			public abstract class curyBilledCost : IBqlField
			{
			}
			[PXDBCalced(typeof(IsNull<POAccrualStatus.curyBilledCost, decimal0>), typeof(decimal))]
			[PXBaseCury]
			public virtual decimal? CuryBilledCost
			{
				get;
				set;
			}
			#endregion
			#region BilledCost
			public abstract class billedCost : IBqlField
			{
			}
			[PXDBCalced(typeof(IsNull<POAccrualStatus.billedCost, decimal0>), typeof(decimal))]
			[PXBaseCury]
			public virtual decimal? BilledCost
			{
				get;
				set;
			}
			#endregion
			#region CuryBilledDiscAmt
			public abstract class curyBilledDiscAmt : IBqlField
			{
			}
			[PXDBCalced(typeof(IsNull<POAccrualStatus.curyBilledDiscAmt, decimal0>), typeof(decimal))]
			[PXBaseCury]
			public virtual decimal? CuryBilledDiscAmt
			{
				get;
				set;
			}
			#endregion
			#region BilledDiscAmt
			public abstract class billedDiscAmt : IBqlField
			{
			}
			[PXDBCalced(typeof(IsNull<POAccrualStatus.billedDiscAmt, decimal0>), typeof(decimal))]
			[PXBaseCury]
			public virtual decimal? BilledDiscAmt
			{
				get;
				set;
			}
			#endregion
			#region ReceivedUOM
			public abstract class receivedUOM : IBqlField
			{
			}
			[PXDBString(6, IsUnicode = true, BqlField = typeof(POAccrualStatus.receivedUOM))]
			public virtual string ReceivedUOM
			{
				get;
				set;
			}
			#endregion
			#region ReceivedQty
			public abstract class receivedQty : IBqlField
			{
			}
			[PXDBQuantity(BqlField = typeof(POAccrualStatus.receivedQty))]
			public virtual decimal? ReceivedQty
			{
				get;
				set;
			}
			#endregion
			#region BaseReceivedQty
			public abstract class baseReceivedQty : IBqlField
			{
			}
			[PXDBCalced(typeof(IsNull<POAccrualStatus.baseReceivedQty, decimal0>), typeof(decimal))]
			[PXQuantity]
			public virtual decimal? BaseReceivedQty
			{
				get;
				set;
			}
			#endregion
			#region ReceivedCost
			public abstract class receivedCost : IBqlField
			{
			}
			[PXDBCalced(typeof(IsNull<POAccrualStatus.receivedCost, decimal0>), typeof(decimal))]
			[PXBaseCury]
			public virtual decimal? ReceivedCost
			{
				get;
				set;
			}
			#endregion
			#region PPVAmt
			public abstract class pPVAmt : IBqlField
			{
			}
			[PXDBCalced(typeof(IsNull<POAccrualStatus.pPVAmt, decimal0>), typeof(decimal))]
			[PXBaseCury]
			public virtual decimal? PPVAmt
			{
				get;
				set;
			}
			#endregion

			#region UnbilledQty
			public abstract class unbilledQty : IBqlField
			{
			}
			[PXDBQuantity(typeof(uOM), typeof(baseUnbilledQty), HandleEmptyKey = true, BqlField = typeof(POLine.unbilledQty))]
			[PXUIField(DisplayName = "Unbilled Qty.", Enabled = false)]
			public virtual decimal? UnbilledQty
			{
				get;
				set;
			}
			#endregion
			#region BaseUnbilledQty
			public abstract class baseUnbilledQty : IBqlField
			{
			}
			[PXDBDecimal(6, BqlField = typeof(POLine.baseUnbilledQty))]
			public virtual decimal? BaseUnbilledQty
			{
				get;
				set;
			}
			#endregion
			#region CuryUnbilledAmt
			public abstract class curyUnbilledAmt : IBqlField
			{
			}
			[PXDBCurrency(typeof(curyInfoID), typeof(unbilledAmt), BqlField = typeof(POLine.curyUnbilledAmt))]
			[PXUIField(DisplayName = "Unbilled Amount", Enabled = false)]
			public virtual decimal? CuryUnbilledAmt
			{
				get;
				set;
			}
			#endregion
			#region UnbilledAmt
			public abstract class unbilledAmt : IBqlField
			{
			}

			[PXDBBaseCury(BqlField = typeof(POLine.unbilledAmt))]
			public virtual decimal? UnbilledAmt
			{
				get;
				set;
			}
			#endregion

			#region IAPTranSource Members

			string IAPTranSource.OrigUOM
			{
				get
				{
					return this.UOM;
				}
			}

			decimal? IAPTranSource.BaseOrigQty
			{
				get
				{
					return this.BaseOrderQty;
				}
			}

			decimal? IAPTranSource.BillQty
			{
				get
				{
					if (this.RefNoteID == null)
					{
						return this.OrderQty;
					}
					decimal? receivedQty = (this.ReceivedQty == 0m || this.ReceivedUOM == this.UOM) ? this.ReceivedQty : null;
					if (receivedQty == null)
					{
						return null;
					}
					decimal? billedQty = (this.BilledQty == 0m || this.BilledUOM == this.UOM) ? this.BilledQty : null;
					if (billedQty == null)
					{
						return null;
					}
					decimal? qtyToBill = ((this.OrderQty < receivedQty) ? receivedQty : this.OrderQty) - billedQty;
					return qtyToBill < 0m ? 0m : qtyToBill;
				}
			}

			decimal? IAPTranSource.BaseBillQty
			{
				get
				{
					if (this.RefNoteID == null)
					{
						return this.BaseOrderQty;
					}
					decimal? baseQtyToBill = ((this.BaseOrderQty < this.BaseReceivedQty) ? this.BaseReceivedQty : this.BaseOrderQty) - this.BaseBilledQty;
					return baseQtyToBill < 0m ? 0m : baseQtyToBill;
				}
			}

			bool IAPTranSource.IsPartiallyBilled
			{
				[PXDependsOnFields(typeof(baseBilledQty), typeof(curyBilledCost))]
				get
				{
					return this.BaseBilledQty != 0m || this.CuryBilledCost != 0m;
				}
			}

			Guid? IAPTranSource.POAccrualRefNoteID
			{
				get
				{
					return this.POAccrualType == Objects.PO.POAccrualType.Order ? this.OrderNoteID : null;
				}
			}

			int? IAPTranSource.POAccrualLineNbr
			{
				get
				{
					return this.POAccrualType == Objects.PO.POAccrualType.Order ? this.LineNbr : null;
				}
			}

			public virtual bool CompareReferenceKey(APTran aTran)
			{
				return (aTran.POAccrualType == this.POAccrualType
					&& aTran.POAccrualRefNoteID == ((IAPTranSource)this).POAccrualRefNoteID
					&& aTran.POAccrualLineNbr == ((IAPTranSource)this).POAccrualLineNbr);
			}

			public virtual void SetReferenceKeyTo(APTran aTran)
			{
				aTran.POAccrualType = this.POAccrualType;
				aTran.PONbr = this.OrderNbr;
				aTran.POOrderType = this.OrderType;
				aTran.POLineNbr = this.LineNbr;
				aTran.POAccrualRefNoteID = ((IAPTranSource)this).POAccrualRefNoteID;
				aTran.POAccrualLineNbr = ((IAPTranSource)this).POAccrualLineNbr;
			}

			public virtual bool IsReturn
			{
				get { return false; }
			}

			public virtual bool AggregateWithExistingTran
			{
				get { return false; }
			}

			#endregion
		}
	}
}