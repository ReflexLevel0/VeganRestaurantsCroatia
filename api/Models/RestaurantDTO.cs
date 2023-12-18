namespace api.Models;

public class RestaurantDTO(int? id, string name, string address, int zipcode, double latitude, double longitude, string? phone, string? openingHours, bool delivery, string city)
	: NewRestaurantDTO(name, address, zipcode, latitude, longitude, phone, openingHours, delivery, city)
{
	/// <summary>
	/// Unique identifier of the restaurant
	/// </summary>
	/// <example>1</example>
	public int? Id { get; set; } = id;
}