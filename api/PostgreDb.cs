using api.Models;
using Npgsql;

namespace api;

public class PostgreDb : IDb
{
	private readonly NpgsqlDataSource _dataSource;

	private readonly string _getRestaurantsQuery = "SELECT id, name, address, zipcode, latitude, longitude, phone, opening_hours, delivery, cityId " +
	                                              "FROM restaurant r";
	private readonly string _getCitiesQuery = "SELECT id, name FROM city";
	
	public PostgreDb(string connString)
	{
		var dataSourceBuilder = new NpgsqlDataSourceBuilder(connString);
		_dataSource = dataSourceBuilder.Build();
	}

	public async Task<NpgsqlConnection> OpenConnectionAsync() => await _dataSource.OpenConnectionAsync();

	public async IAsyncEnumerable<Restaurant> GetRestaurants()
	{
		await using var cmd = _dataSource.CreateCommand(_getRestaurantsQuery);
		await using var reader = await cmd.ExecuteReaderAsync();
		while (await reader.ReadAsync())
		{
			yield return ReaderToRestaurant(reader);
		}
	}

	public async Task<Restaurant?> GetRestaurant(int id)
	{
		await using var cmd = _dataSource.CreateCommand($"{_getRestaurantsQuery} WHERE r.id = {id}");
		await using var reader = await cmd.ExecuteReaderAsync();
		while (await reader.ReadAsync())
		{
			return ReaderToRestaurant(reader);
		}

		return null;
	}

	public async IAsyncEnumerable<City> GetCities()
	{
		await using var cmd = _dataSource.CreateCommand(_getCitiesQuery);
		await using var reader = await cmd.ExecuteReaderAsync();
		while (await reader.ReadAsync())
		{
			yield return ReaderToCity(reader);
		}
	}

	public async Task<City?> GetCity(int id)
	{
		await using var cmd = _dataSource.CreateCommand($"{_getCitiesQuery} WHERE id = {id}");
		await using var reader = await cmd.ExecuteReaderAsync();
		while (await reader.ReadAsync())
		{
			return ReaderToCity(reader);
		}

		return null;
	}

	private static Restaurant ReaderToRestaurant(NpgsqlDataReader reader)
	{
		int id = reader.GetInt32(0);
		string name = reader.GetString(1);
		string address = reader.GetString(2);
		int zipcode = reader.GetInt32(3);
		double latitude = reader.GetDouble(4);
		double longitude = reader.GetDouble(5);
		string phone = reader.GetString(6);
		string openingHours = reader.GetString(7);
		bool delivery = reader.GetBoolean(8);
		int cityId = reader.GetInt32(9);
		return new Restaurant(id, name, address, zipcode, latitude, longitude, phone, openingHours, delivery, cityId);
	}

	private static City ReaderToCity(NpgsqlDataReader reader)
	{
		int id = reader.GetInt32(0);
		string name = reader.GetString(1);
		return new City(id, name);
	}
}