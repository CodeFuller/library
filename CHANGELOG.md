# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [7.2.0] - 2021-01-24

### Changed

- Extended BootstrapLogging method in BasicApplicationBootstrapper with IServiceProvider argument.

## [7.1.0] - 2021-01-23

### Added

- MultiSelectionDataGrid control to CodeFuller.Library.Wpf package.
- Catching of unhandled exceptions to WpfApplication class.

## [7.0.0] - 2021-01-23

### Added

- XML documentation.
- MIT license.

### Changed
- Renamed CF.Library namespace to CodeFuller.Library.
- Re-targeted CodeFuller.Library.Wpf package from netcoreapp3.1 to net5.0-windows.
- Updated .NET Core packages to version 5.0.0.

### Removed

- Package CF.Library.Core.
- Package CF.Library.Database.
- Package CF.Library.Json.
- Package CF.Library.Patterns.
- Types and functionality, which are not shared accross the projects.

[7.1.0]: https://github.com/CodeFuller/library/compare/v7.0.0...v7.1.0
[7.0.0]: https://github.com/CodeFuller/library/compare/v6.0.0...v7.0.0
