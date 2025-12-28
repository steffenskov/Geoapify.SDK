namespace Geoapify.SDK.Shared.Response;

internal class Rank
{
	[JsonPropertyName("importance")] public double? Importance { get; init; }

	[JsonPropertyName("popularity")] public double? Popularity { get; init; }

	[JsonPropertyName("confidence")] public double? Confidence { get; init; }

	[JsonPropertyName("confidence_city_level")]
	public double? ConfidenceCityLevel { get; init; }

	[JsonPropertyName("confidence_street_level")]
	public double? ConfidenceStreetLevel { get; init; }

	[JsonPropertyName("confidence_building_level")]
	public double? ConfidenceBuildingLevel { get; init; }

	[JsonPropertyName("match_type")] public string? MatchType { get; init; }
}