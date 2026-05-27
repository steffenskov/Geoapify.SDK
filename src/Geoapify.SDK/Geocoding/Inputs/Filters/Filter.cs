using Geoapify.SDK.Geocoding.Inputs.SearchAreas;

namespace Geoapify.SDK.Geocoding.Inputs.Filters;

/// <summary>
///     Filters to apply to search, defaults to all properties set to null, which means no filtering.
///     When using multiple filters, they're AND'ed together.
///     <seealso cref="https://apidocs.geoapify.com/docs/geocoding/forward-geocoding/" />
/// </summary>
public sealed class Filter : ISearchAreaComposer
{
	public CountryCodeSearchArea? CountryCode { get; set; }
	public PlaceSearchArea? Place { get; set; }
	public CircleSearchArea? Circle { get; set; }
	public RectangleSearchArea? Rectangle { get; set; }


	string ISearchAreaComposer.QueryStringKey => "filter";
}