#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim-arm32v7 AS base
WORKDIR /app
EXPOSE 80
ENV AppSettings:GatewaySettings:Address http://10.0.1.3
ENV AppSettings:GatewaySettings:Password !helios!
ENV AppSettings:PingOptions:Address 10.0.1.3
ENV TERM xterm

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["Helios/HeliosWeb/HeliosWeb.csproj", "Helios/HeliosWeb/"]
COPY ["Helios/HeliosLib/HeliosLib.csproj", "Helios/HeliosLib/"]
COPY ["Utilities/UtilityLib.Webapp/UtilityLib.Webapp.csproj", "Utilities/UtilityLib.Webapp/"]
COPY ["Utilities/UtilityLib/UtilityLib.csproj", "Utilities/UtilityLib/"]
RUN dotnet restore "Helios/HeliosWeb/HeliosWeb.csproj"
COPY . .
WORKDIR "/src/Helios/HeliosWeb"
RUN dotnet build "HeliosWeb.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HeliosWeb.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HeliosWeb.dll"]