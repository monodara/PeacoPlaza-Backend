# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS first-stage
WORKDIR /
# copy csproj and restore as distinct layers
COPY *.sln .
COPY Server.Infrastructure/Server.Infrastructure.csproj Server.Infrastructure/
COPY Server.Service/Server.Service.csproj Server.Service/
COPY Server.Core/Server.Core.csproj Server.Core/
COPY Server.Controller/Server.Controller.csproj Server.Controller/
COPY Server.Test/Server.Test.csproj Server.Test/
RUN dotnet restore
# copy everything else and build app
COPY Server.Infrastructure/ Server.Infrastructure/
COPY Server.Service/ Server.Service/
COPY Server.Core/ Server.Core/
COPY Server.Controller/ Server.Controller/
COPY Server.Test/ Server.Test/
WORKDIR /Server.Infrastructure
RUN dotnet publish -c release -o /app
#### First stage done. publish folder is built
# second stage/image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=first-stage /app ./
EXPOSE 8080
CMD ["dotnet", "Server.Infrastructure.dll"]