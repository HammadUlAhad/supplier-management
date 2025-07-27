namespace SupplierManagement.Models.Api
{
    public class SupplierRateApiDto
    {
        public int SupplierRateId { get; set; }
        public int SupplierId { get; set; }
        public decimal Rate { get; set; }
        public DateTime RateStartDate { get; set; }
        public DateTime? RateEndDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedByUser { get; set; } = string.Empty;
        
        // Additional fields for overlap detection
        public string? SupplierName { get; set; }
    }
}
