using Newtonsoft.Json;

namespace api.Models;

public class RestaurantWrapper
{
	[JsonProperty(PropertyName = "@context")]
	public string Context { get; } = "https://schema.org";
	
	[JsonProperty(PropertyName = "@graph")]
	public List<RestaurantJsonld> Restaurants { get; set; } = new();
}