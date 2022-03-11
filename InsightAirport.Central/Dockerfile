#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["../InsightAirport.Central/InsightAirport.Central.csproj", "../InsightAirport.Central/"]
RUN dotnet restore "../InsightAirport.Central/InsightAirport.Central.csproj"
COPY . .
WORKDIR "/src/../InsightAirport.Central"
RUN dotnet build "InsightAirport.Central.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "InsightAirport.Central.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "InsightAirport.Central.dll"]