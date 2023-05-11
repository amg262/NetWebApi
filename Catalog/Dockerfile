FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Catalog/Catalog.csproj", "Catalog/"]
RUN dotnet restore "Catalog/Catalog.csproj"
COPY . .
WORKDIR "/src/Catalog"
RUN dotnet build "Catalog.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Catalog.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Catalog.dll"]


# Path: Basket\Dockerfile
# RUN docker run -d --rm --name mongo -p 27017:27017  -v C:\ProgramData\docker\volumes\mongodbdataauth:C:\data\db -e MONGO_INITDB_ROOT_USERNAME=mongoadmin -e MONGO_INITDB_ROOT_PASSWORD=password mongo