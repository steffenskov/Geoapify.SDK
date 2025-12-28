namespace Geoapify.SDK.Shared.Response;

internal class BoundingBox
{
	[JsonPropertyName("lon1")] public double Longitude1 { get; init; }

	[JsonPropertyName("lat1")] public double Latitude1 { get; init; }

	[JsonPropertyName("lon2")] public double Longitude2 { get; init; }

	[JsonPropertyName("lat2")] public double Latitude2 { get; init; }

	public static BoundingBox Empty => new();
}