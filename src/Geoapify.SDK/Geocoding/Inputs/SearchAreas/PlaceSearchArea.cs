namespace Geoapify.SDK.Geocoding.Inputs.SearchAreas;

public sealed class PlaceSearchArea : ISearchArea
{
	private readonly string _placeId;

	public PlaceSearchArea(string placeId)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(placeId);

		_placeId = placeId;
	}

	string ISearchArea.ToQueryString()
	{
		return $"place:{_placeId}";
	}
}