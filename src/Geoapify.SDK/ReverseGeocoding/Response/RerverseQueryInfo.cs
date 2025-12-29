namespace Geoapify.SDK.ReverseGeocoding.Response;

internal class ReverseQueryInfo
{
	[JsonPropertyName("lat")] public double Latitude { get; init; }

	[JsonPropertyName("lon")] public double Longitude { get; init; }

	[JsonPropertyName("plus_code")] public string? PlusCode { get; init; }
}