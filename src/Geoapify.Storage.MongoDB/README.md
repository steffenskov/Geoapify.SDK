# Geoapify.MongoDB

Adds MongoDB support for local storage of results from the Geoapify.SDK.

## Usage

After adding this package, add this to your Dependency Injection setup:

```csharp
services.AddGeoapify(yourApiKey)
        .AddMongoDBStorage(db, "address-collection-name"); // <-- This line is new
```

And you'll have access to the `IAddressRepository` in your Dependency Injection system.