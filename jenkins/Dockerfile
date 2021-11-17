FROM jenkins/jenkins:lts
ENV DOCKER_VERSION=20.10.9
ENV DOCKER_COMPOSE_VERSION=2.1.1

USER root

RUN curl -sfL "https://download.docker.com/linux/static/stable/x86_64/docker-${DOCKER_VERSION}.tgz" -o /usr/local/bin/docker.tgz 
RUN tar -xzf /usr/local/bin/docker.tgz -C /usr/local/bin 
RUN rm /usr/local/bin/docker.tgz

RUN curl -L "https://github.com/docker/compose/releases/download/v${DOCKER_COMPOSE_VERSION}/docker-compose-linux-x86_64" -o /usr/local/bin/docker-compose && \
    chmod +x /usr/local/bin/docker-compose && \
    ln -s /usr/local/bin/docker-compose /usr/bin/docker-compose


ENV PATH="/usr/local/bin/docker:${PATH}"
