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


# Based on instaructions from https://docs.microsoft.com/en-us/dotnet/core/install/linux-scripted-manual#scripted-install
# install .Net automatically
ENV DOTNET_PATH=/usr/.dotnet
RUN mkdir ${DOTNET_PATH}
RUN curl -L https://dot.net/v1/dotnet-install.sh -o ${DOTNET_PATH}/dotnet-install.sh  && \
    chmod +x ${DOTNET_PATH}/dotnet-install.sh && \
    bash ${DOTNET_PATH}/dotnet-install.sh --channel 5.0 --install-dir ${DOTNET_PATH}/

ENV PATH="${DOTNET_PATH}/:${PATH}"

USER jenkins