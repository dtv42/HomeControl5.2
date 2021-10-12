#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["ETAPU11/ETAPU11Web/ETAPU11Web.csproj", "ETAPU11/ETAPU11Web/"]
COPY ["Utilities/UtilityLib.Webapp/UtilityLib.Webapp.csproj", "Utilities/UtilityLib.Webapp/"]
COPY ["Utilities/UtilityLib/UtilityLib.csproj", "Utilities/UtilityLib/"]
COPY ["ETAPU11/ETAPU11Lib/ETAPU11Lib.csproj", "ETAPU11/ETAPU11Lib/"]
COPY ["Modbus/ModbusLib/ModbusLib.csproj", "Modbus/ModbusLib/"]
RUN dotnet restore "ETAPU11/ETAPU11Web/ETAPU11Web.csproj"
COPY . .
WORKDIR "/src/ETAPU11/ETAPU11Web"
RUN dotnet build "ETAPU11Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ETAPU11Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ETAPU11Web.dll"]