FROM node:18.16.0-alpine3.17 as builder

WORKDIR /app

# copy the package.json to install dependencies
COPY . .
RUN yarn 
RUN yarn build

FROM nginx:alpine

COPY ./nginx.conf /etc/nginx/conf.d/default.conf

COPY --from=builder /app/build /usr/share/nginx/html
EXPOSE 80

ENTRYPOINT ["nginx", "-g", "daemon off;"]