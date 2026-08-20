namespace Geoapify.SDK.Geocoding.Inputs.SearchAreas;

internal interface ISearchAreaComposer
{
	string QueryStringKey { get; }

	QueryStringValue? ToQueryString()
	{
		var filterValue = string.Join("|", GetSearchAreas().Select(filter => filter.ToQueryString()));
		if (string.IsNullOrWhiteSpace(filterValue)) // Happens if SearchAreas is empty
		{
			return null;
		}

		return new QueryStringValue(QueryStringKey, filterValue);
	}

	IEnumerable<ISearchArea> GetSearchAreas();
}