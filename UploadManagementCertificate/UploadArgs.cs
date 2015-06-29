using PowerArgs;

namespace UploadManagementCertificate
{
    [ArgExample("Sixeyed.AzureTools.UploadManagementCertificate.exe -s \"abc6371f-8abf-4210-9c21-7f620a9abcd3\" -c \"c:\\path\\to\\my-mgmt-app.cer\"", "Specify ther subscription ID and .cer file path")]
    public class UploadArgs
    {
        [ArgRequired(PromptIfMissing=true)]
        [ArgDescription("ID of the Azure subscription - use GetAzureSubscription to check")]
        public string SubscriptionId { get; set; }

        [ArgRequired(PromptIfMissing=true)]
        [ArgDescription("Path to the .cer file for the management cert - use makecert to generate")]
        public string CertificateFilePath { get; set; }
    }
}

