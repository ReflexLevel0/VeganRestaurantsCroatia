using System.Diagnostics.CodeAnalysis;

namespace api.Models;

public class Restaurant(int id, string name, string address, int zipcode, double latitude, double longitude, string phone, string openingHours, bool delivery, int cityId) {
	/// <summary>
	/// Unique identifier of the restaurant
	/// </summary>
	/// <example>1</example>
	public int Id { get; set; } = id;
	/// <summary>
	/// Restaurant name
	/// </summary>
	/// <example>Restoran Vegehop</example>
	public string Name { get; set; } = name;
	/// <summary>
	/// Restaurant address
	/// </summary>
	/// <example>Vlaška ulica 79</example>
	public string Address { get; set; } = address;
	/// <summary>
	/// Zipcode of restaurants location
	/// </summary>
	/// <example>10000</example>
	public int Zipcode { get; set; } = zipcode;
	/// <summary>
	/// Location latitude
	/// </summary>
	/// <example>45.81462343</example>
	public double Latitude { get; set; } = latitude;
	/// <summary>
	/// Location longitude
	/// </summary>
	/// <example>15.98823879</example>
	public double Longitude { get; set; } = longitude;
	/// <summary>
	/// Phone number used by the restaurant
	/// </summary>
	/// <example>014649400</example>
	public string? Phone { get; set; } = phone;
	/// <summary>
	/// Working hours
	/// </summary>
	/// <example>Mon-Sat 12:00-20:00</example>
	public string? OpeningHours { get; set; } = openingHours;
	/// <summary>
	/// Specifies whether or not restaurant offers delivery services 
	/// </summary>
	/// <example>true</example>
	public bool Delivery { get; set; } = delivery;
	/// <summary>
	/// Unique identifier of the city in which the restaurant is in
	/// </summary>
	/// <example>1</example>
	public int CityId { get; set; } = cityId;
}