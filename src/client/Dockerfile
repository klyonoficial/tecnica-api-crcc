# Etapa de build
FROM node:20-alpine AS build
WORKDIR /app

# Copia package.json e package-lock.json para cache
COPY package*.json ./

# Instala dependências
RUN npm install

# Copia todo o código
COPY . .

# Build da aplicação para produção
RUN npm run build

# Etapa runtime (servidor leve)
FROM nginx:alpine

# Remove configuração default do nginx
RUN rm /etc/nginx/conf.d/default.conf

# Copia os arquivos de build do Vue.js para o Nginx
COPY --from=build /app/dist /usr/share/nginx/html

# Copia arquivo de configuração customizado para Nginx
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Expondo porta do container (Heroku usa $PORT)
EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
# Etapa de build
FROM node:20-alpine AS build
WORKDIR /app

# Copia package.json e package-lock.json para cache
COPY package*.json ./

# Instala dependências
RUN npm install

# Copia todo o código
COPY . .

# Build da aplicação para produção
RUN npm run build

# Etapa runtime (servidor leve)
FROM nginx:alpine

# Remove configuração default do nginx
RUN rm /etc/nginx/conf.d/default.conf

# Copia arquivos de build do Vue.js para Nginx
COPY --from=build /app/dist /usr/share/nginx/html

# Copia arquivo de configuração customizado do Nginx
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Expondo porta do container (Heroku redireciona via $PORT)
EXPOSE 80

# Comando para rodar Nginx
CMD ["nginx", "-g", "daemon off;"]
