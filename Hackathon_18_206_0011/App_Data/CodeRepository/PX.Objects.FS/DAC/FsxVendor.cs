﻿using PX.Data;
using PX.Objects.AP;
using PX.Objects.CS;

namespace PX.Objects.FS
{
    [PXTable(typeof(Vendor.bAccountID), IsOptional = true)]
    public class FSxVendor : PXCacheExtension<Vendor>
	{
        public static bool IsActive()
        {
            return PXAccess.FeatureInstalled<FeaturesSet.serviceManagementModule>();
        }

        #region SDEnabled
        public abstract class sDEnabled : PX.Data.IBqlField
        {
        }
        [PXDBBool]
        [PXDefault(false)]
        [PXUIField(DisplayName = "Staff Member in " + TX.ModuleName.SERVICE_DISPATCH)]
        public virtual bool? SDEnabled { get; set; }
        #endregion
        #region SendAppointmentNotification
        public abstract class sendAppNotification : PX.Data.IBqlField
        {
        }

        [PXDBBool]
        [PXDefault(false)]
        [PXUIField(DisplayName = "Allow Appointment Notifications", Enabled = false)]
        public virtual bool? SendAppNotification { get; set; }
        #endregion
	}
}
