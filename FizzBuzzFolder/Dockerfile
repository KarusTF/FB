# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["FizzBuzzFolder/FizzBuzz.sln", "."]
COPY ["FizzBuzzFolder/FizzBuzz/FizzBuzz.csproj", "FizzBuzz/"]

RUN dotnet restore "FizzBuzz.sln"

COPY FizzBuzzFolder/FizzBuzz/ FizzBuzz/

RUN rm -rf /src/FizzBuzz/bin /src/FizzBuzz/obj

RUN ls -R /src

RUN dotnet build "FizzBuzz/FizzBuzz.csproj" -c Release --no-incremental

RUN dotnet publish "FizzBuzz/FizzBuzz.csproj" -c Release --output /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "FizzBuzz.dll"]
