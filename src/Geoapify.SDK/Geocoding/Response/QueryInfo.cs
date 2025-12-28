namespace Geoapify.SDK.Geocoding.Response;

internal class QueryInfo
{
	[JsonPropertyName("text")] public string? Text { get; init; }

	[JsonPropertyName("housenumber")] public string? HouseNumber { get; init; }

	[JsonPropertyName("street")] public string? Street { get; init; }

	[JsonPropertyName("postcode")] public string? Postcode { get; init; }

	[JsonPropertyName("city")] public string? City { get; init; }

	[JsonPropertyName("state")] public string? State { get; init; }

	[JsonPropertyName("country")] public string? Country { get; init; }

	[JsonPropertyName("parsed")] public ParsedQueryInfo? Parsed { get; init; }

	public static QueryInfo Empty => new();
}

internal class ParsedQueryInfo
{
	[JsonPropertyName("house")] public string? House { get; init; }

	[JsonPropertyName("housenumber")] public string? HouseNumber { get; init; }

	[JsonPropertyName("street")] public string? Street { get; init; }

	[JsonPropertyName("postcode")] public string? Postcode { get; init; }

	[JsonPropertyName("city")] public string? City { get; init; }

	[JsonPropertyName("state")] public string? State { get; init; }

	[JsonPropertyName("country")] public string? Country { get; init; }

	[JsonPropertyName("expected_type")] public string? ExpectedType { get; init; }

	public static ParsedQueryInfo Empty => new();
}