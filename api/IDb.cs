using api.Models;

namespace api;

public interface IDb
{
	IAsyncEnumerable<RestaurantDTO> GetRestaurants();
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