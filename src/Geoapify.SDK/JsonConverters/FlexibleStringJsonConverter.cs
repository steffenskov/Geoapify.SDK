namespace Geoapify.SDK.JsonConverters;

internal sealed class FlexibleStringJsonConverter : JsonConverter<string?>
{
	public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		switch (reader.TokenType)
		{
			case JsonTokenType.String:
				return reader.GetString();

			case JsonTokenType.Number:
				using (var doc = JsonDocument.ParseValue(ref reader))
				{
					return doc.RootElement.GetRawText();
				}

			case JsonTokenType.Null:
				return null;

			default:
				throw new JsonException(
					$"Unexpected token {reader.TokenType} when reading a string value.");
		}
	}

	public override void Write(Utf8JsonWriter writer, string? value, JsonSerializerOptions options)
	{
		writer.WriteStringValue(value);
	}
}