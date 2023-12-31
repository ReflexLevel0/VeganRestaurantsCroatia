using api.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace api.Controllers;

[Route("[controller]")]
[ApiController]
public class LinkController(IDb db) : ControllerBase
{
	/// <summary>
	/// Returns links based on input parameters
	/// </summary>
	/// <param name="restaurantId">Restaurant for which links will be returned (if null, links for all restaurants will be returned)</param>
	/// <param name="type">Type of links that will be returned (if null, links of all types will be returned)</param>
	/// <response code="200">Links fetched</response>
	[HttpGet]
	[Produces("application/json")]
	public async Task<ActionResult<ApiResponseWrapper>> GetLinks(int? restaurantId, string? type)
	{
		try
		{
			var links = new List<LinkDTO?>();
			await foreach (var restaurant in db.GetLinks(restaurantId, type))
			{
				links.Add(restaurant);
			}

			return Ok(new ApiResponseWrapper("OK", "Fetched link objects", JsonConvert.SerializeObject(links)));
		}
		catch (Exception ex)
		{
			return new ErrorHandler().Parse(ex);
		}
	}

	/// <summary>
	/// Returns all links for the specified restaurant ID
	/// </summary>
	/// <param name="restaurantId">ID of the restaurant for which links will be returned</param>
	/// <response code="200">Links fetched</response>
	/// <response code="404">Restaurant ID not found</response>
	[HttpGet("r/{restaurantId}")]
	[Produces("application/json")]
	public async Task<ActionResult<ApiResponseWrapper>> GetLinks(int? restaurantId)
	{
		Console.WriteLine(restaurantId);
		return await GetLinks(restaurantId, null);
	}

	/// <summary>
	/// Returns all links for the specified link type
	/// </summary>
	/// <param name="linkType">Type of the links which will be returned</param>
	/// <response code="200">Links fetched</response>
	/// <response code="404">Link type not found</response>
	[HttpGet("t/{linkType}")]
	[Produces("application/json")]
	public async Task<ActionResult<ApiResponseWrapper>> GetLinks(string? linkType)
	{
		return await GetLinks(null, linkType);
	}

	/// <summary>
	/// Creates a new link
	/// </summary>
	/// <param name="link">Link data</param>
	/// <response code="201">Link created</response>
	/// <response code="400">Generic error</response>
	[HttpPost]
	[Produces("application/json")]
	public async Task<ActionResult<ApiResponseWrapper>> PostLink(LinkDTO link)
	{
		try
		{
			var r = await db.PostLink(link);
			return StatusCode(201, new ApiResponseWrapper("CREATED", "Added new link", JsonConvert.SerializeObject(r)));
		}
		catch (Exception ex)
		{
			return new ErrorHandler().Parse(ex);
		}
	}

	/// <summary>
	/// Creates or updates a link based on provided data
	/// </summary>
	/// <param name="link">Link data</param>
	/// <response code="200">Link updated</response>
	/// <response code="201">Link created</response>
	/// <response code="400">Generic error</response>
	[HttpPut]
	[Produces("application/json")]
	public async Task<ActionResult<ApiResponseWrapper>> PutLink(LinkDTO link)
	{
		try
		{
			bool linkExists = false;
			await foreach (var ll in db.GetLinks(link.RestaurantId, link.LinkType))
			{
				linkExists = true;
			}

			//Creating new link if it doesn't exist
			if (linkExists == false) return await PostLink(link);
			
			//Updating an already existing link
			var l = await db.PutLink(link);
			return Ok(new ApiResponseWrapper("OK", "Updated Link", JsonConvert.SerializeObject(l)));
		}
		catch (Exception ex)
		{
			return new ErrorHandler().Parse(ex);
		}
	}

	/// <summary>
	/// Deletes the specified link
	/// </summary>
	/// <param name="link">Link data identifying the link that should be deleted</param>
	/// <response code="200">Restaurant deleted/updated</response>
	/// <response code="404">Restaurant with specified ID not found</response>
	[HttpDelete]
	[Produces("application/json")]
	public async Task<ActionResult<ApiResponseWrapper>> DeleteLink(DeleteLinkDTO link)
	{
		try
		{
			await db.DeleteLink(link);
			return Ok(new ApiResponseWrapper("OK", "Deleted link", null));
		}
		catch (Exception ex)
		{
			return new ErrorHandler().Parse(ex);
		}
	}
}