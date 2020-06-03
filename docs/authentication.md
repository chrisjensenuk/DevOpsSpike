# Authentication

resources:
    https://briantjackett.com/2018/07/25/azure-functions-calling-azure-ad-application-with-certificate-authentication/
    https://westerndevs.com/certificates-azurefunctions/Testing-Client-Cert-Auth/


## Client Certificates

1) Get the function to do its own authentication using certificates stored in key vault

### Create a self cert certificate
Using PowerShell (Run as **Administrator**)):

```
$CertificateStoreLocation = "cert:\CurrentUser\My"
$ProviderName = "Microsoft Software Key Storage Provider"
$CertificateSubject = "CN=DevOpsSpikeClient"
$CertificateDescription =  "The client certificate to use when calling the Function app"
$CertificateNotAfterYears = 1
$CertificateDNSName = "DevOpsSpikeClient"
$certificatePFXPath = "c:\devopsspikeclient.pfx"
$CertificatePassword = "password"
$certificateCRTPath = "c:\devopsspikeclient.cer"

#Create certificate
$ssc = New-SelfSignedCertificate -CertStoreLocation $CertificateStoreLocation -Provider $ProviderName `
    -Subject "$CertificateSubject" -KeyDescription "$CertificateDescription" `
    -NotBefore (Get-Date).AddDays(-1) -NotAfter (Get-Date).AddYears($CertificateNotAfterYears) `
    -DnsName $CertificateDNSName -KeyExportPolicy Exportable
```

This will create a certificate for the CURRENT USER\Personal\Certificates

```
# Export certificate to PFX (public & private key)
$CertificatePasswordSecure = ConvertTo-SecureString $CertificatePassword -AsPlainText -Force
Export-PfxCertificate -cert cert:\CurrentUser\My\$($ssc.Thumbprint) -FilePath $certificatePFXPath -Password $CertificatePasswordSecure -Force
 
# Export certificate to CER (public key only)
Export-Certificate -Cert cert:\CurrentUser\My\$($ssc.Thumbprint) -FilePath $certificateCRTPath -Force
```

TODO:
- ~~Create a client certificate (self signed if OK for now)~~
- ~~Create local console application that add the certificate to a HttpClient request~~
- Add new Function that pulls a certificate from the key vault and authenticates the request
- need to debug Fubnctions locally using HTTPS

2) Get APIM to authenticate via client certificates

## Bearer Authentication

TODO

