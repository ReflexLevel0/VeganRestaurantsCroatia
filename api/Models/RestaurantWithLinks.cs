using Newtonsoft.Json;

namespace api.Models;

public class RestaurantWithLinks(int? id, string name, string address, int zipcode, double latitude, double longitude, string? phone, string? openingHours, bool delivery, string city) : RestaurantDTO(id, name, address, zipcode, latitude, longitude, phone, openingHours, delivery, city)
{
	[JsonProperty(PropertyName = "websitelinks")]
	public List<LinkDTO>? WebsiteLinks { get; set; }
}