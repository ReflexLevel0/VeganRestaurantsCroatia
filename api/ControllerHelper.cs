namespace api;

public class ControllerHelper
{
	public static void PrintError(Exception ex)
	{
		Console.WriteLine($"ERROR: {ex.Message}");
	}
}