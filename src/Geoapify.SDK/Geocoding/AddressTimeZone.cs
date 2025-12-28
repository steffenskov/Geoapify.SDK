using System.Text.Json.Serialization;

namespace Geoapify.SDK.Geocoding;

public class AddressTimeZone
{
	public string Name { get; init; } = "";

	[JsonPropertyName("offset_STD")] public string OffsetSTD { get; init; } = "";

	[JsonPropertyName("offset_STD_seconds")]
	public int OffsetSTDSeconds { get; init; }

	[JsonPropertyName("abbreviation_STD")] public string AbbreviationSTD { get; init; } = "";


	[JsonPropertyName("offset_DST")] public string OffsetDST { get; init; } = "";

	[JsonPropertyName("offset_DST_seconds")]
	public int OffsetDSTSeconds { get; init; }

	[JsonPropertyName("abbreviation_DST")] public string AbbreviationDST { get; init; } = "";

	public static AddressTimeZone Empty => new();
}