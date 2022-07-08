#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

ENV ASPNETCORE_ENVIRONMENT=Development

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Black.AuthAPI/Black.AuthAPI.csproj", "Black.AuthAPI/"]
COPY ["Black.Infrastructure/Black.Infrastructure.csproj", "Black.Infrastructure/"]
COPY ["Black.Object/Black.Domain.csproj", "Black.Object/"]
COPY ["Black.Service/Black.Service.csproj", "Black.Service/"]
COPY ["Black.Model/Black.Model.csproj", "Black.Model/"]
COPY ["Black.Database/Black.Database.csproj", "Black.Database/"]
RUN dotnet restore "Black.AuthAPI/Black.AuthAPI.csproj"
COPY . .
WORKDIR "/src/Black.AuthAPI"
RUN dotnet build "Black.AuthAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Black.AuthAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Black.AuthAPI.dll"]