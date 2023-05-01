# lednev/docker-node-awscli

Base image for [react-deploy-to-s3](../actions/react-deploy-to-s3)

## Build

```sh
docker build -t lednev/docker-node-awscli:18.16.0 --progress=plain --platform linux/amd64 .  
```

```sh
docker tag lednev/docker-node-awscli:18.16.0 lednev/docker-node-awscli:latest
```

## Deploy

```sh
docker push lednev/docker-node-awscli  
```