# ARM Template

The ARM template is located at the root of the repo [azuredeploy.json](/azuredeploy.json)

## Creating and securing function app and secrets

- Key Vault is created
- The Key Vault secrets are created
- The Function app service is created (depends on Key Vault secret available so it can update app settings)
- Key Vault access policies are then created (depends on Key Vault & Function App being created so Object Id can be set with the Apps Identity Prinaicpal Id)
