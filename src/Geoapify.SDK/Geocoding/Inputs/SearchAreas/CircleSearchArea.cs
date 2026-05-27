namespace Geoapify.SDK.Geocoding.Inputs.SearchAreas;

public class CircleSearchArea : ISearchArea
{
	private readonly double _latitude;
	private readonly double _longitude;
	private readonly uint _radiusInMeters;

	public CircleSearchArea(double longitude, double latitude, uint radiusInMeters)
	{
		if (radiusInMeters == 0)
		{
			throw new ArgumentException("Radius must be greater than 0", nameof(radiusInMeters));
		}

		_longitude = longitude;
		_latitude = latitude;
		_radiusInMeters = radiusInMeters;
	}

	string ISearchArea.ToQueryString()
	{
		return string.Create(
			CultureInfo.InvariantCulture,
			$"circle:{_longitude},{_latitude},{_radiusInMeters}");
	}
}