# Build Stage
FROM node:20-alpine AS build
WORKDIR /app

COPY package*.json ./
RUN npm install
COPY . .

# Build do Vite
RUN npm run build

# Runtime Stage
FROM nginx:alpine
WORKDIR /usr/share/nginx/html

# Copia build
COPY --from=build /app/dist .

# Copia template do Nginx
COPY nginx.conf.template /etc/nginx/conf.d/default.conf.template

# Cria config.js runtime com URL da API (do ENV do Heroku)
RUN echo "window.VITE_API_URL = '${VITE_API_URL:-https://tecnica-api-crcc-f7d9bbf52931.herokuapp.com/api}';" > /usr/share/nginx/html/config.js

EXPOSE 8080

CMD envsubst '$PORT' < /etc/nginx/conf.d/default.conf.template > /etc/nginx/conf.d/default.conf \
    && nginx -g 'daemon off;'
