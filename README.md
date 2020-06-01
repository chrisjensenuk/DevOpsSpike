# DevOpsSpike
A spike project exploring Azure DevOps, ARM templates, deployments, etc.  

The project consists of an Azure Function and Unit Test project.

[![Build Status](https://dev.azure.com/chrisjensenuk/chrisjensenuk-spike/_apis/build/status/DevOpsSpike%20CI%20CD?branchName=master&stageName=Build%20stage)](https://dev.azure.com/chrisjensenuk/chrisjensenuk-spike/_build/latest?definitionId=1&branchName=master)

## Deploy via Custom Templates
[![Deploy to Azure](https://aka.ms/deploytoazurebutton)](https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fchrisjensenuk%2FDevOpsSpike%2Fmaster%2Fazuredeploy.json)

[Visualize on Armviz.io](http://armviz.io/#/?load=https%3A%2F%2Fraw.githubusercontent.com%2Fchrisjensenuk%2FDevOpsSpike%2Fmaster%2Fazuredeploy.json")

## Deploy Resources via CLI
Locally from repo root from Windows CMD with Azure CLI installed:
*TODO* reformat below so works with bash instead.
```
SET rg=rg-devopsspike-dev-001
SET location=eastus
az login
az group create --location %location% --name %rg%
az deployment group create --resource-group=%rg% --template-file azuredeploy.json
```

## Publish the Application via Visual Studio 2019
- Publish (may need to delete the existing profile if Azure environment has changed)
- Azure Functions Consumption Plan
- Select Existing. Run from packahe file checked. Create profile
- Select created App Service. OK
- Click Publish


## Publish the Application via CMD
Publish the project. Zip it. Then deploy to Azure
```
cd src
dotnet publish -c Release
SET publishFolder=DevOpsSpike\bin\Release\netcoreapp3.1\publish
SET publishZip=publish.zip
tar.exe -a -c -f %publishZip% %publishFolder%

*TODO* Need to retrieve the $FunctionAppName for below to work
# az functionapp deployment source config-zip --resource-group %rg% -n %functionAppName% --src %publishZip%
```



# TODO
- Use naming conventions for deployes assets. https://docs.microsoft.com/en-us/azure/cloud-adoption-framework/ready/azure-best-practices/naming-and-tagging
- Create examples: deploy from CLI, PowerShell, ~~Custom Templates ('deploy to Azure button')~~, Azure Cloud Shell
- Create Unit Test and Integration Test projects
- Create DevOps CI/CD pipelines
- Create Key Vault (implement local and server secure config). Add Key Valut to deployment
- Deploy to slots
- Containerize App and deploy to ACS and K8s
- Integrate Azure AD

# Resources
Get quick starts from here: https://github.com/Azure/azure-quickstart-templates
Used ARM template from here https://github.com/Azure/azure-quickstart-templates/tree/master/101-function-app-create-dynamic


# Configuring Linux bash on Windows
For Azure CLI I want to use Linux Bash although I'm a Windows user.  This means I can cut and paste between local and Azure shell. Waiting for Windows 10 2004 Build v19041 to drop then I'll install WSL2. Ubuntu then the Azure CLI.
- Configurating bash and Azure CLi on Windows https://roykim.ca/2020/03/05/managing-azure-with-az-cli-and-windows-subsystem-for-linux/
- Install WSL2 https://docs.microsoft.com/en-us/windows/wsl/install-win10

#Learnings
Originally I created the environment in Azure Portal and exported it as an ARM template to create azuredeploy.json.  There is too much extra cruft in the created JSON that I don't care about.  Instead I'll use quickstart templates as a base template and tweak as I need to. https://github.com/Azure/azure-quickstart-templates

# Azure DevOps
[Documentation regarding my journey with Azure DevOps](docs/azure-devops.md)

# Secrets
[Documentation regarding managing secrets](docs/secrets.md)