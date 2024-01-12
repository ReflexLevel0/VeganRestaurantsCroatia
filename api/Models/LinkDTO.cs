using Newtonsoft.Json;

namespace api.Models;

public class LinkDTO(int restaurantId, string linkType, string link)
{
	/// <summary>
	/// ID of the restaurant to which this link belongs to
	/// </summary>
	/// <example>2</example>
	[JsonIgnore]
	public int RestaurantId { get; set; } = restaurantId;
	
	/// <summary>
	/// Type of link
	/// </summary>
	/// <example>instagram</example>
	[JsonProperty(PropertyName = "type")]
	public string LinkType { get; set; } = linkType;
	
	/// <summary>
	/// Actual link value
	/// </summary>
	/// <example>instagram.com/vegehop_restaurant</example>
	[JsonProperty(PropertyName = "link")]
	public string Link { get; set; } = link;
}