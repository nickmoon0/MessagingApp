# Basic Messaging App

A simple client/server messaging app.

## About

This project is a web-based instant messaging application. It uses C#/ASP.NET Core for the backend and React for the frontend. Data persistence is managed with two MySQL databases.

### Backend

- **Architecture**: Clean Architecture principles were applied.
- **Patterns**: Utilizes CQRS and the mediator pattern
- **API**: RESTful API in ASP.NET Core 8.
- **Infrastructure**: Developed with MySQL and EF Core.

## Setting Up

### Requirements

- Docker
- .NET 8
- EF Core CLI

### Steps

#### MySQL Setup

1. Clone the repository.
2. Navigate to the server directory: `cd MessagingApp/server`.
3. Start Docker containers: `docker compose up -d`.
4. Update databases:
   - `dotnet ef database update --context ApplicationContext`
   - `dotnet ef database update --context AuthContext`

#### API Setup

1. Navigate to the server directory: `cd MessagingApp/server`.
2. Build the project: `dotnet build`.
3. Navigate to the API project: `cd MessagingApp.Api`.
4. Run the API: `dotnet run`.