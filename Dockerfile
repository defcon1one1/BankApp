# Use the official ASP.NET Core runtime as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:5.0

# Set the working directory to /app
WORKDIR /app

# Copy the current directory contents into the container at /app
COPY . .

# Expose port 80 to allow external access
EXPOSE 80

# Define environment variable
ENV ASPNETCORE_URLS=http://+:80

# Run the application
CMD ["dotnet", "BankApp.Api.dll"]
