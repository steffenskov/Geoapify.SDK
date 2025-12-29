# Geoapify.SDK

A FOSS SDK for Geoapify's API written in .Net.

## Usage

I strongly suggest using Dependency Injection via the package [Geoapify.DepedencyInjection](https://www.nuget.org/packages/Geoapify.DepedencyInjection) like this:

```csharp
var apiKey = "YOUR_KEY"; // Probably read this from a secret somewhere

services.AddGeoapify(apiKey);
```

And you'll have access to the `IGeoapifyClient` in your Dependency Injection system.

Furthermore if you want updates to addresses as they change (addresses *do* change over time, albeit not often), you should add both [Geoapify.Storage](https://www.nuget.org/packages/Geoapify.Storage) and a form of database (or provide one
yourself, see the notes for that package).

This allows you to store a copy of any relevant addresses locally (in your database), and have a service continuously check for changes to that address and inform you of those.

A full example of that configuration:

```csharp
var apiKey = "YOUR_KEY"; // Probably read this from a secret somewhere
var client = new MongoClient(mongoConnectionstring);
var db = client.GetDatabase("your-database");

services.AddGeoapify(apiKey) // Injects the IGeoapifyClient
        .AddMongoDBStorage(db, "address-collection-name") // Injects an IAddressRepository for MongoDB
        .AddStorageUpdaterService(TimeSpan.FromDays(7)); // Injects the StorageUpdaterService that'll check for updates to addresses once their date is 7 days or older
```

# Documentation

Auto generated documentation via [DocFx](https://github.com/dotnet/docfx) is available
here: https://steffenskov.github.io/Geoapify.SDK/
