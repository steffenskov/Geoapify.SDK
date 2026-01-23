using Geoapify.SDK.Shared.Outputs;

namespace Geoapify.Storage.Services;

public interface IAddressChangedHandler
{
	/// <summary>
	///     Should handle the fact that the address was updated.
	///     Note: No CancellationToken as all handlers should be invoked to ensure consistency.
	/// </summary>
	/// <param name="updatedAddress">Address that was updated.</param>
	/// <returns></returns>
	ValueTask HandleAsync(Address updatedAddress);
}