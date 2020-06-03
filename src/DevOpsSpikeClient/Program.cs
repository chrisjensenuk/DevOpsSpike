using System;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace DevOpsSpikeClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("What's the absolute URL of the secured Function?");
            var requestUrl = Console.ReadLine();

            Console.WriteLine("Enter the Thumbprint of the cert in the local user's Personal/Certificate store");
            var thumbprint = Console.ReadLine();

            var cert = GetCertificate(thumbprint);

            if (cert == null)
            {
                Console.WriteLine($"Cannot find certificate in the local user's Personal/Certificate store with thumbprint '{thumbprint}'");
            }
            else
            {
                var clientHandler = new HttpClientHandler();
                clientHandler.ClientCertificates.Add(cert);
                var client = new HttpClient(clientHandler);

                var response = await client.GetStringAsync(requestUrl);

                Console.WriteLine($"Function response:{response}");
            }

            Console.WriteLine("Press enter to quit");
            Console.ReadLine();
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
