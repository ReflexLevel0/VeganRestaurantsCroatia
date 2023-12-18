using api.Models;

namespace api;

public interface IDb
{
	/// <summary>
	/// Returns all restaurants
	/// </summary>
	IAsyncEnumerable<RestaurantDTO> GetRestaurants();
	
	/// <summary>
	/// Returns the restaurant with the specified ID
	/// </summary>
	/// <param name="id">ID of the restaurant that is returned</param>
	Task<RestaurantDTO?> GetRestaurantById(int id);

	/// <summary>
	/// Returns the restaurant with the specified name
	/// </summary>
	/// <param name="name">Name of the restaurant to be returned</param>
	Task<RestaurantDTO?> GetRestaurantByName(string name);
	
	/// <summary>
	/// Creates a new restaurant
	/// </summary>
	/// <param name="restaurant">Restaurant data</param>
	/// <returns>Restaurant data fetched from database</returns>
	Task<RestaurantDTO> PostRestaurant(NewRestaurantDTO restaurant);

	/// <summary>
	/// Creates a new restaurant or updates an existing one
	/// </summary>
	/// <param name="restaurant">Updated restaurant data</param>
	/// <returns>Restaurant data fetched from database</returns>
	Task<RestaurantDTO> PutRestaurant(RestaurantDTO restaurant);
	
	/// <summary>
	/// Deletes the restaurant with the specified ID
	/// </summary>
	/// <param name="id">ID of the restaurant that is to be deleted</param>
	Task DeleteRestaurant(int id);
}