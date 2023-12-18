namespace api.Models;

public class DeleteLinkDTO(int restaurantId, string linkType)
{
	public int RestaurantId { get; set; } = restaurantId;
	public string LinkType { get; set; } = linkType;
}