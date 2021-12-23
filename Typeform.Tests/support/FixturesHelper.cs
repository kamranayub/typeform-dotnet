namespace Typeform.Tests;

public class FixturesHelper {
  public static string GetResponsesFixture()
  {
    return System.IO.File.ReadAllText("fixtures/responses.json");
  }
}