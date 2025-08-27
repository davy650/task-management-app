# TaskManagement API

A **.NET 9 Web API** for managing tasks with support for migrations, JWT authentication, and clean architecture principles. This repository is a backend to this [React Frontend Application](https://github.com/davy650/task-management-app-frontend) which is also part of this project.

## Features
- .NET 9 Web API
- Entity Framework Core with PostgreSQL
- Code-first Migrations
- JWT Authentication 
- Dependency Injection
- Unit & Integration Tests (xUnit + Moq)
- Clean Architecture (Domain, Application, Infrastructure, API layers)

## Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/)
- [PostgreSQL](https://www.postgresql.org/) or you can use Postgres With Docker

## Getting Started

### 1. Clone the Repository
```bash
git clone https://github.com/davy650/task-management-app.git
cd TaskManagement
```

### 2. Configure Database Connection
Update your **appsettings.json** with your PostgreSQL connection string:

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=TaskDb;Username=postgres;Password=yourpassword"
}
```

### 3. Apply Migrations
Run the following commands from the root folder of the application

```bash
dotnet ef migrations add InitialCreate --project TaskManagement.Infrastructure --startup-project TaskManagement.Api
dotnet ef database update --project TaskManagement.Infrastructure --startup-project TaskManagement.Api
```

### 4. Run the API
```bash
dotnet run --project TaskManagement.Api
```

The API will be available at: **https://localhost:5001** or **http://localhost:xxxx**. Make the below call to create your first user

```
curl --location 'http://localhost:5218/api/Auth/register' \
--header 'Content-Type: application/json' \
--data-raw '{
  "username": "user",
  "email": "user@testemail.com",
  "password": "123456"
}'
```

This is the same user you will use to login into the front end app on the [React Frontend Application](https://github.com/davy650/task-management-app-frontend).


## JWT Authentication
Add the following section to **appsettings.json**:

```json
"JwtConfigs": {
    "Key": "your-jwt-key"
  },
```

## Running Tests
```bash
cd TaskManagement.Tests
dotnet test
```

## Project Structure
```
TaskManagement/
│── TaskManagement.Api          # API Layer
│── TaskManagement.Application  # Application Layer (DTOs, Services, Interfaces)
│── TaskManagement.Infrastructure # EF Core, Repositories, DB Context
|── TaskManagement.Domain          # business rules. Entities
│── TaskManagement.Tests        # Unit & Integration Tests
```

