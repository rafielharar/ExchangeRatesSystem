#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["ExchangeRatesReader/ExchangeRatesReader.csproj", "ExchangeRatesReader/"]
COPY ["Model/Model.csproj", "Model/"]
COPY ["LiveRatesAPI/LiveRatesAPI.csproj", "LiveRatesAPI/"]
COPY ["ExchangeRatesAPI/ExchangeRatesAPI.csproj", "ExchangeRatesAPI/"]
RUN dotnet restore "ExchangeRatesReader/ExchangeRatesReader.csproj"
COPY . .
WORKDIR "/src/ExchangeRatesReader"
RUN dotnet build "ExchangeRatesReader.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ExchangeRatesReader.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ExchangeRatesReader.dll"]
