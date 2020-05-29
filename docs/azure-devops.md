[&lt; back to README.md](../README.md)

# Azure DevOps

https://dev.azure.com/

Pipeline is version controlled in YAML [azure-pipelines.yml](../azure-pipelines.yml)

You can change the Yaml in DevOps and it will automatically commit back to the repo (master or on a new branch).
I'm using a multi-stage YAML pipeline instead of separate 'Releases'

- Created a new Azure Pipelines account
- Create Azure environment then used the wizard to build a pipeline for deploying a Functions app
- Added a Test stage
- Added an Approval stage for deployment to Developement by speciying `Environment`

# Run CI on Pull Requests
- Created a pipeline `DevOpsSpike PR CI` which just runs a build and test
- Added `pr: master` as the trigger for the pipeline
- Added a rule to the GitHub master branch to `Require status checks to pass before merging` and selected the `DevOpsSpike PR CI`


## TODO
- ~~Create test step in build~~
- ~~Create gated release~~
- ~~Do CI build on repo check-in~~
- Run Integration tests once deployed to Development env
- Prevent PR merge on to master without successful CI
- Add status badge to the README.md https://docs.microsoft.com/en-us/azure/devops/pipelines/create-first-pipeline?view=azure-devops&tabs=java%2Cyaml%2Cbrowser%2Ctfs-2018-2#get-the-status-badge





