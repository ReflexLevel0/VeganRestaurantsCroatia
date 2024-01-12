using Newtonsoft.Json;

namespace api.Models;

public class NewRestaurantDTO(string name, string address, int zipcode, double latitude, double longitude, string? telephone, string? openingHours, bool delivery, string city)
{
	/// <summary>
	/// Restaurant name
	/// </summary>
	/// <example>Restoran Vegehop</example>
	[JsonProperty(PropertyName = "name")]
	public string Name { get; set; } = name;
	
	/// <summary>
	/// Restaurant address
	/// </summary>
	/// <example>Vlaška ulica 79</example>
	[JsonProperty(PropertyName = "address")]
	public string Address { get; set; } = address;
	
	/// <summary>
	/// Zipcode of restaurants location
	/// </summary>
	/// <example>10000</example>
	[JsonProperty(PropertyName = "zipcode")]
	public int Zipcode { get; set; } = zipcode;
	
	/// <summary>
	/// Location latitude
	/// </summary>
	/// <example>45.81462343</example>
	[JsonProperty(PropertyName = "latitude")]
	public double Latitude { get; set; } = latitude;
	
	/// <summary>
	/// Location longitude
	/// </summary>
	/// <example>15.98823879</example>
	[JsonProperty(PropertyName = "longitude")]
	public double Longitude { get; set; } = longitude;
	
	/// <summary>
	/// Phone number used by the restaurant
	/// </summary>
	/// <example>014649400</example>
	[JsonProperty(PropertyName = "telephone")]
	public string? Telephone { get; set; } = telephone;
	
	/// <summary>
	/// Working hours
	/// </summary>
	/// <example>Mon-Sat 12:00-20:00</example>
	[JsonProperty(PropertyName = "openingHours")]
	public string? OpeningHours { get; set; } = openingHours;
	
	/// <summary>
	/// Specifies whether or not restaurant offers delivery services 
	/// </summary>
	/// <example>true</example>
	[JsonProperty(PropertyName = "delivery")]
	public bool Delivery { get; set; } = delivery;
	
	/// <summary>
	/// Name of the city in which the restaurant is located
	/// </summary>
	/// <example>Zagreb</example>
	[JsonProperty(PropertyName = "city")]
	public string City { get; set; } = city;
}