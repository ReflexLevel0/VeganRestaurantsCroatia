using System.Globalization;
using api.Models;
using CsvHelper;
using CsvHelper.Configuration;
using Newtonsoft.Json;

namespace api;

public class FileHelper
{
	public async Task RefreshCsvFile(List<RestaurantWithLinks> restaurants, string filePath)
	{
		var csvConfig = new CsvConfiguration(CultureInfo.CurrentCulture) { Delimiter = ";" };
		var restaurantsCsv = new List<RestaurantCsv>();
		foreach (var r in restaurants)
		{
			if (r.WebsiteLinks != null)
			{
				restaurantsCsv.AddRange(r.WebsiteLinks.Select(l => new RestaurantCsv(r.Id, r.Name, r.Address, r.Zipcode, r.Latitude, r.Longitude, r.Telephone, r.OpeningHours, r.Delivery, r.City, l.LinkType, l.Link)));
			}
			else
			{
				restaurantsCsv.Add(new RestaurantCsv(r.Id, r.Name, r.Address, r.Zipcode, r.Latitude, r.Longitude, r.Telephone, r.OpeningHours, r.Delivery, r.City, null, null));
			}
		}

		await using var writer = new StreamWriter(filePath);
		await using var csv = new CsvWriter(writer, csvConfig);
		csv.Context.RegisterClassMap<RestaurantCsvMap>();
		await csv.WriteRecordsAsync(restaurantsCsv);
	}

	public async Task RefreshCsvFile(string filePath, IDb db)
	{
		var restaurants = await GetAllRestaurants(db);
		await RefreshCsvFile(restaurants, filePath);
	}
	
	public async Task RefreshJsonFile(IEnumerable<RestaurantWithLinks> restaurants, string filePath)
	{
		var jsonRestaurants = restaurants.Select(r => new RestaurantJsonld(r.Id, r.Name, r.Address, r.Zipcode, r.Latitude, r.Longitude, r.Telephone, r.OpeningHours, r.Delivery, r.City, r.WebsiteLinks)).ToList();
		await File.WriteAllTextAsync(filePath, JsonConvert.SerializeObject(jsonRestaurants));
	}

	public async Task RefreshJsonFile(string filePath, IDb db)
	{
		var restaurants = await GetAllRestaurants(db);
		await RefreshJsonFile(restaurants, filePath);
	}

	private async Task<List<RestaurantWithLinks>> GetAllRestaurants(IDb db)
	{
		var restaurants = new List<RestaurantWithLinks>();
		await foreach (var r in db.GetRestaurants(null, null, null, null, null, null, null, null, null, null, null))
		{
			restaurants.Add(r);
		}

		return restaurants;
	}
}