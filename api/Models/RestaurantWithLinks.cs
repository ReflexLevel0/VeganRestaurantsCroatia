using Newtonsoft.Json;

namespace api.Models;

public class RestaurantWithLinks(int? id, string name, string address, int zipcode, double latitude, double longitude, string? telephone, string? openingHours, bool delivery, string city) : RestaurantDTO(id, name, address, zipcode, latitude, longitude, telephone, openingHours, delivery, city)
{
	[JsonProperty(PropertyName = "websitelinks")]
	public List<LinkDTO>? WebsiteLinks { get; set; }
}