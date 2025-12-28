using System.Text.Json.Serialization;

namespace Geoapify.SDK.Geocoding;

public class Address
{
	[JsonPropertyName("lat")] public double Latitude { get; init; }

	[JsonPropertyName("lon")] public double Longitude { get; init; }

	public string HouseNumber { get; init; } = "";
	public string Suburb { get; init; } = "";
	public string City { get; init; } = "";
	public string County { get; init; } = "";
	public string State { get; init; } = "";
	public string Postcode { get; init; } = "";
	public string Municipality { get; init; } = "";

	[JsonPropertyName("country")] public string Country { get; init; } = "";

	[JsonPropertyName("country_code")] public string CountryCode { get; init; } = "";

	[JsonPropertyName("formatted")] public string Formatted { get; init; } = "";

	[JsonPropertyName("address_line1")] public string AddressLine1 { get; init; } = "";

	[JsonPropertyName("address_line2")] public string AddressLine2 { get; init; } = "";

	[JsonPropertyName("state_code")] public string StateCode { get; init; } = "";

	[JsonPropertyName("result_type")] public string ResultType { get; init; } = "";

	public AddressRank Rank { get; init; } = AddressRank.Empty;

	public AddressTimeZone TimeZone { get; init; } = AddressTimeZone.Empty;

	[JsonPropertyName("place_id")] public string PlaceId { get; init; } = "";
}