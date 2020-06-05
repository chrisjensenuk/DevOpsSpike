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

            if (!req.Headers.Keys.Contains("X-ARR-ClientCert"))
                return new OkObjectResult("No header called X-ARR-ClientCert");

            var clientCertString = req.Headers["X-ARR-ClientCert"];
            byte[] clientCertBytes = Convert.FromBase64String(clientCertString);
            var clientCert = new X509Certificate2(clientCertBytes);

            //Validate the certificate
            var chain = new X509Chain();
            var policy = new X509ChainPolicy
            {
                RevocationMode = X509RevocationMode.NoCheck,
            };

            chain.ChainPolicy = policy;

            var errors = "";
            if (!chain.Build(clientCert))
            {
                foreach (X509ChainElement chainElement in chain.ChainElements)
                {
                    foreach (X509ChainStatus chainStatus in chainElement.ChainElementStatus)
                    {
                        errors += chainStatus.StatusInformation;
                    }
                }
            }

            //todo: If certificate is valid then map CN to claims/some kind of identity.

            return new OkObjectResult($"Thumbprint: {clientCert.Thumbprint} Errors: {errors}");
        }
    }
}
