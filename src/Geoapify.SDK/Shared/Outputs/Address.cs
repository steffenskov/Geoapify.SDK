using Geoapify.SDK.Shared.Response;

namespace Geoapify.SDK.Shared.Outputs;

public record Address
{
	public Guid Id { get; init; } = Guid.Empty;
	public string AddressLines { get; init; } = "";
	public string City { get; init; } = "";
	public Coordinate Coordinate { get; init; } = Coordinate.Empty;
	public string Country { get; init; } = "";
	public string CountryCode { get; init; } = "";
	public string County { get; init; } = "";
	public string CountyCode { get; init; } = "";
	public string District { get; init; } = "";
	public string HouseNumber { get; init; } = "";
	public string PlaceId { get; init; } = "";
	public string Municipality { get; init; } = "";
	public string Name { get; init; } = "";
	public string Postcode { get; init; } = "";
	public string State { get; init; } = "";
	public string StateCode { get; init; } = "";
	public string Street { get; init; } = "";
	public string Suburb { get; init; } = "";
	public DateTimeOffset LastUpdated { get; init; }
	public bool Retired { get; init; }

	static internal Address Create(IGeocodingResult address, DateTimeOffset lastUpdated)
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
			Id = CreateId(address.Latitude, address.Longitude),
			Municipality = address.Municipality ?? "",
			Name = address.Name ?? "",
			PlaceId = address.PlaceId ?? throw new ArgumentException("PlaceId cannot be null", nameof(address)),
			Postcode = address.Postcode ?? "",
			State = address.State ?? "",
			StateCode = address.StateCode ?? "",
			Street = address.Street ?? "",
			Suburb = address.Suburb ?? "",
			LastUpdated = lastUpdated
		};
	}

	static internal Guid CreateId(double latitude, double longitude)
	{
		var latBytes = BitConverter.GetBytes(latitude);
		var lonBytes = BitConverter.GetBytes(longitude);

		return new Guid([.. latBytes, .. lonBytes]);
	}

	/// <summary>
	///     Returns whether this has changed since `expiredAddress`.
	/// </summary>
	/// <param name="oldAddress">Old representation of same address to compare against</param>
	/// <returns>True if something has changed, otherwise false</returns>
	public bool HasChanged(Address oldAddress)
	{
		return oldAddress with { LastUpdated = LastUpdated } != this;
	}

	/// <summary>
	///     Returns the address marked as retired, used for addresses that can no longer be found through Reverse Geocoding BUT
	///     are kept in local storage.
	/// </summary>
	public Address Retire()
	{
		return this with { Retired = true };
	}
}

public record Coordinate
{
	public double Latitude { get; init; }
	public double Longitude { get; init; }

	public static Coordinate Empty => new();
}