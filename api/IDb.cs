using api.Models;

namespace api;

public interface IDb
{
	IAsyncEnumerable<Restaurant> GetRestaurants();
	Task<Restaurant?> GetRestaurant(int id);
	Task PostRestaurant(Restaurant restaurant);
	Task PutRestaurant(Restaurant restaurant);
	Task DeleteRestaurant(int id);
	IAsyncEnumerable<City> GetCities();
	Task<City?> GetCity(int id);
	Task PostCity(City city);
	Task PutCity(City city);
	Task DeleteCity(int id);
}