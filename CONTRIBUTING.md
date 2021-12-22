# Contributing

## Setting Your Access Token

The `Typeform.Tests` project uses [.NET User Secrets](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets) to set the Typeform access token:

```sh
cd Typeform.Tests
dotnet user-secrets set "TypeformAccessToken" "<value>"
```

To get your Personal Access Token, [follow the Typeform documentation](https://developer.typeform.com/get-started/personal-access-token/).