using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Cryptography.X509Certificates;

namespace DevOpsSpike
{
    public static class ClientCertificateFunction
    {
        [FunctionName("ClientCertificateFunction")]
        public static async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {

            if (req.Headers.Keys.Contains("X-ARR-ClientCert"))
            {
                byte[] clientCertBytes = Convert.FromBase64String(req.Headers["X-ARR-ClientCert"]);
                var clientCert = new X509Certificate2(clientCertBytes);
                
                return new OkObjectResult("Thumbprint: " + clientCert.Thumbprint);

                //todo: Validate the certificate against Key Vault
            }

            return new OkObjectResult("No header called X-ARR-ClientCert");
        }
    }
}
