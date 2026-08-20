namespace Geoapify.SDK.Geocoding.Inputs.SearchAreas;

public sealed class LocationSearchArea : ISearchArea
{
	private readonly double _latitude;
	private readonly double _longitude;

	public LocationSearchArea(double longitude, double latitude)
	{
		if (latitude is < -90 or > 90)
		{
			throw new ArgumentOutOfRangeException(nameof(latitude), "Latitude must be between -90.0 and 90.0");
		}

		if (longitude is < -180 or > 180)
		{
			throw new ArgumentOutOfRangeException(nameof(longitude), "Longitude must be between -180.0 and 180.0");
		}

		_longitude = longitude;
		_latitude = latitude;
	}

	string ISearchArea.ToQueryString()
	{
		return $"proximity:{_longitude.ToString(CultureInfo.InvariantCulture)},{_latitude.ToString(CultureInfo.InvariantCulture)}";
	}
}