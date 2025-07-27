using Microsoft.AspNetCore.Mvc;
using SupplierManagement.Models.ViewModels;
using SupplierManagement.Services.Interfaces;

namespace SupplierManagement.Controllers
{
    public class SupplierController : Controller
    {
        private readonly ISupplierService _supplierService;
        private readonly ILogger<SupplierController> _logger;

        public SupplierController(ISupplierService supplierService, ILogger<SupplierController> logger)
        {
            _supplierService = supplierService;
            _logger = logger;
        }

        // GET: Supplier
        public async Task<IActionResult> Index()
        {
            try
            {
                var suppliers = await _supplierService.GetAllSuppliersAsync();
                return View(suppliers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving suppliers");
                TempData["Error"] = "An error occurred while loading suppliers.";
                return View(new List<SupplierViewModel>());
            }
        }

        // GET: Supplier/Details/5
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var supplier = await _supplierService.GetSupplierByIdAsync(id);
                if (supplier == null)
                {
                    return NotFound();
                }

                return View(supplier);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving supplier {SupplierId}", id);
                return NotFound();
            }
        }

        // GET: Supplier/Create
        public IActionResult Create()
        {
            return View(new SupplierViewModel());
        }

        // POST: Supplier/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SupplierViewModel supplierViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _supplierService.CreateSupplierAsync(supplierViewModel);
                    TempData["Success"] = "Supplier created successfully.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while creating supplier");
                    ModelState.AddModelError("", "An error occurred while creating the supplier.");
                }
            }

            return View(supplierViewModel);
        }

        // GET: Supplier/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var supplier = await _supplierService.GetSupplierByIdAsync(id);
                if (supplier == null)
                {
                    return NotFound();
                }

                return View(supplier);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving supplier {SupplierId} for editing", id);
                return NotFound();
            }
        }

        // POST: Supplier/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SupplierViewModel supplierViewModel)
        {
            if (id != supplierViewModel.SupplierId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var updatedSupplier = await _supplierService.UpdateSupplierAsync(supplierViewModel);
                    if (updatedSupplier == null)
                    {
                        return NotFound();
                    }

                    TempData["Success"] = "Supplier updated successfully.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while updating supplier {SupplierId}", id);
                    ModelState.AddModelError("", "An error occurred while updating the supplier.");
                }
            }

            return View(supplierViewModel);
        }

        // GET: Supplier/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var supplier = await _supplierService.GetSupplierByIdAsync(id);
                if (supplier == null)
                {
                    return NotFound();
                }

                return View(supplier);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving supplier {SupplierId} for deletion", id);
                return NotFound();
            }
        }

        // POST: Supplier/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var result = await _supplierService.DeleteSupplierAsync(id);
                if (!result)
                {
                    return NotFound();
                }

                TempData["Success"] = "Supplier deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting supplier {SupplierId}", id);
                TempData["Error"] = "An error occurred while deleting the supplier.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
