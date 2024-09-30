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
		string fileName = "veganRestaurants.json";
		await fileHelper.RefreshJsonFile($"../{fileName}", db);
		byte[] fileBytes = await GetFileContent($"../{fileName}");
		return File(fileBytes, "application/json", fileName);
	}
	
	[HttpGet("csv")]
	[Produces("text/csv")]
	public async Task<ActionResult<ApiResponseWrapper>> GetCsv()
	{
		string fileName = "veganRestaurants.csv";
		await fileHelper.RefreshCsvFile($"../{fileName}", db);
		byte[] fileBytes = await GetFileContent($"../{fileName}");
		return File(fileBytes, "text/csv", fileName);
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