#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "WorkerHost/WorkerHost.csproj"
RUN dotnet build "WorkerHost/WorkerHost.csproj" --no-restore -c Release

FROM build AS publish
RUN dotnet publish "WorkerHost/WorkerHost.csproj" --no-restore --no-build -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WorkerHost.dll"]