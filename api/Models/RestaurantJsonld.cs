using Newtonsoft.Json;

namespace api.Models;

public class RestaurantJsonld(int? id, string name, string address, int zipcode, double latitude, double longitude, string? telephone, string? openingHours, bool delivery, string city, List<LinkDTO>? websiteLinks)
{
	[JsonProperty(PropertyName = "@type")]
	public string Type { get; } = "Restaurant";
	
	/// <summary>
	/// Unique identifier of the restaurant
	/// </summary>
	/// <example>1</example>
	[JsonProperty(PropertyName = "id")]
	public string? Id { get; set; } = id.ToString();

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
	public Address Address { get; set; } = new(address, $"{city}, Croatia", zipcode.ToString());

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
	/// A string containing all website links for this restaurant
	/// </summary>
	[JsonProperty(PropertyName = "websiteLinks")]
	public List<LinkDTO>? WebsiteLinks { get; set; } = websiteLinks;
}