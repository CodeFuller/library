# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [9.0.0] - 2025-05-18

### Changed

- Upgraded to .NET 8.0.

## [8.2.0] - 2023-07-17

### Added

- `InverseBooleanValueConverter` to CodeFuller.Library.Wpf package.

## [8.1.0] - 2023-07-06

### Added

- `InvertedVisibility` property in  `VisibilityCollapsedValueConverter` and `VisibilityHiddenValueConverter`.

### Removed

- `AsyncRelayCommand` from CodeFuller.Library.Wpf package.

## [8.0.0] - 2023-06-30

### Changed

- Updated to .NET 7.0.

## [7.5.2] - 2022-05-27

### Changed

- Switched to build via Azure Pipelines.

## [7.5.0] - 2022-02-12

### Added

- GetViewModel extension method to CodeFuller.Library.Wpf package.

## [7.4.0] - 2021-10-23

### Added

- Visibility value converters (`VisibilityCollapsedValueConverter` and `VisibilityHiddenValueConverter`) to CodeFuller.Library.Wpf package.

## [7.3.0] - 2021-09-05

### Added

- MultiSelectionListBox control to CodeFuller.Library.Wpf package.

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

[8.1.0]: https://github.com/CodeFuller/library/compare/v8.1.0...v8.2.0
[8.1.0]: https://github.com/CodeFuller/library/compare/v8.0.0...v8.1.0
[8.0.0]: https://github.com/CodeFuller/library/compare/v7.5.2...v8.0.0
[7.5.2]: https://github.com/CodeFuller/library/compare/v7.5.0...v7.5.2
[7.5.0]: https://github.com/CodeFuller/library/compare/v7.4.0...v7.5.0
[7.4.0]: https://github.com/CodeFuller/library/compare/v7.3.0...v7.4.0
[7.3.0]: https://github.com/CodeFuller/library/compare/v7.2.0...v7.3.0
[7.2.0]: https://github.com/CodeFuller/library/compare/v7.1.0...v7.2.0
[7.1.0]: https://github.com/CodeFuller/library/compare/v7.0.0...v7.1.0
[7.0.0]: https://github.com/CodeFuller/library/compare/v6.0.0...v7.0.0
