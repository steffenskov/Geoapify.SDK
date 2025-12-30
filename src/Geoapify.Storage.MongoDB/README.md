# Geoapify.MongoDB

Adds MongoDB support for local storage of results from the Geoapify.SDK.

## Usage

After adding this package, add this to your Dependency Injection setup:

```csharp
services.AddGeoapify(yourApiKey)
        .AddMongoDBStorage(db, "address-collection-name"); // <-- This line is new
```

And you'll have access to the `IAddressRepository` in your Dependency Injection system.

Use this to `Upsert` any addresses you want to keep track of - the `StorageUpdaterService` will ensure data is continuously updated and notify you via the `AddressChanged` static event.