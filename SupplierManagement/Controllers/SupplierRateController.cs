using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SupplierManagement.Models.ViewModels;
using SupplierManagement.Services.Interfaces;

namespace SupplierManagement.Controllers
{
    public class SupplierRateController : Controller
    {
        private readonly ISupplierRateService _supplierRateService;
        private readonly ILogger<SupplierRateController> _logger;

        public SupplierRateController(ISupplierRateService supplierRateService, ILogger<SupplierRateController> logger)
        {
            _supplierRateService = supplierRateService;
            _logger = logger;
        }

        // GET: SupplierRate
        public async Task<IActionResult> Index()
        {
            try
            {
                var supplierRates = await _supplierRateService.GetAllSupplierRatesAsync();
                return View(supplierRates);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving supplier rates");
                TempData["Error"] = "An error occurred while loading supplier rates.";
                return View(new List<SupplierRateViewModel>());
            }
        }

        // GET: SupplierRate/Details/5
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var supplierRate = await _supplierRateService.GetSupplierRateByIdAsync(id);
                if (supplierRate == null)
                {
                    return NotFound();
                }

                return View(supplierRate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving supplier rate {SupplierRateId}", id);
                return NotFound();
            }
        }

        // GET: SupplierRate/Create
        public async Task<IActionResult> Create()
        {
            await PopulateSupplierDropdown();
            return View(new SupplierRateViewModel 
            { 
                RateStartDate = DateTime.Today 
            });
        }

        // POST: SupplierRate/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SupplierRateViewModel supplierRateViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _supplierRateService.CreateSupplierRateAsync(supplierRateViewModel);
                    TempData["Success"] = "Supplier rate created successfully.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while creating supplier rate");
                    ModelState.AddModelError("", "An error occurred while creating the supplier rate.");
                }
            }

            await PopulateSupplierDropdown();
            return View(supplierRateViewModel);
        }

        // GET: SupplierRate/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var supplierRate = await _supplierRateService.GetSupplierRateByIdAsync(id);
                if (supplierRate == null)
                {
                    return NotFound();
                }

                await PopulateSupplierDropdown();
                return View(supplierRate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving supplier rate {SupplierRateId} for editing", id);
                return NotFound();
            }
        }

        // POST: SupplierRate/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SupplierRateViewModel supplierRateViewModel)
        {
            if (id != supplierRateViewModel.SupplierRateId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var updatedSupplierRate = await _supplierRateService.UpdateSupplierRateAsync(supplierRateViewModel);
                    if (updatedSupplierRate == null)
                    {
                        return NotFound();
                    }

                    TempData["Success"] = "Supplier rate updated successfully.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while updating supplier rate {SupplierRateId}", id);
                    ModelState.AddModelError("", "An error occurred while updating the supplier rate.");
                }
            }

            await PopulateSupplierDropdown();
            return View(supplierRateViewModel);
        }

        // GET: SupplierRate/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var supplierRate = await _supplierRateService.GetSupplierRateByIdAsync(id);
                if (supplierRate == null)
                {
                    return NotFound();
                }

                return View(supplierRate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving supplier rate {SupplierRateId} for deletion", id);
                return NotFound();
            }
        }

        // POST: SupplierRate/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var result = await _supplierRateService.DeleteSupplierRateAsync(id);
                if (!result)
                {
                    return NotFound();
                }

                TempData["Success"] = "Supplier rate deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting supplier rate {SupplierRateId}", id);
                TempData["Error"] = "An error occurred while deleting the supplier rate.";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: SupplierRate/BySupplier/5
        public async Task<IActionResult> BySupplier(int id)
        {
            try
            {
                var supplierRates = await _supplierRateService.GetSupplierRatesBySupplierIdAsync(id);
                ViewBag.SupplierId = id;
                return View(supplierRates);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving supplier rates for supplier {SupplierId}", id);
                TempData["Error"] = "An error occurred while loading supplier rates.";
                return View(new List<SupplierRateViewModel>());
            }
        }

        private async Task PopulateSupplierDropdown()
        {
            var suppliers = await _supplierRateService.GetSuppliersForDropdownAsync();
            ViewBag.Suppliers = new SelectList(suppliers, "SupplierId", "Name");
        }
    }
}
