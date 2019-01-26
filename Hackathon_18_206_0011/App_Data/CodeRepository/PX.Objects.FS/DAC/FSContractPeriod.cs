﻿using System;
using PX.Data;
using PX.Objects.CM;

namespace PX.Objects.FS
{
    [System.SerializableAttribute]
    public class FSContractPeriod : PX.Data.IBqlTable
    {
        #region ServiceContractID
        public abstract class serviceContractID : PX.Data.IBqlField
        {
        }

        [PXDBInt(IsKey = true)]
        [PXParent(typeof(Select<FSServiceContract, Where<FSServiceContract.serviceContractID, Equal<Current<FSContractPeriod.serviceContractID>>>>))]
        [PXDBLiteDefault(typeof(FSServiceContract.serviceContractID))]
        [PXUIField(DisplayName = "Service Contract ID")]
        public virtual int? ServiceContractID { get; set; }
        #endregion
        #region ContractPeriodID
        public abstract class contractPeriodID : PX.Data.IBqlField
        {
        }

        [PXDBIdentity(IsKey = true)]
        public virtual int? ContractPeriodID { get; set; }
        #endregion
        #region ContractPostDocID
        public abstract class contractPostDocID : PX.Data.IBqlField
        {
        }

        [PXDBInt]
        public virtual int? ContractPostDocID { get; set; }
        #endregion
        #region EndPeriodDate
        public abstract class endPeriodDate : PX.Data.IBqlField
        {
        }

        [PXDBDate]
        [PXDefault]
        [PXUIField(DisplayName = "End Period Date")]
        public virtual DateTime? EndPeriodDate { get; set; }
        #endregion
        #region Invoiced
        public abstract class invoiced : PX.Data.IBqlField
        {
        }

        [PXDBBool]
        [PXDefault(false)]
        [PXUIField(DisplayName = "Invoiced")]
        public virtual bool? Invoiced { get; set; }
        #endregion
        #region PeriodTotal
        public abstract class periodTotal : PX.Data.IBqlField
        {
        }

        [PXDBBaseCury]
        [PXDefault(TypeCode.Decimal, "0.0")]
        [PXUIField(DisplayName = "Period Total")]
        public virtual decimal? PeriodTotal { get; set; }
        #endregion
        #region StartPeriodDate
        public abstract class startPeriodDate : PX.Data.IBqlField
        {
        }

        [PXDBDate]
        [PXDefault]
        [PXUIField(DisplayName = "Start Period Date")]
        public virtual DateTime? StartPeriodDate { get; set; }
        #endregion
        #region Status
        public abstract class status : ListField_Status_ContractPeriod
        {
        }

        [PXDefault(ID.Status_ContractPeriod.INACTIVE)]
        [status.ListAtrribute]
        [PXDBString(1, IsUnicode = true)]
        public virtual string Status { get; set; }
        #endregion
        #region CreatedByID
        public abstract class createdByID : PX.Data.IBqlField
        {
        }

        [PXDBCreatedByID]
        [PXUIField(DisplayName = "Created By ID")]
        public virtual Guid? CreatedByID { get; set; }
        #endregion
        #region CreatedByScreenID
        public abstract class createdByScreenID : PX.Data.IBqlField
        {
        }

        [PXDBCreatedByScreenID]
        [PXUIField(DisplayName = "Created By Screen ID")]
        public virtual string CreatedByScreenID { get; set; }
        #endregion
        #region CreatedDateTime
        public abstract class createdDateTime : PX.Data.IBqlField
        {
        }

        [PXDBCreatedDateTime]
        [PXUIField(DisplayName = "Created DateTime")]
        public virtual DateTime? CreatedDateTime { get; set; }
        #endregion
        #region LastModifiedByID
        public abstract class lastModifiedByID : PX.Data.IBqlField
        {
        }

        [PXDBLastModifiedByID]
        [PXUIField(DisplayName = "Last Modified By ID")]
        public virtual Guid? LastModifiedByID { get; set; }
        #endregion
        #region LastModifiedByScreenID
        public abstract class lastModifiedByScreenID : PX.Data.IBqlField
        {
        }

        [PXDBLastModifiedByScreenID]
        [PXUIField(DisplayName = "Last Modified By Screen ID")]
        public virtual string LastModifiedByScreenID { get; set; }
        #endregion
        #region LastModifiedDateTime
        public abstract class lastModifiedDateTime : PX.Data.IBqlField
        {
        }

        [PXDBLastModifiedDateTime]
        [PXUIField(DisplayName = "Last Modified Date Time")]
        public virtual DateTime? LastModifiedDateTime { get; set; }
        #endregion

        #region Memory Fields
        #region BillingPeriod
        public abstract class billingPeriod : PX.Data.IBqlField
        {
        }

        [PXString]
        [PXUIField(DisplayName = "Billing Period", IsReadOnly = true, Enabled = false)]
        public virtual string BillingPeriod { get; set; }
        #endregion
        #endregion
    }
}