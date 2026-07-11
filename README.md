# 🚌 Bus Ticket Booking System

A full-stack bus reservation platform built with **ASP.NET Core Web API (.NET 8)** and an **ASP.NET Core MVC** frontend. The system supports end-to-end trip search, seat booking, payments, and multi-role management for Customers, Agencies, and Offices, backed by a clean layered architecture and a comprehensive automated test suite.

---

## 📖 Table of Contents

- [Overview](#-overview)
- [Features](#-features)
- [Tech Stack](#-tech-stack)
- [Architecture](#-architecture)
- [Project Structure](#-project-structure)
- [Getting Started](#-getting-started)
- [Environment Variables](#-environment-variables)
- [API Reference](#-api-reference)
- [Testing](#-testing)
- [Roles & Access Control](#-roles--access-control)
- [Contributing](#-contributing)
- [License](#-license)

---

## 📌 Overview

The Bus Ticket Booking System digitizes the complete lifecycle of intercity bus travel — from an **Agency** onboarding its **Offices**, buses, drivers, and routes, to a **Customer** searching trips, booking a seat, paying for it, and leaving a review.

The solution is split into three projects:

| Project | Description |
|---|---|
| `API_Bus_Ticket_Booking` | ASP.NET Core Web API — all business logic, data access, and REST endpoints |
| `Bus_Ticket_Booking.Mvc` | ASP.NET Core MVC client application consuming the API |
| `API_Bus_Ticket_Booking.Tests` | xUnit test suite covering every API controller |

---

## ✨ Features

- **Multi-role platform** — separate workflows and dashboards for **Customer**, **Agency**, and **Office** users
- **Trip search & discovery** — search by route, city, date, bus, or driver; view live seat availability
- **Seat booking & cancellation** — real-time seat booking with conflict handling (no double-booking)
- **Payments** — booking-linked payment creation, history, and revenue summaries/analytics
- **Reviews & ratings** — customers can review trips; trip-level review aggregation
- **Agency/Office management** — CRUD for offices, buses, drivers, and routes, with per-office dashboards and summaries
- **JWT authentication** — secure token-based login for all three roles, with BCrypt password hashing
- **Centralized error handling** — consistent API error responses via custom exception middleware
- **Interactive API docs** — Swagger/OpenAPI UI with built-in JWT bearer authorization
- **83 automated unit tests** across all 11 controllers

---

## 🛠 Tech Stack

**Backend**
- ASP.NET Core Web API — .NET 8
- Entity Framework Core 8 (SQL Server provider)
- JWT Bearer Authentication (`Microsoft.AspNetCore.Authentication.JwtBearer`)
- FluentValidation — request validation pipelines
- AutoMapper — entity ↔ DTO mapping
- BCrypt.Net — password hashing
- Swashbuckle (Swagger/OpenAPI) — API documentation
- DotNetEnv — environment variable management

**Frontend**
- ASP.NET Core MVC (.NET 8)
- Bootstrap, jQuery, jQuery Validation

**Testing**
- xUnit
- Moq
- FluentAssertions
- Microsoft.EntityFrameworkCore.InMemory

---

## 🏗 Architecture

The API follows a clean, layered architecture with clear separation of concerns:

```
Controller  →  Service  →  Repository  →  DbContext (EF Core)  →  SQL Server
     ↓             ↓             ↓
   DTOs      Business Logic   Data Access
```

- **Controllers** — thin, handle HTTP concerns and role-based `[Authorize]` checks
- **Services** (`IXxxService` / `XxxService`) — encapsulate business rules
- **Repositories** (`IXxxRepository` / `XxxRepository`) — encapsulate data access via EF Core
- **DTOs + AutoMapper Profiles** — decouple API contracts from database entities
- **FluentValidation Validators** — validate incoming DTOs before they reach business logic
- **Custom Exceptions** (`NotFoundException`, `ConflictException`, `ValidationException`, `UnauthorizedException`, `ForbiddenException`, `BadRequestException`) — mapped to appropriate HTTP status codes by a global `ExceptionMiddleware`

This structure keeps controllers testable in isolation (see [Testing](#-testing)) and makes it straightforward to swap or mock any layer.

---

## 📂 Project Structure

```
Bus_Ticket_Booking/
├── API_Bus_Ticket_Booking/           # Web API project
│   ├── Controllers/                  # 11 REST controllers
│   ├── Services/                     # Business logic + interfaces
│   ├── Repositories/                 # Data access + interfaces
│   ├── Models/                       # EF Core entities
│   ├── DTOs/                         # Request/response contracts
│   ├── Mappings/                     # AutoMapper profiles
│   ├── Validators/                   # FluentValidation rules
│   ├── Exceptions/                   # Custom exception types
│   ├── Middleware/                   # Global exception handling
│   ├── Helpers/JWT/                  # JWT token generation
│   ├── Data/                         # DbContext
│   └── Program.cs                    # App startup & DI configuration
│
├── Bus_Ticket_Booking.Mvc/            # MVC frontend
│   ├── Controllers/
│   ├── Views/
│   ├── ViewModels/
│   └── Services/                     # Typed HTTP clients calling the API
│
└── API_Bus_Ticket_Booking.Tests/      # Test project
    ├── Controllers/                  # One test class per controller
    ├── TestData/                     # Reusable test fixtures
    └── Helpers/
```

---

## 🚀 Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server (LocalDB, Express, or full instance)
- (Optional) Visual Studio 2022 / VS Code

### 1. Clone the repository

```bash
git clone https://github.com/BusBooking-Capgemini/Bus_Ticket_Booking_Repo.git
cd Bus_Ticket_Booking_Repo/Bus_Ticket_Booking
```

### 2. Configure environment variables

Create a `.env` file inside `API_Bus_Ticket_Booking/` (see [Environment Variables](#-environment-variables) below).

### 3. Apply database migrations

```bash
cd API_Bus_Ticket_Booking
dotnet ef database update
```

### 4. Run the API

```bash
dotnet run --project API_Bus_Ticket_Booking
```

The API will start (by default) on `https://localhost:{port}`, with Swagger UI available at `/swagger` in the Development environment.

### 5. Run the MVC frontend

```bash
dotnet run --project Bus_Ticket_Booking.Mvc
```

### 6. Run the tests

```bash
dotnet test API_Bus_Ticket_Booking.Tests
```

---

## 🔐 Environment Variables

The API loads configuration from a `.env` file at startup (via `DotNetEnv`). Create `API_Bus_Ticket_Booking/.env` with:

```env
DB_CONNECTION_STRING=Server=YOUR_SERVER;Database=BusTicketBookingDb;Trusted_Connection=True;TrustServerCertificate=True;
JWT_KEY=your-super-secret-signing-key
JWT_ISSUER=BusTicketBookingAPI
JWT_AUDIENCE=BusTicketBookingClient
JWT_DURATION_IN_MINUTES=60
```

> ⚠️ Never commit your real `.env` file. It's already excluded via `.gitignore`.

---

## 📡 API Reference

Base route: `/api`

| Controller | Base Route | Purpose |
|---|---|---|
| **Auth** | `/api/auth` | Customer signup, customer/agency/office login |
| **Customer** | `/api/customers` | Customer profile management, reviews |
| **Agency** | `/api/agencies` | Agency profile, offices, org-wide summaries |
| **Office** | `/api/offices` | Office CRUD, buses/drivers/trips/bookings/payments per office |
| **Bus** | `/api/buses` | Bus fleet CRUD, search by type/registration/capacity |
| **Driver** | `/api/drivers` | Driver CRUD, search by license/city |
| **Route** | `/api/routes` | Route CRUD, search by city/duration |
| **Trip** | `/api/trips` | Trip scheduling, seat availability, search by route/date/bus/driver |
| **Booking** | `/api/booking` | Create/cancel bookings, booking history, dashboards & analytics |
| **Payment** | `/api/payment` | Create payments, payment history, revenue summaries & analytics |
| **Dropdown** | `/api/dropdowns` | Lookup data (routes, buses, drivers, addresses) for UI forms |

All endpoints (except signup/login and public search endpoints) require a valid **JWT Bearer token**. Full interactive documentation — including request/response schemas — is available via Swagger UI once the API is running.

---

## ✅ Testing

The project ships with an isolated xUnit test suite:

- **83 unit tests** across all **11 controllers**
- **Moq** used to mock service-layer dependencies, keeping controller tests fast and isolated
- **FluentAssertions** for expressive, readable assertions
- **EF Core InMemory** provider for repository/data-layer test scenarios
- Covers both **positive** (happy-path) and **negative** (validation/error) test cases

Run the full suite with:

```bash
dotnet test API_Bus_Ticket_Booking.Tests
```

---

## 👥 Roles & Access Control

The system implements JWT-based role authorization with three primary roles:

| Role | Capabilities |
|---|---|
| **Customer** | Search trips, book/cancel seats, make payments, write/manage reviews |
| **Office** | Manage buses, drivers, and trips for their office; view office-level bookings, payments, and dashboards |
| **Agency** | Manage offices under the agency; view agency-wide summaries, bookings, payments, and analytics |

Authorization is enforced declaratively via `[Authorize(Roles = "...")]` attributes at the controller/action level, and tokens are issued and validated using signed JWTs (`JWT_KEY`, `JWT_ISSUER`, `JWT_AUDIENCE`).

---

## 🤝 Contributing

Contributions are welcome!

1. Fork the repository
2. Create a feature branch: `git checkout -b feature/your-feature`
3. Commit your changes: `git commit -m "Add your feature"`
4. Push to the branch: `git push origin feature/your-feature`
5. Open a Pull Request

Please ensure new features include corresponding unit tests in `API_Bus_Ticket_Booking.Tests`.

---

## 📄 License

This project is available for educational and portfolio purposes. Add your preferred license (e.g., MIT) here.

---

## 🙋 Author

**Nikhil Kumar**
📧 info.nikhil001@gmail.com | 🔗 [LinkedIn](https://linkedin.com/in/info-nikhil)  
**Arshdeep Singh**  
**Harsh Tanwar**  
**Mahesh**  
**Atul Attri**  
