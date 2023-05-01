FROM node:18.16.0-alpine

LABEL version="18.0"
LABEL repository="https://github.com/lednova-irina/BudgetAssistant/docker-node-awscli"

# update 
RUN apk update
# install curl 
RUN apk add curl
# RUN apk add --update nodejs=${NODE_VERSION} npm
# confirm that it was successful 
RUN echo $(node -v)
# npm installs automatically 
RUN npm -v

RUN yarn --version

RUN apk --no-cache add \ 
    bash \
    curl \
    less \
    groff \
    jq \
    git \
    python3 \
    py3-pip \
    py-pip && \
    pip install --upgrade pip awscli s3cmd && \
    mkdir /root/.aws

RUN aws --version

COPY docker-entrypoint.sh /usr/local/bin/
RUN chmod +x /usr/local/bin/docker-entrypoint.sh
ENTRYPOINT ["docker-entrypoint.sh"]