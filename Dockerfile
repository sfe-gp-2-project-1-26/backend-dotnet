# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY ["ElectronicsStore_API/ElectronicsStore_API.csproj", "ElectronicsStore_API/"]
RUN dotnet restore "ElectronicsStore_API/ElectronicsStore_API.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/ElectronicsStore_API"
RUN dotnet build "ElectronicsStore_API.csproj" -c Release -o /app/build

# Publish Stage
FROM build AS publish
RUN dotnet publish "ElectronicsStore_API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Expose port 80
EXPOSE 80

# Note: ASPNETCORE_URLS is used to tell Kestrel to listen on port 80
ENV ASPNETCORE_URLS=http://+:80

ENTRYPOINT ["dotnet", "ElectronicsStore_API.dll"]
