using Geoapify.SDK.Geocoding.Response;

namespace Geoapify.SDK.Geocoding.Outputs;

public class Address
{
	public string AddressLines { get; init; } = "";
	public string City { get; init; } = "";
	public Coordinate Coordinate { get; init; } = Coordinate.Empty;
	public string Country { get; init; } = "";
	public string CountryCode { get; init; } = "";
	public string County { get; init; } = "";
	public string CountyCode { get; init; } = "";
	public string District { get; init; } = "";
	public string HouseNumber { get; init; } = "";
	public string Id { get; init; } = "";
	public string Municipality { get; init; } = "";
	public string Name { get; init; } = "";
	public string Postcode { get; init; } = "";
	public string State { get; init; } = "";
	public string StateCode { get; init; } = "";
	public string Street { get; init; } = "";
	public string Suburb { get; init; } = "";
	public DateTimeOffset LastUpdated { get; init; }


	static internal Address Create(GeocodingResult address, DateTimeOffset lastUpdated)
	{
		return new Address
		{
			AddressLines = $"""
			                {address.AddressLine1}
			                {address.AddressLine2}
			                """.Trim(),
			City = address.City ?? "",
			Coordinate = new Coordinate
			{
				Latitude = address.Latitude,
				Longitude = address.Longitude
			},
			Country = address.Country ?? "",
			CountryCode = address.CountryCode ?? "",
			County = address.County ?? "",
			CountyCode = address.CountyCode ?? "",
			District = address.District ?? "",
			HouseNumber = address.HouseNumber ?? "",
			Id = address.PlaceId ?? throw new ArgumentException("PlaceId cannot be null", nameof(address)),
			Municipality = address.Municipality ?? "",
			Name = address.Name ?? "",
			Postcode = address.Postcode ?? "",
			State = address.State ?? "",
			StateCode = address.StateCode ?? "",
			Street = address.Street ?? "",
			Suburb = address.Suburb ?? "",
			LastUpdated = lastUpdated
		};
	}
}

public class Coordinate
{
	public double Latitude { get; init; }
	public double Longitude { get; init; }

	public static Coordinate Empty => new();
}