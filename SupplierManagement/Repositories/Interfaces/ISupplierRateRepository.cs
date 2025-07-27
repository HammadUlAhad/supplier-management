using SupplierManagement.Models.Domain;

namespace SupplierManagement.Repositories.Interfaces
{
    public interface ISupplierRateRepository
    {
        Task<IEnumerable<SupplierRate>> GetAllAsync();
        Task<IEnumerable<SupplierRate>> GetBySupplierId(int supplierId);
        Task<SupplierRate?> GetByIdAsync(int id);
        Task<SupplierRate> AddAsync(SupplierRate supplierRate);
        Task<SupplierRate?> UpdateAsync(SupplierRate supplierRate);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        
        // High-performance overlap detection method
        Task<IEnumerable<OverlapResult>> GetOverlappingRatesAsync(int? supplierId = null);
    }

    // DTO for overlap query results
    public class OverlapResult
    {
        public int SupplierId { get; set; }
        public string SupplierName { get; set; } = string.Empty;
        public int Rate1Id { get; set; }
        public decimal Rate1Value { get; set; }
        public DateTime Rate1StartDate { get; set; }
        public DateTime? Rate1EndDate { get; set; }
        public int Rate2Id { get; set; }
        public decimal Rate2Value { get; set; }
        public DateTime Rate2StartDate { get; set; }
        public DateTime? Rate2EndDate { get; set; }
        public DateTime OverlapStartDate { get; set; }
        public DateTime OverlapEndDate { get; set; }
    }
}
