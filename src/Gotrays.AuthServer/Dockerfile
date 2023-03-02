#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["NuGet.Config", "."]
COPY ["src/Gotrays.AuthServer/Gotrays.AuthServer.csproj", "src/Gotrays.AuthServer/"]
COPY ["src/Gotrays.EntityFrameworkCore/Gotrays.EntityFrameworkCore.csproj", "src/Gotrays.EntityFrameworkCore/"]
COPY ["src/Gotrays.Domain/Gotrays.Domain.csproj", "src/Gotrays.Domain/"]
COPY ["src/Gotrays.Domain.Shared/Gotrays.Domain.Shared.csproj", "src/Gotrays.Domain.Shared/"]
RUN dotnet restore "src/Gotrays.AuthServer/Gotrays.AuthServer.csproj"
COPY . .
WORKDIR "/src/src/Gotrays.AuthServer"
RUN dotnet build "Gotrays.AuthServer.csproj" --os linux -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Gotrays.AuthServer.csproj" --os linux -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Gotrays.AuthServer.dll"]