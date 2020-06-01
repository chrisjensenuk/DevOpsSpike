# Secrets

Using a Key Vault to store secrets

- Created a Key Vault using the portal
- In the function's App Service. `Identity` > `System assigned identity` for the Function App Service in the Portal
> A system assigned managed identity enables Azure resources to authenticate to cloud services (e.g. Azure Key Vault) without storing credentials in code. Once enabled, all necessary permissions can be granted via Azure role-based-access-control. The lifecycle of this type of managed identity is tied to the lifecycle of this resource. Additionally, each resource (e.g. Virtual Machine) can only have one system assigned managed identity.
- In `Key Vault go` to `Access Policies`. `Add access policy`. Principal = 'name of function app'. Secret Permissions = 'Get'

## Notes
- You can link the AppSettings to the Keyvaule by providing this as the value in appSettings
```
@Microsoft.KeyVault(SecretUri=https://kv-devopsspike.vault.azure.net/secrets/the-secret/02016bcf04e541dd84930b202f597ae2)
```
However this includes the version of the secret `02016bcf04e541dd84930b202f597ae2` so if the secret changes in the Key Vault then the version here needs to be updated to to point to the updated secret.  I think this will be a burden however I think you can just specify the URL without the version slug (todo: need to test).  However (I think) applications that automatically reload on configuration changes will not be aware of the Key Vault change so app will need to be manaully restarted (todo check this hypothesis).

## User Secrets
I like using User Secrets over Environment Variables for local development so I've tried adding User Secrets to the Function app.  I've got it working but it means introducing `Startup.cs` and adding exrtra libraries and setup. It also means that I have to turn my Function into instance based inject a `IConfiguration` into it.  I then have to use this config throughout the function and not rely on environment variables.

You can use `local.settings.json` for local configuration but then you have to add it to `.gitIgnore`. This is unacceptable to me - I really don't want secrets inside the repo structure even if they are being ignored.

*Take away* Functions (v3) REALLY want to only use Enviroment Variables. So in future I won't use User Secrets and just set Environment Variables locally. Therefore avoiding extra cruft and complexity just to get local dev working.

## TODO
- When adding an access policy how does 'Configure from template' and 'Authorized application' work?
- 

