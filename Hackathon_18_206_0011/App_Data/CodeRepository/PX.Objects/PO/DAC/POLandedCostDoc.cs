﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PX.Data;
using PX.Data.EP;
using PX.Data.ReferentialIntegrity.Attributes;
using PX.Objects.AP;
using PX.Objects.AP.MigrationMode;
using PX.Objects.AR;
using PX.Objects.CM;
using PX.Objects.CR;
using PX.Objects.CS;
using PX.Objects.GL;
using PX.Objects.PO.LandedCosts;
using PX.Objects.PO.LandedCosts.Attributes;
using CRLocation = PX.Objects.CR.Standalone.Location;


namespace PX.Objects.PO
{
    [PXTable()]
    [Serializable]
	[PXCacheName(Messages.POLandedCostDoc)]
	[PXPrimaryGraph(typeof(POLandedCostDocEntry))]
	public partial class POLandedCostDoc : PX.Data.IBqlTable, IAssign
	{
		#region Selected
		public abstract class selected : IBqlField
		{
		}
		[PXBool]
		[PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
		[PXUIField(DisplayName = "Selected")]
		public virtual bool? Selected
		{
			get;
			set;
		}
		#endregion
		#region DocType
		public abstract class docType : PX.Data.IBqlField
		{
		}

		/// <summary>
		/// [key] Type of the document.
		/// </summary>
		/// <value>
		/// Possible values are: "LC" - Landed Cost, "LCC" - Correction, "LCR" - Reversal.
		/// </value>
		[PXDBString(1, IsKey = true, IsFixed = true)]
		[PXDefault(POLandedCostDocType.LandedCost)]
		[POLandedCostDocType.List()]
		[PXUIField(DisplayName = "Type", Visibility = PXUIVisibility.SelectorVisible, Enabled = true, TabOrder = 0)]
		public virtual String DocType
		{
			get;
			set;
		}
		#endregion
		#region RefNbr
		public abstract class refNbr : PX.Data.IBqlField
		{
		}

		/// <summary>
		/// [key] Reference number of the document.
		/// </summary>
		[PXDBString(15, IsUnicode = true, IsKey = true, InputMask = "")]
		[PXDefault()]
		[PXUIField(DisplayName = "Reference Nbr.", Visibility = PXUIVisibility.SelectorVisible, TabOrder = 1)]
		[PXSelector(typeof(Search<POLandedCostDoc.refNbr, Where<POLandedCostDoc.docType, Equal<Optional<POLandedCostDoc.docType>>>>),
			Filterable = true)]
		[PXFieldDescription]
		[LandedCostDocNumbering]
		public virtual String RefNbr
		{
			get;
			set; 
		}

		#endregion
		#region BranchID
		public abstract class branchID : PX.Data.IBqlField
		{
		}

		/// <summary>
		/// The identifier of the <see cref="Branch">branch</see> to which the document belongs.
		/// </summary>
		/// <value>
		/// Corresponds to the <see cref="Branch.BranchID"/> field.
		/// </value>
		[Branch(typeof(Coalesce<
			Search<Location.vBranchID, Where<Location.bAccountID, Equal<Current<vendorID>>, And<Location.locationID, Equal<Current<vendorLocationID>>>>>,
			Search<Branch.branchID, Where<Branch.branchID, Equal<Current<AccessInfo.branchID>>>>>), IsDetail = false)]
		public virtual Int32? BranchID
		{
			get;
			set;
		}
		#endregion

		#region OpenDoc
		public abstract class openDoc : PX.Data.IBqlField
		{
		}
		protected Boolean? _OpenDoc;

		/// <summary>
		/// When set to <c>true</c> indicates that the document is open.
		/// </summary>
		[PXDBBool()]
		[PXDefault(true)]
		[PXUIField(DisplayName = "Open", Visible = false)]
		public virtual Boolean? OpenDoc
		{
			get
			{
				return this._OpenDoc;
			}
			set
			{
				this._OpenDoc = value;
			}
		}
		#endregion
		#region Released
		public abstract class released : PX.Data.IBqlField
		{
		}
		protected Boolean? _Released;

		/// <summary>
		/// When set to <c>true</c> indicates that the document was released.
		/// </summary>
		[PXDBBool()]
		[PXDefault(false)]
		[PXUIField(DisplayName = "Released", Visible = false, Enabled = false)]
		public virtual Boolean? Released
		{
			get
			{
				return this._Released;
			}
			set
			{
				this._Released = value;
			}
		}
		#endregion
		#region Hold
		public abstract class hold : PX.Data.IBqlField
		{
		}
		protected Boolean? _Hold;

		/// <summary>
		/// When set to <c>true</c> indicates that the document is on hold and thus cannot be released.
		/// </summary>
		[PXDBBool()]
		[PXUIField(DisplayName = "Hold", Visibility = PXUIVisibility.Visible)]
		[PXDefault(true, typeof(POSetup.holdLandedCosts))]
		public virtual Boolean? Hold
		{
			get
			{
				return this._Hold;
			}
			set
			{
				this._Hold = value;
			}
		}
		#endregion
		#region Status
		public abstract class status : IBqlField { }

		/// <summary>
		/// Status of the document. The field is calculated based on the values of status flag. It can't be changed directly.
		/// The fields tht determine status of a document are: <see cref="POLandedCostDocStatus.Hold"/>, <see cref="POLandedCostDocStatus.Balanced"/>, <see cref="POLandedCostDocStatus.Released"/>.
		/// </summary>
		/// <value>
		/// Possible values are: 
		/// <c>"H"</c> - Hold, <c>"B"</c> - Balanced, <c>"R"</c> - Released.
		/// Defaults to Hold.
		/// </value>
		[PXDBString(1, IsFixed = true)]
		[PXDefault(POLandedCostDocStatus.Hold)]
		[PXUIField(DisplayName = "Status", Visibility = PXUIVisibility.SelectorVisible, Enabled = false)]
		[POLandedCostDocStatus.List]
		[LandedCostDocSetStatus]
		[PXDependsOnFields(
			typeof(POLandedCostDoc.hold),
			typeof(POLandedCostDoc.released))]
		public virtual string Status
		{
			get;
			set;
		}
		#endregion

		#region IsTaxValid
		public abstract class isTaxValid : PX.Data.IBqlField
		{
		}

		/// <summary>
		/// When <c>true</c>, indicates that the amount of tax calculated with the external External Tax Provider is up to date.
		/// If this field equals <c>false</c>, the document was updated since last synchronization with the Tax Engine
		/// and taxes might need recalculation.
		/// </summary>
		[PXDBBool()]
		[PXDefault(false)]
		[PXUIField(DisplayName = "Tax is up to date", Enabled = false)]
		public virtual Boolean? IsTaxValid
		{
			get;
			set;
		}
		#endregion
		#region NonTaxable
		public abstract class nonTaxable : PX.Data.IBqlField
		{
		}
		/// <summary>
		/// Get or set NonTaxable that mark current document does not impose sales taxes.
		/// </summary>
		[PXDBBool]
		[PXDefault(false)]
		[PXUIField(DisplayName = "Non-Taxable", Enabled = false)]
		public virtual Boolean? NonTaxable
		{
			get;
			set;
		}
		#endregion

		#region DocDate
		public abstract class docDate : PX.Data.IBqlField
		{
		}
		protected DateTime? _DocDate;

		/// <summary>
		/// Date of the document.
		/// </summary>
		[PXDBDate()]
		[PXDefault(typeof(AccessInfo.businessDate))]
		[PXUIField(DisplayName = "Date", Visibility = PXUIVisibility.SelectorVisible)]
		public virtual DateTime? DocDate
		{
			get
			{
				return this._DocDate;
			}
			set
			{
				this._DocDate = value;
			}
		}
		#endregion
		#region TranPeriodID
		public abstract class tranPeriodID : PX.Data.IBqlField
		{
		}

		/// <summary>
		/// <see cref="FinPeriod">Financial Period</see> of the document.
		/// </summary>
		/// <value>
		/// Determined by the <see cref="POLandedCostDoc.DocDate">date of the document</see>. Unlike <see cref="POLandedCostDoc.FinPeriodID"/>
		/// the value of this field can't be overriden by user.
		/// </value>
		[TranPeriodID(typeof(POLandedCostDoc.docDate), typeof(POLandedCostDoc.branchID))]
		[PXUIField(DisplayName = "Transaction Period")]
		public virtual String TranPeriodID
		{
			get;
			set;
		}
		#endregion
		#region FinPeriodID
		public abstract class finPeriodID : PX.Data.IBqlField
		{
		}

		/// <summary>
		/// <see cref="FinPeriod">Financial Period</see> of the document.
		/// </summary>
		/// <value>
		/// Defaults to the period, to which the <see cref="POLandedCostDoc.DocDate"/> belongs, but can be overriden by user.
		/// </value>
		[POOpenPeriod(typeof(POLandedCostDoc.docDate), typeof(POLandedCostDoc.branchID))]
		[PXDefault()]
		[PXUIField(DisplayName = "Post Period", Visibility = PXUIVisibility.SelectorVisible)]
		public virtual String FinPeriodID
		{
			get;
			set;
		}
		#endregion

		#region VendorID
		public abstract class vendorID : PX.Data.IBqlField
		{
		}
		/// <summary>
		/// Identifier of the <see cref="Vendor"/>, whom the document belongs to.
		/// </summary>
		[LandedCostVendorActive(Visibility = PXUIVisibility.SelectorVisible, DescriptionField = typeof(Vendor.acctName), CacheGlobal = true, Filterable = true)]
		[PXDefault]
		[PXForeignReference(typeof(Field<POLandedCostDoc.vendorID>.IsRelatedTo<BAccount.bAccountID>))]
		public virtual int? VendorID
		{
			get;
			set;
		}
		#endregion
		#region VendorLocationID
		public abstract class vendorLocationID : PX.Data.IBqlField
		{
		}

		/// <summary>
		/// Identifier of the <see cref="Location">Location</see> of the <see cref="Vendor">Vendor</see>, associated with the document.
		/// </summary>
		/// <value>
		/// Corresponds to the <see cref="Location.LocationID"/> field. Defaults to vendor's <see cref="Vendor.DefLocationID">default location</see>.
		/// </value>
		[LocationID(
			typeof(Where<Location.bAccountID, Equal<Optional<vendorID>>,
				And<Location.isActive, Equal<True>,
				And<MatchWithBranch<Location.vBranchID>>>>),
			DescriptionField = typeof(Location.descr),
			Visibility = PXUIVisibility.SelectorVisible)]
		[PXDefault(typeof(Coalesce<Search2<Vendor.defLocationID,
				InnerJoin<CRLocation, 
					On<CRLocation.locationID, Equal<Vendor.defLocationID>, 
					And<CRLocation.bAccountID, Equal<Vendor.bAccountID>>>>,
				Where<Vendor.bAccountID, Equal<Current<vendorID>>,
					And<CRLocation.isActive, Equal<True>, And<MatchWithBranch<CRLocation.vBranchID>>>>>,
			Search<CRLocation.locationID,
				Where<CRLocation.bAccountID, Equal<Current<vendorID>>,
					And<CRLocation.isActive, Equal<True>, And<MatchWithBranch<CRLocation.vBranchID>>>>>>))]
		/*[PXDefault(typeof(Coalesce<Search2<BAccountR.defLocationID,
				InnerJoin<CRLocation, On<CRLocation.bAccountID, Equal<BAccountR.bAccountID>, And<CRLocation.locationID, Equal<BAccountR.defLocationID>>>>,
				Where<BAccountR.bAccountID, Equal<Current<POLandedCostDoc.vendorID>>,
					And<CRLocation.isActive, Equal<True>,
						And<MatchWithBranch<CRLocation.vBranchID>>>>>,
			Search<CRLocation.locationID,
				Where<CRLocation.bAccountID, Equal<Current<POLandedCostDoc.vendorID>>,
					And<CRLocation.isActive, Equal<True>, And<MatchWithBranch<CRLocation.vBranchID>>>>>>))]*/
		[PXForeignReference(typeof(CompositeKey<Field<vendorID>.IsRelatedTo<Location.bAccountID>,Field<vendorLocationID>.IsRelatedTo<Location.locationID>>))]

		public virtual Int32? VendorLocationID
		{
			get;
			set;
		}
		#endregion

		#region CuryTaxTotal
		public abstract class curyTaxTotal : PX.Data.IBqlField
		{
		}
		[PXDefault(TypeCode.Decimal, "0.0")]
		[PXDBCurrency(typeof(curyInfoID), typeof(POLandedCostDoc.taxTotal))]
		[PXUIField(DisplayName = "Tax Total", Enabled = false)]
		public virtual Decimal? CuryTaxTotal
		{
			get;
			set;
		}
		#endregion
		#region TaxTotal
		public abstract class taxTotal : PX.Data.IBqlField
		{
		}
		[PXDBBaseCury()]
		[PXDefault(TypeCode.Decimal, "0.0")]
		[PXUIField(DisplayName = "Tax Total")]
		public virtual Decimal? TaxTotal
		{
			get;
			set;
		}
		#endregion

		#region LineCntr
		public abstract class lineCntr : PX.Data.IBqlField
		{
		}

		/// <summary>
		/// Counter of the document lines, used <i>internally</i> to assign numbers to newly created lines.
		/// It is not recommended to rely on this fields to determine the exact count of lines, because it might not reflect the latter under various conditions.
		/// </summary>
		[PXDBInt()]
		[PXDefault(0)]
		public virtual Int32? LineCntr
		{
			get;
			set;
		}
		#endregion
		#region CuryID
		public abstract class curyID : PX.Data.IBqlField
		{
		}

		/// <summary>
		/// Code of the <see cref="PX.Objects.CM.Currency">Currency</see> of the document.
		/// </summary>
		/// <value>
		/// Defaults to the <see cref="Company.BaseCuryID">company's base currency</see>.
		/// </value>
		[PXDBString(5, IsUnicode = true, InputMask = ">LLLLL")]
		[PXUIField(DisplayName = "Currency", Visibility = PXUIVisibility.SelectorVisible)]
		[PXDefault(typeof(Search<Vendor.curyID>))]
		[PXSelector(typeof(Currency.curyID))]
		public virtual String CuryID
		{
			get;
			set;
		}
		#endregion
		#region CuryInfoID
		public abstract class curyInfoID : PX.Data.IBqlField
		{
		}

		/// <summary>
		/// The identifier of the <see cref="CurrencyInfo">CurrencyInfo</see> object associated with the document.
		/// </summary>
		/// <value>
		/// Corresponds to the <see cref="CurrencyInfoID"/> field.
		/// </value>
		[PXDBLong()]
		[CurrencyInfo(ModuleCode = BatchModule.PO)]
		public virtual Int64? CuryInfoID
		{
			get;
			set;
		}
		#endregion
		#region CreateBill
		public abstract class createBill : PX.Data.IBqlField
		{
		}
		/// <summary>
		/// Get or set CreateBill that mark current document create bill on release.
		/// </summary>
		[PXDBBool]
		[PXDefault(typeof(POSetup.autoCreateLCAP))]
		[PXUIField(DisplayName = "Create Bill", Enabled = true)]
		public virtual Boolean? CreateBill
		{
			get;
			set;
		}
		#endregion
		#region VendorRefNbr
		public abstract class vendorRefNbr : PX.Data.IBqlField
		{
		}

		[PXDBString(40, IsUnicode = true)]
		[PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
		[PXUIField(DisplayName = "Vendor Ref.", Visibility = PXUIVisibility.SelectorVisible)]
		[POLandedCostDocVendorRefNbr]
		public virtual String VendorRefNbr
		{
			get;
			set;
		}
		#endregion

		#region CuryLineTotal
		public abstract class curyLineTotal : PX.Data.IBqlField
		{
		}

		/// <summary>
		/// The document total presented in the currency of the document. (See <see cref="CuryID"/>)
		/// </summary>
		[PXDBCurrency(typeof(curyInfoID), typeof(lineTotal))]
		[PXUIField(DisplayName = "Amount Total", Visibility = PXUIVisibility.Visible, Enabled = false)]
		[PXDefault(TypeCode.Decimal, "0.0")]
		public virtual Decimal? CuryLineTotal
		{
			get;
			set;
		}
		#endregion
		#region LineTotal
		public abstract class lineTotal : PX.Data.IBqlField
		{
		}

		/// <summary>
		/// The document total presented in the base currency of the company. (See <see cref="Company.BaseCuryID"/>)
		/// </summary>
		[PXDBBaseCury]
		[PXDefault(TypeCode.Decimal, "0.0")]
		public virtual Decimal? LineTotal
		{
			get;
			set;
		}
		#endregion

		#region CuryDocTotal
		public abstract class curyDocTotal : PX.Data.IBqlField
		{
		}

		/// <summary>
		/// The document total presented in the currency of the document. (See <see cref="CuryID"/>)
		/// </summary>
		[PXDBCurrency(typeof(curyInfoID), typeof(docTotal))]
		[PXUIField(DisplayName = "Document Total", Visibility = PXUIVisibility.Visible, Enabled = false)]
		[PXDefault(TypeCode.Decimal, "0.0")]
		public virtual Decimal? CuryDocTotal
		{
			get;
			set;
		}
		#endregion
		#region DocTotal
		public abstract class docTotal : PX.Data.IBqlField
		{
		}

		/// <summary>
		/// The document total presented in the base currency of the company. (See <see cref="Company.BaseCuryID"/>)
		/// </summary>
		[PXDBBaseCury]
		[PXDefault(TypeCode.Decimal, "0.0")]
		public virtual Decimal? DocTotal
		{
			get;
			set;
		}
		#endregion

		#region CuryAllocatedTotal
		public abstract class curyAllocatedTotal : PX.Data.IBqlField
		{
		}

		/// <summary>
		/// The total allocated amount of the document.
		/// Given in the <see cref="CuryID">currency of the document</see>.
		/// </summary>
		[PXDefault(TypeCode.Decimal, "0.0")]
		[PXDBCurrency(typeof(curyInfoID), typeof(allocatedTotal), BaseCalc = false)]
		[PXUIField(DisplayName = "Total Allocated Amount", Visibility = PXUIVisibility.SelectorVisible, Enabled = false)]
		public virtual Decimal? CuryAllocatedTotal
		{
			get;
			set;
		}
		#endregion
		#region AllocatedTotal
		public abstract class allocatedTotal : PX.Data.IBqlField
		{
		}

		/// <summary>
		/// The total allocated amount of the document.
		/// Given in the <see cref="Company.BaseCuryID">base currency of the company</see>.
		/// </summary>
		[PXDBBaseCury()]
		[PXDefault(TypeCode.Decimal, "0.0")]
		public virtual Decimal? AllocatedTotal
		{
			get;
			set;
		}
		#endregion

		#region CuryControlTotal
		public abstract class curyControlTotal : PX.Data.IBqlField
		{
		}

		[PXDBCurrency(typeof(curyInfoID), typeof(controlTotal))]
		[PXDefault(TypeCode.Decimal, "0.0")]
		[PXUIField(DisplayName = "Control Total")]
		public virtual Decimal? CuryControlTotal
		{
			get;
			set;
		}
		#endregion
		#region ControlTotal
		public abstract class controlTotal : PX.Data.IBqlField
		{
		}

		[PXDBBaseCury()]
		[PXDefault(TypeCode.Decimal, "0.0")]
		public virtual Decimal? ControlTotal
		{
			get;
			set;
		}
		#endregion

		#region TermsID
		public abstract class termsID : IBqlField { }

		[PXDBString(10, IsUnicode = true)]
		[PXDefault(
			typeof(Search<Vendor.termsID,
				Where2<FeatureInstalled<FeaturesSet.vendorRelations>,
					And<Vendor.bAccountID, Equal<Current<payToVendorID>>,
						Or2<Not<FeatureInstalled<FeaturesSet.vendorRelations>>,
							And<Vendor.bAccountID, Equal<Current<vendorID>>>>>>>),
			PersistingCheck = PXPersistingCheck.Nothing)]
		[PXUIField(DisplayName = "Terms", Visibility = PXUIVisibility.Visible)]
		[Terms(typeof(billDate), typeof(dueDate), typeof(discDate), typeof(curyLineTotal), typeof(curyDiscAmt))]
		[PXSelector(typeof(Search<Terms.termsID, Where<Terms.visibleTo, Equal<TermsVisibleTo.all>, Or<Terms.visibleTo, Equal<TermsVisibleTo.vendor>>>>), DescriptionField = typeof(Terms.descr), Filterable = true)]
		public virtual string TermsID
		{
			get;
			set;
		}
		#endregion
		#region BillDate
		public abstract class billDate : PX.Data.IBqlField
		{
		}
		[PXDBDate()]
		[PXUIField(DisplayName = "Bill Date", Visibility = PXUIVisibility.SelectorVisible)]
		[PXFormula(typeof(docDate))]
		[PXDefault(typeof(docDate), PersistingCheck = PXPersistingCheck.Nothing)]
		public virtual DateTime? BillDate
		{
			get;
			set;
		}
		#endregion
		#region DueDate
		public abstract class dueDate : PX.Data.IBqlField
		{
		}
		[PXDBDate()]
		[PXUIField(DisplayName = "Due Date", Visibility = PXUIVisibility.SelectorVisible)]
		public virtual DateTime? DueDate
		{
			get;
			set;
		}
		#endregion
		#region DiscDate
		public abstract class discDate : PX.Data.IBqlField
		{
		}
		[PXDBDate()]
		[PXUIField(DisplayName = "Cash Discount Date", Visibility = PXUIVisibility.SelectorVisible)]
		public virtual DateTime? DiscDate
		{
			get;
			set;
		}
		#endregion
		#region CuryDiscAmt
		public abstract class curyDiscAmt : PX.Data.IBqlField
		{
		}

		[PXDefault(TypeCode.Decimal, "0.0")]
		[PXDBCurrency(typeof(curyInfoID), typeof(discAmt))]
		[PXUIField(DisplayName = "Cash Discount", Visibility = PXUIVisibility.SelectorVisible)]
		public virtual Decimal? CuryDiscAmt
		{
			get;
			set;
		}
		#endregion
		#region DiscAmt
		public abstract class discAmt : PX.Data.IBqlField
		{
		}
		[PXDBBaseCury()]
		[PXDefault(TypeCode.Decimal, "0.0")]
		public virtual Decimal? DiscAmt
		{
			get;
			set;
		}
		#endregion
		#region TaxZoneID
		public abstract class taxZoneID : PX.Data.IBqlField
		{
		}

		[PXDBString(10, IsUnicode = true)]
		[PXDefault(typeof(Search<Location.vTaxZoneID, Where<Location.bAccountID, Equal<Current<vendorID>>, And<Location.locationID, Equal<Current<vendorLocationID>>>>>), PersistingCheck = PXPersistingCheck.Nothing)]
		[PXUIField(DisplayName = "Vendor Tax Zone", Visibility = PXUIVisibility.Visible)]
		[PXSelector(typeof(TX.TaxZone.taxZoneID), DescriptionField = typeof(TX.TaxZone.descr), Filterable = true)]
		[PXRestrictor(typeof(Where<TX.TaxZone.isManualVATZone, Equal<False>>), TX.Messages.CantUseManualVAT)]
		public virtual String TaxZoneID
		{
			get;
			set;
		}
		#endregion
		#region PayToVendorID
		public abstract class payToVendorID : IBqlField { }

		/// <summary>
		/// A reference to the <see cref="Vendor"/>.
		/// </summary>
		/// <value>
		/// An integer identifier of the vendor, whom the AP bill will belong to. 
		/// </value>
		[PXFormula(typeof(Validate<curyID>))]
		[POLandedCostPayToVendor(CacheGlobal = true, Filterable = true)]
		[PXDefault]
		[PXForeignReference(typeof(Field<payToVendorID>.IsRelatedTo<Vendor.bAccountID>))]
		public virtual int? PayToVendorID
		{
			get;
			set;
		}
		#endregion
		#region WorkgroupID
		public abstract class workgroupID : PX.Data.IBqlField
		{
		}
		[PXDBInt]
		[PXDefault(typeof(Vendor.workgroupID), PersistingCheck = PXPersistingCheck.Nothing)]
		[PX.TM.PXCompanyTreeSelector]
		[PXUIField(DisplayName = "Workgroup", Visibility = PXUIVisibility.Visible)]
		public virtual int? WorkgroupID
		{
			get;
			set;
		}
		#endregion
		#region OwnerID
		public abstract class ownerID : IBqlField
		{
		}
		protected Guid? _OwnerID;
		[PXDBGuid()]
		[PXDefault(typeof(Vendor.ownerID), PersistingCheck = PXPersistingCheck.Nothing)]
		[PX.TM.PXOwnerSelector(typeof(workgroupID))]
		[PXUIField(DisplayName = "Owner", Visibility = PXUIVisibility.SelectorVisible)]
		public virtual Guid? OwnerID
		{
			get;
			set;
		}
		#endregion
		#region IAssign Members

		int? PX.Data.EP.IAssign.WorkgroupID
		{
			get
			{
				return WorkgroupID;
			}
			set
			{
				WorkgroupID = value;
			}
		}

		Guid? PX.Data.EP.IAssign.OwnerID
		{
			get
			{
				return OwnerID;
			}
			set
			{
				OwnerID = value;
			}
		}

		#endregion

		#region APDocCreated
		public abstract class aPDocCreated : PX.Data.IBqlField
		{
		}

		[PXDBBool()]
		[PXDefault(false)]
		public virtual Boolean? APDocCreated
		{
			get;
			set;
		}
		#endregion
		#region INDocCreated
		public abstract class iNDocCreated : PX.Data.IBqlField
		{
		}

		[PXDBBool()]
		[PXDefault(false)]
		public virtual Boolean? INDocCreated
		{
			get;
			set;
		}
		#endregion

		#region CuryVatExemptTotal
		public abstract class curyVatExemptTotal : PX.Data.IBqlField
		{
		}

		/// <summary>
		/// The part of the document total that is exempt from VAT. 
		/// This total is calculated as a sum of the taxable amounts for the <see cref="PX.Objects.TX.Tax">taxes</see>
		/// of <see cref="PX.Objects.TX.Tax.TaxType">type</see> VAT, which are marked as <see cref="PX.Objects.TX.Tax.ExemptTax">exempt</see> 
		/// and are neither <see cref="PX.Objects.TX.Tax.StatisticalTax">statistical</see> nor <see cref="PX.Objects.TX.Tax.ReverseTax">reverse</see>.
		/// (Presented in the currency of the document, see <see cref="CuryID"/>)
		/// </summary>
		[PXDBCurrency(typeof(curyInfoID), typeof(vatExemptTotal))]
		[PXUIField(DisplayName = "VAT Exempt Total", Visibility = PXUIVisibility.Visible, Enabled = false)]
		[PXDefault(TypeCode.Decimal, "0.0")]
		public virtual Decimal? CuryVatExemptTotal
		{
			get;
			set;
		}
		#endregion
		#region VatExemptTaxTotal
		public abstract class vatExemptTotal : PX.Data.IBqlField
		{
		}

		/// <summary>
		/// The part of the document total that is exempt from VAT. 
		/// This total is calculated as a sum of the taxable amounts for the <see cref="PX.Objects.TX.Tax">taxes</see>
		/// of <see cref="PX.Objects.TX.Tax.TaxType">type</see> VAT, which are marked as <see cref="PX.Objects.TX.Tax.ExemptTax">exempt</see> 
		/// and are neither <see cref="PX.Objects.TX.Tax.StatisticalTax">statistical</see> nor <see cref="PX.Objects.TX.Tax.ReverseTax">reverse</see>.
		/// (Presented in the base currency of the company, see <see cref="Company.BaseCuryID"/>)
		/// </summary>
		[PXDBBaseCury]
		[PXDefault(TypeCode.Decimal, "0.0")]
		public virtual Decimal? VatExemptTotal
		{
			get;
			set;
		}
		#endregion
		#region CuryVatTaxableTotal
		public abstract class curyVatTaxableTotal : PX.Data.IBqlField
		{
		}

		/// <summary>
		/// The part of the document total, which is subject to VAT.
		/// This total is calculated as a sum of the taxable amounts for the <see cref="PX.Objects.TX.Tax">taxes</see>
		/// of <see cref="PX.Objects.TX.Tax.TaxType">type</see> VAT, which are neither <see cref="PX.Objects.TX.Tax.ExemptTax">exempt</see>, 
		/// nor <see cref="PX.Objects.TX.Tax.StatisticalTax">statistical</see>, nor <see cref="PX.Objects.TX.Tax.ReverseTax">reverse</see>.
		/// (Presented in the currency of the document, see <see cref="CuryID"/>)
		/// </summary>
		[PXDBCurrency(typeof(curyInfoID), typeof(vatTaxableTotal))]
		[PXUIField(DisplayName = "VAT Taxable Total", Visibility = PXUIVisibility.Visible, Enabled = false)]
		[PXDefault(TypeCode.Decimal, "0.0")]
		public virtual Decimal? CuryVatTaxableTotal
		{
			get;
			set;
		}
		#endregion
		#region VatTaxableTotal
		public abstract class vatTaxableTotal : PX.Data.IBqlField
		{
		}

		/// <summary>
		/// The part of the document total, which is subject to VAT.
		/// This total is calculated as a sum of the taxable amounts for the <see cref="PX.Objects.TX.Tax">taxes</see>
		/// of <see cref="PX.Objects.TX.Tax.TaxType">type</see> VAT, which are neither <see cref="PX.Objects.TX.Tax.ExemptTax">exempt</see>, 
		/// nor <see cref="PX.Objects.TX.Tax.StatisticalTax">statistical</see>, nor <see cref="PX.Objects.TX.Tax.ReverseTax">reverse</see>.
		/// (Presented in the base currency of the company, see <see cref="Company.BaseCuryID"/>)
		/// </summary>
		[PXDBBaseCury]
		[PXDefault(TypeCode.Decimal, "0.0")]
		public virtual Decimal? VatTaxableTotal
		{
			get;
			set;
		}
		#endregion

		#region NoteID
		public abstract class noteID : PX.Data.IBqlField
		{
		}
		/*[PXSearchable(SM.SearchCategory.PO, "{0}: {1} - {3}", new Type[] { typeof(POReceipt.pOType), typeof(POReceipt.receiptNbr), typeof(POReceipt.vendorID), typeof(Vendor.acctName) },
			new Type[] { typeof(POReceipt.invoiceNbr) },
			NumberFields = new Type[] { typeof(POReceipt.receiptNbr) },
			Line1Format = "{0:d}{1}{2}", Line1Fields = new Type[] { typeof(POReceipt.receiptDate), typeof(POReceipt.status), typeof(POReceipt.invoiceNbr) },
			Line2Format = "{0}", Line2Fields = new Type[] { typeof(POReceipt.orderQty), typeof(POReceipt.orderTotal) },
			MatchWithJoin = typeof(InnerJoin<Vendor, On<Vendor.bAccountID, Equal<POReceipt.vendorID>>>),
			SelectForFastIndexing = typeof(Select2<POReceipt, InnerJoin<Vendor, On<POReceipt.vendorID, Equal<Vendor.bAccountID>>>>)
		)]*/
		[PXNote(new Type[0], ShowInReferenceSelector = true)]
		public virtual Guid? NoteID
		{
			get;
			set;
		}
		#endregion

		#region CreatedByID
		public abstract class createdByID : PX.Data.IBqlField
		{
		}

		[PXDBCreatedByID()]
		public virtual Guid? CreatedByID
		{
			get;
			set;
		}
		#endregion
		#region CreatedByScreenID
		public abstract class createdByScreenID : PX.Data.IBqlField
		{
		}

		[PXDBCreatedByScreenID()]
		public virtual String CreatedByScreenID
		{
			get;
			set;
		}
		#endregion
		#region CreatedDateTime
		public abstract class createdDateTime : PX.Data.IBqlField
		{
		}

		[PXDBCreatedDateTime]
		[PXUIField(DisplayName = PXDBLastModifiedByIDAttribute.DisplayFieldNames.CreatedDateTime, Enabled = false, IsReadOnly = true)]
		public virtual DateTime? CreatedDateTime
		{
			get;
			set;
		}
		#endregion
		#region LastModifiedByID
		public abstract class lastModifiedByID : PX.Data.IBqlField
		{
		}

		[PXDBLastModifiedByID()]
		public virtual Guid? LastModifiedByID
		{
			get;
			set;
		}
		#endregion
		#region LastModifiedByScreenID
		public abstract class lastModifiedByScreenID : PX.Data.IBqlField
		{
		}

		[PXDBLastModifiedByScreenID()]
		public virtual String LastModifiedByScreenID
		{
			get;
			set;
		}
		#endregion
		#region LastModifiedDateTime
		public abstract class lastModifiedDateTime : PX.Data.IBqlField
		{
		}

		[PXDBLastModifiedDateTime]
		[PXUIField(DisplayName = PXDBLastModifiedByIDAttribute.DisplayFieldNames.LastModifiedDateTime, Enabled = false, IsReadOnly = true)]
		public virtual DateTime? LastModifiedDateTime
		{
			get;
			set;
		}
		#endregion
		#region tstamp
		public abstract class Tstamp : PX.Data.IBqlField
		{
		}

		[PXDBTimestamp()]
		public virtual Byte[] tstamp
		{
			get;
			set;
		}
		#endregion
	}
}