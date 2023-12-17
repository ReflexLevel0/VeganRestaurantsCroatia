using api.Models;

namespace api;

public interface IDb
{
	IAsyncEnumerable<Restaurant> GetRestaurants();
	Task<Restaurant?> GetRestaurant(int id);
	IAsyncEnumerable<City> GetCities();
	Task<City?> GetCity(int id);
}