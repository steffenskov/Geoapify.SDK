using System.Text.Json;
using Geoapify.SDK.Geocoding;

namespace Geoapify.UnitTests.Geocoding;

public class GeocodingResponseTests
{
	[Fact]
	public void GeocodingResponse_ValidJson_CanDeserialize()
	{
		// Arrange
		var json = """
		           {
		             "results" : [ {
		               "datasource" : {
		                 "sourcename" : "openstreetmap",
		                 "attribution" : "© OpenStreetMap contributors",
		                 "license" : "Open Database License",
		                 "url" : "https://www.openstreetmap.org/copyright"
		               },
		               "country" : "Denmark",
		               "country_code" : "dk",
		               "state" : "Central Denmark Region",
		               "city" : "Viborg",
		               "municipality" : "Viborg Municipality",
		               "postcode" : "8800",
		               "suburb" : "Søndermarken",
		               "street" : "Falkevej",
		               "housenumber" : "40",
		               "iso3166_2" : "DK-82",
		               "lon" : 9.386432,
		               "lat" : 56.441652,
		               "result_type" : "building",
		               "formatted" : "Falkevej 40, 8800 Viborg, Denmark",
		               "address_line1" : "Falkevej 40",
		               "address_line2" : "8800 Viborg, Denmark",
		               "timezone" : {
		                 "name" : "Europe/Berlin",
		                 "offset_STD" : "+01:00",
		                 "offset_STD_seconds" : 3600,
		                 "offset_DST" : "+02:00",
		                 "offset_DST_seconds" : 7200,
		                 "abbreviation_STD" : "CET",
		                 "abbreviation_DST" : "CEST"
		               },
		               "plus_code" : "9F8FC9RP+MH",
		               "plus_code_short" : "C9RP+MH Viborg, Central Denmark Region, Denmark",
		               "rank" : {
		                 "importance" : 9.99999999995449E-6,
		                 "popularity" : 3.6917845104868667,
		                 "confidence" : 1,
		                 "confidence_city_level" : 1,
		                 "confidence_street_level" : 1,
		                 "confidence_building_level" : 1,
		                 "match_type" : "full_match"
		               },
		               "place_id" : "517841446adac5224059431b800d88384c40f00103f901d9836d1400000000c00203",
		               "bbox" : {
		                 "lon1" : 9.386382,
		                 "lat1" : 56.441602,
		                 "lon2" : 9.386482,
		                 "lat2" : 56.441702
		               }
		             } ],
		             "query" : {
		               "text" : "Falkevej 40, 8800 Viborg, Danmark",
		               "parsed" : {
		                 "housenumber" : "40",
		                 "street" : "falkevej",
		                 "postcode" : "8800",
		                 "city" : "viborg",
		                 "country" : "danmark",
		                 "expected_type" : "building"
		               }
		             }
		           }
		           """;

		// Act
		var result = JsonSerializer.Deserialize<GeocodingResponse>(json, options: new JsonSerializerOptions(JsonSerializerDefaults.Web));

		// Assert
		Assert.NotNull(result);
		Assert.NotEmpty(result.Results);
	}
}