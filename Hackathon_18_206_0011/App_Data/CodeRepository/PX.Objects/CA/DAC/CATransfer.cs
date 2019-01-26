using System;
using PX.Data;
using PX.Data.EP;
using PX.Objects.AR;
using PX.Objects.CM;
using PX.Objects.CS;
using PX.Objects.GL;

namespace PX.Objects.CA
{
    /// <summary>
    /// Contains the main properties of funds transfers and their classes.
    /// Funds transfers are edited on the Funds Transfers (CA301000) form (which corresponds to the <see cref="CashTransferEntry"/> graph).
    /// </summary>
	[PXCacheName(Messages.CATransfer)]
	[Serializable]
	public partial class CATransfer : PX.Data.IBqlTable, ICADocument
	{
        /// <summary>
        /// The type of the document. 
        /// The field implements the member of the <see cref="ICADocument"/> interface.
        /// </summary>
        /// <value>
        /// Always return <see cref="CATranType.CATransfer"/>
        /// </value>
        public string DocType
		{
			get
			{
				return CATranType.CATransfer;
			}
		}

        /// <summary>
        /// The unique identifier of the transfer.
        /// The field implements the member of the <see cref="ICADocument"/> interface.
        /// </summary>
        public string RefNbr
		{
			get
			{
				return TransferNbr;
			}
		}
		#region TransferNbr
		public abstract class transferNbr : IBqlField
		{
		}

        /// <summary>
        /// The user-friendly unique identifier of the transfer.
        /// This field is the auto-numbering key field.
        /// </summary>
        [PXDBString(15, IsUnicode = true, IsKey = true, InputMask = ">CCCCCCCCCCCCCCC")]
		[PXDefault]
		[PXUIField(DisplayName = "Transfer Number", Visibility = PXUIVisibility.SelectorVisible)]
		[PXSelector(typeof(CATransfer.transferNbr))]
		[AutoNumber(typeof(CASetup.transferNumberingID), typeof(CATransfer.inDate))]
		public virtual string TransferNbr
		{
			get;
			set;
		}
		#endregion
		#region Descr
		public abstract class descr : IBqlField
		{
		}

        /// <summary>
        /// A detailed description for the transfer transaction. An alphanumeric string of up to 60 characters may be used.
        /// </summary>
        [PXDBString(Common.Constants.TranDescLength, IsUnicode = true)]
		[PXUIField(DisplayName = "Description", Visibility = PXUIVisibility.SelectorVisible)]
		[PXFieldDescription]
		public virtual string Descr
		{
			get;
			set;
		}
		#endregion
		#region OutAccountID
		public abstract class outAccountID : IBqlField
		{
		}

        /// <summary>
        /// The identifier of the source <see cref="CashAccount">cash account</see> from which the funds are transferred.
        /// </summary>
        /// <value>
        /// Corresponds to the value of the <see cref="CashAccount.CashAccountID"/> field.
        /// </value>
        [PXDefault]
		[CashAccount(DisplayName = "Source Account", DescriptionField = typeof(CashAccount.descr))]
		public virtual int? OutAccountID
		{
			get;
			set;
		}
		#endregion
		#region InAccountID
		public abstract class inAccountID : IBqlField
		{
		}

        /// <summary>
        /// The identifier of the destination <see cref="CashAccount">cash account</see> to which the funds are transferred.
        /// </summary>
        /// <value>
        /// Corresponds to the value of the <see cref="CashAccount.CashAccountID"/> field.
        /// </value>
        [PXDefault]
		[CashAccount(DisplayName = "Destination Account", DescriptionField = typeof(CashAccount.descr))]
		public virtual int? InAccountID
		{
			get;
			set;
		}
		#endregion
		#region OutCuryInfoID
		public abstract class outCuryInfoID : IBqlField
		{
		}

        /// <summary>
        /// The identifier of the exchange rate record for the outcoming amount.
        /// </summary>
        /// <value>
        /// Corresponds to the <see cref="CurrencyInfo.CuryInfoID"/> field.
        /// </value>
		[PXDBLong]
		[CurrencyInfo(ModuleCode = BatchModule.CA, CuryIDField = nameof(OutCuryID), CuryRateField = "OutCuryRate")]
		public virtual long? OutCuryInfoID
		{
			get;
			set;
		}
		#endregion
		#region InCuryInfoID
		public abstract class inCuryInfoID : IBqlField
		{
		}

        /// <summary>
        /// The identifier of the exchange rate record for the incoming amount.
        /// </summary>
        /// <value>
        /// Corresponds to the <see cref="CurrencyInfo.CuryInfoID"/> field.
        /// </value>
		[PXDBLong]
		[CurrencyInfo(ModuleCode = BatchModule.CA, CuryIDField = nameof(InCuryID), CuryRateField = "InCuryRate")]
		public virtual long? InCuryInfoID
		{
			get;
			set;
		}
		#endregion
		#region InCuryID
		public abstract class inCuryID : IBqlField
		{
		}

        /// <summary>
        /// The currency of denomination for the destination cash account.
        /// </summary>
		[PXDBString(5, IsUnicode = true, InputMask = ">LLLLL")]
		[PXUIField(DisplayName = "Destination Currency", Enabled = false)]
		[PXDefault(typeof(Coalesce<Search<CashAccount.curyID, Where<CashAccount.cashAccountID, Equal<Current<CATransfer.inAccountID>>>>, Search<Company.baseCuryID>>))]
		[PXSelector(typeof(Currency.curyID))]
		public virtual string InCuryID
		{
			get;
			set;
		}
		#endregion
		#region OutCuryID
		public abstract class outCuryID : IBqlField
		{
		}

        /// <summary>
        /// The currency of denomination for the source cash account.
        /// </summary>
		[PXDBString(5, IsUnicode = true, InputMask = ">LLLLL")]
		[PXUIField(DisplayName = "Source Currency", Enabled = false)]
		[PXDefault(typeof(Coalesce<Search<CashAccount.curyID, Where<CashAccount.cashAccountID, Equal<Current<CATransfer.outAccountID>>>>, Search<Company.baseCuryID>>))]
		[PXSelector(typeof(Currency.curyID))]
		public virtual string OutCuryID
		{
			get;
			set;
		}
		#endregion
		#region CuryTranOut
		public abstract class curyTranOut : IBqlField
		{
		}

        /// <summary>
        /// The amount of the transfer outcomes from the source cash account (in the specified currency).
        /// </summary>
		[PXDefault(TypeCode.Decimal, "0.0")]
		[PXUIField(DisplayName = "Source Amount")]
		[PXDBCurrency(typeof(CATransfer.outCuryInfoID), typeof(CATransfer.tranOut))]
		public virtual decimal? CuryTranOut
		{
			get;
			set;
		}
		#endregion
		#region CuryTranIn
		public abstract class curyTranIn : IBqlField
		{
		}

        /// <summary>
        /// The amount of the transfer incomes to the destination cash account (in the specified currency).
        /// </summary>
		[PXDefault(TypeCode.Decimal, "0.0")]
		[PXUIField(DisplayName = "Destination Amount")]
		[PXDBCurrency(typeof(CATransfer.inCuryInfoID), typeof(CATransfer.tranIn))]
		public virtual decimal? CuryTranIn
		{
			get;
			set;
		}
		#endregion
		#region TranOut
		public abstract class tranOut : IBqlField
		{
		}

        /// <summary>
        /// The amount of the transfer outcomes from the source cash account (in the base currency).
        /// </summary>
		[PXDBBaseCury]
		[PXDefault(TypeCode.Decimal, "0.0")]
		[PXUIField(DisplayName = "Base Currency Amount", Enabled = false)]
		public virtual decimal? TranOut
		{
			get;
			set;
		}
		#endregion
		#region TranIn
		public abstract class tranIn : IBqlField
		{
		}

        /// <summary>
        /// The amount of the transfer incomes to the destination cash account (in the base currency).
        /// </summary>
		[PXDBBaseCury]
		[PXDefault(TypeCode.Decimal, "0.0")]
		[PXUIField(DisplayName = "Base Currency Amount", Enabled = false)]
		public virtual decimal? TranIn
		{
			get;
			set;
		}
		#endregion
		#region InDate
		public abstract class inDate : IBqlField
		{
		}

        /// <summary>
        /// The date of the transfer receipt.
        /// </summary>
		[PXDBDate]
		[PXDefault(typeof(AccessInfo.businessDate))]
		[PXUIField(DisplayName = "Receipt Date", Visibility = PXUIVisibility.SelectorVisible)]
		public virtual DateTime? InDate
		{
			get;
			set;
		}
		#endregion
		#region OutDate
		public abstract class outDate : IBqlField
		{
		}

        /// <summary>
        /// The date of the transaction (when funds were withdrawn from the source cash account).
        /// </summary>
		[PXDBDate]
		[PXDefault(typeof(AccessInfo.businessDate))]
		[PXUIField(DisplayName = "Transfer Date")]
		public virtual DateTime? OutDate
		{
			get;
			set;
		}
		#endregion

		#region InBranchID
		public abstract class inBranchID : IBqlField
		{
		}
		[PXFormula(typeof(Default<inAccountID>))]
		[PXDefault(typeof(Search<CashAccount.branchID, Where<CashAccount.cashAccountID, Equal<Current<inAccountID>>>>))]
		[PXDBInt]
		public virtual int? InBranchID
		{
			get;
			set;
		}
		#endregion

		#region OutBranchID
		public abstract class outBranchID : IBqlField
		{
		}
		[PXFormula(typeof(Default<outAccountID>))]
		[PXDefault(typeof(Search<CashAccount.branchID, Where<CashAccount.cashAccountID, Equal<Current<outAccountID>>>>))]
		[PXDBInt]
		public virtual int? OutBranchID
		{
			get;
			set;
		}
		#endregion
		#region InPeriodID
		public abstract class inPeriodID : IBqlField
		{
		}

		[CAOpenPeriod(typeof(inDate), typeof(inAccountID), typeof(Selector<inAccountID, CashAccount.branchID>))]
		[PXUIField(DisplayName = "In Period", Visible = false,
			ErrorHandling = PXErrorHandling.Always, MapErrorTo = typeof(inDate))]
		public virtual string InPeriodID
		{
			get;
			set;
		}
		#endregion		
		#region OutFinPeriodID
		public abstract class outPeriodID : IBqlField
		{
		}

		[CAOpenPeriod(typeof(outDate), typeof(outAccountID), typeof(Selector<outAccountID, CashAccount.branchID>))]
		[PXUIField(DisplayName = "Out Period", Visible = false,
			ErrorHandling = PXErrorHandling.Always, MapErrorTo = typeof(outDate))]
		public virtual string OutPeriodID
		{
			get;
			set;
		}
		#endregion
		#region OutExtRefNbr
		public abstract class outExtRefNbr : IBqlField
		{
		}

        /// <summary>
        /// The reference number of the transfer for the source cash account.
        /// This is a number provided by an external bank or organization.
        /// This field is entered manually.
        /// </summary>
		[PXDBString(40, IsUnicode = true)]
		[PXDefault]
		[PXUIField(DisplayName = "Document Ref.")]
		public virtual string OutExtRefNbr
		{
			get;
			set;
		}
		#endregion
		#region InExtRefNbr
		public abstract class inExtRefNbr : IBqlField
		{
		}

        /// <summary>
        /// The reference number of the transfer for the target cash account.
        /// This is a number provided by an external bank or organization.
        /// The value of the field is entered by a user.
        /// </summary>
        [PXDBString(40, IsUnicode = true)]
		[PXDefault]
		[PXUIField(DisplayName = "Document Ref.")]
		public virtual string InExtRefNbr
		{
			get;
			set;
		}
		#endregion
		#region TranIDOut
		public abstract class tranIDOut : IBqlField
		{
		}

        /// <summary>
        /// The unique identifier of the outcoming CA transaction.
        /// </summary>
        /// /// <value>
        /// Corresponds to the value of the <see cref="CATran.TranID"/> field.
        /// </value>
        [PXDBLong]
		[TransferCashTranID]
		[PXSelector(typeof(Search<CATran.tranID>), DescriptionField = typeof(CATran.batchNbr))]
		public virtual long? TranIDOut
		{
			get;
			set;
		}
		#endregion
		#region TranIDIn
		public abstract class tranIDIn : IBqlField
		{
		}

        /// <summary>
        /// The unique identifier of the incoming CA transaction.
        /// </summary>
        /// /// <value>
        /// Corresponds to the value of the <see cref="CATran.TranID"/> field.
        /// </value>
		[PXDBLong]
		[TransferCashTranID]
		[PXSelector(typeof(Search<CATran.tranID>), DescriptionField = typeof(CATran.batchNbr))]
		public virtual long? TranIDIn
		{
			get;
			set;
		}
		#endregion
		#region ExpenseCntr
		public abstract class expenseCntr : IBqlField { }

		[PXDBInt]
		[PXDefault(0)]
		public virtual int? ExpenseCntr { get; set; }
		#endregion
		#region RGOLAmt
		public abstract class rGOLAmt : IBqlField
		{
		}

        /// <summary>
        /// A read-only box that displays the difference between the amount in the base currency specified for the source account 
        /// and the amount in the base currency resulting for the destination cash account, 
        /// for cases when the source and destination currencies are different.
        /// </summary>
		[PXDBBaseCury]
		[PXDefault(TypeCode.Decimal, "0.0")]
		[PXUIField(DisplayName = "RGOL", Enabled = false)]
		public virtual decimal? RGOLAmt
		{
			get;
			set;
		}
		#endregion
		#region Hold
		public abstract class hold : IBqlField
		{
		}
        protected bool? _Hold;

        /// <summary>
        /// Indicates (if set to <c>true</c>) that the transfer is on hold. The value of the field can be set to <c>false</c> only for balanced transfers.
        /// </summary>
		[PXDBBool]
		[PXDefault(typeof(Search<CASetup.holdEntry>))]
		[PXUIField(DisplayName = "Hold")]
		public virtual bool? Hold
		{
            get
            {
                return this._Hold;
            }

            set
            {
                this._Hold = value;
                this.SetStatus();
            }
        }
		#endregion
		#region Released
		public abstract class released : IBqlField
		{
		}
        protected bool? _Released;

        /// <summary>
        /// Specifies (if set to <c>true</c>) that the transfer is released.
        /// </summary>
		[PXDBBool]
		[PXDefault(false)]
		public virtual bool? Released
		{
			get
			{
				return _Released;
			}

			set
			{
				this._Released = value;
				this.SetStatus();
			}
		}
		#endregion
		#region NoteID
		public abstract class noteID : IBqlField
		{
		}

        /// <summary>
        /// A global unique identifier of the transfer in Acumatica ERP.
        /// The field is used for attachments for the transfer (such as notes, files).
        /// </summary>
        /// <value>
        /// Corresponds to the value of the <see cref="Note.NoteID"/> field.
        /// </value>
        [PXSearchable(SM.SearchCategory.CA, Messages.SearchTitleCATransfer, new Type[] { typeof(CATransfer.transferNbr) },
			new Type[] { typeof(CATransfer.descr), typeof(CATransfer.outExtRefNbr), typeof(CATransfer.inExtRefNbr) },
			NumberFields = new Type[] { typeof(CATransfer.transferNbr) },
			Line1Format = "{0}{1}", Line1Fields = new Type[] { typeof(CATransfer.outExtRefNbr), typeof(CATransfer.inExtRefNbr) },
			Line2Format = "{0}", Line2Fields = new Type[] { typeof(CATransfer.descr) }
		)]
		[PXNote(DescriptionField = typeof(CATransfer.transferNbr))]
		public virtual Guid? NoteID
		{
			get;
			set;
		}
		#endregion
		#region CreatedByID
		public abstract class createdByID : IBqlField
		{
		}
		[PXDBCreatedByID]
		public virtual Guid? CreatedByID
		{
			get;
			set;
		}
		#endregion
		#region CreatedByScreenID
		public abstract class createdByScreenID : IBqlField
		{
		}

		[PXDBCreatedByScreenID]
		public virtual string CreatedByScreenID
		{
			get;
			set;
		}
		#endregion
		#region CreatedDateTime
		public abstract class createdDateTime : IBqlField
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
		public abstract class lastModifiedByID : IBqlField
		{
		}

		[PXDBLastModifiedByID]
		public virtual Guid? LastModifiedByID
		{
			get;
			set;
		}
		#endregion
		#region LastModifiedByScreenID
		public abstract class lastModifiedByScreenID : IBqlField
		{
		}

		[PXDBLastModifiedByScreenID]
		public virtual string LastModifiedByScreenID
		{
			get;
			set;
		}
		#endregion
		#region LastModifiedDateTime
		public abstract class lastModifiedDateTime : IBqlField
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
		public abstract class Tstamp : IBqlField
		{
		}

		[PXDBTimestamp]
		public virtual byte[] tstamp
		{
			get;
			set;
		}
        #endregion
        #region Status
        /// <summary>
        /// The status of the transfer.
        /// </summary>
        /// <value>
        /// The field can have one of the values described in <see cref="CATransferStatus.ListAttribute"/>.
        /// </value>
        [PXString(1, IsFixed = true)]
		[PXUIField(DisplayName = "Status", Visibility = PXUIVisibility.SelectorVisible, Enabled = false)]
		[CATransferStatus.List]
		public virtual string Status
		{
			[PXDependsOnFields(typeof(released), typeof(hold))]
			get;
			set;
		}
		#endregion
		#region ClearedOut
		public abstract class clearedOut : IBqlField
		{
		}

        /// <summary>
        /// Indicates (if set to <c>true</c>) that this outcoming transaction has been cleared.
        /// </summary>
		[PXDBBool]
		[PXUIField(DisplayName = "Cleared")]
		[PXDefault(false)]
		public virtual bool? ClearedOut
		{
			get;
			set;
		}
		#endregion
		#region ClearDateOut
		public abstract class clearDateOut : IBqlField
		{
		}

        /// <summary>
        /// The date when the outcoming transaction was cleared in the process of reconciliation.
        /// </summary>
		[PXDBDate]
		[PXUIField(DisplayName = "Clear Date", Required = false)]
		public virtual DateTime? ClearDateOut
		{
			get;
			set;
		}
		#endregion
		#region ClearedIn
		public abstract class clearedIn : IBqlField
		{
		}

        /// <summary>
        /// Indicates (if set to <c>true</c>) that this incoming transaction has been cleared.
        /// </summary>
		[PXDBBool]
		[PXUIField(DisplayName = "Cleared")]
		[PXDefault(false)]
		public virtual bool? ClearedIn
		{
			get;
			set;
		}
		#endregion
		#region ClearDateIn
		public abstract class clearDateIn : IBqlField
		{
		}

        /// <summary>
        /// The date when the incoming transaction was cleared in the process of reconciliation.
        /// </summary>
		[PXDBDate]
		[PXUIField(DisplayName = "Clear Date", Required = false)]
		public virtual DateTime? ClearDateIn
		{
			get;
			set;
		}
		#endregion
		#region CashBalanceIn
		public abstract class cashBalanceIn : IBqlField
		{
		}

        /// <summary>
        /// The actual balance of the target cash account.
        /// </summary>
		[PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
		[PXCury(typeof(CATransfer.inCuryID))]
		[PXUIField(DisplayName = "Available Balance", Enabled = false)]
		[CashBalance(typeof(CATransfer.inAccountID))]
		public virtual decimal? CashBalanceIn
		{
			get;
			set;
		}
		#endregion
		#region CashBalanceOut
		public abstract class cashBalanceOut : IBqlField
		{
		}

        /// <summary>
        /// The actual balance of the source cash account.
        /// </summary>
		[PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
		[PXCury(typeof(CATransfer.outCuryID))]
		[PXUIField(DisplayName = "Available Balance", Enabled = false)]
		[CashBalance(typeof(CATransfer.outAccountID))]
		public virtual decimal? CashBalanceOut
		{
			get;
			set;
		}
		#endregion
		#region InGLBalance
		public abstract class inGLBalance : IBqlField
		{
		}

        /// <summary>
        /// The balance of the target account, as recorded in the General Ledger.
        /// </summary>
		[PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
		[PXCury(typeof(inCuryID))]
		[PXUIField(DisplayName = "GL Balance", Enabled = false)]
		[GLBalance(typeof(inAccountID), null, typeof(inDate))]
		public virtual decimal? InGLBalance
		{
			get;
			set;
		}
		#endregion
		#region OutGLBalance
		public abstract class outGLBalance : IBqlField
		{
		}

        /// <summary>
        /// A read-only box displaying the balance of the source account recorded in the General Ledger
        /// for the financial period that includes the transfer date.
        /// </summary>
		[PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
		[PXCury(typeof(outCuryID))]
		[PXUIField(DisplayName = "GL Balance", Enabled = false)]
		[GLBalance(typeof(outAccountID), null, typeof(outDate))]
		public virtual decimal? OutGLBalance
		{
			get;
			set;
        }
		#endregion
		#region TranIDOut_CATran_batchNbr
		/// <summary>
		/// The batch number for the transfer. Only released transfers have batch numbers.
		/// The field is used as a link to the batch that contains the transaction for the source account in the General Ledger.
		/// This is a virtual field, which is filled in from the <see cref = "CATran.batchNbr" /> field (<see cref = "CashTransferEntry.CATransfer_TranIDOut_CATran_BatchNbr_FieldSelecting" />).
		/// </summary>
		public abstract class tranIDOut_CATran_batchNbr : IBqlField
		{
		}
		#endregion
		#region TranIDIn_CATran_batchNbr
		/// <summary>
		/// The number of the batch that contains the transaction for the target account in the General Ledger.
		/// Only released transfers have batch numbers.
		/// This is a virtual field, which is filled in from the <see cref = "CATran.batchNbr" /> field (<see cref="CashTransferEntry.CATransfer_TranIDIn_CATran_BatchNbr_FieldSelecting" />).
		/// </summary>
		public abstract class tranIDIn_CATran_batchNbr : IBqlField
		{
		}
        #endregion
        #region Methods
        /// <summary>
        /// Sets the status of the transfer based on the <see cref="Hold"/> and <see cref="Released"/> flags.
        /// </summary>
        public virtual void SetStatus()
		{
			if (Released == true)
			{
				Status = CATransferStatus.Released;
			}
			else if (Hold == true)
			{
				Status = CATransferStatus.Hold;
			}
			else
			{
				Status = CATransferStatus.Balanced;
			}
		}
		#endregion
	}

    [Obsolete("Will be removed in Acumatica ERP 2018R2")]
    public class caCredit : DrCr.credit
    { }

    /// <summary>
    /// Contains statuses of the transfer.
    /// </summary>
	public class CATransferStatus
	{
        /// <summary>
        /// The transfer is balanced and can be released.
        /// </summary>
		public const string Balanced = "B";

        /// <summary>
        /// The transfer is a draft only; the actual transfer of funds has not been initiated.
        /// </summary>
        public const string Hold = "H";

        /// <summary>
        /// The transfer has been released.
        /// </summary>
		public const string Released = "R";

        /// <summary>
        /// The transfer has been rejected.
        /// </summary>
        public const string Rejected = "J";

        /// <summary>
        /// The transfer is pending approval.
        /// </summary>
        public const string Pending = "P";

		public static readonly string[] Values =
		{
			Balanced,
			Hold,
			Released,
			Pending,
			Rejected
		};

		public static readonly string[] Labels =
		{
			Messages.Balanced,
			Messages.Hold,
			Messages.Released,
			Messages.Pending,
			Messages.Rejected
		};

		public class ListAttribute : PXStringListAttribute
		{
			public ListAttribute() : base(Values, Labels) { }
		}

		public class balanced : Constant<string>
		{
			public balanced() : base(Balanced) { }
		}

		public class hold : Constant<string>
		{
			public hold() : base(Hold) { }
		}

		public class released : Constant<string>
		{
			public released() : base(Released) { }
		}

		public class rejected : Constant<string>
		{
			public rejected() : base(Rejected) { }
		}

		public class pending : Constant<string>
		{
			public pending() : base(Pending) { }
		}
	}
}
