using Newtonsoft.Json;

namespace api.Models;

public class RestaurantDTO(int? id, string name, string address, int zipcode, double latitude, double longitude, string? telephone, string? openingHours, bool delivery, string city)
	: NewRestaurantDTO(name, address, zipcode, latitude, longitude, telephone, openingHours, delivery, city)
{
	/// <summary>
	/// Unique identifier of the restaurant
	/// </summary>
	/// <example>1</example>
	[JsonProperty(PropertyName = "id")]
	public int? Id { get; set; } = id;

	public RestaurantJsonld ToJsonLd()
	{
		return new RestaurantJsonld(Id, Name, Address, Zipcode, Latitude, Longitude, Telephone, OpeningHours, Delivery, City);
	}
}