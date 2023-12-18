using api.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace api.Controllers;

[Route("[controller]")]
[ApiController]
public class LinkController(IDb db) : ControllerBase
{
	private ActionResult<ApiResponseWrapper> BadRequestError => StatusCode(400, new ApiResponseWrapper("Bad Request", "Error happened in processing your request", null));
	
	/// <summary>
	/// Returns links based on input parameters
	/// </summary>
	/// <param name="restaurantId">Restaurant for which links will be returned (if null, links for all restaurants will be returned)</param>
	/// <param name="type">Type of links that will be returned (if null, links of all types will be returned)</param>
	/// <response code="200">Links fetched</response>
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

	/// <summary>
	/// Creates a new link
	/// </summary>
	/// <param name="link">Link data</param>
	/// <response code="200">Restaurant created</response>
	/// <response code="400">Generic error</response>
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

	/// <summary>
	/// Creates or updates a link based on provided data
	/// </summary>
	/// <param name="link">Link data</param>
	/// <response code="200">Restaurant created/updated</response>
	/// <response code="400">Generic error</response>
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

	/// <summary>
	/// Deletes the specified link
	/// </summary>
	/// <param name="link">Link data identifying the link that should be deleted</param>
	/// <response code="200">Restaurant deleted/updated</response>
	/// <response code="400">Generic error</response>
	/// <response code="404">Restaurant with specified ID not found</response>
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