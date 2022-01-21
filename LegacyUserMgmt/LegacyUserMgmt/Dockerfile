FROM mcr.microsoft.com/dotnet/core/runtime:3.1 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["LegacyUserMgmt/LegacyUserMgmt.csproj", "LegacyUserMgmt/"]
RUN dotnet restore "LegacyUserMgmt/LegacyUserMgmt.csproj"
COPY . .
WORKDIR "/src/LegacyUserMgmt"
RUN dotnet build "LegacyUserMgmt.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LegacyUserMgmt.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LegacyUserMgmt.dll"]
