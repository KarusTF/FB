FROM mcr.microsoft.com/dotnet/sdk:8.0 AS backend-build
WORKDIR /src

COPY ["FizzBuzz-BE.sln", "."]
COPY ["FizzBuzz-BE/FizzBuzz-BE.csproj", "FizzBuzz-BE/"]
COPY ["FizzBuzz-BE/Migrations/", "FizzBuzz-BE/Migrations/"]

RUN dotnet restore "FizzBuzz-BE.sln"

COPY FizzBuzz-BE/ FizzBuzz-BE/


RUN rm -rf /src/FizzBuzz-BE/bin /src/FizzBuzz-BE/obj

RUN dotnet build "FizzBuzz-BE/FizzBuzz-BE.csproj" -c Release --no-incremental
RUN dotnet publish "FizzBuzz-BE/FizzBuzz-BE.csproj" -c Release -o /src/publish

RUN ls /src/publish

FROM node:18 AS frontend-build
WORKDIR /frontend

COPY FizzBuzz-Client/package.json .
COPY FizzBuzz-Client/package-lock.json .  

RUN npm install  

COPY FizzBuzz-Client/. .

RUN npm run build

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=backend-build /src/publish .
COPY --from=frontend-build /frontend/dist /app/wwwroot

ENV ASPNETCORE_URLS=http://+:5000

ENTRYPOINT ["dotnet", "FizzBuzz-BE.dll"]
