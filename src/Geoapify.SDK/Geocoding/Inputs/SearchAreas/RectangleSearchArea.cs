namespace Geoapify.SDK.Geocoding.Inputs.SearchAreas;

public class RectangleSearchArea : ISearchArea
{
	private readonly double _latitude1;
	private readonly double _latitude2;
	private readonly double _longitude1;
	private readonly double _longitude2;

	public RectangleSearchArea(double longitude1, double latitude1, double longitude2, double latitude2)
	{
		if (latitude1 is < -90 or > 90)
		{
			throw new ArgumentOutOfRangeException(nameof(latitude1), "Latitude must be between -90.0 and 90.0");
		}

		if (longitude1 is < -180 or > 180)
		{
			throw new ArgumentOutOfRangeException(nameof(longitude1), "Longitude must be between -180.0 and 180.0");
		}

		if (latitude2 is < -90 or > 90)
		{
			throw new ArgumentOutOfRangeException(nameof(latitude2), "Latitude must be between -90.0 and 90.0");
		}

		if (longitude2 is < -180 or > 180)
		{
			throw new ArgumentOutOfRangeException(nameof(longitude2), "Longitude must be between -180.0 and 180.0");
		}

		_longitude1 = longitude1;
		_latitude1 = latitude1;
		_longitude2 = longitude2;
		_latitude2 = latitude2;
	}

	string ISearchArea.ToQueryString()
	{
		return string.Create(
			CultureInfo.InvariantCulture,
			$"rect:{_longitude1},{_latitude1},{_longitude2},{_latitude2}");
	}
}