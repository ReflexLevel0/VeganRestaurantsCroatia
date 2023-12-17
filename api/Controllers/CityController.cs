using api.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace api.Controllers;

[Route("[controller]")]
[ApiController]
public class CityController(IDb db) : ControllerBase
{
	private ActionResult<ApiResponseWrapper> BadRequestError => StatusCode(400, new ApiResponseWrapper("Bad Request", "Error happened in processing your request", null));
	
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
	
	[HttpPost]
	public async Task<ActionResult<ApiResponseWrapper>> PostCity(City city)
	{
		try
		{
			await db.PostCity(city);
			return Ok(new ApiResponseWrapper("OK", "Added new city", null));
		}
		catch (Exception ex)
		{
			ControllerHelper.PrintError(ex);
			if (ex.Message.Contains("duplicate key") == false) return BadRequestError;
			return Conflict(new ApiResponseWrapper("CONFLICT", "Object already exists", null));
		}
	}
	
	[HttpPut]
	public async Task<ActionResult<ApiResponseWrapper>> PutCity(City city)
	{
		try
		{
			await db.PutCity(city);
			return Ok(new ApiResponseWrapper("OK", "Updated city", null));
		}
		catch(Exception ex)
		{
			ControllerHelper.PrintError(ex);
			return BadRequestError;
		}
	}
	
	[HttpDelete("{id}")]
	public async Task<ActionResult<ApiResponseWrapper>> DeleteCity(int id)
	{
		try
		{
			await db.DeleteCity(id);
			return Ok(new ApiResponseWrapper("OK", "Deleted city", null));
		}
		catch (Exception ex)
		{
			ControllerHelper.PrintError(ex);
			if (ex.Message.Contains("") == false) return BadRequestError;
			return NotFound(new ApiResponseWrapper("Not Found", "City with specified id not found", null));
		}
	}
}