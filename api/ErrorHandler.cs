using api.Models;
using Microsoft.AspNetCore.Mvc;

namespace api;

public class ErrorHandler : ControllerBase
{
	private ActionResult<ApiResponseWrapper> BadRequestError => StatusCode(400, new ApiResponseWrapper("Bad Request", "Error happened in processing your request", null));
	private ActionResult<ApiResponseWrapper> UniqueConstraintError => StatusCode(400, new ApiResponseWrapper("Bad Request", "Unique constraint error, check if primary key/unique value already exists", null));
	
	public ActionResult<ApiResponseWrapper> Parse(Exception ex)
	{
		ControllerHelper.PrintError(ex);
		if (ex.Message.Contains("id not found")) return StatusCode(400, new ApiResponseWrapper("Bad Request", "Restaurant id not found", null));
		if (ex.Message.Contains("type not found")) return StatusCode(400, new ApiResponseWrapper("Bad Request", "Link type not found", null));
		if (ex.Message.Contains("link not found")) return NotFound(new ApiResponseWrapper("Bad Request", "Link corresponding to specified restaurant id and link type not found", null));
		if (ex.Message.Contains("violates unique constraint")) return UniqueConstraintError;
		return BadRequestError;
	}
}