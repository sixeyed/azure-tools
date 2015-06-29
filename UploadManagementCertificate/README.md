
Tool for uploading a new management cert to an Azure subscription.

Usage

1) make sure you have an existing management cert installed locally, which you will get form installing the Azure SDK for Visual Studio or PowerShell. Check your installed Azure certs in PowerShell:

Get-ChildItem -path cert:\\CurrentUser\My -DnsName 'Windows Azure Tools'

2) Generate a cert using MakeCert, specifying a sensible CN so you can idenitfy the cert:

makecert -sky exchange -r -n "CN=my.mgt.app.name" -pe -a sha1 -len 2048 -ss My my-mgmt-app.cer

3) Upload your new cert with the tool, it will use your existing Azure Tools cert to authenticate and upload the new cert:

Sixeyed.AzureTools.UploadManagementCertificate.exe -SubscriptionId 'my-subscription-id' -CertificateFilePath 'my-mgmt-app.cer'
