# typeform-dotnet

[![.NET](https://github.com/kamranayub/typeform-dotnet/actions/workflows/dotnet.yml/badge.svg?branch=main)](https://github.com/kamranayub/typeform-dotnet/actions/workflows/dotnet.yml) [![Nuget](https://img.shields.io/nuget/v/Typeform)](https://nuget.org/packages/Typeform) [![Nuget](https://img.shields.io/nuget/dt/Typeform?label=nuget%20downloads)](https://nuget.org/packages/Typeform)

A .NET Standard 2.0 SDK wrapper built with [Refit](https://github.com/reactiveui/refit) around Typeform's API.

## Twitch Streams

This SDK is (mostly) built in public on Twitch. Watch the collection here:

https://www.twitch.tv/collections/Lm3OF4Q0xBZaSA

## Supported Endpoints

- [Retrieve responses](https://developer.typeform.com/responses/reference/retrieve-responses/)
- [Retrieve response file](https://developer.typeform.com/responses/reference/retrieve-response-file/)
- [Delete responses](https://developer.typeform.com/responses/reference/delete-responses/)


## Install

via [Nuget](https://nuget.org/packages/Typeform)

```sh
# Package Manager
Install-Package Typeform

# dotnet
dotnet add package Typeform
```

## Usage

### Create API client

To create an instance of the Refit `ITypeformApi` client:

```c#
using Typeform;

var client = TypeformClient.CreateApi();
```

### Services / Dependency Injection

```c#
using Typeform;

// TODO: Not Implemented Yet
// services.AddTypeformApi();

// You can always do this
services.AddRefitClient<ITypeformApi>(TypeformClient.DefaultSettings);
```

### Customizing Refit Settings

`TypeformClient.DefaultSettings` contains the default Refit settings used for the API. You can create new derived settings to pass if you need to through the `CreateApi` static method.

`TypeformClient.DefaultSystemTextJsonSerializerOptions` contains the default `System.Text.Json` serializer options. This handles naming policy and JSON deserialization according to the requirements of the Typeform API.

### Consuming the API

You will need to pass your Typeform OAuth or Personal Access Token. Currently, this
is implemented as a string argument to the endpoint methods.

> TODO: Obtaining an OAuth token is not implemented yet. But for server-side flows,
> usually a Personal Access Token is "good enough."

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

# Responses API

## Retrieve Responses

The Typeform Responses API returns form responses that include answers. Each answer can be a different type and to accomodate this, the SDK deserializes into different class implementations based on the answer `type` discriminator.

### Retrieving an answer by type

If you know what type an answer is _supposed_ to be, you can use the `Answers.GetAnswer<T>(index)` method
to retrieve an answer at an index that is the expected type:

**Answer By Index**

```c#
var responses = await _typeformApi.GetFormResponsesAsync(accessToken, formId);

// Retrieve first response's answer as Text type (by index)
var answerText = responses.Items[0].Answers.GetAnswer<TypeformAnswerText>(0);
```

**Answer By Field ID**

```c#
var responses = await _typeformApi.GetFormResponsesAsync(accessToken, formId);

// Retrieve first response's answer as Text type (by field.id)
var answerText = responses.Items[0].Answers.GetAnswerById<TypeformAnswerText>("abc123");
```

**Answer By Field Ref**

```c#
var responses = await _typeformApi.GetFormResponsesAsync(accessToken, formId);

// Retrieve first response's answer as Text type (by field.ref)
var answerText = responses.Items[0].Answers.GetAnswerByRef<TypeformAnswerText>("my_custom_ref");
```

If you _do not_ know what type an answer is _supposed_ to be, you can inspect its type:

```c#
var responses = await _typeformApi.GetFormResponsesAsync(accessToken, formId);

// Retrieve first response's answer type
var firstAnswerType = responses.Items[0].Answers[0].Type;

if (firstAnswerType == TypeformAnswerType.Text) {
  var firstAnswer = responses.Items[0].Answers[0] as TypeformAnswerText;
}
```

Based on Typeform's response structure, it's not easily possible to get static typing of the answers
without knowing the type in advance.

### Answer type mapping

For reference, this is the mapping for each answer type used:

```c#
// Get the answer type enum value
var type = answer.Type;

// Determine the class type to map to
Type answerInstanceType = type switch
{
  TypeformAnswerType.Boolean => typeof(TypeformAnswerBoolean),
  TypeformAnswerType.Choice => typeof(TypeformAnswerChoice),
  TypeformAnswerType.Choices => typeof(TypeformAnswerChoices),
  TypeformAnswerType.Date => typeof(TypeformAnswerDate),
  TypeformAnswerType.Email => typeof(TypeformAnswerEmail),
  TypeformAnswerType.FileUrl => typeof(TypeformAnswerFileUrl),
  TypeformAnswerType.Number => typeof(TypeformAnswerNumber),
  TypeformAnswerType.Payment => typeof(TypeformAnswerPayment),
  TypeformAnswerType.Text => typeof(TypeformAnswerText),
  TypeformAnswerType.Url => typeof(TypeformAnswerUrl),
  _ => typeof(TypeformAnswer)
};
```

The SDK deserialization takes care of deserializing to the correct type so you can safely cast it.

### Retrieving a variable's type

A form variables collection is similar to answers. You can use the same pattern to get variables by type.

**Variables By Index**

```c#
var responses = await _typeformApi.GetFormResponsesAsync(accessToken, formId);

// Retrieve first response's variable (by index)
var answerText = responses.Items[0].Variables.GetVariable<TypeformVariableText>(0);
```

**Variables By Key**

```c#
var responses = await _typeformApi.GetFormResponsesAsync(accessToken, formId);

// Retrieve first response's variable (by key)
var answerText = responses.Items[0].Variables.GetVariable<TypeformVariableText>("name");
```

## Delete Responses

Use `ITypeformApi.DeleteResponsesAsync()` and pass a list of response IDs to delete.

## Retrieve Response File

Uploaded files to a form can be downloaded via the REST API. 

### Retrieve file by `file_url`

The [Typeform docs](https://developer.typeform.com/responses/JSON-response-explanation/) specify that you **cannot** rely on the values of `file_url` in Form Responses to have a consistent structure.

However, I have found that many `file_url` values _do match_ the REST endpoint path value. To accommodate this, I've added an extension method `GetFormResponseFileStreamFromUrlAsync` which you can use to pass a `FileUrl` value directly and attempt to download a file.

```c#
ITypeformApi typeformApi = TypeformClient.CreateApi();

var responses = await typeformApi.GetFormResponsesAsync(accessToken, formId);
var uploadFileAnswer = responses.Items[0].Answers.GetAnswerByRef<TypeformAnswerFileUrl>("my_custom_upload_ref");

ApiResponse<Stream> fileResponse = await typeformApi.GetFormResponseFileStreamFromUrlAsync(
  accessToken,
  uploadFileAnswer.FileUrl
);

var contents = await fileResponse.ReadAllBytesAsync(/* chunkSize: <optional value in bytes> */);

await System.IO.File.WriteAllBytesAsync(filename, contents);
```

### Retrieve file by parameters

You can also manually specify the `form_id`, `response_id`, `field_id` and `filename` to download
using `ITypeformApi.GetFormResponseFileStreamAsync()`.

The return value of this method is a Refit `ApiResponse<Stream>` and you can manipulate the `Stream` response any way
you see fit. There is a `ReadAllBytesAsync()` extension method that will read the full bytes using a chunked buffer:

```c#
ITypeformApi typeformApi = TypeformClient.CreateApi();

ApiResponse<Stream> fileResponse = await typeformApi.GetFormResponseFileStreamAsync(
  accessToken,
  formId,
  responseId,
  fieldId,
  filename
);

var contents = await fileResponse.ReadAllBytesAsync(/* chunkSize: <optional value in bytes> */);

await System.IO.File.WriteAllBytesAsync(filename, contents);
```

If you need to download a file using a `FileUrl` value from the Form Responses API, you will need to construct your own `HttpClient` to download it [like this example](https://dev.to/1001binary/download-file-using-httpclient-wrapper-asynchronously-1p6).

## TODO

- [x] Create a basic API client
- [x] Support for passing in an access token
- [x] Nuget package flow
- [x] Github CI for tests / build / publish
- [x] Target .NET Standard / maximize compatibility
- [ ] Support for [Responses API](https://developer.typeform.com/responses/)
  - [x] [Retrieve responses](https://developer.typeform.com/responses/reference/retrieve-responses/)
  - [x] [Delete responses](https://developer.typeform.com/responses/reference/delete-responses/)
  - [x] [Retrieve response file](https://developer.typeform.com/responses/reference/retrieve-response-file/)
  - [ ] [Retrieve Form Insights](https://developer.typeform.com/responses/reference/retrieve-form-insights/)
- [ ] Support for [Webhooks API](https://developer.typeform.com/webhooks/)
- [ ] Support for [Create API](https://developer.typeform.com/create/)
- [ ] Support OAuth flow to obtain access token
