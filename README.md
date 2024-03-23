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

#### Local Config Setup
1. Add a new configuration file named "appsettings.Local.json" in the root of the `MessagingApp.Api` project with the following content:
   ```json
   {
      "Logging": {
         "LogLevel": {
         "Default": "Information",
         "Microsoft.AspNetCore": "Warning"
         }
      },
      "ConnectionStrings": {
        "MessagingAppDb": "server=localhost;port=3306;user=root;password=password1;database=MessagingApp"
      },
      "Jwt": {
         "Issuer": "http://localhost",
         "Audience": "http://localhost",
         "AccessTokenLife": 60,
         "RefreshTokenLife": 1440,
         "RefreshTokenLength": 64,
         "Key": ""
      }
   }
   ```
   - Please note: token life is stored in minutes, RefreshTokenLength is bytes
2. Generate a token key with the command: `openssl rand -base64 172`
3. Copy the key into `Jwt:Key`

#### MySQL Setup

1. Clone the repository.
2. Navigate to the server directory: `cd MessagingApp/server`.
3. Start Docker containers: `docker compose up -d`.
4. In the `MessagingApp.Infrastructure` project, update databases with:
   - `dotnet ef database update --context ApplicationContext`
   - `dotnet ef database update --context AuthContext`

#### API Setup

1. Navigate to the server directory: `cd MessagingApp/server`.
2. Build the project: `dotnet build`.
3. Navigate to the API project: `cd MessagingApp.Api`.
4. Run the API: `dotnet run`.
