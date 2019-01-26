namespace PX.Objects.TX
{
	using System;
	using PX.Data;

	/// <summary>
	/// The detail of <see cref="TaxCategory"/>. Implements the many-to-many relationship between <see cref="Tax"/> and <see cref="TaxCategory"/>.
	/// </summary>
	[System.SerializableAttribute()]
	[PXCacheName(Messages.TaxCategoryDet)]
	public partial class TaxCategoryDet : PX.Data.IBqlTable, ITaxDetail
	{
		#region TaxCategoryID
		public abstract class taxCategoryID : PX.Data.IBqlField
		{
		}
		protected String _TaxCategoryID;

		/// <summary>
		/// The foreign key to <see cref="TaxCategory"/>.
		/// </summary>
		[PXDBString(10, IsUnicode = true, IsKey = true, InputMask = ">aaaaaaaaaa")]
		[PXDefault(typeof(TaxCategory.taxCategoryID))]
		[PXUIField(DisplayName = "Tax Category", Visible =false)]
		[PXParent(typeof(Select<TaxCategory,Where<TaxCategory.taxCategoryID,Equal<Current<TaxCategoryDet.taxCategoryID>>>>))]
		[PXSelector(typeof(TaxCategory.taxCategoryID))]
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

		/// <summary>
		/// The foreign key to <see cref="Tax"/>.
		/// </summary>
		[PXDBString(Tax.taxID.Length, IsUnicode = true, IsKey = true)]
		[PXDefault()]
		[PXUIField(DisplayName = "Tax ID")]
		[PXSelector(typeof(Search<Tax.taxID, Where<Tax.isExternal, Equal<False>>>), DescriptionField = typeof(Tax.descr))]
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
		#region IsImported
		public abstract class isImported : PX.Data.IBqlField
		{
		}
		protected Boolean? _IsImported;

		/// <summary>
		/// The field was used on importing of tax configuration from Avalara files.
		/// </summary>
		[Obsolete("This property is obsolete and will be removed in Acumatica 8.0")]
		[PXDBBool()]
		[PXDefault(false)]
		public virtual Boolean? IsImported
		{
			get
			{
				return this._IsImported;
			}
			set
			{
				this._IsImported = value;
			}
		}
		#endregion
		#region CreatedByID
		public abstract class createdByID : PX.Data.IBqlField
		{
		}
		protected Guid? _CreatedByID;
		[PXDBCreatedByID()]
		public virtual Guid? CreatedByID
		{
			get
			{
				return this._CreatedByID;
			}
			set
			{
				this._CreatedByID = value;
			}
		}
		#endregion
		#region CreatedByScreenID
		public abstract class createdByScreenID : PX.Data.IBqlField
		{
		}
		protected String _CreatedByScreenID;
		[PXDBCreatedByScreenID()]
		public virtual String CreatedByScreenID
		{
			get
			{
				return this._CreatedByScreenID;
			}
			set
			{
				this._CreatedByScreenID = value;
			}
		}
		#endregion
		#region CreatedDateTime
		public abstract class createdDateTime : PX.Data.IBqlField
		{
		}
		protected DateTime? _CreatedDateTime;
		[PXDBCreatedDateTime()]
		public virtual DateTime? CreatedDateTime
		{
			get
			{
				return this._CreatedDateTime;
			}
			set
			{
				this._CreatedDateTime = value;
			}
		}
		#endregion
		#region LastModifiedByID
		public abstract class lastModifiedByID : PX.Data.IBqlField
		{
		}
		protected Guid? _LastModifiedByID;
		[PXDBLastModifiedByID()]
		public virtual Guid? LastModifiedByID
		{
			get
			{
				return this._LastModifiedByID;
			}
			set
			{
				this._LastModifiedByID = value;
			}
		}
		#endregion
		#region LastModifiedByScreenID
		public abstract class lastModifiedByScreenID : PX.Data.IBqlField
		{
		}
		protected String _LastModifiedByScreenID;
		[PXDBLastModifiedByScreenID()]
		public virtual String LastModifiedByScreenID
		{
			get
			{
				return this._LastModifiedByScreenID;
			}
			set
			{
				this._LastModifiedByScreenID = value;
			}
		}
		#endregion
		#region LastModifiedDateTime
		public abstract class lastModifiedDateTime : PX.Data.IBqlField
		{
		}
		protected DateTime? _LastModifiedDateTime;
		[PXDBLastModifiedDateTime()]
		public virtual DateTime? LastModifiedDateTime
		{
			get
			{
				return this._LastModifiedDateTime;
			}
			set
			{
				this._LastModifiedDateTime = value;
			}
		}
		#endregion
	}
}
