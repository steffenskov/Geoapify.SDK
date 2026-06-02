# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.5.0] - 2026-06-02

### Added

- Support for Autocomplete based searching through `IGeoapifyClient.Autocomplete.AutocompleteAsync()`

## [1.4.0] - 2026-05-27

### Added

- Support for remaining Filters
- Support for Bias

### Changed

- Syntax for Filters was changed to provide a better auto-complete/intellisense end-user experience.

## [1.3.0] - 2026-05-26

### Added

- Added `Filters` property to `GeocodingSearchArguments`, currently only with Country based filtering supported.

## [1.2.0] - 2026-03-06

### Changed

- Updated to .Net 10

## [1.1.0] - 2025-12-30

### Added

- Geocoding API support
- Reverse geocoding API support
- (Optional) Storage of addresses in your own MongoDB
- (Optional) Service that monitors your stored addresses and notifies when they have updates (e.g. a street name change)
