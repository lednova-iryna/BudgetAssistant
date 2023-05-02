# Ops
Following instructions to setup GitHib actions and Terraform Cloud integrations
https://developer.hashicorp.com/terraform/tutorials/automation/github-actions

## Secrets

AWS_ACCESS_KEY_ID
AWS_SECRET_ACCESS_KEY
AWS_S3_BUDGET_UI_BUCKET
CLOUDFRONT_DISTRIBUTION_ID
AWS_ACM_CERTIFICATE_ARN


request ACM Certificate https://us-east-1.console.aws.amazon.com/acm/home?region=us-east-1#/certificates/request manually no terraform

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