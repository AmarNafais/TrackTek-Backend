# Step 1: Build the application using the .NET SDK image (updated to .NET 8.0)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory inside the container
WORKDIR /app

# Copy the entire Web, Service, and Data folders to the container
COPY Web ./Web/
COPY Service ./Service/
COPY Data ./Data/

# Restore dependencies for the Web project (and its dependencies)
WORKDIR /app/Web
RUN dotnet restore

# Step 2: Publish the application in Release mode
RUN dotnet publish Web.csproj -c Release -o /out

# Step 3: Use the .NET runtime image to run the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0

# Set the working directory for the runtime environment
WORKDIR /app

# Set the environment to Development
ENV ASPNETCORE_ENVIRONMENT=Development

# Copy the published application from the build image to the container
COPY --from=build /out .

# Expose the HTTP port (set to 8080 for easier access)
EXPOSE 8080

# Set the entry point for the container (run the Web application's DLL)
ENTRYPOINT ["dotnet", "Web.dll"]