# NetWebApiM
WebAPI project on .NET 7 using a MongoDB NoSQL database

## Installing MongoDB
**Code that fixed volume issue**: ```docker run -d --rm --name mongo -p 27017:27017  -v C:\ProgramData\docker\volumes\mongodbdata:C:\data\db mongo```

**Code provided by example**: 
```docker run -d --rm --name mongo -p 27017:27017  -v mongodbdata:data/db mongo```

-d --rm doesnt attach - and rm means its destoryed when you stop it. -p is opening a port to map to the container
-v is volume, which is a way to persist data. mongodbdata is the name of the volume, and /data/db is the path inside the container
docker ps -- checks if its running