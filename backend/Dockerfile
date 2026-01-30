FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy csproj files and restore dependencies
COPY ShopSphere.API/ShopSphere.API.csproj ShopSphere.API/
COPY ShopSphere.Core/ShopSphere.Core.csproj ShopSphere.Core/
COPY ShopSphere.Infrastructure/ShopSphere.Infrastructure.csproj ShopSphere.Infrastructure/
RUN dotnet restore ShopSphere.API/ShopSphere.API.csproj

# Copy everything else and build
COPY . .
RUN dotnet publish ShopSphere.API/ShopSphere.API.csproj -c Release -o /app/publish

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 5000
ENV ASPNETCORE_URLS=http://+:5000

ENTRYPOINT ["dotnet", "ShopSphere.API.dll"]
