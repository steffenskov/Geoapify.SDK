using System.Text.Json.Serialization;

namespace Geoapify.SDK.Geocoding;

public class AddressRank
{
	public double Importance { get; init; }
	public double Confidence { get; init; }
	public double Popularity { get; init; }

	[JsonPropertyName("confidence_city_level")]
	public double ConfidenceCityLevel { get; init; }

	[JsonPropertyName("confidence_street_level")]
	public double ConfidenceStreetLevel { get; init; }

	public string MatchType { get; init; } = "";

	public static AddressRank Empty => new();
}