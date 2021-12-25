// See https://aka.ms/new-console-template for more information
using Typeform;

var accessToken = System.Environment.GetEnvironmentVariable("TYPEFORM_ACCESS_TOKEN");

var typeformApi = TypeformClient.CreateApi();
var responses = await typeformApi.GetFormResponsesAsync(accessToken, "xVMHX23n");

Console.WriteLine($"Found {responses.TotalItems}");