[&lt; back to README.md](../README.md)

# Azure DevOps

https://dev.azure.com/

Pipeline is version controlled in YAML [azure-pipelines.yml](../azure-pipelines.yml)

You can change the Yaml in DevOps and it will automatically commit back to the repo (master or on a new branch).
I'm using a multi-stage YAML pipeline instead of separate 'Releases'

- Created a new Azure Pipelines account
- Create Azure environment then used the wizard to build a pipeline for deploying a Functions app
- Added a Test stage
- Added an Approval stage for deployment to Developement by speciying Environment

## TODO
- ~~Create test step in build~~
- ~~Create gated release~~
- ~~Do CI build on repo check-in~~
- Run Integration tests once deployed to Development env



