#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM registry-dev.truesight.asia/truesight/aspnet:3.1-buster-slim AS base
WORKDIR /app


FROM registry-dev.truesight.asia/truesight/dotnet-sdk:3.1-buster AS build
WORKDIR /src
COPY ["DMS.csproj", "./"]
RUN dotnet restore "DMS.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "DMS.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DMS.csproj" -c Release -o /app/publish

FROM base AS final
RUN apt-get update && apt-get install -y net-tools curl iputils-ping telnetd nano vim libc6-dev libgdiplus htop redis-tools
WORKDIR /app

COPY --from=publish /app/publish .

COPY ["docker-entrypoint.sh", "."]
RUN chmod a+x docker-entrypoint.sh
CMD ["./docker-entrypoint.sh"]

EXPOSE 80
