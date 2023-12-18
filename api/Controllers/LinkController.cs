using api.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace api.Controllers;

[Route("[controller]")]
[ApiController]
public class LinkController(IDb db) : ControllerBase
{
	private ActionResult<ApiResponseWrapper> BadRequestError => StatusCode(400, new ApiResponseWrapper("Bad Request", "Error happened in processing your request", null));
	
	[HttpGet]
	public async Task<ActionResult<ApiResponseWrapper>> GetLinks(int? restaurantId, string? type)
	{
		var links = new List<LinkDTO?>();
		await foreach (var restaurant in db.GetLinks(restaurantId, type))
		{
			links.Add(restaurant);
		}

		return Ok(new ApiResponseWrapper("OK", "Fetched restaurant objects", JsonConvert.SerializeObject(links)));
	}
