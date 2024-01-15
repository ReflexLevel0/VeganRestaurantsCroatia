using api.Models;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Route("[controller]")]
[ApiController]
public class FileController(IDb db, FileHelper fileHelper) : ControllerBase
{
	[HttpGet("json")]
	[Produces("application/json")]
	public async Task<ActionResult<ApiResponseWrapper>> GetJson()
	{
		await fileHelper.RefreshJsonFile("../veganRestaurants.json", db);
		byte[] fileBytes = await GetFileContent("../veganRestaurants.json");
		return File(fileBytes, "application/json");
	}
	
	[HttpGet("csv")]
	[Produces("text/csv")]
	public async Task<ActionResult<ApiResponseWrapper>> GetCsv()
	{
		await fileHelper.RefreshCsvFile("../veganRestaurants.csv", db);
		byte[] fileBytes = await GetFileContent("../veganRestaurants.csv");
		return File(fileBytes, "text/csv");
	}
	
	[HttpGet("filteredJson")]
	[Produces("application/json")]
	public async Task<ActionResult<ApiResponseWrapper>> GetFilteredJson()
	{
		byte[] fileBytes = await GetFileContent("/tmp/veganRestaurants.json");
		return File(fileBytes, "application/json");
	}
	
	[HttpGet("filteredCsv")]
	[Produces("text/csv")]
	public async Task<ActionResult<ApiResponseWrapper>> GetFilteredCsv()
	{
		byte[] fileBytes = await GetFileContent("/tmp/veganRestaurants.csv");
		return File(fileBytes, "text/csv");
	}

	private async Task<byte[]> GetFileContent(string filePath)
	{
		return await System.IO.File.ReadAllBytesAsync(filePath);
	}
}