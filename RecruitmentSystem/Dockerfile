FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["RecruitmentSystem.API/RecruitmentSystem.API.csproj", "RecruitmentSystem.API/"]
COPY ["RecruitmentSystem.Business/RecruitmentSystem.Business.csproj", "RecruitmentSystem.Business/"]
COPY ["RecruitmentSystem.DataAccess/RecruitmentSystem.DataAccess.csproj", "RecruitmentSystem.DataAccess/"]
COPY ["RecruitmentSystem.Domain/RecruitmentSystem.Domain.csproj", "RecruitmentSystem.Domain/"]
RUN dotnet restore "RecruitmentSystem.API/RecruitmentSystem.API.csproj"
COPY . .
WORKDIR "/src/RecruitmentSystem.API"
RUN dotnet build "RecruitmentSystem.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "RecruitmentSystem.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RecruitmentSystem.API.dll"]
