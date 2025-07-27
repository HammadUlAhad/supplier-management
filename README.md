# Supplier Management System

A comprehensive web application for managing suppliers and their rates, built with ASP.NET Core 9.0 and Entity Framework Core.

## 📋 Project Overview

This application provides a complete CRUD (Create, Read, Update, Delete) interface for managing:
- **Suppliers**: Company information including name, address, and contact details
- **Supplier Rates**: Time-based pricing rates for each supplier with effective date ranges

## 🏗️ Architecture

The application follows industry best practices with a layered architecture:

```
├── Controllers/          # MVC Controllers (Presentation Layer)
├── Services/             # Business Logic Layer
├── Repositories/         # Data Access Layer
├── Models/
│   ├── Domain/          # Entity Models
│   └── ViewModels/      # UI Models
├── Database/            # Entity Framework DbContext
├── Views/               # Razor Views
├── Middleware/          # Custom Middleware
└── Mappings/            # AutoMapper Profiles
```

## 🛠️ Technology Stack

- **Framework**: ASP.NET Core 9.0
- **Database**: SQL Server / LocalDB
- **ORM**: Entity Framework Core 9.0.0
- **Mapping**: AutoMapper 12.0.1
- **Frontend**: Bootstrap 5, Razor Views
- **Authentication**: Cookie-based Authentication
- **Logging**: Built-in ASP.NET Core Logging

### NuGet Packages

```xml
<PackageReference Include="Microsoft.EntityFrameworkCore" Version="9.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="9.0.0">
  <PrivateAssets>all</PrivateAssets>
  <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
</PackageReference>
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="9.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="9.0.0">
  <PrivateAssets>all</PrivateAssets>
  <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
</PackageReference>
<PackageReference Include="AutoMapper" Version="12.0.1" />
<PackageReference Include="AutoMapper.Extensions.Microsoft.DependencyInjection" Version="12.0.1" />
```

## 📊 Database Schema

### Suppliers Table
| Column | Type | Required | Description |
|--------|------|----------|-------------|
| SupplierId | int (Identity) | Yes | Primary Key |
| Name | nvarchar(450) | Yes | Supplier Name |
| Address | nvarchar(450) | No | Supplier Address |
| CreatedByUser | nvarchar(450) | Yes | User who created record |
| CreatedOn | datetime2 | Yes | Creation timestamp |

### SupplierRates Table
| Column | Type | Required | Description |
|--------|------|----------|-------------|
| SupplierRateId | int (Identity) | Yes | Primary Key |
| SupplierId | int | Yes | Foreign Key to Suppliers |
| Rate | decimal(18,2) | Yes | Rate amount |
| RateStartDate | date | Yes | Rate effective start date |
| RateEndDate | date | No | Rate effective end date |
| CreatedByUser | nvarchar(450) | Yes | User who created record |
| CreatedOn | datetime2 | Yes | Creation timestamp |

## 🚀 Getting Started

### Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [SQL Server LocalDB](https://docs.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb) (included with Visual Studio)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)

### Installation & Setup

#### Option 1: Using Visual Studio 2022

1. **Clone the repository**
   ```bash
   git clone https://github.com/HammadUlAhad/supplier-management.git
   ```

2. **Open the solution**
   - Launch Visual Studio 2022
   - Click "Open a project or solution"
   - Navigate to `supplier-management\SupplierManagement\SupplierManagement.sln`
   - Click "Open"

3. **Restore NuGet packages**
   - Visual Studio will automatically restore packages
   - Or manually: Right-click solution → "Restore NuGet Packages"

4. **Set up the database**
   - Open Package Manager Console: `Tools` → `NuGet Package Manager` → `Package Manager Console`
   - Run the following command:
   ```powershell
   Update-Database
   ```
   This will create the database and apply all migrations with sample data.

5. **Run the application**
   - Press `F5` or click the "Play" button (IIS Express)
   - Or press `Ctrl+F5` to run without debugging
   - The application will open automatically in your default browser

#### Option 2: Using Command Line (.NET CLI)

1. **Clone the repository**
   ```bash
   git clone https://github.com/HammadUlAhad/supplier-management.git
   cd supplier-management
   ```

2. **Navigate to the project directory**
   ```bash
   cd SupplierManagement
   ```

3. **Restore NuGet packages**
   ```bash
   dotnet restore
   ```

4. **Update the database**
   ```bash
   dotnet ef database update
   ```
   This will create the database and apply all migrations with sample data.

5. **Build the project**
   ```bash
   dotnet build
   ```

6. **Run the application**
   ```bash
   dotnet run
   ```

7. **Open your browser** and navigate to:
   - **HTTPS**: `https://localhost:7xxx`
   - **HTTP**: `http://localhost:5xxx`
   
   (Port numbers will be displayed in the console)

#### Option 3: Using VS Code

1. **Prerequisites for VS Code**
   - Install [C# Dev Kit extension](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit)
   - Install [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)

2. **Open the project**
   ```bash
   git clone https://github.com/HammadUlAhad/supplier-management.git
   cd supplier-management
   code .
   ```

3. **Open integrated terminal** (`Ctrl+``) and run:
   ```bash
   cd SupplierManagement
   dotnet restore
   dotnet ef database update
   dotnet run
   ```

## 🔧 Configuration

### Connection String

The application uses SQL Server LocalDB by default. Update `appsettings.json` if needed:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=SupplierManagementDB;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

### Environment Variables (Production)

For production deployment, set the connection string as an environment variable:
```bash
export ConnectionStrings__DefaultConnection="Your-Production-Connection-String"
```

## 📱 Features

### Supplier Management
- ✅ Create new suppliers
- ✅ View supplier list with search/filter
- ✅ Update supplier information
- ✅ Delete suppliers (with cascade to rates)
- ✅ View supplier details with associated rates

### Supplier Rate Management
- ✅ Create rates for suppliers
- ✅ View all rates with supplier information
- ✅ Update existing rates
- ✅ Delete rates
- ✅ View rates by specific supplier
- ✅ Date range validation for rate periods

### Security Features
- 🔒 Input validation and sanitization
- 🔒 Anti-forgery token protection
- 🔒 Security headers (XSS, CSRF protection)
- 🔒 HTTPS enforcement
- 🔒 Global exception handling

## 🧪 Sample Data

The application includes seed data with:
- 3 sample suppliers (BestValue, Quality Supplies, Premium Partners)
- 7 sample rates with various date ranges
- Demonstrates active and inactive rates

## 📝 API Endpoints

### Suppliers
- `GET /Supplier` - List all suppliers
- `GET /Supplier/Details/{id}` - View supplier details
- `GET /Supplier/Create` - Create supplier form
- `POST /Supplier/Create` - Submit new supplier
- `GET /Supplier/Edit/{id}` - Edit supplier form
- `POST /Supplier/Edit/{id}` - Update supplier
- `GET /Supplier/Delete/{id}` - Delete confirmation
- `POST /Supplier/Delete/{id}` - Confirm deletion

### Supplier Rates
- `GET /SupplierRate` - List all rates
- `GET /SupplierRate/Details/{id}` - View rate details
- `GET /SupplierRate/Create` - Create rate form
- `POST /SupplierRate/Create` - Submit new rate
- `GET /SupplierRate/Edit/{id}` - Edit rate form
- `POST /SupplierRate/Edit/{id}` - Update rate
- `GET /SupplierRate/Delete/{id}` - Delete confirmation
- `POST /SupplierRate/Delete/{id}` - Confirm deletion
- `GET /SupplierRate/BySupplier/{id}` - Rates for specific supplier

## 🔍 Development

### Database Migrations

Create a new migration:
```bash
dotnet ef migrations add MigrationName
```

Apply migrations:
```bash
dotnet ef database update
```

Remove last migration:
```bash
dotnet ef migrations remove
```

### Building for Production

```bash
dotnet publish -c Release -o ./publish
```

## 🛡️ Security Considerations

- Input validation with regular expressions
- Anti-forgery tokens on all forms
- Secure HTTP headers implemented
- Connection string security for production
- Global exception handling
- HTTPS enforcement

## 📋 Project Structure

```
SupplierManagement/
├── Controllers/
│   ├── HomeController.cs
│   ├── SupplierController.cs
│   └── SupplierRateController.cs
├── Database/
│   └── SupplierManagementDbContext.cs
├── Mappings/
│   └── AutoMapperProfile.cs
├── Middleware/
│   └── GlobalExceptionMiddleware.cs
├── Models/
│   ├── Domain/
│   │   ├── Supplier.cs
│   │   └── SupplierRate.cs
│   ├── ViewModels/
│   │   ├── SupplierViewModel.cs
│   │   └── SupplierRateViewModel.cs
│   └── ErrorViewModel.cs
├── Repositories/
│   ├── Interfaces/
│   └── Implementations/
├── Services/
│   ├── Interfaces/
│   └── Implementations/
├── Views/
│   ├── Home/
│   ├── Supplier/
│   ├── SupplierRate/
│   └── Shared/
├── wwwroot/
├── appsettings.json
└── Program.cs
```

## 👤 Author

**Hammad Ul Ahad**
- GitHub: [@HammadUlAhad](https://github.com/HammadUlAhad)

## 🙏 Acknowledgments

- ASP.NET Core Team for the excellent framework
- Entity Framework Core for robust ORM capabilities
- Bootstrap for responsive UI components