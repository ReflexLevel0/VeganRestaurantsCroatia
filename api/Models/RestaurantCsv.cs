namespace api.Models;

public class RestaurantCsv(int? id, string name, string address, int zipcode, double latitude, double longitude, string? phone, string? openingHours, bool delivery, string city, string? linkType, string? link) : RestaurantDTO(id, name, address, zipcode, latitude, longitude, phone, openingHours, delivery, city)
{
	public string? LinkType { get; set; } = linkType;
	public string? Link { get; set; } = link;
}