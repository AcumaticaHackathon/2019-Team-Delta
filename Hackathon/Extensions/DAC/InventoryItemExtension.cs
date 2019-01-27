using PX.Data;
using PX.Objects.IN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackathon
{
    public class InventoryItemExtension : PXCacheExtension<InventoryItem>
    {
        #region EndpointURL
        public abstract class usrTDEndpoint: PX.Data.IBqlField
        {
        }
        [PXDBString]
        [PXUIField(DisplayName = "Trigger Endpoint")]
        public virtual string UsrTDEndpoint { get; set; }
        #endregion
    }
}
