using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Management;
using Microsoft.WindowsAzure.Management.Models;
using PowerArgs;
using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace UploadManagementCertificate
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var uploadArgs = Args.Parse<UploadArgs>(args);
                UploadNewCertificate(uploadArgs);
            }
            catch (ArgException ex)
            {
                Console.WriteLine(ex.Message);
                ArgUsage.GetStyledUsage<UploadArgs>().Write();
            }            
        }

        private static void UploadNewCertificate(UploadArgs uploadArgs)
        {
            var existingCert = GetExistingManagementCertificate(uploadArgs.SubscriptionId);
            if (existingCert == null)
            {
                Console.WriteLine("Cannot find existing mangement cert from Windows Azure Tools");
                return;
            }

            var status = UploadNewManagementCert(uploadArgs.SubscriptionId, existingCert, uploadArgs.CertificateFilePath);
            if (status == HttpStatusCode.Created)
            {
                Console.WriteLine("Uploaded certificate suceeded!");
            }
            else
            {
                Console.WriteLine("Upload certificate failed. Status: " + Enum.GetName(typeof(HttpStatusCode), status));
            }
        }

        private static X509Certificate2 GetExistingManagementCertificate(string subscriptionId)
        {
            var certStore = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            certStore.Open(OpenFlags.ReadOnly);
            var azureCerts = certStore.Certificates.Find(X509FindType.FindByIssuerName, "Windows Azure Tools", false);
            foreach (var cert in azureCerts)
            {
                var creds = new CertificateCloudCredentials(subscriptionId, cert);
                var client = new ManagementClient(creds);
                try
                {
                    var v = client.Locations.List();
                    return cert;
                }
                catch (CloudException)
                { }
            }

            return null;
        }

        private static HttpStatusCode UploadNewManagementCert(string subscriptionId, X509Certificate2 existingCertificate, string newCertificateCerFilePath)
        {
            var statusCode = HttpStatusCode.Unused;
            var newCertificate = new X509Certificate2(newCertificateCerFilePath);

            var creds = new CertificateCloudCredentials(subscriptionId, existingCertificate);
            var client = new ManagementClient(creds);

            var parm = new ManagementCertificateCreateParameters()
            {
                Data = newCertificate.RawData,
                PublicKey = newCertificate.GetPublicKey(),
                Thumbprint = newCertificate.Thumbprint
            };

            //Hyak throws an exception for a Created result, which is actually  the success code
            try
            {
                var response = client.ManagementCertificates.Create(parm);
            }
            catch (CloudException ex)
            {
                statusCode = ex.Response.StatusCode;
            }

            return statusCode;
        }
    }
}
