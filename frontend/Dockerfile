FROM node:latest as builder
WORKDIR /app
COPY ./package.json ./
RUN npm i
COPY . .
RUN mv ./vite.config.docker.ts ./vite.config.ts
RUN npm run build

FROM nginx
EXPOSE 3000
COPY ./nginx/default.conf /etc/nginx/conf.d/default.conf
COPY --from=builder /app/dist /usr/share/nginx/html