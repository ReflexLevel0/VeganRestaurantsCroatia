using api.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace api.Controllers;

[Route("[controller]")]
[ApiController]
public class CityController(IDb db) : ControllerBase
{
	[HttpGet]
	public async Task<ActionResult<ApiResponseWrapper>> GetCities()
	{
		var cities = new List<City>();
		await foreach (var city in db.GetCities())
		{
			cities.Add(city);
		}

		return Ok(new ApiResponseWrapper("OK", "Fetched city objects", JsonConvert.SerializeObject(cities)));
	}
	
	[HttpGet("{id}")]
	public async Task<ActionResult<ApiResponseWrapper>> GetCity(int id)
	{
		var city = await db.GetCity(id);
		return city == null ? 
			NotFound(new ApiResponseWrapper("Not Found", "City with specified id not found", null)) : 
			Ok(new ApiResponseWrapper("OK", "Fetched city with specified id", JsonConvert.SerializeObject(city)));
	}
	
	// [HttpPost]
	// public async Task<ActionResult<ApiResponseWrapper>> PostCity()
	// {
	// 	
	// }
	//
	// [HttpPut]
	// public async Task<ActionResult<ApiResponseWrapper>> PutCity()
	// {
	// 	
	// }
	//
	// [HttpDelete]
	// public async Task<ActionResult<ApiResponseWrapper>> DeleteCity()
	// {
	// 	
	// }
}