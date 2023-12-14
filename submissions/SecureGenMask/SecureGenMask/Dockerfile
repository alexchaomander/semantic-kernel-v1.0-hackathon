FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["SecureGenMask/SecureGenMask.csproj", "SecureGenMask/"]
RUN dotnet restore "SecureGenMask/SecureGenMask.csproj"
COPY . .
WORKDIR "/src/SecureGenMask"
RUN dotnet build "SecureGenMask.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SecureGenMask.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SecureGenMask.dll"]
