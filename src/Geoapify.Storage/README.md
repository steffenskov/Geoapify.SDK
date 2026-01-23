# Geoapify.Storage

Adds abstract support for local storage of results from the Geoapify.SDK.

## Usage

This package is normally not used in isolation, as it contains abstract definitions required for local storage, as well as the `StorageUpdaterService`.

After adding this package, add this to your Dependency Injection setup:

```csharp
services.AddGeoapify(yourApiKey)
        .AddStorageUpdaterService(); // <-- This line is new
```

Do note that you also need to add some form of database for storage, e.g. [Geoapify.Storage.MongoDB](https://www.nuget.org/packages/Geoapify.Storage.MongoDB).

Also you can supply a `config => { }` argument to `AddStorageUpdaterService()` if you want to customize intervals etc.

Furthermore if you want to react to addresses that has changed, implement one (or more) `IAddressChangedHandler` classes and add those to the Dependency Injection setup.

```csharp
services.AddGeoapify(yourApiKey)
        .AddStorageUpdaterService()
        .AddAddressChangedHandler<MyHandler>(); // <-- This line is new
```

## Custom implementation

If you want to provide your own storage implementation, just implement the following interfaces and dependency inject them:

- `IAddressRepository`