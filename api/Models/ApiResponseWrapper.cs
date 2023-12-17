namespace api.Models;

public class ApiResponseWrapper(string status, string message, string? response)
{
	/// <summary>
	/// Status code
	/// </summary>
	/// <example>200</example>
	public string Status { get; set; } = status;
	/// <summary>
	/// Short message specifying what happened
	/// </summary>
	/// <example>Fetched all objects</example>
	public string Message { get; set; } = message;
	/// <summary>
	/// Data in JSON format
	/// </summary>
	public string? Response { get; set; } = response;
}