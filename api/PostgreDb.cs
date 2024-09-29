using api.Models;
using Npgsql;

namespace api;

public class PostgreDb : IDb
{
	private readonly NpgsqlDataSource _dataSource;

	private readonly string _getRestaurantsQuery = $"SELECT r.id, r.name, address, zipcode, latitude, longitude, telephone, opening_hours, delivery, c.name as cityName " +
	                                              "FROM restaurant r JOIN city c ON r.cityId=c.id";
	private readonly string _getLinksQuery = "SELECT restaurantid, l.type, link FROM restaurantLink rl " +
		"JOIN link l ON rl.linktype = l.id";

	private readonly FileHelper _fileHelper;
	
	public PostgreDb(string connString, FileHelper fileHelper)
	{
		var dataSourceBuilder = new NpgsqlDataSourceBuilder(connString);
		_dataSource = dataSourceBuilder.Build();
		_fileHelper = fileHelper;
	}

	public async Task<NpgsqlConnection> OpenConnectionAsync() => await _dataSource.OpenConnectionAsync();

	public async IAsyncEnumerable<RestaurantWithLinks> GetRestaurants(string? name, string? address, string? city, string? zipcode, string? latitude, string? longitude, string? telephone, string? openingHours, string? delivery, string? linkType, string? link)
	{
		string query = _getRestaurantsQuery + " WHERE";
		if (name != null) query += $" LOWER(r.name) LIKE LOWER('%' || '{name}' || '%') OR";
		if (address != null) query += $" LOWER(address) LIKE LOWER('%' || '{address}' || '%') OR";
		if (city != null) query += $" LOWER(c.name) LIKE LOWER('%' || '{city}' || '%') OR";
		if (zipcode != null) query += $" LOWER(zipcode::varchar(256)) LIKE LOWER('%' || '{zipcode}' || '%') OR";
		if (latitude != null) query += $" LOWER(latitude::varchar(256)) LIKE LOWER('%' || '{latitude}' || '%') OR";
		if (longitude != null) query += $" LOWER(longitude::varchar(256)) LIKE LOWER('%' || '{longitude}' || '%') OR";
		if (delivery != null) query += $" LOWER(delivery::varchar(256)) LIKE LOWER('%' || '{delivery}' || '%') OR";
		if (telephone != null) query += $" LOWER(telephone) LIKE LOWER('%' || '{telephone}' || '%') OR";
		if (openingHours != null) query += $" LOWER(opening_hours) LIKE LOWER('%' || '{openingHours}' || '%')";
		query = query.Replace('\n', ' ').Trim();
		if (query.EndsWith("OR")) query = query[..^2];
		if (query.EndsWith("WHERE")) query = query.Replace("WHERE", "");

		List<RestaurantWithLinks> restaurants;
		await using (var cmd = _dataSource.CreateCommand(query))
		{
			await using (var reader = await cmd.ExecuteReaderAsync())
			{
				restaurants = new List<RestaurantWithLinks>();
				while (await reader.ReadAsync())
				{
					var r = ReaderToRestaurant(reader);
					var restaurant = await RestaurantToRestaurantWithLinks(r);
					restaurants.Add(restaurant);
				}
			}
		}

		await _fileHelper.RefreshJsonFile(restaurants, "/tmp/veganRestaurants.json");
		await _fileHelper.RefreshCsvFile(restaurants, "/tmp/veganRestaurants.csv");
        
		foreach (var r in restaurants)
		{
			yield return r;
		}
	}

	public async Task<RestaurantWithLinks?> GetRestaurantById(int id)
	{
		await using var cmd = _dataSource.CreateCommand($"{_getRestaurantsQuery} WHERE r.id = {id}");
		await using var reader = await cmd.ExecuteReaderAsync();
		while (await reader.ReadAsync())
		{
			var r = ReaderToRestaurant(reader);
			return await RestaurantToRestaurantWithLinks(r);
		}

		return null;
	}
	
	public async Task<RestaurantWithLinks?> GetRestaurantByName(string name)
	{
		await using var cmd = _dataSource.CreateCommand($"{_getRestaurantsQuery} WHERE r.name = '{name}'");
		await using var reader = await cmd.ExecuteReaderAsync();
		while (await reader.ReadAsync())
		{
			var r = ReaderToRestaurant(reader);
			return await RestaurantToRestaurantWithLinks(r);
		}

		return null;
	}

	public async Task<RestaurantWithLinks> PostRestaurant(NewRestaurantDTO restaurant)
	{
		//Inserting restaurant into the database
		string insertQuery = $"INSERT INTO restaurant(name,address,zipcode,latitude,longitude,telephone,opening_hours,delivery,cityid) " +
		                     $"VALUES('{restaurant.Name}', '{restaurant.Address}', {restaurant.Zipcode}, {restaurant.Latitude}, {restaurant.Longitude}, {StringToSqlString(restaurant.Telephone)}, {StringToSqlString(restaurant.OpeningHours)}, {restaurant.Delivery}, {await GetCityId(restaurant.City)})";
		await using var cmd = _dataSource.CreateCommand(insertQuery);
		int changedRows = await cmd.ExecuteNonQueryAsync();
		
		//Checking if insertion has gone through
		var r = await GetRestaurantByName(restaurant.Name);
		if (changedRows == 0 || r == null) throw new Exception("restaurant not found");
		return r;
	}
	
	public async Task<RestaurantWithLinks> PutRestaurant(RestaurantDTO restaurant)
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
		                     $"telephone={StringToSqlString(restaurant.Telephone)}," +
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

	public async IAsyncEnumerable<LinkDTO> GetLinks(int? restaurantId, string? type)
	{
		//Checking if restaurant exists
		if (restaurantId != null)
		{
			var r = await GetRestaurantById((int)restaurantId);
			if (r == null) throw new Exception("Restaurant id not found");
		}

		//Checking if link type exists (will throw exception if it doesn't exist)
		if(type != null) await GetLinkTypeId(type, false);
		
		await using var cmd = _dataSource.CreateCommand(_getLinksQuery + 
		                                                $" WHERE restaurantid::text LIKE '{(restaurantId == null ? "%%" : restaurantId)}'" +
		                                                $" AND type::text LIKE '{(type == null ? "%%" : type)}'");
		await using var reader = await cmd.ExecuteReaderAsync();
		while (await reader.ReadAsync())
		{
			yield return ReaderToLink(reader);
		}
	}

	private async IAsyncEnumerable<LinkDTO> PrivateGetLinks(int? restaurantId, string? type)
	{
		await using var cmd = _dataSource.CreateCommand(_getLinksQuery + 
		                                                $" WHERE restaurantid::text LIKE '{(restaurantId == null ? "%%" : restaurantId)}'" +
		                                                $" AND type::text LIKE '{(type == null ? "%%" : type)}'");
		await using var reader = await cmd.ExecuteReaderAsync();
		while (await reader.ReadAsync())
		{
			yield return ReaderToLink(reader);
		}
	}

	public async Task<LinkDTO> PostLink(LinkDTO link)
	{
		//Inserting link into the database
		await using var cmd = _dataSource.CreateCommand(
			"INSERT INTO restaurantLink(restaurantid, linktype, link) " +
			$"VALUES({link.RestaurantId},{await GetLinkTypeId(link.LinkType, true)},'{link.Link}')");
		int changedRows = await cmd.ExecuteNonQueryAsync();
		if (changedRows == 0) throw new Exception("Link not found");
		
		//Getting link from the database
		await foreach (var l in GetLinks(link.RestaurantId, link.LinkType))
		{
			return l;
		}

		//Throwing an exception if link wasn't inserted into the database
		throw new Exception("Link not found");
	}

	public async Task<LinkDTO> PutLink(LinkDTO link)
	{
		//Checking if link already exists
		bool foundLink = false;
		await foreach (var l in GetLinks(link.RestaurantId, link.LinkType))
		{
			foundLink = true;
			break;
		}

		//Inserting the link if one doesnt exist
		if (foundLink == false)
		{
			return await PostLink(link);
		}
		
		//Inserting the link (or updating if it conflicts with an existing link)
		int linkTypeId = await GetLinkTypeId(link.LinkType, true);
		string updateQuery = $"INSERT INTO restaurantlink(restaurantid,linktype,link) " +
		                     $"VALUES({link.RestaurantId},{linkTypeId},'{link.Link}') " +
		                     $"ON CONFLICT ON CONSTRAINT restaurantlink_pkey DO " +
		                     $"UPDATE SET link='{link.Link}'";
		await using var cmd = _dataSource.CreateCommand(updateQuery);
		int changedRows = await cmd.ExecuteNonQueryAsync();
		
		//Checking if link exists
		LinkDTO? result = null;
		await foreach (var l in GetLinks(link.RestaurantId, link.LinkType))
		{
			result = l;
			break;
		}
		if (changedRows == 0 || result == null) throw new Exception("restaurant not found");
		return result;
	}

	public async Task DeleteLink(DeleteLinkDTO link)
	{
		string deleteQuery = $"DELETE FROM restaurantlink WHERE restaurantid={link.RestaurantId} AND linktype={await GetLinkTypeId(link.LinkType, false)}";
		await using var cmd = _dataSource.CreateCommand(deleteQuery); 
		int rowsAffected = await cmd.ExecuteNonQueryAsync();
		if(rowsAffected == 0) throw new Exception("link not found");
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
		string? telephone = phoneObject is DBNull ? null : (string)phoneObject;
		object openingHoursObject = reader.GetValue(7);
		string? openingHours = openingHoursObject is DBNull ? null : (string)openingHoursObject;
		bool delivery = reader.GetBoolean(8);
		string cityName = reader.GetString(9);
		return new RestaurantDTO(id, name, address, zipcode, latitude, longitude, telephone, openingHours, delivery, cityName);
	}

	private LinkDTO ReaderToLink(NpgsqlDataReader reader)
	{
		int restaurantId = reader.GetInt32(0);
		string type = reader.GetString(1);
		string link = reader.GetString(2);
		return new LinkDTO(restaurantId, type, link);
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

	private async Task<int> GetLinkTypeId(string type, bool createNewIfNotFound)
	{
		await using var cmd = _dataSource.CreateCommand($"SELECT id FROM link WHERE type='{type}'");
		object? reader = await cmd.ExecuteScalarAsync();
		if (reader != null) return (int)reader;
		if (createNewIfNotFound == false) throw new Exception("Link type not found");
		await using var insertCmd = _dataSource.CreateCommand($"INSERT INTO link(type) VALUES('{type}')");
		await insertCmd.ExecuteNonQueryAsync();
		return await GetLinkTypeId(type, createNewIfNotFound);
	}

	private string StringToSqlString(string? value) => value == null ? "null" : $"'{value}'";
	
	private async Task<RestaurantWithLinks> RestaurantToRestaurantWithLinks(RestaurantDTO restaurant){
		var restaurantWithLinks = new RestaurantWithLinks(restaurant.Id, restaurant.Name, restaurant.Address, restaurant.Zipcode, restaurant.Latitude, restaurant.Longitude, restaurant.Telephone, restaurant.OpeningHours, restaurant.Delivery, restaurant.City, new List<LinkDTO>());
		await foreach (var link in PrivateGetLinks(restaurant.Id, null))
		{
			restaurantWithLinks.WebsiteLinks?.Add(link);
		}
		return restaurantWithLinks;
	}
}