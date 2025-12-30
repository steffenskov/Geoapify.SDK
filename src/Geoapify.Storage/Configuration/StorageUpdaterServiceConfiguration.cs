namespace Geoapify.Storage.Configuration;

public class StorageUpdaterServiceConfiguration
{
	/// <summary>
	///     How old an address should be before checking for updated data.
	///     Defaults to 7 Days
	/// </summary>
	public TimeSpan RefreshDataAfter { get; set; } = TimeSpan.FromDays(7);

	/// <summary>
	///     Delay between checks for outdated addresses.
	///     Defaults to 1 Hour
	/// </summary>
	public TimeSpan LoopDelay { get; set; } = TimeSpan.FromHours(1);
}