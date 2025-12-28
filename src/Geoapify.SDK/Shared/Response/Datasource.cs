namespace Geoapify.SDK.Shared.Response;

internal class Datasource
{
	[JsonPropertyName("sourcename")] public string? SourceName { get; init; }

	[JsonPropertyName("attribution")] public string? Attribution { get; init; }

	[JsonPropertyName("license")] public string? License { get; init; }

	[JsonPropertyName("url")] public string? Url { get; init; }

	public static Datasource Empty => new();
}