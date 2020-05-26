# DevOpsSpike
A spike project exploring Azure DevOps, ARM templates, deployments, etc 

## Deploy via Kudu/SlingShot
[![Deploy to Azure](https://aka.ms/deploytoazurebutton)](https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fchrisjensenuk%2FDevOpsSpike%2Fmaster%2Fazuredeploy.json)

## Deploy via CLI
Locally from repo root from Windows CMD with Azure CLI installed:
```
SET rg=rg-devopsspike-dev-001
SET location=eastus
az login
az group create --location %location% --name %rg%
az deployment group create --resource-group=%rg% --template-file azuredeploy.json --parameters @parameters.azuredeploy.json
```
*todo* also need to deploy the code too


# Todo
- Use naming conventions for deployes assets. https://docs.microsoft.com/en-us/azure/cloud-adoption-framework/ready/azure-best-practices/naming-and-tagging
- Create examples: deploy from CLI, PowerShell, ~~Kudu SlingShot ('deploy to Azure button')~~, Azure Cloud Shell
- Create Unit Test and Integration Test projects
- Create DevOps CI/CD pipelines
- Create Key Vault (implement local and server secure config). Add Key Valut to deployment
- Containerize App and deploy to ACS and K8s