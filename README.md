# Assistants: Budget

The Assistants: Budget is a project designed to help individuals and families track their incomes and expenses, calculate relevant statistics, and provide insightful infographics for effective financial management. With the goal of promoting better financial health and planning, this budget assistant offers a comprehensive set of features to simplify the process of tracking and analyzing finances.

# Architecture

![](./docs/architecture-general.drawio.svg)

# Git Flow

![](./docs/git-flow.drawio.svg)

# Ops

## GitHub Variables

| Variable Name                             | Storage | Type   | Level       | Project                | Description                                                 |
| ----------------------------------------- | ------- | ------ | ----------- | ---------------------- | ----------------------------------------------------------- |
| AWS_ACM_CERTIFICATE_ARN                   | Github  | SECRET | Global      | All                    | ARN of the ACM Certificate for HTTPS                        |
| AWS_ROUTE53_HOSTED_ZONE_ID                | Github  | SECRET | Global      | All                    | Route53 Zone ID where domain name is registered             |
| AWS_ACCESS_KEY_ID                         | Github  | SECRET | Global      | All                    | AWS Key ID for terraform user                               |
| AWS_SECRET_ACCESS_KEY                     | Github  | SECRET | Global      | All                    | AWS Key Secret for terraform user                           |
| TF_API_TOKEN                              | Github  | SECRET | Global      | All                    | Key of Terraform Cloud                                      |
| AWS_ECR_REGISTRY                          | Github  | SECRET | Global      | All                    | ECR repo root URL                                           |
| AWS_ECR_REPOSITORY_BE_API                 | Github  | SECRET | Global      | Assistant: Budget - BE | ECR repo name for docker images of Budget.API project       |
| AWS_S3_BUDGET_UI_BUCKET                   | Github  | SECRET | Environment | Assistant: Budget - UI | S3 Bucket name where build assets are stored                |
| AWS_CLOUDFRONT__BUDGET_UI_DISTRIBUTION_ID | Github  | SECRET | Environment | Assistant: Budget - UI | CloudFront ID which should be used for assets distribution  |
| AWS_CLOUDFRONT_BUDGET_UI_DOMAIN           | Github  | SECRET | Environment | Assistant: Budget - UI | URL which should be used distribution                       |
| AWS_BE_API_DOMAIN                         | Github  | SECRET | Environment | Assistant: Budget - BE | Full domain name for Budget.API project                     |
| AWS_LAMBDA_NAME_BE_API                    | Github  | SECRET | Environment | Assistant: Budget - BE | Name of Lambda function which should run Budget.API project |

## Initial brand-new setup
As an initial setup for a fresh environment, before executing Terraform scripts need to perform a few manual actions:

1. Request new domain from Route53: [AWS Route53: Domains](https://us-east-1.console.aws.amazon.com/route53/domains/home)
2. Following instructions to setup GitHib actions and Terraform Cloud integrations [Automate Terraform with GitHub Actions](https://developer.hashicorp.com/terraform/tutorials/automation/github-actions)
3. Request ACM Certificate [AWS ACM](https://us-east-1.console.aws.amazon.com/acm/home?region=us-east-1#/certificates/request)
4. Register Auth0 account and configure ClientId and ClientSecret.

> NOTE: Project currently configured to be deployed on AWS `US-EAST-1` 

## Setup new env
1. Copy and modify [assistants-budget-ops/develop](./assistants-budget-ops/develop/) with new env name. like `production`;
2. Create a new Enviroment in GitHub;
3. Setup Environment GitHub Variables;
4. Run [Assistants. Budget - Ops](./.github/workflows/ops.yml) pipeline to setup env;
   1. Make sure to select appropriate Environment before execution of GitHub Action;
   
TODO:
- Describe: Create ECR (using terraform, comment everything else), builde and deploy BE with actions to ECR, run all other terraforms.

## Add new Parameter into AWS
Budget.API project utilize AWS Parameter Store for storing application configurations. All configurations of backend projects should be 2 level maximum.


- Describe: how to add new Parameter into AWS

# BudgetAssistantUI

# Backend configuration

# Auth0 Configuration
TODO: Describe usage of assistants-budget-auth0 for sync Auth0 configurations and store them in GitHub
TODO: possibility of setup GitHub actions for deploying Auth0
TODO: Describe how to add new permissions and roles using assistants-budget-auth0
TODO: describe that clientId and ClientSecret can be added into .env file, and during token generation in swagger can put any value which will be substituted with .env. Only for local dev.