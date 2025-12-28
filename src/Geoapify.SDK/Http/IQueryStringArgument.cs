namespace Geoapify.SDK.Http;

internal interface IQueryStringArgument
{
	bool HasValues => ToQueryString().Any();

	IEnumerable<QueryStringValue> ToQueryString();
}