FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /app
COPY /app /app
CMD ASPNETCORE_URLS=http://*:$PORT dotnet ModelsWebAPI.dll