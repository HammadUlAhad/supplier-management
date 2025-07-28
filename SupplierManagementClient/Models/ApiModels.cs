namespace SupplierManagementClient.Models
{
    /// <summary>
    /// Authentication response model
    /// </summary>
    public class AuthResponse
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
    }

    /// <summary>
    /// Supplier API response model
    /// </summary>
    public class SupplierApiResponse
    {
        public int SupplierId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Address { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedByUser { get; set; } = string.Empty;
        public List<SupplierRateApiResponse> Rates { get; set; } = new();
    }

    /// <summary>
    /// Supplier rate API response model
    /// </summary>
    public class SupplierRateApiResponse
    {
        public int SupplierRateId { get; set; }
        public int SupplierId { get; set; }
        public decimal Rate { get; set; }
        public DateTime RateStartDate { get; set; }
        public DateTime? RateEndDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedByUser { get; set; } = string.Empty;
        public string? SupplierName { get; set; }
    }

    /// <summary>
    /// Overlapping rate response model
    /// </summary>
    public class OverlappingRateResponse
    {
        public int SupplierId { get; set; }
        public string SupplierName { get; set; } = string.Empty;
        public List<OverlappingRatePair> OverlappingRates { get; set; } = new();
    }

    /// <summary>
    /// Overlapping rate pair model
    /// </summary>
    public class OverlappingRatePair
    {
        public SupplierRateApiResponse Rate1 { get; set; } = null!;
        public SupplierRateApiResponse Rate2 { get; set; } = null!;
        public DateTime OverlapStartDate { get; set; }
        public DateTime OverlapEndDate { get; set; }
        public int OverlapDays { get; set; }
    }

    /// <summary>
    /// Generic API result wrapper
    /// </summary>
    public class ApiResult<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public string? ErrorMessage { get; set; }
        public int StatusCode { get; set; }
        
        public static ApiResult<T> SuccessResult(T data)
        {
            return new ApiResult<T>
            {
                Success = true,
                Data = data,
                StatusCode = 200
            };
        }
        
        public static ApiResult<T> ErrorResult(string errorMessage, int statusCode = 500)
        {
            return new ApiResult<T>
            {
                Success = false,
                ErrorMessage = errorMessage,
                StatusCode = statusCode
            };
        }
    }
}
