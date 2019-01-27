using PX.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackathon
{
    [PXLocalizable(Prefix)]
    public class TDMessages
    {
        public const string Prefix = "TD Hackathon Msg";

        public const string MainError = "Please contact your System Administrator, the following issues have been found:";

        public const string SomePluginRunning = "SomePlugin running on Company {0}";

        public const string NoActiveAccounts = "There are no Active Accounts in Company {0}, Package cannot be published.";

        public const string ErrorInsertingItems = "An exception occurred while trying to insert UpScout records on Company {0}.";

    }
}
