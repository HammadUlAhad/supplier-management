namespace SupplierManagement.Models.Api
{
    public class SupplierApiDto
    {
        public int SupplierId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Address { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedByUser { get; set; } = string.Empty;
        public List<SupplierRateApiDto> Rates { get; set; } = new();
    }
}
