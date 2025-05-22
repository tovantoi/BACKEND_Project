#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/solution_items/chuyennganh.Api/chuyennganh.Api.csproj", "src/solution_items/chuyennganh.Api/"]
COPY ["src/solution_items/chuyennganh.Infrasture/chuyennganh.Infrasture.csproj", "src/solution_items/chuyennganh.Infrasture/"]
COPY ["src/solution_items/chuyennganh.Application/chuyennganh.Application.csproj", "src/solution_items/chuyennganh.Application/"]
COPY ["src/solution_items/chuyennganh.Domain/chuyennganh.Domain.csproj", "src/solution_items/chuyennganh.Domain/"]
RUN dotnet restore "./src/solution_items/chuyennganh.Api/chuyennganh.Api.csproj"
COPY . .
WORKDIR "/src/src/solution_items/chuyennganh.Api"
RUN dotnet build "./chuyennganh.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./chuyennganh.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "chuyennganh.Api.dll"]