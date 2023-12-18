using api.Models;
using Npgsql;

namespace api;

public class PostgreDb : IDb
{
	private readonly NpgsqlDataSource _dataSource;

	private readonly string _getRestaurantsQuery = $"SELECT r.id, r.name, address, zipcode, latitude, longitude, phone, opening_hours, delivery, c.name as cityName " +
	                                              "FROM restaurant r JOIN city c ON r.cityId=c.id";
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

	public async Task<Restaurant?> GetRestaurantById(int id)
	{
		await using var cmd = _dataSource.CreateCommand($"{_getRestaurantsQuery} WHERE r.id = {id}");
		await using var reader = await cmd.ExecuteReaderAsync();
		while (await reader.ReadAsync())
		{
			return ReaderToRestaurant(reader);
		}

		return null;
	}
	
	public async Task<Restaurant?> GetRestaurantByName(string name)
	{
		await using var cmd = _dataSource.CreateCommand($"{_getRestaurantsQuery} WHERE r.name = '{name}'");
		await using var reader = await cmd.ExecuteReaderAsync();
		while (await reader.ReadAsync())
		{
			return ReaderToRestaurant(reader);
		}

		return null;
	}

	public async Task<Restaurant> PostRestaurant(RestaurantBase restaurant)
	{
		await using var cmd = _dataSource.CreateCommand(await CreateRestaurantInsertQuery(restaurant));
		await cmd.ExecuteNonQueryAsync();
		var r = await GetRestaurantByName(restaurant.Name);
		if (r == null) throw new Exception("Restaurant object not added to database");
		return r;
	}
	
	public async Task<Restaurant> PutRestaurant(Restaurant restaurant)
	{
		string insertQuery = await CreateRestaurantInsertQuery(restaurant) + " ON CONFLICT(id) DO UPDATE SET " +
		                     $"name='{restaurant.Name}',address='{restaurant.Address}',cityid={await GetCityId(restaurant.City)},zipcode={restaurant.Zipcode},latitude={restaurant.Latitude},longitude={restaurant.Longitude},phone='{restaurant.Phone}',opening_hours='{restaurant.OpeningHours}',delivery={restaurant.Delivery}";
		await using var cmd = _dataSource.CreateCommand(insertQuery);
		await cmd.ExecuteNonQueryAsync();
		var r = await GetRestaurantByName(restaurant.Name);
		if (r == null) throw new Exception("Restaurant object not added to database");
		return r;
	}
	
	public async Task DeleteRestaurant(int id)
	{
		string deleteQuery = $"DELETE FROM restaurant WHERE id={id}";
		await using var cmd = _dataSource.CreateCommand(deleteQuery); 
		int rowsAffected = await cmd.ExecuteNonQueryAsync();
		if(rowsAffected == 0) throw new Exception("id not found");
	}

	private static Restaurant ReaderToRestaurant(NpgsqlDataReader reader)
	{
		int id = reader.GetInt32(0);
		string name = reader.GetString(1);
		string address = reader.GetString(2);
		int zipcode = reader.GetInt32(3);
		double latitude = reader.GetDouble(4);
		double longitude = reader.GetDouble(5);
		object phoneObject = reader.GetValue(6);
		string? phone = phoneObject is DBNull ? null : (string)phoneObject;
		object openingHoursObject = reader.GetValue(7);
		string? openingHours = openingHoursObject is DBNull ? null : (string)openingHoursObject;
		bool delivery = reader.GetBoolean(8);
		string cityName = reader.GetString(9);
		return new Restaurant(id, name, address, zipcode, latitude, longitude, phone, openingHours, delivery, cityName);
	}

	private async Task<string> CreateRestaurantInsertQuery(RestaurantBase restaurant)
	{
		var fullRestaurant = restaurant is Restaurant ? (Restaurant)restaurant : null;
		return $"INSERT INTO restaurant({(fullRestaurant == null ? "" : "id,")}name,address,zipcode,latitude,longitude,phone,opening_hours,delivery,cityid) " +
		       $"VALUES({(fullRestaurant == null ? "" : $"{fullRestaurant.Id},")}'{restaurant.Name}', '{restaurant.Address}', {restaurant.Zipcode}, {restaurant.Latitude}, {restaurant.Longitude}, {(restaurant.Phone == null ? "null" : $"'{restaurant.Phone}'")}, {(restaurant.OpeningHours == null ? "null" : $"'{restaurant.OpeningHours}'")}, {restaurant.Delivery}, {await GetCityId(restaurant.City)})";
	}

	private async Task<int> GetCityId(string name)
	{
		await using var cmd = _dataSource.CreateCommand($"SELECT id FROM city WHERE name='{name}'");
		object? reader = await cmd.ExecuteScalarAsync();
		if (reader != null) return (int)reader;
		await using var insertCmd = _dataSource.CreateCommand($"INSERT INTO city(name) VALUES('{name}')");
		await insertCmd.ExecuteNonQueryAsync();
		return await GetCityId(name);
	}
}