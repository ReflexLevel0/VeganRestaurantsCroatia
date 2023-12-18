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

		return Ok(new ApiResponseWrapper("OK", "Fetched link objects", JsonConvert.SerializeObject(links)));
	}

	[HttpPost]
	public async Task<ActionResult<ApiResponseWrapper>> PostLink(LinkDTO link)
	{
		try
		{
			var r = await db.PostLink(link);
			return Ok(new ApiResponseWrapper("CREATED", "Added new link", JsonConvert.SerializeObject(r)));
		}
		catch (Exception ex)
		{
			ControllerHelper.PrintError(ex);
			return BadRequestError;
		}
	}

	[HttpPut]
	public async Task<ActionResult<ApiResponseWrapper>> PutLink(LinkDTO link)
	{
		try
		{
			LinkDTO? li = null;
			await foreach (var l in db.GetLinks(link.RestaurantId, link.LinkType))
			{
				li = l;
				break;
			}
			if (li == null) return NotFound(new ApiResponseWrapper("Not Found", "Link with specified id not found", null));
			li = await db.PutLink(link);
			return Ok(new ApiResponseWrapper("OK", "Updated Link", JsonConvert.SerializeObject(li)));
		}
		catch (Exception ex)
		{
			ControllerHelper.PrintError(ex);
			return BadRequestError;
		}
	}
}