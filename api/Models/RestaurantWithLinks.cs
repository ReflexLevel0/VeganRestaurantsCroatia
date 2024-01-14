using Newtonsoft.Json;

namespace api.Models;

public class RestaurantWithLinks(int? id, string name, string address, int zipcode, double latitude, double longitude, string? telephone, string? openingHours, bool delivery, string city, List<LinkDTO> websiteLinks) : RestaurantDTO(id, name, address, zipcode, latitude, longitude, telephone, openingHours, delivery, city)
{
	[JsonProperty(PropertyName = "websitelinks")]
	public List<LinkDTO>? WebsiteLinks { get; set; } = websiteLinks;
	
	public RestaurantJsonld ToJsonLd()
	{
		return new RestaurantJsonld(Id, Name, Address, Zipcode, Latitude, Longitude, Telephone, OpeningHours, Delivery, City, WebsiteLinks);
	}
}