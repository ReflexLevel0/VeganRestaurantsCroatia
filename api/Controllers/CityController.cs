using api.Models;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Route("[controller]")]
[ApiController]
public class CityController(IDb db) : ControllerBase
{
	[HttpGet]
	public async Task<ActionResult<IEnumerable<City>>> GetCities()
	{
		var cities = new List<City>();
		await foreach (var city in db.GetCities())
		{
			cities.Add(city);
		}

		return cities;
	}
	
	[HttpGet("{id}")]
	public async Task<ActionResult<IEnumerable<City>>> GetCity(int id)
	{
		var city = await db.GetCity(id);
		return city == null ? NotFound($"cityId = {id}") : Ok(city);
	}
}