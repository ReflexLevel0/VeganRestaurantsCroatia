using api.Models;
using Npgsql;

namespace api;

public class PostgreDb : IDb
{
	private readonly NpgsqlDataSource _dataSource;

	private readonly string _getRestaurantsQuery = $"SELECT r.id, r.name, address, zipcode, latitude, longitude, phone, opening_hours, delivery, c.name as cityName " +
	                                              "FROM restaurant r JOIN city c ON r.cityId=c.id";
	
	public PostgreDb(string connString)
	{
		var dataSourceBuilder = new NpgsqlDataSourceBuilder(connString);
		_dataSource = dataSourceBuilder.Build();
	}

	public async Task<NpgsqlConnection> OpenConnectionAsync() => await _dataSource.OpenConnectionAsync();

	public async IAsyncEnumerable<RestaurantDTO> GetRestaurants()
	{
		await using var cmd = _dataSource.CreateCommand(_getRestaurantsQuery);
		await using var reader = await cmd.ExecuteReaderAsync();
		while (await reader.ReadAsync())
		{
			yield return ReaderToRestaurant(reader);
		}
	}

	public async Task<RestaurantDTO?> GetRestaurantById(int id)
	{
		await using var cmd = _dataSource.CreateCommand($"{_getRestaurantsQuery} WHERE r.id = {id}");
		await using var reader = await cmd.ExecuteReaderAsync();
		while (await reader.ReadAsync())
		{
			return ReaderToRestaurant(reader);
		}

		return null;
	}
	
	public async Task<RestaurantDTO?> GetRestaurantByName(string name)
	{
		await using var cmd = _dataSource.CreateCommand($"{_getRestaurantsQuery} WHERE r.name = '{name}'");
		await using var reader = await cmd.ExecuteReaderAsync();
		while (await reader.ReadAsync())
		{
			return ReaderToRestaurant(reader);
		}

		return null;
	}

	public async Task<RestaurantDTO> PostRestaurant(NewRestaurantDTO restaurant)
	{
		//Inserting restaurant into the database
		string insertQuery = $"INSERT INTO restaurant(name,address,zipcode,latitude,longitude,phone,opening_hours,delivery,cityid) " +
		                     $"VALUES('{restaurant.Name}', '{restaurant.Address}', {restaurant.Zipcode}, {restaurant.Latitude}, {restaurant.Longitude}, {StringToSqlString(restaurant.Phone)}, {StringToSqlString(restaurant.OpeningHours)}, {restaurant.Delivery}, {await GetCityId(restaurant.City)})";
		await using var cmd = _dataSource.CreateCommand(insertQuery);
		int changedRows = await cmd.ExecuteNonQueryAsync();
		
		//Checking if insertion has gone through
		var r = await GetRestaurantByName(restaurant.Name);
		if (changedRows == 0 || r == null) throw new Exception("restaurant not found");
		return r;
	}
	
	public async Task<RestaurantDTO> PutRestaurant(RestaurantDTO restaurant)
	{
		//Inserting the restaurant into the database if ID isn't specified
		if (restaurant.Id == null) return await PostRestaurant(restaurant);
		
		//Updating the existing restaurant if ID is specified
		string updateQuery = $"UPDATE restaurant SET " +
		                     $"name={StringToSqlString(restaurant.Name)}," +
		                     $"address={StringToSqlString(restaurant.Address)}," +
		                     $"zipcode={restaurant.Zipcode}," +
		                     $"latitude={restaurant.Latitude}," +
		                     $"longitude={restaurant.Longitude}," +
		                     $"phone={StringToSqlString(restaurant.Phone)}," +
		                     $"opening_hours={StringToSqlString(restaurant.OpeningHours)}," +
		                     $"delivery={restaurant.Delivery}," +
		                     $"cityId={await GetCityId(restaurant.City)} " +
		                     $"WHERE id={restaurant.Id}";
		await using var cmd = _dataSource.CreateCommand(updateQuery);
		int changedRows = await cmd.ExecuteNonQueryAsync();
		
		//Checking if restaurant exists
		var r = await GetRestaurantByName(restaurant.Name);
		if (changedRows == 0 || r == null) throw new Exception("restaurant not found");
		return r;
	}
	
	public async Task DeleteRestaurant(int id)
	{
		string deleteQuery = $"DELETE FROM restaurant WHERE id={id}";
		await using var cmd = _dataSource.CreateCommand(deleteQuery); 
		int rowsAffected = await cmd.ExecuteNonQueryAsync();
		if(rowsAffected == 0) throw new Exception("id not found");
	}

	private static RestaurantDTO ReaderToRestaurant(NpgsqlDataReader reader)
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
		return new RestaurantDTO(id, name, address, zipcode, latitude, longitude, phone, openingHours, delivery, cityName);
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

	private string StringToSqlString(string? value) => value == null ? "null" : $"'{value}'";
}