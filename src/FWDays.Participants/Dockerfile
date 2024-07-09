FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
RUN apt-get update && apt-get install -y curl jq

USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/FWDays.Participants/FWDays.Participants.csproj", "src/FWDays.Participants/"]
RUN dotnet restore "src/FWDays.Participants/FWDays.Participants.csproj"
COPY . .
WORKDIR "/src/src/FWDays.Participants"
RUN dotnet build "FWDays.Participants.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "FWDays.Participants.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FWDays.Participants.dll"]
