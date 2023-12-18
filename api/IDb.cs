using api.Models;

namespace api;

public interface IDb
{
	IAsyncEnumerable<Restaurant> GetRestaurants();
	Task<Restaurant?> GetRestaurant(int id);
	Task PostRestaurant(RestaurantBase restaurant);
	Task PutRestaurant(Restaurant restaurant);
	Task DeleteRestaurant(int id);
}