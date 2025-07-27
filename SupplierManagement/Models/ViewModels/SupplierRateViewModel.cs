using System.ComponentModel.DataAnnotations;

namespace SupplierManagement.Models.ViewModels
{
    public class SupplierRateViewModel
    {
        public int SupplierRateId { get; set; }

        [Required(ErrorMessage = "Supplier is required")]
        [Display(Name = "Supplier")]
        public int SupplierId { get; set; }

        [Required(ErrorMessage = "Rate is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Rate must be greater than 0")]
        [Display(Name = "Rate")]
        public decimal Rate { get; set; }

        [Required(ErrorMessage = "Rate Start Date is required")]
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime RateStartDate { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateTime? RateEndDate { get; set; }

        [Required(ErrorMessage = "Created By User is required")]
        [StringLength(450, ErrorMessage = "Created By User cannot exceed 450 characters")]
        [Display(Name = "Created By")]
        public string CreatedByUser { get; set; } = string.Empty;

        [Display(Name = "Created On")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedOn { get; set; }

        // For display purposes
        public string? SupplierName { get; set; }
    }
}
