# Base - This means itll be the first stage of the build 
# Everything that happens will be in the /app directory
# Expose ports 80 and 443
# This is the base image for the build
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Build - This is the second stage of the build
# Uses SDK which has all the built tools, libraries and dependencies
# Copy the csproj file and restore the dependencies
# Copy the rest of the files and build the project
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Catalog/Catalog.csproj", "Catalog/"]
RUN dotnet restore "Catalog/Catalog.csproj"
COPY . .
WORKDIR "/src/Catalog"
RUN dotnet build "Catalog.csproj" -c Release -o /app/build

# From build to publish - Creates new folder called publish that it needs to execute the app
FROM build AS publish
RUN dotnet publish "Catalog.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Catalog.dll"]


# Path: Basket\DockerfileAn
# RUN docker run -d --rm --name mongo -p 27017:27017  -v C:\ProgramData\docker\volumes\mongodbdataauth:C:\data\db -e MONGO_INITDB_ROOT_USERNAME=mongoadmin -e MONGO_INITDB_ROOT_PASSWORD=password mongo