﻿FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
RUN apt-get update && apt-get install -y curl jq

USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/FWDays.Gateway/FWDays.Gateway.csproj", "src/FWDays.Gateway/"]
RUN dotnet restore "src/FWDays.Gateway/FWDays.Gateway.csproj"
COPY . .
WORKDIR "/src/src/FWDays.Gateway"
RUN dotnet build "FWDays.Gateway.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "FWDays.Gateway.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FWDays.Gateway.dll"]
