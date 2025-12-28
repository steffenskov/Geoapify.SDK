# Geoapify.Storage

Adds abstract support for local storage of results from the Geoapify.SDK.

## Usage

This package is normally not used in isolation, rather it just contains the abstractions necessary for local storage of results.

## Custom implementation

If you want to provide your own implementation, just implement the following interfaces and dependency inject them (or provide support for newing them up):

- `IAddressRepository`