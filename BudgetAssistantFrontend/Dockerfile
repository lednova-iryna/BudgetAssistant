FROM node:16.13.0-alpine3.12 as builder

WORKDIR /app

# copy the package.json to install dependencies
COPY . .
RUN npm i 
RUN npm run build

FROM nginx:alpine

COPY ./nginx.conf /etc/nginx/conf.d/default.conf

COPY --from=builder /app/build /usr/share/nginx/html
EXPOSE 80

ENTRYPOINT ["nginx", "-g", "daemon off;"]