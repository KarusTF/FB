FROM mcr.microsoft.com/dotnet/sdk:8.0 AS backend-build
WORKDIR /src

COPY ["FizzBuzz.sln", "."]
COPY ["FizzBuzz/FizzBuzz.csproj", "FizzBuzz/"]
COPY ["FizzBuzz/Migrations/", "FizzBuzz/Migrations/"]

RUN dotnet restore "FizzBuzz.sln"

COPY FizzBuzz/ FizzBuzz/


RUN rm -rf /src/FizzBuzz/bin /src/FizzBuzz/obj

RUN dotnet build "FizzBuzz/FizzBuzz.csproj" -c Release --no-incremental
RUN dotnet publish "FizzBuzz/FizzBuzz.csproj" -c Release -o /src/publish

RUN ls /src/publish

FROM node:18 AS frontend-build
WORKDIR /frontend

COPY fizzbuzz-frontend/package.json .
COPY fizzbuzz-frontend/package-lock.json .  

RUN npm install  

COPY fizzbuzz-frontend/. .

RUN npm run build

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=backend-build /src/publish .
COPY --from=frontend-build /frontend/dist /app/wwwroot

ENV ASPNETCORE_URLS=http://+:5000

ENTRYPOINT ["dotnet", "FizzBuzz.dll"]
