namespace Geoapify.SDK.ReverseGeocoding.Response;

internal class ReverseGeocodingJsonResponse
{
	[JsonPropertyName("results")] public ReverseGeocodingResult[] Results { get; init; } = [];

	[JsonPropertyName("query")] public ReverseQueryInfo? Query { get; init; }
}