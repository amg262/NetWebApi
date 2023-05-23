# NetWebApiM
WebAPI project on .NET 7 using a MongoDB NoSQL database

## Installing MongoDB

1. Install MongoDB from https://www.mongodb.com/try/download/community
2. Install MongoDB Compass from https://www.mongodb.com/try/download/compass
3. Run 
```dockerfile
docker run -d --rm --name mongo -p 27017:27017  -v C:\ProgramData\docker\volumes\mongodbdata:C:\data\db mongo
docker ps
docker stop mongo
```


### Code that fixed volume issue:
```dockerfile
docker run -d --rm --name mongo -p 27017:27017  -v C:\ProgramData\docker\volumes\mongodbdata:C:\data\db mongo
```

This worked for a separate D:\ location and on Windows Server 2019

### Code provided by example: 
```dockerfile
docker run -d --rm --name mongo -p 27017:27017  -v mongodbdata:data/db mongo
-d --rm doesnt attach - and rm means its destoryed when you stop it. 
-p is opening a port to map to the container
-v is volume, which is a way to persist data. mongodbdata is the name of the volume, and /data/db is the path inside the container
docker ps -- checks if its running
```



### New DB with auth
```dockerfile
docker stop mongo
docker volume ls
docker volume rm mongodbdata
```

This line works with a new volume for this Auth DB - while saving old one. Needed to move the 'mongo' to the very end of the line.
```dockerfile
docker run -d --rm --name mongo -p 27017:27017  -v C:\ProgramData\docker\volumes\mongodbdataauth:C:\data\db -e MONGO_INITDB_ROOT_USERNAME=mongoadmin -e MONGO_INITDB_ROOT_PASSWORD=password mongo
```

### Initialize secrets: 

You don't want to store the Password in appsettings.json - can use Command Line, .env, cloud to store the password in a secret store. We will use Secret Manager.

```
dotnet user-secrets init --project .\Catalog\
dotnet user-secrets set MongoDbSettings:Password password
```


### Aysnchronous Programming
https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/async/

Async all the way from controller to repository to database

### Health Checks
https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-5.0

We can use health checks to monitor the health of our application. We can use this to monitor the health of our database.

HealthChecks UI: https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks

### Dockerfile
```dockerfile
docker build -t catalog:v1 .
docker network create NetWebApi
docker network ls
```

Andrew-PC original docker desktop run command
```dockerfile
docker run --hostname=f09e4723dab5 --env=MONGO_INITDB_ROOT_USERNAME=mongoadmin --env=MONGO_INITDB_ROOT_PASSWORD=password --env=PATH=/usr/local/sbin:/usr/local/bin:/usr/sbin:/usr/bin:/sbin:/bin --env=GOSU_VERSION=1.16 --env=JSYA
ML_VERSION=3.13.1 --env=MONGO_PACKAGE=mongodb-org --env=MONGO_REPO=repo.mongodb.org --env=MONGO_MAJOR=6.0 --env=MONGO_VERSION=6.0.5 --env=HOME=/data/db --volume=/data/configdb --volume=/data/db -p 27017:27017 --label='org.opencontainers.image.ref.name=ubuntu'
--label='org.opencontainers.image.version=22.04' --runtime=runc -d mongo:latest
```

Andrew-PC new docker desktop run command to use container
```dockerfile
docker run --hostname=f09e4723dab5 --env=MONGO_INITDB_ROOT_USERNAME=mongoadmin --env=MONGO_INITDB_ROOT_PASSWORD=password --env=PATH=/usr/local/sbin:/usr/local/bin:/usr/sbin:/usr/bin:/sbin:/bin --env=GOSU_VERSION=1.16 --env=JSYA
ML_VERSION=3.13.1 --env=MONGO_PACKAGE=mongodb-org --env=MONGO_REPO=repo.mongodb.org --env=MONGO_MAJOR=6.0 --env=MONGO_VERSION=6.0.5 --env=HOME=/data/db --volume=/data/configdb --volume=/data/db -p 27017:27017 --label='org.opencontainers.image.ref.name=ubuntu'
--label='org.opencontainers.image.version=22.04' --runtime=runc -d --network=NetWebApi mongo:latest

```