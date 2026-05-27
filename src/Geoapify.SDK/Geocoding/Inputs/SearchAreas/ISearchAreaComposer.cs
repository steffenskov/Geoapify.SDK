namespace Geoapify.SDK.Geocoding.Inputs.SearchAreas;

internal interface ISearchAreaComposer
{
	string QueryStringKey { get; }
	CountryCodeSearchArea? CountryCode { get; }
	PlaceSearchArea? Place { get; }
	CircleSearchArea? Circle { get; }
	RectangleSearchArea? Rectangle { get; }

	QueryStringValue? ToQueryString()
	{
		var filterValue = string.Join("|", GetSearchAreas().Select(filter => filter.ToQueryString()));
		if (string.IsNullOrWhiteSpace(filterValue)) // Happens if SearchAreas is empty
		{
			return null;
		}

		return new QueryStringValue(QueryStringKey, filterValue);
	}

	IEnumerable<ISearchArea> GetSearchAreas()
	{
		if (CountryCode is not null)
		{
			yield return CountryCode;
		}

		if (Place is not null)
		{
			yield return Place;
		}

		if (Circle is not null)
		{
			yield return Circle;
		}

		if (Rectangle is not null)
		{
			yield return Rectangle;
		}
	}
}