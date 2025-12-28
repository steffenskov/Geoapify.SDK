# Geoapify.MongoDB

Adds MongoDB support for local storage of results from the Geoapify.SDK.

## Usage

After adding this package, just call

```
services.AddGeoapifyMongoDBStorage(db, "address-collection-name");
```

And you'll have access to the `IAddressRepository` in your Dependency Injection system.

OR you can just `new AddressRepository(db, "address-collection-name");` if you don't use Dependency Injection.
