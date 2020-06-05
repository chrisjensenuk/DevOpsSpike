# Mutual TLS by using Client Certificates Authentication

Create a self signed certificate and save in Users Personal Certificate store. The console app then uses this 'client certificate' for Mutual TLS to the Azure App Service.

- [Function that requires a Client Certificate](/src/DevOpsSpike/ClientCertificateFunction.cs)
- [Console app that will send a client certificate](/src/DevOpsSpikeClient/Program.cs)

resources:  
https://stackoverflow.com/questions/49686316/azure-functions-configure-client-certificate-authentication  
https://docs.microsoft.com/en-us/azure/app-service/app-service-web-configure-tls-mutual-auth  
https://damienbod.com/2019/09/07/using-certificate-authentication-with-ihttpclientfactory-and-httpclient/

### Create a self cert certificate

This will be the certificate the client will use to identify itself.

Using PowerShell (Run as **Administrator**)):

```
$CertificateStoreLocation = "cert:\CurrentUser\My"
$ProviderName = "Microsoft Software Key Storage Provider"
$CertificateSubject = "CN=DevOpsSpikeClient"
$CertificateDescription =  "The client certificate to use when calling the Function app"
$CertificateNotAfterYears = 1
$CertificateDNSName = "DevOpsSpikeClient"

#Create certificate
$ssc = New-SelfSignedCertificate -CertStoreLocation $CertificateStoreLocation -Provider $ProviderName `
    -Subject "$CertificateSubject" -KeyDescription "$CertificateDescription" `
    -NotBefore (Get-Date).AddDays(-1) -NotAfter (Get-Date).AddYears($CertificateNotAfterYears) `
    -DnsName $CertificateDNSName -KeyExportPolicy Exportable
```

This will create a certificate for the CURRENT USER\Personal\Certificates

If you want to export the certificate...
```
# Export certificate to PFX (public & private key)
$certificatePFXPath = "c:\devopsspikeclient.pfx"
$CertificatePassword = "password"
$CertificatePasswordSecure = ConvertTo-SecureString $CertificatePassword -AsPlainText -Force
Export-PfxCertificate -cert cert:\CurrentUser\My\$($ssc.Thumbprint) -FilePath $certificatePFXPath -Password $CertificatePasswordSecure -Force
 
# Export certificate to CER (public key only)
$certificateCRTPath = "c:\devopsspikeclient.cer"
Export-Certificate -Cert cert:\CurrentUser\My\$($ssc.Thumbprint) -FilePath $certificateCRTPath -Force
```

## Client Certificates and Azure
TLS is terminated at Azure's load balancers so Function HttpTriggers run as HTTP. Certificates cannot be 'passed through' over HTTP so Azure adds the client certificate to a header called `X-ARR-ClientCert`.

## Configuring the Function's App Service to require client certificates
In Azure Portal
- App Service > TLS/SSL settings > HTTPS Only : On
- App Service > Configuration > General settings > Require client certificates : On

# TODO:
- ~~Create a client certificate (self signed if OK for now)~~
- ~~Create local console application that add the certificate to a HttpClient request~~