using CsvHelper.Configuration;

namespace api.Models;

public sealed class RestaurantCsvMap : ClassMap<RestaurantCsv>
{
	public RestaurantCsvMap()
	{
		Map(r => r.Id).Name("id").Index(0);
		Map(r => r.Name).Name("name").Index(1);
		Map(r => r.Address).Name("address").Index(2);
		Map(r => r.Zipcode).Name("zipcode").Index(3);
		Map(r => r.Latitude).Name("latitude").Index(4);
		Map(r => r.Longitude).Name("longitude").Index(5);
		Map(r => r.Telephone).Name("telephone").Index(6);
		Map(r => r.OpeningHours).Name("openingHours").Index(7);
		Map(r => r.Delivery).Convert(d => d.Value.Delivery ? "t" : "f").Name("delivery").Index(8);
		Map(r => r.City).Name("city").Index(9);
		Map(r => r.LinkType).Name("linktype").Index(10);
		Map(r => r.Link).Name("link").Index(11);
	}
}