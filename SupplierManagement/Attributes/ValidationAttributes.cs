using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.ComponentModel.DataAnnotations;

namespace SupplierManagement.Attributes
{
    /// <summary>
    /// Validation attribute to ensure supplier ID is within acceptable range
    /// Prevents potential resource exhaustion attacks with extremely large IDs
    /// </summary>
    public class ValidSupplierIdAttribute : ActionFilterAttribute
    {
        private const int MaxSupplierId = 10_000_000; // 10 million max suppliers
        private const int MinSupplierId = 1;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionArguments.TryGetValue("supplierId", out var supplierIdObj))
            {
                if (supplierIdObj is int supplierId)
                {
                    if (supplierId < MinSupplierId || supplierId > MaxSupplierId)
                    {
                        context.Result = new BadRequestObjectResult(new 
                        { 
                            error = $"Supplier ID must be between {MinSupplierId} and {MaxSupplierId:N0}",
                            supplierId = supplierId 
                        });
                        return;
                    }
                }
            }

            base.OnActionExecuting(context);
        }
    }

    /// <summary>
    /// Enhanced supplier ID range validation with configurable limits
    /// </summary>
    public class ValidateSupplierIdRangeAttribute : ValidationAttribute
    {
        private readonly int _minValue;
        private readonly int _maxValue;

        public ValidateSupplierIdRangeAttribute(int minValue, int maxValue)
        {
            _minValue = minValue;
            _maxValue = maxValue;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null) return ValidationResult.Success; // Allow null for optional parameters

            if (value is int intValue)
            {
                if (intValue < _minValue || intValue > _maxValue)
                {
                    return new ValidationResult($"Value must be between {_minValue:N0} and {_maxValue:N0}");
                }
            }

            return ValidationResult.Success;
        }
    }

    /// <summary>
    /// Security attribute to prevent information disclosure in error responses
    /// Returns generic error messages instead of detailed system information
    /// </summary>
    public class SecureErrorResponseAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception != null)
            {
                // Log the actual exception but return generic error to client
                var logger = context.HttpContext.RequestServices
                    .GetRequiredService<ILogger<SecureErrorResponseAttribute>>();
                
                logger.LogError(context.Exception, "API error occurred in {Controller}.{Action}", 
                    context.RouteData.Values["controller"], 
                    context.RouteData.Values["action"]);

                context.Result = new ObjectResult(new { error = "An error occurred while processing your request" })
                {
                    StatusCode = 500
                };
                context.ExceptionHandled = true;
            }

            base.OnActionExecuted(context);
        }
    }
}
