# Supplier Management API Testing Guide

## Base URL
- Development: `http://localhost:5114`
- Swagger UI: `http://localhost:5114/swagger`

## API Testing Steps

### 1. Get Authentication Token

```bash
curl -X POST "http://localhost:5114/api/auth/login" \
     -H "Content-Type: application/json" \
     -d '{"username": "admin", "password": "password123"}'
```

Expected Response:
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiration": "2025-01-28T12:00:00Z"
}
```

### 2. Test Suppliers with Rates API

```bash
curl -X GET "http://localhost:5114/api/suppliers/with-rates" \
     -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

### 3. Test Overlapping Rates API (All Suppliers)

```bash
curl -X GET "http://localhost:5114/api/suppliers/overlapping-rates" \
     -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

### 4. Test Overlapping Rates API (Specific Supplier)

```bash
curl -X GET "http://localhost:5114/api/suppliers/overlapping-rates?supplierId=1" \
     -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

### 5. Get Demo Credentials

```bash
curl -X GET "http://localhost:5114/api/auth/test-credentials"
```

## PowerShell Testing Examples

### Get Token
```powershell
$response = Invoke-RestMethod -Uri "http://localhost:5114/api/auth/login" `
    -Method POST `
    -ContentType "application/json" `
    -Body '{"username": "admin", "password": "password123"}'

$token = $response.token
```

### Use Token for API Calls
```powershell
$headers = @{ Authorization = "Bearer $token" }

# Get all suppliers with rates
$suppliers = Invoke-RestMethod -Uri "http://localhost:5114/api/suppliers/with-rates" `
    -Headers $headers

# Get overlapping rates
$overlaps = Invoke-RestMethod -Uri "http://localhost:5114/api/suppliers/overlapping-rates" `
    -Headers $headers
```

## Expected API Responses

### Suppliers with Rates
```json
[
  {
    "supplierId": 1,
    "name": "BestValue Suppliers",
    "address": "123 Business Ave, Commerce City",
    "createdOn": "2025-01-15T10:30:00Z",
    "createdByUser": "system",
    "rates": [
      {
        "supplierRateId": 1,
        "supplierId": 1,
        "rate": 25.50,
        "rateStartDate": "2025-01-01T00:00:00Z",
        "rateEndDate": "2025-06-30T00:00:00Z",
        "createdOn": "2025-01-15T10:30:00Z",
        "createdByUser": "system"
      }
    ]
  }
]
```

### Overlapping Rates
```json
[
  {
    "supplierId": 1,
    "supplierName": "BestValue Suppliers",
    "overlappingRates": [
      {
        "rate1": {
          "supplierRateId": 1,
          "supplierId": 1,
          "rate": 25.50,
          "rateStartDate": "2025-01-01T00:00:00Z",
          "rateEndDate": "2025-06-30T00:00:00Z"
        },
        "rate2": {
          "supplierRateId": 2,
          "supplierId": 1,
          "rate": 28.00,
          "rateStartDate": "2025-06-01T00:00:00Z",
          "rateEndDate": null
        },
        "overlapStartDate": "2025-06-01T00:00:00Z",
        "overlapEndDate": "2025-06-30T00:00:00Z",
        "overlapDays": 30
      }
    ]
  }
]
```

## Testing Business Rules

### Null End Date Handling
- Rates with `null` end dates are treated as ongoing
- Overlaps are calculated correctly with open-ended rates
- Display end date uses current date for ongoing rates

### Supplier-Specific Filtering
- Use `?supplierId=X` parameter to filter overlaps
- Returns 404 if supplier doesn't exist
- Returns empty array if no overlaps found for supplier

### Authentication
- All protected endpoints require valid JWT token
- Tokens expire after 24 hours (configurable)
- Invalid tokens return 401 Unauthorized

## Manual Testing via Browser
1. Navigate to `http://localhost:5114/swagger`
2. Use the "Authorize" button to add JWT token
3. Test endpoints interactively
4. View request/response examples
