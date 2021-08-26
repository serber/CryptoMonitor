#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["sources/app/CryptoMonitor.WebApp/CryptoMonitor.WebApp.csproj", "sources/app/CryptoMonitor.WebApp/"]
RUN dotnet restore "sources/app/CryptoMonitor.WebApp/CryptoMonitor.WebApp.csproj"
COPY . .
WORKDIR "/src/sources/app/CryptoMonitor.WebApp"
RUN dotnet build "CryptoMonitor.WebApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CryptoMonitor.WebApp.csproj" --no-restore -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CryptoMonitor.WebApp.dll"]