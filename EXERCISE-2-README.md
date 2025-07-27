# Exercise 2 - Supplier Management API

## 🎯 Overview

This is a comprehensive **Exercise 2** implementation featuring a production-ready ASP.NET Core Web API with industry-standard practices. The solution provides two main APIs for supplier management with robust authentication, caching, and overlap detection capabilities.

## 🚀 Exercise 2 Requirements Implementation

### **API 1: All Suppliers with Rates**
- **Endpoint**: `GET /api/v1/suppliers`
- **Purpose**: Retrieve all suppliers with their associated rates
- **Authentication**: JWT Bearer token required
- **Response**: JSON array of suppliers with nested rate information

### **API 2: Overlapping Rate Detection**
- **Endpoint**: `GET /api/v1/suppliers/overlaps?supplierId={optional}`
- **Purpose**: Detect and return suppliers with overlapping rate periods
- **Features**: 
  - Optional supplier filtering by ID
  - High-performance SQL-based overlap detection
  - Detailed overlap information (dates, duration)

### **Authentication System**
- **Endpoint**: `POST /api/v1/auth/login` or `POST /api/v1/auth/token`
- **Purpose**: Generate JWT tokens for API access
- **Demo Credentials**: Available via `GET /api/v1/auth/demo-credentials` (development only)

## 🏗️ Architecture & Industry Standards

### **Clean Architecture Implementation**
```
Controllers (API Layer)
    ↓
Services (Business Logic)
    ↓
Repositories (Data Access)
    ↓
Database (Entity Framework Core)
```

### **Design Patterns Applied**
- ✅ **Repository Pattern**: Data access abstraction
- ✅ **Service Layer Pattern**: Business logic encapsulation
- ✅ **DTO Pattern**: API data transfer objects
- ✅ **Dependency Injection**: IoC container for loose coupling
- ✅ **Builder Pattern**: Configuration and API setup

### **SOLID Principles**
- **Single Responsibility**: Each class has one clear purpose
- **Open/Closed**: Extensible through interfaces
- **Liskov Substitution**: Interface-based contracts
- **Interface Segregation**: Focused, specific interfaces
- **Dependency Inversion**: Depend on abstractions, not concretions

## 🔐 Security Features

### **Authentication & Authorization**
- JWT Bearer token authentication
- Configurable token expiration (24 hours)
- Secure token generation with HMAC-SHA256
- Role-based access control ready

### **API Security**
- Rate limiting (100 requests/minute for APIs, 5 requests/5min for auth)
- Input validation with data annotations
- SQL injection prevention through parameterized queries
- XSS protection headers
- CSRF protection for web endpoints

### **Production Security Headers**
- Content Security Policy (CSP)
- X-Content-Type-Options: nosniff
- X-Frame-Options: DENY
- X-XSS-Protection: enabled
- HSTS for HTTPS enforcement (production)

## ⚡ Performance Optimizations

### **Database Performance**
- Strategic indexes on frequently queried columns
- AsNoTracking for read-only operations
- Connection pooling and retry policies
- Optimized Entity Framework queries

### **Caching Strategy**
- Response caching (5-minute duration)
- Query-specific cache variations
- Memory caching for frequently accessed data
- CDN-ready cache headers

### **High-Performance Overlap Detection**
```sql
-- Raw SQL query for maximum performance
WITH RateOverlaps AS (
    SELECT sr1.*, sr2.*, 
           -- Complex overlap calculation logic
    FROM SupplierRates sr1
    INNER JOIN SupplierRates sr2 ON sr1.SupplierId = sr2.SupplierId
    WHERE -- Overlap conditions
)
SELECT * FROM RateOverlaps WHERE OverlapStartDate <= OverlapEndDate
```

## 📊 API Documentation

### **OpenAPI/Swagger Integration**
- Complete API documentation available at `/swagger`
- Interactive API testing interface
- Request/response examples
- Authentication flow documentation

### **API Response Examples**

#### Get All Suppliers with Rates
```json
[
  {
    "supplierId": 1,
    "name": "BestValue",
    "address": "1, Main Street, The District, City1, XXX-AADA",
    "createdOn": "2021-07-30T00:00:00Z",
    "createdByUser": "admin.user",
    "rates": [
      {
        "supplierRateId": 1,
        "supplierId": 1,
        "rate": 10.00,
        "rateStartDate": "2015-01-01T00:00:00",
        "rateEndDate": "2015-03-31T00:00:00",
        "createdOn": "2021-07-30T00:00:00Z",
        "createdByUser": "admin.user"
      }
    ]
  }
]
```

#### Get Overlapping Rates
```json
[
  {
    "supplierId": 1,
    "supplierName": "BestValue",
    "overlappingRates": [
      {
        "rate1": {
          "supplierRateId": 1,
          "rate": 10.00,
          "rateStartDate": "2015-01-01T00:00:00",
          "rateEndDate": "2015-03-31T00:00:00"
        },
        "rate2": {
          "supplierRateId": 8,
          "rate": 15.00,
          "rateStartDate": "2015-02-15T00:00:00",
          "rateEndDate": "2015-04-15T00:00:00"
        },
        "overlapStartDate": "2015-02-15T00:00:00",
        "overlapEndDate": "2015-03-31T00:00:00",
        "overlapDays": 45
      }
    ]
  }
]
```

## 🛠️ Technology Stack

### **Core Technologies**
- **.NET 9.0**: Latest LTS framework
- **ASP.NET Core**: Web API framework
- **Entity Framework Core 9.0**: ORM for data access
- **SQL Server**: Primary database
- **AutoMapper**: Object mapping
- **JWT Bearer**: Authentication tokens

### **Development Tools**
- **Swagger/OpenAPI**: API documentation
- **Entity Framework Migrations**: Database versioning
- **Structured Logging**: Console, Debug, EventLog
- **Rate Limiting**: API protection
- **Response Compression**: Performance optimization

## 🚀 Getting Started

### **Prerequisites**
- .NET 9.0 SDK
- SQL Server (LocalDB supported)
- Visual Studio 2022 or VS Code

### **Installation Steps**

1. **Clone the Repository**
   ```bash
   git clone <repository-url>
   cd supplier-management
   ```

2. **Restore Dependencies**
   ```bash
   dotnet restore
   ```

3. **Update Database**
   ```bash
   dotnet ef database update
   ```

4. **Run the Application**
   ```bash
   dotnet run --project SupplierManagement
   ```

5. **Access Swagger Documentation**
   - Navigate to: `https://localhost:5114/swagger`

### **Quick API Testing**

1. **Get Demo Credentials** (Development only)
   ```
   GET /api/v1/auth/demo-credentials
   ```

2. **Authenticate**
   ```
   POST /api/v1/auth/login
   Content-Type: application/json
   
   {
     "username": "demo.user",
     "password": "Demo123!"
   }
   ```

3. **Test Exercise 2 APIs**
   ```
   GET /api/v1/suppliers
   Authorization: Bearer <your-jwt-token>
   
   GET /api/v1/suppliers/overlaps
   Authorization: Bearer <your-jwt-token>
   ```

## 📁 Project Structure

```
SupplierManagement/
├── Controllers/
│   ├── AuthController.cs          # JWT Authentication
│   ├── SupplierApiController.cs   # Exercise 2 APIs
│   └── SupplierController.cs      # Web MVC Controllers
├── Services/
│   ├── Interfaces/                # Service contracts
│   └── Implementations/           # Business logic
├── Repositories/
│   ├── Interfaces/                # Data access contracts
│   └── Implementations/           # EF Core implementations
├── Models/
│   ├── Domain/                    # Entity models
│   ├── ViewModels/                # MVC view models
│   └── Api/                       # API DTOs
├── Database/
│   └── SupplierManagementDbContext.cs
├── Mappings/
│   └── AutoMapperProfile.cs       # Object mappings
├── Middleware/
│   └── GlobalExceptionMiddleware.cs
└── Migrations/                    # EF Core migrations
```

## 🧪 Test Data

The application includes comprehensive test data with realistic overlap scenarios:

### **Suppliers**
- **BestValue**: Multiple overlapping rates for testing
- **Quality Supplies**: Rate continuity examples
- **Premium Partners**: Complex overlap patterns

### **Overlapping Rate Examples**
- **Supplier 1**: Rates with 45-day overlap (Feb 15 - Mar 31, 2015)
- **Supplier 2**: Open-ended rate overlaps (Oct 15, 2016 - Feb 1, 2017)
- **Supplier 3**: Multiple overlapping periods with gap analysis

## 🔍 Code Quality & Standards

### **Code Quality Metrics**
- ✅ **SOLID Principles**: Applied throughout
- ✅ **Clean Code**: Descriptive naming, proper documentation
- ✅ **DRY Principle**: No code duplication
- ✅ **KISS Principle**: Simple, maintainable solutions
- ✅ **YAGNI**: No over-engineering

### **Error Handling**
- Global exception middleware
- Structured error responses
- Comprehensive logging
- Production-safe error messages

### **Performance Considerations**
- Database query optimization
- Response caching strategies
- Connection pooling
- Async/await patterns throughout

## 🚀 Production Deployment

### **Environment Configuration**
- Development: Full logging, Swagger enabled
- Production: Secure headers, minimal logging
- Staging: Balance of debugging and security

### **Database Deployment**
```bash
# Apply migrations in production
dotnet ef database update --connection "Production-Connection-String"
```

### **Docker Support** (Ready for containerization)
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
# Build steps...
```

## 📈 Monitoring & Observability

### **Logging Strategy**
- Structured logging with Serilog-compatible format
- Request/response logging
- Performance metrics
- Error tracking with correlation IDs

### **Health Checks** (Ready for implementation)
- Database connectivity
- External service dependencies
- Application performance metrics

## 🤝 Contributing

### **Development Standards**
1. Follow SOLID principles
2. Write comprehensive unit tests
3. Use meaningful commit messages
4. Update documentation for API changes
5. Ensure security best practices

### **Code Review Checklist**
- [ ] Security vulnerabilities addressed
- [ ] Performance implications considered
- [ ] Error handling implemented
- [ ] Logging added appropriately
- [ ] Tests written and passing
- [ ] Documentation updated

## 📝 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 🔗 Additional Resources

- [ASP.NET Core Documentation](https://docs.microsoft.com/en-us/aspnet/core/)
- [Entity Framework Core Documentation](https://docs.microsoft.com/en-us/ef/core/)
- [JWT Authentication Best Practices](https://auth0.com/blog/a-look-at-the-latest-draft-for-jwt-bcp/)
- [API Security Best Practices](https://owasp.org/www-project-api-security/)

---

## 📞 Support

For questions or issues related to Exercise 2 implementation:
1. Check the Swagger documentation at `/swagger`
2. Review the application logs for detailed error information
3. Ensure proper JWT token authentication
4. Verify database connectivity and migrations

**Exercise 2 Status: ✅ COMPLETE - Production Ready**
