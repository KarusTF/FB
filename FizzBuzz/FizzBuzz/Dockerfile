# Use the base image for .NET
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory inside the container
WORKDIR /src

# Copy project files
COPY ["FizzBuzz.csproj", "."]
COPY . .

# Restore dependencies
RUN dotnet restore "FizzBuzz.csproj"

# Build the application
RUN dotnet build "FizzBuzz.csproj" -c Release -o /app/build

# Publish the application
RUN dotnet publish "FizzBuzz.csproj" -c Release -o /app/publish

# Use the runtime image for the final stage
FROM mcr.microsoft.com/dotnet/runtime:6.0 AS runtime

# Copy the published application
WORKDIR /app
COPY --from=build /app/publish .

# Entry point for the application
ENTRYPOINT ["dotnet", "FizzBuzz.dll"]
