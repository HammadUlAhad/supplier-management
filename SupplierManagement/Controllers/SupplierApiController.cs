using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using SupplierManagement.Models.Api;
using SupplierManagement.Services.Interfaces;
using SupplierManagement.Attributes;
using System.ComponentModel.DataAnnotations;

namespace SupplierManagement.Controllers
{
    [ApiController]
    [Route("api/v1/suppliers")]
    [Produces("application/json")]
    [EnableRateLimiting("ApiPolicy")]
    public class SupplierApiController : ControllerBase
    {
        private readonly ISupplierService _supplierService;
        private readonly ISupplierRateService _supplierRateService;
        private readonly ILogger<SupplierApiController> _logger;

        public SupplierApiController(
            ISupplierService supplierService,
            ISupplierRateService supplierRateService,
            ILogger<SupplierApiController> logger)
        {
            _supplierService = supplierService;
            _supplierRateService = supplierRateService;
            _logger = logger;
        }

        /// <summary>
        /// Get all suppliers with their rates
        /// </summary>
        /// <returns>A list of suppliers with their associated rates</returns>
        /// <response code="200">Returns the list of suppliers with rates</response>
        /// <response code="401">Unauthorized - Invalid or missing JWT token</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(typeof(IEnumerable<SupplierApiDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ResponseCache(Duration = 300, Location = ResponseCacheLocation.Any, VaryByHeader = "Authorization")]
        public async Task<ActionResult<IEnumerable<SupplierApiDto>>> GetAllSuppliersWithRates()
        {
            try
            {
                _logger.LogInformation("Getting all suppliers with rates");
                var suppliers = await _supplierService.GetAllSuppliersWithRatesAsync();
                
                _logger.LogInformation("Retrieved {Count} suppliers with rates", suppliers.Count());
                return Ok(suppliers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving suppliers with rates");
                return StatusCode(500, new { error = "An error occurred while retrieving the data" });
            }
        }

        /// <summary>
        /// Get suppliers with overlapping rate periods
        /// </summary>
        /// <param name="supplierId">Optional: Filter by specific supplier ID (1-10,000,000)</param>
        /// <returns>A list of suppliers with overlapping rate periods</returns>
        /// <response code="200">Returns suppliers with overlapping rates</response>
        /// <response code="400">Bad request - Invalid supplier ID</response>
        /// <response code="401">Unauthorized - Invalid or missing JWT token</response>
        /// <response code="404">Not found - Supplier with specified ID not found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("overlaps")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(typeof(IEnumerable<OverlappingRateDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ResponseCache(Duration = 300, Location = ResponseCacheLocation.Any, VaryByQueryKeys = new[] { "supplierId" }, VaryByHeader = "Authorization")]
        public async Task<ActionResult<IEnumerable<OverlappingRateDto>>> GetOverlappingRates(
            [FromQuery][Range(1, 10_000_000, ErrorMessage = "Supplier ID must be between 1 and 10,000,000")] 
            int? supplierId = null)
        {
            try
            {
                // Validate supplierId parameter
                if (supplierId.HasValue && supplierId.Value <= 0)
                {
                    _logger.LogWarning("Invalid supplier ID provided: {SupplierId}", supplierId.Value);
                    return BadRequest(new { error = "Supplier ID must be a positive integer", supplierId = supplierId.Value });
                }

                if (supplierId.HasValue)
                {
                    _logger.LogInformation("Getting overlapping rates for supplier {SupplierId}", supplierId.Value);
                    
                    // Validate supplier exists
                    var supplierExists = await _supplierService.SupplierExistsAsync(supplierId.Value);
                    if (!supplierExists)
                    {
                        _logger.LogWarning("Supplier {SupplierId} not found", supplierId.Value);
                        return NotFound(new { error = $"Supplier with ID {supplierId.Value} not found", supplierId = supplierId.Value });
                    }
                }
                else
                {
                    _logger.LogInformation("Getting overlapping rates for all suppliers");
                }

                var overlappingRates = await _supplierRateService.GetOverlappingRatesAsync(supplierId);
                
                _logger.LogInformation("Found {Count} suppliers with overlapping rates", overlappingRates.Count());
                return Ok(overlappingRates);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving overlapping rates for supplier {SupplierId}", supplierId);
                return StatusCode(500, new { error = "An error occurred while retrieving the data" });
            }
        }
    }
}
