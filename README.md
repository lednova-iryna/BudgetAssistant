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
openssl x509 -trustout -signkey budget-assistant-dev-ca.key -days 365 -req -in budget-assistant-dev-ca.csr -out budget-assistant-dev-ca.pem
```

https://github.com/DamionGans/ubuntu-wsl2-systemd-script