#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim-arm32v7 AS base
WORKDIR /app
EXPOSE 80
ENV AppSettings:GatewaySettings:Address http://10.0.1.6
ENV AppSettings:GatewaySettings:DeviceID 1
ENV AppSettings:PingOptions:Address 10.0.1.6
ENV TERM xterm

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["Fronius/FroniusWeb/FroniusWeb.csproj", "Fronius/FroniusWeb/"]
COPY ["Fronius/FroniusLib/FroniusLib.csproj", "Fronius/FroniusLib/"]
COPY ["Utilities/UtilityLib.Webapp/UtilityLib.Webapp.csproj", "Utilities/UtilityLib.Webapp/"]
COPY ["Utilities/UtilityLib/UtilityLib.csproj", "Utilities/UtilityLib/"]
RUN dotnet restore "Fronius/FroniusWeb/FroniusWeb.csproj"
COPY . .
WORKDIR "/src/Fronius/FroniusWeb"
RUN dotnet build "FroniusWeb.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FroniusWeb.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FroniusWeb.dll"]