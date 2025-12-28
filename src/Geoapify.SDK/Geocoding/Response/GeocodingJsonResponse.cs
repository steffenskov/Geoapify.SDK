namespace Geoapify.SDK.Geocoding.Response;

internal class GeocodingJsonResponse
{
	[JsonPropertyName("results")] public GeocodingResult[] Results { get; init; } = [];

	[JsonPropertyName("query")] public QueryInfo? Query { get; init; }
}