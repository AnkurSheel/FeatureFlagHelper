FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build
WORKDIR /src
COPY ["FeatureFlagHelper/FeatureFlagHelper.csproj", "FeatureFlagHelper/"]
RUN dotnet restore "FeatureFlagHelper/FeatureFlagHelper.csproj"
COPY . .
WORKDIR "/src/FeatureFlagHelper"
RUN dotnet build "FeatureFlagHelper.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FeatureFlagHelper.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FeatureFlagHelper.dll"]
