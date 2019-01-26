using System;
using System.Collections.Generic;
using System.Text;
using PX.Data;
using PX.Objects.TX;
using PX.Objects.CM;
using PX.Objects.CS;

namespace PX.Objects.SO
{
    [PXCacheName(Messages.SOTaxTran)]
    [System.SerializableAttribute()]
    public partial class SOTaxTran : TaxDetail, PX.Data.IBqlTable
    {
        #region OrderType
        public abstract class orderType : PX.Data.IBqlField
        {
        }
        protected String _OrderType;
        [PXDBString(2, IsFixed = true, IsKey = true)]
        [PXDBDefault(typeof(SOOrder.orderType))]
        [PXUIField(DisplayName = "Order Type", Enabled = false, Visible = false)]
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
        [PXDBString(15, IsUnicode = true, InputMask = "", IsKey = true)]
        [PXDBDefault(typeof(SOOrder.orderNbr))]
        [PXUIField(DisplayName = "Order Nbr.", Enabled = false, Visible = false)]
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
        [PXDBInt(IsKey = true)]
        [PXDefault(int.MaxValue)]
        [PXUIField(DisplayName = "Line Nbr.", Visibility = PXUIVisibility.Visible, Visible = false)]
		[PXParent(typeof(Select<SOOrder, Where<SOOrder.orderType, Equal<Current<SOTaxTran.orderType>>, And<SOOrder.orderNbr, Equal<Current<SOTaxTran.orderNbr>>>>>))]
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
        #region TaxID
        public abstract class taxID : PX.Data.IBqlField
        {
        }
        [PXDBString(Tax.taxID.Length, IsUnicode = true, IsKey = true)]
        [PXDefault()]
        [PXUIField(DisplayName = "Tax ID", Visibility = PXUIVisibility.Visible)]
		[PXSelector(typeof(Tax.taxID), DescriptionField = typeof(Tax.descr), DirtyRead = true)]
        public override String TaxID
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
		#region RecordID
		public abstract class recordID : PX.Data.IBqlField
		{
		}
		protected Int32? _RecordID;
		[PXDBIdentity(IsKey = true)]
		public virtual Int32? RecordID
		{
			get
			{
				return this._RecordID;
			}
			set
			{
				this._RecordID = value;
			}
		}
		#endregion
		#region JurisType
		public abstract class jurisType : PX.Data.IBqlField
        {
        }
        protected String _JurisType;
        [PXDBString(9, IsUnicode = true)]
        [PXUIField(DisplayName = "Tax Jurisdiction Type")]
        public virtual String JurisType
        {
            get
            {
                return this._JurisType;
            }
            set
            {
                this._JurisType = value;
            }
        }
        #endregion
        #region JurisName
        public abstract class jurisName : PX.Data.IBqlField
        {
        }
        protected String _JurisName;
        [PXDBString(200, IsUnicode = true)]
        [PXUIField(DisplayName = "Tax Jurisdiction Name")]
        public virtual String JurisName
        {
            get
            {
                return this._JurisName;
            }
            set
            {
                this._JurisName = value;
            }
        }
        #endregion
        #region TaxRate
        public abstract class taxRate : PX.Data.IBqlField
        {
        }
        #endregion
        #region CuryInfoID
        public abstract class curyInfoID : PX.Data.IBqlField
        {
        }
        [PXDBLong()]
        [CurrencyInfo(typeof(SOOrder.curyInfoID))]
        public override Int64? CuryInfoID
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
        #region CuryTaxableAmt
        public abstract class curyTaxableAmt : PX.Data.IBqlField
        {
        }
		protected decimal? _CuryTaxableAmt;
        [PXDBCurrency(typeof(SOTaxTran.curyInfoID), typeof(SOTaxTran.taxableAmt))]
        [PXDefault(TypeCode.Decimal, "0.0")]
        [PXUIField(DisplayName = "Taxable Amount", Visibility = PXUIVisibility.Visible)]
        [PXUnboundFormula(typeof(Switch<Case<Where<WhereExempt<SOTaxTran.taxID>>, SOTaxTran.curyTaxableAmt>, decimal0>), typeof(SumCalc<SOOrder.curyVatExemptTotal>))]
        [PXUnboundFormula(typeof(Switch<Case<Where<WhereTaxable<SOTaxTran.taxID>>, SOTaxTran.curyTaxableAmt>, decimal0>), typeof(SumCalc<SOOrder.curyVatTaxableTotal>))]
        public virtual Decimal? CuryTaxableAmt
        {
            get
            {
                return this._CuryTaxableAmt;
            }
            set
            {
                this._CuryTaxableAmt = value;
            }
        }
		#endregion
		#region CuryExemptedAmt
		public abstract class curyExemptedAmt : IBqlField { }

		/// <summary>
		/// The exempted amount in the record currency.
		/// </summary>
		[PXDBCurrency(typeof(SOTaxTran.curyInfoID), typeof(SOTaxTran.exemptedAmt))]
		[PXDefault(TypeCode.Decimal, "0.0")]
		[PXUIField(DisplayName = "Exempted Amount", Visibility = PXUIVisibility.Visible, FieldClass = nameof(FeaturesSet.ExemptedTaxReporting))]
		public decimal? CuryExemptedAmt
		{
			get;
			set;
		}
		#endregion
		#region CuryUnshippedTaxableAmt
		public abstract class curyUnshippedTaxableAmt : PX.Data.IBqlField
		{
		}
		protected Decimal? _CuryUnshippedTaxableAmt;
		[PXDBCurrency(typeof(SOTaxTran.curyInfoID), typeof(SOTaxTran.unshippedTaxableAmt))]
		[PXDefault(TypeCode.Decimal, "0.0")]
		[PXUIField(DisplayName = "Unshipped Taxable Amount", Visibility = PXUIVisibility.Visible)]
		public virtual Decimal? CuryUnshippedTaxableAmt
		{
			get
			{
				return this._CuryUnshippedTaxableAmt;
			}
			set
			{
				this._CuryUnshippedTaxableAmt = value;
			}
		}
		#endregion
		#region CuryUnbilledTaxableAmt
		public abstract class curyUnbilledTaxableAmt : PX.Data.IBqlField
		{
		}
		protected Decimal? _CuryUnbilledTaxableAmt;
		[PXDBCurrency(typeof(SOTaxTran.curyInfoID), typeof(SOTaxTran.unbilledTaxableAmt))]
		[PXDefault(TypeCode.Decimal, "0.0")]
		[PXUIField(DisplayName = "Unbilled Taxable Amount", Visibility = PXUIVisibility.Visible)]
		public virtual Decimal? CuryUnbilledTaxableAmt
		{
			get
			{
				return this._CuryUnbilledTaxableAmt;
			}
			set
			{
				this._CuryUnbilledTaxableAmt = value;
			}
		}
		#endregion
        #region TaxableAmt
        public abstract class taxableAmt : PX.Data.IBqlField
        {
        }
		protected Decimal? _TaxableAmt;
		[PXDBDecimal(4)]
		[PXDefault(TypeCode.Decimal, "0.0")]
		[PXUIField(DisplayName = "Taxable Amount", Visibility = PXUIVisibility.Visible)]
		public virtual Decimal? TaxableAmt
		{
			get
			{
				return this._TaxableAmt;
			}
			set
			{
				this._TaxableAmt = value;
			}
		}
		#endregion
		#region ExemptedAmt
		public abstract class exemptedAmt : IBqlField { }

		/// <summary>
		/// The exempted amount in the base currency.
		/// </summary>
		[PXDBDecimal(4)]
		[PXDefault(TypeCode.Decimal, "0.0")]
		[PXUIField(DisplayName = "Exempted Amount", Visibility = PXUIVisibility.Visible, FieldClass = nameof(FeaturesSet.ExemptedTaxReporting))]
		public decimal? ExemptedAmt
		{
			get;
			set;
		}
		#endregion
		#region UnshippedTaxableAmt
		public abstract class unshippedTaxableAmt : PX.Data.IBqlField
		{
		}
		protected Decimal? _UnshippedTaxableAmt;
		[PXDBDecimal(4)]
		[PXDefault(TypeCode.Decimal, "0.0")]
		public virtual Decimal? UnshippedTaxableAmt
		{
			get
			{
				return this._UnshippedTaxableAmt;
			}
			set
			{
				this._UnshippedTaxableAmt = value;
			}
		}
		#endregion
		#region UnbilledTaxableAmt
		public abstract class unbilledTaxableAmt : PX.Data.IBqlField
		{
		}
		protected Decimal? _UnbilledTaxableAmt;
		[PXDBDecimal(4)]
		[PXDefault(TypeCode.Decimal, "0.0")]
		public virtual Decimal? UnbilledTaxableAmt
		{
			get
			{
				return this._UnbilledTaxableAmt;
			}
			set
			{
				this._UnbilledTaxableAmt = value;
			}
		}
		#endregion
        #region CuryTaxAmt
        public abstract class curyTaxAmt : PX.Data.IBqlField
        {
        }
		protected decimal? _CuryTaxAmt;
        [PXDBCurrency(typeof(SOTaxTran.curyInfoID), typeof(SOTaxTran.taxAmt))]
        [PXDefault(TypeCode.Decimal, "0.0")]
        [PXUIField(DisplayName = "Tax Amount", Visibility = PXUIVisibility.Visible)]
        public virtual Decimal? CuryTaxAmt
        {
            get
            {
                return this._CuryTaxAmt;
            }
            set
            {
                this._CuryTaxAmt = value;
            }
        }
        #endregion
		#region CuryUnshippedTaxAmt
		public abstract class curyUnshippedTaxAmt : PX.Data.IBqlField
		{
		}
		protected Decimal? _CuryUnshippedTaxAmt;
		[PXDBCurrency(typeof(SOTaxTran.curyInfoID), typeof(SOTaxTran.unshippedTaxAmt))]
		[PXDefault(TypeCode.Decimal, "0.0")]
		[PXUIField(DisplayName = "Unshipped Tax Amount", Visibility = PXUIVisibility.Visible)]
		public virtual Decimal? CuryUnshippedTaxAmt
		{
			get
			{
				return this._CuryUnshippedTaxAmt;
			}
			set
			{
				this._CuryUnshippedTaxAmt = value;
			}
		}
		#endregion
		#region CuryUnbilledTaxAmt
		public abstract class curyUnbilledTaxAmt : PX.Data.IBqlField
		{
		}
		protected Decimal? _CuryUnbilledTaxAmt;
		[PXDBCurrency(typeof(SOTaxTran.curyInfoID), typeof(SOTaxTran.unbilledTaxAmt))]
		[PXDefault(TypeCode.Decimal, "0.0")]
		[PXUIField(DisplayName = "Unbilled Tax Amount", Visibility = PXUIVisibility.Visible)]
		public virtual Decimal? CuryUnbilledTaxAmt
		{
			get
			{
				return this._CuryUnbilledTaxAmt;
			}
			set
			{
				this._CuryUnbilledTaxAmt = value;
			}
		}
		#endregion
        #region TaxAmt
        public abstract class taxAmt : PX.Data.IBqlField
        {
        }
		protected Decimal? _TaxAmt;
		[PXDBDecimal(4)]
		[PXDefault(TypeCode.Decimal, "0.0")]
		[PXUIField(DisplayName = "Tax Amount", Visibility = PXUIVisibility.Visible)]
		public virtual Decimal? TaxAmt
		{
			get
			{
				return this._TaxAmt;
			}
			set
			{
				this._TaxAmt = value;
			}
		}
		#endregion
		#region CuryExpenseAmt
		public abstract class curyExpenseAmt : PX.Data.IBqlField
		{
		}
		[PXDBCurrency(typeof(SOTaxTran.curyInfoID), typeof(SOTaxTran.expenseAmt))]
		[PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
		[PXUIField(DisplayName = "Expense Amount", Visibility = PXUIVisibility.Visible)]
		public override Decimal? CuryExpenseAmt
		{
			get; set;
		}
		#endregion
		#region ExpenseAmt
		public abstract class expenseAmt : PX.Data.IBqlField
		{
		}
		#endregion
		#region UnshippedTaxAmt
		public abstract class unshippedTaxAmt : PX.Data.IBqlField
		{
		}
		protected Decimal? _UnshippedTaxAmt;
		[PXDBDecimal(4)]
		[PXDefault(TypeCode.Decimal, "0.0")]
		public virtual Decimal? UnshippedTaxAmt
		{
			get
			{
				return this._UnshippedTaxAmt;
			}
			set
			{
				this._UnshippedTaxAmt = value;
			}
		}
		#endregion
		#region UnbilledTaxAmt
		public abstract class unbilledTaxAmt : PX.Data.IBqlField
		{
		}
		protected Decimal? _UnbilledTaxAmt;
		[PXDBDecimal(4)]
		[PXDefault(TypeCode.Decimal, "0.0")]
		public virtual Decimal? UnbilledTaxAmt
		{
			get
			{
				return this._UnbilledTaxAmt;
			}
			set
			{
				this._UnbilledTaxAmt = value;
			}
		}
		#endregion
		#region TaxZoneID
		public abstract class taxZoneID : IBqlField { }
        [PXDBString(10, IsUnicode = true)]
        public virtual string TaxZoneID
        {
            get;
            set;
        }
        #endregion
    }
}
