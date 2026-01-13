# ECommerceApp

A Clean Architecture modularized E-Commerce application built with ASP.NET Core 8.0.

## Project Structure

The solution is divided into four distinct projects following Clean Architecture principles:

- **ECommerceApp.Domain**
  - Contains the core entities, enums, and repository interfaces.
  - No dependencies on other projects.

- **ECommerceApp.Application**
  - Contains business logic, DTOs (Data Transfer Objects), and Service interfaces.
  - Depends on `ECommerceApp.Domain`.

- **ECommerceApp.Infrastructure**
  - Contains data access implementation (DbContext, Repository implementations) and Migrations.
  - Depends on `ECommerceApp.Domain` and `ECommerceApp.Application`.

- **ECommerceApp.Web**
  - The main entry point (ASP.NET Core MVC).
  - Contains Controllers, Views, and UI logic.
  - Depends on `ECommerceApp.Application` and `ECommerceApp.Infrastructure`.

## Prerequisites

- .NET 8.0 SDK
- SQL Server (LocalDB or full instance)

## How to Run

1. **Build the Solution**
   ```powershell
   dotnet build
   ```

2. **Run the Application**
   ```powershell
   dotnet run --project ECommerceApp.Web
   ```

3. **Run Tests**
   ```powershell
   dotnet test
   ```

## Database Migrations

Migrations are managed in the **Infrastructure** project but executed from the **Web** project (startup project).

- **Add a Migration:**
  ```powershell
  dotnet ef migrations add <MigrationName> --project ECommerceApp.Infrastructure --startup-project ECommerceApp.Web
  ```

- **Update Database:**
  ```powershell
  dotnet ef database update --project ECommerceApp.Infrastructure --startup-project ECommerceApp.Web
  ```
