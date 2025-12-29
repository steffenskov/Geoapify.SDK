# Geoapify.Storage

Adds abstract support for local storage of results from the Geoapify.SDK.

## Usage

This package is normally not used in isolation, as it contains abstract definitions required for local storage, as well as the `StorageUpdaterService`.

After adding this package, add this to your Dependency Injection setup:

```csharp
services.AddGeoapify(yourApiKey)
        .AddStorageUpdaterService(); // <-- This line is new
```

Do note that you also need to add some form of database for storage, e.g. `Geoapify.MongoDB`.

Furthermore if you want to react to addresses that has changed, use this:

```csharp
StorageUpdaterService.AddressChanged += YourHandler; // AddressChanged event is raised ONLY for addresses that actually changed
```

## Custom implementation

If you want to provide your own implementation, just implement the following interfaces and dependency inject them:

- `IAddressRepository`