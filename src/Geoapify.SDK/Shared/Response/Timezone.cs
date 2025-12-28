namespace Geoapify.SDK.Shared.Response;

internal class Timezone
{
	[JsonPropertyName("name")] public string? Name { get; init; }

	[JsonPropertyName("name_alt")] public string? NameAlt { get; init; }

	[JsonPropertyName("offset_STD")] public string? OffsetStd { get; init; }

	[JsonPropertyName("offset_STD_seconds")]
	public int? OffsetStdSeconds { get; init; }

	[JsonPropertyName("offset_DST")] public string? OffsetDst { get; init; }

	[JsonPropertyName("offset_DST_seconds")]
	public int? OffsetDstSeconds { get; init; }

	[JsonPropertyName("abbreviation_STD")] public string? AbbreviationStd { get; init; }

	[JsonPropertyName("abbreviation_DST")] public string? AbbreviationDst { get; init; }

	public static Timezone Empty => new();
}