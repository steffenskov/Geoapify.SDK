using Geoapify.SDK.Geocoding.Response;

namespace Geoapify.SDK.Geocoding.Outputs;

public class Address
{
	public Coordinate Coordinate { get; init; } = Coordinate.Empty;

	public string Name { get; init; } = "";

	static internal Address Create(GeocodingResult address)
	{
		return new Address
		{
			Name = address.Name,
			Coordinate = new Coordinate
			{
				Latitude = address.Latitude,
				Longitude = address.Longitude
			}
		};
	}
}

public class Coordinate
{
	public double Latitude { get; init; }
	public double Longitude { get; init; }

	public static Coordinate Empty => new();
}