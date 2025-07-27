using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SupplierManagement.Models.Domain
{
    public class Supplier
    {
        [Key]
        public int SupplierId { get; set; }

        [Required]
        [StringLength(450)]
        public string Name { get; set; } = string.Empty;

        [StringLength(450)]
        public string? Address { get; set; }

        [Required]
        [StringLength(450)]
        public string CreatedByUser { get; set; } = string.Empty;

        [Required]
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        // Navigation property
        public virtual ICollection<SupplierRate> SupplierRates { get; set; } = new List<SupplierRate>();
    }
}
