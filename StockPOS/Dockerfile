#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine AS base
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build
WORKDIR /src
COPY . .
RUN dotnet restore

#ENV MysqlDbSettings:Password="${DB_PWD}"
RUN dotnet build -c Release -o build --no-restore

RUN dotnet publish -c Release -o publish --no-restore /p:UseAppHost=false /p:AssemblyName=DotNetApplication

FROM base AS final
WORKDIR /app
COPY --from=build /src/publish .
ENTRYPOINT ["dotnet", "DotNetApplication.dll"]
