﻿using PX.Data;
using PX.Objects.CS;
using PX.Objects.IN;
using PX.Objects.PO;
using System;

namespace PX.Objects.AP
{
	public partial class APInvoiceEntry
	{
		[Obsolete(Common.Messages.WillBeRemovedInAcumatica2019R1)]
		public partial class LinkLineFilter : IBqlTable
		{
			#region POOrderNbr
			public abstract class pOOrderNbr : IBqlField { }
			[PXDBString(15, IsUnicode = true, InputMask = "")]
			[PXUIField(DisplayName = "Order Nbr.")]
			[PXSelector(typeof(Search5<POOrder.orderNbr,
				LeftJoin<LinkLineReceipt,
					On<POOrder.orderNbr, Equal<LinkLineReceipt.orderNbr>,
						And<POOrder.orderType, Equal<LinkLineReceipt.orderType>,
						And<Current<LinkLineFilter.selectedMode>, Equal<LinkLineFilter.selectedMode.receipt>>>>,
				LeftJoin<LinkLineOrder,
					On<POOrder.orderNbr, Equal<LinkLineOrder.orderNbr>,
						And<POOrder.orderType, Equal<LinkLineOrder.orderType>,
						And<Current<LinkLineFilter.selectedMode>, Equal<LinkLineFilter.selectedMode.order>>>>>>,
				Where2<
					Where<
						LinkLineReceipt.orderNbr, IsNotNull,
						Or<LinkLineOrder.orderType, IsNotNull>>,
					And<Where<
							POOrder.vendorID, Equal<Current<APInvoice.vendorID>>,
							And<POOrder.vendorLocationID, Equal<Current<APInvoice.vendorLocationID>>,
							And2<Not<FeatureInstalled<FeaturesSet.vendorRelations>>,
						Or2<FeatureInstalled<FeaturesSet.vendorRelations>,
							And<POOrder.vendorID, Equal<Current<APInvoice.suppliedByVendorID>>,
							And<POOrder.vendorLocationID, Equal<Current<APInvoice.suppliedByVendorLocationID>>,
							And<POOrder.payToVendorID, Equal<Current<APInvoice.vendorID>>>>>>>>>>>,
				Aggregate<
					GroupBy<POOrder.orderNbr,
					GroupBy<POOrder.orderType>>>>))]
			public virtual string POOrderNbr { get; set; }
			#endregion

			#region POReceiptNbr
			public abstract class pOReceiptNbr : PX.Data.IBqlField
			{
			}
			protected String _POReceiptNbr;
			[PXDBString(15, IsUnicode = true, InputMask = "")]
			[PXUIField(DisplayName = "Receipt Nbr.")]
			[PXSelector(typeof(Search<POReceipt.receiptNbr>))]
			public virtual String POReceiptNbr
			{
				get
				{
					return this._POReceiptNbr;
				}
				set
				{
					this._POReceiptNbr = value;
				}
			}
			#endregion

			#region SiteID
			public abstract class siteID : PX.Data.IBqlField
			{
			}
			protected Int32? _SiteID;
			[PXDBInt()]
			[PXUIField(DisplayName = "Warehouse", FieldClass = SiteAttribute.DimensionName)]
			[PXSelector(typeof(Search5<
				INSite.siteID
					, LeftJoin<LinkLineReceipt, On<INSite.siteID, Equal<LinkLineReceipt.receiptSiteID>, And<Current<LinkLineFilter.selectedMode>, Equal<LinkLineFilter.selectedMode.receipt>>>
						, LeftJoin<LinkLineOrder, On<INSite.siteID, Equal<LinkLineOrder.orderSiteID>, And<Current<LinkLineFilter.selectedMode>, Equal<LinkLineFilter.selectedMode.order>>>>
					>
				, Where<LinkLineReceipt.receiptSiteID, IsNotNull, Or<LinkLineOrder.orderSiteID, IsNotNull>>
				, Aggregate<GroupBy<INSite.siteID>>>), SubstituteKey = typeof(INSite.siteCD), DescriptionField = typeof(INSite.descr))]
			[PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
			[PXFormula(typeof(Default<LinkLineFilter.selectedMode>))]
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

			#region selectedMode
			public abstract class selectedMode : PX.Data.IBqlField
			{
				public const string Order = "O";
				public const string Receipt = "R";
				public const string LandedCost = "L";
				public class order : Constant<string>
				{
					public order() : base(Order) { }
				}

				public class receipt : Constant<string>
				{
					public receipt() : base(Receipt) { }
				}

				public class landedCost : Constant<string>
				{
					public landedCost() : base(LandedCost) { }
				}
			}
			protected String _SelectedMode;

			[PXDBString(1)]
			[PXFormula(typeof(Switch<Case<Where<Selector<inventoryID, InventoryItem.stkItem>, NotEqual<True>, And<Selector<inventoryID, InventoryItem.nonStockReceipt>, NotEqual<True>>>, selectedMode.order>, selectedMode.receipt>))]
			[PXUIField(DisplayName = "Selected Mode")]
			[PXStringList(new[] { selectedMode.Order, selectedMode.Receipt, selectedMode.LandedCost }, new[] { AP.Messages.POOrderMode, AP.Messages.POReceiptMode, AP.Messages.POLandedCostMode })]
			public virtual String SelectedMode
			{
				get
				{
					return this._SelectedMode;
				}
				set
				{
					this._SelectedMode = value;
				}
			}
			#endregion
			#region InventoryID
			public abstract class inventoryID : PX.Data.IBqlField
			{
			}
			protected Int32? _InventoryID;

			[Inventory(Enabled = false)]
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

			#region UOM
			public abstract class uOM : PX.Data.IBqlField
			{
			}
			protected String _UOM;

			[INUnit(typeof(inventoryID), Enabled = false)]
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

		}
	}
}