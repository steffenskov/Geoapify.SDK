using System.Diagnostics;

namespace Geoapify.SDK.Shared;

static internal class AddressId
{
	public static Guid Create(double latitude, double longitude)
	{
		var latBytes = BitConverter.GetBytes(latitude);
		var lonBytes = BitConverter.GetBytes(longitude);

		return new Guid([.. latBytes, .. lonBytes]);
	}

	public static (double latitude, double longitude) Parse(Guid id)
	{
		Span<byte> bytes = stackalloc byte[16];
		if (!id.TryWriteBytes(bytes))
		{
			throw new UnreachableException($"Failed to extract bytes from id: {id}");
		}

		var latBytes = bytes[..8];
		var lonBytes = bytes[8..16];

		var latitude = BitConverter.ToDouble(latBytes);
		var longitude = BitConverter.ToDouble(lonBytes);
		return (latitude, longitude);
	}
}