#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim-arm32v7 AS base
WORKDIR /app
EXPOSE 80
ENV AppSettings:GatewaySettings:EndPoint 10.0.1.9:7090
ENV AppSettings:GatewaySettings:Port 7090
ENV AppSettings:GatewaySettings:Timeout 1000
ENV AppSettings:PingOptions:Address 10.0.1.9
ENV TERM xterm

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["Wallbox/WallboxWeb/WallboxWeb.csproj", "Wallbox/WallboxWeb/"]
COPY ["Wallbox/WallboxLib/WallboxLib.csproj", "Wallbox/WallboxLib/"]
COPY ["Utilities/UtilityLib.Webapp/UtilityLib.Webapp.csproj", "Utilities/UtilityLib.Webapp/"]
COPY ["Utilities/UtilityLib/UtilityLib.csproj", "Utilities/UtilityLib/"]
RUN dotnet restore "Wallbox/WallboxWeb/WallboxWeb.csproj"
COPY . .
WORKDIR "/src/Wallbox/WallboxWeb"
RUN dotnet build "WallboxWeb.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WallboxWeb.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WallboxWeb.dll"]