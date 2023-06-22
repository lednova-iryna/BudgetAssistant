# Assistants: Budget

The Assistants: Budget is a project designed to help individuals and families track their incomes and expenses, calculate relevant statistics, and provide insightful infographic for effective financial management. With the goal of promoting better financial health and planning, this budget assistant offers a comprehensive set of features to simplify the process of tracking and analyzing finances.

- [Assistants: Budget](#assistants-budget)
- [Architecture](#architecture)
- [Git Flow](#git-flow)
- [Ops](#ops)
  - [GitHub Variables](#github-variables)
  - [Budget BE API Variables](#budget-be-api-variables)
    - [Setup AWS Parameter Store](#setup-aws-parameter-store)
    - [Setup locally](#setup-locally)
  - [Initial brand-new setup](#initial-brand-new-setup)
  - [Setup new env](#setup-new-env)
- [BudgetAssistantUI](#budgetassistantui)
- [Backend configuration](#backend-configuration)
- [Auth0 Configuration](#auth0-configuration)


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

## Budget BE API Variables
| Variable Name                          | Storage         | Type   | Level       | Project                | Description |
| -------------------------------------- | --------------- | ------ | ----------- | ---------------------- | ----------- |
| Auth__Authority                        | Parameter Store | SECRET | Environment | Assistant: Budget - BE |             |
| Auth__Audience                         | Parameter Store | SECRET | Environment | Assistant: Budget - BE |             |
| Auth__OAuth2TokenUrl                   | Parameter Store | SECRET | Environment | Assistant: Budget - BE |             |
| Auth__ClientId                         | Parameter Store | SECRET | Environment | Assistant: Budget - BE |             |
| Auth__ClientSecret                     | Parameter Store | SECRET | Environment | Assistant: Budget - BE |             |
| General__Environment                   | Parameter Store | SECRET | Environment | Assistant: Budget - BE |             |
| General__IsSwaggerEnabled              | Parameter Store | SECRET | Environment | Assistant: Budget - BE |             |
| Database__ConnectionString             | Parameter Store | SECRET | Environment | Assistant: Budget - BE |             |
| Database__Name                         | Parameter Store | SECRET | Environment | Assistant: Budget - BE |             |
| Auth__ManagementApiClientId            | Parameter Store | SECRET | Environment | Assistant: Budget - BE |             |
| Auth__ManagementApiAudience            | Parameter Store | SECRET | Environment | Assistant: Budget - BE |             |
| Auth__ManagementApiClientSecret        | Parameter Store | SECRET | Environment | Assistant: Budget - BE |             |
|                                        |                 |        | Environment | Assistant: Budget - BE |             |
| Aws__Parameters__Ignore                | Terraform. Env  | PARAM  | Environment | Assistant: Budget - BE |             |
| Aws__Parameters__Names                 | Terraform. Env  | PARAM  | Environment | Assistant: Budget - BE |             |
| Aws__Parameters__SecretPollingInterval | Terraform. Env  | PARAM  | Environment | Assistant: Budget - BE |             |
| AWS_PROFILE=lednova                    | .env            | PARAM  | Local       | Assistant: Budget - BE |             |

### Setup AWS Parameter Store
Let review how AWS Parameter Store item can be used in application, with help of [Assistants.Libs.Aws.Parameters](./assistants-budget-be/Assistants.Libs.Aws.Parameters/) library, on a example.
For instance, project requires to use `Auth__Authority`. Need to perform the following steps:
1. Define project name which should be deployed. Let's assume it is `assistants-budget-be-api`;
2. Define environment project should be deployed to. Let's assume it is `develop`;
3. In this case into project variables, usually thought the Environment variable, need to setup `Aws__Parameters__Names` with value `/develop/assistants-budget-be-api/`;
4. Create `/develop/assistants-budget-be-api/auth` Parameter in AWS Parameter Store with a following example of content:
   ``` json
   {
      "OAuth2TokenUrl": "oauth/token",
      "Audience": "{AUTH_AUDIENCE}",
      "Authority": "{AUTH_AUTHORITY}",
      "ManagementApiAudience": "AUTH0_MANAGEMENT_AUDIENCE",
      "ManagementApiClientId": "AUTH0_MANAGEMENT_CLIENT_ID",
      "ManagementApiClientSecret": "AUTH0_MANAGEMENT_CLIENT_SECRET"
   }
   ```
5. With help of [Assistants.Libs.Aws.Parameters](./assistants-budget-be/Assistants.Libs.Aws.Parameters/) library, such Parameter will be loaded into ServiceCollection as IOption and parsed into a following structure:
   ``` c#
      class AuthOptions : BaseOptions
      {
         public override string SectionName => "Auth";

         public string Audience { get; set; }
         public string Authority { get; set; }
         public string OAuth2TokenUrl { get; set; }
         public string ManagementApiAudience { get; set; }
         public string ManagementApiClientId { get; set; }
         public string ManagementApiClientSecret { get; set; }
      }
   ```

### Setup locally
For local setup all project variables can be setup into `.env` file. The following example can be used
``` env
Aws__Parameters__Ignore=true

DATABASE_USERNAME=username
DATABASE_PASSWORD=password

General__Environment=test

Database__ConnectionString=mongodb://username:password@localhost:27017
Database__Name=integration_tests

Auth__OAuth2TokenUrl=Auth__OAuth2TokenUrl
Auth__Authority=https://Auth__Domain
Auth__Audience=Auth__Audience
Auth__ClientId=Auth__ClientId
Auth__ClientSecret=Auth__ClientSecret
Auth__TestClientId=Auth__ClientId
Auth__TestClientSecret=Auth__ClientSecret
Auth__ManagementApiAudience=Auth__Audience
Auth__ManagementApiClientId=Auth__ClientId
Auth__ManagementApiClientSecret=Auth__ClientSecret
```

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

# BudgetAssistantUI

# Backend configuration

# Auth0 Configuration
TODO: Describe usage of assistants-budget-auth0 for sync Auth0 configurations and store them in GitHub
TODO: possibility of setup GitHub actions for deploying Auth0
TODO: Describe how to add new permissions and roles using assistants-budget-auth0
TODO: describe that clientId and ClientSecret can be added into .env file, and during token generation in swagger can put any value which will be substituted with .env. Only for local dev.
TODO: describe that all permissions should be created in Auth0 before their usage in API.