using api.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace api.Controllers;

[Route("[controller]")]
[ApiController]
public class RestaurantController(IDb db) : ControllerBase
{
	private ActionResult<ApiResponseWrapper> BadRequestError => StatusCode(400, new ApiResponseWrapper("Bad Request", "Error happened in processing your request", null));

	/// <summary>
	/// Returns all restaurants
	/// </summary>
	/// <response code="200">Restaurants fetched</response>
	[HttpGet]
	public async Task<ActionResult<ApiResponseWrapper>> GetRestaurants()
	{
		var restaurants = new List<Restaurant?>();
		await foreach (var restaurant in db.GetRestaurants())
		{
			restaurants.Add(restaurant);
		}

		return Ok(new ApiResponseWrapper("OK", "Fetched restaurant objects", JsonConvert.SerializeObject(restaurants)));
	}

	/// <summary>
	/// Returns a specific restaurant
	/// </summary>
	/// <param name="id">ID of the restaurant that is to be returned</param>
	/// <response code="200">Restaurant fetched</response>
	/// <response code="404">Restaurant with specified ID not found</response>
	[HttpGet("{id}")]
	public async Task<ActionResult<ApiResponseWrapper>> GetRestaurant(int id)
	{
		var restaurant = await db.GetRestaurant(id);
		return restaurant == null ? NotFound(new ApiResponseWrapper("Not Found", "Restaurant with specified id not found", null)) : Ok(new ApiResponseWrapper("OK", "Fetched restaurant with specified id", JsonConvert.SerializeObject(restaurant)));
	}

	/// <summary>
	/// Creates a new restaurant
	/// </summary>
	/// <param name="restaurant">Restaurant data</param>
	/// <response code="200">Restaurant created</response>
	/// <response code="400">Generic error</response>
	[HttpPost]
	public async Task<ActionResult<ApiResponseWrapper>> PostRestaurant(RestaurantBase restaurant)
	{
		try
		{
			await db.PostRestaurant(restaurant);
			return Ok(new ApiResponseWrapper("CREATED", "Added new restaurant", null));
		}
		catch (Exception ex)
		{
			ControllerHelper.PrintError(ex);
			return BadRequestError;
		}
	}

	/// <summary>
	/// Creates or updates a restaurant
	/// </summary>
	/// <param name="restaurant">Restaurant to be created or updated</param>
	/// <response code="200">Restaurant created/updated</response>
	/// <response code="400">Generic error</response>
	[HttpPut]
	public async Task<ActionResult<ApiResponseWrapper>> PutRestaurant(Restaurant restaurant)
	{
		try
		{
			await db.PutRestaurant(restaurant);
			return Ok(new ApiResponseWrapper("OK", "Updated restaurant", null));
		}
		catch(Exception ex)
		{
			ControllerHelper.PrintError(ex);
			return BadRequestError;
		}
	}

	/// <summary>
	/// Deletes the specified restaurant
	/// </summary>
	/// <param name="id">ID of the restaurant to be deleted</param>
	/// <response code="200">Restaurant deleted/updated</response>
	/// <response code="400">Generic error</response>
	/// <response code="404">Restaurant with specified ID not found</response>
	[HttpDelete("{id}")]
	public async Task<ActionResult<ApiResponseWrapper>> DeleteRestaurant(int id)
	{
		try
		{
			await db.DeleteRestaurant(id);
			return Ok(new ApiResponseWrapper("OK", "Deleted restaurant", null));
		}
		catch (Exception ex)
		{
			ControllerHelper.PrintError(ex);
			if (ex.Message.Contains("") == false) return BadRequestError;
			return NotFound(new ApiResponseWrapper("Not Found", "Restaurant with specified id not found", null));
		}
	}
}