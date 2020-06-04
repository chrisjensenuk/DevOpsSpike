# Authentication

resources:
    https://briantjackett.com/2018/07/25/azure-functions-calling-azure-ad-application-with-certificate-authentication/
    https://westerndevs.com/certificates-azurefunctions/Testing-Client-Cert-Auth/
    https://stackoverflow.com/questions/49686316/azure-functions-configure-client-certificate-authentication
    https://docs.microsoft.com/en-us/azure/app-service/app-service-web-configure-tls-mutual-auth



## Client Certificates

1) Get the function to do its own authentication using certificates stored in key vault

### Create a self cert certificate

This will be the certificate the client will use to identify itself

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

## Running Functions locally in Https
Export the localhost certificate (ASP.NET Core HTTPS development certificate) from Current User > Personal > Cetificates to c:\localhost.pfx with a password of "password"

In Visual Studio get the Functions Host to use HTTPS. In Project Properties > Debug add the below to Application arguments
```
start --build --cert "C:\localhost.pfx" --password "password" --useHttps
```

**Update - I don't think we should do this. In Azure, TLS is terminated at the load balancer so locally we should use HTTP.**

## Client Certificates and Azure
TLS is terminated at Azure's load balancers so Function HttpTriggers run as HTTP. Certificates cannot be 'passed through' over HTTP so Azure adds the client certificate to a header called `X-ARR-ClientCert`.

## Configuring Function App Service to require clinet certificates
In Azure Portal
- App Service > TLS/SSL settings > HTTPS Only : On
- App Service > Configuration > General settings > Require client certificates : On

# TODO:
- ~~Create a client certificate (self signed if OK for now)~~
- ~~Create local console application that add the certificate to a HttpClient request~~
- Add new Function that pulls a certificate from the key vault and authenticates the request
- need to debug Fubnctions locally using HTTPS

2) Get APIM to authenticate via client certificates

## Bearer Authentication

TODO

