# Use the official .NET 6.0 SDK image as a build stage
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

# Set the working directory in the container
WORKDIR /app

# Copy the .csproj files to the container
COPY ./BankApp.Api/*.csproj ./BankApp.Api/
COPY ./BankApp.Core/*.csproj ./BankApp.Core/
COPY ./BankApp.Infrastructure/*.csproj ./BankApp.Infrastructure/
COPY ./BankApp.Tests/*.csproj ./BankApp.Tests/
COPY ./BankApp.sln ./

# Restore dependencies using dotnet restore
RUN dotnet restore

# Copy the remaining source code to the container
COPY ./BankApp.Api ./BankApp.Api/
COPY ./BankApp.Core ./BankApp.Core/
COPY ./BankApp.Infrastructure ./BankApp.Infrastructure/
COPY ./BankApp.Tests ./BankApp.Tests/

# Build the application
RUN dotnet publish -c Release -o out

# Use the official ASP.NET runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime

# Set the working directory in the container
WORKDIR /app

# Copy the built application from the build stage
COPY --from=build /app/out ./

# Expose the port your application listens on
EXPOSE 80

# Specify the entry point for your application
ENTRYPOINT ["dotnet", "BankApp.Api.dll"]
