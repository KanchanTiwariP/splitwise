# SplitWise Backend Services

A Splitwise-style expense management backend built with **.NET 9, ASP.NET Core Web API, Entity Framework Core, and SQL Server**.

The application provides REST APIs for managing users, groups, expenses, expense splits, balances, and settlements.

## 🚀 Tech Stack

* .NET 9
* C#
* ASP.NET Core Web API
* Entity Framework Core
* SQL Server
* Docker
* JWT Authentication
* Swagger / OpenAPI
* Repository & Service Pattern
* Layered Architecture

## 🏗️ Architecture

The project follows a layered architecture:

```text
SplitWise
│
├── SplitWise.API
│   └── Controllers, Middleware, API Configuration
│
├── SplitWise.Application
│   └── Services, DTOs, Interfaces, Business Logic
│
├── SplitWise.Domain
│   └── Entities, Enums, Domain Models
│
└── SplitWise.Infrastructure
    └── EF Core, Repositories, Database Configurations
```

### Architecture Flow

```text
Client
   ↓
API Controllers
   ↓
Application Services
   ↓
Repositories
   ↓
Entity Framework Core
   ↓
SQL Server
```

## ✨ Features

### 🔐 Authentication

* User registration and login
* JWT-based authentication
* Protected API endpoints

### 👥 Group Management

* Create groups
* Add/remove group members
* Retrieve group details and members

### 💰 Expense Management

* Create expenses
* Support multiple participants
* Track who paid for an expense
* Expense split validation

### ➗ Expense Splitting

Supports three split types:

* **Equal Split**
* **Exact Amount Split**
* **Percentage Split**

The application validates split amounts and percentages and handles rounding safely to ensure the complete expense amount is accounted for.

### 📊 Group Balances

* Calculate how much each member owes
* Calculate how much each member should receive
* Generate net group balances

### 💸 Settlements

* Record settlements between users
* Support partial settlements
* Update outstanding balances after settlement

## 🗄️ Database

The application uses **Microsoft SQL Server** with **Entity Framework Core**.

### Key Entities

* Users
* Groups
* Group Members
* Expenses
* Expense Splits
* Settlements

Entity relationships and database configurations are implemented using **EF Core Fluent API**.

## 🐳 Running SQL Server with Docker

SQL Server can be run locally using Docker.

### Create SQL Server Container

```bash
docker run \
  -e "ACCEPT_EULA=Y" \
  -e "MSSQL_SA_PASSWORD=YourStrongPassword" \
  -p 1433:1433 \
  --name splitwise-sql \
  -d mcr.microsoft.com/mssql/server:2022-latest
```

Verify that the container is running:

```bash
docker ps
```

> Replace `YourStrongPassword` with your local SQL Server password. Do not commit real passwords or secrets to source control.

## ⚙️ Configuration

Update the connection string in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=SplitWiseDb;User Id=sa;Password=YourStrongPassword;TrustServerCertificate=True"
  }
}
```

For production environments, sensitive credentials should be stored using environment variables or a secure secret-management solution rather than committed to source control.

## 🗃️ Database Migrations

The project uses **Entity Framework Core Code-First migrations** to manage database schema changes.

Migrations are maintained in:

```text
SplitWise.Infrastructure/Migrations
```

### Apply Existing Migrations

After configuring SQL Server and the connection string, run:

```bash
dotnet ef database update \
  --project SplitWise.Infrastructure \
  --startup-project SplitWise.API
```

This applies the existing migrations and creates or updates the SplitWise database.

### Create a New Migration

When entities or database configurations are changed, create a new migration:

```bash
dotnet ef migrations add <MigrationName> \
  --project SplitWise.Infrastructure \
  --startup-project SplitWise.API
```

For example:

```bash
dotnet ef migrations add AddSettlement \
  --project SplitWise.Infrastructure \
  --startup-project SplitWise.API
```

Apply the migration:

```bash
dotnet ef database update \
  --project SplitWise.Infrastructure \
  --startup-project SplitWise.API
```

### Existing Migration

The initial database schema is created through:

```text
SplitWise.Infrastructure/Migrations/20260806114841_InitialCreate.cs
```

## ▶️ Run the Application

### 1. Clone the Repository

```bash
git clone https://github.com/KanchanTiwariP/splitwise.git
cd splitwise
```

### 2. Start SQL Server

Start the SQL Server Docker container:

```bash
docker start splitwise-sql
```

Or create a new container using the Docker command shown above.

### 3. Configure the Database

Update the SQL Server connection string in `appsettings.json`.

### 4. Restore Dependencies

```bash
dotnet restore
```

### 5. Apply Database Migrations

```bash
dotnet ef database update \
  --project SplitWise.Infrastructure \
  --startup-project SplitWise.API
```

### 6. Run the API

```bash
dotnet run --project SplitWise.API
```

## 📖 API Documentation

The API includes **Swagger / OpenAPI** documentation.

After starting the application, open the Swagger endpoint configured by the API project to explore and test the available REST APIs.

Swagger can be used to:

* Explore available endpoints
* View request and response models
* Test APIs
* Provide authentication credentials for protected endpoints

## 📁 Project Structure

```text
SplitWise
│
├── SplitWise.API
│   ├── Controllers
│   └── Program.cs
│
├── SplitWise.Application
│   ├── DTOs
│   ├── Interfaces
│   └── Services
│
├── SplitWise.Domain
│   ├── Entities
│   └── Enums
│
└── SplitWise.Infrastructure
    ├── Migrations
    ├── Persistence
    │   └── AppDbContext.cs
    ├── Repositories
    └── Configurations
```

## 🔮 Future Enhancements

* React frontend
* Direct expenses between two users
* Simplified debt settlement
* Recurring expenses
* Notifications
* Docker Compose for complete application setup
* Deployment to Azure

## 👩‍💻 Author

**Kanchan Pandey**

[GitHub](https://github.com/KanchanTiwariP/splitwise)
