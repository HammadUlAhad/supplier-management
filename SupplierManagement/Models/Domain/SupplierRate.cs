using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SupplierManagement.Models.Domain
{
    public class SupplierRate
    {
        [Key]
        public int SupplierRateId { get; set; }

        [Required]
        public int SupplierId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Rate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        public DateTime RateStartDate { get; set; }

        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        public DateTime? RateEndDate { get; set; }

        [Required]
        [StringLength(450)]
        public string CreatedByUser { get; set; } = string.Empty;

        [Required]
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        // Navigation property
        [ForeignKey("SupplierId")]
        public virtual Supplier Supplier { get; set; } = null!;
    }
}
