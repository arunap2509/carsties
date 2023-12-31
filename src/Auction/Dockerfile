FROM mcr.microsoft.com/dotnet/sdk:7.0 as build
WORKDIR /app
EXPOSE 80

# copy all .csproj files and restore as distinct layers.   Use of the same COPY command
# for every dockerfile in the project to take advantage of docker caching
COPY Carsties.sln Carsties.sln
COPY src/Auction/Auction.csproj src/Auction/Auction.csproj
COPY src/SearchService/SearchService.csproj src/SearchService/SearchService.csproj
COPY src/GatewayService/GatewayService.csproj src/GatewayService/GatewayService.csproj
COPY src/Contracts/Contracts.csproj src/Contracts/Contracts.csproj
COPY src/IdentityService/IdentityService.csproj src/IdentityService/IdentityService.csproj
COPY src/BiddingService/BiddingService.csproj src/BiddingService/BiddingService.csproj
COPY src/NotificationService/NotificationService.csproj src/NotificationService/NotificationService.csproj
COPY tests/Auction.UnitTests/Auction.UnitTests.csproj tests/Auction.UnitTests/Auction.UnitTests.csproj
COPY tests/Auction.IntegrationTests/Auction.IntegrationTests.csproj tests/Auction.IntegrationTests/Auction.IntegrationTests.csproj

# Restore package deps
RUN dotnet restore Carsties.sln

# Copy the app folders over
COPY src/Auction src/Auction
COPY src/Contracts src/Contracts
WORKDIR /app/src/Auction
RUN dotnet publish -c Release -o /app/src/out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app/src/out .
ENTRYPOINT [ "dotnet", "Auction.dll" ]