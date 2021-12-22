# typeform-dotnet

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

public class HomeController : Controller {
  private readonly ITypeformApi _typeformApi;

  public Controller(ITypeformApi typeformApi) {
    _typeformApi = typeformApi;
  }
}
```