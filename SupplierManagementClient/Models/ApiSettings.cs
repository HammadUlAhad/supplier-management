namespace SupplierManagementClient.Models
{
    /// <summary>
    /// Configuration model for API settings
    /// </summary>
    public class ApiSettings
    {
        public const string SectionName = "ApiSettings";
        
        public string BaseUrl { get; set; } = string.Empty;
        public string AuthEndpoint { get; set; } = string.Empty;
        public string SuppliersEndpoint { get; set; } = string.Empty;
        public string OverlapsEndpoint { get; set; } = string.Empty;
        public int TimeoutSeconds { get; set; } = 30;
        public bool EnableRetry { get; set; } = true;
        public int MaxRetryAttempts { get; set; } = 3;
    }

    /// <summary>
    /// Demo credentials for testing
    /// </summary>
    public class DemoCredentials
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
