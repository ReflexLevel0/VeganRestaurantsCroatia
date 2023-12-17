using api.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace api.Controllers;

[Route("[controller]")]
[ApiController]
public class RestaurantController(IDb db) : ControllerBase
{
	private ActionResult<ApiResponseWrapper> BadRequestError => StatusCode(400, new ApiResponseWrapper("Bad Request", "Error happened in processing your request", null));

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

	[HttpGet("{id}")]
	public async Task<ActionResult<ApiResponseWrapper>> GetRestaurant(int id)
	{
		var restaurant = await db.GetRestaurant(id);
		return restaurant == null ? NotFound(new ApiResponseWrapper("Not Found", "Restaurant with specified id not found", null)) : Ok(new ApiResponseWrapper("OK", "Fetched restaurant with specified id", JsonConvert.SerializeObject(restaurant)));
	}

	[HttpPost]
	public async Task<ActionResult<ApiResponseWrapper>> PostRestaurant(Restaurant restaurant)
	{
		try
		{
			await db.PostRestaurant(restaurant);
			return Ok(new ApiResponseWrapper("CREATED", "Added new restaurant", null));
		}
		catch (Exception ex)
		{
			ControllerHelper.PrintError(ex);
			if (ex.Message.Contains("duplicate key") == false) return BadRequestError;
			return Conflict(new ApiResponseWrapper("CONFLICT", "Object already exists", null));
		}
	}

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