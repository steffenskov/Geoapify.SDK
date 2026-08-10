using System.Text;
using System.Text.Json;
using Geoapify.SDK.JsonConverters;

namespace Geoapify.UnitTests.JsonConverters;

public class FlexibleStringJsonConverterTests
{
	private readonly FlexibleStringJsonConverter _converter = new();

	[Fact]
	public void Read_StringToken_ReturnsStringValue()
	{
		// Arrange
		var reader = CreateReader("\"test value\"");

		// Act
		var result = _converter.Read(ref reader, typeof(string), new JsonSerializerOptions());

		// Assert
		Assert.Equal("test value", result);
	}

	[Theory]
	[InlineData("123", "123")]
	[InlineData("-123", "-123")]
	[InlineData("123.45", "123.45")]
	[InlineData("1.23E+4", "1.23E+4")]
	public void Read_NumberToken_ReturnsRawNumberText(string json, string expected)
	{
		// Arrange
		var reader = CreateReader(json);

		// Act
		var result = _converter.Read(ref reader, typeof(string), new JsonSerializerOptions());

		// Assert
		Assert.Equal(expected, result);
	}

	[Fact]
	public void Read_NullToken_ReturnsNull()
	{
		// Arrange
		var reader = CreateReader("null");

		// Act
		var result = _converter.Read(ref reader, typeof(string), new JsonSerializerOptions());

		// Assert
		Assert.Null(result);
	}

	[Theory]
	[InlineData("true")]
	[InlineData("{}")]
	[InlineData("[]")]
	public void Read_UnsupportedToken_ThrowsJsonException(string json)
	{
		// Arrange

		// Act
		var exception = Assert.Throws<JsonException>(() =>
		{
			var reader = CreateReader(json);
			_converter.Read(ref reader, typeof(string), new JsonSerializerOptions());
		});

		// Assert
		Assert.Contains("Unexpected token", exception.Message);
	}

	[Fact]
	public void Write_StringValue_WritesJsonString()
	{
		// Arrange
		using var stream = new MemoryStream();
		using var writer = new Utf8JsonWriter(stream);

		// Act
		_converter.Write(writer, "test value", new JsonSerializerOptions());
		writer.Flush();

		// Assert
		Assert.Equal("\"test value\"", Encoding.UTF8.GetString(stream.ToArray()));
	}

	[Fact]
	public void Write_NullValue_WritesJsonNull()
	{
		// Arrange
		using var stream = new MemoryStream();
		using var writer = new Utf8JsonWriter(stream);

		// Act
		_converter.Write(writer, null, new JsonSerializerOptions());
		writer.Flush();

		// Assert
		Assert.Equal("null", Encoding.UTF8.GetString(stream.ToArray()));
	}

	private static Utf8JsonReader CreateReader(string json)
	{
		var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(json));
		reader.Read();

		return reader;
	}
}