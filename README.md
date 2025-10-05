# tecnica-api-crcc
API C# contendo todo o negócio do sistema: Controle de Recebimento de Crédito Confiança (CRCC).

# Logar no Heroku
heroku login
heroku container:login

# API - BACKEND
## Criar a imagem:
docker buildx build --platform linux/amd64 --provenance=false --load -t registry.heroku.com/tecnica-client-crcc/web .

## Enviar para heroku:
docker push registry.heroku.com/tecnica-api-crcc/web

## publicar:
heroku container:release web --app tecnica-api-crcc


# FRONTEND
## Criar a imagem front:
docker buildx build --platform linux/amd64 --provenance=false --load -t registry.heroku.com/tecnica-client-crcc/web .

## Enviar para heroku:
docker push registry.heroku.com/tecnica-client-crcc/web

## publicar:
heroku container:release web --app tecnica-client-crcc

# Rollback Deploy
## Listar Versões das Releases
heroku releases --app <app>

## Fazer o rollback
heroku rollback <versão> --app <app>
