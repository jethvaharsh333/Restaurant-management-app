using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using restaurant_management_backend.Utility;

namespace restaurant_management_backend.Filters
{
    public class ValidateModelStateAttribute: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var apiResponse = ApiResponse<object>.FailureResponse("One or more validation errors occurred.", 400);
                context.Result = new BadRequestObjectResult(apiResponse);
            }
        }
    }
}
