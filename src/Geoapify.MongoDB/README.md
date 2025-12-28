# Geoapify.MongoDB

Adds MongoDB support for local storage of results from the Geoapify.SDK.

## Usage

After adding this package, just call

```
var apiKey = "YOUR_KEY"; // Probably read this from a secret somewhere

services.AddGeoapify(apiKey);
```

And you'll have access to the `IGeoapifyClient` in your Dependency Injection system.
