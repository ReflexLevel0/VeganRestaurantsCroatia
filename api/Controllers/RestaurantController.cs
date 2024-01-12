using api.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace api.Controllers;

[Route("[controller]")]
[ApiController]
public class RestaurantController(IDb db) : ControllerBase
{
	/// <summary>
	/// Returns all restaurants
	/// </summary>
	/// <response code="200">Restaurants fetched</response>
	[HttpGet]
	[Produces("application/json")]
	public async Task<ActionResult<ApiResponseWrapper>> GetRestaurants(string? all, string? name, string? address, string? city, string? zipcode, string? latitude, string? longitude, string? telephone, string? openingHours, string? delivery, string? linkType, string? link)
	{
		var wrapper = new RestaurantWrapper();

		if (all != null)
		{
			name = all;
			address = all;
			city = all;
			zipcode = all;
			latitude = all;
			longitude = all;
			telephone = all;
			openingHours = all;
			delivery = all;
			linkType = all;
			link = all;
		}
		
		await foreach (var r in db.GetRestaurants(name, address, city, zipcode, latitude, longitude, telephone, openingHours, delivery, linkType, link))
		{
			wrapper.Restaurants.Add(r.ToJsonLd());
		}

		return Ok(new ApiResponseWrapper("OK", "Fetched restaurant objects", JsonConvert.SerializeObject(wrapper)));
	}

	/// <summary>
	/// Returns a specific restaurant
	/// </summary>
	/// <param name="id">ID of the restaurant that is to be returned</param>
	/// <response code="200">Restaurant fetched</response>
	/// <response code="404">Restaurant with specified ID not found</response>
	[HttpGet("{id}")]
	[Produces("application/json")]
	public async Task<ActionResult<ApiResponseWrapper>> GetRestaurant(int id)
	{
		var r = await db.GetRestaurantById(id);
		if (r == null) return NotFound(new ApiResponseWrapper("Not Found", "Restaurant with specified id not found", null));
		var wrapper = new RestaurantWrapper();
		wrapper.Restaurants.Add(r.ToJsonLd());
		return Ok(new ApiResponseWrapper("OK", "Fetched restaurant with specified id", JsonConvert.SerializeObject(wrapper)));
	}

	/// <summary>
	/// Creates a new restaurant
	/// </summary>
	/// <param name="restaurant">Restaurant data</param>
	/// <response code="201">Restaurant created</response>
	/// <response code="400">Generic error</response>
	[HttpPost]
	[Produces("application/json")]
	public async Task<ActionResult<ApiResponseWrapper>> PostRestaurant(NewRestaurantDTO restaurant)
	{
		try
		{
			var r = await db.PostRestaurant(restaurant);
			var wrapper = new RestaurantWrapper();
			wrapper.Restaurants.Add(r.ToJsonLd());
			return StatusCode(201, new ApiResponseWrapper("CREATED", "Added new restaurant", JsonConvert.SerializeObject(wrapper)));
		}
		catch (Exception ex)
		{
			return new ErrorHandler().Parse(ex);
		}
	}

	/// <summary>
	/// Creates or updates a restaurant with a specific ID
	/// </summary>
	/// <param name="id">ID of the restaurant to be updated</param>
	/// <param name="restaurant">Restaurant data</param>
	/// <response code="200">Restaurant created/updated</response>
	/// <response code="400">Generic error</response>
	[HttpPut("{id}")]
	[Produces("application/json")]
	public async Task<ActionResult<ApiResponseWrapper>> PutRestaurant(int id, NewRestaurantDTO restaurant)
	{
		return await PutRestaurant(new RestaurantDTO(id, restaurant.Name, restaurant.Address, restaurant.Zipcode, restaurant.Latitude, restaurant.Longitude, restaurant.Telephone, restaurant.OpeningHours, restaurant.Delivery, restaurant.City));
	}

	/// <summary>
	/// Creates or updates a restaurant
	/// </summary>
	/// <param name="restaurant">Restaurant to be created or updated</param>
	/// <response code="200">Restaurant updated</response>
	/// <response code="201">Restaurant created</response>
	/// <response code="400">Generic error</response>
	[HttpPut]
	[Produces("application/json")]
	public async Task<ActionResult<ApiResponseWrapper>> PutRestaurant(RestaurantDTO restaurant)
	{
		try
		{
			//Create new restaurant
			if (restaurant.Id == null) return await PostRestaurant(restaurant);

			//Upate existing restaurant
			var r = await db.GetRestaurantById((int)restaurant.Id);
			if (r == null) return NotFound(new ApiResponseWrapper("Not Found", "Restaurant with specified id not found", null));
			r = await db.PutRestaurant(restaurant);
			var wrapper = new RestaurantWrapper();
			wrapper.Restaurants.Add(r.ToJsonLd());
			return Ok(new ApiResponseWrapper("OK", "Updated restaurant", JsonConvert.SerializeObject(wrapper)));
		}
		catch (Exception ex)
		{
			return new ErrorHandler().Parse(ex);
		}
	}

	/// <summary>
	/// Deletes the specified restaurant
	/// </summary>
	/// <param name="id">ID of the restaurant to be deleted</param>
	/// <response code="200">Restaurant deleted/updated</response>
	/// <response code="404">Restaurant with specified ID not found</response>
	[HttpDelete("{id}")]
	[Produces("application/json")]
	public async Task<ActionResult<ApiResponseWrapper>> DeleteRestaurant(int id)
	{
		try
		{
			await db.DeleteRestaurant(id);
			return Ok(new ApiResponseWrapper("OK", "Deleted restaurant", null));
		}
		catch (Exception ex)
		{
			return new ErrorHandler().Parse(ex);
		}
	}
}