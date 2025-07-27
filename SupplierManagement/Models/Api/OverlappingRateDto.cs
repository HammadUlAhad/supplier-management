namespace SupplierManagement.Models.Api
{
    public class OverlappingRateDto
    {
        public int SupplierId { get; set; }
        public string SupplierName { get; set; } = string.Empty;
        public List<OverlappingRatePairDto> OverlappingRates { get; set; } = new();
    }

    public class OverlappingRatePairDto
    {
        public SupplierRateApiDto Rate1 { get; set; } = null!;
        public SupplierRateApiDto Rate2 { get; set; } = null!;
        public DateTime OverlapStartDate { get; set; }
        public DateTime OverlapEndDate { get; set; }
        public int OverlapDays { get; set; }
    }
}
