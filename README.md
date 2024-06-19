![Logo](https://raw.githubusercontent.com/Marfusios/strike-client/master/strike_wide.png)
# Strike .NET client [![NuGet version](https://badge.fury.io/nu/StrikeWallet.Client.svg)](https://www.nuget.org/packages/StrikeWallet.Client) [![Nuget downloads](https://img.shields.io/nuget/dt/StrikeWallet.Client)](https://www.nuget.org/packages/StrikeWallet.Client)

This is a C# implementation of the Strike API found here:

https://docs.strike.me/api

[Releases and breaking changes](https://github.com/Marfusios/strike-client/releases)

### License: 
    MIT

### Features

* installation via NuGet ([StrikeWallet.Client](https://www.nuget.org/packages/StrikeWallet.Client))
* targeting .NET Standard 2.0 (.NET Core, Linux/MacOS compatible) + Standard 2.1, .NET 5-8

### Usage

```csharp
var apiKey = "YOUR_API_KEY";
var environment = Environment.Live;

var client = new StrikeClient(environment, apiKey);
var profile = await client.Accounts.GetProfile("marfusios");
```

More usage examples:
* console sample ([link](test_integration/Strike.Client.Sample/Program.cs))


**Pull Requests are welcome!**
