# Event Driven Order Processing System
A production-grade event-driven backend system built with .NET 9, Clean Architecture, and Domain-Driven Design (DDD) principles.
This project demonstrates how to design and implement reliable, scalable, and decoupled systems using Domain Events, and asynchronous messaging.
The project is designed to be easily extended with the Outbox Pattern for reliable message delivery.

## Architectural Overview

This system is designed around event-driven principles, where business actions inside the domain emit domain events, which are then safely published to the outside world using the Outbox Pattern.

Key architectural goals:
- Loose coupling between bounded contexts
- Reliable event publishing (no lost events)
- Clear separation of concerns
- High testability and maintainability
- Real-world production readiness

## Features
- 📦 Order lifecycle management

- 🔔 Domain Events (OrderCreated, etc.)

- 🧩 Clean Architecture (API / Application / Domain / Infrastructure)

- 🧠 Domain-Driven Design (DDD) approach

- 🔄 Asynchronous event publishing

- 🛡️ Transactional consistency

- 🗄️ Entity Framework Core with MS SQL

- 🧪 Designed for extensibility (Payment, Shipping, Notification consumers)

## Tech Stack

**Backend**
- .NET 9 Web API
- Clean Architecture (API / Application / Domain / Infrastructure)
- Domain-Driven Design (DDD)

**Database**
- MS SQL
- Entity Framework Core

**Messaging & Integration**
- Domain Events
- Event Publisher abstraction (broker-ready)

**Persistence & Infrastructure**
- EF Core Migrations
- Fluent API configurations
  
**Authentication & Security**
- JWT-based authentication using access tokens
- Secure password hashing with industry-standard algorithms
- Role-based authorization for protected endpoints
- Centralized authentication and authorization handling via middleware
- Claims-based user identity management
- Secure API design following stateless authentication principles

**Validation & Middleware**
- Centralized request validation using DTO-based validation
- Global exception handling middleware for consistent error responses
- Validation pipeline preventing invalid data from reaching the domain layer
- Standardized API error response format
- Separation of concerns between validation, business logic, and infrastructure
---

## Design Decisions
- Clean Architecture principles were adopted to enforce separation of concerns between Domain, Application, Infrastructure, and API layers
- Business logic is isolated within the Domain and Application layers, keeping the system independent from frameworks and external services
- Domain Events are used to capture important business state changes without tightly coupling the domain to infrastructure concerns
- An event-driven approach was chosen to improve scalability and allow asynchronous processing of side effects
- RabbitMQ is used as the message broker to enable loose coupling between producers and consumers
- Entity Framework Core is used with explicit configuration to maintain control over persistence and avoid leaking infrastructure concerns into the domain
- Centralized error handling and validation were preferred over controller-level logic to keep controllers thin and maintainable
---

## 📁 Project Structure
```
EventDrivenOrderProcessing/
├── EventDrivenOrderProcessing.API/
│   ├── Controllers/
│   ├── Program.cs
│   └── ...
├── EventDrivenOrderProcessing.Application/
│   ├── Commands/
│   ├── Interfaces/
│   ├── Services/
│   └── ...
├── EventDrivenOrderProcessing.Domain/
│   ├── Orders/
│   │   ├── Order.cs
│   │   ├── Events/
│   │   └── ValueObjects/
│   ├── Common/
│   │   ├── BaseEntity.cs
│   │   └── DomainEvent.cs
│   └── ...
├── EventDrivenOrderProcessing.Infrastructure/
│   ├── Persistence/
│   │   ├── Context/
│   │   ├── Configurations/
│   │   └── Migrations/
│   ├── Messaging/
│   └── ...
└── README.md
```
---
## API Endpoints

### Authentication
 Method| Endpoint    |  Description                           |
|------|---------------|--------------------------------------|
| POST |/api/auth/login|Login and receive JWT token           |

### Orders
|Method|	Endpoint	                  |  Description                           |
|------|------------------------------|----------------------------------------|
| POST |/api/orders                   |Create a new order (Authenticated user) |
| GET  |/api/orders/{id}              |Get order details by id (Owner or Admin)|

### Orders (Admin(role-based))
|Method|	Endpoint	         |  Description                        |
|------|---------------------|-------------------------------------|
| GET	 |/api/orders          |	Get all orders(Admin only)         |

> Note: All endpoints requiring authentication must include JWT Bearer token in the Authorization header. Also scalar can help with the request bodies.

## ⚙️ Environment Variables
Copy appsettings.Example.json to appsettings.Development.json and fill your local secrets:

```
{
    "ConnectionStrings": {
        "Default": "Server=YOUR_SERVER;Database=YOUR_DATABASE;User Id=YOUR_USER;Password=YOUR_PASSWORD;TrustServerCertificate=True"
    },
    "Logging": {
        "LogLevel": {
            "Default": "Information",
            "Microsoft.AspNetCore": "Warning"
        }
    },
    "Jwt": {
        "Key": "YOUR_SECRET_KEY_HERE",
        "Issuer": "YOUR_API_NAME",
        "Audience": "YOUR_API_NAME"
    }
}

```
## 🐳 Docker & Docker Compose
This project supports running required infrastructure services using Docker Compose.
Sensitive configuration such as database credentials and RabbitMQ settings are externalized via environment variables.

### Environment Configuration
```
cp .env.example .env
```

##Running Infrastructure Services
**Start the required services (SQL Server, RabbitMQ) using Docker Compose:*
```
docker compose up -d
```
**This will start:**
- Microsoft SQL Server
- RabbitMQ (with Management UI)

**RabbitMQ Management UI:**
```
http://localhost:15672
```
**Default credentials:**
- Username: guest
- Password: guest

## 📌 Notes
- Docker is used only for infrastructure services, not for the API itself
- The API runs locally to simplify debugging and development


## 🚀 Getting Started
### Install dependencies
```
dotnet restore
```

## Apply database migrations
```
dotnet ef database update -s EventDrivenOrderProcessing.API -p EventDrivenOrderProcessing.Infrastructure
```
## Run the development server
```
dotnet run --project EventDrivenOrderProcessing.API
```
The server will start on https://localhost:5033

## License
This project is licensed under the MIT License.

## 👤 Author
**Oğuzhan Bilgin**
- [Github](https://github.com/oguzbilgin)
- [LinkedIn](https://www.linkedin.com/in/oguzhanbilgin/)
