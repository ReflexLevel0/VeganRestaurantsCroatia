namespace api.Models;

public class City(int id, string name)
{
	/// <summary>
	/// Unique identifier of the city
	/// </summary>
	/// <example>1</example>
	public int Id { get; set; } = id;
	/// <summary>
	/// City name
	/// </summary>
	/// <example>Zagreb</example>
	public string Name { get; set; } = name;
}