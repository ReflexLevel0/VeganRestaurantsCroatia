namespace api.Models;

public class ApiResponseWrapper(string status, string message, string? response)
{
	public string Status { get; set; } = status;
	public string Message { get; set; } = message;
	public string? Response { get; set; } = response;
}