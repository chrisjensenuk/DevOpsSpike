using System;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DevOpsSpikeClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Are you testing locally against Functions Core Tools? (y/n)");
            var local = Console.ReadLine().ToLower() == "y";

            Console.WriteLine("What's the absolute URL of the secured Function?");
            var requestUrl = Console.ReadLine();

            Console.WriteLine("What's the Thumbprint of the cert in the local user's Personal/Certificate store");
            var thumbprint = Console.ReadLine();

            var cert = GetCertificate(thumbprint);

            if (cert == null)
            {
                Console.WriteLine($"Cannot find certificate in the local user's Personal/Certificate store with thumbprint '{thumbprint}'");
            }
            else
            {
                while (true)
                {
                    HttpClient client = local ? GetClientForLocal(cert) : GetClientForAzure(cert);

                    var response = await client.GetStringAsync(requestUrl);

                    Console.WriteLine($"Function response:{response}");

                    Console.ReadLine();
                }
            }            
        }

        private static HttpClient GetClientForAzure(X509Certificate2 cert)
        {
            var clientHandler = new HttpClientHandler();
            clientHandler.ClientCertificates.Add(cert);
            return new HttpClient(clientHandler);
        }

        private static HttpClient GetClientForLocal(X509Certificate2 cert)
        {
            //Debugging locally so need to add the certificate as a header
            //When in Azure the load balancer will terminate the TLS and put the client certificate into the header
            var client = new HttpClient();

            var certString = cert.GetRawCertDataString();
            var certBytes = Encoding.UTF8.GetBytes(certString);
            var certStringBase64 = Convert.ToBase64String(cert.RawData);

            client.DefaultRequestHeaders.Add("X-ARR-ClientCert", certStringBase64);

            return client;
        }

        public static X509Certificate2 GetCertificate(string thumbprint)
        {
            X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            try
            {
                store.Open(OpenFlags.ReadOnly);

                var col = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false);
                if (col == null || col.Count == 0)
                {
                    return null;
                }
                return col[0];
            }
            finally
            {
                store.Close();
            }
        }
    }
}
