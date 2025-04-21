# Use the official Microsoft .NET SDK image to compile the projects
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /src

# Copy csproj files individually and restore to cache the layers
COPY HealthyU.DAL/HealthyU.DAL.csproj HealthyU.DAL/
COPY HealthuU.BLL/HealthuU.BLL.csproj HealthuU.BLL/
COPY HealthyU/HealthyU.WebApi.csproj HealthyU/
RUN dotnet restore HealthyU/HealthyU.WebApi.csproj

# Copy the rest of the source files
COPY HealthyU.DAL/ HealthyU.DAL/
COPY HealthuU.BLL/ HealthuU.BLL/
COPY HealthyU/ HealthyU/

# Publish the web API
RUN dotnet publish HealthyU/HealthyU.WebApi.csproj -c Release -o /app

# Use the ASP.NET runtime image to run the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app .
ENTRYPOINT ["dotnet", "HealthyU.WebApi.dll"]
