
#Upload Management Certificate

Tool for uploading a new management cert to an Azure subscription - see [How to programmatically upload a new Azure Management Certificate] (http://geekswithblogs.net/EltonStoneman/archive/2015/06/29/how-to-programmatically-upload-a-new-azure-management-certificate.aspx).

##Usage

1) make sure you have an existing management cert installed locally, which you will get form installing the Azure SDK for Visual Studio or PowerShell. Check your installed Azure certs in PowerShell:

```powershell
Get-ChildItem -path cert:\\CurrentUser\My -DnsName 'Windows Azure Tools'
```

2) Generate a cert using MakeCert, specifying a sensible CN so you can idenitfy the cert:

```
makecert -sky exchange -r -n "CN=my.mgt.app.name" -pe -a sha1 -len 2048 -ss My my-mgmt-app.cer
```

3) Upload your new cert with the tool, it will use your existing Azure Tools cert to authenticate and upload the new cert:

```
Sixeyed.AzureTools.UploadManagementCertificate.exe -SubscriptionId 'my-subscription-id' -CertificateFilePath 'my-mgmt-app.cer'
```

##Calling Azure APIs 

You can use the cert in your local store to call APIs via the Management Libraries, or export the cert as a .PFX to use with other tools.

The simplest is to call the REST APIs with [cURL] (http://curl.haxx.se), but you'll need to reformat the PFX file using [OpenSSL] (https://www.openssl.org):

```
openssl pkcs12 -in my-mgmt-app.pfx -out my-mgmt-app.pem
```

Then you can call the APIs specifying the cert file (you need the -k because your management cert is not an SSL client cert for the Azure domain):

```
curl 
  -H x-ms-version:2015-04-01 
  -E my-mgmt-app.pem 
  -k
  https://management.core.windows.net/362fad90-26f2-41b3-ad73-ebcd83bc74be/services/hostedservices
```

