#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
ENV AppSettings:GatewaySettings:Address http://10.0.1.5
ENV AppSettings:GatewaySettings:SerialNumber 72130509
ENV AppSettings:GatewaySettings:Password Yellow31
ENV AppSettings:PingOptions:Address 10.0.1.5
ENV TERM xterm

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["EM300LR/EM300LRWeb/EM300LRWeb.csproj", "EM300LR/EM300LRWeb/"]
COPY ["Utilities/UtilityLib.Webapp/UtilityLib.Webapp.csproj", "Utilities/UtilityLib.Webapp/"]
COPY ["Utilities/UtilityLib/UtilityLib.csproj", "Utilities/UtilityLib/"]
COPY ["EM300LR/EM300LRLib/EM300LRLib.csproj", "EM300LR/EM300LRLib/"]
RUN dotnet restore "EM300LR/EM300LRWeb/EM300LRWeb.csproj"
COPY . .
WORKDIR "/src/EM300LR/EM300LRWeb"
RUN dotnet build "EM300LRWeb.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EM300LRWeb.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EM300LRWeb.dll"]