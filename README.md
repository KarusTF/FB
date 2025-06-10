FizzBuzz App
============

A full-stack FizzBuzz web application with a .NET backend and a Vite-powered JavaScript frontend. 
This application allows users to play default games and support customised game creation. 
Game rule: replace numbers with a specific word if divisible.
This project supports both local development and Docker deployment.
Unit tests are included for both the frontend and backend.

--------------------------
Docker Usage
--------------------------

1. Build Docker Images:
   ```bash
   docker-compose build

2. Run with Docker:
   ```bash
   docker-compose up

--------------------------
Local Development
--------------------------

1. Run Backend:
   ```bash
   dotnet build
   dotnet run

2. Run Frontend:
   ```bash
   cd fizzbuzz-frontend
   npm install
   npm run dev

--------------------------
Running Tests
--------------------------

1. Backend Unit Tests:
   ```bash
   cd UnitTests
   dotnet test

2. Frontend Unit Tests:
   ```bash
   cd UnitTests/FrontEnd
   npm install
   npm test
