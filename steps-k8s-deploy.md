# API

## Inicializando o cluster
```
minikube start
```

## Build da Imagem Docker
```
docker build -f src/FixedIncome.API/Dockerfile -t leogcrocha/fixedincome.api:0.1 .
```

## Push da Imagem no Docker Hub
```
docker push leogcrocha/fixedincome.api:0.1
```

## Criação do Namespace
```
kubectl apply -f k8s/namespace.yaml
```

## Criação do Deployment
```
kubectl apply -f k8s/deployment.yaml
```

## Criação do Service
```
kubectl apply -f k8s/service.yaml
```

# Banco de Dados
## Variaveis de Ambiente
```
kubectl apply -f k8s/postgres/configmap.yaml
```

## Persistent Volumes
```
kubectl apply -f k8s/postgresql/persintentvolumes.yaml
```

## Deploy do Postgres DB
```
kubectl apply -f k8s/postgres/deployment.yaml
```
