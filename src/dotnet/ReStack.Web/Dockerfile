#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ReStack.Web/ReStack.Web.csproj", "ReStack.Web/"]
RUN dotnet restore "ReStack.Web/ReStack.Web.csproj"
COPY . .
WORKDIR "/src/ReStack.Web"
RUN dotnet build "ReStack.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ReStack.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ReStack.Web.dll"]