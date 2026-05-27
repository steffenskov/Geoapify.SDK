using Geoapify.SDK.Geocoding.Inputs.SearchAreas;

namespace Geoapify.SDK.Geocoding.Inputs.Biases;

/// <summary>
///     Biases to apply to search, defaults to all properties set to null, which results in countrycode:auto bias, where
///     geoapify will base its bias on the client IP.
///     When using multiple biases, they're OR'ed together.
///     <seealso cref="https://apidocs.geoapify.com/docs/geocoding/forward-geocoding/" />
/// </summary>
public sealed class Bias : ISearchAreaComposer
{
	public CountryCodeSearchArea? CountryCode { get; set; }
	public PlaceSearchArea? Place { get; set; }
	public CircleSearchArea? Circle { get; set; }
	public RectangleSearchArea? Rectangle { get; set; }


	string ISearchAreaComposer.QueryStringKey => "bias";
}