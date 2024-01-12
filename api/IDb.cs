using api.Models;

namespace api;

public interface IDb
{
	IAsyncEnumerable<RestaurantWithLinks> GetRestaurants(string? name, string? address, string? city, string? zipcode, string? latitude, string? longitude, string? telephone, string? openingHours, string? delivery, string? linkType, string? link);
	Task<RestaurantDTO?> GetRestaurantById(int id);
	Task<RestaurantDTO?> GetRestaurantByName(string name);
	Task<RestaurantDTO> PostRestaurant(NewRestaurantDTO restaurant);
	Task<RestaurantDTO> PutRestaurant(RestaurantDTO restaurant);
	Task DeleteRestaurant(int id);
	IAsyncEnumerable<LinkDTO> GetLinks(int? restaurantId = null, string? type = null);
	Task<LinkDTO> PostLink(LinkDTO link);
	Task<LinkDTO> PutLink(LinkDTO link);
	Task DeleteLink(DeleteLinkDTO link);
}