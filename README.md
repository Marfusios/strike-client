![Logo](https://raw.githubusercontent.com/Marfusios/strike-client/master/strike_wide.png)
# Strike .NET client 
[![NuGet version](https://img.shields.io/nuget/v/StrikeWallet.Client?style=flat-square)](https://www.nuget.org/packages/StrikeWallet.Client)
[![Nuget downloads](https://img.shields.io/nuget/dt/StrikeWallet.Client?style=flat-square)](https://www.nuget.org/packages/StrikeWallet.Client)
[![CI build](https://img.shields.io/github/check-runs/marfusios/strike-client/master?style=flat-square&label=build)](https://github.com/Marfusios/strike-client/actions/workflows/dotnet-core.yml)

This is a C# implementation of the Strike API found here:

https://docs.strike.me/api

[Releases and breaking changes](https://github.com/Marfusios/strike-client/releases)

### License: 
    MIT

### Features

* installation via NuGet ([StrikeWallet.Client](https://www.nuget.org/packages/StrikeWallet.Client))
* targeting .NET Core 3.1, .NET 6, .NET 8, and .NET 10 (Windows/Linux/macOS compatible)
* framework-specific dependencies retain compatibility for existing consumers; legacy targets remain available even though their runtimes are out of support
* coverage of all 49 operations in the published Strike API reference, including events, webhook subscriptions, payouts, and account limits; see the [API coverage audit](docs/api-coverage.md)

### Usage

```csharp
var apiKey = "YOUR_API_KEY";
var environment = StrikeEnvironment.Live;

var client = new StrikeClient(environment, apiKey);
var profile = await client.Accounts.GetProfile("marfusios");
```

Read recent webhook events and account limits:

```csharp
var events = await client.Events.GetEvents(top: 20, orderBy: "created desc");
var limits = await client.Accounts.GetLimits();
```

For paginated collections, check `IsCountUnknown` before using the total. Existing collection types expose the full 64-bit value through `TotalCount`; the new events, payouts, and payout-originators collections use `long Count`.

#### With dependency injection support:

appsettings.json

```json
{
  "Strike": {
    "Environment": "Live",
    "ApiKey": "YOUR_API_KEY"
  }
}
```

Startup.cs

```csharp
services.AddStrike(configuration);
```

More usage examples:
* console sample ([link](test_integration/Strike.Client.Sample/Program.cs))
* integration tests ([link](test_integration/Strike.Client.IntegrationTests))

### Building and testing

Install the .NET 10 SDK and the .NET 8 runtime. `global.json` selects a stable .NET 10 SDK. The library builds for all four targets in both Debug and Release; offline tests run on .NET 8 and .NET 10.

```sh
dotnet restore
dotnet build Strike.Client.sln --configuration Release --no-restore
dotnet test test/Strike.Client.Tests/Strike.Client.Tests.csproj --configuration Release --no-build
dotnet pack src/Strike.Client/Strike.Client.csproj --configuration Release --no-build -o artifacts
dotnet list Strike.Client.sln package --vulnerable --include-transitive
```

The offline tests use an in-memory HTTP handler to check requests, authentication and idempotency headers, JSON converters, dependency injection, and error responses. They require no API credentials.

Integration tests load `appsettings.Tests.json`, environment variables, and user secrets. Set `Strike__ApiKey` and `Strike__Environment` for the account being tested; user secrets take precedence. Without an API key, integration tests are skipped. CI runs only the read-only checks:

```sh
dotnet test test_integration/Strike.Client.IntegrationTests/Strike.Client.IntegrationTests.csproj --configuration Release --no-build --filter "FullyQualifiedName~GetBalances_ShouldWork|FullyQualifiedName~GetRates_ShouldWork|FullyQualifiedName~ApiReadOnlyTests"
```

Reviewed live tests read balances, rates, limits, and lists of invoices, deposits, payment methods, payouts, payout originators, events, subscriptions, receive requests, and receives. The limits check skips with an explicit reason if the key lacks `partner.account.profile.read-own` and Strike returns 403. All integration flows that create, execute, initiate, update, or delete resources skip before making requests. A test-only HTTP handler also rejects every method except GET and HEAD before transmission, even if a test is accidentally marked read-only. This restriction applies to every environment and has no configuration override. The sample application uses its own user-secrets ID and is separate from this test setup.

### Publishing

Update `Version` in `Directory.Build.props` and add `docs/releases/<version>.md` for every NuGet release. CI validates that the notes exist, builds and tests the library, publishes through NuGet trusted publishing, and then creates the matching `v<version>` GitHub release from those notes. Before tagging, it waits for the public package and verifies its repository commit. Reruns preserve existing releases.


**Pull Requests are welcome!**
