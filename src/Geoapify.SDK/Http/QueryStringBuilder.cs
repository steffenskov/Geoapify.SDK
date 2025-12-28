namespace Geoapify.SDK.Http;

internal class QueryStringBuilder
{
	private readonly List<QueryStringValue> _values = [];

	public QueryStringBuilder With(QueryStringValue value)
	{
		_values.Add(value);
		return this;
	}

	public QueryStringBuilder With(string key, string value)
	{
		_values.Add(new QueryStringValue(key, value));
		return this;
	}

	public QueryStringBuilder With(IQueryStringArgument argument)
	{
		_values.AddRange(argument.ToQueryString());
		return this;
	}

	public string Build()
	{
		return string.Join("&", _values.Select(val => $"{val.Key}={HttpUtility.UrlEncode(val.Value)}"));
	}
}

public record QueryStringValue(string Key, string Value);