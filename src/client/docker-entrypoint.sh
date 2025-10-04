#!/bin/sh

# Define porta padrão caso não exista
: "${PORT:=8080}"

# Substitui ${PORT} no template
envsubst '$PORT' < /etc/nginx/conf.d/default.conf.template > /etc/nginx/conf.d/default.conf

# Inicia Nginx
exec nginx -g 'daemon off;'