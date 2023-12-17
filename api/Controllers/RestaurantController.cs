using api.Models;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Route("[controller]")]
[ApiController]
public class RestaurantController(IDb db) : ControllerBase
{
	[HttpGet]
	public async Task<ActionResult<IEnumerable<Restaurant>>> GetRestaurants()
	{
		var restaurants = new List<Restaurant?>();
		await foreach (var restaurant in db.GetRestaurants())
		{
			restaurants.Add(restaurant);
		}
		
		return Ok(restaurants);
	}

	[HttpGet("{id}")]
	public async Task<ActionResult<IEnumerable<Restaurant>>> GetRestaurant(int id)
	{
		var restaurant = await db.GetRestaurant(id);
		return restaurant == null ? NotFound($"restaurantId = {id}") : Ok(restaurant);
	}
}