# Architecture

![](./docs/architecture-general.drawio.svg)

# Ops

## Initial manual setup
As an initial setup for a fresh environment, before executing Terraform scripts need to perform a few manual actions:

1. Request new domain from Route53: [AWS Route53: Domains](https://us-east-1.console.aws.amazon.com/route53/domains/home)
2. Following instructions to setup GitHib actions and Terraform Cloud integrations [Automate Terraform with GitHub Actions](https://developer.hashicorp.com/terraform/tutorials/automation/github-actions)
3. Request ACM Certificate [AWS ACM](https://us-east-1.console.aws.amazon.com/acm/home?region=us-east-1#/certificates/request)

> NOTE: Project currently configured to be deployed on AWS `US-EAST-1` 

## GitHub Variables

| Variable Name                             | Type   | Level       | Project                | Description                                                |
| ----------------------------------------- | ------ | ----------- | ---------------------- | ---------------------------------------------------------- |
| AWS_S3_BUDGET_UI_BUCKET                   | SECRET | Environment | Assistant: Budget - UI | S3 Bucket name where build assets are stored               |
| AWS_CLOUDFRONT__BUDGET_UI_DISTRIBUTION_ID | SECRET | Environment | Assistant: Budget - UI | CloudFront ID which should be used for assets distribution |
| AWS_CLOUDFRONT_BUDGET_UI_DOMAIN           | SECRET | Environment | Assistant: Budget - UI | URL which should be used distribution                      |
| AWS_ACM_CERTIFICATE_ARN                   | SECRET | Global      | All                    | ARN of the ACM Certificate for HTTPS                       |
| AWS_ROUTE53_HOSTED_ZONE_ID                | SECRET | Global      | All                    | Route53 Zone ID where domain name is registered            |
| AWS_ACCESS_KEY_ID                         | SECRET | Global      | All                    | AWS Key ID for terraform user                              |
| AWS_SECRET_ACCESS_KEY                     | SECRET | Global      | All                    | AWS Key Secret for terraform user                          |
| TF_API_TOKEN                              | SECRET | Global      | All                    | Key of Terraform Cloud                                     |

## Setup new env
1. Copy and modify [assistants-budget-ops/develop](./assistants-budget-ops/develop/) with new env name. like `production`;
2. Create a new Enviroment in GitHub;
3. Setup Environment GitHub Variables;
4. Run [Assistants. Budget - Ops](./.github/workflows/ops.yml) pipeline to setup env;
   1. Make sure to select appropriate Environment before execution of GitHub Action;


TODO:
- Describe: Create ECR (using terraform, comment everything else), builde and deploy BE with actions to ECR, run all other terraforms.
- Describe: how to add new Parameter into AWS

# BudgetAssistantUI

# Backend configuration

### Jenkins

#### Plugins

- https://plugins.jenkins.io/docker-workflow/

### Create Keys for self-signed HTTPS

#### Dev

```bash
openssl dhparam -out dhparams.pem 4096
openssl req -new -newkey rsa:4096 -nodes -out lednova.net-ca.csr -keyout lednova.net-ca.key
openssl x509 -trustout -signkey assistants-budget-dev-ca.key -days 365 -req -in assistants-budget-dev-ca.csr -out assistants-budget-dev-ca.pem
```

https://github.com/DamionGans/ubuntu-wsl2-systemd-script