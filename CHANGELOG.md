# Changelog

All notable changes to this project will be documented in this file.

## [1.0.0] - 2026-07-25

### Added
- Complete rewrite with Clean Architecture (Domain, Infrastructure, Facade layers)
- Dependency Injection support via `AddTikTokExplode()` extension method
- NuGet package with full metadata (TikTokExplode)
- `TikTokClient` facade with methods: GetPublicationAsync, GetVideoAsync, GetImagesAsync, DownloadVideoAsync, DownloadImageAsync, DownloadImagesAsync
- GitHub Actions CI workflow (build, test, pack)
- GitHub Actions CD workflow (NuGet publish on Release)
- XML documentation on all public APIs
- 47 unit tests with xUnit, FluentAssertions, and WireMock
- `PublicationUrlValidator` for TikTok URL validation
- Immutable domain entities with init-only properties
- Value objects: PublicationId, AuthorId, SoundtrackId
- Custom exceptions: TikTokExplodeException, ApiException, ValidationException
- File download with progress reporting and cancellation support
- Configurable HTTP client with retry policy and User-Agent rotation

### Changed
- Project structure migrated from flat layout to layered architecture
- HttpClient management improved with IHttpClientFactory
- JSON extractors refactored to separate interfaces and implementations
- Repositories now properly separated from HTTP client code
- README updated with NuGet installation and DI examples

### Fixed
- Empty aweme_list handling in all extractors (throws ValidationException)
- IServiceProvider disposal removed from TikTokClient
- Retry loop in TikTokApiClient now properly catches exceptions
- FileDownloadService no longer uses non-seekable Stream.Length
- UrlHandler uses IHttpClientFactory (no more socket exhaustion)
- CD workflow uses --no-build --no-restore for pack step