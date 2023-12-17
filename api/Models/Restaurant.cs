namespace api.Models;

public class Restaurant(int id, string name, string address, int zipcode, double latitude, double longitude, string phone, string openingHours, bool delivery, int cityId) {
	public int Id { get; set; } = id;
	public string Name { get; set; } = name;
	public string Address { get; set; } = address;
	public int Zipcode { get; set; } = zipcode;
	public double Latitude { get; set; } = latitude;
	public double Longitude { get; set; } = longitude;
	public string Phone { get; set; } = phone;
	public string OpeningHours { get; set; } = openingHours;
	public bool Delivery { get; set; } = delivery;
	public int CityId { get; set; } = cityId;
}