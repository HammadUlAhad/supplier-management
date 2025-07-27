using System.ComponentModel.DataAnnotations;

namespace SupplierManagement.Models.ViewModels
{
    public class SupplierViewModel
    {
        public int SupplierId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(450, ErrorMessage = "Name cannot exceed 450 characters")]
        [RegularExpression(@"^[a-zA-Z0-9\s\-\.\&]+$", ErrorMessage = "Name contains invalid characters")]
        [Display(Name = "Supplier Name")]
        public string Name { get; set; } = string.Empty;

        [StringLength(450, ErrorMessage = "Address cannot exceed 450 characters")]
        [RegularExpression(@"^[a-zA-Z0-9\s\-\.\,\#\/]+$", ErrorMessage = "Address contains invalid characters")]
        [Display(Name = "Address")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "Created By User is required")]
        [StringLength(450, ErrorMessage = "Created By User cannot exceed 450 characters")]
        [RegularExpression(@"^[a-zA-Z]+\.[a-zA-Z]+$", ErrorMessage = "Format must be firstname.lastname")]
        [Display(Name = "Created By")]
        public string CreatedByUser { get; set; } = string.Empty;

        [Display(Name = "Created On")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedOn { get; set; }

        public List<SupplierRateViewModel> SupplierRates { get; set; } = new List<SupplierRateViewModel>();
    }
}
