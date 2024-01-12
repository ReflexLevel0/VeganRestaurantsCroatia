using Newtonsoft.Json;

namespace api.Models;

public class Address(string? streetAddress, string? addressLocality, string? postalCode)
{
	[JsonProperty(PropertyName = "@type")]
	public string Type { get; } = "PostalAddress";
	
	/// <summary>
	/// Restaurant address
	/// </summary>
	/// <example>Vlaška ulica 79</example>
	[JsonProperty(PropertyName = "streetAddress")]
	public string? StreetAddress { get; set; } = streetAddress;

	/// <summary>
	/// Restaurant locality
	/// </summary>
	/// <example>Zagreb, Croatia</example>
	[JsonProperty(PropertyName = "addressLocality")]
	public string? AddressLocality { get; set; } = addressLocality;
	
	/// <summary>
	/// Zipcode of restaurants location
	/// </summary>
	/// <example>10000</example>
	[JsonProperty(PropertyName = "postalCode")]
	public string? PostalCode { get; set; } = postalCode;
}