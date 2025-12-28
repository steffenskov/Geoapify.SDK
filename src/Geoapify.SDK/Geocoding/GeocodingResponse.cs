namespace Geoapify.SDK.Geocoding;

public class GeocodingResponse
{
	public Address[] Results { get; init; } = [];
	// Also contains parsed Query - ignored for the time being. See https://apidocs.geoapify.com/docs/geocoding/forward-geocoding/
}