namespace Geoapify.SDK.Geocoding.Inputs.SearchAreas;

public class RectangleSearchArea : ISearchArea
{
	private readonly double _latitude1;
	private readonly double _latitude2;
	private readonly double _longitude1;
	private readonly double _longitude2;

	public RectangleSearchArea(double longitude1, double latitude1, double longitude2, double latitude2)
	{
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