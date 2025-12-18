using HotelListing.API.Constants;
using HotelListing.API.Results;
using Microsoft.AspNetCore.Mvc;

namespace HotelListing.API.Controllers
{
    public abstract class BaseApiController : ControllerBase
    {// Helper methods to convert Result to ActionResult
        protected ActionResult<T> ToActionResult<T>(Result<T> result)
            => result.IsSuccess ? Ok(result.Value) : MapErrorsToResponse(result.Errors);

        // Helper method for non-generic Result
        protected ActionResult ToActionResult(Result result)
            => result.IsSuccess ? NoContent() : MapErrorsToResponse(result.Errors);

        // Maps errors to appropriate HTTP responses
        protected ActionResult MapErrorsToResponse(Error[] errors)
        {
            if (errors is null || errors.Length == 0) return Problem();

            var e = errors[0];
            return e.Code switch
            {
                ErrorCodes.NotFound => NotFound(e.Description),
                ErrorCodes.Validation => BadRequest(e.Description),
                ErrorCodes.BadRequest => BadRequest(e.Description),
                ErrorCodes.Conflict => Conflict(e.Description),
                _ => Problem(detail: string.Join("; ", errors.Select(x => x.Description)), title: e.Code)
            };
        }
    }
}
