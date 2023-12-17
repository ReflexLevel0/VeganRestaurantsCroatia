using api.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace api.Controllers;

[Route("[controller]")]
[ApiController]
public class CityController(IDb db) : ControllerBase
{
	private ActionResult<ApiResponseWrapper> BadRequestError => StatusCode(400, new ApiResponseWrapper("Bad Request", "Error happened in processing your request", null));
	
	/// <summary>
	/// Returns all cities
	/// </summary>
	/// <response code="200">Cities fetched</response>
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
	
	/// <summary>
	/// Returns a specific city
	/// </summary>
	/// <param name="id">ID of the city that is to be returned</param>
	/// <response code="200">City fetched</response>
	/// <response code="404">City with specified ID not found</response>
	[HttpGet("{id}")]
	public async Task<ActionResult<ApiResponseWrapper>> GetCity(int id)
	{
		var city = await db.GetCity(id);
		return city == null ? 
			NotFound(new ApiResponseWrapper("Not Found", "City with specified id not found", null)) : 
			Ok(new ApiResponseWrapper("OK", "Fetched city with specified id", JsonConvert.SerializeObject(city)));
	}
	
	/// <summary>
	/// Creates a new city
	/// </summary>
	/// <param name="city">City data</param>
	/// <response code="200">City created</response>
	/// <response code="400">Generic error</response>
	/// <response code="409">Tried to insert city with ID that already exists</response>
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
	
	/// <summary>
	/// Creates or updates a city
	/// </summary>
	/// <param name="city">City to be created or updated</param>
	/// <response code="200">City created/updated</response>
	/// <response code="400">Generic error</response>
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
	
	/// <summary>
	/// Deletes the specified city
	/// </summary>
	/// <param name="id">ID of the city to be deleted</param>
	/// <response code="200">City deleted/updated</response>
	/// <response code="400">Generic error</response>
	/// <response code="404">City with specified ID not found</response>
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