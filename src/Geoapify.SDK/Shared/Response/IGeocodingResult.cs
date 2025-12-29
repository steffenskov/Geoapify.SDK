namespace Geoapify.SDK.Shared.Response;

internal interface IGeocodingResult
{
	double Longitude { get; init; }
	double Latitude { get; init; }
	string? Country { get; init; }
	string? CountryCode { get; init; }
	string? State { get; init; }
	string? StateCode { get; init; }
	string? County { get; init; }
	string? CountyCode { get; init; }
	string? City { get; init; }
	string? Postcode { get; init; }
	string? District { get; init; }
	string? Suburb { get; init; }
	string? Street { get; init; }
	string? HouseNumber { get; init; }
	string? Formatted { get; init; }
	string? AddressLine1 { get; init; }
	string? AddressLine2 { get; init; }
	string? Name { get; init; }
	Datasource? Datasource { get; init; }
	string? Category { get; init; }
	ResultTypes ResultType { get; init; }
	Rank? Rank { get; init; }
	string? PlaceId { get; init; }
	BoundingBox? BoundingBox { get; init; }
	Timezone? Timezone { get; init; }
	string? PlusCode { get; init; }
	string? PlusCodeShort { get; init; }
	double? Distance { get; init; }
	string? Municipality { get; init; }
}