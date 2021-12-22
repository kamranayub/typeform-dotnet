# typeform-dotnet

A .NET SDK wrapper built with [Refit](https://github.com/reactiveui/refit) around Typeform's API.

## Usage

### Create API client

To create an instance of the Refit `ITypeformApi` client:

```c#
using Typeform;

var client = TypeformClient.CreateApi(settings);
```

### Services / Dependency Injection

```c#
using Typeform;

// Nice-to-have
services.AddTypeformApi();

// You can always do this
services.AddRefitClient<ITypeformApi>(TypeformClient.DefaultSettings);
// TODO?
services.AddRefitClient<ITypeformApi>(TypeformClient.CreateSettings(settings));
```

### Consuming the API

```c#
public class HomeController : Controller {
  private readonly IConfiguration _configuration;
  private readonly ITypeformApi _typeformApi;

  public Controller(IConfiguration configuration, ITypeformApi typeformApi) {
    _configuration = configuration;
    _typeformApi = typeformApi;
  }

  public async Task<ActionResult> Index() {
    var accessToken = _configuration["TypeformAccessToken"]
    var formId = "abc123";

    var responses = await _typeformApi.GetFormResponsesAsync(accessToken, formId);
  }
}
```

## TODO

- [x] Create a basic API client
- [x] Support for passing in an access token
- [ ] Nuget package flow
- [ ] Github CI for tests / build / publish
- [ ] Target .NET Standard / maximize compatibility
- [ ] Support for [Responses API](https://developer.typeform.com/responses/)
  - [ ] [Retrieve responses](https://developer.typeform.com/responses/reference/retrieve-responses/)
  - [ ] [Delete responses](https://developer.typeform.com/responses/reference/delete-responses/)
  - [ ] [Retrieve response file](https://developer.typeform.com/responses/reference/retrieve-response-file/)
  - [ ] [Retrieve Form Insights](https://developer.typeform.com/responses/reference/retrieve-form-insights/)
- [ ] Support for [Webhooks API](https://developer.typeform.com/webhooks/)
- [ ] Support for [Create API](https://developer.typeform.com/create/)
- [ ] Support OAuth flow to obtain access token
