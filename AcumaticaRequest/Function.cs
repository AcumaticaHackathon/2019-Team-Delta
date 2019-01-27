using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace AcumaticaRequest
{
    public class Function
    {

        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public void FunctionHandler(ILambdaContext context)
        {
            string error;
            try
            {
                using (AcumaticaProcessor proc = new AcumaticaProcessor())
                {
                    proc.Login("admin", "123");
                    proc.UpdateContractUsageDetails();

                }
            }
            catch (Exception e)
            {
                error = "Error";
            }
        }
    }
}
