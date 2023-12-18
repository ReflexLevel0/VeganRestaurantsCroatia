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
			var l = await db.PutLink(link);
			return Ok(new ApiResponseWrapper("OK", "Updated Link", JsonConvert.SerializeObject(l)));
		}
		catch (Exception ex)
		{
			ControllerHelper.PrintError(ex);
			return BadRequestError;
		}
	}

	[HttpDelete]
	public async Task<ActionResult<ApiResponseWrapper>> DeleteLink(DeleteLinkDTO link)
	{
		try
		{
			await db.DeleteLink(link);
			return Ok(new ApiResponseWrapper("OK", "Deleted link", null));
		}
		catch (Exception ex)
		{
			ControllerHelper.PrintError(ex);
			if (ex.Message.Contains("id not found") == false) return BadRequestError;
			return NotFound(new ApiResponseWrapper("Not Found", "Link with specified restaurant id and link type not found", null));
		}
	}
}