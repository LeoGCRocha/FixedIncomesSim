FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/FixedIncome.API/FixedIncome.API.csproj", "src/FixedIncome.API/"]
COPY ["src/FixedIncome.Domain/FixedIncome.Domain.csproj", "src/FixedIncome.Domain/"]
COPY ["src/FixedIncome.Infrastructure/FixedIncome.Infrastructure.csproj", "src/FixedIncome.Infrastructure/"]
RUN dotnet restore "src/FixedIncome.API/FixedIncome.API.csproj"
COPY . .
WORKDIR "/src/src/FixedIncome.API"
RUN dotnet build "FixedIncome.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "FixedIncome.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FixedIncome.API.dll"]
