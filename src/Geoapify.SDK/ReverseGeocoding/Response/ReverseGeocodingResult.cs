using Geoapify.SDK.Shared.Response;

namespace Geoapify.SDK.ReverseGeocoding.Response;

internal class ReverseGeocodingResult : IGeocodingResult
{
	// Coordinates
	[JsonPropertyName("lon")] public double Longitude { get; init; }

	[JsonPropertyName("lat")] public double Latitude { get; init; }

	// Address Components
	[JsonPropertyName("country")] public string? Country { get; init; }

	[JsonPropertyName("country_code")] public string? CountryCode { get; init; }

	[JsonPropertyName("state")] public string? State { get; init; }

	[JsonPropertyName("state_code")] public string? StateCode { get; init; }

	[JsonPropertyName("city")] public string? City { get; init; }

	[JsonPropertyName("county")] public string? County { get; init; }

	[JsonPropertyName("county_code")] public string? CountyCode { get; init; }

	[JsonPropertyName("postcode")] public string? Postcode { get; init; }

	[JsonPropertyName("district")] public string? District { get; init; }


	[JsonPropertyName("suburb")] public string? Suburb { get; init; }

	[JsonPropertyName("street")] public string? Street { get; init; }

	[JsonPropertyName("housenumber")] public string? HouseNumber { get; init; }

	// Formatted Address
	[JsonPropertyName("formatted")] public string? Formatted { get; init; }

	[JsonPropertyName("address_line1")] public string? AddressLine1 { get; init; }

	[JsonPropertyName("address_line2")] public string? AddressLine2 { get; init; }

	// Additional Properties
	[JsonPropertyName("name")] public string? Name { get; init; }

	[JsonPropertyName("datasource")] public Datasource? Datasource { get; init; }

	[JsonPropertyName("category")] public string? Category { get; init; }

	[JsonPropertyName("result_type")] public ResultTypes ResultType { get; init; }

	[JsonPropertyName("rank")] public Rank? Rank { get; init; }

	[JsonPropertyName("place_id")] public string? PlaceId { get; init; }

	[JsonPropertyName("bbox")] public BoundingBox? BoundingBox { get; init; }

	[JsonPropertyName("timezone")] public Timezone? Timezone { get; init; }

	[JsonPropertyName("plus_code")] public string? PlusCode { get; init; }

	[JsonPropertyName("plus_code_short")] public string? PlusCodeShort { get; init; }

	[JsonPropertyName("distance")] public double? Distance { get; init; }
	[JsonPropertyName("municipality")] public string? Municipality { get; init; }
}