﻿using System;
using PX.Data;
using PX.Objects.CS;

namespace PX.Objects.SO
{
	[Serializable]
	[PXHidden]
	[PXProjection(typeof(Select<AR.ARTran>), Persistent = false)]
	public class ARTranAsSplit : IBqlTable, IN.ILSDetail
	{
		#region TranType
		public abstract class tranType : IBqlField { }

		[PXDBString(3, IsKey = true, IsFixed = true, BqlField = typeof(AR.ARTran.tranType))]
		public virtual string TranType { get; set; }
		#endregion

		#region RefNbr
		public abstract class refNbr : IBqlField { }

		[PXDBString(15, IsUnicode = true, IsKey = true, BqlField = typeof(AR.ARTran.refNbr))]
		public virtual string RefNbr { get; set; }
		#endregion

		#region LineNbr
		public abstract class lineNbr : IBqlField { }

		[PXDBInt(IsKey = true, BqlField = typeof(AR.ARTran.lineNbr))]
		[PXParent(typeof(Select<AR.ARTran, Where<AR.ARTran.tranType, Equal<Current<ARTranAsSplit.tranType>>, And<AR.ARTran.refNbr, Equal<Current<ARTranAsSplit.refNbr>>,
			And<AR.ARTran.lineNbr, Equal<Current<ARTranAsSplit.lineNbr>>>>>>))]
		public virtual int? LineNbr { get; set; }
		#endregion

		#region SplitLineNbr
		public abstract class splitLineNbr : IBqlField { }

		[PXInt(IsKey = true)]
		[PXFormula(typeof(int1))]
		public virtual int? SplitLineNbr { get; set; }
		#endregion

		#region LineType
		public abstract class lineType : IBqlField { }

		[PXDBString(2, IsFixed = true, BqlField = typeof(AR.ARTran.lineType))]
		public virtual string LineType { get; set; }
		#endregion

		#region TranDate
		public abstract class tranDate : IBqlField { }

		[PXDBDate(BqlField = typeof(AR.ARTran.tranDate))]
		public virtual DateTime? TranDate { get; set; }
		#endregion

		#region InventoryID
		public abstract class inventoryID : IBqlField { }

		[IN.NonStockNonKitCrossItem(IN.INPrimaryAlternateType.CPN, Messages.CannotAddNonStockKitDirectly, typeof(AR.ARTran.sOOrderNbr),
			typeof(FeaturesSet.advancedSOInvoices), BqlField = typeof(AR.ARTran.inventoryID))]
		public virtual int? InventoryID { get; set; }
		#endregion

		#region SubItemID
		public abstract class subItemID : IBqlField { }

		[IN.SubItem(BqlField = typeof(AR.ARTran.subItemID))]
		public virtual int? SubItemID { get; set; }
		#endregion

		#region IsStockItem
		public abstract class isStockItem : IBqlField { }

		[PXBool]
		[PXFormula(typeof(Selector<ARTranAsSplit.inventoryID, IN.InventoryItem.stkItem>))]
		public bool? IsStockItem { get; set; }
		#endregion

		#region InvtMult
		public abstract class invtMult : IBqlField { }

		[PXShort]
		[PXDBCalced(typeof(Switch<Case<Where<AR.ARTran.qty, Less<decimal0>>, Minus<AR.ARTran.invtMult>>, AR.ARTran.invtMult>), typeof(short))]
		public virtual short? InvtMult { get; set; }
		#endregion

		#region SiteID
		public abstract class siteID : IBqlField { }

		[IN.SiteAvail(typeof(ARTranAsSplit.inventoryID), typeof(ARTranAsSplit.subItemID), BqlField = typeof(AR.ARTran.siteID))]
		public virtual int? SiteID { get; set; }
		#endregion

		#region LocationID
		public abstract class locationID : IBqlField { }

		[IN.Location(BqlField = typeof(AR.ARTran.locationID))]
		public virtual int? LocationID { get; set; }
		#endregion

		#region UOM
		public abstract class uOM : IBqlField { }

		[IN.INUnit(typeof(ARTranAsSplit.inventoryID), BqlField = typeof(AR.ARTran.uOM))]
		public virtual string UOM { get; set; }
		#endregion

		#region Qty
		public abstract class qty : IBqlField { }

		[IN.PXQuantity(typeof(ARTranAsSplit.uOM), typeof(ARTranAsSplit.baseQty))]
		[PXDBCalced(typeof(Switch<Case<Where<AR.ARTran.qty, Less<decimal0>>, Minus<AR.ARTran.qty>>, AR.ARTran.qty>), typeof(decimal))]
		public virtual decimal? Qty { get; set; }
		#endregion

		#region BaseQty
		public abstract class baseQty : IBqlField { }

		[IN.PXQuantity]
		[PXDBCalced(typeof(Switch<Case<Where<AR.ARTran.qty, Less<decimal0>>, Minus<AR.ARTran.baseQty>>, AR.ARTran.baseQty>), typeof(decimal))]
		public virtual decimal? BaseQty { get; set; }
		#endregion

		#region LotSerialNbr
		public abstract class lotSerialNbr : IBqlField { }

		[LSARTran.ARLotSerialNbr(typeof(ARTranAsSplit.inventoryID), typeof(ARTranAsSplit.subItemID), typeof(ARTranAsSplit.locationID),
			PersistingCheck = PXPersistingCheck.Nothing, FieldClass = "LotSerial", BqlField = typeof(AR.ARTran.lotSerialNbr))]
		public virtual string LotSerialNbr { get; set; }
		#endregion

		#region LotSerClassID
		public abstract class lotSerClassID : IBqlField { }

		[PXString(10, IsUnicode = true)]
		public virtual string LotSerClassID { get; set; }
		#endregion

		#region AssignedNbr
		public abstract class assignedNbr : IBqlField { }

		[PXString(30, IsUnicode = true)]
		public virtual string AssignedNbr { get; set; }
		#endregion

		#region ExpireDate
		public abstract class expireDate : IBqlField { }

		[PXDBDate(BqlField = typeof(AR.ARTran.expireDate))]
		public virtual DateTime? ExpireDate { get; set; }
		#endregion

		#region ProjectID
		public abstract class projectID : IBqlField { }

		[PXDBInt(BqlField = typeof(AR.ARTran.projectID))]
		public virtual int? ProjectID { get; set; }
		#endregion

		#region TaskID
		public abstract class taskID : IBqlField { }

		[PXDBInt(BqlField = typeof(AR.ARTran.taskID))]
		public virtual int? TaskID { get; set; }
		#endregion

		public static ARTranAsSplit FromARTran(AR.ARTran item)
		{
			ARTranAsSplit ret = new ARTranAsSplit
			{
				TranType = item.TranType,
				RefNbr = item.RefNbr,
				LineNbr = item.LineNbr,
				SplitLineNbr = 1,
				TranDate = item.TranDate,
				InventoryID = item.InventoryID,
				SubItemID = item.SubItemID,
				InvtMult = item.InvtMult,
				SiteID = item.SiteID,
				LocationID = item.LocationID,
				UOM = item.UOM,
				Qty = item.Qty,
				BaseQty = item.BaseQty,
				LotSerialNbr = item.LotSerialNbr,
				ExpireDate = item.ExpireDate,
				ProjectID = item.ProjectID,
				TaskID = item.TaskID
			};

			return ret;
		}

	}
}
