namespace api.Models;

public class DeleteLinkDTO(int restaurantId, string linkType)
{
	/// <summary>
	/// Unique identifier of the restaurant
	/// </summary>
	/// <example>5</example>
	public int RestaurantId { get; set; } = restaurantId;
	
	/// <summary>
	/// Type of the link
	/// </summary>
	/// <example>twitter</example>
	public string LinkType { get; set; } = linkType;
}