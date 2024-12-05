#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER app
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /scr

COPY --from=mcr.microsoft.com/dotnet/sdk:9.0 /usr/share/dotnet/shared /usr/share/dotnet/shared

ARG BUILD_VERSION
ENV BUILD_VERSION=${BUILD_VERSION}

RUN --mount=type=cache,target=/var/cache/apt \
    apt-get update && \
    apt-get install -y --quiet --no-install-recommends \
    apt-transport-https && \
    apt-get -y autoremove && \
    apt-get clean autoclean

RUN wget https://download.oracle.com/java/21/latest/jdk-21_linux-x64_bin.tar.gz -O jdk-21_linux-x64_bin.tar.gz
RUN mkdir /usr/lib/jvm && \
    tar -xvf jdk-21_linux-x64_bin.tar.gz -C /usr/lib/jvm

RUN --mount=type=cache,target=/var/cache/apt \
    apt-get update && \   
    apt-get install -f -y --quiet --no-install-recommends \
    ant ca-certificates-java && \
    apt-get -y autoremove && \
    apt-get clean autoclean

# Fix certificate issues
RUN update-ca-certificates -f

ENV JAVA_HOME /usr/lib/jvm/jdk-21.0.1
RUN export JAVA_HOME=/usr/lib/jvm/jdk-21.0.1
RUN export PATH=$JAVA_HOME/bin:$PATH

RUN dotnet new tool-manifest

# Not supported for .net 9.0 release, will be fixed, as soon as, the dedicated updated will be implemented
# RUN dotnet tool install snitch --tool-path /tools --version 2.0.0

RUN dotnet tool restore

RUN echo "##vso[task.prependpath]$HOME/.dotnet/tools"
RUN export PATH="$PATH:/root/.dotnet/tools"

COPY ["HomeBudget.Components.Users/*.csproj", "HomeBudget.Components.Users/"]
COPY ["HomeBudget.Core/*.csproj", "HomeBudget.Core/"]
COPY ["HomeBudget.Identity.Domain/*.csproj", "HomeBudget.Identity.Domain/"]
COPY ["HomeBudget.Identity.Infrastructure/*.csproj", "HomeBudget.Identity.Infrastructure/"]

COPY ["HomeBudgetIdentityApi.sln", "HomeBudgetIdentityApi.sln"]

COPY ["startsonar.sh", "startsonar.sh"]

COPY . .

RUN dotnet build HomeBudgetIdentityApi.sln -c Release --no-incremental --framework:net9.0 -maxcpucount:1 -o /app/build

# Not supported for .net 9.0 release, will be fixed, as soon as, the dedicated updated will be implemented
# RUN /tools/snitch

FROM build AS publish
RUN dotnet publish HomeBudgetIdentityApi.sln \
    --no-dependencies \
    --no-restore \
    --framework net9.0 \
    -c Release \
    -v Diagnostic \
    -o /app/publish

FROM base AS final
WORKDIR /app
LABEL build_version="${BUILD_VERSION}"
LABEL service=IdentityService
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "HomeBudget.Identity.Api.dll"]