using Customization;
using PX.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackathon
{
    public class TDCustomizationPlugin : CustomizationPlugin
    {

        //For DB Events
        public override void UpdateDatabase()
        {
            List<Tuple<string, object[]>> errors = new List<Tuple<string, object[]>>();

            TDPlugin.UpdateDatabase(this, ref errors);

            HandleExceptions(errors);
        }

        //Fires after the publish is finished. Don't make DB changes here
        public override void OnPublished()
        {
            List<Tuple<string, object[]>> errors = new List<Tuple<string, object[]>>();

            //TDPlugin.OnPublished(this, ref errors);
        }

        private void HandleExceptions(List<Tuple<string, object[]>> errors)
        {
            if (errors.Count != 0)
            {
                //Normally you shouldn't localize Exceptions, but this code generates an amalgamation of errors.
                StringBuilder builder = new StringBuilder(PXLocalizer.Localize(TDMessages.MainError, typeof(TDMessages).FullName));

                foreach (Tuple<string, object[]> error in errors)
                {
                    builder.AppendLine(PXLocalizer.LocalizeFormatWithKey(error.Item1, typeof(TDMessages).FullName, error.Item2));
                }

                throw new PXException(builder.ToString());
            }
        }
    }
}
