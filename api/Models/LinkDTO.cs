namespace api.Models;

public class LinkDTO(int restaurantId, string linkType, string link)
{
	/// <summary>
	/// ID of the restaurant to which this link belongs to
	/// </summary>
	/// <example>2</example>
	public int RestaurantId { get; set; } = restaurantId;
	
	/// <summary>
	/// Type of link
	/// </summary>
	/// <example>instagram</example>
	public string LinkType { get; set; } = linkType;
	
	/// <summary>
	/// Actual link value
	/// </summary>
	/// <example>instagram.com/vegehop_restaurant</example>
	public string Link { get; set; } = link;
}