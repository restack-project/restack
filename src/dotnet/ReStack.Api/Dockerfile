#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ReStack.Api/ReStack.Api.csproj", "ReStack.Api/"]
COPY ["ReStack.Application/ReStack.Application.csproj", "ReStack.Application/"]
COPY ["ReStack.Common/ReStack.Common.csproj", "ReStack.Common/"]
COPY ["ReStack.Domain/ReStack.Domain.csproj", "ReStack.Domain/"]
COPY ["ReStack.Persistence/ReStack.Persistence.csproj", "ReStack.Persistence/"]
RUN dotnet restore "ReStack.Api/ReStack.Api.csproj"
COPY . .
WORKDIR "/src/ReStack.Api"
RUN dotnet build "ReStack.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ReStack.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

RUN mkdir -p /restack/data

USER 0

RUN apt-get update && apt-get install -y openssh-server
RUN apt-get install -y git
RUN apt-get install -y curl
RUN apt-get install -y tar
RUN apt-get install -y iputils-ping
#RUN apt-get install -y python
#RUN apt-get install -y pip

RUN curl -sSL -o powershell.tar.gz https://github.com/PowerShell/PowerShell/releases/download/v7.4.2/powershell-7.4.2-linux-x64.tar.gz \
    && mkdir -p /usr/local/powershell \
    && tar zxf powershell.tar.gz -C /usr/local/powershell \
    && chmod +x /usr/local/powershell/pwsh \
    && ln -s /usr/local/powershell/pwsh /usr/bin/pwsh \
    && rm powershell.tar.gz

RUN curl -LO "https://dl.k8s.io/release/$(curl -L -s https://dl.k8s.io/release/stable.txt)/bin/linux/amd64/kubectl" \
    && chmod +x kubectl \
    && mv kubectl /usr/local/bin/

ENTRYPOINT ["dotnet", "ReStack.Api.dll"]
