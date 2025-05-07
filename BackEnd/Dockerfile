# Use the official .NET 6 SDK image since we need the build tools and watch functionality
FROM mcr.microsoft.com/dotnet/sdk:8.0

# Set the working directory inside the container
WORKDIR /app

# Copy the project file(s) and restore dependencies first to leverage Docker cache
COPY *.csproj ./
RUN dotnet restore

# Copy all source files to the container
COPY . ./

# Set the environment to Development so that appsettings.Development.json is used
ENV ASPNETCORE_ENVIRONMENT=Development

# Expose port 5166 so you can access your API (Swagger, endpoints, etc.)
EXPOSE 5166

# In development mode, run with dotnet watch to enable hot reload and rebuild on file changes
CMD ["dotnet", "watch", "run", "--urls", "http://0.0.0.0:5166"]